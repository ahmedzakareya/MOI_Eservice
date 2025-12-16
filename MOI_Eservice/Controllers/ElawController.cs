using Business.ViewModel.Account;
using Business.ViewModel.AddressPaciModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using Business.ViewModel.Tourism;
using System.Net;
using Business.ViewModel.Dynamic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Business.Helpers;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Business.ViewModel;
using System.Net.Http.Headers;
using System.Security.Policy;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Business.Enums;
using NuGet.Protocol;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Business.ViewModel.Elaw;
using Microsoft.AspNetCore.Http;
using NuGet.Common;
using Domain.Entities;





namespace MOI_Eservice.Controllers
{
    //[Authorize(AuthenticationSchemes = "UserScheme")]

    public class ElawController : Controller
    {
        private readonly string GetPACIAddressURL;
        private readonly string PaciAPIUserName;
        private readonly string PaciAPIPassword;
        public static string token;
        public static string statusCode;
        private readonly string GetTokenURL;
        public static string _PaciapiUrl;
        public static string _PaciInfoApiUrl;
        public static string _PaciPassword;
        public static string _PaciUsername;
        private readonly string _file;
        private readonly IConfiguration _configuration;
        private readonly GeneralReqNo _generalReqNo;
        private readonly HelperUrlApi _helperUrlApi;
        private readonly IWebHostEnvironment _env;
        private readonly string _baseUrl;
        public ElawController(IConfiguration configuration, GeneralReqNo generalReqNo, HelperUrlApi helperUrlApi, IWebHostEnvironment env)
        {
            GetPACIAddressURL = configuration["PaciAddressData:GetPACIAddressURL"];
            PaciAPIUserName = configuration["PaciAddressData:GetPACIUserTourism"];
            PaciAPIPassword = configuration["PaciAddressData:GetPACIPasswordTourism"];
            GetTokenURL = configuration["PaciAddressData:GetTokenURL"];
            _file = configuration["Path:Elaw"];
            _PaciapiUrl = configuration["PaciData:PaciAPI"];
            _PaciInfoApiUrl = configuration["PaciData:PaciInfoAPI"];
            _PaciUsername = configuration["PaciData:PaciAPIUserName"];
            _PaciPassword = configuration["PaciData:PaciAPIPassword"];
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _configuration = configuration;
            _generalReqNo = generalReqNo;
            _helperUrlApi = helperUrlApi;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region ApplyRequest
        private async Task<List<SelectListItem>> _Activities()
        {
            var apiSetting = _baseUrl + "api/ElawFront/GetActivities";

            // Create a list to hold SelectListItems
            List<SelectListItem> activitiesList = new List<SelectListItem>();

            try
            {
                // Use _helperUrlApi to fetch the data asynchronously
                var response = await _helperUrlApi.GetDataFromApi<List<ActivityTypeVM>>(apiSetting);

                if (response != null && response.Any())
                {
                    // Add default item
                    activitiesList.Add(new SelectListItem
                    {
                        Text = "-- إختر النشاط --",
                        Value = "0",
                        Selected = true
                    });

                    // Add other activities
                    foreach (var item in response)
                    {
                        activitiesList.Add(new SelectListItem
                        {
                            Text = item.NameAr, // The display name of the activity
                            Value = item.ActivityCode,


                        });
                    }
                }
                else
                {
                    // Handle case where no data was returned
                    activitiesList.Add(new SelectListItem
                    {
                        Text = "No activities available",
                        Value = "0",
                        Selected = true
                    });
                }
            }
            catch (Exception ex)
            {
                // Log the error if something goes wrong
                LogManager.Instance.AddErrorLog(ex);
                activitiesList.Add(new SelectListItem
                {
                    Text = "Error loading activities",
                    Value = "0",
                    Selected = true
                });
            }

            return activitiesList;
        }
        private async Task<List<SelectListItem>> _Activities(int id)
        {
            var apiSetting = _baseUrl + $"api/ElawFront/GetActivities/{id}";

            // Create a list to hold SelectListItems
            List<SelectListItem> activitiesList = new List<SelectListItem>();

            try
            {
                // Use _helperUrlApi to fetch the data asynchronously
                var response = await _helperUrlApi.GetDataFromApi<List<ActivityTypeVM>>(apiSetting);

                if (response != null && response.Any())
                {
                    // Add default item
                    activitiesList.Add(new SelectListItem
                    {
                        Text = "-- إختر النشاط --",
                        Value = "0",
                        Selected = true
                    });

                    // Add other activities
                    foreach (var item in response)
                    {
                        activitiesList.Add(new SelectListItem
                        {
                            Text = item.NameAr, // The display name of the activity
                            Value = item.ActivityCode,


                        });
                    }
                }
                else
                {
                    // Handle case where no data was returned
                    activitiesList.Add(new SelectListItem
                    {
                        Text = "No activities available",
                        Value = "0",
                        Selected = true
                    });
                }
            }
            catch (Exception ex)
            {
                // Log the error if something goes wrong
                LogManager.Instance.AddErrorLog(ex);
                activitiesList.Add(new SelectListItem
                {
                    Text = "Error loading activities",
                    Value = "0",
                    Selected = true
                });
            }

            return activitiesList;
        }
        public async Task<JsonResult> GetActivity(int id)
        {
            List<SelectListItem> Activities2 = new List<SelectListItem>();

            Activities2 = await _Activities(id);
            //return Json(Activities2);
            return Json(Activities2);
        }

        #region  إصدار مؤسسة إعلامية-افراد
        [HttpGet]
        public async Task<ActionResult> LicRequestForPerson(int id)
       {

            try
            {
                
                var token = HttpContext.Session.GetString("UserToken");
                var userId = HttpContext.Session.GetString("UserId");
                var userName = HttpContext.Session.GetString("UserUserName");
                var civilId = HttpContext.Session.GetString("UserCivilId");
                var fullName = HttpContext.Session.GetString("UserFullName");
                var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
                if (token != null) { 

                var apiSetting = _baseUrl + $"api/ElawFront/GetLicRequestForPerson?CivilId={civilId}&&id={id}";
                var RequestLicForPerson = await _helperUrlApi.GetDataFromApi<RequestLicPerPerson>(apiSetting);

                return View(RequestLicForPerson);
            }
                else
            {
                return RedirectToAction("Index", "Home");
            }


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


        public async Task<JsonResult> CheckMediaName(string medianame)
        {
            var apiUrl = $"{_baseUrl}api/ElawFront/CheckMediaName?MediaName={Uri.EscapeDataString(medianame)}";

            var result = await _helperUrlApi.GetDataFromApi<MediaCheckResult>(apiUrl);
            return Json(result);
        }
        public async Task<JsonResult> CheckManager(string ManagerCivilId)
        {
            var apiUrl = $"{_baseUrl}api/ElawFront/CheckManager?ManagerCivilId={Uri.EscapeDataString(ManagerCivilId)}";

            var result = await _helperUrlApi.GetDataFromApi<bool>(apiUrl);
            return Json(result);
        }
        [HttpPost]
        public async Task<ActionResult> LicRequestForPerson([FromForm] RequestLicPerPerson model)
        {
            var apiSetting = _baseUrl + $"api/ElawFront/PostLicRequestForPerson";
            List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();
            int ReqTypeId = model.LicencesInfoVM.ReqTypeId??0;
            int LicType = model.LicencesInfoVM.LicTypeId??0;
            Tuple<long, string> RequestData = await _generalReqNo.GetRequestNoForElaw(ReqTypeId,LicType);

            // Unpack the values
            long sequenceNo = RequestData.Item1;
            string reqNo = RequestData.Item2;
            var token = HttpContext.Session.GetString("UserToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserUserName");
            var civilId = HttpContext.Session.GetString("UserCivilId");
            var fullName = HttpContext.Session.GetString("UserFullName");
            var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");

           

            try
            {

                foreach (var file in model.NamedFile)
                {
                    if (file.File != null)
                    {
                        // Save each file to disk
                        var savedFilePath = await SaveFileToDiskAsync(file.File, file.FieldName, _file, reqNo, file.IsRequired, file.FieldName, file.LabelName);

                        filePath.Add(savedFilePath);
                        // Perform any additional logic with the saved file path, if needed
                        Console.WriteLine($"File saved at: {savedFilePath}");

                    }
                }
                var ModelToApi = new RequestLicPerPersonApi
                {
                    SessionCivilId=civilId,
                    SessionName=fullName,
                    FacebookUrl=model.FacebookUrl,
                    Instagram=model.Instagram,
                    Twitter=model.Twitter,
                    website=model.website,
                    Licname = model.LicName,
                    CivilId=model.RequestVM.ApplicantPerson.CivilId,
                    Name1Applicant=model.RequestVM.ApplicantPerson.Name1,
                    Name2Applicant = model.RequestVM.ApplicantPerson.Name2,
                    Name3Applicant = model.RequestVM.ApplicantPerson.Name3,
                    Name4Applicant = model.RequestVM.ApplicantPerson.Name4,
                    Email = model.RequestVM.ApplicantPerson.Email,
                    Mobile = model.RequestVM.ApplicantPerson.Phone,
                 ReqtypeId=model.LicencesInfoVM.ReqTypeId,
                 RequestStatusId=(int)RequestStatusEnum.WaitingForReview,
                    ActivityTypeId = model.LicencesInfoVM.ActvityTypeId,
                 QualificationApplicantId=model.QualificationApplicantId,
                 QualificationManagerId=model.QualificationManagerId,
                 AaliNOApplicant=model.RequestVM.ApplicantPerson.AddressNavigation.AalliNo,
                 AreaApplicant= model.RequestVM.ApplicantPerson.AddressNavigation.Area,
                 BlockApplicant= model.RequestVM.ApplicantPerson.AddressNavigation.BlockArabic,
                 BuildingNameApplicant= model.RequestVM.ApplicantPerson.AddressNavigation.BuildingName,
                 BuildingNOApplicant= model.RequestVM.ApplicantPerson.AddressNavigation.BuildingNo,
                 FloorNOApplicant= model.RequestVM.ApplicantPerson.AddressNavigation.FloorNo,
                 GovernateApplicant= model.RequestVM.ApplicantPerson.AddressNavigation.GovernorateArabic,
                 StreetApplicant= model.RequestVM.ApplicantPerson.AddressNavigation.StreetArabic,
                 UnitNOApplicant= model.RequestVM.ApplicantPerson.AddressNavigation.UnitNo,
                 saveResponseVMs=filePath,
                  LicTypeId=model.LicencesInfoVM.LicTypeId,
                    AaliNOManager = model.RequestVM.Manager.AddressNavigation.AalliNo,
                    AreaManager = model.RequestVM.Manager.AddressNavigation.Area,
                    BlockManager = model.RequestVM.Manager.AddressNavigation.BlockArabic,
                    BuildingNameManager = model.RequestVM.Manager.AddressNavigation.BuildingName,
                    BuildingNoManager = model.RequestVM.Manager.AddressNavigation.BuildingNo,
                    FloorNOManager = model.RequestVM.ApplicantPerson.AddressNavigation.FloorNo,
                    GovernateManager = model.RequestVM.ApplicantPerson.AddressNavigation.GovernorateArabic,
                    StreetManager = model.RequestVM.ApplicantPerson.AddressNavigation.StreetArabic,
                    UnitNOManager = model.RequestVM.ApplicantPerson.AddressNavigation.UnitNo,
                    Name1Manager=model.RequestVM.Manager.Name1,
                    Name2Manager = model.RequestVM.Manager.Name2,
                    Name3Manager = model.RequestVM.Manager.Name3,
                    Name4Manager = model.RequestVM.Manager.Name4,
                    
                    ManCivilId = model.RequestVM.Manager.CivilId,
                    EmailManager=model.RequestVM.Manager.Email,
                    AppCivilId=model.RequestVM.ApplicantPerson.CivilId,
                    Licreqtime=DateTime.Now,
                    LicStatusId=(int)licencesStatusEnum.Pending,
                    SequenceNo=sequenceNo,
                    Reqno=reqNo,
                     OwnerSameManager=model.RequestVM.OwnerSameManager,
                     NationalitynameManager= model.RequestVM.Manager.NationaliyName,
                     NationalitynameApplicant= model.RequestVM.ApplicantPerson.NationaliyName,
                     PhoneManager= model.RequestVM.Manager.Phone

                };

                var response = await _helperUrlApi.PostDataToApi<RequestLicPerPersonApi, RequestLicPerPersonApi>(apiSetting, ModelToApi);

                TempData["AlertTitle"] = "تأكـيد";
                TempData["Message"] = "تم حفظ البيانات بنجاح";
                TempData["AlertType"] = "Success";

                return RedirectToAction("Index", "Home");

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
            return View();
        }
        #endregion

        #region  إصدار مؤسسة إعلامية-شركات
        [HttpGet]
        public async Task<ActionResult> LicRequestForCompany(int id)
        {

            try
            {
                
                var token = HttpContext.Session.GetString("UserToken");
                var userId = HttpContext.Session.GetString("UserId");
                var userName = HttpContext.Session.GetString("UserUserName");
                var civilId = HttpContext.Session.GetString("UserCivilId");
                var fullName = HttpContext.Session.GetString("UserFullName");
                var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
                if (token != null)
                { 
                var apiSetting = _baseUrl + $"api/ElawFront/GetLicRequestForCompany?CivilId={civilId}&id={id}";
                var RequestLic = await _helperUrlApi.GetDataFromApi<RequestLicPerPerson>(apiSetting);
                //RequestLic.RequestVM.LicTypeId= LicTypeId;
                return View(RequestLic);
            }
                else
            {
                return RedirectToAction("Index", "Home");
            }


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


        
        [HttpPost]
        public async Task<ActionResult> LicRequestForCompany([FromForm] RequestLicPerPerson model)
        {
            var apiSetting = _baseUrl + $"api/ElawFront/PostLicRequestForCompany";
            List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();
            int ReqTypeId = model.LicencesInfoVM.ReqTypeId ?? 0;
            int LicType = model.LicencesInfoVM.LicTypeId??0;
            Tuple<long, string> RequestData = await _generalReqNo.GetRequestNoForElaw(ReqTypeId, LicType);

            // Unpack the values
            long sequenceNo = RequestData.Item1;
            string reqNo = RequestData.Item2;
            var token = HttpContext.Session.GetString("UserToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserUserName");
            var civilId = HttpContext.Session.GetString("UserCivilId");
            var fullName = HttpContext.Session.GetString("UserFullName");
            var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
            


            try
            {

                foreach (var file in model.NamedFile)
                {
                    if (file.File != null)
                    {
                        // Save each file to disk
                        var savedFilePath = await SaveFileToDiskAsync(file.File, file.FieldName, _file, reqNo, file.IsRequired, file.FieldName, file.LabelName);

                        filePath.Add(savedFilePath);
                        // Perform any additional logic with the saved file path, if needed
                        Console.WriteLine($"File saved at: {savedFilePath}");

                    }
                }
                var ModelToApi = new RequestLicPerCompanyApi
                {
                    FacebookUrl = model.FacebookUrl,
                    Instagram = model.Instagram,
                    Twitter = model.Twitter,
                    website = model.website,
                    Licname = model.LicName,
                  SessionCivilId=civilId,
                  SessionName=fullName,
                    PartnerName1 = model.Partner1,
                    PartnerName2 = model.Partner2,
                    PartnerName3 = model.Partner3,
                    PartnerName4 = model.Partner4,
                    PartnerName5 = model.Partner5,
                    CompanyCivilId=model.RequestVM.company.CompanyCivilId,
                    CompanyEmail = model.RequestVM.company.Email,
                    CompanyFax = model.RequestVM.company.CompanyNo,
                    CompanyPhone = model.RequestVM.company.PhoneNo,
                    CompanyName = model.RequestVM.company.Name,
                    


                    ReqtypeId = model.LicencesInfoVM.ReqTypeId,
                    RequestStatusId = (int)RequestStatusEnum.WaitingForReview,
                    ActivityTypeId = model.LicencesInfoVM.ActvityTypeId,
                    //QualificationApplicantId = model.QualificationApplicantId,
                    QualificationManagerId = model.QualificationManagerId,
                    AaliNOCompany = model.RequestVM.company.AddressNavigation.AalliNo,
                    AreaCompany = model.RequestVM.company.AddressNavigation.Area,
                    BlockCompany = model.RequestVM.company.AddressNavigation.BlockArabic,
                    BuildingNameCompany = model.RequestVM.company.AddressNavigation.BuildingName,
                    BuildingNOCompany = model.RequestVM.company.AddressNavigation.BuildingNo,
                    FloorNOCompany = model.RequestVM.company.AddressNavigation.FloorNo,
                    GovernateCompany = model.RequestVM.company.AddressNavigation.GovernorateArabic,
                    StreetCompany = model.RequestVM.company.AddressNavigation.StreetArabic,
                    UnitNOCompany = model.RequestVM.company.AddressNavigation.UnitNo,
                    saveResponseVMs = filePath,
                    LicTypeId = model.LicencesInfoVM.LicTypeId,
                    AaliNOManager = model.RequestVM.Manager.AddressNavigation.AalliNo,
                    AreaManager = model.RequestVM.Manager.AddressNavigation.Area,
                    BlockManager = model.RequestVM.Manager.AddressNavigation.BlockArabic,
                    BuildingNameManager = model.RequestVM.Manager.AddressNavigation.BuildingName,
                    BuildingNoManager = model.RequestVM.Manager.AddressNavigation.BuildingNo,
                    FloorNOManager = model.RequestVM.Manager.AddressNavigation.FloorNo,
                    GovernateManager = model.RequestVM.Manager.AddressNavigation.GovernorateArabic,
                    StreetManager = model.RequestVM.Manager.AddressNavigation.StreetArabic,
                    UnitNOManager = model.RequestVM.Manager.AddressNavigation.UnitNo,
                    Name1Manager = model.RequestVM.Manager.Name1,
                    Name2Manager = model.RequestVM.Manager.Name2,
                    Name3Manager = model.RequestVM.Manager.Name3,
                    Name4Manager = model.RequestVM.Manager.Name4,

                    ManCivilId = model.RequestVM.Manager.CivilId,
                    EmailManager = model.RequestVM.Manager.Email,
                    //AppCivilId = model.RequestVM.ApplicantPerson.CivilId,
                    Licreqtime = DateTime.Now,
                    LicStatusId = (int)licencesStatusEnum.Pending,
                    SequenceNo = sequenceNo,
                    Reqno = reqNo,
                    OwnerSameManager = model.OwnerSameManager,
                    NationalitynameManager = model.RequestVM.Manager.NationaliyName,
                   
                    PhoneManager = model.RequestVM.Manager.Phone

                };

                var response = await _helperUrlApi.PostDataToApi<RequestLicPerCompanyApi, RequestLicPerCompanyApi>(apiSetting, ModelToApi);

                TempData["AlertTitle"] = "تأكـيد";
                TempData["Message"] = "تم حفظ البيانات بنجاح";
                TempData["AlertType"] = "Success";

                return RedirectToAction("Index", "Home");

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
            return View();
        }
        #endregion
       
        #region Request Detail
        public async Task<ActionResult> RequestDetails(string id)
        {
            try
            {
                int ReqID = 0;
                if (int.TryParse(MyCrypto.Decode(id), out ReqID))
                {


                    var apiSetting = _baseUrl + $"api/ElawFront/GetRequestDetails/{ReqID}";

                    ViewBag.PathAttachment = _file;

                    var response = await _helperUrlApi.GetDataFromApi<RequestFrontVM>(apiSetting);

                    return View(response);
                }
                else
                {
                    return RedirectToAction("RequestsList");
                }

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

        #endregion

        #region Address PaciData

        private PaciAddressData GetPaciAddressData(string Token, string PaciNo)
        {
            var PaciAddress = GetPACIAddressURL + "paci/gis/Addressinfobypacino";

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
            //var token = GetTokenURL + "GetToken";

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
            JObject jObject = GetToken(PaciAPIUserName, PaciAPIPassword);

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
        public async Task<FileSaveResponseVM> SaveFileToDiskAsync(IFormFile file, string fileNameFromFile, string relativePath, string? reqNo, bool? IsRequired, string? FieldName, string LabelName)
        {
            string filepath = Path.Combine(_env.WebRootPath, relativePath);
            string uploadsFolder;
            if (!string.IsNullOrEmpty(reqNo))
            {
                uploadsFolder = Path.Combine(_env.WebRootPath, relativePath, reqNo);
            }
            else
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
                fileName = $"{fileNameFromFile}.pdf";
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
                    FileName = fileNameFromFile,
                    IsRequired = IsRequired,
                    Flag = FieldName,
                    LabelName = LabelName
                };
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine("Error: " + ex.Message);
                throw; // Rethrow exception or handle accordingly
            }


        }

        #region ViewRequestsWithDetails
      
        public async Task<ActionResult> UpdateAttachment(NamedFile file, int attachId, string reqNo, long RequestId)
        {
            if (file.File == null)
                return BadRequest("No file uploaded.");

            // Save new file to disk
            var savedFile = await SaveFileToDiskAsync(
                file.File,
                file.FieldName,
                _file, // this is your path setting
                reqNo,
                file.IsRequired,
                file.FieldName,
                file.LabelName);
            var SendData = new UpdatedAttachVM
            {
                FileSaveResponseVM = savedFile,
                RequestId = RequestId,
                AttachId = attachId
            };
            // Send new attachment info to API for DB update
            var apiUrl = _baseUrl + "api/ElawFront/Request/InsertUpdateAttachement";
            var result = await _helperUrlApi.PostDataToApi<UpdatedAttachVM, ErrorMessage>(apiUrl, SendData);
            if (result != null && !result.Error)
            {
                return Json(new { success = true, message = result.Message });
            }

            return Json(new { success = false, message = result?.Message ?? "Unknown error." });

        }

        [HttpPost]
        public async Task<IActionResult> UploadNewAttachment(NamedFile file, string reqNo, long RequestId)
        {
            if (file.File == null)
                return BadRequest("No file uploaded.");

            // Save file to disk (same as you do in UpdateAttachment)
            var savedFile = await SaveFileToDiskAsync(
                file.File,
                file.FieldName,
                _file, // your file storage path from config
                reqNo,
                file.IsRequired,
                file.FieldName,
                file.LabelName);

            // Wrap into view model to send to your API
            var SendData = new UpdatedAttachVM
            {
                FileSaveResponseVM = savedFile,
                RequestId = RequestId

            };

            // Send to API for DB update
            var apiUrl = _baseUrl + "api/ElawFront/Request/InsertUpdateAttachement";
            var result = await _helperUrlApi.PostDataToApi<UpdatedAttachVM, ErrorMessage>(apiUrl, SendData);

            if (result != null && !result.Error)
            {
                return Json(new { success = true, message = result.Message });
            }

            return Json(new { success = false, message = result?.Message ?? "Unknown error." });
        }

        #endregion
        #region LicencesListWithDetails

        public async Task<ActionResult> LicencesDetails(string id)
        {
            try
            {
                int LicId = 0;
                if (int.TryParse(MyCrypto.Decode(id), out LicId))
                {

                    var token = HttpContext.Session.GetString("UserToken");
                    var userId = HttpContext.Session.GetString("UserId");
                    var userName = HttpContext.Session.GetString("UserUserName");
                    var civilId = HttpContext.Session.GetString("UserCivilId");
                    var fullName = HttpContext.Session.GetString("UserFullName");
                    var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
                    var apiSetting = _baseUrl + $"api/ElawFront/GetLicenseDetails?id={LicId}";

                    ViewBag.PathAttachment = _file;

                    var response = await _helperUrlApi.GetDataFromApi<LicenceDetailsVM>(apiSetting);

                    return View(response);
                }
                else
                {
                    return RedirectToAction("RequestsList");
                }

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
        #endregion


        #region Handle
        [HttpGet]
        public async Task<ActionResult> HandleRequest(string id, int? requestTypeId, List<int>? selectedTransactionTypeIds)
        {
            int licId = int.TryParse(MyCrypto.Decode(id), out int decoded) ? decoded : 0;
            string queryParams = "";
            if (selectedTransactionTypeIds != null)
            {
               queryParams = string.Join("&", selectedTransactionTypeIds.Select(id => $"TransactionTypeIds={id}"));
            }
            var requestType = (RequestTypeEnum)requestTypeId;
            var apiSetting = requestType switch
            {
                RequestTypeEnum.Renew => $"api/ElawFront/GetLicenseDetailsForRenew?LicId={licId}",
                RequestTypeEnum.EndLicences => $"api/ElawFront/GetLicenseDetailsForEndLicences?LicId={licId}",
                RequestTypeEnum.Renouncement => $"api/ElawFront/GetLicenseDetailsForRenouncement?LicId={licId}",
               
                RequestTypeEnum.ReplacementOfLost => $"api/ElawFront/GetLicenceDetailsForReplacementOfLost?LicId={licId}",

                RequestTypeEnum.ChangeData=> $"api/ElawFront/GetLicenceDetailsForChangeData?LicId={licId}&&{queryParams}",
                _ => null
            };

            if (string.IsNullOrEmpty(apiSetting))
                return RedirectToAction("RequestsList");

            var result = await _helperUrlApi.GetDataFromApi<RequestElawBaseVM>(apiSetting); // You may cast dynamically

            ViewBag.RequestTypeId = requestTypeId;
            result.ReqtypeId = (int)requestTypeId;
            ViewBag.PathAttachment = _file;

            return requestType switch
            {
                RequestTypeEnum.Renew => View("RenewTourLicRequest", result),

                RequestTypeEnum.Renouncement => View("RenouncementLicRequest", result),
                RequestTypeEnum.EndLicences => View("EndLicencesTourLicRequest", result),
               
                RequestTypeEnum.ReplacementOfLost => View("ReplacementOfLostLicRequest", result),

                RequestTypeEnum.ChangeData=>View("ChangedataLicRequest",result),
                _ => View("DefaultRequestView", result)
            };
        }

        [HttpPost]
        public async Task<IActionResult> HandleRequest(RequestElawBaseVM model)
        {
            var token = HttpContext.Session.GetString("UserToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserUserName");
            var civilId = HttpContext.Session.GetString("UserCivilId");
            var fullName = HttpContext.Session.GetString("UserFullName");
            var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");

            // Setup user-related values
            model.SessionCivilId = civilId;
            model.SessionName = fullName;
            model.accountTypeId = accountTypeId;

           
            // File processing
            var savedFiles = new List<FileSaveResponseVM>();
            foreach (var file in model.NamedFile)
            {
                if (file.File != null)
                    savedFiles.Add(await SaveFileToDiskAsync(file.File, file.FieldName, _file, model.reqno, file.IsRequired, file.FieldName, file.LabelName));
            }

            // Generate request number
            var reqInfo = await _generalReqNo.GetRequestNoForElaw(model.ReqtypeId, model.LicencesVM.LicTypeId??0);
            model.reqno = reqInfo.Item2;
            model.SequenceNo = reqInfo.Item1;

            var apiRequest = BuildRequestApiModel(model, model.ReqtypeId, model.reqno, model.SequenceNo, savedFiles, civilId, fullName);

           
            string apiRoute = model.ReqtypeId switch
            {
                (int)RequestTypeEnum.Renew => "PostDataRenewRequest",           
                (int)RequestTypeEnum.ReplacementOfLost => "PostDataReplacementOfLostRequest",
                (int)RequestTypeEnum.Renouncement => "PostDataRenouncementRequest",
                (int)RequestTypeEnum.EndLicences => "PostDataEndLicencesRequest",
                (int)RequestTypeEnum.ChangeData=> "PostDataChangeDataRequest",
                _ => null
            };

            if (apiRoute == null)
                return BadRequest("Invalid request type.");

            var apiEndpoint = $"{_baseUrl}api/ElawFront/{apiRoute}";

            // ✅ Use the transformed model here
            var response=await _helperUrlApi.PostDataToApi<PostRequestApiModel, ErrorMessage>(apiEndpoint, apiRequest);

            if (response.Error != null || response.Error == false)
            {
                TempData["AlertTitle"] = "تأكـيد";
                TempData["Message"] = "تم حفظ البيانات بنجاح";
                TempData["AlertType"] = "Success";
                return RedirectToAction("GetAllRequest", "Home");

            }
            return RedirectToAction("Index", "Home");
        }
        private PostRequestApiModel BuildRequestApiModel(RequestElawBaseVM model, int requestTypeId, string reqNo, long sequenceNo, List<FileSaveResponseVM> files, string civilId, string fullName)
        {
            var apiModel = new PostRequestApiModel
            {
                reqno = reqNo,
                SequenceNo = sequenceNo,
                accountTypeId = model.accountTypeId,

                ActivityTypeId = model.LicencesVM.ActiivityTypeId,
                AppCivilId = model.LicencesVM.ApplicantCivilId,
               LicName=model.LicencesVM.LicName,
               //LicOwner=model.LicencesVM.Applicant.personName,
                //UserCivilID = model.UserCivilID,
                SessionCivilId = civilId,
                SessionName = fullName,
                CompanyId = model.LicencesVM.CompanyId,
                ManId = model.LicencesVM.ManagerId,
               ManagerCivilid=model.LicencesVM.ManagerCivilId,
                saveResponseVMs = files,
                LicId = model.LicencesVM.LicId,
                LicNo = model.LicencesVM.LicNo,
                OldCivilIdManager = model.LicencesVM.ManagerCivilId,
                
                UserName = fullName,
                LictypeId=model.LicencesVM.LicTypeId??0,
                MandoobId = model.MandoobId,
                AppId = model.LicencesVM.ApplicantId,
                ReqtypeId = requestTypeId
            };

            // Add conditional properties
            switch ((RequestTypeEnum)requestTypeId)
            {
                case RequestTypeEnum.Renouncement:
                    apiModel.NewCivilIdApplicant = model.NewCivilIdApplicant;
                    apiModel.NewMobileApplicant = model.NewMobileApplicant;
                    apiModel.NewEmailApplicant = model.NewEmailApplicant;
                    apiModel.NewAaliNoApplicant = model.NewAaliNoApplicant;
                    apiModel.NewAreaApplicant = model.NewAreaApplicant;
                    apiModel.NewBlockApplicant = model.NewBlockApplicant;
                    apiModel.NewBuildingNameApplicant = model.NewBuildingNameApplicant;
                    apiModel.NewBuildingNoApplicant = model.NewBuildingNoApplicant;
                    apiModel.NewFloorNoApplicant = model.NewFloorNoApplicant;
                    apiModel.NewGovernateApplicant = model.NewGovernateApplicant;
                    apiModel.NewStreetApplicant = model.NewStreetApplicant;
                    apiModel.NewUnitNoApplicant = model.NewUnitNoApplicant;
                    apiModel.NewQualificationApplicant=model.NewQualificationApplicant;
                    apiModel.NewApplicantName1 = model.NewApplicantName1;
                    apiModel.NewApplicantName2=model.NewApplicantName2;
                    apiModel.NewApplicantName3=model.NewApplicantName3;
                    apiModel.NewApplicantName4=model.NewApplicantName4;
                    apiModel.OldApplicantName1 = model.LicencesVM.Applicant.Name1;
                    apiModel.OldApplicantName2 = model.LicencesVM.Applicant.Name2;
                    apiModel.OldApplicantName3 = model.LicencesVM.Applicant.Name3;
                    apiModel.OldApplicantName4 = model.LicencesVM.Applicant.Name4;
                    var address = model.LicencesVM.Applicant.AddressNavigation;
                    apiModel.OldCivilIdApplicant = model.LicencesVM.Applicant.CivilId;
                    apiModel.OldMobileApplicant = model.LicencesVM.Applicant.Phone;
                    apiModel.OldEmailApplicant = model.LicencesVM.Applicant.Email;
                    apiModel.OldAaliNoApplicant = address.AalliNo;
                    apiModel.OldAreaApplicant = address.Area;
                    apiModel.OldBlockApplicant = address.BlockArabic;
                    apiModel.OldBuildingNameApplicant = address.BuildingName;
                    apiModel.OldBuildingNoApplicant = address.BuildingNo;
                    apiModel.OldFloorNoApplicant = address.FloorNo;
                    apiModel.OldGovernateApplicant = address.GovernorateArabic;
                    apiModel.OldStreetApplicant = address.StreetArabic;
                    apiModel.OldUnitNoApplicant = address.UnitNo;
                    apiModel.OldQualificationApplicant = model.LicencesVM.Applicant.QualificationsLookup.Id;
                    break;

                case RequestTypeEnum.EndLicences:
                    apiModel.EndingReasonId = model.EndingReasonId;
                    
                    break;

                case RequestTypeEnum.ChangeData:
                    apiModel.SelectedTransactionTypeIds = model.SelectedTransactionTypeIds;
                    if (model.SelectedTransactionTypeIds.Contains((int)TransactionTypesEnum.ChangeManager))
                    {
                        apiModel.NewCivilIdManager = model.NewManangerCivilId;
                        apiModel.NewMobileManager = model.NewManagerMobile;
                        apiModel.NewEmailManager = model.NewManagerEmail;
                        apiModel.NewAaliNoManager = model.NewAaliNoManager;
                        apiModel.NewAreaManager = model.NewAreaManager;
                        apiModel.NewBlockManager = model.NewBlockManager;
                        apiModel.NewBuildingNameManager = model.NewBuildingNameManager;
                        apiModel.NewBuildingNoManager = model.NewBuildingNoManager;
                        apiModel.NewFloorNoManager = model.NewFloorNoManager;
                        apiModel.NewGovernateManager = model.NewGovernateManager;
                        apiModel.NewStreetManager = model.NewStreetManager;
                        apiModel.NewUnitNoManager = model.NewUnitNoManager;
                        apiModel.NewQualificationManager = model.QualificationManagerId;
                        apiModel.NewManagerName1 = model.NewManagerName1;
                        apiModel.NewManagerName2 = model.NewManagerName2;
                        apiModel.NewManagerName3 = model.NewManagerName3;
                        apiModel.NewManagerName4 = model.NewManagerName4;
                        var addressManager = model.LicencesVM.Manager.AddressNavigation ;
                        apiModel.OldManagerName1 = model.LicencesVM.Manager.Name1;
                        apiModel.OldManagerName2 = model.LicencesVM.Manager.Name2;
                        apiModel.OldManagerName3 = model.LicencesVM.Manager.Name3;
                        apiModel.OldManagerName4 = model.LicencesVM.Manager.Name4;
                        apiModel.OldCivilIdManager = model.LicencesVM?.Manager?.CivilId;
                        apiModel.OldMobileManager = model.LicencesVM?.Manager?.Phone;
                        apiModel.OldEmailManager = model.LicencesVM?.Manager?.Email;

                        apiModel.OldAreaManager = addressManager?.Area;
                        apiModel.OldGovernateManager = addressManager?.GovernorateArabic;
                        apiModel.OldBlockManager = addressManager?.BlockArabic;
                        apiModel.OldStreetManager = addressManager?.StreetArabic;
                        apiModel.OldBuildingNoManager = addressManager?.BuildingNo;
                        apiModel.OldBuildingNameManager = addressManager?.BuildingName;
                        apiModel.OldUnitNoManager = addressManager?.UnitNo;
                        apiModel.OldFloorNoManager = addressManager?.FloorNo;
                        apiModel.OldAaliNoManager = addressManager?.AalliNo;
                       
                    }
                    if (model.SelectedTransactionTypeIds.Contains((int)TransactionTypesEnum.ChangeLicencesName))
                    {
                        apiModel.NewLicencesName = model.NewLicencesName;
                        apiModel.OldLicencesName = model.LicencesVM?.LicName;
                    }
                    if (model.SelectedTransactionTypeIds.Contains((int)TransactionTypesEnum.ChangeAddress))
                    {
                        apiModel.NewAreaManager = model.NewArea;
                        apiModel.NewGovernateManager = model.NewGovernate;
                        apiModel.NewAreaManager = model.NewArea;
                        apiModel.NewBlockManager = model.NewBlock;
                        apiModel.NewStreetManager = model.NewStreet;
                        apiModel.NewBuildingNoManager = model.NewBuildingNo;
                        apiModel.NewBuildingNameManager = model.NewBuildingName;
                        apiModel.NewUnitNoManager = model.NewUnitNo;
                        apiModel.NewFloorNoManager = model.NewFloorNo;
                        var addressManager = model.LicencesVM.Manager.AddressNavigation;
                        apiModel.OldManagerName1 = model.LicencesVM.Manager.Name1;
                        apiModel.OldManagerName2 = model.LicencesVM.Manager.Name2;
                        apiModel.OldManagerName3 = model.LicencesVM.Manager.Name3;
                        apiModel.OldManagerName4 = model.LicencesVM.Manager.Name4;
                        apiModel.OldCivilIdManager = model.LicencesVM?.Manager?.CivilId;
                        apiModel.OldMobileManager = model.LicencesVM?.Manager?.Phone;
                        apiModel.OldEmailManager = model.LicencesVM?.Manager?.Email;

                        apiModel.OldAreaManager = addressManager?.Area;
                        apiModel.OldGovernateManager = addressManager?.GovernorateArabic;
                        apiModel.OldBlockManager = addressManager?.BlockArabic;
                        apiModel.OldStreetManager = addressManager?.StreetArabic;
                        apiModel.OldBuildingNoManager = addressManager?.BuildingNo;
                        apiModel.OldBuildingNameManager = addressManager?.BuildingName;
                        apiModel.OldUnitNoManager = addressManager?.UnitNo;
                        apiModel.OldFloorNoManager = addressManager?.FloorNo;
                        apiModel.OldAaliNoManager = addressManager?.AalliNo;
                    }
                    if (model.SelectedTransactionTypeIds.Contains((int)TransactionTypesEnum.ChangeSocialMedia))
                    {
                        apiModel.NewFacebook = model.NewFacebook;
                        apiModel.NewInsta = model.NewInsta;
                        apiModel.NewTwitter = model.NewTwitter;
                        apiModel.NewWebSite = model.NewWebSite;
                    }
                    if (model.SelectedTransactionTypeIds.Contains((int)TransactionTypesEnum.ChangeEmail))
                    {
                        apiModel.NewEmailApplicant = model.NewEmailApplicant;
                        apiModel.OldEmailApplicant = model.LicencesVM?.Applicant?.Email;

                        apiModel.NewEmailManager = model.NewManagerEmail;
                        apiModel.OldEmailManager = model.LicencesVM?.Manager?.Email;
                    }
                    if (model.SelectedTransactionTypeIds.Contains((int)TransactionTypesEnum.ChangePartnerName))
                    {
                        apiModel.NewPartner1 = model.NewPartner1;
                        apiModel.NewPartner2 = model.NewPartner2;
                        apiModel.NewPartner3 = model.NewPartner3;
                        apiModel.NewPartner4 = model.NewPartner4;
                        apiModel.NewPartner5 = model.NewPartner5;

                    }
                    if (model.SelectedTransactionTypeIds.Contains((int)TransactionTypesEnum.ChangeLicencesType))
                    {
                        apiModel.NewLicencesTpeId = model.NewLicencesTpeId;
                        apiModel.OldLicencesTpeId = model.LicencesVM?.LicTypeId;

                    }
                    break;
            }

            return apiModel;
        }

        #endregion
        #region Payment 

        [HttpGet] 
        public async Task<ActionResult> TourismPay(string id)
        {
            try
            {
                int ReqID = 0;
                if (int.TryParse(MyCrypto.Decode(id), out ReqID))
                {

                    var apiSetting = _baseUrl + $"api/TourismFront/GetRequestDetails/{ReqID}";

                    ViewBag.PathAttachment = _file;

                    var response = await _helperUrlApi.GetDataFromApi<RequestFrontVM>(apiSetting);

                    return View(response);
                }
                else
                {
                    return RedirectToAction("RequestsList");
                }

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

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> TourismPay(RequestFrontVM model)
        {
            try
            {
                

                var paymentRequest = new PaymentRequestModel
                {
                    reqID = model.RequestVM.RequestId,
                    ServiceAmount = 10, 
                    userDateName = model.AspnetUserVM.FullNameAr,
                    StrRequesterMobile = model.AspnetUserVM.Mobile,
                    StrRequesterEmail = model.AspnetUserVM.Email,
                    ServicePrefixPaymentId=(int)ServicePrefixPaymentEnum.Tourism,
                
                    ApplicantCivilId= model.RequestVM.AppCivilId,
                    LicId=model.RequestVM.LicenseId,
                    ApplicantId = model.AspnetUserVM.Id,
                    
                };

                var payService = new PaymentGatewayService(_configuration);
                var link =await payService.GetPaymentLink((int)ServicePrefixPaymentEnum.Tourism, paymentRequest);

                if (string.IsNullOrEmpty(link))
                {
                    ViewBag.Message = "error";
                    ViewBag.test = "خطأ في عملية الدفع";
                    return View(model);
                }

                var apiSetting = _baseUrl + $"api/TourismFront/PostTourismPayment";

                ViewBag.PathAttachment = _file;

                var response = await _helperUrlApi.PostDataToApi<PaymentRequestModel,ErrorMessage>(apiSetting, paymentRequest);


                if (response!=null &&!response.Error) 
                {
                    return Redirect(link);
                }

                ViewBag.Message = "error";
                ViewBag.test = "خطأ في حفظ البيانات";
                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "error";
                ViewBag.test = ex.Message;
                return View(model);
            }
        }


        #endregion

    }
}
#endregion