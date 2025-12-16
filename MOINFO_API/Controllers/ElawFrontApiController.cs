using AutoMapper;
using Azure.Core;
using Business.Enums;
using Business.Helpers;
using Business.Interfaces;
using Business.ModelWithSpecification;
using Business.Repository;
using Business.ViewModel;
using Business.ViewModel.ClassificationVM;
using Business.ViewModel.Dynamic;
using Business.ViewModel.Elaw;
using Business.ViewModel.HomePage;
using Business.ViewModel.Tourism;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Transactions;
using static Azure.Core.HttpHeader;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MOINFO_API.Controllers
{
    [Route("api/ElawFront")]
    public class ElawFrontApiController : BaseController
    {
        private readonly IUnitOfwork _unitOfwork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly GenerateLicNo _generateLicNo;
        private readonly EmailService _emailService;
        private readonly IDataFetchService _dataFetchService;
        private readonly IUpdateDataService _updateDataService;
        private readonly ILogger<ElawFrontApiController> _logger;


        public ElawFrontApiController(IUnitOfwork unitOfwork, IConfiguration configuration
            , IMapper mapper, GenerateLicNo generateLicNo, ILogger<ElawFrontApiController> logger, EmailService emailService, IDataFetchService dataFetchService, IUpdateDataService updateDataService)
        {
            _unitOfwork = unitOfwork;
            _configuration = configuration;
            _mapper = mapper;
            _generateLicNo = generateLicNo;
            _emailService = emailService;
            _dataFetchService = dataFetchService;
            _updateDataService = updateDataService;
            _logger = logger;

        }
        //Check MediaName or LicName
        public static string NormalizeArabic(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            input = input.Trim();

            // Normalize Arabic characters (e.g., Persian variants)
            input = input.Replace("أ", "ا")
                         .Replace("إ", "ا")
                         .Replace("آ", "ا")
                         .Replace("ى", "ي")
                         .Replace("ة", "ه")
                         .Replace("ئ", "ي")
                         .Replace("ؤ", "و");

            // Remove Tashkeel (diacritics)
            string diacritics = @"[\u064B-\u065F\u0610-\u061A\u06D6-\u06ED]";
            input = Regex.Replace(input, diacritics, "");

            return input;
        }
        [HttpGet]
        [Route("CheckMediaName")]
        public async Task<IActionResult> CheckMediaName(string MediaName)
        {
            try
            {
                //var ExistMadia = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                //              .GetByCondition(r => r.Licname == MediaName
                //                            &&r.ServiceId==(int)ServiceEnum.Elaw
                //                            &&r.RequestStatusId==(int)RequestStatusEnum.FinalLicenseIssued).CountAsync();
                string normalizedMediaName = NormalizeArabic(MediaName);

                var existMedia = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                    .GetByCondition(r =>
                        r.ServiceId == (int)ServiceEnum.Elaw &&
                        r.RequestStatusId == (int)RequestStatusEnum.FinalLicenseIssued)
                    .ToListAsync();

                bool isExist = existMedia
                    .Any(r => NormalizeArabic(r.Licname) == normalizedMediaName);
                if (isExist)
                {
                    return Ok(new MediaCheckResult
                    {
                        exists = true,
                        message = "يوجد وسيلة اعلامية بهذا الاسم"
                    });
                }

                return Ok(new MediaCheckResult
                {
                    exists = false,
                    message = ""
                });
            }
            catch (Exception ex)
            {
                LogManager.Instance.AddErrorLog(ex);
                return BadRequest(new
                {
                    exists = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("CheckManager")]
        public async Task<IActionResult> CheckManager(string ManagerCivilId)
        {
            try
            {
                var ExistMananger = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                              .GetByCondition(r => r.ManCivilId == ManagerCivilId
                                            && r.ServiceId == (int)ServiceEnum.Elaw
                                            && r.RequestStatusId == (int)RequestStatusEnum.FinalLicenseIssued).CountAsync();
                if (ExistMananger > 0)
                {
                    return Ok(new
                    {
                        exists = true,
                        message = "المدير المسئول مسجل لدى وسيلة إعلامية أخرى"
                    });
                }

                return Ok(new
                {
                    exists = false
                });
            }
            catch (Exception ex)
            {
                LogManager.Instance.AddErrorLog(ex);
                return BadRequest(new
                {
                    exists = false,
                    message = ex.Message
                });
            }
        }
        #region Lic For Person
        [HttpGet]
        [Route("GetLicRequestForPerson")]
        public async Task<IActionResult> GetLicRequestForPerson(string CivilId, int id)
        {
            var licencesinfoWithSpec = new LicencesInfoWithSpec(id);
            var licencesInfo = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                .GetByIdWithSpec(licencesinfoWithSpec);

            var AttachValid = await _unitOfwork.genericRepository<AttachRule>()
                         .GetByCondition(f => f.ViewType == "LicRequestPerPerson").ToListAsync();
            var ActivityPerElaw = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                                    .GetByCondition(a => a.ServiceId == (int)ServiceEnum.Elaw)
                                    .Select(a => new SelectListItem
                                    {
                                        Value = a.Id.ToString(),
                                        Text = a.NameAr
                                    }).ToListAsync();
            var aspnetUserInformation = await _unitOfwork.genericRepository<AspNetUser>()
                                    .GetByCondition(a => a.CivilId == CivilId).FirstOrDefaultAsync();
            var aspMapped = _mapper.Map<AspNetUser, AspnetUserVM>(aspnetUserInformation);
            var qualification = await _unitOfwork.genericRepository<QualificationsLookup>()
                .GetByCondition(q => !string.IsNullOrEmpty(q.Name))
                            .Select(q => new SelectListItem
                            {
                                Value = q.Id.ToString(),
                                Text = q.Name
                            }).ToListAsync();

            return Ok(new RequestLicPerPerson
            {
                RequestVM = new RequestVM(),
                AspnetUserVM = aspMapped,
                ActivitySelectedList = ActivityPerElaw,
                QualificationSelectedList = qualification,
                LicencesInfoVM = _mapper.Map<MoiEserviceLicenseInfo, LicencesInfoVM>(licencesInfo),
                FileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(AttachValid)
            });
        }

        [HttpPost]
        [Route("PostLicRequestForPerson")]
        public async Task<dynamic> PostLicRequestForPerson(RequestLicPerPersonApi model)
        {
            string error = string.Empty;
            try
            {

                //--------------- Start trunsaction -----------------------------------
                using (IDbContextTransaction dbTransaction = _unitOfwork.BeginTransaction())
                {
                    try
                    {
                        var activity = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                           .GetByCondition(a => a.Id == model.ActivityTypeId).Select(a => a.NameAr).FirstOrDefaultAsync();
                        // 1. Get or Create Applicant Address and Person
                        var PersonApplicant = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == model.CivilId)
                            .FirstOrDefaultAsync();

                        if (PersonApplicant == null)
                        {
                            var AddressApplicant = new Address
                            {
                                GovernorateArabic = model.GovernateApplicant,
                                FloorNo = model.FloorNOApplicant,
                                ServiceId = (int)ServiceEnum.Elaw,
                                StreetArabic = model.StreetApplicant,
                                AalliNo = model.AaliNOApplicant,
                                BuildingName = model.BuildingNameApplicant,
                                BuildingNo = model.BuildingNOApplicant,
                                BlockArabic = model.BlockApplicant,
                                UnitNo = model.UnitNOApplicant,
                                Area = model.AreaApplicant,
                            };

                            await _unitOfwork.genericRepository<Address>().Create(AddressApplicant);
                            await _unitOfwork.Complete();

                            PersonApplicant = new Person
                            {
                                AddressId = AddressApplicant.Id,
                                CivilId = model.CivilId,
                                Name1 = model.Name1Applicant,
                                Name2 = model.Name2Applicant,
                                Name3 = model.Name3Applicant,
                                Name4 = model.Name4Applicant,
                                PersonName = $"{model.Name1Applicant} {model.Name2Applicant} {model.Name3Applicant} {model.Name4Applicant}",
                                Email = model.Email,
                                IsApplicant = true,
                                NationaliyName = model.NationalitynameApplicant,
                                ServiceId = (int)ServiceEnum.Elaw,
                                Phone = model.Mobile,
                                QualificationId = model.QualificationApplicantId
                            };

                            await _unitOfwork.genericRepository<Person>().Create(PersonApplicant);
                            await _unitOfwork.Complete();
                        }

                        // 2. Get or Create Manager Address and Person
                        var PersonManager = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == model.ManCivilId)
                            .FirstOrDefaultAsync();

                        if (PersonManager == null)
                        {
                            var AddressManager = new Address
                            {
                                GovernorateArabic = model.GovernateManager,
                                FloorNo = model.FloorNOManager,
                                ServiceId = (int)ServiceEnum.Elaw,
                                StreetArabic = model.StreetManager,
                                AalliNo = model.AaliNOManager,
                                BuildingName = model.BuildingNameManager,
                                BuildingNo = model.BuildingNoManager,
                                BlockArabic = model.BlockManager,
                                UnitNo = model.UnitNOManager,
                                Area = model.AreaManager,
                            };

                            await _unitOfwork.genericRepository<Address>().Create(AddressManager);
                            await _unitOfwork.Complete();

                            PersonManager = new Person
                            {
                                AddressId = AddressManager.Id,
                                CivilId = model.ManCivilId,
                                Name1 = model.Name1Manager,
                                Name2 = model.Name2Manager,
                                Name3 = model.Name3Manager,
                                Name4 = model.Name4Manager,
                                PersonName = $"{model.Name1Manager} {model.Name2Manager} {model.Name3Manager} {model.Name4Manager}",
                                Email = model.EmailManager,
                                IsApplicant = false,
                                NationaliyName = model.NationalitynameManager,
                                ServiceId = (int)ServiceEnum.Elaw,
                                Phone = model.PhoneManager,
                                QualificationId = model.QualificationManagerId
                            };

                            await _unitOfwork.genericRepository<Person>().Create(PersonManager);
                            await _unitOfwork.Complete();
                        }

                        var Licences = new Licence()
                        {
                            ActiivityTypeId = model.ActivityTypeId,
                            ApplicantCivilId = model.CivilId,
                            LicName = model.Licname,
                            LicStatusId = (int)licencesStatusEnum.Pending,
                            LicTypeId = (int)LicTypeEnum.Media_Organization_Individuals,
                            ManagerId = PersonManager.Id,
                            ManagerCivilId = model.ManCivilId,
                            ServiceId = (int)ServiceEnum.Elaw,
                            Licowner = PersonApplicant.PersonName,
                            ApplicantId = PersonApplicant.Id,


                        };
                        await _unitOfwork.genericRepository<Licence>().Create(Licences);
                        await _unitOfwork.Complete();
                        var Request = new MoiEserviceLicensesRequest()
                        {
                            AppId = PersonApplicant.Id,
                            LicenseId = Licences.LicId,
                            ActivityTypeId = model.ActivityTypeId,
                            RequestStatusId = (int)RequestStatusEnum.WaitingForReview,
                            ServiceId = (int)ServiceEnum.Elaw,
                            LicTypeId = (int)LicTypeEnum.Media_Organization_Individuals,
                            ManagerId = PersonManager.Id,

                            ManCivilId = model.ManCivilId,
                            AppCivilId = model.AppCivilId,
                            Licname = model.Licname,
                            Licowner = PersonApplicant.PersonName,
                            IsArchived = false,
                            Licreqtime = model.Licreqtime,
                            LicStatusId = (int)licencesStatusEnum.Pending,
                            OwnerSameManager = model.OwnerSameManager,
                            LicrequestIsDeleted = false,
                            Reqno = model.Reqno,
                            SequenceNo = model.SequenceNo,
                            ReqtypeId = (int)RequestTypeEnum.Request,
                            CategoryId = 1,
                            SectorId = 3,
                            ActivityType = activity,
                            Licpaystatus = "0",


                        };
                        await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(Request);
                        await _unitOfwork.Complete();

                        var socialMediaList = new List<MoiSocialMedia>();

                        if (!string.IsNullOrEmpty(model.FacebookUrl))
                        {
                            socialMediaList.Add(new MoiSocialMedia
                            {
                                Requestid = Request.RequestId,
                                LicenceId = Licences.LicId,
                                SocialType = (int)SocialMediaEnum.Facebook, // Facebook
                                AccountSocial = model.FacebookUrl
                            });
                        }

                        if (!string.IsNullOrEmpty(model.Twitter))
                        {
                            socialMediaList.Add(new MoiSocialMedia
                            {
                                Requestid = Request.RequestId,
                                LicenceId = Licences.LicId,
                                SocialType = (int)SocialMediaEnum.Twitter, // Twitter
                                AccountSocial = model.Twitter
                            });
                        }

                        if (!string.IsNullOrEmpty(model.Instagram))
                        {
                            socialMediaList.Add(new MoiSocialMedia
                            {
                                Requestid = Request.RequestId,
                                LicenceId = Licences.LicId,
                                SocialType = (int)SocialMediaEnum.instegram, // Instagram
                                AccountSocial = model.Instagram
                            });
                        }

                        if (!string.IsNullOrEmpty(model.website))
                        {
                            socialMediaList.Add(new MoiSocialMedia
                            {
                                Requestid = Request.RequestId,
                                LicenceId = Licences.LicId,
                                SocialType = (int)SocialMediaEnum.Website, // Website
                                AccountSocial = model.website
                            });
                        }

                        // Save all
                        foreach (var sm in socialMediaList)
                        {
                            await _unitOfwork.genericRepository<MoiSocialMedia>().Create(sm);
                        }
                        await _unitOfwork.Complete();

                        await InsertAttachements(model.saveResponseVMs, Request.RequestId, model.SessionCivilId);

                        dbTransaction.Commit();

                    }

                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        return new ErrorMessage()
                        {
                            Error = true,
                            Status = "Failure",
                            Message = ex.Message + "" + ex.InnerException + "" + error,
                        };
                    }

                }
                //--------------- End trunsaction -----------------------------------
                return new ErrorMessage()
                {
                    Error = false,
                    Status = "Success",
                    Message = "inserted suceesfully",
                };

            }
            catch (Exception ex)
            {

                return new ErrorMessage()
                {
                    Error = true,
                    Status = "Failure",
                    Message = ex.Message + "" + ex.InnerException + "" + error,
                };

            }
        }
        #endregion
        #region Lic For Company
        [HttpGet]
        [Route("GetLicRequestForCompany")]
        public async Task<IActionResult> GetLicRequestForCompany(string CivilId, int id)
        {
            var licencesinfoWithSpec = new LicencesInfoWithSpec(id);
            var licencesInfo = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                .GetByIdWithSpec(licencesinfoWithSpec);

            string viewType = licencesInfo.LicTypeId switch
            {
                (int)LicTypeEnum.Media_Organization_Company => "LicRequestPerCompany",
                (int)LicTypeEnum.Government_Entity => "LicRequestForGov",
                (int)LicTypeEnum.Public_Benefit_Association => "LicRequestForBenefit",

                (int)LicTypeEnum.PrintedNewspapersAndLicensedAVChannels => "LicRequestForNewspaperAndChannel",
                _ => "LicRequestDefault"
            };

            var AttachValid = await _unitOfwork.genericRepository<AttachRule>()
                         .GetByCondition(f => f.ViewType == viewType)
                         .ToListAsync();
            var ActivityPerElaw = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                                    .GetByCondition(a => a.ServiceId == (int)ServiceEnum.Elaw)
                                    .Select(a => new SelectListItem
                                    {
                                        Value = a.Id.ToString(),
                                        Text = a.NameAr
                                    }).ToListAsync();
            var aspnetUserInformation = await _unitOfwork.genericRepository<AspNetUser>()
                                    .GetByCondition(a => a.CivilId == CivilId).FirstOrDefaultAsync();
            var aspMapped = _mapper.Map<AspNetUser, AspnetUserVM>(aspnetUserInformation);
            var qualification = await _unitOfwork.genericRepository<QualificationsLookup>()
                .GetByCondition(q => !string.IsNullOrEmpty(q.Name))
                            .Select(q => new SelectListItem
                            {
                                Value = q.Id.ToString(),
                                Text = q.Name
                            }).ToListAsync();

            return Ok(new RequestLicPerPerson
            {
                RequestVM = new RequestVM(),
                AspnetUserVM = aspMapped,
                ActivitySelectedList = ActivityPerElaw,
                QualificationSelectedList = qualification,
                LicencesInfoVM = _mapper.Map<MoiEserviceLicenseInfo, LicencesInfoVM>(licencesInfo),
                FileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(AttachValid)
            });
        }

        [HttpPost]
        [Route("PostLicRequestForCompany")]
        public async Task<dynamic> PostLicRequestForCompany(RequestLicPerCompanyApi model)
        {
            string error = string.Empty;
            try
            {

                //--------------- Start trunsaction -----------------------------------
                using (IDbContextTransaction dbTransaction = _unitOfwork.BeginTransaction())
                {
                    try
                    {
                        var activity = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                            .GetByCondition(a => a.Id == model.ActivityTypeId).Select(a => a.NameAr).FirstOrDefaultAsync();
                        var AddressCompany = new Address()
                        {
                            GovernorateArabic = model.GovernateCompany,
                            FloorNo = model.FloorNOCompany,
                            ServiceId = (int)ServiceEnum.Elaw,
                            StreetArabic = model.StreetCompany,
                            AalliNo = model.AaliNOCompany,
                            BuildingName = model.BuildingNameCompany,
                            BuildingNo = model.BuildingNOCompany,
                            BlockArabic = model.BlockCompany,
                            UnitNo = model.UnitNOCompany,
                            Area = model.AreaCompany,


                        };
                        await _unitOfwork.genericRepository<Address>().Create(AddressCompany);
                        await _unitOfwork.Complete();
                        var CompanyApplicant = new Company()
                        {
                            Name = model.CompanyName,
                            ActivityTypeId = model.ActivityTypeId,
                            AddressId = AddressCompany.Id,
                            Email = model.CompanyEmail,
                            CompanyCivilId = model.CompanyCivilId,
                            CompanyNo = model.CompanyFax,
                            PhoneNo = model.CompanyPhone,
                            ServiceId = (int)ServiceEnum.Elaw,


                        };
                        await _unitOfwork.genericRepository<Company>().Create(CompanyApplicant);
                        await _unitOfwork.Complete();
                        var PersonManager = await _unitOfwork.genericRepository<Person>()
        .GetByCondition(a => a.CivilId == model.ManCivilId)
        .FirstOrDefaultAsync();

                        if (PersonManager == null)
                        {
                            // 🟢 Create address only if person doesn't exist
                            var AddressManager = new Address()
                            {
                                GovernorateArabic = model.GovernateManager,
                                FloorNo = model.FloorNOManager,
                                ServiceId = (int)ServiceEnum.Elaw,
                                StreetArabic = model.StreetManager,
                                AalliNo = model.AaliNOManager,
                                BuildingName = model.BuildingNameManager,
                                BuildingNo = model.BuildingNoManager,
                                BlockArabic = model.BlockManager,
                                UnitNo = model.UnitNOManager,
                                Area = model.AreaManager,
                            };

                            await _unitOfwork.genericRepository<Address>().Create(AddressManager);
                            await _unitOfwork.Complete();

                            PersonManager = new Person()
                            {
                                AddressId = AddressManager.Id,
                                CivilId = model.ManCivilId,
                                Email = model.EmailManager,
                                Name1 = model.Name1Manager,
                                Name2 = model.Name2Manager,
                                Name3 = model.Name3Manager,
                                Name4 = model.Name4Manager,
                                PersonName = $"{model.Name1Manager} {model.Name2Manager} {model.Name3Manager} {model.Name4Manager}",
                                IsApplicant = false,
                                NationaliyName = model.NationalitynameManager,
                                ServiceId = (int)ServiceEnum.Elaw,
                                Phone = model.PhoneManager,
                            };

                            await _unitOfwork.genericRepository<Person>().Create(PersonManager);
                            await _unitOfwork.Complete();

                            // 🟡 Optional: reload person
                            PersonManager = await _unitOfwork.genericRepository<Person>()
                                .GetByCondition(a => a.CivilId == model.ManCivilId)
                                .FirstOrDefaultAsync();
                        }
                        var managerExistinAsp = await _unitOfwork.genericRepository<AspNetUser>()
                            .GetByCondition(a => a.CivilId == model.ManCivilId).FirstOrDefaultAsync();

                        if (managerExistinAsp == null)
                        {
                            managerExistinAsp = new AspNetUser
                            {
                                CivilId = model.ManCivilId,
                                FullNameAr = model.Name1Manager + " " + model.Name2Manager + " " + model.Name3Manager + " " + model.Name4Manager,
                                Email = model.EmailManager,
                                Mobile = model.PhoneManager,

                            };
                            await _unitOfwork.genericRepository<AspNetUser>().Create(managerExistinAsp);
                            await _unitOfwork.Complete();
                        }
                        var Licences = new Licence()
                        {
                            ActiivityTypeId = model.ActivityTypeId,
                            ApplicantCivilId = model.ManCivilId,
                            ApplicantId = PersonManager.Id,
                            LicName = model.Licname,
                            LicStatusId = (int)licencesStatusEnum.Pending,
                            LicTypeId = model.LicTypeId,
                            ManagerId = PersonManager.Id,
                            ManagerCivilId = model.ManCivilId,
                            ServiceId = (int)ServiceEnum.Elaw,
                            Licowner = model.CompanyName,
                            CompanyId = CompanyApplicant.Id,

                        };
                        await _unitOfwork.genericRepository<Licence>().Create(Licences);
                        await _unitOfwork.Complete();
                        var Request = new MoiEserviceLicensesRequest()
                        {
                            LicenseId = Licences.LicId,
                            ActivityTypeId = model.ActivityTypeId,
                            RequestStatusId = (int)RequestStatusEnum.WaitingForReview,
                            ServiceId = (int)ServiceEnum.Elaw,
                            LicTypeId = model.LicTypeId,
                            ManagerId = PersonManager.Id,
                            ManCivilId = model.ManCivilId,
                            AppCivilId = model.ManCivilId,
                            AppId = PersonManager.Id,
                            Licname = model.Licname,
                            Licowner = model.CompanyName,
                            IsArchived = false,
                            Licreqtime = model.Licreqtime,
                            LicStatusId = (int)licencesStatusEnum.Pending,
                            OwnerSameManager = model.OwnerSameManager,
                            LicrequestIsDeleted = false,
                            Reqno = model.Reqno,
                            SequenceNo = model.SequenceNo,
                            CompanyId = CompanyApplicant.Id,
                            ReqtypeId = (int)RequestTypeEnum.Request,
                            CategoryId = 1,
                            SectorId = 3,
                            ActivityType = activity,
                            Licpaystatus = "0",

                        };
                        await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(Request);
                        await _unitOfwork.Complete();

                        var socialMediaList = new List<MoiSocialMedia>();

                        if (!string.IsNullOrEmpty(model.FacebookUrl))
                        {
                            socialMediaList.Add(new MoiSocialMedia
                            {
                                Requestid = Request.RequestId,
                                LicenceId = Licences.LicId,
                                SocialType = (int)SocialMediaEnum.Facebook, // Facebook
                                AccountSocial = model.FacebookUrl
                            });
                        }

                        if (!string.IsNullOrEmpty(model.Twitter))
                        {
                            socialMediaList.Add(new MoiSocialMedia
                            {
                                Requestid = Request.RequestId,
                                LicenceId = Licences.LicId,
                                SocialType = (int)SocialMediaEnum.Twitter, // Twitter
                                AccountSocial = model.Twitter
                            });
                        }

                        if (!string.IsNullOrEmpty(model.Instagram))
                        {
                            socialMediaList.Add(new MoiSocialMedia
                            {
                                Requestid = Request.RequestId,
                                LicenceId = Licences.LicId,
                                SocialType = (int)SocialMediaEnum.instegram, // Instagram
                                AccountSocial = model.Instagram
                            });
                        }

                        if (!string.IsNullOrEmpty(model.website))
                        {
                            socialMediaList.Add(new MoiSocialMedia
                            {
                                Requestid = Request.RequestId,
                                LicenceId = Licences.LicId,
                                SocialType = (int)SocialMediaEnum.Website, // Website
                                AccountSocial = model.website
                            });
                        }

                        // Save all
                        foreach (var sm in socialMediaList)
                        {
                            await _unitOfwork.genericRepository<MoiSocialMedia>().Create(sm);
                        }
                        await _unitOfwork.Complete();
                        if (model.LicTypeId == (int)LicTypeEnum.Media_Organization_Company)
                        {
                            var PartnerList = new List<Partner>();
                            if (!string.IsNullOrEmpty(model.PartnerName1))
                            {
                                PartnerList.Add(new Partner
                                {

                                    RequestId = Request.RequestId,
                                    LicenseId = Licences.LicId,
                                    Name = model.PartnerName1,
                                    ServiceId = (int)ServiceEnum.Elaw,
                                    LastUpdateDate = DateTime.Now
                                });
                            }
                            if (!string.IsNullOrEmpty(model.PartnerName2))
                            {
                                PartnerList.Add(new Partner
                                {

                                    RequestId = Request.RequestId,
                                    LicenseId = Licences.LicId,
                                    Name = model.PartnerName2,
                                    ServiceId = (int)ServiceEnum.Elaw,
                                    LastUpdateDate = DateTime.Now
                                });
                            }
                            if (!string.IsNullOrEmpty(model.PartnerName3))
                            {
                                PartnerList.Add(new Partner
                                {

                                    RequestId = Request.RequestId,
                                    LicenseId = Licences.LicId,
                                    Name = model.PartnerName3,
                                    ServiceId = (int)ServiceEnum.Elaw,
                                    LastUpdateDate = DateTime.Now
                                });
                            }
                            if (!string.IsNullOrEmpty(model.PartnerName4))
                            {
                                PartnerList.Add(new Partner
                                {

                                    RequestId = Request.RequestId,
                                    LicenseId = Licences.LicId,
                                    Name = model.PartnerName4,
                                    ServiceId = (int)ServiceEnum.Elaw,
                                    LastUpdateDate = DateTime.Now
                                });
                            }
                            if (!string.IsNullOrEmpty(model.PartnerName5))
                            {
                                PartnerList.Add(new Partner
                                {

                                    RequestId = Request.RequestId,
                                    LicenseId = Licences.LicId,
                                    Name = model.PartnerName5,
                                    ServiceId = (int)ServiceEnum.Elaw,
                                    LastUpdateDate = DateTime.Now
                                });
                            }
                            foreach (var item in PartnerList)
                            {
                                await _unitOfwork.genericRepository<Partner>().Create(item);
                            }
                            await _unitOfwork.Complete();
                        }

                        await InsertAttachements(model.saveResponseVMs, Request.RequestId, model.SessionCivilId);

                        dbTransaction.Commit();

                    }

                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        return new ErrorMessage()
                        {
                            Error = true,
                            Status = "Failure",
                            Message = ex.Message + "" + ex.InnerException + "" + error,
                        };
                    }

                }
                //--------------- End trunsaction -----------------------------------
                return new ErrorMessage()
                {
                    Error = false,
                    Status = "Success",
                    Message = "inserted suceesfully",
                };

            }
            catch (Exception ex)
            {

                return new ErrorMessage()
                {
                    Error = true,
                    Status = "Failure",
                    Message = ex.Message + "" + ex.InnerException + "" + error,
                };

            }
        }
        #endregion
        #region Details
        [HttpGet]
        [Route("GetRequestDetails/{id}")]
        public async Task<RequestFrontVM> GetRequestDetails(long id)
        {
            //var SpecRequest = new RequestWithSpecificService((int)id, (int)ServiceEnum.Tourism, false);
            var SpecRequest = new RequestWithSpecificService((int)id, true);
            var RequestDetails = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                              .GetByIdWithSpec(SpecRequest);
            var PaymentPerRequest = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>()
                            .GetByCondition(x => x.RequestId == id).FirstOrDefaultAsync();
            var AttachmenttRequest = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>()
                           .GetByCondition(x => x.AttachRequestid == id && !(x.IsLatest == false && x.IsApproved == true)).ToListAsync();
            var UserApplicant = await _unitOfwork.genericRepository<AspNetUser>()
                          .GetByCondition(x => x.CivilId == RequestDetails.AppCivilId).FirstOrDefaultAsync();
            var person = new PersonVM();

            if (RequestDetails.LicTypeId == (int)LicTypeEnum.Media_Organization_Individuals)
            {
                var PersonSpec = new PersonApplicantWithSpec(RequestDetails.AppCivilId, (int)ServiceEnum.Elaw);
                var applicant = await _unitOfwork.genericRepository<Person>()
                    .GetByIdWithSpec(PersonSpec);
                person = _mapper.Map<Person, PersonVM>(applicant);


            }
            List<string> allowedGroups = new();
            List<string> ViewType = new();

            //if (RequestDetails.LicTypeId == (int)LicTypeEnum.Media_Organization_Individuals)
            //{
            //    switch (RequestDetails?.RequestStatusId)
            //    {
            //        case (int)RequestStatusEnum.CriminalCase: // بإنتظار إرفاق الصحيفة الجنائية
            //            allowedGroups.Add("CriminalCase");
            //            ViewType.Add("LicRequestForPerson");
            //            break;

            //        case (int)RequestStatusEnum.WaitingApprovalCommittee: // بإنتظار توقيع الإقرار
            //            allowedGroups.Add("WaitingApprovalCommittee");
            //            ViewType.Add("LicRequestForPerson");
            //            break;

            //            // You can add more cases here if needed
            //    }
            //}
            //else
            //{
            //    switch (RequestDetails?.RequestStatusId)
            //    {
            //        case (int)RequestStatusEnum.CriminalCase: // بإنتظار إرفاق الصحيفة الجنائية
            //            allowedGroups.Add("CriminalCase");
            //            //ViewType.Add("LicRequestForPerson");
            //            break;

            //        case (int)RequestStatusEnum.WaitingApprovalCommittee: // بإنتظار توقيع الإقرار
            //            allowedGroups.Add("WaitingApprovalCommittee");
            //            //ViewType.Add("LicRequestForPerson");
            //            break;


            //            // You can add more cases here if needed
            //    }
            //}

            //// 2. Fetch file upload configs based on allowed groups
            //List<AttachRule> fileUploadConfigs = new();
            var fileUploadConfigs = await _unitOfwork.genericRepository<AttachRule>()
               .GetByCondition(f => f.ServiceId== RequestDetails.ServiceId
               &&f.RequestStatusId== RequestDetails.RequestStatusId
               &&f.RequestTypeId== RequestDetails.ReqtypeId&&f.FlagView=="user")
               .ToListAsync();
            //if (allowedGroups.Any())
            //{
            //    fileUploadConfigs = await _unitOfwork.genericRepository<AttachRule>()
            //        .GetByCondition(f => allowedGroups.Contains(f.FieldName))
            //        .ToListAsync();
            //}
            return new RequestFrontVM
            {
                RequestVM = _mapper.Map<MoiEserviceLicensesRequest, RequestVM>(RequestDetails),
                PaymentDetailsVM = _mapper.Map<MoiEserviceRequestPaymentDetail, PaymentDetailsVM>(PaymentPerRequest),
                attachVMs = _mapper.Map<IEnumerable<MoiEserviceRequestsAttach>, IEnumerable<AttachVM>>(AttachmenttRequest),
                //AspnetUserVM = _mapper.Map<AspNetUser, AspnetUserVM>(UserApplicant),
                ApplicantPerson = person,
                fileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(fileUploadConfigs),

            };

        }

        [HttpGet]
        [Route("GetLicenseDetails")]
        public async Task<LicenceDetailsVM> GetLicenseDetails(int id)
        {
            // Try to get license details
            var licencesSpec = new LicencesWithSpecificService(id, (int)ServiceEnum.Elaw);
            var licencesDetails = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
            bool isRenewable = false;
            MoiPreApprovement? preApprovDetails = null;
            string applicantCivilId = string.Empty;



            // Get the applicant based on whichever civil ID we found
            var applicant = !string.IsNullOrEmpty(licencesDetails.ApplicantCivilId)
                ? await _unitOfwork.genericRepository<Person>()
                    .GetByCondition(u => u.CivilId == licencesDetails.ApplicantCivilId)
                    .FirstOrDefaultAsync()
                : null;

            List<long> requestIds = new List<long>();


            // Get RequestId from `licencesDetails`
            var RequestForLicences = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                .GetByCondition(r => r.LicenseId == licencesDetails.LicId).ToListAsync();
            requestIds = RequestForLicences.Select(r => r.RequestId).ToList();

            if (licencesDetails?.ExpireDate != null)
            {
                var remainingTime = licencesDetails.ExpireDate.Value - DateTime.Now;
                isRenewable = remainingTime.TotalDays <= 30;
            }

            List<int?> HaveRequestInSameRequestType = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                        .GetByCondition(r => r.LicenseId == id &&
                        r.AppCivilId == licencesDetails.ApplicantCivilId
                        && r.RequestStatusId != (int)RequestStatusEnum.FinalLicenseIssued
                        && r.RequestStatusId != (int)RequestStatusEnum.RequestDeclined)
                        .Select(r => r.ReqtypeId).ToListAsync();
            // Retrieve all attachments for the requestIds
            var AttachmentForLicences = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>()
                .GetByCondition(a => requestIds.Contains(a.AttachRequestid.Value))
                .ToListAsync();
            var allowedTypes = new List<TransactionTypesEnum>
                 {

                    TransactionTypesEnum.ChangeEmail,
                    TransactionTypesEnum.ChangeSocialMedia,
                    //TransactionTypesEnum.ChangePartnerName,
                    TransactionTypesEnum.ChangeAddress,
                    TransactionTypesEnum.ChangeManager,
                    TransactionTypesEnum.ChangeLicencesName,
                    TransactionTypesEnum.ChangeLicencesType
                 };
            if (licencesDetails.LicTypeId == (int)LicTypeEnum.Media_Organization_Company)
            {
                allowedTypes.Add(TransactionTypesEnum.ChangePartnerName);
            }

            var transactionOptions = allowedTypes
                    .Select(e => new EnumOptionVM
                    {
                        Value = (int)e,
                        Text = EnumHelper.GetDisplayName(e)
                    }).ToList();

            return new LicenceDetailsVM
            {
                TransactionTypeOptions = transactionOptions,
                LicencesVM = licencesDetails != null
                    ? _mapper.Map<Licence, LicencesVM>(licencesDetails)
                    : null,
                PersonApplicantVM = applicant != null
                    ? _mapper.Map<Person, PersonVM>(applicant)
                    : null,

                attachmentVM = AttachmentForLicences != null
                ? _mapper.Map<IEnumerable<MoiEserviceRequestsAttach>, IEnumerable<AttachVM>>(AttachmentForLicences) : null,
                IsRenewable = isRenewable,
                RequestTypesId = HaveRequestInSameRequestType
            };
        }
        #endregion
        [HttpPost]
        [Route("Request/InsertAttachements")]
        public async Task<dynamic> InsertAttachements(List<FileSaveResponseVM> fileSaveResponseVMs, long reqid, string sessioncivilid)
        {
            string error = string.Empty;
            try
            {

                foreach (var file in fileSaveResponseVMs)
                {

                    MoiEserviceRequestsAttach ReqAttach = new MoiEserviceRequestsAttach()
                    {
                        AttachName = file.LabelName,
                        AttachRequestid = reqid,
                        AttachStatus = "OK",
                        AttachType = "Main",
                        AttachPath = file.FilePath,
                        IsApproved = true,
                        IsMandatory = file.IsRequired,
                        ServiceId = (int)ServiceEnum.Elaw,
                        AttachFlag = file.FileName,
                        IsLatest = true,
                        UploadedBy = sessioncivilid,
                        UploadedDate = DateTime.Now,
                        IsDeleted = false,

                    };

                    var _mappedReqAttach = _mapper.Map<MoiEserviceRequestsAttach>(ReqAttach);
                    await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>().Create(ReqAttach);
                }

                await _unitOfwork.Complete();

                return new ErrorMessage()
                {
                    Error = false,
                    Status = "Success",
                    Message = "inserted suceesfully",
                };

            }
            catch (Exception ex)
            {

                return new ErrorMessage()
                {
                    Error = true,
                    Status = "Failure",
                    Message = ex.Message + "" + ex.InnerException + "" + error,
                };

            }
        }

        [HttpPost]
        [Route("Request/InsertUpdateAttachement")]
        public async Task<dynamic> InsertUpdateAttachement(UpdatedAttachVM model)
        {
            try
            {
                var result = await _updateDataService.InsertUpdateAttachementToTable(model, (int)ServiceEnum.Elaw);

                bool allCorrected = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>()
    .GetByCondition(a => a.AttachRequestid == model.RequestId && a.IsApproved == false && a.IsLatest == false)
    .AnyAsync();
                var request = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                        .GetByCondition(r => r.RequestId == model.RequestId)
                        .FirstOrDefaultAsync();

                if (!allCorrected)
                {

                    if (request.RequestStatusId == (int)RequestStatusEnum.CorrectData)
                    {
                        if (request != null)
                        {
                            request.RequestStatusId = (int)RequestStatusEnum.WaitingForReview;
                            await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Update(request);
                            await _unitOfwork.Complete();
                        }
                    }
                }
                if (request != null)
                {
                    var nextstatus = await _unitOfwork.genericRepository<WorkFlow>()
                        .GetByCondition(w => w.CurrentStatusId == request.RequestStatusId
                        && w.RequestTypeId == request.ReqtypeId &&
                        w.ServiceId == request.ServiceId).FirstOrDefaultAsync();
                    if (nextstatus != null)
                    {
                        request.RequestStatusId = nextstatus.NextStatusId;
                        await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Update(request);
                        await _unitOfwork.Complete();
                    }
                }
                return Ok(result); // returns ErrorMessage as JSON
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorMessage
                {
                    Error = true,
                    Status = "Failure",
                    Message = ex.Message + (ex.InnerException?.Message ?? "")
                });
            }
        }
        #region Common
        // private async Task<ErrorMessage> HandleCommonElawRequest(
        //PostRequestApiModel model,
        //RequestTypeEnum requestType,
        //Func<long, int, Task> handleExtras = null!)
        // {
        //     using var dbTransaction = _unitOfwork.BeginTransaction();
        //     try
        //     {
        //         string requesterId = model.accountTypeId switch
        //         {
        //             "100" => model.AppId.ToString(),
        //             "300" => model.MandoobId,
        //             _ => null
        //         };

        //         var licence = await _unitOfwork.genericRepository<Licence>()
        //                           .GetByCondition(l => l.LicId == model.LicId)
        //                           .FirstOrDefaultAsync();

        //         var reqModel = new MoiEserviceLicensesRequest
        //         {
        //             Reqno = model.reqno,
        //             ReqtypeId = (int)requestType,
        //             Licno = model.LicNo,
        //             ActivityType = await _unitOfwork.genericRepository<ActivityTypesLookup>()
        //                 .GetByCondition(a => a.Id == model.ActivityTypeId).Select(a => a.NameAr).FirstOrDefaultAsync(),
        //             ServiceId = (int)ServiceEnum.Elaw,

        //             ManagerId = model.ManId,

        //             CompanyId = model.CompanyId,
        //             LicenseId = model.LicId,

        //             SequenceNo = model.SequenceNo,
        //             Licreqtime = DateTime.Now,
        //             Requesterid = requesterId,
        //             RequestStatusId = (int)RequestStatusEnum.Received,
        //             RequestAttach = "Yes",
        //             Licamount = model.Amount,
        //             Licpaystatus = "0",
        //             CategoryId = 1,
        //             SectorId = 3,
        //             AppCivilId = model.AppCivilId,
        //             ManCivilId = model.ManagerCivilid,
        //             UserCivilId = model.UserCivilID,

        //             LicStatusId = (int)licencesStatusEnum.Pending,
        //             ActivityTypeId = model.ActivityTypeId,
        //             LicrequestIsDeleted = false,
        //             IsArchived = false,
        //             LicTypeId = model.LictypeId,

        //         };

        //         await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(reqModel);
        //         await _unitOfwork.Complete();

        //         long requestId = reqModel.RequestId;

        //         var transaction = new RequestTransaction
        //         {
        //             ReqStatusId = (int)RequestStatusEnum.Received,
        //             ReqTypeId = (int)requestType,
        //             RequestId = requestId,
        //             Notes = requestType.ToString(),
        //             Status = RequestStatusEnum.Received.ToString(),
        //             CreatedDate = DateTime.Now,
        //             CreatedBy = model.SessionName,
        //             CivilIdUser = model.SessionCivilId,
        //             ServiceId = (int)ServiceEnum.Elaw,
        //             UpdatedDate = DateTime.Now
        //         };

        //         await _unitOfwork.genericRepository<RequestTransaction>().Create(transaction);
        //         await _unitOfwork.Complete();

        //         int requestTransactionId = transaction.Id;

        //         if (handleExtras != null)
        //             await handleExtras.Invoke(requestId, requestTransactionId); // <-- pass both IDs

        //         await InsertAttachements(model.saveResponseVMs, requestId);

        //         dbTransaction.Commit();
        //         return new ErrorMessage { Error = false, Status = "Success", Message = "Inserted successfully" };
        //     }
        //     catch (Exception ex)
        //     {
        //         dbTransaction.Rollback();
        //         return new ErrorMessage
        //         {
        //             Error = true,
        //             Status = "Failure",
        //             Message = ex.Message + " " + ex.InnerException?.Message
        //         };
        //     }
        // }




        #endregion
        #region Renew
        [HttpGet]
        [Route("GetLicenseDetailsForRenew")]
        public async Task<RequestElawBaseVM> GetLicenseDetailsForRenew(int LicId)
        {
            var licencesSpec = new LicencesWithSpecificService(LicId, (int)ServiceEnum.Elaw);
            var licencesDetails = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
            var fileUploadConfigurationsFront = await _unitOfwork.genericRepository<AttachRule>()
                                             .GetByCondition(f => f.ViewType == "RenewRequestElaw").ToListAsync();
            return new RequestElawBaseVM
            {
                FileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(fileUploadConfigurationsFront),
                LicencesVM = _mapper.Map<Licence, LicencesVM>(licencesDetails)
            };


        }
        [HttpPost]
        [Route("PostDataRenewRequest")]
        public async Task<dynamic> PostDataRenewRequest(PostRequestApiModel model)
        {
            return await HandleCommonElawRequest(model, RequestTypeEnum.Renew, async (requestId, reqTransId) =>
            {
                var licence = await _unitOfwork.genericRepository<Licence>()
                    .GetByCondition(l => l.LicId == model.LicId).FirstOrDefaultAsync();

                await _unitOfwork.genericRepository<LicenseRenew>().Create(new LicenseRenew
                {
                    LicenseId = model.LicId,
                    OldExpiryDate = licence?.ExpireDate,
                    RequestStatusId = (int)RequestStatusEnum.Received,
                    ServiceId = (int)ServiceEnum.Elaw,
                    ReqTransId = reqTransId // ✅ Correct assignment
                });

                await _unitOfwork.Complete();
            });
        }

        #endregion

        #region إنهاء
        //-------------------------إنهاء-----------------
        [HttpGet]
        [Route("GetLicenseDetailsForendLicences")]
        public async Task<RequestElawBaseVM> GetLicenseDetailsForendLicences(int LicId)
        {
            var licencesSpec = new LicencesWithSpecificService(LicId, (int)ServiceEnum.Elaw);
            var licencesDetails = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
            var fileUploadConfigurationsFront = await _unitOfwork.genericRepository<AttachRule>()
                                             .GetByCondition(f => f.ViewType == "EndLicencesRequestElaw").ToListAsync();
            var EndReason = await _unitOfwork.genericRepository<MoiEserviceLicEndingReason>().GetAllAsync();

            return new RequestElawBaseVM
            {
                FileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(fileUploadConfigurationsFront),
                LicencesVM = _mapper.Map<Licence, LicencesVM>(licencesDetails),
                EndingReasons = EndReason.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.ReasonName

                }).ToList()

            };


        }
        [HttpPost]
        [Route("PostDataEndLicencesRequest")]
        public async Task<dynamic> PostDataEndLicencesRequest(PostRequestApiModel model)
        {
            return await HandleCommonElawRequest(model, RequestTypeEnum.EndLicences, async (requestId, requestTransactionId) =>
            {
                var licence = await _unitOfwork.genericRepository<Licence>()
                    .GetByCondition(l => l.LicId == model.LicId).FirstOrDefaultAsync();

                await _unitOfwork.genericRepository<LicenseEndingTransaction>().Create(new LicenseEndingTransaction
                {
                    LicenseId = model.LicId,
                    EndReasonId = model.EndingReasonId,
                    LicExpiredate = licence?.ExpireDate,
                    RequestId = (int)requestId,
                    TransactionId = requestTransactionId,
                    ServiceId = (int)ServiceEnum.Elaw,
                    LastUpdateDate = DateTime.Now
                });

                await _unitOfwork.Complete();
            });
        }

        #endregion
        #region  التنازل 
        [HttpGet]
        [Route("GetLicenseDetailsForRenouncement")]
        public async Task<RequestElawBaseVM> GetLicenseDetailsForRenouncement(int LicId)
        {
            var licencesSpec = new LicencesWithSpecificService(LicId, (int)ServiceEnum.Elaw);
            var licencesDetails = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
            var fileUploadConfigurationsFront = await _unitOfwork.genericRepository<AttachRule>()
                                             .GetByCondition(f => f.ViewType == "RenouncementRequestElaw").ToListAsync();
            var EndReason = await _unitOfwork.genericRepository<MoiEserviceLicEndingReason>().GetAllAsync();

            return new RequestElawBaseVM
            {
                FileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(fileUploadConfigurationsFront),
                LicencesVM = _mapper.Map<Licence, LicencesVM>(licencesDetails),


            };


        }

        [HttpPost]
        [Route("PostDataRenouncementRequest")]
        public async Task<dynamic> PostDataRenouncementRequest(PostRequestApiModel model)
        {
            return await HandleCommonElawRequest(model, RequestTypeEnum.Renouncement, async (requestId, requestTransactionId) =>
            {
                // Get the full name of the old applicant
                var usernameForApplicant = await _unitOfwork.genericRepository<Person>()
                    .GetByCondition(a => a.CivilId == model.AppCivilId).FirstOrDefaultAsync();

                // Insert into RenouncementTransaction
                await _unitOfwork.genericRepository<RenouncementTransaction>().Create(new RenouncementTransaction
                {
                    LicencesId = model.LicId,
                    NewCivilId = model.NewCivilIdApplicant,
                    NewName = model.NewApplicantName1 + model.NewApplicantName2 + model.NewApplicantName3 + model.NewApplicantName4,
                    OldCivilId = model.AppCivilId,
                    OldName = usernameForApplicant?.Name1,
                    RequestId = (int)requestId,
                    ReqTransactionId = requestTransactionId,
                    ServiceId = (int)ServiceEnum.Elaw,
                    LastUpdateDate = DateTime.Now
                });

                await _unitOfwork.Complete();
            });
        }


        #endregion
        #region بدل فاقد
        [HttpGet]
        [Route("GetLicenceDetailsForReplacementOfLost")]
        public async Task<RequestBaseVM> GetLicenceDetailsForReplacementOfLost(int LicId)
        {
            var licencesSpec = new LicencesWithSpecificService(LicId, (int)ServiceEnum.Elaw);
            var licencesDetails = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
            var fileUploadConfigurationsFront = await _unitOfwork.genericRepository<AttachRule>()
                                             .GetByCondition(f => f.ViewType == "ReplacementOfLostRequestElaw").ToListAsync();
            var EndReason = await _unitOfwork.genericRepository<MoiEserviceLicEndingReason>().GetAllAsync();

            return new RequestBaseVM
            {
                FileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(fileUploadConfigurationsFront),
                LicencesVM = _mapper.Map<Licence, LicencesVM>(licencesDetails),


            };


        }
        [HttpPost]
        [Route("PostDataReplacementOfLostRequest")]
        public async Task<dynamic> PostDataReplacementOfLostRequest(PostRequestApiModel model)
        {
            return await HandleCommonElawRequest(model, RequestTypeEnum.ReplacementOfLost, async (requestId, requestTransactionId) =>
            {
                // Get the full name of the old applicant
                var usernameForApplicant = await _unitOfwork.genericRepository<Person>()
                    .GetByCondition(a => a.CivilId == model.AppCivilId).FirstOrDefaultAsync();

                // Insert into RenouncementTransaction
                await _unitOfwork.genericRepository<ReplacementOfLostTransaction>().Create(new ReplacementOfLostTransaction
                {
                    LicId = model.LicId,

                    RequestId = (int)requestId,
                    ReqTransactionId = requestTransactionId,
                    ServiceId = (int)ServiceEnum.Elaw,
                    LastUpdateDate = DateTime.Now
                });

                await _unitOfwork.Complete();
            });
        }
        #endregion
        #region تغيير البيانات 
        [HttpGet]
        [Route("GetLicenceDetailsForChangeData")]
        public async Task<RequestElawBaseVM> GetLicenceDetailsForChangeData(int LicId, [FromQuery(Name = "TransactionTypeIds")] List<int> TransactionTypeIds)
        {
            var licencesSpec = new LicencesWithSpecificService(LicId, (int)ServiceEnum.Elaw);
            var licencesDetails = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
            // List<FileUploadConfigurationsFront> fileUploads = new List<FileUploadConfigurationsFront>();
            var socialMedia = await _unitOfwork.genericRepository<MoiSocialMedia>().
                             GetByCondition(s => s.LicenceId == LicId).ToListAsync();
            var Partner = await _unitOfwork.genericRepository<Partner>().
                            GetByCondition(s => s.LicenseId == LicId).ToListAsync();
            var qualification = await _unitOfwork.genericRepository<QualificationsLookup>()
                      .GetByCondition(q => !string.IsNullOrEmpty(q.Name))
                .Select(q => new SelectListItem
                {
                    Value = q.Id.ToString(),
                    Text = q.Name
                }).ToListAsync();

            var viewTypeMapping = new Dictionary<TransactionTypesEnum, string>
    {
        { TransactionTypesEnum.ChangeManager, "ChangeManagerRequestElaw" },
        { TransactionTypesEnum.ChangeEmail, "ChangeEmailRequestElaw" },
        { TransactionTypesEnum.ChangeAddress, "ChangeAddressRequestElaw" },
        { TransactionTypesEnum.ChangeLicencesName, "ChangeLicencesNameRequestElaw" },
        { TransactionTypesEnum.ChangePartnerName, "ChangePartnerNameRequestElaw" },
        { TransactionTypesEnum.ChangeLicencesType, "ChangeLicencesTypeRequestElaw" },
        { TransactionTypesEnum.ChangeSocialMedia, "ChangeSocialMediaRequestElaw" }
    };

            var fileUploadsDict = new Dictionary<string, AttachRule>();

            foreach (var transId in TransactionTypeIds)
            {
                if (viewTypeMapping.TryGetValue((TransactionTypesEnum)transId, out var viewType))
                {
                    var uploads = await _unitOfwork.genericRepository<AttachRule>()
                                                   .GetByCondition(f => f.ViewType == viewType)
                                                   .ToListAsync();

                    foreach (var upload in uploads)
                    {
                        if (!string.IsNullOrEmpty(upload.FieldName) && !fileUploadsDict.ContainsKey(upload.FieldName))
                        {
                            fileUploadsDict[upload.FieldName] = upload;
                        }
                    }
                }
            }
            //var fileUploads = fileUploadsDict.Values.ToList();

            var LicencesType = await _unitOfwork.genericRepository<LicenceTypesLookup>()
                          .GetByCondition(l => l.Id != (int)LicTypeEnum.Company
                          && l.Id != (int)LicTypeEnum.OrganizationOrPerson
                          && l.Id != (int)LicTypeEnum.Organization
                          && l.Id != licencesDetails.LicTypeId).ToListAsync();
            var licencesInfoDetails = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                          .GetByCondition(r => r.ActvityTypeId == licencesDetails.ActiivityTypeId
                          && r.ReqTypeId == (int)RequestTypeEnum.ChangeData
                          && r.ServiceId == (int)ServiceEnum.Elaw).FirstOrDefaultAsync();
            var baseFee = licencesInfoDetails?.FixedFees ?? 0;

            // Calculate total based on how many distinct transaction types were selected
            var totalFee = baseFee * TransactionTypeIds.Distinct().Count();

            // Update the mapped licence info before returning

            var mappedLicence = _mapper.Map<LicencesInfoVM>(licencesInfoDetails);
            mappedLicence.FixedFees = totalFee;
            List<AttachRule> fileUploads = fileUploadsDict.Values.ToList();

            return new RequestElawBaseVM
            {
                socialMediaVMs = _mapper.Map<List<MoiSocialMedia>, List<SocialMediaVM>>(socialMedia),
                FileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(fileUploads),
                LicencesVM = _mapper.Map<Licence, LicencesVM>(licencesDetails),
                SelectedTransactionTypeIds = TransactionTypeIds,
                QualificationSelectedList = qualification,
                licencesTypes = _mapper.Map<List<LicenceTypesLookup>, List<LicencesTypeVM>>(LicencesType),
                PartnerVMs = _mapper.Map<List<Partner>, List<PartnerVM>>(Partner),

            };
        }

        [HttpPost]
        [Route("PostDataChangeDataRequest")]
        public async Task<dynamic> PostDataChangeDataRequest(PostRequestApiModel model)
        {
            using var dbTransaction = _unitOfwork.BeginTransaction();

            try
            {
                if (model.SelectedTransactionTypeIds == null || !model.SelectedTransactionTypeIds.Any())
                {
                    return new ErrorMessage
                    {
                        Error = true,
                        Status = "Failure",
                        Message = "لم يتم اختيار أي نوع تعديل"
                    };
                }
                await HandleCommonElawRequest(model, RequestTypeEnum.ChangeData, async (requestId, requestTransactionId) =>
              {
                  foreach (var transTypeId in model.SelectedTransactionTypeIds)
                  {
                      switch ((TransactionTypesEnum)transTypeId)
                      {
                          case TransactionTypesEnum.ChangeManager:
                              await HandleChangeManager(model, requestId);
                              break;
                          case TransactionTypesEnum.ChangeAddress:
                              await HandleChangeAddress(model, requestId);
                              break;
                          case TransactionTypesEnum.ChangeLicencesName:
                              await HandleChangeLicenceName(model, requestId);
                              break;

                          case TransactionTypesEnum.ChangeSocialMedia:
                              await HandleChangeSocialMedia(model, requestId);
                              break;
                          case TransactionTypesEnum.ChangeEmail:
                              await HandleChangeEmail(model, requestId);
                              break;
                          case TransactionTypesEnum.ChangeLicencesType:
                              await HandleChangeLicType(model, requestId);
                              break;
                      }
                  }
                  await _unitOfwork.Complete();
              });
                await dbTransaction.CommitAsync();

                return new ErrorMessage
                {
                    Error = false,
                    Status = "Success",
                    Message = "تمت الإضافة بنجاح"
                };
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();

                return new ErrorMessage
                {
                    Error = true,
                    Status = "Failure",
                    Message = ex.Message + (ex.InnerException != null ? " - " + ex.InnerException.Message : "")
                };
            }
        }

        private async Task<ErrorMessage> HandleCommonElawRequest(
            PostRequestApiModel model,
            RequestTypeEnum requestType,
            Func<long, int?, Task> handleExtras)
        {
            //using var dbTransaction = _unitOfwork.BeginTransaction();
            try
            {
                //var requesterId = model.accountTypeId switch
                //{
                //    "100" => model.AppId.ToString(),
                //    "300" => model.MandoobId,
                //    _ => null
                //};

                var licence = await _unitOfwork.genericRepository<Licence>()
                    .GetByCondition(l => l.LicId == model.LicId).FirstOrDefaultAsync();


                var requesterId = await _unitOfwork.genericRepository<AspNetUser>()
                    .GetByCondition(a => a.CivilId == model.SessionCivilId).FirstOrDefaultAsync();
                var reqModel = new MoiEserviceLicensesRequest
                {
                    Reqno = model.reqno,
                    ReqtypeId = (int)requestType,
                    Licno = model.LicNo,
                    ActivityType = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                        .GetByCondition(a => a.Id == model.ActivityTypeId).Select(a => a.NameAr).FirstOrDefaultAsync(),
                    ServiceId = (int)ServiceEnum.Elaw,
                    Licowner = licence.Licowner,
                    Licname = licence.LicName,
                    ManagerId = model.ManId,
                    CompanyId = model.CompanyId,
                    LicenseId = model.LicId,
                    AppId=model.AppId,
                    SequenceNo = model.SequenceNo,
                    Licreqtime = DateTime.Now,
                    Requesterid = requesterId.Id,
                    RequestStatusId = (int)RequestStatusEnum.WaitingForReview,
                    RequestAttach = "Yes",
                    Licamount = model.Amount,
                    Licpaystatus = "0",
                    CategoryId = 1,
                    SectorId = 3,
                    AppCivilId = model.AppCivilId,
                    RequesterCivilId = model.SessionCivilId,
                    ManCivilId=model.ManagerCivilid,
                    //ManCivilId = model.ManCivil,
                    //UserCivilId = model.SessionCivilId,
                    LicStatusId = (int)licencesStatusEnum.Released,
                    ActivityTypeId = model.ActivityTypeId,
                    LicrequestIsDeleted = false,
                    IsArchived = false,
                    LicTypeId = model.LictypeId,

                };

                await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(reqModel);
                await _unitOfwork.Complete();

                var requestId = reqModel.RequestId;
                int requestTransactionId = 0;

                if (requestType != RequestTypeEnum.ChangeData)
                {
                    var transaction = new RequestTransaction
                    {
                        ReqStatusId = (int)RequestStatusEnum.Received,
                        LicenseId = model.LicId,
                        ReqTypeId = (int)requestType,
                        RequestId = requestId,
                        Notes = requestType.ToString(),
                        Status = RequestStatusEnum.Received.ToString(),
                        CreatedDate = DateTime.Now,
                        CreatedBy = model.UserName,
                        CivilIdUser = model.SessionCivilId,
                        ServiceId = (int)ServiceEnum.Elaw,
                        UpdatedDate = DateTime.Now
                    };

                    await _unitOfwork.genericRepository<RequestTransaction>().Create(transaction);
                    await _unitOfwork.Complete();
                    requestTransactionId = transaction.Id;
                }

                if (handleExtras != null)
                    await handleExtras.Invoke(requestId, requestTransactionId);

                await InsertAttachements(model.saveResponseVMs, requestId, model.SessionCivilId);
                await _unitOfwork.Complete();

                //dbTransaction.Commit();
                return new ErrorMessage { Error = false, Status = "Success", Message = "Inserted successfully" };
            }
            catch (Exception ex)
            {
                //dbTransaction.Rollback();
                return new ErrorMessage
                {
                    Error = true,
                    Status = "Failure",
                    Message = ex.Message + " " + ex.InnerException?.Message
                };
            }
        }

        private async Task HandleChangeManager(PostRequestApiModel model, long requestId)
        {
            try
            {
                var licen = await _unitOfwork.genericRepository<Licence>().GetByCondition(l => l.LicId == model.LicId)
                    .FirstOrDefaultAsync();
                var trans = new Domain.Entities.Transaction
                {
                    RequestId = requestId,
                    Commited = false,
                    LicenseId = model.LicId,
                    ReqStatusId = (int)RequestStatusEnum.Received,
                    RequestDate = DateTime.Now,
                    TransTypeId = (int)TransactionTypesEnum.ChangeManager,
                    UsercivilId = model.SessionCivilId,
                    TransDate = DateTime.Now,
                    ServiceId = (int)ServiceEnum.Elaw,
                    Notes = "تغيير المدير"
                };
                await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(trans);
                await _unitOfwork.Complete();


                var addressChange = new AddressChangeTransaction
                {
                    RequestId = (int)requestId,
                    TransactionId = trans.Id,
                    ServiceId = (int)ServiceEnum.Elaw,
                    NewArea = model.NewAreaManager,
                    OldArea = model.OldAreaManager,
                    NewBlock = model.NewBlockManager,
                    OldBlock = model.OldBlockManager,
                    NewStreet = model.NewStreetManager,
                    OldStreet = model.OldStreetManager,
                    NewBuildingNo = model.NewBuildingNoManager,
                    OldBuildingNo = model.OldBuildingNoManager,
                    NewBuildingName = model.NewBuildingNameManager,
                    OldBuildingName = model.OldBuildingNameManager,
                    NewUnitNo = model.NewUnitNoManager,
                    OldUnitNo = model.OldUnitNoManager,
                    NewFloor = model.NewFloorNoManager,
                    OldFloor = model.OldFloorNoManager,
                    NewGovernorate = model.NewGovernateManager,
                    OldGovernorate = model.OldGovernateManager,
                    AalliNoNew = model.NewAaliNoManager,
                    AalliNoOld = model.OldAaliNoManager,
                    LicenceId = model.LicId,
                    LastUpdateDate = DateTime.Now
                };
                await _unitOfwork.genericRepository<AddressChangeTransaction>().Create(addressChange);
                await _unitOfwork.Complete();
                var change = new TchangeManager
                {
                    RequestId = (int)requestId,
                    TransactionId = trans.Id,
                    ServiceId = (int)ServiceEnum.Elaw,
                    ManagerNewcivilid = model.NewCivilIdManager,
                    ManagerNewname1 = model.NewManagerName1,
                    ManagerNewname2 = model.NewManagerName2,
                    ManagerNewname3 = model.NewManagerName3,
                    ManagerNewname4 = model.NewManagerName4,
                    NewMobile = model.NewMobileManager,
                    NewEmail = model.NewEmailManager,
                    ManagerOldcivilid = model.OldCivilIdManager,
                    ManagerOldname = model.OldManagerName1,
                    OldMobile = model.OldMobileManager,
                    OldEmail = model.OldEmailManager,
                    LastUpdateDate = DateTime.Now,
                    OldManagerId = licen.ManagerId ?? 0,
                    ManagerLicno = licen.LicNo,
                    ChanAddressId = addressChange.Id,


                };
                await _unitOfwork.genericRepository<TchangeManager>().Create(change);
                await _unitOfwork.Complete();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HandleChangeLicenceName: " + ex.Message);
                throw; // rethrow or log more deeply
            }
        }

        private async Task HandleChangeAddress(PostRequestApiModel model, long requestId)
        {
            try
            {
                var trans = new Domain.Entities.Transaction
                {
                    RequestId = requestId,
                    Commited = false,
                    LicenseId = model.LicId,
                    ReqStatusId = (int)RequestStatusEnum.Received,
                    RequestDate = DateTime.Now,
                    TransTypeId = (int)TransactionTypesEnum.ChangeAddress,
                    UsercivilId = model.SessionCivilId,
                    TransDate = DateTime.Now,
                    ServiceId = (int)ServiceEnum.Elaw,
                    Notes = "تغيير العنوان"
                };
                await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(trans);
                await _unitOfwork.Complete();

                var addressChange = new AddressChangeTransaction
                {
                    RequestId = (int)requestId,
                    TransactionId = trans.Id,
                    ServiceId = (int)ServiceEnum.Elaw,
                    NewArea = model.NewAreaManager,
                    OldArea = model.OldAreaManager,
                    NewBlock = model.NewBlockManager,
                    OldBlock = model.OldBlockManager,
                    NewStreet = model.NewStreetManager,
                    OldStreet = model.OldStreetManager,
                    NewBuildingNo = model.NewBuildingNoManager,
                    OldBuildingNo = model.OldBuildingNoManager,
                    NewBuildingName = model.NewBuildingNameManager,
                    OldBuildingName = model.OldBuildingNameManager,
                    NewUnitNo = model.NewUnitNoManager,
                    OldUnitNo = model.OldUnitNoManager,
                    NewFloor = model.NewFloorNoManager,
                    OldFloor = model.OldFloorNoManager,
                    NewGovernorate = model.NewGovernateManager,
                    OldGovernorate = model.OldGovernateManager,
                    AalliNoNew = model.NewAaliNoManager,
                    AalliNoOld = model.OldAaliNoManager,

                    LastUpdateDate = DateTime.Now
                };
                await _unitOfwork.genericRepository<AddressChangeTransaction>().Create(addressChange);
                await _unitOfwork.Complete();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HandleChangeLicenceName: " + ex.Message);
                throw; // rethrow or log more deeply
            }

        }

        private async Task HandleChangeLicenceName(PostRequestApiModel model, long requestId)
        {
            try
            {
                var trans = new Domain.Entities.Transaction
                {
                    RequestId = requestId,
                    Commited = false,
                    LicenseId = model.LicId,
                    ReqStatusId = (int)RequestStatusEnum.Received,
                    RequestDate = DateTime.Now,
                    TransTypeId = (int)TransactionTypesEnum.ChangeLicencesName,
                    UsercivilId = model.SessionCivilId,
                    TransDate = DateTime.Now,
                    ServiceId = (int)ServiceEnum.Elaw,
                    Notes = "تغيير إسم الرخصة"
                };
                await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(trans);
                await _unitOfwork.Complete();

                await _unitOfwork.genericRepository<LicencesNameChangeTransaction>().Create(new LicencesNameChangeTransaction
                {
                    RequestId = (int)requestId,
                    TransactionId = trans.Id,
                    ServiceId = (int)ServiceEnum.Elaw,
                    LicencesNameNew = model.NewLicencesName,
                    LicencesNameOld = model.OldLicencesName,
                    LicencesId = model.LicId
                });
                await _unitOfwork.Complete();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HandleChangeLicenceName: " + ex.Message);
                throw; // rethrow or log more deeply
            }
        }

        private async Task HandleChangeSocialMedia(PostRequestApiModel model, long requestId)
        {
            try
            {
                var trans = new Domain.Entities.Transaction
                {
                    RequestId = requestId,
                    Commited = false,
                    LicenseId = model.LicId,
                    ReqStatusId = (int)RequestStatusEnum.Received,
                    RequestDate = DateTime.Now,
                    TransTypeId = (int)TransactionTypesEnum.ChangeSocialMedia,
                    UsercivilId = model.SessionCivilId,
                    TransDate = DateTime.Now,
                    ServiceId = (int)ServiceEnum.Elaw,
                    Notes = "تغيير السوشيال ميديا"
                };
                await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(trans);
                await _unitOfwork.Complete();
                var OldSocialMedia = await _unitOfwork.genericRepository<MoiSocialMedia>()
                                  .GetByCondition(s => s.LicenceId == model.LicId).ToListAsync();
                var socialMediaList = new List<ChangeSocialMediaTransaction>();
                if (!string.IsNullOrEmpty(model.NewTwitter))
                {
                    socialMediaList.Add(new ChangeSocialMediaTransaction
                    {
                        RequestId = (int)requestId,
                        TransactionId = trans.Id,
                        LicenceId = model.LicId,
                        SocialMediaType = (int)SocialMediaEnum.Twitter,
                        SocialMediaRequestType = SocialMediaEnum.Twitter.ToString(),
                        NewAccountSocial_Media = model.NewTwitter,
                        OldAccountSocial_MediaName = OldSocialMedia.Where(o => o.SocialType == (int)SocialMediaEnum.Twitter).Select
                        (o => o.AccountSocial).FirstOrDefault(),
                        RequestDate = DateTime.Now,
                        Status = true
                    });
                }

                if (!string.IsNullOrEmpty(model.NewInsta))
                {
                    socialMediaList.Add(new ChangeSocialMediaTransaction
                    {
                        RequestId = (int)requestId,
                        TransactionId = trans.Id,
                        LicenceId = model.LicId,
                        SocialMediaType = (int)SocialMediaEnum.instegram,
                        SocialMediaRequestType = SocialMediaEnum.instegram.ToString(),
                        NewAccountSocial_Media = model.NewInsta,
                        OldAccountSocial_MediaName = OldSocialMedia.Where(o => o.SocialType == (int)SocialMediaEnum.instegram).Select
                        (o => o.AccountSocial).FirstOrDefault(),
                        RequestDate = DateTime.Now,
                        Status = true
                    });
                }

                if (!string.IsNullOrEmpty(model.NewWebSite))
                {
                    socialMediaList.Add(new ChangeSocialMediaTransaction
                    {
                        RequestId = (int)requestId,
                        TransactionId = trans.Id,
                        LicenceId = model.LicId,
                        SocialMediaType = (int)SocialMediaEnum.Website,
                        SocialMediaRequestType = SocialMediaEnum.Website.ToString(),
                        NewAccountSocial_Media = model.NewWebSite,
                        OldAccountSocial_MediaName = OldSocialMedia.Where(o => o.SocialType == (int)SocialMediaEnum.Website).Select
                        (o => o.AccountSocial).FirstOrDefault(),
                        RequestDate = DateTime.Now,
                        Status = true
                    });
                }
                if (!string.IsNullOrEmpty(model.NewFacebook))
                {
                    socialMediaList.Add(new ChangeSocialMediaTransaction
                    {
                        RequestId = (int)requestId,
                        TransactionId = trans.Id,
                        LicenceId = model.LicId,
                        SocialMediaType = (int)SocialMediaEnum.Facebook,
                        SocialMediaRequestType = SocialMediaEnum.Facebook.ToString(),
                        NewAccountSocial_Media = model.NewFacebook,
                        OldAccountSocial_MediaName = OldSocialMedia.Where(o => o.SocialType == (int)SocialMediaEnum.Facebook).Select
                        (o => o.AccountSocial).FirstOrDefault(),
                        RequestDate = DateTime.Now,
                        Status = true
                    });
                }
                // Save all
                foreach (var sm in socialMediaList)
                {
                    await _unitOfwork.genericRepository<ChangeSocialMediaTransaction>().Create(sm);
                }
                await _unitOfwork.Complete();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HandleChangeLicenceName: " + ex.Message);
                throw; // rethrow or log more deeply
            }
        }
        private async Task HandleChangePartnerName(PostRequestApiModel model, long requestId)
        {
            try
            {
                var trans = new Domain.Entities.Transaction
                {
                    RequestId = requestId,
                    Commited = false,
                    LicenseId = model.LicId,
                    ReqStatusId = (int)RequestStatusEnum.Received,
                    RequestDate = DateTime.Now,
                    TransTypeId = (int)TransactionTypesEnum.ChangePartnerName,
                    UsercivilId = model.SessionCivilId,
                    TransDate = DateTime.Now,
                    ServiceId = (int)ServiceEnum.Elaw,
                    Notes = "تغيير أسماء الشركاء"
                };
                await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(trans);
                await _unitOfwork.Complete();
                var OldPartners = await _unitOfwork.genericRepository<Partner>()
                                  .GetByCondition(s => s.LicenseId == model.LicId).ToListAsync();


                foreach (var old in OldPartners)
                {
                    var oldChange = new PartnerOldChangeTransaction
                    {
                        TransactionId = trans.Id,
                        ServiceId = (int)ServiceEnum.Elaw,
                        OldPartner = old.Name,
                        LastUpdateDate = DateTime.Now,
                        LastUpdateUser = model.SessionCivilId,
                        PartId = old.Id,
                        RequestId = requestId,
                        LicencesId = model.LicId
                    };
                    await _unitOfwork.genericRepository<PartnerOldChangeTransaction>().Create(oldChange);
                    await _unitOfwork.Complete();

                }

                // Map new partners (assuming from NewPartner1..5)
                var newPartners = new List<string?>
    {
        model.NewPartner1,
        model.NewPartner2,
        model.NewPartner3,
        model.NewPartner4,
        model.NewPartner5
    };

                foreach (var partnerName in newPartners.Where(p => !string.IsNullOrWhiteSpace(p)))
                {
                    var newChange = new PartnerNewChangeTransaction
                    {
                        TransactionId = trans.Id,
                        ServiceId = (int)ServiceEnum.Elaw,
                        NewPartner = partnerName,
                        LastUpdateDate = DateTime.Now,
                        LastUpdateUser = model.SessionCivilId,
                        RequestId = requestId,
                        LicencesId = model.LicId
                    };
                    await _unitOfwork.genericRepository<PartnerNewChangeTransaction>().Create(newChange);
                    await _unitOfwork.Complete();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HandleChangeLicenceName: " + ex.Message);
                throw; // rethrow or log more deeply
            }
        }

        private async Task HandleChangeEmail(PostRequestApiModel model, long requestId)
        {
            try
            {
                var trans = new Domain.Entities.Transaction
                {
                    RequestId = requestId,
                    Commited = false,
                    LicenseId = model.LicId,
                    ReqStatusId = (int)RequestStatusEnum.Received,
                    RequestDate = DateTime.Now,
                    TransTypeId = (int)TransactionTypesEnum.ChangeEmail,
                    UsercivilId = model.SessionCivilId,
                    TransDate = DateTime.Now,
                    ServiceId = (int)ServiceEnum.Elaw,
                    Notes = "تغيير البريد الإلكتروني"
                };
                await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(trans);
                await _unitOfwork.Complete();

                await _unitOfwork.genericRepository<ChangeEmailTranaction>().Create(new ChangeEmailTranaction
                {
                    RequestId = (int)requestId,
                    TransactionId = trans.Id,
                    RequestDate = DateTime.Now,
                    OldOwnerEmail = model.OldEmailApplicant,
                    NewOwnerEmail = model.NewEmailApplicant,
                    OldManagerEmail = model.OldEmailManager,
                    NewmanagerEmail = model.NewEmailManager,
                    Status = true,
                    LicenceId = model.LicId

                });
                await _unitOfwork.Complete();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HandleChangeLicenceName: " + ex.Message);
                throw; // rethrow or log more deeply
            }
        }

        private async Task HandleChangeLicType(PostRequestApiModel model, long requestId)
        {
            try
            {
                var licen = await _unitOfwork.genericRepository<Licence>().GetByCondition(l => l.LicId == model.LicId)
                    .FirstOrDefaultAsync();

                var trans = new Domain.Entities.Transaction
                {
                    RequestId = requestId,
                    Commited = false,
                    LicenseId = model.LicId,
                    ReqStatusId = (int)RequestStatusEnum.Received,
                    RequestDate = DateTime.Now,
                    TransTypeId = (int)TransactionTypesEnum.ChangeLicencesType,
                    UsercivilId = model.SessionCivilId,
                    TransDate = DateTime.Now,
                    ServiceId = (int)ServiceEnum.Elaw,
                    Notes = "تغيير نوع الترخيص"
                };
                await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(trans);
                await _unitOfwork.Complete();

                await _unitOfwork.genericRepository<LicenseTypeChangeTransaction>().Create(new LicenseTypeChangeTransaction
                {
                    Requestid = requestId,
                    TransactionId = trans.Id,
                    LicenceId = model.LicId,
                    LicenseNo = licen.LicNo,

                    LicTypeOldId = model.OldLicencesTpeId,
                    LicTypeNewId = model.NewLicencesTpeId,
                    LicTypeOld = await _unitOfwork.genericRepository<LicenceTypesLookup>()
                    .GetByCondition(l => l.Id == model.OldLicencesTpeId).Select(l => l.NameAr).FirstOrDefaultAsync(),
                    LicTypeNew = await _unitOfwork.genericRepository<LicenceTypesLookup>()
                    .GetByCondition(l => l.Id == model.NewLicencesTpeId).Select(l => l.NameAr).FirstOrDefaultAsync(),
                    ServiceId = (int)ServiceEnum.Elaw,
                    LastUpdateDate = DateTime.Now
                });
                await _unitOfwork.Complete();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HandleChangeLicenceName: " + ex.Message);
                throw; // rethrow or log more deeply
            }
        }

        #endregion
    }
}
