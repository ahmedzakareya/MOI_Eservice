using Business.ViewModel;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

using System.Text.Json;
using Business.Helpers;
using System.Net.Http.Headers;
using System.Drawing;
using AutoMapper;
using Azure.Core;
using Business.Helpers;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Business.ViewModel.Dynamic;
using Castle.Components.DictionaryAdapter;
using Business.Enums;
using Business.ViewModel.CombinedView;
using Business.ViewModel.Account;
using System.Collections.Generic;
using System.Web.Helpers;
using Newtonsoft.Json.Linq;
using RestSharp;

using Business.ViewModel.AddressPaciModel;
using MOI_Eservice.Areas.Admin.Service;
using System.Runtime.Intrinsics.X86;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Business.ViewModel.Elaw;
using Business.ViewModel.Tourism;


namespace MOI_Eservice.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(AuthenticationSchemes = "AdminScheme")]
    [Authorize(Roles = "Admin")]
    public class TourismController : Controller
    {
        private readonly string _baseUrl;
        private readonly string _file;
        private readonly HttpClient _httpClient;
        private readonly GenerateLicNo _generateLicNo;
        private readonly string GetPACIAddressURL;
        private readonly string GetPACIUserTourism;
        private readonly string GetPACIPasswordTourism;
        private readonly string GetTokenURL;

        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;
        private readonly HelperUrlApi _helperUrlApi;
        private readonly EmailService _emailService;

        public TourismController(IConfiguration configuration, HttpClient httpClient, GenerateLicNo generateLicNo
            , ILogger<TourismController> logger, IWebHostEnvironment env, HelperUrlApi helperUrlApi, EmailService emailService)
        {
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _file = configuration["Path:Tourism"];
            GetPACIAddressURL = configuration["PaciAddressData:GetPACIAddressURL"];
            GetPACIUserTourism = configuration["PaciAddressData:GetPACIUserTourism"];
            GetPACIPasswordTourism = configuration["PaciAddressData:GetPACIPasswordTourism"];
            GetTokenURL = configuration["PaciAddressData:GetTokenURL"];


            _httpClient = httpClient;
            _generateLicNo = generateLicNo;
            _logger = logger;
            _env = env;
            _helperUrlApi = helperUrlApi;
            _emailService=emailService;
        }
        #region Request PreAprovement

        public async Task<IActionResult> Statistics()
        {
            var apiSettings = _baseUrl + $"api/AdminTourism/GetAllStatistics";
            var apiSettingViewbag = _baseUrl + $"api/AdminTourism/";
            ViewBag.ApiBaseUrl = apiSettingViewbag;
            var statistics = await _helperUrlApi.GetDataFromApi<StatisticsViewModel>(apiSettings);

            return View(statistics);
        }
        public async Task<ActionResult> GetAllLicences()
        {
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
           
            var apiSettings = _baseUrl + $"api/AdminTourism";
            ViewBag.ApiBaseUrl = apiSettings;
            var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            if (loggedInUser)
            {
                var url = $"api/AdminTourism/GetAllLicences?serviceId={(int)ServiceEnum.Tourism}";
                var Licences = await _helperUrlApi.GetDataFromApi<List<LicencesVM>>(url);
                return View(Licences);
     
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
        }
        public async Task<ActionResult> GetLicenceDetails(int id)
        {
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }

            var apiSettings = _baseUrl + $"api/AdminTourism";
            ViewBag.ApiBaseUrl = apiSettings;
            var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            if (loggedInUser)
            {
                var licenes = $"api/AdminTourism/GetLicenceById?licId={id}";
                var licencesDetails = await _helperUrlApi.GetDataFromApi<LicenceDetailsVM>(licenes);
                return View(licencesDetails);
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
        }
        public async Task<ActionResult> GetAllLicencesForEdit()
        {
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }

            var apiSettings = _baseUrl + $"api/AdminTourism";
            ViewBag.ApiBaseUrl = apiSettings;
            var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            if (loggedInUser)
            {
                var url = $"api/AdminTourism/GetAllLicences?serviceId={(int)ServiceEnum.Tourism}";
                var Licences = await _helperUrlApi.GetDataFromApi<List<LicencesVM>>(url);
                return View(Licences);

            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
        }

        public async Task<ActionResult> EditLicencesDetails(int id)
        {
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }

            var apiSettings = _baseUrl + $"api/AdminTourism";
            ViewBag.ApiBaseUrl = apiSettings;
            var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            if (loggedInUser)
            {
                var licenes = $"api/AdminTourism/GetLicenceById?licId={id}";
                var licencesDetails = await _helperUrlApi.GetDataFromApi<LicenceDetailsVM>(licenes);
                return View(licencesDetails);
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
        }

        [HttpPost]
        public async Task<ActionResult> EditLicencesDetails(LicenceDetailsVM model)
        {
            try
            {
                var token = HttpContext.Session.GetString("UserToken");
                var userId = HttpContext.Session.GetString("UserId");
                var civilId = HttpContext.Session.GetString("UserCivilId");
                var fullName = HttpContext.Session.GetString("UserFullName");

                //var apiModel = new PostRequestApiModel
                //{
                //    // Example mapping (you can expand based on full model structure)
                //    LicName = model.LicencesVM?.LicName,

                //    SessionCivilId = civilId
                //};

                string apiUrl = _baseUrl + "api/AdminTourism/UpdateLicenceData";
                var ApiModel = new PreApprovalRequestApiModel
                {
                    
                    PreApproveId = model.LicencesVM.PreApprovalId,
                    AppId = model.LicencesVM.ApplicantId,
                    //BuildingId = model.RequestVM.BuildingId,
                    CompanyId = model.LicencesVM.CompanyId,
                    ManId = model.LicencesVM.ManagerId,
                    MarketingManagerId = model.LicencesVM.MarketingManagerId,
                    SalesManagerId = model.LicencesVM.SalesManagerId,
                    OperationManagerId = model.LicencesVM.OperationsManagerId,
                    ActivityTypeId = model.LicencesVM.ActiivityTypeId,
                    LicId = model.LicencesVM.LicId,
                  AppEmail=model.LicencesVM.Applicant.Email,
                  AppPhone=model.LicencesVM.Applicant.Phone,
                    IssueDate=model.LicencesVM.IssueDate,
                    AppName=model.LicencesVM.Applicant.Name1,
                    ExpireDate=model.LicencesVM.ExpireDate,
                    LicNo=model.LicencesVM.LicNo,
                  UserCivilID=model.Mandoob.CivilId,
                  
                  UserName=model.Mandoob.UserName,
                  MandoobEmail=model.Mandoob.Email,
                  MandoobPhone=model.Mandoob.Mobile,

                    AaliNumber = model.LicencesVM.Company.AddressNavigation.AalliNo,
                    //ReqtypeId = model.RequestVM.ReqtypeId,
                    //ActivityCode = model.RequestVM.ActivityCode,

                    AppCivilId = model.LicencesVM.Applicant.CivilId,

                    SalesManagerCivilId = model.LicencesVM.SalesManager.CivilId,
                    SalesManagerEmail = model.LicencesVM.SalesManager.Email,
                    SalesManagerName = model.LicencesVM.SalesManager.Name1,
                    SalesManagerPhone = model.LicencesVM.SalesManager.Phone,

                    MarketingManagerCivilId = model.LicencesVM.MarketingManager.CivilId,
                    MarketingManagerEmail = model.LicencesVM.MarketingManager.Email,
                    MarketingManagerName = model.LicencesVM.MarketingManager.Name1,
                    MarketingManagerPhone = model.LicencesVM.MarketingManager.Phone,

                    OperationManagerCivilId = model.LicencesVM.OperationsManager.CivilId,
                    OperationManagerEmail = model.LicencesVM.OperationsManager.Email,
                    OperationManagerName = model.LicencesVM.OperationsManager.Name1,
                    OperationManagerPhone = model.LicencesVM.OperationsManager.Phone,

                    LicencesName = model.LicencesVM.LicName,

                    Area = model.LicencesVM.Company.AddressNavigation.Area,
                    AreaSize = model.LicencesVM.Company.AddressNavigation.AreaSize,
                    AreaChartNo = model.LicencesVM.Company.AddressNavigation.AreaChartNo,
                    BlockNo = model.LicencesVM.Company.AddressNavigation.BlockArabic,
                    BuildingNo = model.LicencesVM.Company.AddressNavigation.BuildingNo,

                    ManagerEmail = model.LicencesVM.Manager.Email,
                    ManagerName = model.LicencesVM.Manager.Name1,
                    ManagerMobile = model.LicencesVM.Manager.Phone,
                    ManCivilId = model.LicencesVM.Manager.CivilId,

                    CompanyCivilId = model.LicencesVM.Company.CompanyCivilId,

                    DirCompanyAr = model.LicencesVM.Company.DirCompanyAr,
                    BuildingName = model.LicencesVM.Company.AddressNavigation.BuildingName,
                    Street = model.LicencesVM.Company.AddressNavigation.StreetArabic,
                    Governrate = model.LicencesVM.Company.AddressNavigation.GovernorateArabic,
                    OwnerCompanyAr = model.LicencesVM.Company.OwnerCompanyAr,

                    RecordNo = model.LicencesVM.RecordNo,
                    CommercialLicNo = model.LicencesVM.CommercialLicNo,


                    UnitNo = model.LicencesVM.Company.AddressNavigation.UnitNo,
                    FloorNo = model.LicencesVM.Company.AddressNavigation.FloorNo,

                    
                };

                var result = await _helperUrlApi.PostDataToApi<PreApprovalRequestApiModel, ErrorMessage>(apiUrl, ApiModel);

                // Redirect or show confirmation
                TempData["Success"] = "تم تحديث البيانات بنجاح.";
                return RedirectToAction("GetAllLicencesForEdit","Tourism");
            }
            catch (Exception ex)
            {
                string fileName = "Admin_UpdateLicenseInfo_";
                string exId = ExceptionLog.LogException(ex, fileName);
                TempData["Ex"] = exId;
                throw;
            }
        }
        public async Task<ActionResult> GetAllLicencesPreApprovement()
        {
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }

            var apiSettings = _baseUrl + $"api/AdminTourism";
            ViewBag.ApiBaseUrl = apiSettings;
            var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            if (loggedInUser)
            {
                var url = $"api/AdminTourism/GetAllLicencesPreApprove";
                var Licences = await _helperUrlApi.GetDataFromApi<List<PreApprovementVM>>(url);
                return View(Licences);

            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
        }
        public async Task<ActionResult> GetLicencePreApprovementDetails(int id)
        {
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }

            var apiSettings = _baseUrl + $"api/AdminTourism";
            ViewBag.ApiBaseUrl = apiSettings;
            var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            if (loggedInUser)
            {
                var licenes = $"api/AdminTourism/GetLicencePreApproveById?licId={id}";
                var licencesDetails = await _helperUrlApi.GetDataFromApi<PreApproveDetails>(licenes);
                return View(licencesDetails);
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
        }


        public async Task<ActionResult> RequestDetails(int? ID)
        {
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            var userid = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.UserData)?.Value;

            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            var apiSettings = _baseUrl + $"api/AdminTourism";
            var dynamicUrl = _baseUrl + $"Dynamic";
            ViewBag.ApiBaseUrl = apiSettings;
            ViewBag.DynamicUrlApi = dynamicUrl;
            ViewBag.RequestTypes = JsonConvert.SerializeObject(Enum.GetValues(typeof(RequestTypeEnum))
                                    .Cast<RequestTypeEnum>()
                                    .ToDictionary(e => e.ToString(), e => (int)e));

            if (loggedInUser != null)
            {
                using (var client = new HttpClient())
                {

                    _httpClient.BaseAddress = new Uri(_baseUrl);

                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await _httpClient.GetAsync($"api/AdminTourism/GetRequestById?id={ID}&serviceId={(int)ServiceEnum.Tourism}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();

                        var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestDetailsVM>(jsonData);


                        return View(responseData);
                    }
                    else
                    {

                        return RedirectToAction("ErrorPage", "Home");
                    }

                }
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
        }
       
        public async Task<ActionResult> RequestOperatingLicenseDetails(int id)
        {
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            var userid = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.UserData)?.Value;
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            var apiSettings = _baseUrl + $"api/AdminTourism";
            var dynamicUrl = _baseUrl + $"Dynamic";
            ViewBag.ApiBaseUrl = apiSettings;
            ViewBag.DynamicUrlApi = dynamicUrl;
            ViewBag.RequestTypes = JsonConvert.SerializeObject(Enum.GetValues(typeof(RequestTypeEnum))
                                    .Cast<RequestTypeEnum>()
                                    .ToDictionary(e => e.ToString(), e => (int)e));

            if (loggedInUser != null)
            {
                using (var client = new HttpClient())
                {

                    _httpClient.BaseAddress = new Uri(_baseUrl);

                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await _httpClient.GetAsync($"api/AdminTourism/GetRequestById?id={id}&serviceId={(int)ServiceEnum.Tourism}&userId={userid}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();

                        var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestDetailsVM>(jsonData);


                        return View(responseData);
                    }
                    else
                    {

                        return RedirectToAction("ErrorPage", "Home");
                    }

                }
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
        }
        public async Task<ActionResult> RequestTransactionDetails(int id)
        {
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            var userid = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.UserData)?.Value;

            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            var apiSettings = _baseUrl + $"api/AdminTourism";
            var dynamicUrl = _baseUrl + $"Dynamic";
            ViewBag.ApiBaseUrl = apiSettings;
            ViewBag.DynamicUrlApi = dynamicUrl;
            ViewBag.RequestTypes = JsonConvert.SerializeObject(Enum.GetValues(typeof(RequestTypeEnum))
                                    .Cast<RequestTypeEnum>()
                                    .ToDictionary(e => e.ToString(), e => (int)e));

            if (loggedInUser != null)
            {
                using (var client = new HttpClient())
                {

                    _httpClient.BaseAddress = new Uri(_baseUrl);

                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var apsettingApi = _baseUrl + $"api/AdminTourism/GetRequestById?id={id}&serviceId={(int)ServiceEnum.Tourism}";
                    var response =await _helperUrlApi.GetDataFromApi<RequestDetailsVM>(apsettingApi);

                    return View(response);

                 

                }
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
        }
        public async Task<ActionResult> RequestWhoConcDetails(int id)
        {
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            var apiSettings = _baseUrl + $"api/AdminTourism";
            var dynamicUrl = _baseUrl + $"Dynamic";
            ViewBag.ApiBaseUrl = apiSettings;
            ViewBag.DynamicUrlApi = dynamicUrl;
            ViewBag.RequestTypes = JsonConvert.SerializeObject(Enum.GetValues(typeof(RequestTypeEnum))
                                    .Cast<RequestTypeEnum>()
                                    .ToDictionary(e => e.ToString(), e => (int)e));

            if (loggedInUser != null)
            {
                using (var client = new HttpClient())
                {

                    _httpClient.BaseAddress = new Uri(_baseUrl);

                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await _httpClient.GetAsync($"api/AdminTourism/GetRequestById?id={id}&serviceId={(int)ServiceEnum.Tourism}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();

                        var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestDetailsVM>(jsonData);


                        return View(responseData);
                    }
                    else
                    {

                        return RedirectToAction("ErrorPage", "Home");
                    }

                }
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
        }
        public async Task<ActionResult> RequestMOICDetails(int id)
        {
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            var userid = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.UserData)?.Value;

            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            var apiSettings = _baseUrl + $"api/AdminTourism";
            var dynamicUrl = _baseUrl + $"Dynamic";
            ViewBag.ApiBaseUrl = apiSettings;
            ViewBag.DynamicUrlApi = dynamicUrl;
            ViewBag.RequestTypes = JsonConvert.SerializeObject(Enum.GetValues(typeof(RequestTypeEnum))
                                    .Cast<RequestTypeEnum>()
                                    .ToDictionary(e => e.ToString(), e => (int)e));

            if (loggedInUser != null)
            {
                using (var client = new HttpClient())
                {

                    _httpClient.BaseAddress = new Uri(_baseUrl);

                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await _httpClient.GetAsync($"api/AdminTourism/GetRequestById?id={id}&serviceId={(int)ServiceEnum.Tourism}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();

                        var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestDetailsVM>(jsonData);


                        return View(responseData);
                    }
                    else
                    {

                        return RedirectToAction("ErrorPage", "Home");
                    }

                }
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
        }
        #region Forms
        public async Task<IActionResult> AddForm()
        {
            var apiSettings = _baseUrl + $"api/AdminTourism/GetForms";

            var getform =await _helperUrlApi.GetDataFromApi<List<FormsViewModel>>(apiSettings);
            if(getform==null)
            {
                getform = new List<FormsViewModel>(); // Initialize an empty list if no data is returned

            }

            return View(getform);
        }
       
        [HttpPost]
        public async Task<IActionResult> AddForm( IFormFile UploadedFile,string fileName)
        {
            if (UploadedFile == null || UploadedFile.Length == 0)
            {
                ModelState.AddModelError("UploadedFile", "Please upload a valid file.");
                return View("Index"); // Or redirect back with validation errors
            }

            try
            {
                var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
                if (!loggedInUser)
                {
                    // Handle case where user is not logged in
                    return RedirectToAction("Login", "Account");
                }
                var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
                if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
                {
                    return RedirectToAction("ErrorPage", "Home");
                }

                var apiSettings = _baseUrl + $"api/AdminTourism/SaveForms";
                string pathForm = Path.Combine(_file, "Forms");
                // Use the existing function to save the file
                var response = await SaveFileToDiskAsync(UploadedFile, fileName, pathForm,null);

                //// Save form details to the database
                //SaveFormToDatabase(ModelName, response.FilePath);
                var requestData = new FormsViewModel
                {
                    FormPath = response.FilePath,
                    FormName = response.FileName,
                    ServiceId=(int)ServiceEnum.Tourism,
                    IsDeleted=false,
                    FormType=".pdf"
                };
                var requesttoapi = await _helperUrlApi.PostDataToApi<FormsViewModel, FormsViewModel>(
                           apiSettings,
                           requestData
                       );
                TempData["SuccessMessage"] = "Form added successfully!";
                return RedirectToAction("AddForm");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the file. Please try again.";
                Console.WriteLine(ex.Message); // Log the exception
            }

            return View("AddForm");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteForm(int formId)
        {
            var apiSettings = _baseUrl + $"api/AdminTourism/DeleteForm/{formId}";

            var form = _helperUrlApi.GetDataFromApi<FormsViewModel>(apiSettings);

            return RedirectToAction("AddForm");
        }
        #endregion

        public async Task<ActionResult> RequestClassificationDetails(int id)
        {
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            var apiSettings = _baseUrl + $"api/AdminTourism";
            var dynamicUrl = _baseUrl + $"Dynamic";
            ViewBag.ApiBaseUrl = apiSettings;
            ViewBag.DynamicUrlApi = dynamicUrl;
            ViewBag.RequestTypes = JsonConvert.SerializeObject(Enum.GetValues(typeof(RequestTypeEnum))
                                    .Cast<RequestTypeEnum>()
                                    .ToDictionary(e => e.ToString(), e => (int)e));

            if (loggedInUser != null)
            {
                using (var client = new HttpClient())
                {

                    _httpClient.BaseAddress = new Uri(_baseUrl);

                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await _httpClient.GetAsync($"api/AdminTourism/GetRequestById?id={id}&serviceId={(int)ServiceEnum.Tourism}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();

                        var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestDetailsVM>(jsonData);


                        return View(responseData);
                    }
                    else
                    {

                        return RedirectToAction("ErrorPage", "Home");
                    }

                }
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
        }


        public async Task<ActionResult> PrintPreApprovement(int Id)
        {
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            int serviceId = 6;
            var apiSettings = _baseUrl + $"api/AdminTourism";
            var dynamicUrl = _baseUrl + $"Dynamic";
            ViewBag.ApiBaseUrl = apiSettings;
            ViewBag.DynamicUrlApi = dynamicUrl;
            if (loggedInUser != null)
            {
                using (var client = new HttpClient())
                {

                    _httpClient.BaseAddress = new Uri(_baseUrl);

                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await _httpClient.GetAsync($"api/AdminTourism/GetRequestById?id={Id}&serviceId={(int)ServiceEnum.Tourism}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();

                        var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestDetailsVM>(jsonData);


                        return View(responseData);
                    }
                    else
                    {

                        return RedirectToAction("ErrorPage", "Home");
                    }

                }
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
        }
        #endregion

        #region طلبات الأنشطة السياحية
       
        public async Task<ActionResult> RequestTourismActivityDetails(int? ID)
        {
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            var apiSettings = _baseUrl + $"api/AdminTourism";
            ViewBag.ApiBaseUrl = apiSettings;
            if (loggedInUser)
            {
                using (var client = new HttpClient())
                {

                    _httpClient.BaseAddress = new Uri(_baseUrl);

                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await _httpClient.GetAsync($"api/AdminTourism/GetRequestbyTypeById?id={ID}&serviceId={serviceId}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();

                        var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestDetailsVM>(jsonData);


                        return View(responseData);
                    }
                    else
                    {

                        return RedirectToAction("ErrorPage", "Home");
                    }

                }
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
        }
        #endregion

        #region Function Attachment
        public async Task<FileSaveResponseVM> SaveFileToDiskAsync(IFormFile file, string fileNameFromFile, string relativePath, string? reqNo)
        {
            string filepath = Path.Combine(_env.WebRootPath, relativePath);
            string uploadsFolder;
            if (!string.IsNullOrEmpty(reqNo))
            {
                uploadsFolder = Path.Combine(_env.WebRootPath, relativePath, reqNo);
            }else
            {
                uploadsFolder = Path.Combine(_env.WebRootPath, relativePath);
            }

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder); // Create directory if it doesn't exist
            }
            string fileName;

            // Generate the sequence number and file name
            if (!string.IsNullOrEmpty(reqNo))
            {
                string _Reqno = reqNo + "/AttachNo-";
                Random random = new Random();
                int sequenceNumber = random.Next(100, 1000); // Generating a random number for sequence
                fileName = $"{_Reqno}{sequenceNumber}.pdf"; // AttachNo-{sequenceNumber}.pdf
            }
            else
            {
                fileName= $"{fileNameFromFile}.pdf";
            }
            string filePath = Path.Combine(filepath, fileName);
            string filePathWithoutSlash = filePath.Replace("/", "\\"); // Replace / with backslash for Windows compatibility

            try
            {
                // Save the file asynchronously
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream); // Copy file to the disk
                }

                return new FileSaveResponseVM
                {
                    FilePath = fileName,
                    FileName = fileNameFromFile
                };
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine("Error: " + ex.Message);
                throw; // Rethrow exception or handle accordingly
            }


        }
        [HttpPost]
        public async Task<ActionResult> SaveFile([FromForm] fileAttach file)
        {
            var referrer = HttpContext.Request.Headers["Referer"].ToString();
            string pageName = string.IsNullOrEmpty(referrer)
                ? "Unknown"
                : new Uri(referrer).AbsolutePath;
            pageName = pageName.Substring(pageName.LastIndexOf("/") + 1);
            int serviceId = (int)ServiceEnum.Tourism;
            // Retrieve user data from session
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            string logs = "";
            
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var username = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;

            var userid = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.UserData)?.Value;
            // Initialize API settings
            var apiSettings = _baseUrl + "api/AdminTourism";
            ViewBag.ApiBaseUrl = apiSettings;
            var apisettingsDynamic = _baseUrl + "Dynamic";
           
            
            List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();

            List<string> fileNames = new List<string>();
            ViewBag.ApiBaseUrl = apiSettings;
            if (loggedInUser)
            {
                using (var client = new HttpClient())
                {
                    foreach (var fileAttach in file.files)
                    {
                        if (fileAttach.Files != null)
                        {
                            // Save each file to disk
                            var savedFilePath = await SaveFileToDiskAsync(fileAttach.Files, fileAttach.filename, _file, file.ReqNo);
                          
                            filePath.Add(savedFilePath);
                            fileNames.Add(fileAttach.filename);
                            Console.WriteLine($"File saved at: {savedFilePath}");

                        }
                    }
                    List<string> changeLogs = new List<string>
                        {
                            "Files uploaded: " + string.Join(", ", fileNames)
                        }; 
                    var requestData = new UpdatedRequestVM
                    {
                        
                        RequestId = file.RequestId??0,
                       
                        saveResponseVMs = filePath,
                       
                        ServiceId=serviceId,
                        NameUser = username,
                        ActionName = pageName,
                        UserId = int.Parse(userid),

                        ChangeLogs = changeLogs,
                        Action = "AddAdditionalFiles",
                        

                    };
                    var jsonContent = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var updateResponse = await client.PostAsync($"{apiSettings}/SaveAttachmentAdditional", content);

                    if (!updateResponse.IsSuccessStatusCode)
                    {
                        return StatusCode((int)updateResponse.StatusCode, "Failed to update request status");
                    }

                    var responseData = await updateResponse.Content.ReadAsStringAsync();
                    //var updatedRequest = JsonConvert.DeserializeObject<dynamic>(responseData);

                    // Return the updated request data
                    return Ok(new
                    {
                        UpdatedRequest = responseData,
                      
                    });
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
            return null;
        }

        #endregion


        #region AllRequests
        //______________________NewOne Include All_______________________//
        private async Task<List<RequestVM>> FetchRequestsAsync(int serviceId, List<RequestTypeEnum> requestTypes, List<ActivityTypeEnum> activityTypes = null)
        {
            try
            {
                // Build request types parameter
                var requestTypeIds = string.Join(",", requestTypes.Select(rt => (int)rt));
                var requestUrl = $"api/AdminTourism/GetRequests?serviceId={serviceId}&requestTypes={requestTypeIds}";

                // Append activity types if provided
                if (activityTypes != null && activityTypes.Any())
                {
                    var activityTypeNames = string.Join(",", activityTypes.Select(at => (int)at));
                    requestUrl += $"&activityTypeNames={activityTypeNames}";
                }

                // Fetch data using the helper method
                var response = await _helperUrlApi.GetDataFromApi<List<RequestVM>>(requestUrl);

                return response ?? new List<RequestVM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching requests.");
                return new List<RequestVM>();
            }
        }
        public async Task<IActionResult> HandleRequests(string viewName, List<RequestTypeEnum> requestTypes, List<ActivityTypeEnum> activityTypes = null)
        {
            try
            {
                // Check user authentication
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Extract ServiceId from user claims
                var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
                if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
                {
                    return RedirectToAction("ErrorPage", "Home");
                }

                // Fetch requests using the existing FetchRequestsAsync method
                var requests = await FetchRequestsAsync((int)ServiceEnum.Tourism, requestTypes, activityTypes);

                // Return the specified view with the retrieved data
                return View(viewName, requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling requests.");
                return RedirectToAction("ErrorPage", "Home");
            }
        }
        public async Task<IActionResult> PreApprovementRequests()
        {
            return await HandleRequests(
                        "PreApprovementRequests",
                        new List<RequestTypeEnum>
                        {
                            RequestTypeEnum.PreApprovementNew,
                            RequestTypeEnum.PreApprovementConvert
                        },
                        new List<ActivityTypeEnum>
                        {
                            ActivityTypeEnum.Hotel,
                            ActivityTypeEnum.ApartmentHotel,
                            ActivityTypeEnum.Resorts
                        }
                    );
        }
        public async Task<IActionResult> GetRenouncementRequests()
        {
            return await HandleRequests(
                "GetRenouncementRequests",
                new List<RequestTypeEnum> { RequestTypeEnum.Renouncement },
                new List<ActivityTypeEnum> { ActivityTypeEnum.Hotel,
                ActivityTypeEnum.ApartmentHotel,ActivityTypeEnum.Sailing,ActivityTypeEnum.Resorts,ActivityTypeEnum.Parks }
            );
        }
        public async Task<IActionResult> GetEndLicencesRequests()
        {
            return await HandleRequests(
                "GetEndLicencesRequests",
                new List<RequestTypeEnum> { RequestTypeEnum.EndLicences },
                new List<ActivityTypeEnum> { ActivityTypeEnum.Hotel,
                ActivityTypeEnum.ApartmentHotel,ActivityTypeEnum.Sailing,ActivityTypeEnum.Resorts,ActivityTypeEnum.Parks }
            );
        }
        public async Task<IActionResult> GetOperatingLicencesRequests()
        {
            return await HandleRequests(
                "GetOperatingLicencesRequests",
                new List<RequestTypeEnum> { RequestTypeEnum.Request },
                new List<ActivityTypeEnum> { ActivityTypeEnum.Hotel, ActivityTypeEnum.ApartmentHotel, ActivityTypeEnum.Resorts }
            );
        }

        public async Task<IActionResult> GetParksRequest()
        {
            return await HandleRequests(
                "GetParksRequest",
                new List<RequestTypeEnum> { RequestTypeEnum.Request },
                new List<ActivityTypeEnum> { ActivityTypeEnum.Parks, ActivityTypeEnum.Sailing }
            );
        }

        public async Task<IActionResult> GetSailingRequest()
        {
            return await HandleRequests(
                "GetSailingRequest",
                new List<RequestTypeEnum> { RequestTypeEnum.Request },
                new List<ActivityTypeEnum> { ActivityTypeEnum.Sailing, ActivityTypeEnum.Parks }
            );
        }

        public async Task<IActionResult> GetClassificationRequest()
        {
            return await HandleRequests(
                "GetClassificationRequest",
                new List<RequestTypeEnum> { RequestTypeEnum.Classification, RequestTypeEnum.ReClassification },
                new List<ActivityTypeEnum> { ActivityTypeEnum.Hotel, ActivityTypeEnum.ApartmentHotel,ActivityTypeEnum.Resorts }
            );
        }

        public async Task<IActionResult> GetWhoConcRequest()
        {
            return await HandleRequests(
                "GetWhoConcRequest",
                new List<RequestTypeEnum> { RequestTypeEnum.WhoConc }
            );
        }

        public async Task<IActionResult> GetRenewRequest()
        {
            return await HandleRequests(
                "GetRenewRequest",
                new List<RequestTypeEnum> { RequestTypeEnum.Renew }
            );
        }

        public async Task<IActionResult> GetChangeRequest()
        {
            return await HandleRequests(
                "GetChangeRequest",
                new List<RequestTypeEnum> { RequestTypeEnum.ChangeData }
            );
        }
        public async Task<IActionResult> GetMOICRequest()
        {
            return await HandleRequests(
                "GetMOICRequest",
                new List<RequestTypeEnum> { RequestTypeEnum.DeleteMOIC,
                RequestTypeEnum.AddMoIC,
                RequestTypeEnum.RenewMOIC,
                RequestTypeEnum.ChangeAddressMOIC,
                RequestTypeEnum.RenewOrChangeMOIC},
                new List<ActivityTypeEnum> { ActivityTypeEnum.Hotel,
                ActivityTypeEnum.ApartmentHotel,ActivityTypeEnum.Sailing,ActivityTypeEnum.Resorts,ActivityTypeEnum.Parks}
            );
           
        }

        #endregion

        #region SaveData
        public async Task<ActionResult> SaveData([FromForm] SaveDataViewModel model)
        {
            // Extract page name from the referrer
            var referrer = HttpContext.Request.Headers["Referer"].ToString();
            string pageName = string.IsNullOrEmpty(referrer)
                ? "Unknown"
                : new Uri(referrer).AbsolutePath;
            pageName = pageName.Substring(pageName.LastIndexOf("/") + 1);
            
            long SequenceNo = 0;

            // Retrieve user data from session
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            string logs = "";
            if (model.ChangeLogs != null)
            {
                logs = string.Join(", ", model.ChangeLogs);  // You can change the separator as needed
            }
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var username = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;
            var ServiceId = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;

            var userid = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.UserData)?.Value;
            // Initialize API settings
            var apiSettings = _baseUrl + "api/AdminTourism";
            ViewBag.ApiBaseUrl = apiSettings;
            var apisettingsDynamic = _baseUrl + "Dynamic";
            // Initialize default values
            string licNo = "";
            string nextStatusName = "";
           List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();
            int licStatusId = (int)licencesStatusEnum.Pending;

            try
            {
                // Create a new HttpClient instance for each request
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //var hotelClassIdsList = JsonConvert.DeserializeObject<List<int>>(model.HotelClassIds);
                    //var evaluationIdsList = JsonConvert.DeserializeObject<List<int>>(model.EvaluationIds);
                    //var valuesList = JsonConvert.DeserializeObject<List<string>>(model.Values);
                    UpdatedRequestVM requestData;
                    // Fetch the next status dynamically from the workflow/*activityTypeId={model.ActivityTypeId}&*/
                    if (model.Action == "RequestStatusbutton")
                    {
                        var workflowResponse = await client.GetAsync($"{apisettingsDynamic}/GetNextStatus?serviceId={(int)ServiceEnum.Tourism}&requestTypeId={model.ReqTypeId}&currentStatusId={model.ReqStatusId}");
                        if (!workflowResponse.IsSuccessStatusCode)
                        {
                            return StatusCode((int)workflowResponse.StatusCode, "Failed to fetch next status");
                        }

                        var workflowData = await workflowResponse.Content.ReadAsStringAsync();
                        var workflowResult = JsonConvert.DeserializeObject<dynamic>(workflowData);





                        if (workflowResult.success == false)
                        {
                            return BadRequest(workflowResult.message.ToString());
                        }
                        Console.WriteLine($"Conditions: {workflowResult.conditions}");
                        Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");
                        // Extract conditions JSON as a string
                        //if(workflowResult.)
                        string conditionsJson = workflowResult.conditions?.ToString() ?? string.Empty;

                        // Declare variables outside the if block
                        string requestStatusValue = null;
                        string requestTypeValue = null;

                        // Check if conditionsJson is not null or empty before proceeding
                        if (!string.IsNullOrEmpty(conditionsJson))
                        {
                            Console.WriteLine($"Conditions: {workflowResult.conditions}");
                            Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");

                            // Deserialize conditions JSON
                            var conditions = JsonConvert.DeserializeObject<List<Condition>>(conditionsJson);

                            // Instantiate the evaluator
                            ConditionEvaluator conditionEvaluator = new ConditionEvaluator();

                            // Evaluate the conditions and extract values
                            var (areConditionsMet, extractedValues) = conditionEvaluator.Evaluate(conditionsJson, new
                            {
                                RequestStatus = "Prefinal",  // Assuming ReqStatusId represents the current request status
                                RequestType = "Preapprove" // Assuming ReqTypeId represents the current request type
                            });

                            // Extract specific values
                            requestStatusValue = extractedValues.ContainsKey("RequestStatus") ? extractedValues["RequestStatus"] : null;
                            requestTypeValue = extractedValues.ContainsKey("RequestType") ? extractedValues["RequestType"] : null;

                            Console.WriteLine($"RequestStatusValue: {requestStatusValue}");
                            Console.WriteLine($"RequestTypeValue: {requestTypeValue}");
                        }

                        // Now you can use requestStatusValue and requestTypeValue outside the if block
                        Console.WriteLine($"Outside If - RequestStatusValue: {requestStatusValue}");
                        Console.WriteLine($"Outside If - RequestTypeValue: {requestTypeValue}");

                        // Extract the next status
                        int nextStatusId = workflowResult.nextStatusId;
                        nextStatusName = workflowResult.nextStatusName;
                        string flag = workflowResult.flag;
                        var url = $"{apisettingsDynamic}/GetAllowedButtons?requestId={model.RequestId}&NextStatusId={workflowResult.nextStatusId}&userId={userid}";
                        var GetAccessUser = await _helperUrlApi.GetDataFromApiNewHttpClient<bool>(url);
                        if (flag == "LicencesNo"|| flag == "final")
                        {
                            var licenseResult = await _generateLicNo.GenerateUniqueLicenseNumberTourism((int)ServiceEnum.Tourism,model.ReqTypeId, model.ActivityTypeId);
                            licNo = licenseResult.Item2;
                            SequenceNo = licenseResult.Item1;
                        }
                        // Additional logic for specific statuses
                        if (model.files != null && model.files.Count > 0)
                        {
                            foreach (var file in model.files)
                            {
                                if (file.Files != null)
                                {
                                    // Save each file to disk
                                    var savedFilePath = await SaveFileToDiskAsync(file.Files, file.filename, _file, model.ReqNo);
                                    savedFilePath.IsRequired = file.ismandatory;
                                    savedFilePath.FieldName = file.fieldname;
                                    filePath.Add(savedFilePath);
                                    // Perform any additional logic with the saved file path, if needed
                                    Console.WriteLine($"File saved at: {savedFilePath}");

                                }
                            }
                        }
                            // Create request data
                            requestData = new UpdatedRequestVM
                        {
                            LicNo = licNo,
                            StatusId = nextStatusId,
                            RequestId = model.RequestId,
                            SequenceNo=SequenceNo,
                                //FilePath = filePath.FilePath,
                                //FileName = filePath.FileName,
                             saveResponseVMs=filePath,
                            ReqTypeId = model.ReqTypeId,
                            LicStatusId = licStatusId,
                            Note = model.Note,
                            NameUser = username,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            requestStatusValue = requestStatusValue,
                            requestTypeValue = requestTypeValue,
                           
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                            selectedAttachments = model.selectedAttachments,
                            uncheckedAttachments = model.uncheckedAttachments,
                            
                            AttachmentStates = model.allAttachmentsState,


                        };
                    }
                    else if (model.Action == "SendNotifyToUser")
                    {
                        var workflowResponse = await client.GetAsync($"{apisettingsDynamic}/GetCurrentStatus?serviceId={(int)ServiceEnum.Tourism}&activityTypeId={model.ActivityTypeId}&requestTypeId={model.ReqTypeId}&currentStatusId={model.ReqStatusId}");
                        if (!workflowResponse.IsSuccessStatusCode)
                        {
                            return StatusCode((int)workflowResponse.StatusCode, "Failed to fetch next status");
                        }

                        var workflowData = await workflowResponse.Content.ReadAsStringAsync();
                        var workflowResult = JsonConvert.DeserializeObject<dynamic>(workflowData);





                        if (workflowResult.success == false)
                        {
                            return BadRequest(workflowResult.message.ToString());
                        }
                        Console.WriteLine($"Conditions: {workflowResult.conditions}");
                        Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");
                        // Extract conditions JSON as a string
                        //if(workflowResult.)
                        string conditionsJson = workflowResult.conditions?.ToString() ?? string.Empty;

                        // Declare variables outside the if block
                        string requestStatusValue = null;
                        string requestTypeValue = null;

                        // Check if conditionsJson is not null or empty before proceeding
                        if (!string.IsNullOrEmpty(conditionsJson))
                        {
                            Console.WriteLine($"Conditions: {workflowResult.conditions}");
                            Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");

                            // Deserialize conditions JSON
                            var conditions = JsonConvert.DeserializeObject<List<Condition>>(conditionsJson);

                            // Instantiate the evaluator
                            ConditionEvaluator conditionEvaluator = new ConditionEvaluator();

                            // Evaluate the conditions and extract values
                            var (areConditionsMet, extractedValues) = conditionEvaluator.Evaluate(conditionsJson, new
                            {
                                RequestStatus = "Prefinal",  // Assuming ReqStatusId represents the current request status
                                RequestType = "Preapprove" // Assuming ReqTypeId represents the current request type
                            });

                            // Extract specific values
                            requestStatusValue = extractedValues.ContainsKey("RequestStatus") ? extractedValues["RequestStatus"] : null;
                            requestTypeValue = extractedValues.ContainsKey("RequestType") ? extractedValues["RequestType"] : null;

                            Console.WriteLine($"RequestStatusValue: {requestStatusValue}");
                            Console.WriteLine($"RequestTypeValue: {requestTypeValue}");
                        }

                        // Now you can use requestStatusValue and requestTypeValue outside the if block
                        Console.WriteLine($"Outside If - RequestStatusValue: {requestStatusValue}");
                        Console.WriteLine($"Outside If - RequestTypeValue: {requestTypeValue}");

                        // Extract the next status
                        int nextStatusId = workflowResult.nextStatusId;
                        nextStatusName = workflowResult.nextStatusName;
                        string flag = workflowResult.flag;
                        if (flag == "LicencesNo")
                        {
                             var resultlic= await _generateLicNo.GenerateUniqueLicenseNumberTourism((int)ServiceEnum.Tourism,model.ReqTypeId, model.ActivityTypeId);
                            licNo = resultlic.Item2;
                            SequenceNo = resultlic.Item1;
                        }
                        if ( model.files != null && model.files.Count > 0)
                        {
                            foreach (var file in model.files)
                            {
                                if (file.Files != null)
                                {
                                    // Save each file to disk
                                    var savedFilePath = await SaveFileToDiskAsync(file.Files, file.filename, _file, model.ReqNo);
                                    savedFilePath.IsRequired = file.ismandatory;
                                    savedFilePath.FieldName = file.fieldname;
                                    filePath.Add(savedFilePath);
                                    // Perform any additional logic with the saved file path, if needed
                                    Console.WriteLine($"File saved at: {savedFilePath}");

                                }
                            }
                        }
                        requestData = new UpdatedRequestVM
                        {
                            LicNo = licNo,
                            StatusId = model.ReqStatusId,
                            RequestId = model.RequestId,
                            //FilePath = filePath.FilePath,
                            //FileName = filePath.FileName,
                            saveResponseVMs=filePath,
                            ReqTypeId = model.ReqTypeId,
                            LicStatusId = licStatusId,
                            Note = model.Note,
                            NameUser = username,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            requestStatusValue = requestStatusValue,
                            requestTypeValue = requestTypeValue,
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                            selectedAttachments = model.selectedAttachments,
                            uncheckedAttachments = model.uncheckedAttachments,
                            AttachmentStates = model.allAttachmentsState

                        };
                    }
                    else
                    {
                        requestData = new UpdatedRequestVM
                        {
                            LicNo = licNo,
                            StatusId = model.ReqStatusId,
                            RequestId = model.RequestId,
                            ReqTypeId = model.ReqTypeId,
                            LicStatusId = licStatusId,
                            Note = model.Note,
                            NameUser = username,
                            //saveResponseVMs = filePath,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                            selectedAttachments = model.selectedAttachments,
                            uncheckedAttachments = model.uncheckedAttachments,
                            AttachmentStates = model.allAttachmentsState

                        };
                    }
                    // Send API request to update request status
                    var jsonContent = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var updateResponse = await client.PostAsync($"{apiSettings}/UpdateRequestStatus", content);

                    if (!updateResponse.IsSuccessStatusCode)
                    {
                        return StatusCode((int)updateResponse.StatusCode, "Failed to update request status");
                    }

                    var responseData = await updateResponse.Content.ReadAsStringAsync();
                    //var updatedRequest = JsonConvert.DeserializeObject<dynamic>(responseData);

                    // Return the updated request data
                    return Ok(new
                    {
                        UpdatedRequest = responseData,
                        Message = $"Status updated to {nextStatusName}"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while accessing the Index page.");

                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        public async Task<ActionResult> SaveDataTransaction([FromForm] SaveDataViewModelTransactonType model)
        {
            // Extract page name from the referrer
            var referrer = HttpContext.Request.Headers["Referer"].ToString();
            string pageName = string.IsNullOrEmpty(referrer)
                ? "Unknown"
                : new Uri(referrer).AbsolutePath;
            pageName = pageName.Substring(pageName.LastIndexOf("/") + 1);
            int serviceId = (int)ServiceEnum.Tourism;
            // Retrieve user data from session
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            string logs = "";
            if (model.ChangeLogs != null)
            {
                logs = string.Join(", ", model.ChangeLogs);  // You can change the separator as needed
            }
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var username = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;
            var ServiceId = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;

            var userid = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.UserData)?.Value;
            // Initialize API settings
            var apiSettings = _baseUrl + "api/AdminTourism";
            ViewBag.ApiBaseUrl = apiSettings;
            var apisettingsDynamic = _baseUrl + "Dynamic";
            // Initialize default values
            string licNo = "";
            long SequenceNo = 0;
            string nextStatusName = "";
            List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();
            int licStatusId = (int)licencesStatusEnum.Pending;

            try
            {
                // Create a new HttpClient instance for each request
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //var hotelClassIdsList = JsonConvert.DeserializeObject<List<int>>(model.HotelClassIds);
                    //var evaluationIdsList = JsonConvert.DeserializeObject<List<int>>(model.EvaluationIds);
                    //var valuesList = JsonConvert.DeserializeObject<List<string>>(model.Values);
                    UpdatedRequestVM requestData;
                    // Fetch the next status dynamically from the workflow
                    if (model.Action == "RequestStatusbutton")
                    {
                        var workflowResponse = await client.GetAsync($"{apisettingsDynamic}/GetNextStatusToTransaction?serviceId={(int)ServiceEnum.Tourism}&activityTypeId={model.ActivityTypeId}&requestTypeId={model.ReqTypeId}&currentStatusId={model.ReqStatusId}&TransId={model.transTypeId}");
                        if (!workflowResponse.IsSuccessStatusCode)
                        {
                            return StatusCode((int)workflowResponse.StatusCode, "Failed to fetch next status");
                        }

                        var workflowData = await workflowResponse.Content.ReadAsStringAsync();
                        var workflowResult = JsonConvert.DeserializeObject<dynamic>(workflowData);





                        if (workflowResult.success == false)
                        {
                            return BadRequest(workflowResult.message.ToString());
                        }
                        Console.WriteLine($"Conditions: {workflowResult.conditions}");
                        Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");
                        // Extract conditions JSON as a string
                        //if(workflowResult.)
                        string conditionsJson = workflowResult.conditions?.ToString() ?? string.Empty;

                        // Declare variables outside the if block
                        string requestStatusValue = null;
                        string requestTypeValue = null;

                        // Check if conditionsJson is not null or empty before proceeding
                        if (!string.IsNullOrEmpty(conditionsJson))
                        {
                            Console.WriteLine($"Conditions: {workflowResult.conditions}");
                            Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");

                            // Deserialize conditions JSON
                            var conditions = JsonConvert.DeserializeObject<List<Condition>>(conditionsJson);

                            // Instantiate the evaluator
                            ConditionEvaluator conditionEvaluator = new ConditionEvaluator();

                            // Evaluate the conditions and extract values
                            var (areConditionsMet, extractedValues) = conditionEvaluator.Evaluate(conditionsJson, new
                            {
                                RequestStatus = "final",  // Assuming ReqStatusId represents the current request status
                                RequestType = "Preapprove" // Assuming ReqTypeId represents the current request type
                            });

                            // Extract specific values
                            requestStatusValue = extractedValues.ContainsKey("RequestStatus") ? extractedValues["RequestStatus"] : null;
                            requestTypeValue = extractedValues.ContainsKey("RequestType") ? extractedValues["RequestType"] : null;

                            Console.WriteLine($"RequestStatusValue: {requestStatusValue}");
                            Console.WriteLine($"RequestTypeValue: {requestTypeValue}");
                        }

                        // Now you can use requestStatusValue and requestTypeValue outside the if block
                        Console.WriteLine($"Outside If - RequestStatusValue: {requestStatusValue}");
                        Console.WriteLine($"Outside If - RequestTypeValue: {requestTypeValue}");

                        // Extract the next status
                        int nextStatusId = workflowResult.nextStatusId;
                        nextStatusName = workflowResult.nextStatusName;
                        string flag = workflowResult.flag;

                        
                        // Additional logic for specific statuses
                        if (model.files != null && model.files.Count > 0)
                        {
                            foreach (var file in model.files)
                            {
                                if (file.Files != null)
                                {
                                    // Save each file to disk
                                    var savedFilePath = await SaveFileToDiskAsync(file.Files, file.filename, _file, model.ReqNo);
                                    savedFilePath.IsRequired = file.ismandatory;
                                    savedFilePath.FieldName = file.fieldname;
                                    filePath.Add(savedFilePath);
                                    // Perform any additional logic with the saved file path, if needed
                                    Console.WriteLine($"File saved at: {savedFilePath}");

                                }
                            }
                        }
                        // Create request data
                        requestData = new UpdatedRequestVM
                        {
                            LicNo = licNo,
                            StatusId = nextStatusId,
                            RequestId = model.RequestId,
                            SequenceNo=SequenceNo,
                            TransId=model.TransactionId,
                            //FilePath = filePath.FilePath,
                            //FileName = filePath.FileName,
                            saveResponseVMs = filePath,
                            ReqTypeId = model.ReqTypeId,
                            LicStatusId = licStatusId,
                            Note = model.Note,
                            NameUser = username,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            requestStatusValue = requestStatusValue,
                            requestTypeValue = requestTypeValue,
                            TransTypeId = model.transTypeId,
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                            selectedAttachments = model.selectedAttachments,
                            uncheckedAttachments = model.uncheckedAttachments,

                            // AttachmentStates = model.AttachmentState,


                        };
                    }
                    else if (model.Action == "SendNotifyToUser")
                    {
                        var workflowResponse = await client.GetAsync($"{apisettingsDynamic}/GetCurrentStatusToTransaction?serviceId={serviceId}&activityTypeId={model.ActivityTypeId}&requestTypeId={model.ReqTypeId}&currentStatusId={model.ReqStatusId}&TransId={model.transTypeId}");
                        if (!workflowResponse.IsSuccessStatusCode)
                        {
                            return StatusCode((int)workflowResponse.StatusCode, "Failed to fetch next status");
                        }

                        var workflowData = await workflowResponse.Content.ReadAsStringAsync();
                        var workflowResult = JsonConvert.DeserializeObject<dynamic>(workflowData);





                        if (workflowResult.success == false)
                        {
                            return BadRequest(workflowResult.message.ToString());
                        }
                        Console.WriteLine($"Conditions: {workflowResult.conditions}");
                        Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");
                        // Extract conditions JSON as a string
                        //if(workflowResult.)
                        string conditionsJson = workflowResult.conditions?.ToString() ?? string.Empty;

                        // Declare variables outside the if block
                        string requestStatusValue = null;
                        string requestTypeValue = null;

                        // Check if conditionsJson is not null or empty before proceeding
                        if (!string.IsNullOrEmpty(conditionsJson))
                        {
                            Console.WriteLine($"Conditions: {workflowResult.conditions}");
                            Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");

                            // Deserialize conditions JSON
                            var conditions = JsonConvert.DeserializeObject<List<Condition>>(conditionsJson);

                            // Instantiate the evaluator
                            ConditionEvaluator conditionEvaluator = new ConditionEvaluator();

                            // Evaluate the conditions and extract values
                            var (areConditionsMet, extractedValues) = conditionEvaluator.Evaluate(conditionsJson, new
                            {
                                RequestStatus = "final",  // Assuming ReqStatusId represents the current request status
                                RequestType = "Preapprove" // Assuming ReqTypeId represents the current request type
                            });

                            // Extract specific values
                            requestStatusValue = extractedValues.ContainsKey("RequestStatus") ? extractedValues["RequestStatus"] : null;
                            requestTypeValue = extractedValues.ContainsKey("RequestType") ? extractedValues["RequestType"] : null;

                            Console.WriteLine($"RequestStatusValue: {requestStatusValue}");
                            Console.WriteLine($"RequestTypeValue: {requestTypeValue}");
                        }

                        // Now you can use requestStatusValue and requestTypeValue outside the if block
                        Console.WriteLine($"Outside If - RequestStatusValue: {requestStatusValue}");
                        Console.WriteLine($"Outside If - RequestTypeValue: {requestTypeValue}");

                        // Extract the next status
                        int nextStatusId = workflowResult.nextStatusId;
                        nextStatusName = workflowResult.nextStatusName;
                        string flag = workflowResult.flag;
                        
                        if ( model.files != null && model.files.Count > 0)
                        {
                            foreach (var file in model.files)
                            {
                                if (file.Files != null)
                                {
                                    // Save each file to disk
                                    var savedFilePath = await SaveFileToDiskAsync(file.Files, file.filename, _file, model.ReqNo);
                                    savedFilePath.IsRequired = file.ismandatory;
                                    savedFilePath.FieldName = file.fieldname;
                                    filePath.Add(savedFilePath);
                                    // Perform any additional logic with the saved file path, if needed
                                    Console.WriteLine($"File saved at: {savedFilePath}");

                                }
                            }
                        }
                        requestData = new UpdatedRequestVM
                        {
                            LicNo = licNo,
                            StatusId = model.ReqStatusId,
                            RequestId = model.RequestId,
                            //FilePath = filePath.FilePath,
                            //FileName = filePath.FileName,
                            saveResponseVMs = filePath,
                            ReqTypeId = model.ReqTypeId,
                            LicStatusId = licStatusId,
                            TransTypeId = model.transTypeId,
                            Note = model.Note,
                            NameUser = username,
                            ActionName = pageName,
                             TransId=model.TransactionId,
                            UserId = int.Parse(userid),
                            requestStatusValue = requestStatusValue,
                            requestTypeValue = requestTypeValue,
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                            selectedAttachments = model.selectedAttachments,
                            uncheckedAttachments = model.uncheckedAttachments,
                            // AttachmentStates = model.AttachmentState

                        };
                    }
                    else
                    {
                        requestData = new UpdatedRequestVM
                        {
                            LicNo = licNo,
                            StatusId = model.ReqStatusId,
                            RequestId = model.RequestId,
                            ReqTypeId = model.ReqTypeId,
                            LicStatusId = licStatusId,
                            TransId = model.TransactionId,
                            TransTypeId=model.transTypeId,
                            Note = model.Note,
                            NameUser = username,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                            selectedAttachments = model.selectedAttachments,
                            uncheckedAttachments = model.uncheckedAttachments,
                            // AttachmentStates = model.AttachmentState

                        };
                    }
                    // Send API request to update request status
                    var jsonContent = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var updateResponse = await client.PostAsync($"{apiSettings}/UpdateRequestStatus", content);

                    if (!updateResponse.IsSuccessStatusCode)
                    {
                        return StatusCode((int)updateResponse.StatusCode, "Failed to update request status");
                    }

                    var responseData = await updateResponse.Content.ReadAsStringAsync();
                    //var updatedRequest = JsonConvert.DeserializeObject<dynamic>(responseData);

                    // Return the updated request data
                    return Ok(new
                    {
                        UpdatedRequest = responseData,
                        Message = $"Status updated to {nextStatusName}"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while accessing the Index page.");

                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        public async Task<ActionResult> SaveDataClassification([FromForm] SaveDataClassificationViewModel model)
        {
            // Extract page name from the referrer
            var referrer = HttpContext.Request.Headers["Referer"].ToString();
            string pageName = string.IsNullOrEmpty(referrer)
                ? "Unknown"
                : new Uri(referrer).AbsolutePath;
            pageName = pageName.Substring(pageName.LastIndexOf("/") + 1);
           
            // Retrieve user data from session
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            string logs = "";
            if (model.ChangeLogs != null)
            {
                logs = string.Join(", ", model.ChangeLogs);  // You can change the separator as needed
            }
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var username = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;
            var ServiceId = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;

            var userid = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.UserData)?.Value;
            // Initialize API settings
            var apiSettings = _baseUrl + "api/AdminTourism";
            ViewBag.ApiBaseUrl = apiSettings;
            var apisettingsDynamic = _baseUrl + "Dynamic";
            // Initialize default values
            string licNo = "";
            string nextStatusName = "";
            long SequenceNo = 0;
            List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();

            //FileSaveResponseVM filePath = new FileSaveResponseVM();
            // int licStatusId = 1;

            try
            {
                // Create a new HttpClient instance for each request
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var hotelClassIds = model.HotelClassIds;
                    var evaluationIds = model.EvaluationIds;
                   
                    var values = model.Values;

                    //var hotelClassIdsList = JsonConvert.DeserializeObject<List<int>>(model.HotelClassIds);
                    //var evaluationIdsList = JsonConvert.DeserializeObject<List<int>>(model.EvaluationIds);
                    //var valuesList = JsonConvert.DeserializeObject<List<string>>(model.Values);
                    UpdatedRequestVM requestData;
                    // Fetch the next status dynamically from the workflow
                    if (model.Action == "RequestStatusbutton")
                    {
                        var workflowResponse = await client.GetAsync($"{apisettingsDynamic}/GetNextStatus?serviceId={(int)ServiceEnum.Tourism}&activityTypeId={model.ActivityTypeId}&requestTypeId={model.ReqTypeId}&currentStatusId={model.ReqStatusId}");
                        if (!workflowResponse.IsSuccessStatusCode)
                        {
                            return StatusCode((int)workflowResponse.StatusCode, "Failed to fetch next status");
                        }

                        var workflowData = await workflowResponse.Content.ReadAsStringAsync();
                        var workflowResult = JsonConvert.DeserializeObject<dynamic>(workflowData);





                        if (workflowResult.success == false)
                        {
                            return BadRequest(workflowResult.message.ToString());
                        }
                        Console.WriteLine($"Conditions: {workflowResult.conditions}");
                        Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");
                        // Extract conditions JSON as a string
                        //if(workflowResult.)
                        string conditionsJson = workflowResult.conditions?.ToString() ?? string.Empty;

                        // Declare variables outside the if block
                        string requestStatusValue = null;
                        string requestTypeValue = null;

                        // Check if conditionsJson is not null or empty before proceeding
                        if (!string.IsNullOrEmpty(conditionsJson))
                        {
                            Console.WriteLine($"Conditions: {workflowResult.conditions}");
                            Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");

                            // Deserialize conditions JSON
                            var conditions = JsonConvert.DeserializeObject<List<Condition>>(conditionsJson);

                            // Instantiate the evaluator
                            ConditionEvaluator conditionEvaluator = new ConditionEvaluator();

                            // Evaluate the conditions and extract values
                            var (areConditionsMet, extractedValues) = conditionEvaluator.Evaluate(conditionsJson, new
                            {
                                RequestStatus = "final",  // Assuming ReqStatusId represents the current request status
                                RequestType = "Preapprove" // Assuming ReqTypeId represents the current request type
                            });

                            // Extract specific values
                            requestStatusValue = extractedValues.ContainsKey("RequestStatus") ? extractedValues["RequestStatus"] : null;
                            requestTypeValue = extractedValues.ContainsKey("RequestType") ? extractedValues["RequestType"] : null;

                            Console.WriteLine($"RequestStatusValue: {requestStatusValue}");
                            Console.WriteLine($"RequestTypeValue: {requestTypeValue}");
                        }

                        // Now you can use requestStatusValue and requestTypeValue outside the if block
                        Console.WriteLine($"Outside If - RequestStatusValue: {requestStatusValue}");
                        Console.WriteLine($"Outside If - RequestTypeValue: {requestTypeValue}");

                        // Extract the next status
                        int nextStatusId = workflowResult.nextStatusId;
                        nextStatusName = workflowResult.nextStatusName;
                        string flag = workflowResult.flag;

                        if (flag == "LicencesNo")
                        {
                            var resultLic = await _generateLicNo.GenerateUniqueLicenseNumberTourism((int)ServiceEnum.Tourism, model.ReqTypeId, model.ActivityTypeId);
                            SequenceNo = resultLic.Item1;
                            licNo = resultLic.Item2;
                        }
                        // Additional logic for specific statuses
                        if (model.files != null && model.files.Count > 0)
                        {
                            foreach (var file in model.files)
                            {
                                if (file.Files != null)
                                {
                                    // Save each file to disk
                                    var savedFilePath = await SaveFileToDiskAsync(file.Files, file.filename, _file, model.ReqNo);
                                    savedFilePath.IsRequired = file.ismandatory;
                                    savedFilePath.FieldName = file.fieldname;
                                    filePath.Add(savedFilePath);
                                    // Perform any additional logic with the saved file path, if needed
                                    Console.WriteLine($"File saved at: {savedFilePath}");

                                }
                            }
                        }
                        // Create request data
                        requestData = new UpdatedRequestVM
                        {
                            LicNo = licNo,
                            StatusId = nextStatusId,
                            RequestId = model.RequestId,
                            SequenceNo=SequenceNo,
                            //FilePath = filePath.FilePath,
                            //FileName = filePath.FileName,
                            ReqTypeId = model.ReqTypeId,
                          //  LicStatusId = licStatusId,
                            Note = model.Note,
                            saveResponseVMs = filePath,
                            Flag= flag,
                            NameUser = username,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            requestStatusValue = requestStatusValue,
                            requestTypeValue = requestTypeValue,
                            HotelClassEvaluations = model.HotelClassEvaluations,
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                            selectedAttachments = model.selectedAttachments,
                            uncheckedAttachments = model.uncheckedAttachments,
                            Values = model.Values,
                            AttachmentStates = model.AttachmentStates,
                           ClassificationId=model.ClassificationId,

                        };
                    }
                    else if (model.Action == "SendNotifyToUser")
                    {
                        var workflowResponse = await client.GetAsync($"{apisettingsDynamic}/GetCurrentStatus?serviceId={(int)ServiceEnum.Tourism}&activityTypeId={model.ActivityTypeId}&requestTypeId={model.ReqTypeId}&currentStatusId={model.ReqStatusId}");
                        if (!workflowResponse.IsSuccessStatusCode)
                        {
                            return StatusCode((int)workflowResponse.StatusCode, "Failed to fetch next status");
                        }

                        var workflowData = await workflowResponse.Content.ReadAsStringAsync();
                        var workflowResult = JsonConvert.DeserializeObject<dynamic>(workflowData);





                        if (workflowResult.success == false)
                        {
                            return BadRequest(workflowResult.message.ToString());
                        }
                        Console.WriteLine($"Conditions: {workflowResult.conditions}");
                        Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");
                        // Extract conditions JSON as a string
                        //if(workflowResult.)
                        string conditionsJson = workflowResult.conditions?.ToString() ?? string.Empty;

                        // Declare variables outside the if block
                        string requestStatusValue = null;
                        string requestTypeValue = null;

                        // Check if conditionsJson is not null or empty before proceeding
                        if (!string.IsNullOrEmpty(conditionsJson))
                        {
                            Console.WriteLine($"Conditions: {workflowResult.conditions}");
                            Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");

                            // Deserialize conditions JSON
                            var conditions = JsonConvert.DeserializeObject<List<Condition>>(conditionsJson);

                            // Instantiate the evaluator
                            ConditionEvaluator conditionEvaluator = new ConditionEvaluator();

                            // Evaluate the conditions and extract values
                            var (areConditionsMet, extractedValues) = conditionEvaluator.Evaluate(conditionsJson, new
                            {
                                RequestStatus = "final",  // Assuming ReqStatusId represents the current request status
                                RequestType = "Preapprove" // Assuming ReqTypeId represents the current request type
                            });

                            // Extract specific values
                            requestStatusValue = extractedValues.ContainsKey("RequestStatus") ? extractedValues["RequestStatus"] : null;
                            requestTypeValue = extractedValues.ContainsKey("RequestType") ? extractedValues["RequestType"] : null;

                            Console.WriteLine($"RequestStatusValue: {requestStatusValue}");
                            Console.WriteLine($"RequestTypeValue: {requestTypeValue}");
                        }

                        // Now you can use requestStatusValue and requestTypeValue outside the if block
                        Console.WriteLine($"Outside If - RequestStatusValue: {requestStatusValue}");
                        Console.WriteLine($"Outside If - RequestTypeValue: {requestTypeValue}");

                        // Extract the next status
                        int nextStatusId = workflowResult.nextStatusId;
                        nextStatusName = workflowResult.nextStatusName;
                        string flag = workflowResult.flag;

                        // Additional logic for specific statuses
                        if (model.files != null && model.files.Count > 0)
                        {
                            foreach (var file in model.files)
                            {
                                if (file.Files != null)
                                {
                                    // Save each file to disk
                                    var savedFilePath = await SaveFileToDiskAsync(file.Files, file.filename, _file, model.ReqNo);
                                    savedFilePath.IsRequired = file.ismandatory;
                                    savedFilePath.FieldName = file.fieldname;
                                    filePath.Add(savedFilePath);
                                    // Perform any additional logic with the saved file path, if needed
                                    Console.WriteLine($"File saved at: {savedFilePath}");

                                }
                            }
                        }
                        requestData = new UpdatedRequestVM
                        {
                            LicNo = licNo,
                            StatusId = model.ReqStatusId,
                            RequestId = model.RequestId,
                            //FilePath = filePath.FilePath,
                            //FileName = filePath.FileName,
                            saveResponseVMs = filePath,
                           

                            ReqTypeId = model.ReqTypeId,
                           // LicStatusId = licStatusId,
                            Note = model.Note,
                            NameUser = username,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            requestStatusValue = requestStatusValue,
                            requestTypeValue = requestTypeValue,
                            HotelClassEvaluations = model.HotelClassEvaluations,
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                            selectedAttachments = model.selectedAttachments,
                            uncheckedAttachments = model.uncheckedAttachments,
                            Values = model.Values,
                            AttachmentStates = model.AttachmentStates

                        };
                    }
                    else
                    {
                        requestData = new UpdatedRequestVM
                        {
                            LicNo = licNo,
                            StatusId = model.ReqStatusId,
                            RequestId = model.RequestId,
                            ReqTypeId = model.ReqTypeId,
                          

                            //    LicStatusId = licStatusId,
                            Note = model.Note,
                            NameUser = username,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            HotelClassEvaluations = model.HotelClassEvaluations,
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                            selectedAttachments = model.selectedAttachments,
                            uncheckedAttachments = model.uncheckedAttachments,
                            Values = model.Values,
                            AttachmentStates = model.AttachmentStates,
                            ClassificationId = model.ClassificationId,
                        };
                    }
                    // Send API request to update request status
                    var jsonContent = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var updateResponse = await client.PostAsync($"{apiSettings}/UpdateRequestStatus", content);

                    if (!updateResponse.IsSuccessStatusCode)
                    {
                        return StatusCode((int)updateResponse.StatusCode, "Failed to update request status");
                    }

                    var responseData = await updateResponse.Content.ReadAsStringAsync();
                    //var updatedRequest = JsonConvert.DeserializeObject<dynamic>(responseData);

                    // Return the updated request data
                    return Ok(new
                    {
                        UpdatedRequest = responseData,
                        Message = $"Status updated to {nextStatusName}"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while accessing the Index page.");

                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        public async Task<ActionResult> SaveDataPreApprove([FromForm] SaveDataViewModelPreApprove model)
        {
            // Extract page name from the referrer
            var referrer = HttpContext.Request.Headers["Referer"].ToString();
            string pageName = string.IsNullOrEmpty(referrer)
                ? "Unknown"
                : new Uri(referrer).AbsolutePath;
            pageName = pageName.Substring(pageName.LastIndexOf("/") + 1);
            int serviceId = (int)ServiceEnum.Tourism;
            // Retrieve user data from session
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            string logs = "";
            if (model.ChangeLogs != null)
            {
                logs = string.Join(", ", model.ChangeLogs);  // You can change the separator as needed
            }
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var username = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;
            var ServiceId = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;

            var userid = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.UserData)?.Value;
            // Initialize API settings
            var apiSettings = _baseUrl + "api/AdminTourism";
            ViewBag.ApiBaseUrl = apiSettings;
            var apisettingsDynamic = _baseUrl + "Dynamic";
            // Initialize default values
            string licNo = "";
            string nextStatusName = "";
            long SequenceNo = 0;
           List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();
            int licStatusId =(int) licencesStatusEnum.Pending;

            try
            {
                // Create a new HttpClient instance for each request
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                   

                   
                    UpdatedRequestVM requestData;
                    // Fetch the next status dynamically from the workflow
                    if (model.Action == "RequestStatusbutton")
                    {
                        var workflowResponse = await client.GetAsync($"{apisettingsDynamic}/GetNextStatus?serviceId={(int)ServiceEnum.Tourism}&activityTypeId={model.ActivityTypeId}&requestTypeId={model.ReqTypeId}&currentStatusId={model.ReqStatusId}");
                        if (!workflowResponse.IsSuccessStatusCode)
                        {
                            return StatusCode((int)workflowResponse.StatusCode, "Failed to fetch next status");
                        }

                        var workflowData = await workflowResponse.Content.ReadAsStringAsync();
                        var workflowResult = JsonConvert.DeserializeObject<dynamic>(workflowData);





                        if (workflowResult.success == false)
                        {
                            return BadRequest(workflowResult.message.ToString());
                        }
                        Console.WriteLine($"Conditions: {workflowResult.conditions}");
                        Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");
                        // Extract conditions JSON as a string
                        //if(workflowResult.)
                        string conditionsJson = workflowResult.conditions?.ToString() ?? string.Empty;

                        // Declare variables outside the if block
                        string requestStatusValue = null;
                        string requestTypeValue = null;

                        // Check if conditionsJson is not null or empty before proceeding
                        if (!string.IsNullOrEmpty(conditionsJson))
                        {
                            Console.WriteLine($"Conditions: {workflowResult.conditions}");
                            Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");

                            // Deserialize conditions JSON
                            var conditions = JsonConvert.DeserializeObject<List<Condition>>(conditionsJson);

                            // Instantiate the evaluator
                            ConditionEvaluator conditionEvaluator = new ConditionEvaluator();

                            // Evaluate the conditions and extract values
                            var (areConditionsMet, extractedValues) = conditionEvaluator.Evaluate(conditionsJson, new
                            {
                                RequestStatus = "final",  // Assuming ReqStatusId represents the current request status
                                RequestType = "Preapprove" // Assuming ReqTypeId represents the current request type
                            });

                            // Extract specific values
                            requestStatusValue = extractedValues.ContainsKey("RequestStatus") ? extractedValues["RequestStatus"] : null;
                            requestTypeValue = extractedValues.ContainsKey("RequestType") ? extractedValues["RequestType"] : null;

                            Console.WriteLine($"RequestStatusValue: {requestStatusValue}");
                            Console.WriteLine($"RequestTypeValue: {requestTypeValue}");
                        }

                        // Now you can use requestStatusValue and requestTypeValue outside the if block
                        Console.WriteLine($"Outside If - RequestStatusValue: {requestStatusValue}");
                        Console.WriteLine($"Outside If - RequestTypeValue: {requestTypeValue}");

                        // Extract the next status
                        int nextStatusId = workflowResult.nextStatusId;
                        nextStatusName = workflowResult.nextStatusName;
                        string flag = workflowResult.flag;

                        if (flag == "LicencesNo")
                        {
                            var resutLic = await _generateLicNo.GenerateUniqueLicenseNumberTourismPreApproval(model.ReqTypeId,(int)ServiceEnum.Tourism);
                            SequenceNo = resutLic.Item1;
                                licNo = resutLic.Item2;
                        }
                        //// Additional logic for specific statuses
                        //if (requestStatusValue == "final" && model.Files != null)
                        //{
                        //    filePath = await SaveFileToDiskAsync(model.Files, model.filname, _file, model.ReqNo);
                        //    licStatusId = (int)licencesStatusEnum.Released;

                        //}
                        if ( model.files != null && model.files.Count > 0)
                        {
                            foreach (var file in model.files)
                            {
                                if (file.Files != null)
                                {
                                    // Save each file to disk
                                    var savedFilePath = await SaveFileToDiskAsync(file.Files, file.filename, _file, model.ReqNo);
                                    savedFilePath.IsRequired = file.ismandatory;
                                    savedFilePath.FieldName = file.fieldname;
                                    filePath.Add(savedFilePath);
                                    
                                    // Perform any additional logic with the saved file path, if needed
                                    Console.WriteLine($"File saved at: {savedFilePath}");

                                }
                            }

                            // Update the status to Released
                            licStatusId = (int)licencesStatusEnum.Released;
                        }
                        // Create request data
                        requestData = new UpdatedRequestVM
                        {
                            PreApprovalNo = licNo,
                            StatusId = nextStatusId,
                            RequestId = model.RequestId,
                            SequenceNo=SequenceNo,
                            //FilePath = filePath.FilePath,
                            //FileName = filePath.FileName,
                            saveResponseVMs=filePath,
                            ReqTypeId = model.ReqTypeId,
                            LicStatusId = licStatusId,
                            Note = model.Note,
                            NameUser = username,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            requestStatusValue = requestStatusValue,
                            requestTypeValue = requestTypeValue,
                            Flag=flag,
                            
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                           selectedAttachments=model.SelectedAttachments,
                           uncheckedAttachments=model.UncheckedAttachments,
                           AttachmentStates=model.allAttachmentsState


                        };
                    }
                    else if (model.Action == "SendNotifyToUser")
                    {
                        var workflowResponse = await client.GetAsync($"{apisettingsDynamic}/GetCurrentStatus?serviceId={serviceId}&activityTypeId={model.ActivityTypeId}&requestTypeId={model.ReqTypeId}&currentStatusId={model.ReqStatusId}");
                        if (!workflowResponse.IsSuccessStatusCode)
                        {
                            return StatusCode((int)workflowResponse.StatusCode, "Failed to fetch next status");
                        }

                        var workflowData = await workflowResponse.Content.ReadAsStringAsync();
                        var workflowResult = JsonConvert.DeserializeObject<dynamic>(workflowData);





                        if (workflowResult.success == false)
                        {
                            return BadRequest(workflowResult.message.ToString());
                        }
                        Console.WriteLine($"Conditions: {workflowResult.conditions}");
                        Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");
                        // Extract conditions JSON as a string
                        //if(workflowResult.)
                        string conditionsJson = workflowResult.conditions?.ToString() ?? string.Empty;

                        // Declare variables outside the if block
                        string requestStatusValue = null;
                        string requestTypeValue = null;

                        // Check if conditionsJson is not null or empty before proceeding
                        if (!string.IsNullOrEmpty(conditionsJson))
                        {
                            Console.WriteLine($"Conditions: {workflowResult.conditions}");
                            Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");

                            // Deserialize conditions JSON
                            var conditions = JsonConvert.DeserializeObject<List<Condition>>(conditionsJson);

                            // Instantiate the evaluator
                            ConditionEvaluator conditionEvaluator = new ConditionEvaluator();

                            // Evaluate the conditions and extract values
                            var (areConditionsMet, extractedValues) = conditionEvaluator.Evaluate(conditionsJson, new
                            {
                                RequestStatus = "final",  // Assuming ReqStatusId represents the current request status
                                RequestType = "Preapprove" // Assuming ReqTypeId represents the current request type
                            });

                            // Extract specific values
                            requestStatusValue = extractedValues.ContainsKey("RequestStatus") ? extractedValues["RequestStatus"] : null;
                            requestTypeValue = extractedValues.ContainsKey("RequestType") ? extractedValues["RequestType"] : null;

                            Console.WriteLine($"RequestStatusValue: {requestStatusValue}");
                            Console.WriteLine($"RequestTypeValue: {requestTypeValue}");
                        }

                        // Now you can use requestStatusValue and requestTypeValue outside the if block
                        Console.WriteLine($"Outside If - RequestStatusValue: {requestStatusValue}");
                        Console.WriteLine($"Outside If - RequestTypeValue: {requestTypeValue}");

                        // Extract the next status
                        int nextStatusId = workflowResult.nextStatusId;
                        nextStatusName = workflowResult.nextStatusName;
                        string flag = workflowResult.flag;
                        //if (flag == "LicencesNo")
                        //{
                        //    var resutLic = await _generateLicNo.GenerateUniqueLicenseNumberTourism((int)ServiceEnum.Tourism, model.ReqTypeId, model.ActivityTypeId);
                        //    SequenceNo = resutLic.Item1;
                        //    licNo = resutLic.Item2;
                        //}
                        // Additional logic for specific statuses
                        //if (requestStatusValue == "final" && model.Files != null)
                        //{
                        //    filePath = await SaveFileToDiskAsync(model.Files, model.filname, _file, model.ReqNo);
                        //    licStatusId = 5;

                        //}
                        if ( model.files != null && model.files.Count > 0)
                        {
                            foreach (var file in model.files)
                            {
                                if (file.Files != null)
                                {
                                    // Save each file to disk
                                    var savedFilePath = await SaveFileToDiskAsync(file.Files, file.filename, _file, model.ReqNo);
                                    savedFilePath.IsRequired = file.ismandatory;
                                    savedFilePath.FieldName = file.fieldname;
                                    filePath.Add(savedFilePath);
                                    // Perform any additional logic with the saved file path, if needed
                                    Console.WriteLine($"File saved at: {savedFilePath}");
                                }
                            }

                            // Update the status to Released
                            licStatusId = (int)licencesStatusEnum.Released;
                        }
                        requestData = new UpdatedRequestVM
                        {
                            LicNo = licNo,
                            StatusId = model.ReqStatusId,
                            RequestId = model.RequestId,

                            //FilePath = filePath.FilePath,
                            //FileName = filePath.FileName,
                            saveResponseVMs=filePath,
                            ReqTypeId = model.ReqTypeId,
                            LicStatusId = licStatusId,
                            Note = model.Note,
                            NameUser = username,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            requestStatusValue = requestStatusValue,
                            requestTypeValue = requestTypeValue,
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                            selectedAttachments=model.SelectedAttachments,
                            uncheckedAttachments=model.UncheckedAttachments,
                            AttachmentStates=model.allAttachmentsState,
                            
                            

                        };
                    }
                    else
                    {
                        requestData = new UpdatedRequestVM
                        {
                            LicNo = licNo,
                            StatusId = model.ReqStatusId,
                            RequestId = model.RequestId,
                            ReqTypeId = model.ReqTypeId,
                            LicStatusId =(int) licencesStatusEnum.Refused,
                            Note = model.Note,
                            NameUser = username,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            ChangeLogs = model.ChangeLogs,
                            //saveResponseVMs = filePath,
                            Action = model.Action,
                            selectedAttachments = model.SelectedAttachments,
                            uncheckedAttachments = model.UncheckedAttachments,
                            AttachmentStates = model.allAttachmentsState,

                        };
                    }
                    // Send API request to update request status
                    var jsonContent = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var updateResponse = await client.PostAsync($"{apiSettings}/UpdateRequestStatus", content);

                    if (!updateResponse.IsSuccessStatusCode)
                    {
                        return StatusCode((int)updateResponse.StatusCode, "Failed to update request status");
                    }

                    var responseData = await updateResponse.Content.ReadAsStringAsync();
                    //var updatedRequest = JsonConvert.DeserializeObject<dynamic>(responseData);

                    // Return the updated request data
                    return Ok(new
                    {
                        UpdatedRequest = responseData,
                        Message = $"Status updated to {nextStatusName}"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while accessing the Index page.");

                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        private async Task<HttpResponseMessage> SendUpdateRequest(UpdatedRequestVM requestData)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                var jsonContent = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                return await client.PostAsync($"{_baseUrl}/api/AdminTourism/UpdateRequestStatus", content);
            }
        }

        private string GetPageNameFromReferrer()
        {
            var referrer = HttpContext.Request.Headers["Referer"].ToString();
            if (string.IsNullOrEmpty(referrer)) return "Unknown";
            string pageName = new Uri(referrer).AbsolutePath;
            return pageName.Substring(pageName.LastIndexOf("/") + 1);
        }
        private async Task<HttpResponseMessage> SendUpdateRequest(UpdatedRequestVM requestData, string endpoint)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                var jsonContent = JsonConvert.SerializeObject(requestData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                return await client.PostAsync($"{_baseUrl}/api/AdminTourism/{endpoint}", content);
            }
        }
        private async Task<dynamic> FetchWorkflowData(HttpClient client, string endpoint, int serviceId, int activityTypeId, int requestTypeId, int currentStatusId)
        {
            var url = $"{_baseUrl}/Dynamic/{endpoint}?serviceId={serviceId}&activityTypeId={activityTypeId}&requestTypeId={requestTypeId}&currentStatusId={currentStatusId}";
            var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<dynamic>(responseData);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateMoic([FromBody]UpdateMoicViewModel model)
        {
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            int serviceId = 6;
            var apiSettings = _baseUrl + $"api/AdminTourism/UpdateMoicForm";
            var postupdate = await _helperUrlApi.PostDataToApi<UpdateMoicViewModel, UpdateMoicViewModel>(apiSettings, model);

            return View(model);
        }
        #endregion

        #region Address PaciData

        private PaciAddressData GetPaciAddressData(string Token, string PaciNo)
        {
            var client = new RestClient(GetPACIAddressURL);

            var request = new RestRequest();
            request.Method = RestSharp.Method.Post;
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", "Bearer " + Token);
            request.AddParameter("application/x-www-form-urlencoded", "pacino=" + PaciNo, ParameterType.RequestBody);
            var response = client.Execute(request);

            PaciAddressData model = new PaciAddressData();

            model = JsonConvert.DeserializeObject<PaciAddressData>(response.Content);

            return model;
        }

        public JObject GetPaciAddressLocation(string Token, string longitude, string latitude)
        {

            //var config = new ClientConfig()
            //{
            //    grant_type = "password",
            //    username = username,
            //    password = password
            //};

            var client = new RestClient("https://apitest.media.gov.kw/OtherMinistries/api/paci/gis/InspectorLocation");


            var request = new RestRequest();
            request.Method = RestSharp.Method.Post;
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", "Bearer " + Token);
            request.AddParameter("application/x-www-form-urlencoded", "longitude=" + longitude + "&latitude=" + latitude, ParameterType.RequestBody);
            var response = client.Execute(request);


            JObject jObject = JsonConvert.DeserializeObject<dynamic>(response.Content);


            return jObject;

        }
        public JObject GetToken(string username, string password)
        {

            var config = new ClientConfig()
            {
                grant_type = "password",
                username = username,
                password = password
            };


            var client = new RestClient(GetTokenURL);



            var request = new RestRequest();
            request.Method = RestSharp.Method.Post;
            request.AddHeader("content-type", "application/x-www-form-urlencoded");

            request.AddParameter("application/x-www-form-urlencoded", "grant_type=" + config.grant_type + "&Username=" + config.username + "&Password=" + config.password, ParameterType.RequestBody);
            var response = client.Execute(request);

            string ysysy = response.ResponseStatus.ToString();
            JObject jObject = JsonConvert.DeserializeObject<dynamic>(response.Content);


            return jObject;

        }
        //getPaciAddressApi
        public JsonResult getAddressDataFromPaci(string ID)
        {
            string token = "";
            JObject jObject = GetToken(GetPACIUserTourism, GetPACIPasswordTourism);

            token = jObject.Value<string>("access_token").ToString();

            PaciAddressData AddressData = GetPaciAddressData(token, ID);

            returnAddressValues obj = null;
            if (AddressData != null)
            {

                obj = new returnAddressValues()
                {
                    governoratearabic = AddressData.governoratearabic,
                    blockarabic = AddressData.blockarabic,
                    buildingnamearabic = AddressData.buildingnamearabic,
                    floor_no = AddressData.floor_no,
                    neighborhoodarabic = AddressData.neighborhoodarabic,
                    parcelarabic = AddressData.parcelarabic,
                    streetarabic = AddressData.streetarabic,
                    latitude = AddressData.lat,
                    longitude = AddressData.lon,
                    buildingtypearabic = AddressData.buildingtypearabic,
                    unit_no = AddressData.unit_no
                };

            }
            JObject paciLocation = null;
            try
            {
                if (obj != null)
                {
                    paciLocation = GetPaciAddressLocation(token, obj.longitude, obj.latitude);

                    //return Json(new { success = true, responseText = "" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //return Json(new { error = true, responseText = "" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                //return Json(new { error = true, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }




            return Json(obj);
        }
        #endregion
    }
}
