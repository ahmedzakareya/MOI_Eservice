using AutoMapper;
using Business.Enums;
using Business.Interfaces;
using Business.ModelWithSpecification;
using Business.Repository;
using Business.ViewModel;
using Business.ViewModel.Dynamic;
using Business.ViewModel.HomePage;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Business.ViewModel.Account;
using Business.ViewModel.Tourism;

namespace MOINFO_API.Controllers
{
    [Route("HomePage")]
    public class HomePageApiController : BaseController
    {

        private readonly IUnitOfwork _unitOfWork;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;

        private readonly ILogger<HomePageApiController> _logger;



        public HomePageApiController(IUnitOfwork unitOfwork, ILogger<HomePageApiController> logger, UserManager<AspNetUser> userManager, IMapper mapper, HttpClient httpClient)
        {
            _unitOfWork = unitOfwork;
            _userManager = userManager;
            _mapper = mapper;
            _httpClient = httpClient;
            _logger = logger;   
        }
        #region HomePage
        [HttpGet]
        [Route("GetActivityTypes")]
        public async Task<IActionResult> GetActivityTypes()
        {
            var activities = await _unitOfWork.genericRepository<ActivityTypesLookup>()
                .GetFilteredWithProjection(
                    selector: a => new { a.Id, a.NameAr,a.EserviceId,a.ServiceId,a.ActivityCode })
                .ToListAsync();
            return Ok(activities);
        }

        [HttpGet]
        [Route("services")]
        public async Task<IActionResult> GetServices()
        {
            var services = await _unitOfWork.genericRepository<Eservice>()
                .GetFilteredWithProjection(
                 selector: x => new { x.EserviceName, x.ServiceId, x.EserviceNameAr, x.Url })
                .ToListAsync();
            return Ok(services);
        }
        [HttpGet]
        [Route("GetActivityWithService")]
        public async Task<IActionResult> GetActivityWithService()
        {
            var activityWithService = await _unitOfWork.genericRepository<ActivityTypesLookup>()
                .GetFilteredWithProjection(
                    selector: a => new EserviceActvityTypeModel
                    {
                       Id= a.Id,
                        NameAr = a.NameAr,
                       EserviceId= a.EserviceId,
                        ServiceId = a.ServiceId,
                       ActivityCode= a.ActivityCode,
                        EserviceName = a.Eservice.EserviceNameAr,
                        EserviceUrl = a.Eservice.Url
                    },
                    includes: a => a.Eservice)
                .ToListAsync();

            return Ok(activityWithService); // no mapping needed
        }
        [HttpGet]
        [Route("GetCardActivityList")]
        public async Task<IActionResult> GetCardActivityList()
        {
            // Load master data
            var activities = await _unitOfWork.genericRepository<ActivityTypesLookup>().GetAllAsync();

            var branches = await _unitOfWork.genericRepository<EserviceTypeBranch>().GetFilteredWithProjection(
                b => !b.IsDeleted,
                x => new { x.Id, x.EserviceTypeBranchAr, x.ActivityTypesId,x.Url }
            ).ToListAsync();

            var licenses = await _unitOfWork.genericRepository<MoiEserviceLicenseInfo>().GetFilteredWithProjection(
                l => l.Status,
                x => new
                {
                    x.Id,
                    x.Name,
                    x.ActvityTypeId,
                    x.EserviceTypeBranchId,
                    x.ReqTypeId,
                    x.Action,
                    x.Description,
                    x.Controller,
                    x.Conditions,
                    x.VariableFees,
                    x.RequiredDocuments,
                    x.Url,
                    x.FixedFees,
                    x.Measures,
                    x.Branch,
                    x.ServiceId,
                    x.TransTypeId,
                    x.LicTypeId,
                   
                }
            ).ToListAsync();

            var services = await _unitOfWork.genericRepository<Eservice>().GetFilteredWithProjection(
                s => !s.IsDeleted,
                s => new { s.Id, s.EserviceNameAr,s.ServiceId }
            ).ToListAsync();

            var validCombos = await _unitOfWork.genericRepository<ValidEserviceCombinations>().GetFilteredWithProjection(
                c => c.IsAllowed,
                c => new
                {
                    c.ActivityTypeId,
                    c.RequestTypeId,
                    c.LicenceTypeId,
                 
                }
            ).ToListAsync();

            // Compose card view
            var cardList = activities
                        .Select(activity =>
                        {

                            var licenseList = licenses
                                .Where(l => l.ActvityTypeId == activity.Id &&
                                            validCombos.Any(vc =>
                                                vc.ActivityTypeId == l.ActvityTypeId &&
                                                vc.RequestTypeId == l.ReqTypeId /*&&*/
                                                /*vc.LicenceTypeId == l.LicTypeId*/))
                                .Select(l => 
                                {
                                    var branch = branches.FirstOrDefault(b => b.Id == l.EserviceTypeBranchId);

                                    return new LicencesInfoVM
                                    {
                                        Id = l.Id,
                                       // Name = !string.IsNullOrWhiteSpace(branch?.EserviceTypeBranchAr) ? branch.EserviceTypeBranchAr : l.Name,
                                       Name=l.Name,
                                        Description = l.Description,
                                        //Url = !string.IsNullOrWhiteSpace(branch?.Url) ? branch.Url : l.Url,
                                        Url= branch?.Url,
                                        Controller = l.Controller,
                                        Action = l.Action,
                                        ActvityTypeId = l.ActvityTypeId,
                                        EserviceTypeBranchId = l.EserviceTypeBranchId,
                                        ReqTypeId = l.ReqTypeId,
                                        ServiceId = l.ServiceId,
                                        FixedFees = l.FixedFees,
                                        VariableFees = l.VariableFees,
                                        Conditions = l.Conditions,
                                        RequiredDocuments = l.RequiredDocuments,
                                        Measures = l.Measures,
                                        Branch = l.Branch
                                    };
                                })
                                    .ToList();

                            return new HomeCardViewModel
                            {
                                ActivityName = activity.NameAr,
                                EserviceName = licenseList.Count > 0
                                    ? services.FirstOrDefault(s => s.ServiceId == licenseList.FirstOrDefault()?.ServiceId)?.EserviceNameAr
                                    : "",
                                Licenses = licenseList
                            };
                                })
                                .Where(card => card.Licenses.Any())
                                .ToList();

                                        var systemOptionsDetails = await _unitOfWork
                                            .genericRepository<SystemOption>()
                                            .GetByCondition(s => s.IsDeleted == false && s.IsActive == true)
                                            .ToListAsync();

            // Map to VM list
            var optionVMs = _mapper.Map<List<SystemOption>, List<SystemOptionVM>>(systemOptionsDetails);

            //Convert to Dictionary for easier access in view

           var optionDictionary = optionVMs
               .Where(x => !string.IsNullOrEmpty(x.NameEnglish))
               .ToDictionary(x => x.NameEnglish, x => x.Value);


            return Ok( new HomePageViewModel
            {
                Cards=cardList,
                SystemOptions=optionDictionary,
                SystemOptionVMs=optionVMs
            });
        }


        [HttpGet]
        [Route("GetLicenseWithId")]
        public async Task<IActionResult> GetLicenseWithId(int id)
        {
            var licencesInfoSpec = new LicencesInfoWithSpec(id);
            var licencesDetails =await _unitOfWork.genericRepository<MoiEserviceLicenseInfo>()
                .GetByIdWithSpec(licencesInfoSpec);
          

            if (licencesDetails == null)
                return NotFound("No license info found.");

            return Ok(licencesDetails);
        }
        [HttpGet]
        [Route("GetLicenseFullInfo")]
        public async Task<IActionResult> GetLicenseFullInfo(int activityId, int requestTypeId)
        {
            var licenseInfo = await _unitOfWork
                .genericRepository<MoiEserviceLicenseInfo>()
                .GetFilteredWithProjection(
                    filter: x => x.ActvityTypeId == activityId && x.ReqTypeId == requestTypeId,
                    selector: x => new LicencesInfoVM
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        Conditions = x.Conditions,
                        RequiredDocuments = x.RequiredDocuments,
                        Measures = x.Measures,
                        ActivityType = x.ActivityTypesLookup.NameAr,
                        RequestType = x.RequestsTypesLookup.NameAr,
                        TransactionType = x.TransactionTypesLookup.NameAr,
                      
                        BranchType = x.EserviceTypeBranch.EserviceTypeBranchAr,
                        Url = x.Url
                    },
                    includes: new Expression<Func<MoiEserviceLicenseInfo, object>>[]
                    {
                x => x.ActivityTypesLookup,
                x => x.RequestsTypesLookup,
                x => x.TransactionTypesLookup,
             
                x => x.EserviceTypeBranch
                    }
                )
                .FirstOrDefaultAsync();

            if (licenseInfo == null)
                return NotFound("No license info found.");

            return Ok(licenseInfo);
        }


        [HttpGet]
        [Route("servicesWithCondition")]
        public async Task<IActionResult> servicesWithCondition()
        {
            var services = await _unitOfWork.genericRepository<Eservice>()
                .GetFilteredWithProjection(
                filter: a => a.EserviceName != "PACI" && a.EserviceName != "BasicServices" && a.IsDeleted == false,
                 selector: x => new { x.EserviceName, x.ServiceId, x.EserviceNameAr, x.Url })
                .ToListAsync();
            return Ok(services);
        }

        [HttpGet]
        [Route("GetServiceBranchTypes")]
        public async Task<IActionResult> GetServiceBranchTypes()
        {
            var services = await _unitOfWork.genericRepository<EserviceTypeBranch>()
                .GetFilteredWithProjection(
                 selector: x => new {
                     EserviceTypeBranchEn = x.EserviceTypeBranchEn ?? "", 
                     EserviceTypeBranchAr = x.EserviceTypeBranchAr ?? "",
                     Url = x.Url ?? "",
                     Fees = x.Fees ?? 0.0m })
                .ToListAsync();
            return Ok(services);
        }
        [HttpGet]
        [Route("GetEserviceTypes")]
        public async Task<IActionResult> GetEserviceTypes()
        {
            var services = await _unitOfWork.genericRepository<EserviceTypesLookup>()
                .GetFilteredWithProjection(
                 selector: x => new { x.EserviceId, x.Url, x.IsDeleted, x.EserviceTypeAr, x.EserviceTypeEn })
                .ToListAsync();
            return Ok(services);
        }
        [HttpGet]
        [Route("GetLicencesInfos")]
        public async Task<IActionResult> GetLicencesInfos()
        {
            var services = await _unitOfWork.genericRepository<MoiEserviceLicenseInfo>()
                .GetFilteredWithProjection(
                filter: x => x.Status == true,
                 selector: x => new
                 {
                     x.Url,
                     x.Name,
                     x.Description,
                     x.Controller,
                     x.Branch,
                     x.ActvityTypeId,
                     x.Conditions,
                     x.FixedFees,
                     x.EserviceTypeBranchId,
                     x.ReqTypeId,
                     x.Measures,
                     x.Status,
                     x.Sort
                 })
                .ToListAsync();
            return Ok(services);
        }
        [HttpGet]
        [Route("GetEserviceTypeById")]
        public async Task<IActionResult> GetEserviceTypeById(int id)
        {
            var services = await _unitOfWork.genericRepository<EserviceTypesLookup>()
                .GetFilteredWithProjection(
                filter: x => x.Id == id,
                 selector: x => new { x.EserviceId, x.Url, x.IsDeleted, x.EserviceTypeAr, x.EserviceTypeEn })
                .FirstOrDefaultAsync();
            return Ok(services);
        }
        [HttpGet]
        [Route("GetEserviceTypeByServiceId")]
        public async Task<IActionResult> GetEserviceTypeByServiceId(string serviceId)
        {
            var services = await _unitOfWork.genericRepository<EserviceTypesLookup>()
                .GetFilteredWithProjection(
                filter: x => x.EserviceId == serviceId,
                 selector: x => new { x.EserviceId,x.Id, x.Url, x.IsDeleted, x.EserviceTypeAr, x.EserviceTypeEn })
                .ToListAsync();
            return Ok(services);
        }
        [HttpGet]
        [Route("GetBranchWithActivityandServiceType")]
        public async Task<IActionResult> GetBranchWithActivityandServiceType( int? ActivityTypeId)
        {
            var services = await _unitOfWork.genericRepository<EserviceTypeBranch>()
                .GetFilteredWithProjection(
                filter: x =>  x.ActivityTypesId == ActivityTypeId,
                 selector: x => new { x.EserviceTypeBranchEn,x.Id, x.EserviceTypeBranchAr, x.Url, x.Fees })
                .ToListAsync();
            return Ok(services);
        }

        //[HttpGet]
        //[Route("GetBranchWithServiceType")]
        //public async Task<IActionResult> GetBranchWithServiceType(int EserviceTypeId)
        //{
        //    var services = await _unitOfWork.genericRepository<EserviceTypeBranch>()
        //        .GetFilteredWithProjection(
        //        filter: x => x.EserviceTypeId == EserviceTypeId,
        //         selector: x => new { x.EserviceTypeBranchEn, x.Id, x.EserviceTypeBranchAr, x.Url, x.Fees })
        //        .ToListAsync();
        //    return Ok(services);
        //}
        #endregion
        [HttpGet]
        [Route("GetDataForUser")]
        public async Task<IActionResult> GetDataForUser(string civilid)
        {
            var user = await _unitOfWork.genericRepository<AspNetUser>()
                          .GetByCondition(u => u.CivilId == civilid).FirstOrDefaultAsync();
            if (user == null) return BadRequest();
            else return Ok(_mapper.Map<AspNetUser, AspnetUserVM>(user));

        }

        [HttpPost]
        [Route("SubmitMyProfile")]
        public async Task<IActionResult> SubmitMyProfile(UserProfile model)
        {
            var userEdit=await _unitOfWork.genericRepository<AspNetUser>().GetByCondition
                (u=>u.CivilId==model.CivilId).FirstOrDefaultAsync();

            if (userEdit == null)
                return BadRequest();
            // Update email and phone
            userEdit.Email = model.Email;
            userEdit.PhoneNumber = model.Mobile;

            // Change password if a new one is provided
            if (!string.IsNullOrEmpty(model.Password))
            {
                // Remove old password and add new one
                var userManager = HttpContext.RequestServices.GetService<UserManager<AspNetUser>>();

                var token = await _userManager.GeneratePasswordResetTokenAsync(userEdit);
                var result = await _userManager.ResetPasswordAsync(userEdit, token, model.Password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors);
            }

            // Save changes
            await _unitOfWork.genericRepository<AspNetUser>().Update(userEdit);
            await _unitOfWork.Complete();

            return Ok("User profile updated successfully");
            //return Ok();
        }
        #region RequestAndLicences
        [HttpGet]
        [Route("AllInformationForApplicant")]
        public async Task<IActionResult> AllInformationForApplicant(string CivilId)
        {
            try
            {
                //--------For Request-----------------
                //var specMosanafatRequest = new RequestWithSpecificService(CivilId, (int)ServiceEnum.Mosanafat);
                //var RequestMosanafat = await _unitOfWork.genericRepository<MoiEserviceLicensesRequest>().GetTableWithSpecService(specMosanafatRequest);
                //var specTourismRequest = new RequestWithSpecificService(CivilId, (int)ServiceEnum.Tourism);
                //var RequestTourism = await _unitOfWork.genericRepository<MoiEserviceLicensesRequest>().GetTableWithSpecService(specTourismRequest);
                //var specElawRequest = new RequestWithSpecificService(CivilId, (int)ServiceEnum.Elaw);
                //var RequestElaw = await _unitOfWork.genericRepository<MoiEserviceLicensesRequest>().GetTableWithSpecService(specElawRequest);
                //var specPublishingRequest = new RequestWithSpecificService(CivilId, (int)ServiceEnum.publishing);
                //var RequestPublishing = await _unitOfWork.genericRepository<MoiEserviceLicensesRequest>().GetTableWithSpecService(specPublishingRequest);
                var Allrequest = new RequestWithSpecificService(CivilId);
                var RequestForAll = await _unitOfWork.genericRepository<MoiEserviceLicensesRequest>().GetTableWithSpecService(Allrequest);
                //----------ForLicences---------

                //var specMosanafatLicences = new LicencesWithSpecificService(CivilId,(int)ServiceEnum.Mosanafat);
                //var LicencesMosanafat = await _unitOfWork.genericRepository<Licence>().GetTableWithSpecService(specMosanafatLicences);
                //var specTourismLicences = new LicencesWithSpecificService(CivilId, (int)ServiceEnum.Tourism);
                //var LicencesTourism = await _unitOfWork.genericRepository<Licence>().GetTableWithSpecService(specTourismLicences);
                //var specElawLicences = new LicencesWithSpecificService(CivilId, (int)ServiceEnum.Elaw);
                //var LicencesElaw = await _unitOfWork.genericRepository<Licence>().GetTableWithSpecService(specElawLicences);
                //var specPublishingLicences = new LicencesWithSpecificService(CivilId, (int)ServiceEnum.publishing);
                //var LicencesPublishing = await _unitOfWork.genericRepository<Licence>().GetTableWithSpecService(specPublishingLicences);
                //var specLicencesForall = new LicencesWithSpecificService(CivilId, (int)ServiceEnum.Mosanafat);
                //var LicencesForAll = await _unitOfWork.genericRepository<Licence>().GetTableWithSpecService(specMosanafatLicences);
                //-------------ForApplicant----------

                var applicant = await _unitOfWork.genericRepository<AspNetUser>().GetByIdObject(a => a.CivilId == CivilId);
                var allServices = await _unitOfWork.genericRepository<Eservice>().GetAllAsync();

                var requestVMs = _mapper
                            .Map<IEnumerable<MoiEserviceLicensesRequest>, IEnumerable<RequestVM>>(RequestForAll)
                            .ToList();

                // Add service name to each request
                foreach (var req in requestVMs)
                {
                    var service = allServices.FirstOrDefault(s => s.ServiceId == req.ServiceId);
                    if (service != null)
                    {
                        req.ServiceName = service.EserviceNameAr;
                    }
                }
                var licencesDetails = new LicencesWithSpecificService(CivilId, (int)licencesStatusEnum.Released);
                var licenceWithSpec = await _unitOfWork.genericRepository<Licence>().GetTableWithSpec(licencesDetails);
                var licencesVM = _mapper.Map<IEnumerable<Licence>, IEnumerable<LicencesVM>>(licenceWithSpec);
                //--------------ForMandoob----------------
                var AllMandoobsFromApplicant = await _unitOfWork.genericRepository<AspNetMultipleUser>().GetByCondition(m => m.MainUserId == applicant.Id).ToListAsync();
                var MnadoobIds = AllMandoobsFromApplicant.Select(m => m.MandoobId).ToList();
                var MandoobInformation = await _unitOfWork.genericRepository<AspNetUser>().GetByCondition(m => MnadoobIds.Contains(m.Id)).ToListAsync();
                foreach (var req in licencesVM)
                {
                    var service = allServices.FirstOrDefault(s => s.ServiceId == req.ServiceId);
                    if (service != null)
                    {
                        req.ServiceName = service.EserviceNameAr;
                    }
                }
                var LicencesPreApprove = new PreApprovementWithSpec(CivilId, (int)licencesStatusEnum.Released);
                var licencePreAppWithDetails = await _unitOfWork.genericRepository<MoiPreApprovement>().GetTableWithSpec(LicencesPreApprove);

                return Ok(new LicencesWithRequestForUser
                {
                    preApprovementVMs = _mapper.Map<IEnumerable<MoiPreApprovement>, IEnumerable<PreApprovementVM>>(licencePreAppWithDetails),
                    RequestVM = requestVMs,
                    licencesVMs = licencesVM,
                    AspnetUserVM = _mapper.Map<AspNetUser, AspnetUserVM>(applicant),
                    Mandoob = _mapper.Map<IEnumerable<AspNetUser>, IEnumerable<AspnetUserVM>>(MandoobInformation)
                });
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Exception occurred: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("GetAllDelegateUser")]
        public async Task<IActionResult> GetAllDelegateUser(string CivilId)
        {
            var AspnetUserToCivilId=await _unitOfWork.genericRepository<AspNetUser>()
                     .GetByCondition(a=>a.CivilId == CivilId).FirstOrDefaultAsync();
            var MutipleUserForThisUser = new AspNetMultipleUserWithSpec(AspnetUserToCivilId.Id,true);
            var MultipleMandoob = await _unitOfWork.genericRepository<AspNetMultipleUser>()
                         .GetTableWithSpec(MutipleUserForThisUser);
            return Ok(MultipleMandoob);
        }
        [HttpGet]
        [Route("GetRequestDetails/{id}")]
        public async Task<RequestFrontVM> GetRequestDetails(long id)
        {
            //var SpecRequest = new RequestWithSpecificService((int)id, (int)ServiceEnum.Tourism, false);
            var SpecRequest = new RequestWithSpecificService((int)id, true);
            var RequestDetails = await _unitOfWork.genericRepository<MoiEserviceLicensesRequest>()
                              .GetByIdWithSpec(SpecRequest);
            var PaymentPerRequest = await _unitOfWork.genericRepository<MoiEserviceRequestPaymentDetail>()
                            .GetByCondition(x => x.RequestId == id).FirstOrDefaultAsync();
            var AttachmenttRequest = await _unitOfWork.genericRepository<MoiEserviceRequestsAttach>()
                           .GetByCondition(x => x.AttachRequestid == id).ToListAsync();
            var UserApplicant = await _unitOfWork.genericRepository<AspNetUser>()
                          .GetByCondition(x => x.CivilId == RequestDetails.AppCivilId).FirstOrDefaultAsync();


            return new RequestFrontVM
            {
                RequestVM = _mapper.Map<MoiEserviceLicensesRequest, RequestVM>(RequestDetails),
                PaymentDetailsVM = _mapper.Map<MoiEserviceRequestPaymentDetail, PaymentDetailsVM>(PaymentPerRequest),
                attachVMs = _mapper.Map<IEnumerable<MoiEserviceRequestsAttach>, IEnumerable<AttachVM>>(AttachmenttRequest),
                AspnetUserVM = _mapper.Map<AspNetUser, AspnetUserVM>(UserApplicant)

            };

        }


        [HttpGet]
        [Route("GetAllLicencesForspecificUserToDelegate")]
        public async Task<dynamic> GetAllLicencesForspecificUserToDelegate(string CivilId)
        {
            var spec = new LicencesWithSpecificService(CivilId);
            var allLicenses = await _unitOfWork.genericRepository<Licence>()
                                        .GetTableWithSpecService(spec);

            var allServiceData = await _unitOfWork.genericRepository<Eservice>()
                                        .GetAllAsync(); 

            var mapped = allLicenses.Select(lic => new LicenseAssignmentVM
            {
                Id = lic.LicId,
                LicName = lic.LicName,
                ServiceId = lic.ServiceId,
                ServiceName = allServiceData
                                .FirstOrDefault(s => s.ServiceId == lic.ServiceId)?.EserviceNameAr
            }).ToList();
            var usedPreApprovalIds = allLicenses
        .Where(l => l.PreApprovalId != null)
        .Select(l => l.PreApprovalId.Value)
        .ToHashSet();
            var preApprovals = await _unitOfWork.genericRepository<MoiPreApprovement>()
        .GetByCondition(p => p.ApplicantCivilId == CivilId).ToListAsync();

            // 6. Filter out pre-approvals that are already used
            var preApprovalsMapped = preApprovals
                .Where(pre => !usedPreApprovalIds.Contains(pre.PreAppId))
                .Select(pre => new LicenseAssignmentVM
                {
                    Id = pre.PreAppId,
                    LicName = pre.LicenseName ?? "موافقة مبدئية",
                  
                  
                }).ToList();

            // 7. Merge both
            var mergedLicenses = mapped.Concat(preApprovalsMapped).ToList();

            return new RegisterDelegateVM
            {
                Licenses = mapped
            };

        }

        [HttpPost]
        [Route("RegisterDelegateUser")]
        public async Task<dynamic> RegisterDelegateUser(RegisterApiDelegateVM model)
        {
            if (model == null || string.IsNullOrEmpty(model.MandoobCivilId))
                return BadRequest("Invalid data");
            var GetApplicantUser = await _unitOfWork.genericRepository<AspNetUser>()
                    .GetByCondition(u => u.CivilId == model.ApplicantCivilId).FirstOrDefaultAsync();
            try
            {
                // 1. التحقق من وجود المفوض مسبقًا
                var mandoob = await _unitOfWork.genericRepository<AspNetUser>()
                    .GetByCondition(u => u.CivilId == model.MandoobCivilId).FirstOrDefaultAsync();

                if (mandoob == null)
                {
                    mandoob = new AspNetUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        CivilId = model.MandoobCivilId,
                        FullNameAr = model.FullNameAr,
                        Mobile = model.Mobile,
                        AccountTypeId =(int) AccountTypeEnum.User,
                    };

                    var createResult = await _userManager.CreateAsync(mandoob, model.Password);
                    if (!createResult.Succeeded)
                    {
                        return BadRequest(new { message = "فشل إنشاء المفوض", errors = createResult.Errors });
                    }
                }

                // 2. التحقق من وجود علاقة تفويض مسبقًا
                


                var existingDelegation = await _unitOfWork.genericRepository<AspNetMultipleUser>()
                    .GetByCondition(d => d.MainUserId == GetApplicantUser.Id && d.MandoobId == mandoob.Id).FirstOrDefaultAsync();

                if (existingDelegation == null)
                {
                    var newDelegation = new AspNetMultipleUser
                    {
                        MainUserId = GetApplicantUser.Id,
                        MandoobId = mandoob.Id,
                        IsActive = true
                    };
                  await  _unitOfWork.genericRepository<AspNetMultipleUser>().Create(newDelegation);
                    await _unitOfWork.Complete();
                }

                // 3. التحقق من التراخيص الموجودة مسبقًا
                foreach (var license in model.Licenses.Where(l => l.IsSelected))
                {
                    var MutipleUser = new AspNetMultipleUserWithSpec(mandoob.Id, false);
                    var multiple = await _unitOfWork.genericRepository<AspNetMultipleUser>().GetByIdWithSpec(MutipleUser);
                    var alreadyAssigned = await _unitOfWork.genericRepository<AspNetMultipleLicenseUser>()
                        .GetByCondition(x => x.LicenseId == license.Id &&
                            x.MultipleUserId == multiple.Id).AnyAsync();
                        //.AnyAsync(x =>
                        //    x.LicenseId == license.Id.ToString() &&
                        //    x.MultipleUserId == mandoob.Id);

                    if (alreadyAssigned)
                    {
                        // skip duplicate
                        continue;
                    }

                    var newLicenseAssignment = new AspNetMultipleLicenseUser
                    {
                        MultipleUserId = multiple.Id,
                        LicenseId = license.Id,
                        ServiceId = license.ServiceId?.ToString(),
                        AttachmentUrl = license.AttachmentUrl,
                        IsApproved=false,
                        
                    };

                  await  _unitOfWork.genericRepository<AspNetMultipleLicenseUser>().Create(newLicenseAssignment);
                }

                await _unitOfWork.Complete();

                // ✅ Log success
                _logger.LogInformation("Delegate created: {mandoobId} by user {mainUserId}", mandoob.Id, GetApplicantUser.Id);

                return Ok(new { message = "تم تسجيل المفوض وربطه بالتراخيص بنجاح" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering delegate for user {mainUserId}", GetApplicantUser.Id);
                return StatusCode(500, new { message = "حدث خطأ أثناء معالجة الطلب", error = ex.Message });
            }

        }
        [HttpGet]
        [Route("GetAllLicencesForMandoob")]
        public async Task<IActionResult> GetAllLicencesForMandoob(string MandoobId)
        {
            if (string.IsNullOrEmpty(MandoobId))
                return BadRequest("Mandoob ID is required");
            var multipleUsers = await _unitOfWork
                                    .genericRepository<AspNetMultipleUser>()
                                    .GetByCondition(x => x.MandoobId == MandoobId)
                                    .Select(x => x.Id)
                                    .ToListAsync();
            // 1. Get all licenses assigned to the mandoob
            var licenses = await _unitOfWork
                             .genericRepository<AspNetMultipleLicenseUser>()
                             .GetFilteredWithProjection(
                             
                                 x => multipleUsers.Contains(x.MultipleUserId ?? 0)&& x.IsApproved == true,
                                 x => new
                                 {
                                     x.Licence.LicId,
                                     x.Licence.LicNo,
                                     x.Licence.LicName,
                                     x.Licence.ServiceId,
                                     x.AttachmentUrl
                                 },
                                 x => x.Licence
                             ).ToListAsync();
            var allServices = await _unitOfWork.genericRepository<Eservice>().GetAllAsync();

            var result = licenses.Select(l => new LicenseAssignmentVM
            {
                Id = l.LicId,
                LicNo = l.LicNo,
                LicName = l.LicName,
                ServiceId = l.ServiceId,
                ServiceName = allServices.FirstOrDefault(s => s.ServiceId == l.ServiceId)?.EserviceNameAr,
                AttachmentUrl = l.AttachmentUrl
            }).ToList();

            return Ok(result);
        }

        [HttpGet]
        [Route("GetLicencesDelegateFor")]
        public async Task<IActionResult> GetLicencesDelegateFor(string MandoobCivilId)
        {
            if (string.IsNullOrEmpty(MandoobCivilId))
                return BadRequest("Mandoob Civil ID is required.");

            // Step 1: Get Mandoob UserId (GUID) from CivilId
            var mandoobUser = await _unitOfWork
                .genericRepository<AspNetUser>()
                .GetByCondition(x => x.CivilId == MandoobCivilId)
                .FirstOrDefaultAsync();

            if (mandoobUser == null)
                return NotFound("Mandoob user not found.");

            var mandoobUserId = mandoobUser.Id;

            // Step 2: Get mappings from AspNetMultipleUser
            var multipleUserIds = await _unitOfWork
                .genericRepository<AspNetMultipleUser>()
                .GetByCondition(x => x.MandoobId == mandoobUserId)
                .Select(x => x.Id)
                .ToListAsync();

            if (!multipleUserIds.Any())
                return Ok(new List<LicenseAssignmentVM>()); // No licenses assigned

            // Step 3: Get delegated licenses
            var licenses = await _unitOfWork
                .genericRepository<AspNetMultipleLicenseUser>()
                .GetFilteredWithProjection(
                    x => multipleUserIds.Contains(x.MultipleUserId ?? 0)&&x.IsApproved==true,
                    x => new
                    {
                        x.Licence.LicId,
                        x.Licence.LicNo,
                        x.Licence.LicName,
                        x.Licence.ServiceId,
                        x.AttachmentUrl
                    },
                    x => x.Licence
                ).ToListAsync();

            // Step 4: Get all services
            var allServices = await _unitOfWork.genericRepository<Eservice>().GetAllAsync();

            // Step 5: Construct final result
            var result = licenses.Select(l => new LicenseAssignmentVM
            {
                Id = l.LicId,
                LicNo = l.LicNo,
                LicName = l.LicName,
                ServiceId = l.ServiceId,
                ServiceName = allServices.FirstOrDefault(s => s.ServiceId == l.ServiceId)?.EserviceNameAr,
                AttachmentUrl = l.AttachmentUrl
            }).ToList();

            return Ok(result);
        }
        #endregion
    }
}
