using Business.Enums;
using Business.Helpers;
using Business.ViewModel;
using Business.ViewModel.Account;
using Business.ViewModel.HomePage;
using Business.ViewModel.Tourism;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MOI_Eservice.Models;
using Newtonsoft.Json;
using NuGet.Configuration;
using NuGet.Protocol;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Policy;

namespace MOI_Eservice.Controllers
{
    //[Authorize(AuthenticationSchemes = "UserScheme")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HelperUrlApi _helperUrlApi;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _file;
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration, ILogger<HomeController> logger,HelperUrlApi helperUrlApi, HttpClient httpClient)
        {
            _logger = logger;
            _helperUrlApi = helperUrlApi;
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _configuration = configuration;
            _file = configuration["Path:Delegations"];

        }

        #region Index

        public async Task<ActionResult> Index(int? serviceTypeId, int? actvityTypeId)
        {
            try
            {
                var token = HttpContext.Session.GetString("UserToken");
                // Log the received token (optional for debugging)
                Console.WriteLine("Token received in Index: " + token);
               
                var userId = HttpContext.Session.GetString("UserId");
                var userName = HttpContext.Session.GetString("UserUserName");
                var civilId = HttpContext.Session.GetString("UserCivilId");
                var fullName = HttpContext.Session.GetString("UserFullName");
                var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
                var isDelegate = HttpContext.Session.GetString("UserIsDelegate");
                var isApplicant = HttpContext.Session.GetString("UserIsApplicant");

                var apiSettings = $"{_baseUrl}HomePage/";
                var homePageViewModel = new HomePageViewModel();

                var cardData = await _helperUrlApi.GetDataFromApi<HomePageViewModel>($"{apiSettings}GetCardActivityList");

            
                // Return the model to the view
                return View(cardData);
            }
            catch (Exception ex)
            {
                // Error handling and logging
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                string fileName = controllerName + "_" + actionName + "_";
                string exId = ExceptionLog.LogException(ex, fileName);

                TempData["Ex"] = exId;
                throw;
            }
        }
        public async Task<ActionResult> ServiceDetails(int id,int? LicId)
        {
            var apiSettings = $"{_baseUrl}HomePage/";
           

            var response = await _helperUrlApi.GetDataFromApi<LicencesInfoVM>($"{apiSettings}GetLicenseWithId?id={id}");
            if(LicId !=null || LicId!=0)
            {

            }
            response.LicId = LicId;
            return View(response);
        }
        //public async Task<ActionResult> LicenseDetails(int? activityId,int? requestTypeId)
        //{
            
        //    return View();
        //}
        #region HomeCardList
        public async Task<ActionResult> HomePageCardList()
        {
            try
            {
                var apiSettings = $"{_baseUrl}HomePage/";
               // var list = db.MOI_Eservice_License_Info.Where(a => a.Status == true).ToList();

                var actvityTypes = _helperUrlApi.GetDataFromApi<List<EserviceActvityTypeModel>>($"{apiSettings}GetActivityTypes");

                var eServiceLicenseInfo = _helperUrlApi.GetDataFromApi<List<EserviceActvityTypeModel>>($"{apiSettings}GetLicencesInfos");

                var eservices =  _helperUrlApi.GetDataFromApi<List<EserviceActvityTypeModel>>($"{apiSettings}servicesWithCondition");

                ViewBag.EservicesList = eservices;


                List<EserviceTypeBranchModel> eserviceTypeBranches =await  _helperUrlApi.GetDataFromApi<List<EserviceTypeBranchModel>>($"{apiSettings}GetServiceBranchTypes");
                var etbm = eserviceTypeBranches.OrderByDescending(a => a.Id).ToList();
                ViewBag.EserviceTypeBranchesList = etbm;


                ViewBag.LicenceInfoList = eServiceLicenseInfo;// tourismLiceInfo;
                ViewBag.ActvityTypesList = actvityTypes;


                return PartialView("_HomePageCardList", eServiceLicenseInfo);
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                string fileName = controllerName + "_" + actionName + "_";

                string exId = ExceptionLog.LogException(ex, fileName);

                TempData["Ex"] = exId;
                throw;
            }
        }
        private bool IsSpecialActivityType(int? activityTypeId)
        {
            // Define the activity type IDs that are considered special
            var specialActivityTypes = new List<int> { 22, 23, 24, 32, 25, 26, 27, 28, 29, 30, 31, 1, 2, 3, 4, 5, 6, 19, 20 };

            return activityTypeId.HasValue && specialActivityTypes.Contains(activityTypeId.Value);
        }
        #endregion
        #endregion
        #region MyAccount
        [HttpGet]
        public async Task<ActionResult> MyProfile()
        {
            var token = HttpContext.Session.GetString("UserToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserUserName");
            var civilId = HttpContext.Session.GetString("UserCivilId");
            var fullName = HttpContext.Session.GetString("UserFullName");
            var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
            if (token != null) { 
            var apiSettings = _baseUrl + $"HomePage/GetDataForUser?civilid={civilId}";
            var response = await _helperUrlApi.GetDataFromApi<AspnetUserVM>(apiSettings);
            return View(response);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> GetAllDelegatUser()
        {

            var token = HttpContext.Session.GetString("UserToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserUserName");
            var civilId = HttpContext.Session.GetString("UserCivilId");
            var fullName = HttpContext.Session.GetString("UserFullName");
            var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
            var apiSettings = _baseUrl + $"HomePage/GetAllDelegateUser?civilid={civilId}";
            if (token != null)
            {
                var response = await _helperUrlApi.GetDataFromApi<List<DelegationRequestVM>>(apiSettings);
                return View(response);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public async Task<ActionResult> RegisterDelegateUser()
        {

            var token = HttpContext.Session.GetString("UserToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserUserName");
            var civilId = HttpContext.Session.GetString("UserCivilId");
            var fullName = HttpContext.Session.GetString("UserFullName");
            var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
            var apiSettings = _baseUrl + $"HomePage/GetAllLicencesForspecificUserToDelegate?CivilId={civilId}";
            if (token != null)
            {
                var response = await _helperUrlApi.GetDataFromApi<RegisterDelegateVM>(apiSettings);
                return View(response);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> RegisterDelegateUser(RegisterDelegateVM model)
        {
            var token = HttpContext.Session.GetString("UserToken");
            var civilId = HttpContext.Session.GetString("UserCivilId"); // main user

            if (token == null)
                return RedirectToAction("Index", "Home");

            // „”«— —›⁄ «·„·›«  „‰ «·≈⁄œ«œ« 
            var baseRelativePath = _file?.Replace('/', Path.DirectorySeparatorChar)
                                         .Replace('\\', Path.DirectorySeparatorChar)
                                         .Trim(Path.DirectorySeparatorChar);

            var mainUserCivilId = civilId;
            var mandoobCivilId = model.MandoobCivilId;

            //  ÃÂÌ“ „·›«  «·„—›ﬁ« 
            foreach (var license in model.Licenses.Where(x => x.IsSelected && x.FilePath != null))
            {
                var fileName = $"{license.Id}{Path.GetExtension(license.FilePath.FileName)}";
                var relativePath = Path.Combine(baseRelativePath, mainUserCivilId, mandoobCivilId);
                var fullDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

                if (!Directory.Exists(fullDirectoryPath))
                    Directory.CreateDirectory(fullDirectoryPath);

                var fullFilePath = Path.Combine(fullDirectoryPath, fileName);

                using (var stream = new FileStream(fullFilePath, FileMode.Create))
                {
                    await license.FilePath.CopyToAsync(stream);
                }

                license.AttachmentUrl = Path.Combine("/", relativePath, fileName).Replace("\\", "/");
            }

            //  ÕÊÌ·  —«ŒÌ’ LicenseAssignmentVM ? LicenseApiAssignmentVM
            var apiLicenses = model.Licenses
                .Where(l => l.IsSelected)
                .Select(l => new LicenseApiAssignmentVM
                {
                    Id = l.Id,
                    LicName = l.LicName,
                    ServiceId = l.ServiceId,
                    ServiceName = l.ServiceName,
                    IsSelected = true,
                    AttachmentUrl = l.AttachmentUrl
                }).ToList();

            //  ÃÂÌ“ «·ﬂ«∆‰ «·‰Â«∆Ì ·≈—”«·Â ≈·Ï «·‹ API
            var apiModel = new RegisterApiDelegateVM
            {
                ApplicantCivilId = civilId,
                MandoobCivilId = model.MandoobCivilId,
                FullNameAr = model.FullNameAr,
                Email = model.Email,
                Mobile = model.Mobile,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                AccountTypeId = (int)AccountTypeEnum.User,
                Licenses = apiLicenses,
                
                
            };

            // ≈—”«· «·»Ì«‰«  ≈·Ï «·‹ API
            var apiUrl = _baseUrl + "HomePage/RegisterDelegateUser";
            var response = await _helperUrlApi.PostDataToApi<RegisterApiDelegateVM, string>(apiUrl, apiModel);

            if (response != null)
            {
                TempData["Success"] = " „ ≈—”«· «·ÿ·» »‰Ã«Õ";
                return RedirectToAction("Success");
            }

            ModelState.AddModelError("", "›‘· ›Ì ≈—”«· «·ÿ·». Ì—ÃÏ «·„Õ«Ê·… ·«Õﬁ«.");
            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> ShowLicensesMandoob(string MandoobId)
        {
            var token = HttpContext.Session.GetString("UserToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserUserName");
            var civilId = HttpContext.Session.GetString("UserCivilId");
            var fullName = HttpContext.Session.GetString("UserFullName");
            var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");

            var apiSettings = _baseUrl + $"HomePage/GetAllLicencesForMandoob?MandoobId={MandoobId}";
            if (token != null)
            {
                var response = await _helperUrlApi.GetDataFromApi<List<LicenseAssignmentVM>>(apiSettings);
                return View(response);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ActionResult> GetLicencesDelegateFor()
        {
            var token = HttpContext.Session.GetString("UserToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserUserName");
            var civilId = HttpContext.Session.GetString("UserCivilId");
            var fullName = HttpContext.Session.GetString("UserFullName");
            var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
           var isDelegate= HttpContext.Session.GetString("UserIsDelegate");
           var isApplicant= HttpContext.Session.GetString("UserIsApplicant");
            var apiSettings = _baseUrl + $"HomePage/GetLicencesDelegateFor?MandoobCivilId={civilId}";
            if (token != null)
            {
                var response = await _helperUrlApi.GetDataFromApi<List<LicenseAssignmentVM>>(apiSettings);
                return View(response);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> MyProfile(AspnetUserVM model)
        {
            var token = HttpContext.Session.GetString("UserToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserUserName");
            var civilId = HttpContext.Session.GetString("UserCivilId");
            var fullName = HttpContext.Session.GetString("UserFullName");
            var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
            var apiSettings = _baseUrl + $"HomePage/SubmitMyProfile?civilid={civilId}";

            var responseModel = new UserProfile
            {
                Email=model.Email,
                Mobile=model.Mobile,
                Password=model.Password,
                CivilId= civilId
            };

            var response = await _helperUrlApi.PostDataToApi<UserProfile, AspnetUserVM>(apiSettings, responseModel);
            return View(response);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllRequest()
        {
            var token = HttpContext.Session.GetString("UserToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserUserName");
            var civilId = HttpContext.Session.GetString("UserCivilId");
            var fullName = HttpContext.Session.GetString("UserFullName");
            var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
            var apiSettings = _baseUrl + $"HomePage/AllInformationForApplicant?civilid={civilId}";
            if(token != null) { 
            var response = await _helperUrlApi.GetDataFromApi<LicencesWithRequestForUser>(apiSettings);
            return View(response);
            }
            return RedirectToAction("Index", "Home");
        }
    

        [HttpGet]
        public async Task<ActionResult> GetAllLicences()
       {
            var token = HttpContext.Session.GetString("UserToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserUserName");
            var civilId = HttpContext.Session.GetString("UserCivilId");
            var fullName = HttpContext.Session.GetString("UserFullName");
            var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
            var apiSettings = _baseUrl + $"HomePage/AllInformationForApplicant?civilid={civilId}";
            if (token != null)
            {
                var response = await _helperUrlApi.GetDataFromApi<LicencesWithRequestForUser>(apiSettings);
                return View(response);
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public async Task<ActionResult> MyAccount()
        {
            var token = HttpContext.Session.GetString("UserToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserUserName");
            var civilId = HttpContext.Session.GetString("UserCivilId");
            var fullName = HttpContext.Session.GetString("UserFullName");
            var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
            var apiSettings = _baseUrl + $"HomePage/AllInformationForApplicant?civilid={civilId}";

            var response = await _helperUrlApi.GetDataFromApi<LicencesWithRequestForUser>(apiSettings);
            return View(response);
        }

       
        #endregion
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
