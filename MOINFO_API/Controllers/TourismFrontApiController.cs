using AutoMapper;
using Business.Enums;
using Business.Helpers;
using Business.Interfaces;
using Business.ModelWithSpecification;
using Business.Repository;
using Business.ViewModel;
using Business.ViewModel.ClassificationVM;
using Business.ViewModel.Dynamic;
using Business.ViewModel.HomePage;
using Business.ViewModel.Tourism;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using static Azure.Core.HttpHeader;

namespace MOINFO_API.Controllers
{
    [Route("api/TourismFront")]
    public class TourismFrontApiController : BaseController
    {
        private readonly IUnitOfwork _unitOfwork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly GenerateLicNo _generateLicNo;
        private readonly EmailService _emailService;
        private readonly IDataFetchService _dataFetchService;
        private readonly IUpdateDataService _updateDataService;
        private readonly ILogger<TourismFrontApiController> _logger;


        public TourismFrontApiController(IUnitOfwork unitOfwork, IConfiguration configuration
            , IMapper mapper, GenerateLicNo generateLicNo, ILogger<TourismFrontApiController> logger, EmailService emailService, IDataFetchService dataFetchService, IUpdateDataService updateDataService)
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

        [HttpGet]
        [Route("Licenses/GetUserDetails/{CivilID}")]
        public async Task<dynamic> GetUserDetails(string CivilID)
        {
            try
            {
                var UserDetails = _unitOfwork.genericRepository<AspNetUser>().GetByCondition(a => a.CivilId == CivilID).Select(a => new AspnetUserVM
                {
                    Id = a.Id,
                    UserName = a.UserName,
                    Mobile = a.Mobile,
                    Email = a.Email,
                    CivilId = a.CivilId,
                    AccountTypeId = a.AccountTypeId,
                }).FirstOrDefault();
                if (UserDetails != null)
                {

                    return UserDetails;
                }
                else
                {
                    return new ErrorMessage()
                    {
                        Error = true,
                        Status = "Failure",
                        Message = "No data Found",
                    };
                }

            }
            catch (Exception ex)
            {

                LogManager.Instance.AddErrorLog(ex);
                return new ErrorMessage()
                {
                    Error = true,
                    Status = "Failure",
                    Message = ex.Message,
                };
            }
        }


        [HttpGet]
        [Route("GetActivitiesForPreApproval")]
        public async Task<dynamic> GetActivitiesForPreApproval()
        {
            try
            {
                var ActivitiesList = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                    .GetByCondition(a => a.Id != 19 && a.Id != 20 && a.ServiceId == (int)ServiceEnum.Tourism).ToListAsync();
                if (ActivitiesList != null)
                {

                    return ActivitiesList;
                    // Convert the list to a SelectList for the dropdown
                    //var selectList = new SelectList(ActivitiesList, "Id", "ActivityTypeName");

                    //return selectList;
                }
                else
                {
                    return new ErrorMessage()
                    {
                        Error = true,
                        Status = "Failure",
                        Message = "No data Found",
                    };
                }

            }
            catch (Exception ex)
            {

                LogManager.Instance.AddErrorLog(ex);
                return new ErrorMessage()
                {
                    Error = true,
                    Status = "Failure",
                    Message = ex.Message,
                };
            }
        }
        [HttpGet]
        [Route("GetActivitiesForPreApproval/{id}")]
        public async Task<dynamic> GetActivitiesForPreApproval(int id)
        {
            try
            {
                var ActivitiesList = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                    .GetByCondition(a => a.Id != 19 && a.Id != 20 && a.ServiceId == (int)ServiceEnum.Tourism && a.Id == id).ToListAsync();
                if (ActivitiesList != null)
                {

                    return ActivitiesList;
                    // Convert the list to a SelectList for the dropdown
                    //var selectList = new SelectList(ActivitiesList, "Id", "ActivityTypeName");

                    //return selectList;
                }
                else
                {
                    return new ErrorMessage()
                    {
                        Error = true,
                        Status = "Failure",
                        Message = "No data Found",
                    };
                }

            }
            catch (Exception ex)
            {

                LogManager.Instance.AddErrorLog(ex);
                return new ErrorMessage()
                {
                    Error = true,
                    Status = "Failure",
                    Message = ex.Message,
                };
            }
        }
        [HttpGet]
        [Route("Licenses/GetActivitiesAll")]
        public async Task<dynamic> GetActivitiesAll()
        {
            try
            {
                var ActivitiesList = _unitOfwork.genericRepository<ActivityTypesLookup>().GetByCondition(a => a.ServiceId == (int)ServiceEnum.Tourism);
                if (ActivitiesList != null)
                {

                    return ActivitiesList;
                }
                else
                {
                    return new ErrorMessage()
                    {
                        Error = true,
                        Status = "Failure",
                        Message = "No data Found",
                    };
                }

            }
            catch (Exception ex)
            {

                LogManager.Instance.AddErrorLog(ex);
                return new ErrorMessage()
                {
                    Error = true,
                    Status = "Failure",
                    Message = ex.Message,
                };
            }
        }
       
        [HttpGet]
        [Route("GetFilesForPreApproval")]
        public async Task<IEnumerable<FileUploadConfigVM>> GetFilesForPreApproval()
        {
            var files = await _unitOfwork.genericRepository<FileUploadConfigurationsFront>().GetByCondition(f => f.ViewType == "PreAprroval").ToListAsync();
            return _mapper.Map<IEnumerable<FileUploadConfigurationsFront>, IEnumerable<FileUploadConfigVM>>(files);
        }
        [HttpGet]
        [Route("GetServiceDetails")]
        public async Task<dynamic> GetServiceDetails(int LicId,int ReqType)
        {
            var licen = await _unitOfwork.genericRepository<Licence>().GetbyId(LicId);
            var licenceInfo = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                .GetByCondition(l => l.ReqTypeId == ReqType
                && l.LicTypeId == licen.LicTypeId
                && l.ServiceId == licen.ServiceId
                && l.ActvityTypeId == licen.ActiivityTypeId).FirstOrDefaultAsync();


            var vm = new LicencesInfoVM
            {
                Id = licenceInfo.Id,
                ActvityTypeId = licenceInfo.ActvityTypeId,
                ReqTypeId = licenceInfo.ReqTypeId,
                LicTypeId = licenceInfo.LicTypeId,
                ServiceId = licenceInfo.ServiceId,
                EserviceTypeBranchId = licenceInfo.EserviceTypeBranchId,
                Name = licenceInfo.Name,
                Description = licenceInfo.Description,
                Conditions = licenceInfo.Conditions,
                RequiredDocuments = licenceInfo.RequiredDocuments,
                Measures = licenceInfo.Measures,
                VariableFees = licenceInfo.VariableFees,
                FixedFees = licenceInfo.FixedFees,
                Status = licenceInfo.Status,
                Sort = licenceInfo.Sort,
                Branch = licenceInfo.Branch,
                Controller = licenceInfo.Controller,
                Action = licenceInfo.Action,
                Url = licenceInfo.Url,
                LicId = LicId, // Inject the passed LicId here
               
            };

            return Ok(vm);
        }
        [HttpGet]
        [Route("Licenses/GetActivityName/{Activityid}")]
        public async Task<IActionResult> GetActivityCodeName(int ActivityId)
        {
            try
            {
                var activity = await _unitOfwork.genericRepository<ActivityTypesLookup>()
               .GetByCondition(a => a.Id == ActivityId)
               .Select(a => new
               {
                   a.Id,
                   a.ActivityCode,
                   a.NameAr,
                   a.NameEn
               })
               .FirstOrDefaultAsync();

                if (activity == null)
                {
                    return NotFound(new { message = $"No activity found with ActivityId '{ActivityId}'." });
                }

                return Ok(activity);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"An error occurred: {ex.Message}");

                // Return error response
                return StatusCode(500, new { error = "An error occurred while fetching activity details." });
            }
        }

        [HttpGet]
        [Route("GetActivityWithService")]
        public async Task<TourLicActivityVm> GetActivityWithService(int id)
        {
            var licencesInfo = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                .GetByCondition(l => l.Id == id).FirstOrDefaultAsync();
            var Activity = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                         .GetByCondition(x => x.Id == licencesInfo.ActvityTypeId).FirstOrDefaultAsync();
            var Service = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                  .GetByCondition(x => x.ActvityTypeId == licencesInfo.ActvityTypeId && x.ServiceId == (int)ServiceEnum.Tourism && x.ReqTypeId == licencesInfo.ReqTypeId).FirstOrDefaultAsync();

            var FileConfig = new List<AttachRule>();


            if (licencesInfo.ActvityTypeId == (int)ActivityTypeEnum.Parks)
            {
                FileConfig = await _unitOfwork.genericRepository<AttachRule>()
                          .GetByCondition(f => f.ViewType == "RequestParks").ToListAsync();

            }
            else if (licencesInfo.ActvityTypeId == (int)ActivityTypeEnum.Sailing)
            {
                FileConfig = await _unitOfwork.genericRepository<AttachRule>()
                          .GetByCondition(f => f.ViewType == "RequestSailing").ToListAsync();
            }

            return new TourLicActivityVm
            {
                ActivityType = _mapper.Map<ActivityTypesLookup, ActivityTypeVM>(Activity),
                LicencesInfo = _mapper.Map<MoiEserviceLicenseInfo, LicencesInfoVM>(licencesInfo),
                fileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddWorkflowWithAttachmentsVM>>(FileConfig)
            };
        }
        [HttpGet]
        [Route("GetPreApproveRequest")]
        public async Task<PreApprovalRequestModel> GetPreApproveRequest(int licencesInfoId)
        {
            var GetlicencesInfo = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>().GetbyId(licencesInfoId);
            var Activity = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                         .GetByCondition(x => x.Id == GetlicencesInfo.ActvityTypeId).FirstOrDefaultAsync();
            var activitySelectList = new List<SelectListItem>
                                            {
                                                new SelectListItem
                                                {
                                                    Value = Activity?.Id.ToString(),
                                                    Text = Activity?.NameAr,
                                                    Selected = true
                                                }
                                            };

            //var Service = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
            //      .GetByCondition(x => x.ActvityTypeId == GetlicencesInfo.ActvityTypeId && x.ServiceId == (int)ServiceEnum.Tourism &&( x.ReqTypeId == (int)RequestTypeEnum.PreApprovementConvert|| x.ReqTypeId == (int)RequestTypeEnum.PreApprovementNew)).FirstOrDefaultAsync();

            var FileConfig = new List<AttachRule>();


            //if (activityId == (int)ActivityTypeEnum.Hotel)
            //{
            //    FileConfig = await _unitOfwork.genericRepository<FileUploadConfigurationsFront>()
            //              .GetByCondition(f => f.ViewType == "RequestParks").ToListAsync();

            //}
            //else if (activityId == (int)ActivityTypeEnum.ApartmentHotel)
            //{
            //    FileConfig = await _unitOfwork.genericRepository<FileUploadConfigurationsFront>()
            //              .GetByCondition(f => f.ViewType == "RequestSailing").ToListAsync();
            //}
            //else if (activityId == (int)ActivityTypeEnum.Resorts)
            //{
            //    FileConfig = await _unitOfwork.genericRepository<FileUploadConfigurationsFront>()
            //              .GetByCondition(f => f.ViewType == "RequestSailing").ToListAsync();
            //}
            FileConfig = await _unitOfwork.genericRepository<AttachRule>()
                          .GetByCondition(f => f.ViewType == "PreAprroval").ToListAsync();
            return new PreApprovalRequestModel
            {
                Activities = activitySelectList,
                ActivityName = Activity.NameAr,
                ActivityCode = Activity.ActivityCode,
                ActivityTypeId = GetlicencesInfo.ActvityTypeId,
                LicencesInfoVM = _mapper.Map<MoiEserviceLicenseInfo, LicencesInfoVM>(GetlicencesInfo),
                fileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(FileConfig)
            };
        }
        [HttpPost]
        [Route("PreApprovalRequest")]
        public async Task<dynamic> PreApprovalRequest(PreApprovalRequestApiModel PreApprovalRequestModel)
        {
            string error = string.Empty;
            try
            {

                //--------------- Start trunsaction -----------------------------------
                using (IDbContextTransaction dbTransaction = _unitOfwork.BeginTransaction())
                {
                    try
                    {
                        ////------- insert in Company table ------------
                        //Company CompModel = new Company()
                        //{

                        //    CompanyCivilId = PreApprovalRequestModel.CompanyCivilId,
                        //    DirCompanyAr = PreApprovalRequestModel.DirCompanyAr,
                        //    OwnerCompanyAr = PreApprovalRequestModel.OwnerCompanyAr,
                        //    CommercialLicNo = PreApprovalRequestModel.CommercialLicNo,
                        //    RecordNo = PreApprovalRequestModel.RecordNo,
                        //    IsBuilding = false,
                        //    ActivityCode = PreApprovalRequestModel.ActivityCode,
                        //    CompanyActivity = PreApprovalRequestModel.CompanyActivity,
                        //    ServiceId = (int)ServiceEnum.Tourism,
                        //    OwnerName = PreApprovalRequestModel.UserName,
                        //    ActivityTypeId = PreApprovalRequestModel.ActivityTypeId,


                        //};
                        //var _mappedCompany = _mapper.Map<Company>(CompModel);

                        //await _unitOfwork.genericRepository<Company>().Create(CompModel);
                        //await _unitOfwork.Complete();

                        Address addressModel = new Address()
                        {
                            AreaChartNo = PreApprovalRequestModel.AreaChartNo,
                            AreaSize = PreApprovalRequestModel.AreaSize,
                            ServiceId = (int)ServiceEnum.Tourism,
                            AalliNo = PreApprovalRequestModel.AaliNumber,

                            GovernorateArabic = PreApprovalRequestModel.Governrate,
                            Area = PreApprovalRequestModel.Area,
                            BlockArabic = PreApprovalRequestModel.BlockNo,
                            StreetArabic = PreApprovalRequestModel.Street,
                            BuildingNo = PreApprovalRequestModel.BuildingNo,
                            BuildingName = PreApprovalRequestModel.BuildingName,
                            ActivityTypeId = PreApprovalRequestModel.ActivityTypeId,
                            ActivityCode = PreApprovalRequestModel.ActivityCode,
                            FloorNo = PreApprovalRequestModel.FloorNo,
                            UnitNo = PreApprovalRequestModel.UnitNo

                        };

                        await _unitOfwork.genericRepository<Address>().Create(addressModel);
                        await _unitOfwork.Complete();

                        int addressId = addressModel.Id;

                        //------- insert in MOI_EService_TourismBuilding table ------------
                        Company BuildingModel = new Company()
                        {

                            Name = PreApprovalRequestModel.OwnerCompanyAr,
                            ActivityTypeId = PreApprovalRequestModel.ActivityTypeId,
                            ActivityCode = PreApprovalRequestModel.ActivityCode,
                            AddressId = addressId,
                            CompanyCivilId = PreApprovalRequestModel.CompanyCivilId,
                            IsBuilding = true,
                            ServiceId = (int)ServiceEnum.Tourism,
                            CommercialLicNo = PreApprovalRequestModel.CommercialLicNo,
                            DirCompanyAr = PreApprovalRequestModel.DirCompanyAr,
                            OwnerCompanyAr = PreApprovalRequestModel.OwnerCompanyAr,
                            RecordNo = PreApprovalRequestModel.RecordNo,
                            OwnerName = PreApprovalRequestModel.OwnerCompanyAr,
                            CompanyActivity=await _unitOfwork.genericRepository<ActivityTypesLookup>()
                                    .GetByCondition(a=>a.Id== PreApprovalRequestModel.ActivityTypeId).Select(c=>c.NameAr).FirstOrDefaultAsync()

                        };

                        var _mappedBuilding = _mapper.Map<Company>(BuildingModel);


                        await _unitOfwork.genericRepository<Company>().Create(BuildingModel);
                        await _unitOfwork.Complete();
                        //string RequesterId = PreApprovalRequestModel.SessionCivilId;
                        var RequesterId = await _unitOfwork.genericRepository<AspNetUser>()
                            .GetByCondition(a => a.CivilId == PreApprovalRequestModel.SessionCivilId).FirstOrDefaultAsync();
                        #region Applicant
                        int ApplicantId;

                        var ApplicantExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == PreApprovalRequestModel.AppCivilId)
                            .FirstOrDefaultAsync();

                        if (ApplicantExist == null)
                        {
                            var aspnetuser = await _unitOfwork.genericRepository<AspNetUser>()
                                .GetByCondition(a => a.CivilId == PreApprovalRequestModel.AppCivilId)
                                .FirstOrDefaultAsync();

                            var newApplicant = new Person
                            {
                                Name1 = PreApprovalRequestModel.UserName,
                                Phone = aspnetuser?.Mobile,
                                Email = aspnetuser?.Email,
                                CivilId = PreApprovalRequestModel.AppCivilId,
                                ServiceId = (int)ServiceEnum.Tourism
                            };

                            await _unitOfwork.genericRepository<Person>().Create(newApplicant);
                            await _unitOfwork.Complete();

                            ApplicantId = newApplicant.Id; // ✅ Get Id after create
                        }
                        else
                        {
                            ApplicantId = ApplicantExist.Id; // ✅ Use existing Id
                        }
                        #endregion
                        #region Insert Manager
                        int ManagerId;

                        var managerExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == PreApprovalRequestModel.ManCivilId)
                            .FirstOrDefaultAsync();

                        if (managerExist == null)
                        {
                            var newManager = new Person
                            {
                                Name1 = PreApprovalRequestModel.ManagerName,
                                Phone = PreApprovalRequestModel.ManagerMobile,
                                Email = PreApprovalRequestModel.ManagerEmail,
                                ServiceId = (int)ServiceEnum.Tourism,
                                CivilId = PreApprovalRequestModel.ManCivilId
                            };

                            await _unitOfwork.genericRepository<Person>().Create(newManager);
                            await _unitOfwork.Complete();

                            ManagerId = newManager.Id; // ✅ Get Id after create
                        }
                        else
                        {
                            ManagerId = managerExist.Id; // ✅ Use existing Id
                        }
                        #endregion
                        #region Insert SalesManager
                        int SalesManagerId;

                        var SalesmanagerExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == PreApprovalRequestModel.SalesManagerCivilId)
                            .FirstOrDefaultAsync();

                        if (SalesmanagerExist == null)
                        {
                            var newSalesManager = new Person
                            {
                                Name1 = PreApprovalRequestModel.SalesManagerName,
                                Phone = PreApprovalRequestModel.SalesManagerPhone,
                                Email = PreApprovalRequestModel.SalesManagerEmail,
                                ServiceId = (int)ServiceEnum.Tourism,
                                CivilId = PreApprovalRequestModel.SalesManagerCivilId
                            };

                            await _unitOfwork.genericRepository<Person>().Create(newSalesManager);
                            await _unitOfwork.Complete();

                            SalesManagerId = newSalesManager.Id; // ✅ Get Id after create
                        }
                        else
                        {
                            SalesManagerId = SalesmanagerExist.Id; // ✅ Use existing Id
                        }
                        #endregion
                        #region Insert MarketingManager
                        int MarketingManagerId;

                        var MarketingmanagerExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == PreApprovalRequestModel.MarketingManagerCivilId)
                            .FirstOrDefaultAsync();

                        if (MarketingmanagerExist == null)
                        {
                            var newMarketingManager = new Person
                            {
                                Name1 = PreApprovalRequestModel.MarketingManagerName,
                                Phone = PreApprovalRequestModel.MarketingManagerPhone,
                                Email = PreApprovalRequestModel.MarketingManagerEmail,
                                ServiceId = (int)ServiceEnum.Tourism,
                                CivilId = PreApprovalRequestModel.MarketingManagerCivilId
                            };

                            await _unitOfwork.genericRepository<Person>().Create(newMarketingManager);
                            await _unitOfwork.Complete();

                            MarketingManagerId = newMarketingManager.Id; // ✅ Get Id after create
                        }
                        else
                        {
                            MarketingManagerId = MarketingmanagerExist.Id; // ✅ Use existing Id
                        }
                        #endregion
                        #region Insert OperationManager
                        int OperationManagerId;

                        var OperationmanagerExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == PreApprovalRequestModel.OperationManagerCivilId)
                            .FirstOrDefaultAsync();

                        if (OperationmanagerExist == null)
                        {
                            var newOperationManager = new Person
                            {
                                Name1 = PreApprovalRequestModel.OperationManagerName,
                                Phone = PreApprovalRequestModel.OperationManagerPhone,
                                Email = PreApprovalRequestModel.OperationManagerEmail,
                                ServiceId = (int)ServiceEnum.Tourism,
                                CivilId = PreApprovalRequestModel.OperationManagerCivilId
                            };

                            await _unitOfwork.genericRepository<Person>().Create(newOperationManager);
                            await _unitOfwork.Complete();

                            OperationManagerId = newOperationManager.Id; // ✅ Get Id after create
                        }
                        else
                        {
                            OperationManagerId = OperationmanagerExist.Id; // ✅ Use existing Id
                        }
                        #endregion
                        long SequenceNo = PreApprovalRequestModel.SequenceNo;

                        var ReqType = await _unitOfwork.genericRepository<RequestsTypesLookup>()
                                   .GetByCondition(r => r.Id == PreApprovalRequestModel.ReqtypeId).FirstOrDefaultAsync();
                        
                        #region Insert in Request Table
                        MoiEserviceLicensesRequest ReqModel = new MoiEserviceLicensesRequest()
                        {
                            AppId=ApplicantId,
                            
                            Reqno = PreApprovalRequestModel.reqno,
                            ReqtypeId = PreApprovalRequestModel.ReqtypeId,
                            Licno = null,
                            ActivityType = ReqType.NameAr,
                            ServiceId = (int)ServiceEnum.Tourism,
                            Licowner = PreApprovalRequestModel.OwnerCompanyAr,
                            Licname = PreApprovalRequestModel.LicencesName,
                            Licexpiredate = null,
                            SequenceNo = SequenceNo,
                            Licreqtime = DateTime.Now,
                            Requesterid = RequesterId.Id,
                            RequestNote = null,
                            RequestStatusId = (int)RequestStatusEnum.Received,
                            RequestAttach = "Yes",
                            LicenseId = null,
                            Licamount = 0,
                            Licpaystatus = "0",
                            CategoryId = 1,
                            SectorId = 3,
                            AppCivilId = PreApprovalRequestModel.AppCivilId,
                            ManCivilId = PreApprovalRequestModel.ManCivilId,
                            //CompanyId= CompModel.Id,
                            CompanyId= BuildingModel.Id,
                            //UserCivilId = PreApprovalRequestModel.UserCivilID,
                            RequesterCivilId =PreApprovalRequestModel.SessionCivilId,
                            // LicrequestLictypeId = 1,
                            LicStatusId = (int)licencesStatusEnum.Pending,
                            ActivityTypeId = PreApprovalRequestModel.ActivityTypeId,
                            LicrequestIsDeleted = false,
                            IsArchived = false,
                            LicTypeId = (int)LicTypeEnum.Company,
                            ActivityCode = PreApprovalRequestModel.ActivityCode,
                            SalesManagerCivilId=PreApprovalRequestModel.SalesManagerCivilId,
                            OperationsManagerCivilId=PreApprovalRequestModel.OperationManagerCivilId,
                            MarketingManagerCivilId=PreApprovalRequestModel.MarketingManagerCivilId,
                            SalesManagerId=SalesManagerId,
                            OperationsManagerId=OperationManagerId,
                            MarketingManagerId=MarketingManagerId,
                            ManagerId=ManagerId,

                        };

                        var _mappedRequest = _mapper.Map<MoiEserviceLicensesRequest>(ReqModel);

                        await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(ReqModel);
                        await _unitOfwork.Complete();

                        long reqid = _mappedRequest.RequestId;
                        #endregion

                        #region Insert In PreApprove Table 
                        MoiPreApprovement PreApprovModel = new MoiPreApprovement()
                        {
                            CompanyId = BuildingModel.Id,
                            //CompanyId = CompModel.Id,
                            
                            ManagerId = ManagerId,
                            AppId= ApplicantId,
                           
                            IsConsumed = false,
                            ReqTypeId= PreApprovalRequestModel.ReqtypeId,
                            RequestId = Convert.ToInt32(reqid),
                            LicTypeId = (int)LicTypeEnum.Company,
                            ClassificationName = null,
                            Flag = "موافقة مبدئية غير مستخدمة",
                            ComIssuingDate = null,
                            ComExpiryDate = null,
                            ManagerCivilId = PreApprovalRequestModel.ManCivilId,
                            ActivityTypeId = PreApprovalRequestModel.ActivityTypeId,
                            LicStatusId = (int)licencesStatusEnum.Pending,
                            ApplicantCivilId = PreApprovalRequestModel.AppCivilId,
                            ReqStatusId = (int)RequestStatusEnum.Received,
                            //UserCivilId = PreApprovalRequestModel.UserCivilID,
                            CommercialLicNo = PreApprovalRequestModel.CommercialLicNo,
                            RecordNo = PreApprovalRequestModel.RecordNo,
                            //ClassificationId=(int)ClassificationEnum.
                            LicenseName = PreApprovalRequestModel.LicencesName,
                            LicenseNo = null,
                            LicenseExpireDate = null,
                            LicenseIssueDate = null,
                            SalesManagerCivilId= PreApprovalRequestModel.SalesManagerCivilId,
                            MarketingManagerCivilId= PreApprovalRequestModel.MarketingManagerCivilId,
                            OperationsManagerCivilId= PreApprovalRequestModel.OperationManagerCivilId,
                            SalesManagerId=SalesManagerId,
                            OperationsManagerId=OperationManagerId,
                            MarketingManagerId=MarketingManagerId,
                            
                        };

                        var _mappedPreApprov = _mapper.Map<MoiPreApprovement>(PreApprovModel);
                        await _unitOfwork.genericRepository<MoiPreApprovement>().Create(PreApprovModel);
                        await _unitOfwork.Complete();

                
                        #endregion
         
                        #region Update Request Table
                        MoiEserviceLicensesRequest UpdateReqModel = _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                     .GetByCondition(c => c.RequestId == Convert.ToInt32(reqid)).FirstOrDefault();
                        if (UpdateReqModel != null)
                        {
                            UpdateReqModel.PreApprovalId = PreApprovModel.PreAppId;

                        }

                        var _mappedUpdateRequest = _mapper.Map<MoiEserviceLicensesRequest>(UpdateReqModel);
                        await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Update(UpdateReqModel);
                        await _unitOfwork.Complete();
                        #endregion

                        RequestTransaction requestTransaction = new RequestTransaction()
                        {
                            ReqStatusId = (int)RequestStatusEnum.Received,
                            ReqTypeId = PreApprovalRequestModel.ReqtypeId,
                            
                            RequestId = reqid,
                            Notes = "إصدار موافقة مبدئية",
                            Status = RequestStatusEnum.Received.ToString(),
                            CreatedDate = DateTime.Now,
                            CreatedBy = PreApprovalRequestModel.SessionName,
                            CivilIdUser = PreApprovalRequestModel.SessionCivilId,
                            UpdatedDate = DateTime.Now
                        };
                        await _unitOfwork.genericRepository<RequestTransaction>().Create(requestTransaction);
                        await _unitOfwork.Complete();


                        //--------Insert InTable Attachment---------------

                        await InsertAttachements(PreApprovalRequestModel.saveResponseVMs, reqid, PreApprovalRequestModel.SessionCivilId);




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
        #region update PreApprove in request WHen the status is Received
        [HttpPost]
        [Route("UpdatePreApprovalDetails")]
        public async Task<dynamic> UpdatePreApprovalDetails(PreApprovalRequestApiModel model)
        {
            if (!ModelState.IsValid)
                return Ok(new ErrorMessage { Error = true, Message = "البيانات غير صالحة" });

            try
            {
                var req = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                    .GetByCondition(r => r.RequestId == model.RequestId)
                    .FirstOrDefaultAsync();

                if (req == null)
                    return Ok(new ErrorMessage { Error = true, Message = "الطلب غير موجود" });

               

                

                var company = await _unitOfwork.genericRepository<Company>()
                    .GetByCondition(c=>c.Id==model.CompanyId).FirstOrDefaultAsync();
                if (company != null)
                {
                    company.OwnerCompanyAr = model.OwnerCompanyAr;
                    company.DirCompanyAr = model.DirCompanyAr;
                    company.CompanyCivilId = model.CompanyCivilId;
                    //company.RecordNo = model.RecordNo;
                    //company.CommercialLicNo = model.CommercialLicNo;
                    await _unitOfwork.genericRepository<Company>().Update(company);
                }
                
                if (company?.AddressId != null)
                {
                    var address = await _unitOfwork.genericRepository<Address>()
                        .GetByCondition(a=>a.Id==company.AddressId).FirstOrDefaultAsync();

                    if (address != null)
                    {
                        address.AalliNo = model.AaliNumber;
                        address.Area = model.Area;
                        address.BlockArabic = model.BlockNo;
                        address.StreetArabic = model.Street;
                        address.BuildingName = model.BuildingName;
                        address.BuildingNo = model.BuildingNo;
                        address.AreaChartNo = model.AreaChartNo;
                        address.AreaSize = model.AreaSize;
                        address.GovernorateArabic = model.Governrate;
                        address.FloorNo = model.FloorNo;
                        address.UnitNo = model.UnitNo;

                        await _unitOfwork.genericRepository<Address>().Update(address);
                    }
                }

                #region Managers
                var manager = await _unitOfwork.genericRepository<Person>()
             .GetByCondition(p => p.CivilId == model.ManCivilId)
             .FirstOrDefaultAsync();
                int managerId = 0;
                if (manager == null)
                {
                    manager = new Person
                    {
                        Name1 = model.ManagerName,
                        Email = model.ManagerEmail,
                        Phone = model.ManagerMobile,
                        CivilId = model.ManCivilId,
                        ServiceId = (int)ServiceEnum.Tourism
                    };
                   
                    await _unitOfwork.genericRepository<Person>().Create(manager);
                    await _unitOfwork.Complete();
                    managerId = manager.Id;
                }
                else
                {
                    manager.Name1 = model.ManagerName;
                    manager.Email = model.ManagerEmail;
                    manager.Phone = model.ManagerMobile;
                    managerId= manager.Id;
                    await _unitOfwork.genericRepository<Person>().Update(manager);
                }

                // Sales Manager
                var salesManager = await _unitOfwork.genericRepository<Person>()
                    .GetByCondition(p => p.CivilId == model.SalesManagerCivilId)
                    .FirstOrDefaultAsync();
                int salesManagerId = 0;
                if (salesManager == null)
                {
                    salesManager = new Person
                    {
                        Name1 = model.SalesManagerName,
                        Email = model.SalesManagerEmail,
                        Phone = model.SalesManagerPhone,
                        CivilId = model.SalesManagerCivilId,
                        ServiceId = (int)ServiceEnum.Tourism
                    };
                    await _unitOfwork.genericRepository<Person>().Create(salesManager);
                    await _unitOfwork.Complete();
                    salesManagerId = salesManager.Id;   
                }
                else
                {
                    salesManager.Name1 = model.SalesManagerName;
                    salesManager.Email = model.SalesManagerEmail;
                    salesManager.Phone = model.SalesManagerPhone;
                    salesManagerId = salesManager.Id;
                    await _unitOfwork.genericRepository<Person>().Update(salesManager);
                }

                // Marketing Manager
                var marketingManager = await _unitOfwork.genericRepository<Person>()
                    .GetByCondition(p => p.CivilId == model.MarketingManagerCivilId)
                    .FirstOrDefaultAsync();
                int marketingManagerId = 0;
                if (marketingManager == null)
                {
                    marketingManager = new Person
                    {
                        Name1 = model.MarketingManagerName,
                        Email = model.MarketingManagerEmail,
                        Phone = model.MarketingManagerPhone,
                        CivilId = model.MarketingManagerCivilId,
                        ServiceId = (int)ServiceEnum.Tourism
                    };
                    await _unitOfwork.genericRepository<Person>().Create(marketingManager);
                    await _unitOfwork.Complete();
                    marketingManagerId=marketingManager.Id;
                }
                else
                {
                    marketingManager.Name1 = model.MarketingManagerName;
                    marketingManager.Email = model.MarketingManagerEmail;
                    marketingManager.Phone = model.MarketingManagerPhone;
                    marketingManagerId= marketingManager.Id;
                    await _unitOfwork.genericRepository<Person>().Update(marketingManager);
                }

                // Operation Manager
                var operationManager = await _unitOfwork.genericRepository<Person>()
                    .GetByCondition(p => p.CivilId == model.OperationManagerCivilId)
                    .FirstOrDefaultAsync();
                int OperatingManagerId = 0;
                if (operationManager == null)
                {
                    operationManager = new Person
                    {
                        Name1 = model.OperationManagerName,
                        Email = model.OperationManagerEmail,
                        Phone = model.OperationManagerPhone,
                        CivilId = model.OperationManagerCivilId,
                        ServiceId = (int)ServiceEnum.Tourism
                    };
                    await _unitOfwork.genericRepository<Person>().Create(operationManager);
                    await _unitOfwork.Complete();
                    OperatingManagerId= operationManager.Id;
                }
                else
                {
                    operationManager.Name1 = model.OperationManagerName;
                    operationManager.Email = model.OperationManagerEmail;
                    operationManager.Phone = model.OperationManagerPhone;
                    OperatingManagerId=operationManager.Id;
                    await _unitOfwork.genericRepository<Person>().Update(operationManager);
                }

                #endregion
                var preApp = await _unitOfwork.genericRepository<MoiPreApprovement>()
                    .GetByCondition(p => p.PreAppId == model.PreApproveId)
                    .FirstOrDefaultAsync();
                if (preApp != null)
                {
                    preApp.LicenseName = model.LicencesName;

                    preApp.ManagerCivilId = model.ManCivilId;
                    preApp.ApplicantCivilId = model.AppCivilId;
                    preApp.SalesManagerCivilId = model.SalesManagerCivilId;
                    preApp.OperationsManagerCivilId = model.OperationManagerCivilId;
                    preApp.MarketingManagerCivilId = model.MarketingManagerCivilId;
                    preApp.ManagerId = managerId;
                    preApp.SalesManagerId = salesManagerId;
                    preApp.OperationsManagerId = OperatingManagerId;
                    preApp.MarketingManagerId = OperatingManagerId; 
                    await _unitOfwork.genericRepository<MoiPreApprovement>().Update(preApp);
                }
                req.Licowner = model.OwnerCompanyAr;
                req.ManCivilId = model.ManCivilId;
                req.Licname = model.LicencesName;
                req.AppCivilId = model.AppCivilId;
                req.SalesManagerCivilId = model.SalesManagerCivilId;
                req.OperationsManagerCivilId = model.OperationManagerCivilId;
                req.MarketingManagerCivilId = model.MarketingManagerCivilId;
                req.ManagerId = managerId;
                req.SalesManagerId = salesManagerId;
                req.OperationsManagerId = OperatingManagerId;
                req.MarketingManagerId = OperatingManagerId;
                req.RequestModDate = DateTime.Now;
                await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Update(req);
                await _unitOfwork.Complete();

                RequestTransaction requestTransaction = new RequestTransaction()
                {
                    ReqStatusId = (int)RequestStatusEnum.Received,
                    ReqTypeId = req.ReqtypeId,
                    RequestId = req.RequestId,
                    Notes = "تحديث موافقة مبدئية",
                    Status = RequestStatusEnum.Received.ToString(),
                    CreatedDate = DateTime.Now,
                    CreatedBy = model.SessionName,
                    CivilIdUser = model.SessionCivilId,
                    UpdatedDate = DateTime.Now
                };

                await _unitOfwork.genericRepository<RequestTransaction>().Create(requestTransaction);
                await _unitOfwork.Complete();

                return Ok(new ErrorMessage { Error = false, Message = "تم تحديث البيانات بنجاح" });
            }
            catch (Exception ex)
            {
                return Ok(new ErrorMessage { Error = true, Message = ex.Message });
            }
        }

        #endregion
        #region Update Requestdetails For All Licences
        [HttpPost]
        [Route("UpdateLicRequestDetails")]
        public async Task<dynamic> UpdateLicRequestDetails(PreApprovalRequestApiModel model)
        {
            if (!ModelState.IsValid)
                return Ok(new ErrorMessage { Error = true, Message = "البيانات غير صالحة" });

            try
            {
                var req = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                    .GetByCondition(r => r.RequestId == model.RequestId)
                    .FirstOrDefaultAsync();

                if (req == null)
                    return Ok(new ErrorMessage { Error = true, Message = "الطلب غير موجود" });





                var company = await _unitOfwork.genericRepository<Company>()
                    .GetByCondition(c => c.Id == model.CompanyId).FirstOrDefaultAsync();
                if (company != null)
                {
                    company.OwnerCompanyAr = model.OwnerCompanyAr;
                    company.DirCompanyAr = model.DirCompanyAr;
                    company.CompanyCivilId = model.CompanyCivilId;
                    company.RecordNo = model.RecordNo;
                    company.CommercialLicNo = model.CommercialLicNo;
                    await _unitOfwork.genericRepository<Company>().Update(company);
                }

                if (company?.AddressId != null)
                {
                    var address = await _unitOfwork.genericRepository<Address>()
                        .GetByCondition(a => a.Id == company.AddressId).FirstOrDefaultAsync();

                    if (address != null)
                    {
                        address.AalliNo = model.AaliNumber;
                        address.Area = model.Area;
                        address.BlockArabic = model.BlockNo;
                        address.StreetArabic = model.Street;
                        address.BuildingName = model.BuildingName;
                        address.BuildingNo = model.BuildingNo;
                        address.AreaChartNo = model.AreaChartNo;
                        address.AreaSize = model.AreaSize;
                        address.GovernorateArabic = model.Governrate;
                        address.FloorNo = model.FloorNo;
                        address.UnitNo = model.UnitNo;

                        await _unitOfwork.genericRepository<Address>().Update(address);
                    }
                }

                #region Managers
                var manager = await _unitOfwork.genericRepository<Person>()
             .GetByCondition(p => p.CivilId == model.ManCivilId)
             .FirstOrDefaultAsync();
                int managerId = 0;
                if (manager == null)
                {
                    manager = new Person
                    {
                        Name1 = model.ManagerName,
                        Email = model.ManagerEmail,
                        Phone = model.ManagerMobile,
                        CivilId = model.ManCivilId,
                        ServiceId = (int)ServiceEnum.Tourism
                    };

                    await _unitOfwork.genericRepository<Person>().Create(manager);
                    await _unitOfwork.Complete();
                    managerId = manager.Id;
                }
                else
                {
                    manager.Name1 = model.ManagerName;
                    manager.Email = model.ManagerEmail;
                    manager.Phone = model.ManagerMobile;
                    managerId = manager.Id;
                    await _unitOfwork.genericRepository<Person>().Update(manager);
                }

                // Sales Manager
                var salesManager = await _unitOfwork.genericRepository<Person>()
                    .GetByCondition(p => p.CivilId == model.SalesManagerCivilId)
                    .FirstOrDefaultAsync();
                int salesManagerId = 0;
                if (salesManager == null)
                {
                    salesManager = new Person
                    {
                        Name1 = model.SalesManagerName,
                        Email = model.SalesManagerEmail,
                        Phone = model.SalesManagerPhone,
                        CivilId = model.SalesManagerCivilId,
                        ServiceId = (int)ServiceEnum.Tourism
                    };
                    await _unitOfwork.genericRepository<Person>().Create(salesManager);
                    await _unitOfwork.Complete();
                    salesManagerId = salesManager.Id;
                }
                else
                {
                    salesManager.Name1 = model.SalesManagerName;
                    salesManager.Email = model.SalesManagerEmail;
                    salesManager.Phone = model.SalesManagerPhone;
                    salesManagerId = salesManager.Id;
                    await _unitOfwork.genericRepository<Person>().Update(salesManager);
                }

                // Marketing Manager
                var marketingManager = await _unitOfwork.genericRepository<Person>()
                    .GetByCondition(p => p.CivilId == model.MarketingManagerCivilId)
                    .FirstOrDefaultAsync();
                int marketingManagerId = 0;
                if (marketingManager == null)
                {
                    marketingManager = new Person
                    {
                        Name1 = model.MarketingManagerName,
                        Email = model.MarketingManagerEmail,
                        Phone = model.MarketingManagerPhone,
                        CivilId = model.MarketingManagerCivilId,
                        ServiceId = (int)ServiceEnum.Tourism
                    };
                    await _unitOfwork.genericRepository<Person>().Create(marketingManager);
                    await _unitOfwork.Complete();
                    marketingManagerId = marketingManager.Id;
                }
                else
                {
                    marketingManager.Name1 = model.MarketingManagerName;
                    marketingManager.Email = model.MarketingManagerEmail;
                    marketingManager.Phone = model.MarketingManagerPhone;
                    marketingManagerId = marketingManager.Id;
                    await _unitOfwork.genericRepository<Person>().Update(marketingManager);
                }

                // Operation Manager
                var operationManager = await _unitOfwork.genericRepository<Person>()
                    .GetByCondition(p => p.CivilId == model.OperationManagerCivilId)
                    .FirstOrDefaultAsync();
                int OperatingManagerId = 0;
                if (operationManager == null)
                {
                    operationManager = new Person
                    {
                        Name1 = model.OperationManagerName,
                        Email = model.OperationManagerEmail,
                        Phone = model.OperationManagerPhone,
                        CivilId = model.OperationManagerCivilId,
                        ServiceId = (int)ServiceEnum.Tourism
                    };
                    await _unitOfwork.genericRepository<Person>().Create(operationManager);
                    await _unitOfwork.Complete();
                    OperatingManagerId = operationManager.Id;
                }
                else
                {
                    operationManager.Name1 = model.OperationManagerName;
                    operationManager.Email = model.OperationManagerEmail;
                    operationManager.Phone = model.OperationManagerPhone;
                    OperatingManagerId = operationManager.Id;
                    await _unitOfwork.genericRepository<Person>().Update(operationManager);
                }

                #endregion
                var Licence = await _unitOfwork.genericRepository<Licence>()
                    .GetByCondition(p => p.LicId == model.LicId)
                    .FirstOrDefaultAsync();
                if (Licence != null)
                {
                    Licence.LicName = model.LicencesName;
                    Licence.ManagerCivilId = model.ManCivilId;
                    Licence.ApplicantCivilId = model.AppCivilId;
                    Licence.SalesManagerCivilId = model.SalesManagerCivilId;
                    Licence.OperationsManagerCivilId = model.OperationManagerCivilId;
                    Licence.MarketingManagerCivilId = model.MarketingManagerCivilId;
                    Licence.ManagerId = managerId;
                    Licence.SalesManagerId = salesManagerId;
                    Licence.OperationsManagerId = OperatingManagerId;
                    Licence.MarketingManagerId = OperatingManagerId;
                    await _unitOfwork.genericRepository<Licence>().Update(Licence);
                }
                req.Licowner = model.OwnerCompanyAr;
                req.ManCivilId = model.ManCivilId;
                req.Licname = model.LicencesName;
                req.AppCivilId = model.AppCivilId;
                req.SalesManagerCivilId = model.SalesManagerCivilId;
                req.OperationsManagerCivilId = model.OperationManagerCivilId;
                req.MarketingManagerCivilId = model.MarketingManagerCivilId;
                req.ManagerId = managerId;
                req.SalesManagerId = salesManagerId;
                req.OperationsManagerId = OperatingManagerId;
                req.MarketingManagerId = OperatingManagerId;
                req.RequestModDate = DateTime.Now;
                await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Update(req);
                await _unitOfwork.Complete();

                RequestTransaction requestTransaction = new RequestTransaction()
                {
                    ReqStatusId = (int)RequestStatusEnum.Received,
                    ReqTypeId = req.ReqtypeId,
                    RequestId = req.RequestId,
                    Notes = "تحديث الرخصة",
                    Status = RequestStatusEnum.Received.ToString(),
                    CreatedDate = DateTime.Now,
                    CreatedBy = model.SessionName,
                    CivilIdUser = model.SessionCivilId,
                    UpdatedDate = DateTime.Now
                };

                await _unitOfwork.genericRepository<RequestTransaction>().Create(requestTransaction);
                await _unitOfwork.Complete();

                return Ok(new ErrorMessage { Error = false, Message = "تم تحديث البيانات بنجاح" });
            }
            catch (Exception ex)
            {
                return Ok(new ErrorMessage { Error = true, Message = ex.Message });
            }
        }

        #endregion
        #region Check PreApproval for user have it and use or or not 
        //[HttpPost]
        //[Route("CheckPreApprovalForUserAndUse")]
        //public async Task<bool> CheckPreApprovalForUserAndUse([FromBody] CheckPreAprroval checkPreApprovalVM)
        //{
        //    try
        //    {
        //        var preApprovalCheck1 = await _unitOfwork.genericRepository<MoiPreApprovement>()
        //   .GetByCondition(a => a.ApplicantCivilId == checkPreApprovalVM.CivilId
        //   && a.LicenseNo.Contains(checkPreApprovalVM.PreApprove)
        //   )
        //   .AnyAsync();
        //        var LicInfo = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
        //            .GetByCondition(l => l.Id == checkPreApprovalVM.id).FirstOrDefaultAsync();
        //        // Check the second condition in MoiEserviceTourismLicences
        //        var preApprovalCheck2 = await _unitOfwork.genericRepository<Licence>()
        //            .GetByCondition(a => a.PreApprovalNo == checkPreApprovalVM.PreApprove)
        //            .AnyAsync();
        //        var preapproveLic = await _unitOfwork.genericRepository<MoiPreApprovement>()
        //   .GetByCondition(a => a.ApplicantCivilId == checkPreApprovalVM.CivilId
        //   && a.LicenseNo == checkPreApprovalVM.PreApprove
        //   )
        //   .FirstOrDefaultAsync();
        //        var ActivitySame = await _unitOfwork.genericRepository<ActivityTypesLookup>()
        //            .GetByCondition(a => a.Id == preapproveLic.ActivityTypeId).AnyAsync();

        //        if (preApprovalCheck1 && !preApprovalCheck2)
        //        {
        //            return true;
        //        }


        //        if (preApprovalCheck1 && preApprovalCheck2)
        //        {
        //            return false;
        //        }


        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception if needed (optional)
        //        Console.WriteLine(ex.Message);

        //        // Return false in case of an error
        //        return false;
        //    }
        //}
        [HttpPost]
        [Route("CheckPreApprovalForUserAndUse")]
        public async Task<PreApprovalResult> CheckPreApprovalForUserAndUse([FromBody] CheckPreAprroval checkPreApprovalVM)
        {
            try
            {
                var preapproveLic = await _unitOfwork.genericRepository<MoiPreApprovement>()
                    .GetByCondition(a => a.ApplicantCivilId == checkPreApprovalVM.CivilId
                                      && a.LicenseNo == checkPreApprovalVM.PreApprove)
                    .FirstOrDefaultAsync();

                if (preapproveLic == null)
                {
                    return new PreApprovalResult
                    {
                        IsValid = false,
                        Message = "رقم الموافقة غير موجود"
                    };
                }
                var LicInfo = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                    .GetByCondition(l => l.Id == checkPreApprovalVM.id).FirstOrDefaultAsync();
                if (preapproveLic.ActivityTypeId != LicInfo.ActvityTypeId)
                {
                    var actualActivity = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                        .GetbyId(preapproveLic.ActivityTypeId);

                    var activityName = actualActivity?.NameAr ?? "غير معروف";

                    return new PreApprovalResult
                    {
                        IsValid = false,
                        Message = $"رقم الموافقة يخص نشاط {activityName}، وليس النشاط المختار"
                    };
                }

                var preApprovalAlreadyUsed = await _unitOfwork.genericRepository<Licence>()
                    .GetByCondition(a => a.PreApprovalNo == checkPreApprovalVM.PreApprove)
                    .AnyAsync();

                if (preApprovalAlreadyUsed)
                {
                    return new PreApprovalResult
                    {
                        IsValid = false,
                        Message = "رقم الموافقة مستخدم من قبل"
                    };
                }

                return new PreApprovalResult
                {
                    IsValid = true,
                    Message = "تم التحقق من رقم الموافقة بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new PreApprovalResult
                {
                    IsValid = false,
                    Message = "حدث خطأ أثناء التحقق من رقم الموافقة"
                };
            }
        }


        #endregion
        #region Get Information about PreApproval 
        [HttpGet]
        [Route("GetPreApprovalDetails")]
        public async Task<dynamic> GetPreApprovalDetails(string PreApproval)
        {
            //var preApprovalDetails=await _unitOfwork.genericRepository<TourMoiEserviceTourismPreApprovement>()
            //                     .GetFilteredWithProjection(
            //    filter: x => x.LicenseNo == PreApproval,
            //    selector:x=>x.
            //           }).firs();
            var preApproveWithSpec = new PreApprovementWithSpec(PreApproval);

            var preApprovalDetails = await _unitOfwork.genericRepository<MoiPreApprovement>()
                            .GetByIdWithSpec(preApproveWithSpec);



            return

               _mapper.Map<MoiPreApprovement, RequestTourLic>(preApprovalDetails);



        }
        #endregion
        #region --------------------------- إصدار فندق أو شقق فندقيه أو منتجعات ----------------
        [HttpGet]
        [Route("GetTourLicRequestDetails")]
        public async Task<ActionResult<TourLicRequestResponseVM>> GetTourLicRequestDetails(string PreApproval)
        {
            try
            {
                // Pre-approval data
                var preApproveWithSpec = new PreApprovementWithSpec(PreApproval);
                var preApprovalDetails = await _unitOfwork
                    .genericRepository<MoiPreApprovement>()
                    .GetByIdWithSpec(preApproveWithSpec);

                var mappedDetails = _mapper.Map<RequestTourLic>(preApprovalDetails);

                //Applicant

                var ApplicantLicences = await _unitOfwork.genericRepository<AspNetUser>()
                                     .GetByCondition(x => x.CivilId == preApprovalDetails.ApplicantCivilId).FirstOrDefaultAsync();

                var MappedApplicantLicences = _mapper.Map<AspnetUserVM>(ApplicantLicences);
                // File upload config
                var fileConfigs = await _unitOfwork
                    .genericRepository<FileUploadConfigurationsFront>()
                    .GetByCondition(f => f.ViewType == "Request")
                    .ToListAsync();


                var mappedFiles = _mapper.Map<List<FileUploadConfigVM>>(fileConfigs);
         



                var licencesInfoDetails = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                                          .GetByCondition(r => r.ActvityTypeId == preApprovalDetails.ActivityTypeId
                                          && r.ReqTypeId == (int)RequestTypeEnum.Request
                                          &&r.ServiceId==(int)ServiceEnum.Tourism).FirstOrDefaultAsync();

                var mappedLicence = _mapper.Map<LicencesInfoVM>(licencesInfoDetails);
                return Ok(new TourLicRequestResponseVM
                {
                    PreApprovalDetails = mappedDetails,
                    AspnetUserVM = MappedApplicantLicences,
                    FileUploadConfigs = mappedFiles,
                    LicencesInfoVM = mappedLicence
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        //--------------------------- إصدار فندق أو شقق فندقيه أو منتجعات ----------------
        [HttpPost]
        [Route("PostDataRequest")]
        public async Task<dynamic> PostDataRequest(PreApprovalRequestApiModel PreApprovalRequestModel)
        {
            string error = string.Empty;
            try
            {

                //--------------- Start trunsaction -----------------------------------
                using (IDbContextTransaction dbTransaction = _unitOfwork.BeginTransaction())
                {
                    try
                    {
                        var ClassificationId = 0;
                        if (PreApprovalRequestModel.ActivityTypeId == (int)ActivityTypeEnum.Hotel)
                        {
                            ClassificationId = (int)ClassificationEnum.NoClassificationHotel;
                        }
                        else if (PreApprovalRequestModel.ActivityTypeId == (int)ActivityTypeEnum.ApartmentHotel)
                        {
                            ClassificationId = (int)ClassificationEnum.NoClassificationApart;
                        }
                        else if (PreApprovalRequestModel.ActivityTypeId == (int)ActivityTypeEnum.Resorts)
                        {
                            ClassificationId = (int)ClassificationEnum.NoClassificationResort;
                        }
                        //------- insert in Company table ------------
                        //Company CompModel = new Company()
                        //{

                        //    CompanyCivilId = PreApprovalRequestModel.CompanyCivilId,
                        //    DirCompanyAr = PreApprovalRequestModel.DirCompanyAr,
                        //    OwnerCompanyAr = PreApprovalRequestModel.OwnerCompanyAr,
                        //    CommercialLicNo = PreApprovalRequestModel.CommercialLicNo,
                        //    RecordNo = PreApprovalRequestModel.RecordNo,
                        //    IsBuilding = false,
                        //    ActivityCode = PreApprovalRequestModel.ActivityCode,
                        //    CompanyActivity = PreApprovalRequestModel.CompanyActivity,
                        //    ServiceId = (int)ServiceEnum.Tourism,
                        //    OwnerName = PreApprovalRequestModel.OwnerCompanyAr,
                        //    ActivityTypeId = PreApprovalRequestModel.ActivityTypeId,


                        //};
                        //var _mappedCompany = _mapper.Map<Company>(CompModel);

                        //await _unitOfwork.genericRepository<Company>().Create(CompModel);
                        //await _unitOfwork.Complete();


                 

                        Address addressModel = new Address()
                        {
                            AreaChartNo = PreApprovalRequestModel.AreaChartNo,
                            AreaSize = PreApprovalRequestModel.AreaSize,
                            ServiceId = (int)ServiceEnum.Tourism,
                            AalliNo = PreApprovalRequestModel.AaliNumber,

                            GovernorateArabic = PreApprovalRequestModel.Governrate,
                            Area = PreApprovalRequestModel.Area,
                            BlockArabic = PreApprovalRequestModel.BlockNo,
                            StreetArabic = PreApprovalRequestModel.Street,
                            BuildingNo = PreApprovalRequestModel.BuildingNo,
                            BuildingName = PreApprovalRequestModel.BuildingName,
                            ActivityTypeId = PreApprovalRequestModel.ActivityTypeId,
                            ActivityCode = PreApprovalRequestModel.ActivityCode,
                            FloorNo = PreApprovalRequestModel.FloorNo,
                            UnitNo = PreApprovalRequestModel.UnitNo

                        };

                        await _unitOfwork.genericRepository<Address>().Create(addressModel);
                        await _unitOfwork.Complete();

                        int addressId = addressModel.Id;

                        //------- insert in MOI_EService_TourismBuilding table ------------
                        Company BuildingModel = new Company()
                        {

                            Name = PreApprovalRequestModel.OwnerCompanyAr,
                            ActivityTypeId = PreApprovalRequestModel.ActivityTypeId,
                            ActivityCode = PreApprovalRequestModel.ActivityCode,
                            AddressId = addressId,
                            CompanyCivilId = PreApprovalRequestModel.CompanyCivilId,
                            IsBuilding = true,
                            ServiceId = (int)ServiceEnum.Tourism,
                            CommercialLicNo = PreApprovalRequestModel.CommercialLicNo,
                            DirCompanyAr = PreApprovalRequestModel.DirCompanyAr,
                            OwnerCompanyAr = PreApprovalRequestModel.OwnerCompanyAr,
                            RecordNo = PreApprovalRequestModel.RecordNo,
                            OwnerName = PreApprovalRequestModel.OwnerCompanyAr,
                            CompanyActivity=await _unitOfwork.genericRepository<ActivityTypesLookup>()
                                         .GetByCondition(a=>a.Id== PreApprovalRequestModel.ActivityTypeId)
                                         .Select(a=>a.NameAr).FirstOrDefaultAsync()

                        };

                        var _mappedBuilding = _mapper.Map<Company>(BuildingModel);


                        await _unitOfwork.genericRepository<Company>().Create(BuildingModel);
                        await _unitOfwork.Complete();

                        int BuildID = _mappedBuilding.Id;

                        #region------- insert in Applicant table ------------
                        int ApplicantId;

                        var ApplicantExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == PreApprovalRequestModel.AppCivilId)
                            .FirstOrDefaultAsync();

                        if (ApplicantExist == null)
                        {
                            var aspnetuser = await _unitOfwork.genericRepository<AspNetUser>()
                                .GetByCondition(a => a.CivilId == PreApprovalRequestModel.AppCivilId)
                                .FirstOrDefaultAsync();

                            var newApplicant = new Person
                            {
                                Name1 = PreApprovalRequestModel.UserName,
                                Phone = aspnetuser?.Mobile,
                                Email = aspnetuser?.Email,
                                CivilId = PreApprovalRequestModel.AppCivilId,
                                ServiceId = (int)ServiceEnum.Tourism
                            };

                            await _unitOfwork.genericRepository<Person>().Create(newApplicant);
                            await _unitOfwork.Complete();

                            ApplicantId = newApplicant.Id; // ✅ Get Id after create
                        }
                        else
                        {
                            ApplicantId = ApplicantExist.Id; // ✅ Use existing Id
                        }
                        #endregion
                        #region Insert Manager
                        int ManagerId;

                        var managerExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == PreApprovalRequestModel.ManCivilId)
                            .FirstOrDefaultAsync();

                        if (managerExist == null)
                        {
                            var newManager = new Person
                            {
                                Name1 = PreApprovalRequestModel.ManagerName,
                                Phone = PreApprovalRequestModel.ManagerMobile,
                                Email = PreApprovalRequestModel.ManagerEmail,
                                ServiceId = (int)ServiceEnum.Tourism,
                                CivilId = PreApprovalRequestModel.ManCivilId
                            };

                            await _unitOfwork.genericRepository<Person>().Create(newManager);
                            await _unitOfwork.Complete();

                            ManagerId = newManager.Id; // ✅ Get Id after create
                        }
                        else
                        {
                            ManagerId = managerExist.Id; // ✅ Use existing Id
                        }
                        #endregion
                        #region Insert SalesManager
                        int SalesManagerId;

                        var SalesmanagerExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == PreApprovalRequestModel.SalesManagerCivilId)
                            .FirstOrDefaultAsync();

                        if (SalesmanagerExist == null)
                        {
                            var newSalesManager = new Person
                            {
                                Name1 = PreApprovalRequestModel.SalesManagerName,
                                Phone = PreApprovalRequestModel.SalesManagerPhone,
                                Email = PreApprovalRequestModel.SalesManagerEmail,
                                ServiceId = (int)ServiceEnum.Tourism,
                                CivilId = PreApprovalRequestModel.SalesManagerCivilId
                            };

                            await _unitOfwork.genericRepository<Person>().Create(newSalesManager);
                            await _unitOfwork.Complete();

                            SalesManagerId = newSalesManager.Id; // ✅ Get Id after create
                        }
                        else
                        {
                            SalesManagerId = SalesmanagerExist.Id; // ✅ Use existing Id
                        }
                        #endregion
                        #region Insert MarketingManager
                        int MarketingManagerId;

                        var MarketingmanagerExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == PreApprovalRequestModel.MarketingManagerCivilId)
                            .FirstOrDefaultAsync();

                        if (MarketingmanagerExist == null)
                        {
                            var newMarketingManager = new Person
                            {
                                Name1 = PreApprovalRequestModel.MarketingManagerName,
                                Phone = PreApprovalRequestModel.MarketingManagerPhone,
                                Email = PreApprovalRequestModel.MarketingManagerEmail,
                                ServiceId = (int)ServiceEnum.Tourism,
                                CivilId = PreApprovalRequestModel.MarketingManagerCivilId
                            };

                            await _unitOfwork.genericRepository<Person>().Create(newMarketingManager);
                            await _unitOfwork.Complete();

                            MarketingManagerId = newMarketingManager.Id; // ✅ Get Id after create
                        }
                        else
                        {
                            MarketingManagerId = MarketingmanagerExist.Id; // ✅ Use existing Id
                        }
                        #endregion
                        #region Insert OperationManager
                        int OperationManagerId;

                        var OperationmanagerExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == PreApprovalRequestModel.OperationManagerCivilId)
                            .FirstOrDefaultAsync();

                        if (OperationmanagerExist == null)
                        {
                            var newOperationManager = new Person
                            {
                                Name1 = PreApprovalRequestModel.OperationManagerName,
                                Phone = PreApprovalRequestModel.OperationManagerPhone,
                                Email = PreApprovalRequestModel.OperationManagerEmail,
                                ServiceId = (int)ServiceEnum.Tourism,
                                CivilId = PreApprovalRequestModel.OperationManagerCivilId
                            };

                            await _unitOfwork.genericRepository<Person>().Create(newOperationManager);
                            await _unitOfwork.Complete();

                            OperationManagerId = newOperationManager.Id; // ✅ Get Id after create
                        }
                        else
                        {
                            OperationManagerId = OperationmanagerExist.Id; // ✅ Use existing Id
                        }
                        #endregion


                        //string RequesterId = PreApprovalRequestModel.SessionCivilId;
                        var RequesterId = await _unitOfwork.genericRepository<AspNetUser>()
                                 .GetByCondition(a => a.CivilId == PreApprovalRequestModel.SessionCivilId).FirstOrDefaultAsync();


                        long SequenceNo = PreApprovalRequestModel.SequenceNo;

                        //------- insert in TourismPreApprovement table ------------
                        Licence PreApprovModel = new Licence()
                        {
                            SequenceNo = PreApprovalRequestModel.SequenceNo,
                            ServiceId = (int)ServiceEnum.Tourism,
                            //BuildingId = BuildID,
                            CompanyId = BuildingModel.Id,
                            ManagerId = ManagerId,
                            ApplicantId = ApplicantId,
                            LicTypeId = (int)LicTypeEnum.Company,
                            ClassificationId = ClassificationId,
                            CommercialLicNo = PreApprovalRequestModel.CommercialLicNo,
                            RecordNo = PreApprovalRequestModel.RecordNo,
                            ActiivityTypeId = PreApprovalRequestModel.ActivityTypeId,
                            ApplicantCivilId = PreApprovalRequestModel.AppCivilId,
                            LicName = PreApprovalRequestModel.LicencesName,
                            LicStatusId = (int)licencesStatusEnum.Pending,
                            ManagerCivilId = PreApprovalRequestModel.ManCivilId,
                            PreApprovalNo = PreApprovalRequestModel.PreApprove,
                            OperationsManagerCivilId=PreApprovalRequestModel.OperationManagerCivilId,
                            SalesManagerCivilId=PreApprovalRequestModel.SalesManagerCivilId,
                            MarketingManagerCivilId=PreApprovalRequestModel.MarketingManagerCivilId,
                            OperationsManagerId=OperationManagerId,
                            SalesManagerId=SalesManagerId,
                            MarketingManagerId=MarketingManagerId,
                            PreApprovalId=PreApprovalRequestModel.PreApproveId,
                            
                        };

                        var _mappedPreApprov = _mapper.Map<Licence>(PreApprovModel);
                        await _unitOfwork.genericRepository<Licence>().Create(PreApprovModel);
                        await _unitOfwork.Complete();
                        int prelicid = _mappedPreApprov.LicId;
                        var PreAprove = await _unitOfwork.genericRepository<MoiPreApprovement>()
                            .GetByCondition(p => p.PreAppId == PreApprovalRequestModel.PreApproveId).FirstOrDefaultAsync();
                        //------- insert in Request table ------------

                        MoiEserviceLicensesRequest ReqModel = new MoiEserviceLicensesRequest();
                        try
                        {


                            ReqModel = new MoiEserviceLicensesRequest()
                            {
                                Reqno = PreApprovalRequestModel.reqno,
                                ReqtypeId = (int)RequestTypeEnum.Request,
                                Licno = null,
                                ActivityType = "طلب إصدار ترخيص تشغيلي",
                                ServiceId = (int)ServiceEnum.Tourism,
                                Licowner = PreApprovalRequestModel.OwnerCompanyAr,
                                Licname = PreApprovalRequestModel.LicencesName,
                                //BuildingId = BuildID,
                                CompanyId = BuildingModel.Id,
                                ManagerId = ManagerId,
                                AppId= ApplicantId,
                                Licexpiredate = null,
                                SequenceNo = SequenceNo,
                                Licreqtime = DateTime.Now,
                                Requesterid = RequesterId.Id,
                                RequestNote = null,
                                RequestStatusId = (int)RequestStatusEnum.Received,
                                RequestAttach = "Yes",
                                LicenseId = prelicid,
                                RequesterCivilId = PreApprovalRequestModel.SessionCivilId,
                                Licamount = PreApprovalRequestModel.Amount,
                                Licpaystatus = "0",
                                CategoryId = 1,
                                SectorId = 3,
                                AppCivilId = PreApprovalRequestModel.AppCivilId,
                                ManCivilId = PreApprovalRequestModel.ManCivilId,
                                //UserCivilId = PreApprovalRequestModel.UserCivilID,
                                PreApprovalNo = PreApprovalRequestModel.PreApprove,
                                PreApprovalId = PreAprove.PreAppId,
                                LicStatusId = (int)licencesStatusEnum.Pending,
                                ActivityTypeId = PreApprovalRequestModel.ActivityTypeId,
                                LicrequestIsDeleted = false,
                                IsArchived = false,
                                LicTypeId = (int)LicTypeEnum.Company,
                                ActivityCode = PreApprovalRequestModel.ActivityCode,
                                MarketingManagerCivilId = PreApprovalRequestModel.MarketingManagerCivilId,
                                SalesManagerCivilId = PreApprovalRequestModel.SalesManagerCivilId,
                                OperationsManagerCivilId = PreApprovalRequestModel.OperationManagerCivilId,
                                MarketingManagerId = MarketingManagerId,
                                SalesManagerId = SalesManagerId,
                                OperationsManagerId = OperationManagerId
                            };
                        }catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        var _mappedRequest = _mapper.Map<MoiEserviceLicensesRequest>(ReqModel);

                        await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(ReqModel);
                        await _unitOfwork.Complete();

                        long reqid = _mappedRequest.RequestId;

                        if (PreAprove != null)
                        {
                            PreAprove.IsConsumed = true;  // Assuming you added a field for that
                            PreAprove.ConsumedDate = DateTime.UtcNow;
                            PreAprove.LinkedLicenseId = prelicid;
                            PreAprove.Flag = "موافقة مبدئية مستخدمة";
                            await _unitOfwork.genericRepository<MoiPreApprovement>().Update(PreAprove);
                            await _unitOfwork.Complete();  // Save changes
                        }

                        RequestTransaction requestTransaction = new RequestTransaction()
                        {
                            ReqStatusId = (int)RequestStatusEnum.Received,
                            ReqTypeId =(int)RequestTypeEnum.Request,
                            RequestId = reqid,
                            Notes ="إصدار ترخيص تشغيلي " ,
                            Status = RequestStatusEnum.Received.ToString(),
                            CreatedDate = DateTime.Now,
                            CreatedBy = PreApprovalRequestModel.SessionName,
                            CivilIdUser = PreApprovalRequestModel.SessionCivilId,
                            UpdatedDate = DateTime.Now
                        };
                        await _unitOfwork.genericRepository<RequestTransaction>().Create(requestTransaction);
                        await _unitOfwork.Complete();

                        //--------Insert InTable Attachment---------------

                        await InsertAttachements(PreApprovalRequestModel.saveResponseVMs, reqid,PreApprovalRequestModel.SessionCivilId);

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
        #region update RequestLic  in request WHen the status is Received
        //[HttpPost]
        //[Route("UpdateLicRequestDetails")]
        //public async Task<ActionResult> UpdateLicRequestDetails(RequestFrontVM model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Ok(new { success = false, message = "البيانات غير صالحة" });
        //    }

        //    try
        //    {
        //        var req = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
        //            .GetByCondition(r => r.Reqno == model.RequestVM.Reqno)
        //            .FirstOrDefaultAsync();

        //        if (req == null)
        //            return Ok (new{ success = false, message = "الطلب غير موجود" });

        //        // 2. Update Request
        //        req.Licowner = model.RequestVM.company?.OwnerCompanyAr;
        //        req.Licname = model.RequestVM.Licname;
        //        req.ManCivilId = model.RequestVM.Manager?.CivilId;
            
               

        //        await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Update(req);

        //        // 3. Update Company (main)
        //        var company = await _unitOfwork.genericRepository<Company>()
        //            .GetByCondition(c => c.Id == req.CompanyId)
        //            .FirstOrDefaultAsync();

        //        if (company != null)
        //        {
        //            company.OwnerCompanyAr = model.RequestVM.company?.OwnerCompanyAr;
        //            company.CompanyCivilId = model.RequestVM.company.CompanyCivilId;
        //            company.RecordNo = model.RequestVM.company.RecordNo;
        //            company.CommercialLicNo = model.RequestVM.company.CommercialLicNo;
        //            company.DirCompanyAr = model.RequestVM.company.DirCompanyAr;
        //            await _unitOfwork.genericRepository<Company>().Update(company);
        //        }

               

        //        // 5. Update Licence table
        //        var licence = await _unitOfwork.genericRepository<Licence>()
        //            .GetByCondition(l => l.LicId == req.LicenseId)
        //            .FirstOrDefaultAsync();

        //        if (licence != null)
        //        {
        //            licence.ManagerCivilId = model.RequestVM.Manager?.CivilId;
        //            licence.LicName = model.RequestVM.Licname;
        //            //licence.OwnerCompanyAr = model.RequestVM.company?.OwnerCompanyAr;
        //            licence.RecordNo = model.RequestVM.company?.RecordNo;
        //            licence.CommercialLicNo = model.RequestVM.company?.CommercialLicNo;
        //            await _unitOfwork.genericRepository<Licence>().Update(licence);
        //        }

        //        // 6. Update MoiPreApprovement
        //        var preApp = await _unitOfwork.genericRepository<MoiPreApprovement>()
        //            .GetByCondition(p => p.RequestId == req.RequestId)
        //            .FirstOrDefaultAsync();

        //        if (preApp != null)
        //        {
        //            preApp.ManagerCivilId = model.RequestVM.Manager?.CivilId;
        //            preApp.LicenseName = model.RequestVM.Licname;
        //            //preApp.OwnerCompanyAr = model.RequestVM.company?.OwnerCompanyAr;
        //            await _unitOfwork.genericRepository<MoiPreApprovement>().Update(preApp);
        //        }

        //        // 7. Update Manager (Person table)
        //        var manager = await _unitOfwork.genericRepository<Person>()
        //            .GetByCondition(p => p.CivilId == model.RequestVM.ManCivilId)
        //            .FirstOrDefaultAsync();

        //        if (manager != null)
        //        {
        //            manager.Name1 = model.RequestVM.Manager?.Name1;
        //            manager.Email = model.RequestVM.Manager?.Email;
        //            manager.Phone = model.RequestVM.Manager?.Phone;
        //            await _unitOfwork.genericRepository<Person>().Update(manager);
        //        }



        //        int? addressId = null;

        //        // First try from Building
        //        if (req.BuildingId.HasValue)
        //        {
        //            var building = await _unitOfwork.genericRepository<Company>()
        //                .GetByCondition(c => c.Id == req.BuildingId.Value)
        //                .FirstOrDefaultAsync();

        //            addressId = building?.AddressId;
        //        }

        //        // If not found, fallback to Company
        //        if (!addressId.HasValue && req.CompanyId > 0)
        //        {
        //            var companyWithAddress = await _unitOfwork.genericRepository<Company>()
        //                .GetByCondition(c => c.Id == req.CompanyId)
        //                .FirstOrDefaultAsync();

        //            addressId = companyWithAddress?.AddressId;
        //        }

        //        if (addressId.HasValue)
        //        {
        //            var address = await _unitOfwork.genericRepository<Address>()
        //                .GetByCondition(a => a.Id == addressId.Value)
        //                .FirstOrDefaultAsync();

        //            if (address != null)
        //            {
        //                address.AalliNo = model.RequestVM.Building?.AddressNavigation?.AalliNo;
        //                address.Area = model.RequestVM.Building?.AddressNavigation?.Area;
        //                address.BlockArabic = model.RequestVM.Building?.AddressNavigation?.BlockArabic;
        //                address.StreetArabic = model.RequestVM.Building?.AddressNavigation?.StreetArabic;
        //                address.BuildingName = model.RequestVM.Building?.AddressNavigation?.BuildingName;
        //                address.BuildingNo = model.RequestVM.Building?.AddressNavigation?.BuildingNo;
        //                address.AreaChartNo = model.RequestVM.Building?.AddressNavigation?.AreaChartNo;
        //                address.AreaSize = model.RequestVM.Building?.AddressNavigation?.AreaSize;
        //                address.GovernorateArabic = model.RequestVM.Building?.AddressNavigation?.GovernorateArabic;
        //                address.FloorNo = model.RequestVM.Building?.AddressNavigation?.FloorNo;
        //                address.UnitNo = model.RequestVM.Building?.AddressNavigation?.UnitNo;

        //                await _unitOfwork.genericRepository<Address>().Update(address);
        //            }
        //        }
        //        // Update Manager Person
                
        //        await _unitOfwork.Complete();
        //        // Log the update in RequestTransaction
        //        RequestTransaction transaction = new RequestTransaction()
        //        {
        //            ReqStatusId = (int)RequestStatusEnum.Received,
        //            ReqTypeId = req.ReqtypeId,
        //            RequestId = req.RequestId,
        //            Notes = "تحديث بيانات الترخيص (كامل)",
        //            Status = RequestStatusEnum.Received.ToString(),
        //            CreatedDate = DateTime.Now,
        //            CreatedBy = model.RequestVM.SessionFullNaame,
        //            CivilIdUser = model.RequestVM.SessionCivilId,
        //            UpdatedDate = DateTime.Now
        //        };

        //        await _unitOfwork.genericRepository<RequestTransaction>().Create(transaction);

        //        // 10. Final commit
        //        await _unitOfwork.Complete();
        //        return Ok(new { success = true, message = "تم تحديث البيانات بنجاح" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(new { success = false, message = ex.Message });
        //    }
        //}
        #endregion
        
        #region ------------------------------إصدار لمن يهمه الأمر ---------------
        [HttpGet]
        [Route("GetLicenceDetailsForWhoConc")]
        public async Task<RequestBaseVM> GetLicenceDetailsForWhoConc(int licId)
        {
            var LicencesSpec = new LicencesWithSpecificService(licId, (int)ServiceEnum.Tourism);

            var licenceDetails = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(LicencesSpec);

            var FileUpload = await _unitOfwork.genericRepository<AttachRule>()
                             .GetByCondition(f => f.ViewType == "WhoConcRequest").ToListAsync();
            var Applicant = await _unitOfwork.genericRepository<AspNetUser>()
                                .GetByCondition(a => a.CivilId == licenceDetails.ApplicantCivilId).FirstOrDefaultAsync();
            var licencesInfoDetails = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                                          .GetByCondition(r => r.ActvityTypeId == licenceDetails.ActiivityTypeId
                                          && r.ReqTypeId ==(int)RequestTypeEnum.WhoConc
                                          && r.ServiceId == (int)ServiceEnum.Tourism).FirstOrDefaultAsync();

            var mappedLicenceInfo = _mapper.Map<LicencesInfoVM>(licencesInfoDetails);

            return new RequestBaseVM
            {
                LicencesVM = _mapper.Map<Licence, LicencesVM>(licenceDetails),
                 FileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(FileUpload),
                AspnetUserVM = _mapper.Map<AspNetUser, AspnetUserVM>(Applicant),
                LicencesInfo=mappedLicenceInfo

            };

        }


        [HttpPost]
        [Route("PostDataWhoConcRequest")]
        public async Task<dynamic> PostDataWhoConcRequest(PreApprovalRequestApiModel model)
        {
            return await HandleCommonTourismRequest(model, RequestTypeEnum.WhoConc, async (requestId, requestTransactionId) =>
            {
                // Get the full name of the old applicant
                var usernameForApplicant = await _unitOfwork.genericRepository<AspNetUser>()
                    .GetByCondition(a => a.CivilId == model.AppCivilId).FirstOrDefaultAsync();

               
            });
        }

        #endregion


        #region-----------------------------إبداء الرأي للتجارة----------------------------------------
        [HttpGet]
        [Route("GetChooseWhichMOICLetter")]
        public async Task<IActionResult> GetChooseWhichMOICLetter()
        {
            var allowedActivityIds = new List<int>
            { (int)ActivityTypeEnum.Hotel,
                (int)ActivityTypeEnum.ApartmentHotel,
                (int)ActivityTypeEnum.Resorts
            ,(int)ActivityTypeEnum.Parks
            ,(int)ActivityTypeEnum.Sailing};

            var allowedReqTypeIds = new List<int>
            { (int)RequestTypeEnum.DeleteMOIC,
                 (int)RequestTypeEnum.ChangeData,
                 (int)RequestTypeEnum.RenewOrChangeMOIC
            , (int)RequestTypeEnum.AddMoIC
            , (int)RequestTypeEnum.ChangeAddressMOIC};

            var activityTypes = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                .GetByCondition(a => allowedActivityIds.Contains(a.Id))
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = a.NameAr,

                }).ToListAsync();

            var requestTypes = await _unitOfwork.genericRepository<RequestsTypesLookup>()
                .GetByCondition(r => allowedReqTypeIds.Contains(r.Id))
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.NameAr,

                }).ToListAsync();

            var dropdownModel = new MOICDropdownVM
            {
                ActivityTypes = activityTypes,
                RequestTypes = requestTypes
            };

            return Ok(dropdownModel);


        }
        [HttpGet]
        [Route("GetLicencesDropDownPerUser")]
        public async Task<IActionResult> GetLicencesDropDownPerUser(string CivilId)
        {
            int LicStatus = (int)licencesStatusEnum.Released;

            var licencesDetails = await _unitOfwork.genericRepository<Licence>()
                .GetByCondition(l => l.ApplicantCivilId == CivilId && l.LicStatusId == LicStatus)
                .Select(r => new SelectListItem
                {
                    Value = r.LicId.ToString(),
                    Text = r.LicNo,

                }).ToListAsync();

            return Ok(licencesDetails);

        }

        [HttpGet]
        [Route("GetLicenseDetailsMOIC")]
        public async Task<MoicRequestVM> GetLicenseDetailsMOIC(int LicId, int ReqType, int ActivitiID)
        {
            List<AttachRule> fileUploadConfigurationsFront = await _unitOfwork.genericRepository<AttachRule>()
                                            .GetByCondition(f => f.RequestTypeId==ReqType).ToListAsync();
            Licence licence = new Licence();

            if (ReqType == (int)RequestTypeEnum.AddMoIC)
            {
                //fileUploadConfigurationsFront = await _unitOfwork.genericRepository<FileUploadConfigurationsFront>()
                //                             .GetByCondition(f => f.ViewType == "AddMoIC").ToListAsync();


            }
            else if (ReqType == (int)RequestTypeEnum.RenewMOIC || ReqType == (int)RequestTypeEnum.DeleteMOIC)
            {
                //fileUploadConfigurationsFront = await _unitOfwork.genericRepository<FileUploadConfigurationsFront>()
                //                             .GetByCondition(f => f.ViewType == "RenewMOIC").ToListAsync();
                var licenceSpec = new LicencesWithSpecificService(LicId, true);
                licence = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licenceSpec);
            }
            else if (ReqType == (int)RequestTypeEnum.RenewOrChangeMOIC || ReqType == (int)RequestTypeEnum.ChangeAddressMOIC)
            {
                //fileUploadConfigurationsFront = await _unitOfwork.genericRepository<FileUploadConfigurationsFront>()
                //                             .GetByCondition(f => f.ViewType == "RenewOrChangeMOIC").ToListAsync();
                var licenceSpec = new LicencesWithSpecificService(LicId, true);
                licence = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licenceSpec);
            }
            var Activity = await _unitOfwork.genericRepository<ActivityTypesLookup>().GetByCondition(a => a.Id == ActivitiID).FirstOrDefaultAsync();
            var Requesttype = await _unitOfwork.genericRepository<RequestsTypesLookup>().GetByCondition(r => r.Id == ReqType).FirstOrDefaultAsync();
            return new MoicRequestVM
            {
                fileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(fileUploadConfigurationsFront),
                ActivityCode = Activity.ActivityCode,
                ActivityName = Activity.NameAr,
                ActivityTypeId = ActivitiID,
                LicencesVM = _mapper.Map<Licence, LicencesVM>(licence),
                ReqTypeId = ReqType,
                ReqTypeName = Requesttype.NameAr,
                LicId = LicId,

            };
        }

        [HttpPost]
        [Route("PostDataLicMOIC")]
        public async Task<dynamic> PostDataLicMOIC(PreApprovalRequestApiModel preApprovalRequestApiModel)
        {

            string error = string.Empty;
            try
            {

                //--------------- Start trunsaction -----------------------------------
                using (IDbContextTransaction dbTransaction = _unitOfwork.BeginTransaction())
                {
                    try
                    {

                        Address addressModel = new Address()
                        {

                            ServiceId = (int)ServiceEnum.Tourism,
                            AalliNo = preApprovalRequestApiModel.AaliNumber,

                            GovernorateArabic = preApprovalRequestApiModel.Governrate,
                            Area = preApprovalRequestApiModel.Area,
                            BlockArabic = preApprovalRequestApiModel.BlockNo,
                            StreetArabic = preApprovalRequestApiModel.Street,
                            BuildingNo = preApprovalRequestApiModel.BuildingNo,
                            BuildingName = preApprovalRequestApiModel.BuildingName,
                            ActivityTypeId = preApprovalRequestApiModel.ActivityTypeId,
                            ActivityCode = preApprovalRequestApiModel.ActivityCode,
                            FloorNo = preApprovalRequestApiModel.FloorNo,
                            UnitNo = preApprovalRequestApiModel.UnitNo

                        };

                        await _unitOfwork.genericRepository<Address>().Create(addressModel);
                        await _unitOfwork.Complete();

                        int addressId = addressModel.Id;
                        //------- insert in Company table ------------
                        Company CompModel = new Company()
                        {

                            CompanyCivilId = preApprovalRequestApiModel.CompanyCivilId,

                            OwnerCompanyAr = preApprovalRequestApiModel.OwnerCompanyAr,
                            CommercialLicNo = preApprovalRequestApiModel.CommercialLicNo,
                            RecordNo = preApprovalRequestApiModel.RecordNo,
                            CentralNoMoci=preApprovalRequestApiModel.CentralNoMOIc,
                            IsBuilding = false,
                            ActivityCode = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                                      .GetByCondition(a => a.Id == preApprovalRequestApiModel.ActivityTypeId)
                                      .Select(a => a.ActivityCode).FirstOrDefaultAsync(),
                            CompanyActivity = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                                    .GetByCondition(a => a.Id == preApprovalRequestApiModel.ActivityTypeId).Select(c => c.NameAr).FirstOrDefaultAsync(),
                            ServiceId = (int)ServiceEnum.Tourism,
                            OwnerName = preApprovalRequestApiModel.OwnerCompanyAr,
                            ActivityTypeId = preApprovalRequestApiModel.ActivityTypeId,
                            AddressId = addressId

                        };
                        var _mappedCompany = _mapper.Map<Company>(CompModel);

                        await _unitOfwork.genericRepository<Company>().Create(CompModel);
                        await _unitOfwork.Complete();
                        int ComID = _mappedCompany.Id;


                        //string RequesterId = preApprovalRequestApiModel.SessionCivilId;
                        var RequesterId = await _unitOfwork.genericRepository<AspNetUser>()
                              .GetByCondition(a => a.CivilId == preApprovalRequestApiModel.SessionCivilId).FirstOrDefaultAsync();
                        string MandoobId = "";

                        //if (preApprovalRequestApiModel.accountTypeId == "100")
                        //{
                        //    RequesterId = preApprovalRequestApiModel.AppId;
                        //    AppId = preApprovalRequestApiModel.AppId;
                        //    MandoobId = "";
                        //}
                        //if (preApprovalRequestApiModel.accountTypeId == "300")
                        //{
                        //    RequesterId = preApprovalRequestApiModel.MandoobId;
                        //    AppId = "";

                        //    MandoobId = preApprovalRequestApiModel.AppId;
                        //}

                        int ApplicantId;

                        var ApplicantExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == preApprovalRequestApiModel.AppCivilId)
                            .FirstOrDefaultAsync();

                        if (ApplicantExist == null)
                        {
                            var aspnetuser = await _unitOfwork.genericRepository<AspNetUser>()
                                .GetByCondition(a => a.CivilId == preApprovalRequestApiModel.AppCivilId)
                                .FirstOrDefaultAsync();

                            var newApplicant = new Person
                            {
                                Name1 = preApprovalRequestApiModel.UserName,
                                Phone = aspnetuser?.Mobile,
                                Email = aspnetuser?.Email,
                                CivilId = preApprovalRequestApiModel.AppCivilId,
                                ServiceId = (int)ServiceEnum.Tourism
                            };

                            await _unitOfwork.genericRepository<Person>().Create(newApplicant);
                            await _unitOfwork.Complete();

                            ApplicantId = newApplicant.Id; // ✅ Get Id after create
                        }
                        else
                        {
                            ApplicantId = ApplicantExist.Id; // ✅ Use existing Id
                        }


                        

                        long SequenceNo = preApprovalRequestApiModel.SequenceNo;

                        //------- insert in Licenserequest table ------------
                        MoiEserviceLicensesRequest ReqModel = new MoiEserviceLicensesRequest()
                        {
                            Reqno = preApprovalRequestApiModel.reqno,
                            ReqtypeId = preApprovalRequestApiModel.ReqtypeId,
                            Licno = null,
                            ActivityType = "طلب إصدار أنشطة سياحية",
                            ServiceId = (int)ServiceEnum.Tourism,
                            Licowner = preApprovalRequestApiModel.OwnerCompanyAr,
                            Licname = preApprovalRequestApiModel.OwnerCompanyAr,
                            ManagerId = preApprovalRequestApiModel.ManId,
                            CompanyId = ComID,
                            LicenseId = preApprovalRequestApiModel.LicId,
                            AppId=ApplicantId,
                            RequesterCivilId= preApprovalRequestApiModel.SessionCivilId,
                            AddressIdMocI= addressModel.Id,
                            Licexpiredate = null,
                            SequenceNo = SequenceNo,
                            Licreqtime = DateTime.Now,

                            Requesterid = RequesterId.Id,
                            RequestNote = null,
                            RequestStatusId = (int)RequestStatusEnum.Received,
                            RequestAttach = "Yes",

                            Licpaystatus = "0",
                            CategoryId = 1,
                            SectorId = 3,
                            AppCivilId = preApprovalRequestApiModel.AppCivilId,
                            ManCivilId = preApprovalRequestApiModel.ManCivilId,
                            //UserCivilId = preApprovalRequestApiModel.UserCivilID,
                            PreApprovalNo = preApprovalRequestApiModel.PreApprove,
                            LicStatusId = (int)licencesStatusEnum.Pending,
                            ActivityTypeId = preApprovalRequestApiModel.ActivityTypeId,
                            LicrequestIsDeleted = false,
                            IsArchived = false,
                            LicTypeId = (int)LicTypeEnum.Company,
                            ActivityCode = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                                      .GetByCondition(a=>a.Id==preApprovalRequestApiModel.ActivityTypeId)
                                      .Select(a=>a.ActivityCode).FirstOrDefaultAsync(),


                        };

                        var _mappedRequest = _mapper.Map<MoiEserviceLicensesRequest>(ReqModel);

                        await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(ReqModel);
                        await _unitOfwork.Complete();

                        long reqid = _mappedRequest.RequestId;


                        string Notes = "";
                        if (preApprovalRequestApiModel.ReqtypeId == (int)RequestTypeEnum.AddMoIC)
                        {
                            Notes = "إصدار أو إضافة إبداء الرأي للتجارة";
                        }
                        else if (preApprovalRequestApiModel.ReqtypeId == (int)RequestTypeEnum.ChangeAddressMOIC)
                        {
                            Notes = "تغيير العنوان إبداء الرأي للتجارة";
                        }
                        else if (preApprovalRequestApiModel.ReqtypeId == (int)RequestTypeEnum.RenewOrChangeMOIC)
                        {
                            Notes = "تجديد أو تعديل إبداء الرأي للتجارة";
                        }
                        else if (preApprovalRequestApiModel.ReqtypeId == (int)RequestTypeEnum.DeleteMOIC)
                        {
                            Notes = "حذف إبداء الرأي للتجارة";
                        }
                        else if (preApprovalRequestApiModel.ReqtypeId == (int)RequestTypeEnum.RenewMOIC)
                        {
                            Notes = "تجديد إبداء الرأي للتجارة";
                        }

                        RequestTransaction requestTransaction = new RequestTransaction()
                        {
                            ReqStatusId = (int)RequestStatusEnum.Received,
                            ReqTypeId = preApprovalRequestApiModel.ReqtypeId,
                            RequestId = reqid,
                            Notes = Notes,
                            Status= RequestStatusEnum.Received.ToString(),
                            CreatedDate = DateTime.Now,
                            CreatedBy = preApprovalRequestApiModel.SessionName,
                            CivilIdUser = preApprovalRequestApiModel.SessionCivilId,
                            UpdatedDate = DateTime.Now
                        };
                        await _unitOfwork.genericRepository<RequestTransaction>().Create(requestTransaction);
                        await _unitOfwork.Complete();

                        await InsertAttachements(preApprovalRequestApiModel.saveResponseVMs, reqid,preApprovalRequestApiModel.SessionCivilId);




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
        #region-------------------------إصدار منتزهات ورحلات Parks and Sailing ---------------------
        [HttpPost]
        [Route("PostDataLicActivity")]
        public async Task<dynamic> PostDataLicActivity(PreApprovalRequestApiModel PreApprovalRequestModel)
        {
            string error = string.Empty;
            try
            {

                //--------------- Start trunsaction -----------------------------------
                using (IDbContextTransaction dbTransaction = _unitOfwork.BeginTransaction())
                {
                    try
                    {

                        Address addressModel = new Address()
                        {

                            ServiceId = (int)ServiceEnum.Tourism,
                            AalliNo = PreApprovalRequestModel.AaliNumber,

                            GovernorateArabic = PreApprovalRequestModel.Governrate,
                            Area = PreApprovalRequestModel.Area,
                            BlockArabic = PreApprovalRequestModel.BlockNo,
                            StreetArabic = PreApprovalRequestModel.Street,
                            BuildingNo = PreApprovalRequestModel.BuildingNo,
                            BuildingName = PreApprovalRequestModel.BuildingName,
                            ActivityTypeId = PreApprovalRequestModel.ActivityTypeId,
                            ActivityCode = PreApprovalRequestModel.ActivityCode,
                            FloorNo = PreApprovalRequestModel.FloorNo,
                            UnitNo = PreApprovalRequestModel.UnitNo

                        };

                        await _unitOfwork.genericRepository<Address>().Create(addressModel);
                        await _unitOfwork.Complete();

                        int addressId = addressModel.Id;
                        //------- insert in Company table ------------
                        Company CompModel = new Company()
                        {

                            CompanyCivilId = PreApprovalRequestModel.CompanyCivilId,

                            OwnerCompanyAr = PreApprovalRequestModel.OwnerCompanyAr,
                            CommercialLicNo = PreApprovalRequestModel.CommercialLicNo,
                            RecordNo = PreApprovalRequestModel.RecordNo,
                            IsBuilding = false,
                            ActivityCode = PreApprovalRequestModel.ActivityCode,
                            CompanyActivity = await _unitOfwork.genericRepository<ActivityTypesLookup>()
        .GetByCondition(a => a.Id == PreApprovalRequestModel.ActivityTypeId).Select(c => c.NameAr).FirstOrDefaultAsync(),
                            ServiceId = (int)ServiceEnum.Tourism,
                            OwnerName = PreApprovalRequestModel.OwnerCompanyAr,
                            ActivityTypeId = PreApprovalRequestModel.ActivityTypeId,
                            AddressId = addressId

                        };
                       

                        await _unitOfwork.genericRepository<Company>().Create(CompModel);
                        await _unitOfwork.Complete();
                        

                        #region------- insert in Applicant table ------------
                        int ApplicantId;

                        var ApplicantExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == PreApprovalRequestModel.AppCivilId)
                            .FirstOrDefaultAsync();

                        if (ApplicantExist == null)
                        {
                            var aspnetuser = await _unitOfwork.genericRepository<AspNetUser>()
                                .GetByCondition(a => a.CivilId == PreApprovalRequestModel.AppCivilId)
                                .FirstOrDefaultAsync();

                            var newApplicant = new Person
                            {
                                Name1 = PreApprovalRequestModel.UserName,
                                Phone = aspnetuser?.Mobile,
                                Email = aspnetuser?.Email,
                                CivilId = PreApprovalRequestModel.AppCivilId,
                                ServiceId = (int)ServiceEnum.Tourism
                            };

                            await _unitOfwork.genericRepository<Person>().Create(newApplicant);
                            await _unitOfwork.Complete();

                            ApplicantId = newApplicant.Id; // ✅ Get Id after create
                        }
                        else
                        {
                            ApplicantId = ApplicantExist.Id; // ✅ Use existing Id
                        }
                        #endregion
                        #region Insert Manager
                        int ManagerId;

                        var managerExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == PreApprovalRequestModel.ManCivilId)
                            .FirstOrDefaultAsync();

                        if (managerExist == null)
                        {
                            var newManager = new Person
                            {
                                Name1 = PreApprovalRequestModel.ManagerName,
                                Phone = PreApprovalRequestModel.ManagerMobile,
                                Email = PreApprovalRequestModel.ManagerEmail,
                                ServiceId = (int)ServiceEnum.Tourism,
                                CivilId = PreApprovalRequestModel.ManCivilId
                            };

                            await _unitOfwork.genericRepository<Person>().Create(newManager);
                            await _unitOfwork.Complete();

                            ManagerId = newManager.Id; // ✅ Get Id after create
                        }
                        else
                        {
                            ManagerId = managerExist.Id; // ✅ Use existing Id
                        }
                        #endregion
                        #region Insert SalesManager
                        int SalesManagerId;

                        var SalesmanagerExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == PreApprovalRequestModel.SalesManagerCivilId)
                            .FirstOrDefaultAsync();

                        if (SalesmanagerExist == null)
                        {
                            var newSalesManager = new Person
                            {
                                Name1 = PreApprovalRequestModel.SalesManagerName,
                                Phone = PreApprovalRequestModel.SalesManagerPhone,
                                Email = PreApprovalRequestModel.SalesManagerEmail,
                                ServiceId = (int)ServiceEnum.Tourism,
                                CivilId = PreApprovalRequestModel.SalesManagerCivilId
                            };

                            await _unitOfwork.genericRepository<Person>().Create(newSalesManager);
                            await _unitOfwork.Complete();

                            SalesManagerId = newSalesManager.Id; // ✅ Get Id after create
                        }
                        else
                        {
                            SalesManagerId = SalesmanagerExist.Id; // ✅ Use existing Id
                        }
                        #endregion
                        #region Insert MarketingManager
                        int MarketingManagerId;

                        var MarketingmanagerExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == PreApprovalRequestModel.MarketingManagerCivilId)
                            .FirstOrDefaultAsync();

                        if (MarketingmanagerExist == null)
                        {
                            var newMarketingManager = new Person
                            {
                                Name1 = PreApprovalRequestModel.MarketingManagerName,
                                Phone = PreApprovalRequestModel.MarketingManagerPhone,
                                Email = PreApprovalRequestModel.MarketingManagerEmail,
                                ServiceId = (int)ServiceEnum.Tourism,
                                CivilId = PreApprovalRequestModel.MarketingManagerCivilId
                            };

                            await _unitOfwork.genericRepository<Person>().Create(newMarketingManager);
                            await _unitOfwork.Complete();

                            MarketingManagerId = newMarketingManager.Id; // ✅ Get Id after create
                        }
                        else
                        {
                            MarketingManagerId = MarketingmanagerExist.Id; // ✅ Use existing Id
                        }
                        #endregion
                        #region Insert OperationManager
                        int OperationManagerId;

                        var OperationmanagerExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == PreApprovalRequestModel.OperationManagerCivilId)
                            .FirstOrDefaultAsync();

                        if (OperationmanagerExist == null)
                        {
                            var newOperationManager = new Person
                            {
                                Name1 = PreApprovalRequestModel.OperationManagerName,
                                Phone = PreApprovalRequestModel.OperationManagerPhone,
                                Email = PreApprovalRequestModel.OperationManagerEmail,
                                ServiceId = (int)ServiceEnum.Tourism,
                                CivilId = PreApprovalRequestModel.OperationManagerCivilId
                            };

                            await _unitOfwork.genericRepository<Person>().Create(newOperationManager);
                            await _unitOfwork.Complete();

                            OperationManagerId = newOperationManager.Id; // ✅ Get Id after create
                        }
                        else
                        {
                            OperationManagerId = OperationmanagerExist.Id; // ✅ Use existing Id
                        }
                        #endregion


                        await _unitOfwork.Complete();


                        //string RequesterId = PreApprovalRequestModel.SessionCivilId;
                        var RequesterId = await _unitOfwork.genericRepository<AspNetUser>()
                            .GetByCondition(a => a.CivilId == PreApprovalRequestModel.SessionCivilId).FirstOrDefaultAsync();


                        long SequenceNo = PreApprovalRequestModel.SequenceNo;
                        //------- insert in TourismPreApprovement table ------------
                        Licence PreApprovModel = new Licence()
                        {
                            
                            ServiceId = (int)ServiceEnum.Tourism,
                            BuildingId = null,
                            CompanyId = CompModel.Id,
                            ManagerId = ManagerId,
                          Licowner=PreApprovalRequestModel.OwnerCompanyAr,
                            ApplicantId = ApplicantId,
                            LicTypeId = (int)LicTypeEnum.Company,
                            CommercialLicNo = PreApprovalRequestModel.CommercialLicNo,
                            RecordNo = PreApprovalRequestModel.RecordNo,
                            ActiivityTypeId = PreApprovalRequestModel.ActivityTypeId,
                            ApplicantCivilId = PreApprovalRequestModel.AppCivilId,
                            LicName = PreApprovalRequestModel.LicencesName,
                            LicStatusId = (int)licencesStatusEnum.Pending,
                            ManagerCivilId = PreApprovalRequestModel.ManCivilId,
                            OperationsManagerCivilId=PreApprovalRequestModel.OperationManagerCivilId,
                            SalesManagerCivilId=PreApprovalRequestModel.SalesManagerCivilId,
                            MarketingManagerCivilId=PreApprovalRequestModel.MarketingManagerCivilId,
                            OperationsManagerId=OperationManagerId,
                            SalesManagerId=SalesManagerId,
                            MarketingManagerId=MarketingManagerId,


                        };

                        var _mappedPreApprov = _mapper.Map<Licence>(PreApprovModel);
                        await _unitOfwork.genericRepository<Licence>().Create(PreApprovModel);
                        await _unitOfwork.Complete();

                        int prelicid = _mappedPreApprov.LicId;
                        //------- insert in Licenserequest table ------------
                        MoiEserviceLicensesRequest ReqModel = new MoiEserviceLicensesRequest()
                        {
                            Reqno = PreApprovalRequestModel.reqno,
                            ReqtypeId = (int)RequestTypeEnum.Request,
                            Licno = null,
                            ActivityType = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                            .GetByCondition(a => a.Id == PreApprovalRequestModel.ActivityTypeId).Select(a => a.NameAr).FirstOrDefaultAsync(),
                            ServiceId = (int)ServiceEnum.Tourism,
                            Licowner = PreApprovalRequestModel.OwnerCompanyAr,
                            Licname = PreApprovalRequestModel.LicencesName,
                            ManagerId = ManagerId,
                            CompanyId = CompModel.Id,
                            LicenseId = prelicid,

                            Licexpiredate = null,
                            SequenceNo = SequenceNo,
                            Licreqtime = DateTime.Now,

                            Requesterid = RequesterId.Id,
                            RequestNote = null,
                            RequestStatusId = (int)RequestStatusEnum.Received,
                            RequestAttach = "Yes",
                            Licamount = PreApprovalRequestModel.Amount,
                            Licpaystatus = "0",
                            CategoryId = 1,
                            SectorId = 3,
                            AppCivilId = PreApprovalRequestModel.AppCivilId,
                            AppId= ApplicantId,
                            RequesterCivilId= PreApprovalRequestModel.SessionCivilId,
                            ManCivilId = PreApprovalRequestModel.ManCivilId,
                            //UserCivilId = PreApprovalRequestModel.UserCivilID,
                            PreApprovalNo = PreApprovalRequestModel.PreApprove,
                            LicStatusId = (int)licencesStatusEnum.Pending,
                            ActivityTypeId = PreApprovalRequestModel.ActivityTypeId,
                            LicrequestIsDeleted = false,
                            IsArchived = false,
                            LicTypeId = (int)LicTypeEnum.Company,
                            ActivityCode = PreApprovalRequestModel.ActivityCode,
                            OperationsManagerCivilId=PreApprovalRequestModel.OperationManagerCivilId,
                            SalesManagerCivilId=PreApprovalRequestModel.SalesManagerCivilId,
                            MarketingManagerCivilId=PreApprovalRequestModel.MarketingManagerCivilId,
                            OperationsManagerId=OperationManagerId,
                            SalesManagerId=SalesManagerId,
                            MarketingManagerId=MarketingManagerId,
                            
                        };

                        var _mappedRequest = _mapper.Map<MoiEserviceLicensesRequest>(ReqModel);

                        await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(ReqModel);
                        await _unitOfwork.Complete();

                        long reqid = _mappedRequest.RequestId;

                        //--------There is no building in Parks and Sailing 
                        //--------There is no Aprrovement Also
                        RequestTransaction requestTransaction = new RequestTransaction()
                        {
                            ReqStatusId = (int)RequestStatusEnum.Received,
                            ReqTypeId = (int)RequestTypeEnum.Request,
                            RequestId = reqid,
                            Notes = "إصدار أنشطة سياحية",
                            Status = RequestStatusEnum.Received.ToString(),
                            CreatedDate = DateTime.Now,
                            CreatedBy = PreApprovalRequestModel.SessionName,
                            CivilIdUser = PreApprovalRequestModel.SessionCivilId,
                            UpdatedDate=DateTime.Now,
                        };
                        await _unitOfwork.genericRepository<RequestTransaction>().Create(requestTransaction);
                        await _unitOfwork.Complete();
                        //--------Insert InTable Attachment---------------

                        await InsertAttachements(PreApprovalRequestModel.saveResponseVMs, reqid,PreApprovalRequestModel.SessionCivilId);




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
        //-------------------------تجديد ---------------
        #region التجديد
        [HttpGet]
        [Route("GetLicenseDetailsForRenew")]
        public async Task<RequestBaseVM> GetLicenseDetailsForRenew(int LicId)
        {
            var licencesSpec = new LicencesWithSpecificService(LicId, (int)ServiceEnum.Tourism);
            var licencesDetails = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
            var fileUploadConfigurationsFront = await _unitOfwork.genericRepository<AttachRule>()
                                             .GetByCondition(f => f.ViewType == "RenewRequest").ToListAsync();
            var licencesInfoDetails = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                                          .GetByCondition(r => r.ActvityTypeId == licencesDetails.ActiivityTypeId
                                          && r.ReqTypeId == (int)RequestTypeEnum.Renew
                                          && r.ServiceId == (int)ServiceEnum.Tourism).FirstOrDefaultAsync();

            var mappedLicence = _mapper.Map<LicencesInfoVM>(licencesInfoDetails);
            return new RequestBaseVM
            {
                FileUploadConfigs=_mapper.Map<List<AttachRule>,List<AddAttachmentsRulesVM>>(fileUploadConfigurationsFront),
                LicencesVM=_mapper.Map<Licence,LicencesVM>(licencesDetails),
                LicencesInfo=mappedLicence
            };


        }

      
        [HttpPost]
        [Route("PostDataRenewRequest")]
        public async Task<dynamic> PostDataRenewRequest(PreApprovalRequestApiModel model)
        {
            return await HandleCommonTourismRequest(model, RequestTypeEnum.Renew, async (requestId, reqTransId) =>
            {
                var licence = await _unitOfwork.genericRepository<Licence>()
                    .GetByCondition(l => l.LicId == model.LicId).FirstOrDefaultAsync();

                await _unitOfwork.genericRepository<LicenseRenew>().Create(new LicenseRenew
                {
                    LicenseId = model.LicId,
                    OldExpiryDate = licence?.ExpireDate,
                    RequestStatusId = (int)RequestStatusEnum.Received,
                    ServiceId = (int)ServiceEnum.Tourism,
                    ReqTransId = reqTransId // ✅ Correct assignment
                });

                await _unitOfwork.Complete();
            });
        }



        #endregion
        //-------------------------التصنيف-----------------------------
        #region التصنيف وإعادة التصنيف
        [HttpGet]
        [Route("GetClassificationForm")]
        public async Task<RequestBaseVM> GetClassificationForm(int LicId)
        {
            int categoryId = 0;

            var licencesSpec = new LicencesWithSpecificService(LicId, (int)ServiceEnum.Tourism);
            var licencesDetails = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);

            if (licencesDetails.ActiivityTypeId == (int)ActivityTypeEnum.Hotel)
                categoryId = (int)CategoryClassificationEnum.Hotel;
            else if (licencesDetails.ActiivityTypeId == (int)ActivityTypeEnum.ApartmentHotel)
                categoryId = (int)CategoryClassificationEnum.HotelApartment;
            else if (licencesDetails.ActiivityTypeId == (int)ActivityTypeEnum.Resorts)
                categoryId = (int)CategoryClassificationEnum.Resorts;

            // Load evaluation types
            var evaluationTypes = await _unitOfwork.genericRepository<TourEvaluationLookUp>().GetAllAsync();

            // Get branches and hotel classes
            var classBranches = await _unitOfwork.genericRepository<TourClassBranchLookUp>()
                .GetFilteredWithProjection(
                    filter: x => x.ClassId == categoryId,
                    selector: x => new ClassificationBranchDetail
                    {
                        BranchId = x.Id,
                        BranchName = x.Name,
                        HotelClasses = x.TourHotelClassLookUp
                            .Where(t => t.CategoryId == categoryId)
                            .Select(hotelClass => new HotelClassDetail
                            {
                                HotelClassId = hotelClass.Id,
                                HotelClassName = hotelClass.Name,
                                CategoryId = hotelClass.CategoryId,
                                Status = hotelClass.Status,
                                ClassType = hotelClass.TourClassTypeLookUp != null
                                    ? new ClassTypeDetail
                                    {
                                        ClassTypeId = hotelClass.TourClassTypeLookUp.Id,
                                        ClassTypeName = hotelClass.TourClassTypeLookUp.Name
                                    }
                                    : null,
                                Evaluations = evaluationTypes.Select(ev => new EvaluationDetail
                                {
                                    EvaluationId = ev.Id,
                                    EvaluationName = ev.Name,
                                    IsSelected = false // No pre-selection
                                }).ToList()
                            }).ToList()
                    }).ToListAsync();

            // Classification dropdown
            var classificationdrop = await _unitOfwork.genericRepository<MoiClassification>()
                .GetByCondition(c => c.ActivityTypeId == licencesDetails.ActiivityTypeId)
                .ToListAsync();

            // File upload rules
            var fileUploadConfigurationsFront = await _unitOfwork.genericRepository<AttachRule>()
                .GetByCondition(f => f.ViewType == "ClassRequest").ToListAsync();
            var licencesInfoDetails = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                                          .GetByCondition(r => r.ActvityTypeId == licencesDetails.ActiivityTypeId
                                          && (r.ReqTypeId == (int)RequestTypeEnum.Classification||r.ReqTypeId== (int)RequestTypeEnum.Classification)
                                          && r.ServiceId == (int)ServiceEnum.Tourism).FirstOrDefaultAsync();

            var mappedLicence = _mapper.Map<LicencesInfoVM>(licencesInfoDetails);

            // Return view model
            return new RequestBaseVM
            {
                ClassificationBranches = classBranches,
                Classificaion = classificationdrop.Select(r => new SelectListItem
                {
                    Value = r.ClassifyId.ToString(),
                    Text = r.ClassifiyName
                }).ToList(),
                LicencesVM = _mapper.Map<Licence, LicencesVM>(licencesDetails),
                FileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(fileUploadConfigurationsFront),
                EvaluationSelections = new Dictionary<int, int>(), // Will be populated on post
                ClassificationId = null,
                LicencesInfo=mappedLicence
            };
        }


       
        [HttpPost]
        [Route("PostDataClassificationRequest")]
        public async Task<dynamic> PostDataClassificationRequest(PreApprovalRequestApiModel model)
        {
            return await HandleCommonTourismRequest(model, RequestTypeEnum.Classification, async (requestId, requestTransactionId) =>
            {
                // Get classification name for logging/evaluation
                var classificationName = await _unitOfwork.genericRepository<MoiClassification>()
                    .GetByCondition(c => c.ClassifyId == model.ClassificationId)
                    .Select(c => c.ClassifiyName)
                    .FirstOrDefaultAsync();

                // Insert evaluation items
                if (model.EvaluationSelections != null)
                {
                    foreach (var evaluation in model.EvaluationSelections)
                    {
                        await _unitOfwork.genericRepository<TourEvaluationListHotel>().Create(new TourEvaluationListHotel
                        {
                            ClassificationName = classificationName,
                            HotelClassId = evaluation.Key,
                            EvalitemId = evaluation.Value,
                            RequestId = requestId,
                            ClassificationId = model.ClassificationId,
                            LicId = model.LicId
                        });
                    }

                    await _unitOfwork.Complete();
                }
            });
        }

        #endregion
        #region إنهاء
        //-------------------------إنهاء-----------------
        [HttpGet]
        [Route("GetLicenseDetailsForendLicences")]
        public async Task<RequestBaseVM> GetLicenseDetailsForendLicences(int LicId)
        {
            var licencesSpec = new LicencesWithSpecificService(LicId, (int)ServiceEnum.Tourism);
            var licencesDetails = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
            var fileUploadConfigurationsFront = await _unitOfwork.genericRepository<AttachRule>()
                                             .GetByCondition(f => f.ViewType == "EndLicRequest").ToListAsync();
            var EndReason = await _unitOfwork.genericRepository<MoiEserviceLicEndingReason>().GetAllAsync();
            var licencesInfoDetails = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                          .GetByCondition(r => r.ActvityTypeId == licencesDetails.ActiivityTypeId
                          && r.ReqTypeId == (int)RequestTypeEnum.EndLicences
                          && r.ServiceId == (int)ServiceEnum.Tourism).FirstOrDefaultAsync();

            var mappedLicence = _mapper.Map<LicencesInfoVM>(licencesInfoDetails);
            return new RequestBaseVM
            {
                FileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(fileUploadConfigurationsFront),
                LicencesVM = _mapper.Map<Licence, LicencesVM>(licencesDetails),
                EndingReasons = EndReason.Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.ReasonName

                }).ToList(),
                LicencesInfo=mappedLicence

            };


        }
        [HttpPost]
        [Route("PostDataEndLicencesRequest")]
        public async Task<dynamic> PostDataEndLicencesRequest(PreApprovalRequestApiModel model)
        {
            return await HandleCommonTourismRequest(model, RequestTypeEnum.EndLicences, async (requestId, requestTransactionId) =>
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
                    ServiceId = (int)ServiceEnum.Tourism,
                    LastUpdateDate = DateTime.Now
                });

                await _unitOfwork.Complete();
            });
        }

        #endregion
        #region  التنازل 
        [HttpGet]
        [Route("GetLicenseDetailsForRenouncement")]
        public async Task<RequestBaseVM> GetLicenseDetailsForRenouncement(int LicId)
        {
            var licencesSpec = new LicencesWithSpecificService(LicId, (int)ServiceEnum.Tourism);
            var licencesDetails = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
            var fileUploadConfigurationsFront = await _unitOfwork.genericRepository<AttachRule>()
                                             .GetByCondition(f => f.ViewType == "RenouncementRequest").ToListAsync();
            var EndReason = await _unitOfwork.genericRepository<MoiEserviceLicEndingReason>().GetAllAsync();
            var licencesInfoDetails = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                          .GetByCondition(r => r.ActvityTypeId == licencesDetails.ActiivityTypeId
                          && r.ReqTypeId == (int)RequestTypeEnum.Renouncement
                          && r.ServiceId == (int)ServiceEnum.Tourism).FirstOrDefaultAsync();

            var mappedLicence = _mapper.Map<LicencesInfoVM>(licencesInfoDetails);
            return new RequestBaseVM
            {
                FileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(fileUploadConfigurationsFront),
                LicencesVM = _mapper.Map<Licence, LicencesVM>(licencesDetails),
               LicencesInfo=mappedLicence

            };


        }

        [HttpPost]
        [Route("PostDataRenouncementRequest")]
        public async Task<dynamic> PostDataRenouncementRequest(PreApprovalRequestApiModel model)
        {
            // Get the full name of the old applicant
            var usernameForApplicant = await _unitOfwork.genericRepository<AspNetUser>()
                .GetByCondition(a => a.CivilId == model.AppCivilId).FirstOrDefaultAsync();
            return await HandleCommonTourismRequest(model, RequestTypeEnum.Renouncement, async (requestId, requestTransactionId) =>
            {
                

                // Insert into RenouncementTransaction
                await _unitOfwork.genericRepository<RenouncementTransaction>().Create(new RenouncementTransaction
                {
                    LicencesId = model.LicId,
                    NewCivilId = model.NewCivilId,
                    NewName = model.NewUserName,
                    OldMobile=model.OldMobile,
                    NewMobile=model.NewMobile,
                    OldEmail=model.OldEmail,
                    NewEmail=model.NewEmail,
                    OldCivilId = model.AppCivilId,
                    OldName = usernameForApplicant?.FullNameAr,
                    RequestId = (int)requestId,
                    ReqTransactionId = requestTransactionId,
                    ServiceId = (int)ServiceEnum.Tourism,
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
            var licencesSpec = new LicencesWithSpecificService(LicId, (int)ServiceEnum.Tourism);
            var licencesDetails = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
            var fileUploadConfigurationsFront = await _unitOfwork.genericRepository<AttachRule>()
                                             .GetByCondition(f => f.ViewType == "ReplacementOfLostRequest").ToListAsync();
            
            var licencesInfoDetails = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                          .GetByCondition(r => r.ActvityTypeId == licencesDetails.ActiivityTypeId
                          && r.ReqTypeId == (int)RequestTypeEnum.ReplacementOfLost
                          && r.ServiceId == (int)ServiceEnum.Tourism).FirstOrDefaultAsync();

            var mappedLicence = _mapper.Map<LicencesInfoVM>(licencesInfoDetails);
            return new RequestBaseVM
            {
                FileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(fileUploadConfigurationsFront),
                LicencesVM = _mapper.Map<Licence, LicencesVM>(licencesDetails),
                LicencesInfo=mappedLicence

            };


        }
        [HttpPost]
        [Route("PostDataReplacementOfLostRequest")]
        public async Task<dynamic> PostDataReplacementOfLostRequest(PreApprovalRequestApiModel model)
        {
            return await HandleCommonTourismRequest(model, RequestTypeEnum.ReplacementOfLost, async (requestId, requestTransactionId) =>
            {
                // Get the full name of the old applicant
                var usernameForApplicant = await _unitOfwork.genericRepository<AspNetUser>()
                    .GetByCondition(a => a.CivilId == model.AppCivilId).FirstOrDefaultAsync();

                // Insert into RenouncementTransaction
                await _unitOfwork.genericRepository<ReplacementOfLostTransaction>().Create(new ReplacementOfLostTransaction
                {
                    LicId = model.LicId,
                    
                    RequestId = (int)requestId,
                    ReqTransactionId = requestTransactionId,
                    ServiceId = (int)ServiceEnum.Tourism,
                    LastUpdateDate = DateTime.Now
                });

                await _unitOfwork.Complete();
            });
        }
        #endregion
        #region تغيير البيانات 
        //[FromQuery(Name = "queryParams")]
        [HttpGet]
        [Route("GetLicenceDetailsForChangeData")]
        public async Task<RequestBaseVM> GetLicenceDetailsForChangeData(int LicId, [FromQuery] List<int> selectedTransactionTypeIds)
        {
            var licencesSpec = new LicencesWithSpecificService(LicId, (int)ServiceEnum.Tourism);
            var licencesDetails = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
            //List<FileUploadConfigurationsFront> fileUploads = new List<FileUploadConfigurationsFront>();
            //if (TransactionTypeIds.Contains((int)TransactionTypesEnum.ChangeManager))
            //{
            //    fileUploads = await _unitOfwork.genericRepository<FileUploadConfigurationsFront>()
            //                                     .GetByCondition(f => f.ViewType == "ChangeManagerRequest").ToListAsync();
            //}
            ////if (TransactionTypeIds.Contains((int)TransactionTypesEnum.ReplacementOfLost))
            ////{
            ////    fileUploads = await _unitOfwork.genericRepository<FileUploadConfigurationsFront>()
            ////                                     .GetByCondition(f => f.ViewType == "ReplacementOfLostRequest").ToListAsync();
            ////}
            // if (TransactionTypeIds.Contains((int)TransactionTypesEnum.ChangeCompaneName))
            //{
            //    fileUploads = await _unitOfwork.genericRepository<FileUploadConfigurationsFront>()
            //                                     .GetByCondition(f => f.ViewType == "ChangeCompaneNameRequest").ToListAsync();
            //}
            // if (TransactionTypeIds.Contains((int)TransactionTypesEnum.ChangeAddress))
            //{
            //    fileUploads = await _unitOfwork.genericRepository<FileUploadConfigurationsFront>()
            //                                     .GetByCondition(f => f.ViewType == "ChangeAddressRequest").ToListAsync();
            //}
            // if (TransactionTypeIds.Contains((int)TransactionTypesEnum.ChangeLicencesName))
            //{
            //    fileUploads = await _unitOfwork.genericRepository<FileUploadConfigurationsFront>()
            //                                     .GetByCondition(f => f.ViewType == "ChangeLicencesNameRequest").ToListAsync();
            //}
            var viewTypeMapping = new Dictionary<TransactionTypesEnum, string>
{
    { TransactionTypesEnum.ChangeManager, "ChangeManagerRequest" },
    { TransactionTypesEnum.ChangeCompaneName, "ChangeCompaneNameRequest" },
    { TransactionTypesEnum.ChangeAddress, "ChangeAddressRequest" },
    { TransactionTypesEnum.ChangeLicencesName, "ChangeLicencesNameRequest" },
    // Uncomment if needed:
    // { TransactionTypesEnum.ReplacementOfLost, "ReplacementOfLostRequest" }
};

            var fileUploadsDict = new Dictionary<string, AttachRule>();

            foreach (var transId in selectedTransactionTypeIds.Distinct())
            {
                if (viewTypeMapping.TryGetValue((TransactionTypesEnum)transId, out var viewType))
                {
                    var uploads = await _unitOfwork.genericRepository<AttachRule>()
                                                   .GetByCondition(f => f.ViewType == viewType)
                                                   .ToListAsync();

                    foreach (var upload in uploads)
                    {
                        // Add only unique FieldNames
                        if (!string.IsNullOrEmpty(upload.FieldName) && !fileUploadsDict.ContainsKey(upload.FieldName))
                        {
                            fileUploadsDict[upload.FieldName] = upload;
                        }
                    }
                }
            }
            var licencesInfoDetails = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                          .GetByCondition(r => r.ActvityTypeId == licencesDetails.ActiivityTypeId
                          && r.ReqTypeId == (int)RequestTypeEnum.ChangeData
                          && r.ServiceId == (int)ServiceEnum.Tourism).FirstOrDefaultAsync();
            var baseFee = licencesInfoDetails?.FixedFees ?? 0;

            // Calculate total based on how many distinct transaction types were selected
            var totalFee = baseFee * selectedTransactionTypeIds.Distinct().Count();

            // Update the mapped licence info before returning
           
            var mappedLicence = _mapper.Map<LicencesInfoVM>(licencesInfoDetails);
            mappedLicence.FixedFees = totalFee;
            List<AttachRule> fileUploads = fileUploadsDict.Values.ToList();
            return new RequestBaseVM
            {
                FileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(fileUploads),
                LicencesVM = _mapper.Map<Licence, LicencesVM>(licencesDetails),
                SelectedTransactionTypeIds= selectedTransactionTypeIds,
                LicencesInfo=mappedLicence

            };


        }
        [HttpPost]
        [Route("PostDataChangeDataRequest")]
        public async Task<dynamic> PostDataChangeDataRequest(PreApprovalRequestApiModel model)
        {
            using var dbTransaction =  _unitOfwork.BeginTransaction();

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

                await HandleCommonTourismRequest(model, RequestTypeEnum.ChangeData, async (requestId, requestTransactionId) =>
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

                            case TransactionTypesEnum.ChangeCompaneName:
                                await HandleChangeCompanyName(model, requestId);
                                break;

                            default:
                                throw new Exception($"نوع التعديل غير معروف: {transTypeId}");
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

        private async Task HandleChangeManager(PreApprovalRequestApiModel model, long requestId)
        {
            try { 
            var Trans = new Domain.Entities.Transaction
            {
                RequestId = requestId,
                Commited = false,
                LicenseId = model.LicId,
                ReqStatusId = (int)RequestStatusEnum.Received,
                RequestDate = DateTime.Now,
                TransTypeId = (int)TransactionTypesEnum.ChangeManager,
                UsercivilId = model.SessionCivilId,
                TransDate = DateTime.Now,
                ServiceId = (int)ServiceEnum.Tourism,
                Notes = "تغيير المدير"
            };
            await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(Trans);
            await _unitOfwork.Complete();

            var transaction = new TchangeManager
            {
                RequestId = (int)requestId,
                TransactionId = Trans.Id,
                OldManagerId = model.ManId??0,               
                ServiceId = (int)ServiceEnum.Tourism,
                ManagerNewcivilid = model.NewManCivilId,
                ManagerNewname1 = model.NewManagerName,
                NewMobile = model.NewManagerMobile,
                NewEmail = model.NewManagerEmail,
                OldEmail = model.OldManagerEmail,
                OldMobile = model.OldManagerMobile,
                ManagerOldcivilid=model.OldManCivilId,
                ManagerOldname = model.OldManagerName,                
                LastUpdateDate = DateTime.Now
            };
            await _unitOfwork.genericRepository<TchangeManager>().Create(transaction);
        }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HandleChangeLicenceName: " + ex.Message);
                throw; // rethrow or log more deeply
            }
}
        private async Task HandleChangeAddress(PreApprovalRequestApiModel model, long requestId)
        {

            try { 
          var Trans=  new Domain.Entities.Transaction
            {
                RequestId = requestId,
                Commited = false,
                LicenseId = model.LicId,
                ReqStatusId = (int)RequestStatusEnum.Received,
                RequestDate = DateTime.Now,
                TransTypeId = (int)TransactionTypesEnum.ChangeAddress,
                UsercivilId = model.SessionCivilId,
                TransDate = DateTime.Now,
                ServiceId = (int)ServiceEnum.Tourism,             
                Notes = "تغيير العنوان"
            };
            await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(Trans);
            await _unitOfwork.Complete();
                
                int addressId = 0;
                var oldCompany = new Company();
                if (model.ActivityTypeId == (int)ActivityTypeEnum.ApartmentHotel
                            || model.ActivityTypeId == (int)ActivityTypeEnum.Hotel
                            || model.ActivityTypeId == (int)ActivityTypeEnum.Resorts)
                {
                    var companySpec = new CompanyWithSpec(model.CompanyId ?? 0, (int)ServiceEnum.Tourism);
                    oldCompany = await _unitOfwork.genericRepository<Company>().GetByIdWithSpec(companySpec);
                }else
                {
                    var companySpec = new CompanyWithSpec(model.BuildingId??0, (int)ServiceEnum.Tourism);
                    oldCompany = await _unitOfwork.genericRepository<Company>().GetByIdWithSpec(companySpec);
                }
                    await _unitOfwork.genericRepository<AddressChangeTransaction>().Create(new AddressChangeTransaction
            {
                RequestId = (int)requestId,
                TransactionId=Trans.Id,
                ServiceId = (int)ServiceEnum.Tourism,
                AalliNoNew = model.NewAaliNumber,
                AalliNoOld = oldCompany.AddressNavigation.AalliNo,
                NewArea = model.NewArea,
                OldArea = oldCompany.AddressNavigation.Area,
                NewBlock = model.NewBlockNo,
                OldBlock = oldCompany.AddressNavigation.BlockArabic,
                NewStreet = model.NewStreet,
                OldStreet = oldCompany.AddressNavigation.StreetArabic,
                NewBuildingNo = model.NewBuildingNo,
                OldBuildingNo = oldCompany.AddressNavigation.BuildingNo,
                OldBuildingName = oldCompany.AddressNavigation.BuildingName,
                AreaSizeNew=model.NewAreaSize,
                AreaChartNoNew=model.NewAreaChartNo,
                NewBuildingName = model.NewBuildingName,
                NewFloor = model.NewFloorNo,
                NewUnitNo = model.NewUnitNo,
                NewGovernorate = model.NewGovernrate,
                OldGovernorate = oldCompany.AddressNavigation.GovernorateArabic,
                LastUpdateDate = DateTime.Now,
                AddId=model.AddressId,
                LicenceId=model.LicId,
                
            });
        }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HandleChangeLicenceName: " + ex.Message);
                throw; // rethrow or log more deeply
            }
}
        private async Task HandleChangeCompanyName(PreApprovalRequestApiModel model, long requestId)
        {
            try { 

            var Trans = new Domain.Entities.Transaction
            {
                RequestId = requestId,
                Commited = false,
                LicenseId = model.LicId,
                ReqStatusId = (int)RequestStatusEnum.Received,
                RequestDate = DateTime.Now,
                TransTypeId = (int)TransactionTypesEnum.ChangeCompaneName,
                UsercivilId = model.SessionCivilId,
                TransDate = DateTime.Now,
                ServiceId = (int)ServiceEnum.Tourism,
                Notes = "تغيير إسم الشركة"
            };
            await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(Trans);
            await _unitOfwork.Complete();

            await _unitOfwork.genericRepository<CompanyNameChangeTransaction>().Create(new CompanyNameChangeTransaction
            {
                RequestId = (int)requestId,
                TransactionId = Trans.Id,
                ServiceId = (int)ServiceEnum.Tourism,
               NewCompanyNameDir=model.NewDirCompanyAr,
               OldCompnayNameDir=model.OldDirCompanyAr,
               NewCompanyNameOwner=model.NewOwnerCompanyAr,
               OldCompnayNameOwner=model.OldOwnerCompanyAr,
                LastUpdateDate = DateTime.Now,
                LicenceId=model.LicId,
                CompId = model.CompanyId,
                
            });
        }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HandleChangeLicenceName: " + ex.Message);
                throw; // rethrow or log more deeply
            }
    //await _unitOfwork.Complete();
}
        private async Task HandleChangeLicenceName(PreApprovalRequestApiModel model, long requestId)
        {

            try
            {
                var Trans = new Domain.Entities.Transaction
                {
                    RequestId = requestId,
                    Commited = false,
                    LicenseId = model.LicId,
                    ReqStatusId = (int)RequestStatusEnum.Received,
                    RequestDate = DateTime.Now,
                    TransTypeId = (int)TransactionTypesEnum.ChangeLicencesName,
                    UsercivilId = model.SessionCivilId,
                    TransDate = DateTime.Now,
                    ServiceId = (int)ServiceEnum.Tourism,
                    Notes = "تغيير إسم الرخصة"
                };

                await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(Trans);
                await _unitOfwork.Complete();

                await _unitOfwork.genericRepository<LicencesNameChangeTransaction>().Create(new LicencesNameChangeTransaction
                {
                    RequestId = (int)requestId,
                    TransactionId = Trans.Id,
                    ServiceId = (int)ServiceEnum.Tourism,
                    LicencesNameNew = model.NewLicencesName,
                    LicencesNameOld = model.OldLicencesName,
                    LicencesId=model.LicId
                });

                await _unitOfwork.Complete(); // You MUST commit after the second Create
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in HandleChangeLicenceName: " + ex.Message);
                throw; // rethrow or log more deeply
            }
        }
        #endregion

        #region Common
        private async Task<ErrorMessage> HandleCommonTourismRequest(
       PreApprovalRequestApiModel model,
       RequestTypeEnum requestType,
       Func<long, int?, Task> handleExtras = null!)
        {
            //using var dbTransaction = _unitOfwork.BeginTransaction();
            try
            {
                //string requesterId = model.accountTypeId switch
                //{
                //    "100" => model.AppId.ToString(),
                //    "300" => model.MandoobId,
                //    _ => null
                //};

                var licence = await _unitOfwork.genericRepository<Licence>()
                                  .GetByCondition(l => l.LicId == model.LicId)
                                  .FirstOrDefaultAsync();
                var requesterId = await _unitOfwork.genericRepository<AspNetUser>()
                    .GetByCondition(a => a.CivilId == model.SessionCivilId).FirstOrDefaultAsync();
                var reqModel = new MoiEserviceLicensesRequest
                {
                    Reqno = model.reqno,
                    ReqtypeId = (int)requestType,
                    Licno = model.LicNo,
                    ActivityType = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                        .GetByCondition(a => a.Id == model.ActivityTypeId).Select(a => a.NameAr).FirstOrDefaultAsync(),
                    ServiceId = (int)ServiceEnum.Tourism,
                    Licowner = model.OwnerCompanyAr,
                    Licname = model.LicencesName,
                    ManagerId = model.ManId,
                    
                    SalesManagerId=model.SalesManagerId,
                    MarketingManagerId=model.MarketingManagerId,
                    OperationsManagerId=model.OperationManagerId,
                    MarketingManagerCivilId=model.MarketingManagerCivilId,
                    SalesManagerCivilId=model.SalesManagerCivilId,
                    OperationsManagerCivilId= model.OperationManagerCivilId,
                    Licexpiredate=model.ExpireDate,
                    LicIssuedate=model.IssueDate,
                    CompanyId = model.CompanyId,
                    LicenseId = model.LicId,
                    //BuildingId = model.BuildingId,
                    PreApprovalId = model.PreApproveId,
                    SequenceNo = model.SequenceNo,
                    Licreqtime = DateTime.Now,
                    Requesterid = requesterId.Id,
                    RequestStatusId = (int)RequestStatusEnum.Received,
                    RequestAttach = "Yes",
                    Licamount = model.Amount,
                    Licpaystatus = "0",
                    CategoryId = 1,
                    SectorId = 3,
                    AppCivilId = model.AppCivilId,
                    AppId=model.AppId,
                    RequesterCivilId=model.SessionCivilId,
                    ManCivilId = model.ManCivilId,
                    //UserCivilId = model.UserCivilID,
                    PreApprovalNo = model.PreApprove,
                    LicStatusId = (int)licencesStatusEnum.Pending,
                    ActivityTypeId = model.ActivityTypeId,
                    LicrequestIsDeleted = false,
                    IsArchived = false,
                    LicTypeId = (int)LicTypeEnum.Company,
                    ActivityCode = model.ActivityCode
                };

                await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(reqModel);
                await _unitOfwork.Complete();

                long requestId = reqModel.RequestId;

                int requestTransactionId = 0;

                if(requestType != RequestTypeEnum.ChangeData){
                    var transaction = new RequestTransaction
                    {
                        ReqStatusId = (int)RequestStatusEnum.Received,
                        LicenseId= model.LicId,
                        ReqTypeId = (int)requestType,
                        RequestId = requestId,
                        Notes = requestType.ToString(),
                        Status = RequestStatusEnum.Received.ToString(),
                        CreatedDate = DateTime.Now,
                        CreatedBy = model.SessionName,
                        CivilIdUser = model.SessionCivilId,
                        ServiceId = (int)ServiceEnum.Tourism,
                        UpdatedDate = DateTime.Now
                    };

                    await _unitOfwork.genericRepository<RequestTransaction>().Create(transaction);
                    await _unitOfwork.Complete();

                    requestTransactionId = transaction.Id;
                }


             

                if (handleExtras != null)
                    await handleExtras.Invoke(requestId, requestTransactionId); // <-- pass both IDs

                await InsertAttachements(model.saveResponseVMs, requestId,model.SessionCivilId);

               // dbTransaction.Commit();
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




        #endregion

        [HttpGet]
        [Route("GetFilesForRequest")]
        public async Task<IEnumerable<FileUploadConfigVM>> GetFilesForRequest()
        {
            var files = await _unitOfwork.genericRepository<FileUploadConfigurationsFront>().GetByCondition(f => f.ViewType == "Request").ToListAsync();
            return _mapper.Map<IEnumerable<FileUploadConfigurationsFront>, IEnumerable<FileUploadConfigVM>>(files);
        }

        [HttpPost]
        [Route("Request/InsertAttachements")]
        public async Task<dynamic> InsertAttachements(List<FileSaveResponseVM> fileSaveResponseVMs, long reqid,string SessionCivilid)
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
                        ServiceId = (int)ServiceEnum.Tourism,
                        AttachFlag = file.FileName,
                        IsLatest=true,
                        UploadedBy=SessionCivilid,
                        UploadedDate=DateTime.Now,
                        IsDeleted=false,
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
              
               

                var result = await _updateDataService.InsertUpdateAttachementToTable(model, (int)ServiceEnum.Tourism);

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
        private async Task SendStatusUpdateEmail(MoiEserviceLicensesRequest request, string ReqStatus, string requestType, DateTime? ExpireDate, DateTime? IssueDate)
        {
            try
            {
                var UserInformation = await GetUserDetailsByCivilId(request.AppCivilId);
                // Prepare dynamic email content
                var placeholders = new Dictionary<string, string>
        {
            { "{customer_name}", UserInformation.FullNameAr },
            { "{request_no}", request.Reqno },
            { "{phone_no}", UserInformation.Mobile },
            { "{request_type}", requestType },
            { "{request_status}", ReqStatus },
            { "{request_date}", request.Licreqtime?.ToString("yyyy-MM-dd") },
             { "{request_licno}", request.Licno ?? "N/A" },
           { "{request_lictype}", request.LicenceTypeNavigation.NameAr ?? "N/A" },
           { "{request_owner}", request.Licowner ?? "N/A" },

            { "{issue_date}", IssueDate?.ToString("yyyy-MM-dd") },
            { "{expire_date}",ExpireDate?.ToString("yyyy-MM-dd") },
        };

                // Prepare the email body using the template
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "RequestStatusTemplate.html");
                string emailBody = _emailService.PrepareEmailBody(templatePath, placeholders);
                // Remove rows dynamically based on conditions
                if (string.IsNullOrEmpty(request.Licno))
                {
                    emailBody = RemoveHtmlRow(emailBody, "conditional-license-no");
                }
                if (!IssueDate.HasValue)
                {
                    emailBody = RemoveHtmlRow(emailBody, "conditional-license-issue-date");
                }
                if (!ExpireDate.HasValue)
                {
                    emailBody = RemoveHtmlRow(emailBody, "conditional-license-expire-date");
                }

                // Send the email to the user
                bool isEmailSent = await _emailService.SendEmail(UserInformation.Email, "Request Status Update", emailBody);
                if (isEmailSent)
                {
                    Console.WriteLine("Email sent successfully.");
                }
                else
                {
                    Console.WriteLine("Email failed.");
                }
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
        private string RemoveHtmlRow(string htmlContent, string rowId)
        {
            string pattern = $@"<tr id=""{rowId}"">.*?</tr>";
            return System.Text.RegularExpressions.Regex.Replace(htmlContent, pattern, string.Empty, System.Text.RegularExpressions.RegexOptions.Singleline);
        }
        [HttpGet]
        [Route("GetUserDetailsByCivilId")]
        public async Task<dynamic> GetUserDetailsByCivilId(string civilId)
        {
            var userdetails = await _unitOfwork.genericRepository<AspNetUser>()
                          .GetFilteredWithProjection(
                           filter: x => x.CivilId == civilId,
                           selector: x => new
                           {
                               CivilId = x.CivilId,
                               UserName = x.UserName,
                               PhoneNumber = x.PhoneNumber,
                               Email = x.Email,
                               FullNameAr = x.FullNameAr,
                               Mobile = x.Mobile
                           }).FirstOrDefaultAsync();

            if (userdetails != null)
            {
                return userdetails;
            }
            else
            {
                return new ErrorMessage()
                {
                    Error = true,
                    Status = "Failure",
                    Message = "No data Found",
                };
            }

        }
        #region Payment

        [HttpPost]
        [Route("PostTourismPayment")]
        public async Task<dynamic> PostTourismPayment(PaymentRequestModel TourismPaymentModel)
        {
            string error = string.Empty;
            try
            {
                using (IDbContextTransaction dbTransaction = _unitOfwork.BeginTransaction())
                {
                    try
                    {
                        MoiEserviceRequestPaymentDetail ReqPayment = new MoiEserviceRequestPaymentDetail()
                        {
                            RequestId = TourismPaymentModel.reqID,
                            AppCivilId = TourismPaymentModel.ApplicantCivilId,
                            LicenceId = Convert.ToInt32(TourismPaymentModel.LicId),
                            TotalAmount = TourismPaymentModel.ServiceAmount,
                            Payed = 0,
                            ServiceId=(int) ServiceEnum.Tourism,
                            UserId=TourismPaymentModel.ApplicantId
                        };
                        var _mappedPayment = _mapper.Map<MoiEserviceRequestPaymentDetail>(ReqPayment);

                       await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>().Create(ReqPayment);
                        await _unitOfwork.Complete();


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
        [Route("UpdatePayment")]
        public async Task<dynamic> UpdatePayment([FromBody] PaymentResponse PaymentResponse)
        {
            string error = string.Empty;
            try
            {

                //--------------- Start trunsaction -----------------------------------
                using (IDbContextTransaction dbTransaction = _unitOfwork.BeginTransaction())
                {
                    try
                    {
                        //------- Update in Payment table ------------
                        int reqidPayTbl = int.Parse(PaymentResponse.MerchantRequestID);
                        MoiEserviceRequestPaymentDetail UpdatePayTable = _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>()
                            .GetByCondition(p => p.RequestId == reqidPayTbl).FirstOrDefault();
                        if (UpdatePayTable != null)
                        {
                            UpdatePayTable.PaymentId = PaymentResponse.PaymentID;
                            UpdatePayTable.Result = PaymentResponse.Result;
                            UpdatePayTable.TranId = PaymentResponse.TranID;
                            UpdatePayTable.Ref = PaymentResponse.Ref;
                            UpdatePayTable.Postdate = PaymentResponse.Postdate;
                            UpdatePayTable.Auth = PaymentResponse.Auth;
                            UpdatePayTable.TrackId = PaymentResponse.TrackID;
                            UpdatePayTable.Payed = PaymentResponse.Payed;
                            UpdatePayTable.Status = PaymentResponse.Status;
                        }

                        var _mappedUpdatePay = _mapper.Map<MoiEserviceRequestPaymentDetail>(UpdatePayTable);
                        await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>().Update(UpdatePayTable);
                        await _unitOfwork.Complete();

                        if (PaymentResponse.Payed == 1)
                        {
                            long reqId = long.Parse(PaymentResponse.MerchantRequestID);
                            //------- Update in Request table ------------
                            MoiEserviceLicensesRequest UpdateReqModel = _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().GetByCondition(c => c.RequestId == reqId).FirstOrDefault();
                            if (UpdateReqModel != null)
                            {
                                UpdateReqModel.RequestStatusId = (int)RequestStatusEnum.FinalLicenseIssued;
                                UpdateReqModel.Licpaystatus = "1";
                            }

                            var _mappedUpdateRequest = _mapper.Map<MoiEserviceLicensesRequest>(UpdateReqModel);
                            await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Update(UpdateReqModel);
                            await _unitOfwork.Complete();
                        }




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


        [HttpGet]
        [Route("GetAllRequestsForUser/{CivilId}")]
        public async Task<IEnumerable<RequestVM>> GetAllRequestsForUser(string CivilId)
        {
            var SpecRequest = new RequestWithSpecificService(CivilId, (int)ServiceEnum.Tourism);
            var AllRequest = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                            .GetTableWithSpecService(SpecRequest);

            return _mapper.Map<IEnumerable<MoiEserviceLicensesRequest>, IEnumerable<RequestVM>>(AllRequest);


        }

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
                           .GetByCondition(x => x.AttachRequestid == id&&x.IsLatest==true).ToListAsync();
            var UserApplicant = await _unitOfwork.genericRepository<AspNetUser>()
                          .GetByCondition(x => x.CivilId == RequestDetails.AppCivilId).FirstOrDefaultAsync();
            var licencesInfo = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                .GetByCondition(m => m.ActvityTypeId == RequestDetails.ActivityTypeId
                && m.ReqTypeId == RequestDetails.ReqtypeId
                && m.ServiceId == RequestDetails.ServiceId).FirstOrDefaultAsync();
           
            //var UserApplicant = await _unitOfwork.genericRepository<Person>()
            //  .GetByCondition(x => x.CivilId == RequestDetails.AppCivilId).FirstOrDefaultAsync();
            return new RequestFrontVM
            {
                RequestVM = _mapper.Map<MoiEserviceLicensesRequest, RequestVM>(RequestDetails),
                PaymentDetailsVM = _mapper.Map<MoiEserviceRequestPaymentDetail, PaymentDetailsVM>(PaymentPerRequest),
                attachVMs = _mapper.Map<IEnumerable<MoiEserviceRequestsAttach>, IEnumerable<AttachVM>>(AttachmenttRequest),
                AspnetUserVM = _mapper.Map<AspNetUser, AspnetUserVM>(UserApplicant),
                LicencesInfoVM=_mapper.Map<MoiEserviceLicenseInfo,LicencesInfoVM>(licencesInfo)

            };

        }

        [HttpGet]
        [Route("GetPreApprovalDetails/{id}")]
        public async Task<RequestFrontVM> GetPreApprovalDetails(long id)
        {
            string PreApprovalExist = "";
            var SpecRequest = new RequestWithSpecificService((int)id, (int)ServiceEnum.Tourism, false);
            var RequestDetails = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                              .GetByIdWithSpec(SpecRequest);
            if (RequestDetails.PreApprovalNo != null)
            {
                var ChechPreApprovalusedorNot = await _unitOfwork.genericRepository<Licence>()
                                      .GetByCondition(l => l.PreApprovalNo == RequestDetails.PreApprovalNo).FirstOrDefaultAsync();
                if (RequestDetails.RequestStatusId != (int)RequestStatusEnum.RequestDeclined)
                {
                    if (ChechPreApprovalusedorNot != null)
                    {
                        PreApprovalExist = "الموافقة المبدئية مستخدمة";
                    }
                    else
                    {
                        PreApprovalExist = "الموافقة المبدئية غير مستخدمة";
                    }
                }
                else
                {
                    PreApprovalExist = "الموافقة المبدئية لم يتم الموافقة عليها";
                }

            }
            else
            {
                PreApprovalExist = "الموافقة المبدئية لم يتم الموافقة عليها";
            }
            var AttachmenttRequest = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>()
                           .GetByCondition(x => x.AttachRequestid == id).ToListAsync();
            var UserApplicant = await _unitOfwork.genericRepository<AspNetUser>()
                          .GetByCondition(x => x.CivilId == RequestDetails.AppCivilId).FirstOrDefaultAsync();

            return new RequestFrontVM
            {
                RequestVM = _mapper.Map<MoiEserviceLicensesRequest, RequestVM>(RequestDetails),
                attachVMs = _mapper.Map<IEnumerable<MoiEserviceRequestsAttach>, IEnumerable<AttachVM>>(AttachmenttRequest),
               PreApprovalExist=PreApprovalExist,
                AspnetUserVM = _mapper.Map<AspNetUser, AspnetUserVM>(UserApplicant)

            };

        }
        [HttpGet]
        [Route("GetLicenseDetails")]
        public async Task<LicenceDetailsVM> GetLicenseDetails(int id)
        {
            // Try to get license details
            var licencesSpec = new LicencesWithSpecificService(id, (int)ServiceEnum.Tourism);
            var licencesDetails = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
            bool isRenewable = false;
            MoiPreApprovement? preApprovDetails = null;
            string applicantCivilId = string.Empty;
            List<int?> HaveRequestInSameRequestType = new List<int?>();

            if (licencesDetails == null)
            {
                // Try to get pre-approval instead
                var preApproveSpec = new PreApprovementWithSpec(id, true);
                preApprovDetails = await _unitOfwork
                    .genericRepository<MoiPreApprovement>()
                    .GetByIdWithSpec(preApproveSpec);

                applicantCivilId = preApprovDetails?.ApplicantCivilId;
            }
            else
            {
                applicantCivilId = licencesDetails.ApplicantCivilId;
            }

            // Get the applicant based on whichever civil ID we found
            var applicant = !string.IsNullOrEmpty(applicantCivilId)
                ? await _unitOfwork.genericRepository<Person>()
                    .GetByCondition(u => u.CivilId == applicantCivilId)
                    .FirstOrDefaultAsync()
                : null;

            List<long> requestIds = new List<long>();

            if (licencesDetails != null)
            {
                // Get RequestId from `licencesDetails`
                var RequestForLicences = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                    .GetByCondition(r => r.LicenseId == licencesDetails.LicId).ToListAsync();
                requestIds = RequestForLicences.Select(r => r.RequestId).ToList();

                if (licencesDetails?.ExpireDate != null)
                {
                    var remainingTime = licencesDetails.ExpireDate.Value - DateTime.Now;
                    isRenewable = remainingTime.TotalDays <= 30;
                }
            }
            else if (preApprovDetails != null)
            {
                // Get RequestId from `preApprovDetails`
                var RequestForPreApprov = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                    .GetByCondition(r => r.LicenseId == preApprovDetails.PreAppId).ToListAsync();
                requestIds = RequestForPreApprov.Select(r => r.RequestId).ToList();
            }
            

             HaveRequestInSameRequestType = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                        .GetByCondition(r => r.LicenseId == id&&
                        r.AppCivilId== applicantCivilId
                        && r.RequestStatusId!=(int)RequestStatusEnum.FinalLicenseIssued
                        && r.RequestStatusId != (int)RequestStatusEnum.RequestDeclined)
                        .Select(r=>r.ReqtypeId).ToListAsync();
            // Retrieve all attachments for the requestIds
            var AttachmentForLicences = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>()
                .GetByCondition(a => requestIds.Contains(a.AttachRequestid.Value))
                .ToListAsync();
            var allowedTypes = new[]
 {
    TransactionTypesEnum.ChangeCompaneName,
    
    TransactionTypesEnum.ChangeAddress,
    TransactionTypesEnum.ChangeManager,
   
    TransactionTypesEnum.ChangeLicencesName
            };

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
                PreApprovementVM = preApprovDetails != null
                    ? _mapper.Map<MoiPreApprovement, PreApprovementVM>(preApprovDetails)
                    : null,
                attachmentVM = AttachmentForLicences != null
                ? _mapper.Map<IEnumerable<MoiEserviceRequestsAttach>, IEnumerable<AttachVM>>(AttachmentForLicences) : null,
                IsRenewable = isRenewable,
                RequestTypesId= HaveRequestInSameRequestType
            };
        }

        [HttpGet]
        [Route("GetPreApprovalLicDetails")]
        public async Task<LicenceDetailsVM> GetPreApprovalLicDetails(int id)
        {
            // Try to get license details
            var licencesSpec = new LicencesWithSpecificService(id, (int)ServiceEnum.Tourism);
            var licencesDetails = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
            bool isRenewable = false;
            MoiPreApprovement? preApprovDetails = null;
            string applicantCivilId = string.Empty;
            List<int?> HaveRequestInSameRequestType = new List<int?>();

            if (licencesDetails == null)
            {
                // Try to get pre-approval instead
                var preApproveSpec = new PreApprovementWithSpec(id, true);
                preApprovDetails = await _unitOfwork
                    .genericRepository<MoiPreApprovement>()
                    .GetByIdWithSpec(preApproveSpec);

                applicantCivilId = preApprovDetails?.ApplicantCivilId;
            }
            else
            {
                applicantCivilId = licencesDetails.ApplicantCivilId;
            }

            // Get the applicant based on whichever civil ID we found
            var applicant = !string.IsNullOrEmpty(applicantCivilId)
                ? await _unitOfwork.genericRepository<Person>()
                    .GetByCondition(u => u.CivilId == applicantCivilId)
                    .FirstOrDefaultAsync()
                : null;

            List<long> requestIds = new List<long>();

            if (licencesDetails != null)
            {
                // Get RequestId from `licencesDetails`
                var RequestForLicences = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                    .GetByCondition(r => r.LicenseId == licencesDetails.LicId).ToListAsync();
                requestIds = RequestForLicences.Select(r => r.RequestId).ToList();

                if (licencesDetails?.ExpireDate != null)
                {
                    var remainingTime = licencesDetails.ExpireDate.Value - DateTime.Now;
                    isRenewable = remainingTime.TotalDays <= 30;
                }
            }
            else if (preApprovDetails != null)
            {
                // Get RequestId from `preApprovDetails`
                var RequestForPreApprov = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                    .GetByCondition(r => r.PreApprovalId == preApprovDetails.PreAppId).ToListAsync();
                requestIds = RequestForPreApprov.Select(r => r.RequestId).ToList();
            }


            HaveRequestInSameRequestType = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                       .GetByCondition(r => r.LicenseId == id &&
                       r.AppCivilId == applicantCivilId
                       && r.RequestStatusId != (int)RequestStatusEnum.FinalLicenseIssued
                       && r.RequestStatusId != (int)RequestStatusEnum.RequestDeclined)
                       .Select(r => r.ReqtypeId).ToListAsync();
            // Retrieve all attachments for the requestIds
            var AttachmentForLicences = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>()
                .GetByCondition(a => requestIds.Contains(a.AttachRequestid.Value))
                .ToListAsync();
            var allowedTypes = new[]
 {
    TransactionTypesEnum.ChangeCompaneName,

    TransactionTypesEnum.ChangeAddress,
    TransactionTypesEnum.ChangeManager,

    TransactionTypesEnum.ChangeLicencesName
            };

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
                PreApprovementVM = preApprovDetails != null
                    ? _mapper.Map<MoiPreApprovement, PreApprovementVM>(preApprovDetails)
                    : null,
                attachmentVM = AttachmentForLicences != null
                ? _mapper.Map<IEnumerable<MoiEserviceRequestsAttach>, IEnumerable<AttachVM>>(AttachmentForLicences) : null,
           
          
            };
        }

        [HttpGet]
        [Route("GetAllLicencesForUser/{CivilId}")]
        public async Task<LicenceDetailsForUserVM> GetAllLicencesForUser(string CivilId)
        {
            int LicStatus = (int)licencesStatusEnum.Released;
            var licencesSpec = new LicencesWithSpecificService(CivilId, (int)ServiceEnum.Tourism, LicStatus);
            var licencesDetails = await _unitOfwork.genericRepository<Licence>().GetTableWithSpec(licencesSpec);

            var licencesPreAproveSpec = new PreApprovementWithSpec(CivilId, LicStatus);
            var licencesPre = await _unitOfwork.genericRepository<MoiPreApprovement>().GetTableWithSpec(licencesPreAproveSpec);

            return new LicenceDetailsForUserVM
            {
                PreApprovementVM = _mapper.Map<IEnumerable<MoiPreApprovement>, IEnumerable<PreApprovementVM>>(licencesPre),
                LicencesVM = _mapper.Map<IEnumerable<Licence>, IEnumerable<LicencesVM>>(licencesDetails),

            };
        }

    }
}
