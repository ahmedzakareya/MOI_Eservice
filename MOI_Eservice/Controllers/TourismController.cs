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
using System.Drawing.Drawing2D;
using Business.ViewModel.HomePage;





namespace MOI_Eservice.Controllers
{
    //[Authorize(AuthenticationSchemes = "UserScheme")]

    public class TourismController : Controller
    {
        private readonly string GetPACIAddressURL;
        private readonly string GetPACIUserTourism;
        private readonly string GetPACIPasswordTourism;
        private readonly string GetTokenURL;
        private readonly string _file;
        private readonly IConfiguration _configuration;
        private readonly GeneralReqNo _generalReqNo;
        private readonly HelperUrlApi _helperUrlApi;
        private readonly IWebHostEnvironment _env;
        private readonly string _baseUrl;
        public TourismController(IConfiguration configuration, GeneralReqNo generalReqNo, HelperUrlApi helperUrlApi, IWebHostEnvironment env)
        {
            GetPACIAddressURL = configuration["PaciAddressData:GetPACIAddressURL"];
            GetPACIUserTourism = configuration["PaciAddressData:GetPACIUserTourism"];
            GetPACIPasswordTourism = configuration["PaciAddressData:GetPACIPasswordTourism"];
            GetTokenURL = configuration["PaciAddressData:GetTokenURL"];
            _file = configuration["Path:Tourism"];

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
            var apiSetting = _baseUrl + "api/TourismFront/GetActivitiesForPreApproval";

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
            var apiSetting = _baseUrl + $"api/TourismFront/GetActivitiesForPreApproval/{id}";

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

        #region موافقة مبدئية
        //[HttpGet]
        //public async Task<ActionResult> PreApprovRequest()
        //{

        //    try
        //    {
        //        var (activities2, fileUploadConfigs) = await _helperUrlApi.GetMultipleDataFromApiWithSelectListHandling(
        //                      _baseUrl + "api/TourismFront/GetActivitiesForPreApproval",
        //                      _baseUrl + "api/TourismFront/GetFilesForPreApproval"
        //                  );


        //        var token = HttpContext.Session.GetString("UserToken");
        //        var userId = HttpContext.Session.GetString("UserId");
        //        var userName = HttpContext.Session.GetString("UserUserName");
        //        var civilId = HttpContext.Session.GetString("UserCivilId");
        //        var fullName = HttpContext.Session.GetString("UserFullName");

        //        var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
        //        if (token != null)
        //        {
        //            PreApprovalRequestModel PreApprovalRequest = new PreApprovalRequestModel
        //            {
        //                Activities = activities2,
        //                AppCivilId = civilId,
        //                UserName = fullName,

        //                fileUploadConfigs = fileUploadConfigs
        //            };
        //            if (accountTypeId == "100")
        //            {
        //                PreApprovalRequest.AppId = userId;
        //            }
        //            else if (accountTypeId == "300")
        //            {
        //                PreApprovalRequest.MandoobId = userId;
        //            }
        //            //PreApprovalRequest.CompanyUserId = userId;
        //            return View(PreApprovalRequest);


        //        }
        //        return RedirectToAction("Index", "Home");
        //    }
        //    catch (Exception ex)
        //    {
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        string fileName = controllerName + "_" + actionName + "_";

        //        string exId = ExceptionLog.LogException(ex, fileName);

        //        TempData["Ex"] = exId;
        //        throw;
        //    }


        //}

        [HttpGet]
        public async Task<ActionResult> PreApprovRequest(int id)
        {

            try
            {
                //var (activities2, fileUploadConfigs) = await _helperUrlApi.GetMultipleDataFromApiWithSelectListHandling(
                //              _baseUrl + "api/TourismFront/GetActivitiesForPreApproval",
                //              _baseUrl + "api/TourismFront/GetFilesForPreApproval"
                //          );


                var token = HttpContext.Session.GetString("UserToken");
                var userId = HttpContext.Session.GetString("UserId");
                var userName = HttpContext.Session.GetString("UserUserName");
                var civilId = HttpContext.Session.GetString("UserCivilId");
                var fullName = HttpContext.Session.GetString("UserFullName");

                var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
                if (token != null)
                {

                    var url = _baseUrl + $"api/TourismFront/GetPreApproveRequest?licencesInfoId={id}";
                    var response = await _helperUrlApi.GetDataFromApi<PreApprovalRequestModel>(url);
                    response.LicencesInfoVM.Id = id;
                    return View(response);


                }
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


        }
        public async Task<List<FileUploadConfigVM>> GetPreApproveConfigs()
        {

            var apiSetting = _baseUrl + "api/TourismFront/GetFilesForPreApproval";
            var filemodel = await _helperUrlApi.GetDataFromApi<List<FileUploadConfigVM>>(apiSetting);
            return filemodel;
        }

        [HttpPost]
        public async Task<ActionResult> PreApprovRequest([FromForm] PreApprovalRequestModel preApprovalRequest)
        {
            var apiSetting = _baseUrl + $"api/TourismFront/PreApprovalRequest";
            List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();
           
            Tuple<long, string> preApprovalData = await _generalReqNo.GetRequestNo(preApprovalRequest.LicencesInfoVM.ReqTypeId??0, preApprovalRequest.ActivityCode);

            // Unpack the values
            long sequenceNo = preApprovalData.Item1;
            string reqNo = preApprovalData.Item2;
            var token = HttpContext.Session.GetString("UserToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserUserName");
            var civilId = HttpContext.Session.GetString("UserCivilId");
            var fullName = HttpContext.Session.GetString("UserFullName");
            var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
            if (token != null)
            { 
            //if (accountTypeId == "100")
            //{
            //    preApprovalRequest.AppId = userId;
            //    preApprovalRequest.AppCivilId = civilId;
            //}
            //else if (accountTypeId == "300")
            //{
            //    preApprovalRequest.MandoobId = userId;
            //    preApprovalRequest.UserCivilID = civilId;
            //}

            try
            {

                foreach (var file in preApprovalRequest.NamedFile)
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
                var ModelToApi = new PreApprovalRequestApiModel
                {
                    reqno = reqNo,
                    SequenceNo = sequenceNo,
                    accountTypeId = accountTypeId,
                    licencesInfoId= preApprovalRequest.LicencesInfoVM.Id,
                    AaliNumber = preApprovalRequest.AaliNumber,
                    ReqtypeId= preApprovalRequest.LicencesInfoVM.ReqTypeId,
                    ActivityCode = preApprovalRequest.ActivityCode,
                    ActivityTypeId = preApprovalRequest.ActivityTypeId,
                    AppCivilId = preApprovalRequest.UserCivilID,
                    SalesManagerCivilId= preApprovalRequest.SalesManagerCivilId,
                    SalesManagerEmail = preApprovalRequest.SalesManagerEmail,
                    SalesManagerName = preApprovalRequest.SalesManagerName,
                    SalesManagerPhone=preApprovalRequest.SalesManagerMobile,
                    MarketingManagerCivilId=preApprovalRequest.MarketingManagerCivilId,
                    MarketingManagerEmail = preApprovalRequest.MarketingManagerEmail,
                    MarketingManagerName = preApprovalRequest.MarketingManagerName,
                    MarketingManagerPhone=preApprovalRequest.MarketingManagerMobile,
                    OperationManagerCivilId=preApprovalRequest.OperationManagerCivilId,
                    OperationManagerEmail=preApprovalRequest.OperationManagerEmail,
                    OperationManagerName=preApprovalRequest.OperationManagerName,
                    OperationManagerPhone=preApprovalRequest.OperationManagerMobile,
                    LicencesName=preApprovalRequest.Request.Licname,
                    //UserCivilID = preApprovalRequest.UserCivilID,
                    Area = preApprovalRequest.Area,
                    AreaSize = preApprovalRequest.AreaSize,
                    AreaChartNo = preApprovalRequest.AreaChartNo,
                    BlockNo = preApprovalRequest.BlockNo,
                    BuildingNo = preApprovalRequest.BuildingNo,
                    ManagerEmail = preApprovalRequest.ManagerEmail,
                    ManagerName = preApprovalRequest.ManagerName,
                    CompanyCivilId = preApprovalRequest.CompanyCivilId,
                    CompanyActivity = preApprovalRequest.ActivityName,
                    ManagerMobile = preApprovalRequest.ManagerMobile,
                    
                    saveResponseVMs = filePath,
                    DirCompanyAr = preApprovalRequest.DirCompanyAr,
                    BuildingName = preApprovalRequest.BuildingName,
                    Street = preApprovalRequest.Street,
                    Governrate = preApprovalRequest.Governrate,
                    ManCivilId = preApprovalRequest.ManCivilId,
                    OwnerCompanyAr = preApprovalRequest.OwnerCompanyAr,
                    UserName = preApprovalRequest.UserName,
                    RecordNo = preApprovalRequest.RecordNo,
                    CommercialLicNo = preApprovalRequest.CommercialLicNo,
                    OwnerCoAddress = preApprovalRequest.OwnerCoAddress,
                    UnitNo = preApprovalRequest.UnitNo,
                    FloorNo = preApprovalRequest.FloorNo,
                    SessionCivilId=civilId,
                    SessionName=fullName
                };

                var response = await _helperUrlApi.PostDataToApi<PreApprovalRequestApiModel, ErrorMessage>(apiSetting, ModelToApi);

                
                    if (response != null && response.Error == false)
                    {
                        TempData["AlertTitle"] = "تأكـيد";
                        TempData["Message"] = "تم حفظ البيانات بنجاح";
                        TempData["AlertType"] = "Success";
                        return RedirectToAction("GetAllRequest", "Home");
                    }
                    else
                    {
                        return View(preApprovalRequest);

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
            return RedirectToAction("Index", "Home");
            
        }

        [HttpPost]
        public async Task<ActionResult> UpdatePreApprovalDetails(RequestFrontVM model)
        {
            var token = HttpContext.Session.GetString("UserToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserUserName");
            var civilId = HttpContext.Session.GetString("UserCivilId");
            var fullName = HttpContext.Session.GetString("UserFullName");
            List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();

            var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
            if(token!=null)
            {
                try 
                {
                    if (model.NamedFile != null)
                    {
                        foreach (var file in model.NamedFile)
                        {
                            if (file.File != null)
                            {
                                // Save each file to disk
                                var savedFilePath = await SaveFileToDiskAsync(file.File, file.FieldName, _file, model.RequestVM.Reqno, file.IsRequired, file.FieldName, file.LabelName);

                                filePath.Add(savedFilePath);
                                // Perform any additional logic with the saved file path, if needed
                                Console.WriteLine($"File saved at: {savedFilePath}");

                            }
                        }
                    }
                    var apiSetting = _baseUrl + $"api/TourismFront/UpdatePreApprovalDetails";
                    
                    var ModelToApi = new PreApprovalRequestApiModel
                    {
                        RequestId = model.RequestVM.RequestId,
                        PreApproveId = model.RequestVM.PreApprovalId,
                        AppId = model.RequestVM.AppId,
                        //BuildingId = model.RequestVM.BuildingId,
                        CompanyId = model.RequestVM.CompanyId,
                        ManId = model.RequestVM.ManagerId,
                        MarketingManagerId = model.RequestVM.MarketingManagerId,
                        SalesManagerId = model.RequestVM.SalesManagerId,
                        OperationManagerId = model.RequestVM.OperationsManagerId,
                        ActivityTypeId = model.RequestVM.ActivityTypeId,
                        //LicId = model.RequestVM.LicenseId,
                        reqno = model.RequestVM.Reqno,
                        SequenceNo = model.RequestVM.SequenceNo??0,
                        accountTypeId = accountTypeId,

                        saveResponseVMs = filePath,
                        AaliNumber = model.RequestVM.company?.AddressNavigation?.AalliNo,
                        //ReqtypeId = model.RequestVM.ReqtypeId,
                        //ActivityCode = model.RequestVM.ActivityCode,
                        
                        AppCivilId = model.RequestVM.ApplicantPerson.CivilId,

                        SalesManagerCivilId = model.RequestVM.SalesManager.CivilId,
                        SalesManagerEmail = model.RequestVM.SalesManager.Email,
                        SalesManagerName = model.RequestVM.SalesManager.Name1,
                        SalesManagerPhone = model.RequestVM.SalesManager.Phone,

                        MarketingManagerCivilId = model.RequestVM.MarketingManager.CivilId,
                        MarketingManagerEmail = model.RequestVM.MarketingManager.Email,
                        MarketingManagerName = model.RequestVM.MarketingManager.Name1,
                        MarketingManagerPhone = model.RequestVM.MarketingManager.Phone,

                        OperationManagerCivilId = model.RequestVM.OperationsManager.CivilId,
                        OperationManagerEmail = model.RequestVM.OperationsManager.Email,
                        OperationManagerName = model.RequestVM.OperationsManager.Name1,
                        OperationManagerPhone = model.RequestVM.OperationsManager.Phone,

                        LicencesName = model.RequestVM.Licname,

                        Area = model.RequestVM.company?.AddressNavigation?.Area,
                        AreaSize = model.RequestVM.company?.AddressNavigation?.AreaSize,
                        AreaChartNo = model.RequestVM.company?.AddressNavigation?.AreaChartNo,
                        BlockNo = model.RequestVM.company?.AddressNavigation?.BlockArabic,
                        BuildingNo = model.RequestVM.company?.AddressNavigation?.BuildingNo,

                        ManagerEmail = model.RequestVM.Manager?.Email,
                        ManagerName = model.RequestVM.Manager?.Name1,
                        ManagerMobile = model.RequestVM.Manager?.Phone,
                        ManCivilId = model.RequestVM.Manager.CivilId,

                        CompanyCivilId = model.RequestVM.company?.CompanyCivilId,
                      
                        DirCompanyAr = model.RequestVM.company?.DirCompanyAr,
                        BuildingName = model.RequestVM.company?.AddressNavigation?.BuildingName,
                        Street = model.RequestVM.company?.AddressNavigation?.StreetArabic,
                        Governrate = model.RequestVM.company?.AddressNavigation?.GovernorateArabic,
                        OwnerCompanyAr = model.RequestVM.company?.OwnerCompanyAr,
                    
                        RecordNo = model.RequestVM.company?.RecordNo,
                        CommercialLicNo = model.RequestVM.company?.CommercialLicNo,
                   

                        UnitNo = model.RequestVM.company?.AddressNavigation?.UnitNo,
                        FloorNo = model.RequestVM.company?.AddressNavigation?.FloorNo,

                        SessionCivilId =civilId,
                        SessionName = fullName
                    };

                    var response = await _helperUrlApi.PostDataToApi<PreApprovalRequestApiModel, ErrorMessage>(apiSetting, ModelToApi);
             

                    
                    if(response==null ||response.Error !=true)
                    {
                        TempData["AlertTitle"] = "تأكـيد";
                        TempData["Message"] = "تم حفظ البيانات بنجاح";
                        TempData["AlertType"] = "Success";
                        return RedirectToAction("GetAllRequest", "Home");
                    }
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
              
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region CheckPreApproval

        [HttpGet]
        public ActionResult CheckPreApproval(int? id)
        {

            try
            {
                
                CheckPreAprroval preapprovDetails = new CheckPreAprroval();
                //preapprovDetails.AppId = userId;
                preapprovDetails.id = id;
                return View(preapprovDetails);
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
        public async Task<ActionResult> CheckPreApproval(CheckPreAprroval preapprovDetails)
        {
            try
            {
                
                var civilId = HttpContext.Session.GetString("UserCivilId");
               
                var apiSetting = _baseUrl + $"api/TourismFront/CheckPreApprovalForUserAndUse";
                preapprovDetails.CivilId = civilId;
                preapprovDetails.PreApprove = preapprovDetails.PreApprove;
                preapprovDetails.id = preapprovDetails.id;
                // Call API
                var response = await _helperUrlApi.PostDataToApi<CheckPreAprroval, PreApprovalResult>(apiSetting, preapprovDetails);

                // Assuming the response will be a boolean value indicating success or failure
                if (response != null && response.IsValid)
                {
                    TempData["AlertTitle"] = "تأكـيد";
                    TempData["Message"] = response.Message;
                    TempData["AlertType"] = "Success";

                    return RedirectToAction("TourLicRequest", "Tourism", new { PreApprovNo = preapprovDetails.PreApprove });
                }

                ViewBag.Message = "Error";
                ViewBag.test = response?.Message ?? "فشل التحقق من الموافقة";
                return View(preapprovDetails);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error";
                ViewBag.test = ex.Message.ToString();
                return View(preapprovDetails);
            }
        }

        #endregion
        #region طلب إصدار فندق أو شقق فندقية أو منتجعات  


        [HttpGet]
        public async Task<ActionResult> TourLicRequest(string PreApprovNo)
        {

            try
            {
                


                var apiSetting = _baseUrl + $"api/TourismFront/GetTourLicRequestDetails?PreApproval={PreApprovNo}";


                var response = await _helperUrlApi.GetDataFromApi<TourLicRequestResponseVM>(apiSetting);





                return View(response);
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
        public async Task<ActionResult> TourLicRequest(TourLicRequestResponseVM TourLicRequestResponseVM)
        {

            try
            {
                var token = HttpContext.Session.GetString("UserToken");
                var userId = HttpContext.Session.GetString("UserId");
                var userName = HttpContext.Session.GetString("UserUserName");
                var civilId = HttpContext.Session.GetString("UserCivilId");
                var fullName = HttpContext.Session.GetString("UserFullName");
                var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
                Tuple<long, string> requestData = await _generalReqNo.GetRequestNo((int)RequestTypeEnum.Request, TourLicRequestResponseVM.PreApprovalDetails.Company.ActivityCode);
                string reqNo = requestData.Item2;
                long SequenceNo = requestData.Item1;
                List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();

                //if (accountTypeId == "100")
                //{
                //    //TourLicRequestResponseVM.PreApprovalDetails.AppId = userId;
                //    TourLicRequestResponseVM.PreApprovalDetails.ApplicantCivilId = civilId;
                //}
                //else if (accountTypeId == "300")
                //{
                //    TourLicRequestResponseVM.PreApprovalDetails.MandoobId = userId;
                //    TourLicRequestResponseVM.PreApprovalDetails.UserCivilId = civilId;
                //}


                foreach (var file in TourLicRequestResponseVM.NamedFile)
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
                var ModelToApi = new PreApprovalRequestApiModel
                {
                    reqno = reqNo,
                    SequenceNo = SequenceNo,
                    accountTypeId = accountTypeId,
                    PreApproveId= TourLicRequestResponseVM.PreApprovalDetails.PreAppId,
                    ManId= TourLicRequestResponseVM.PreApprovalDetails.ManagerId,
                    SalesManagerId= TourLicRequestResponseVM.PreApprovalDetails.Request.SalesManagerId,
                    MarketingManagerId= TourLicRequestResponseVM.PreApprovalDetails.Request.MarketingManagerId,
                    OperationManagerId= TourLicRequestResponseVM.PreApprovalDetails.Request.OperationsManagerId,
                    CompanyId= TourLicRequestResponseVM.PreApprovalDetails.Request.CompanyId,
                    AddressId= TourLicRequestResponseVM.PreApprovalDetails.Company.AddressId,
                    SalesManagerCivilId = TourLicRequestResponseVM.PreApprovalDetails.Request.SalesManager.CivilId,
                    SalesManagerEmail = TourLicRequestResponseVM.PreApprovalDetails.Request.SalesManager.Email,
                    SalesManagerName = TourLicRequestResponseVM.PreApprovalDetails.Request.SalesManager.Name1,
                    SalesManagerPhone = TourLicRequestResponseVM.PreApprovalDetails.Request.SalesManager.Phone,
                    MarketingManagerCivilId = TourLicRequestResponseVM.PreApprovalDetails.Request.MarketingManager.CivilId,
                    MarketingManagerEmail = TourLicRequestResponseVM.PreApprovalDetails.Request.MarketingManager.Email,
                    MarketingManagerName = TourLicRequestResponseVM.PreApprovalDetails.Request.MarketingManager.Name1,
                    MarketingManagerPhone = TourLicRequestResponseVM.PreApprovalDetails.Request.MarketingManager.Phone,
                    OperationManagerCivilId = TourLicRequestResponseVM.PreApprovalDetails.Request.OperationsManager.CivilId,
                    OperationManagerEmail = TourLicRequestResponseVM.PreApprovalDetails.Request.OperationsManager.Email,
                    OperationManagerName = TourLicRequestResponseVM.PreApprovalDetails.Request.OperationsManager.Name1,
                    OperationManagerPhone = TourLicRequestResponseVM.PreApprovalDetails.Request.OperationsManager.Phone,
                    Amount = TourLicRequestResponseVM.LicencesInfoVM.FixedFees,
                    AaliNumber = TourLicRequestResponseVM.PreApprovalDetails.Company.AddressNavigation.AalliNo,
                    ActivityCode = TourLicRequestResponseVM.PreApprovalDetails.Company.ActivityCode,
                    ActivityTypeId = TourLicRequestResponseVM.PreApprovalDetails.Company.ActivityTypeId,
                    AppCivilId = TourLicRequestResponseVM.PreApprovalDetails.Applicant.CivilId,
                    //UserCivilID = TourLicRequestResponseVM.PreApprovalDetails.Applicant.UserCivilId,
                    Area = TourLicRequestResponseVM.PreApprovalDetails.Company.AddressNavigation.Area,
                    AreaSize = TourLicRequestResponseVM.PreApprovalDetails.Company.AddressNavigation.AreaSize,
                    AreaChartNo = TourLicRequestResponseVM.PreApprovalDetails.Company.AddressNavigation.AreaChartNo,
                    BlockNo = TourLicRequestResponseVM.PreApprovalDetails.Company.AddressNavigation.BlockArabic,
                    BuildingNo = TourLicRequestResponseVM.PreApprovalDetails.Company.AddressNavigation.BuildingNo,
                    ManagerEmail = TourLicRequestResponseVM.PreApprovalDetails.Manager.Email,
                    ManagerName = TourLicRequestResponseVM.PreApprovalDetails.Manager.Name1,
                    CompanyCivilId = TourLicRequestResponseVM.PreApprovalDetails.Company.CompanyCivilId,
                    CompanyActivity = TourLicRequestResponseVM.PreApprovalDetails.Company.CompanyActivity,
                    ManagerMobile = TourLicRequestResponseVM.PreApprovalDetails.Manager.Phone,
                    ReqtypeId = (int)RequestTypeEnum.Request,
                    LicencesName = TourLicRequestResponseVM.PreApprovalDetails.LicenseName,
                    PreApprove = TourLicRequestResponseVM.PreApprovalDetails.LicenseNo,
                    saveResponseVMs = filePath,
                    DirCompanyAr = TourLicRequestResponseVM.PreApprovalDetails.Company.DirCompanyAr,
                    BuildingName = TourLicRequestResponseVM.PreApprovalDetails.Company.AddressNavigation.BuildingName,
                    Street = TourLicRequestResponseVM.PreApprovalDetails.Company.AddressNavigation.StreetArabic,
                    Governrate = TourLicRequestResponseVM.PreApprovalDetails.Company.AddressNavigation.GovernorateArabic,
                    ManCivilId = TourLicRequestResponseVM.PreApprovalDetails.Manager.CivilId,
                    OwnerCompanyAr = TourLicRequestResponseVM.PreApprovalDetails.Company.OwnerCompanyAr,
                    UserName = fullName,
                    RecordNo = TourLicRequestResponseVM.PreApprovalDetails.RecordNo,
                    CommercialLicNo = TourLicRequestResponseVM.PreApprovalDetails.CommercialLicNo,
                    UnitNo = TourLicRequestResponseVM.PreApprovalDetails.Company.AddressNavigation.UnitNo,
                    FloorNo = TourLicRequestResponseVM.PreApprovalDetails.Company.AddressNavigation.FloorNo,
                    MandoobId = TourLicRequestResponseVM.PreApprovalDetails.MandoobId,
                    AppId = TourLicRequestResponseVM.PreApprovalDetails.AppId,
                    SessionCivilId = civilId,
                    SessionName = fullName,

                };
                var apiSetting = _baseUrl + $"api/TourismFront/PostDataRequest";

                var response = await _helperUrlApi.PostDataToApi<PreApprovalRequestApiModel, ErrorMessage>(apiSetting, ModelToApi);

                if (response != null && response.Error == false)
                {
                    TempData["AlertTitle"] = "تأكـيد";
                    TempData["Message"] = "تم حفظ البيانات بنجاح";
                    TempData["AlertType"] = "Success";
                    return RedirectToAction("GetAllRequest", "Home");
                }
                else
                {
                    TempData["AlertTitle"] = "تأكـيد";
                    TempData["Message"] = "حدث خطأ أثناء الإرسال";
                    TempData["AlertType"] = "فشل";
                    return View();

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
        public async Task<ActionResult> UpdateLicRequestDetails(RequestFrontVM model)
        {
            var token = HttpContext.Session.GetString("UserToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserUserName");
            var civilId = HttpContext.Session.GetString("UserCivilId");
            var fullName = HttpContext.Session.GetString("UserFullName");

            var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
            if (token != null)
            {
                try
                {
                    var apiSetting = _baseUrl + $"api/TourismFront/UpdateLicRequestDetails";
                    var ModelApi = new PreApprovalRequestApiModel
                    {
                        RequestId = model.RequestVM.RequestId,
                        PreApproveId = model.RequestVM.PreApprovalId,
                        AppId = model.RequestVM.AppId,
                        //BuildingId = model.RequestVM.BuildingId,
                        CompanyId = model.RequestVM.CompanyId,
                        ManId = model.RequestVM.ManagerId,
                        MarketingManagerId = model.RequestVM.MarketingManagerId,
                        SalesManagerId = model.RequestVM.SalesManagerId,
                        OperationManagerId = model.RequestVM.OperationsManagerId,
                        ActivityTypeId = model.RequestVM.ActivityTypeId,
                        //LicId = model.RequestVM.LicenseId,
                        reqno = model.RequestVM.Reqno,
                        SequenceNo = model.RequestVM.SequenceNo ?? 0,
                        accountTypeId = accountTypeId,

                   
                        AaliNumber = model.RequestVM.company?.AddressNavigation?.AalliNo,
                        //ReqtypeId = model.RequestVM.ReqtypeId,
                        //ActivityCode = model.RequestVM.ActivityCode,

                        AppCivilId = model.RequestVM.ApplicantPerson.CivilId,

                        SalesManagerCivilId = model.RequestVM.SalesManager.CivilId,
                        SalesManagerEmail = model.RequestVM.SalesManager.Email,
                        SalesManagerName = model.RequestVM.SalesManager.Name1,
                        SalesManagerPhone = model.RequestVM.SalesManager.Phone,

                        MarketingManagerCivilId = model.RequestVM.MarketingManager.CivilId,
                        MarketingManagerEmail = model.RequestVM.MarketingManager.Email,
                        MarketingManagerName = model.RequestVM.MarketingManager.Name1,
                        MarketingManagerPhone = model.RequestVM.MarketingManager.Phone,

                        OperationManagerCivilId = model.RequestVM.OperationsManager.CivilId,
                        OperationManagerEmail = model.RequestVM.OperationsManager.Email,
                        OperationManagerName = model.RequestVM.OperationsManager.Name1,
                        OperationManagerPhone = model.RequestVM.OperationsManager.Phone,

                        LicencesName = model.RequestVM.Licname,

                        Area = model.RequestVM.company?.AddressNavigation?.Area,
                        AreaSize = model.RequestVM.company?.AddressNavigation?.AreaSize,
                        AreaChartNo = model.RequestVM.company?.AddressNavigation?.AreaChartNo,
                        BlockNo = model.RequestVM.company?.AddressNavigation?.BlockArabic,
                        BuildingNo = model.RequestVM.company?.AddressNavigation?.BuildingNo,

                        ManagerEmail = model.RequestVM.Manager?.Email,
                        ManagerName = model.RequestVM.Manager?.Name1,
                        ManagerMobile = model.RequestVM.Manager?.Phone,
                        ManCivilId = model.RequestVM.Manager.CivilId,

                        CompanyCivilId = model.RequestVM.company?.CompanyCivilId,

                        DirCompanyAr = model.RequestVM.company?.DirCompanyAr,
                        BuildingName = model.RequestVM.company?.AddressNavigation?.BuildingName,
                        Street = model.RequestVM.company?.AddressNavigation?.StreetArabic,
                        Governrate = model.RequestVM.company?.AddressNavigation?.GovernorateArabic,
                        OwnerCompanyAr = model.RequestVM.company?.OwnerCompanyAr,

                        RecordNo = model.RequestVM.company?.RecordNo,
                        CommercialLicNo = model.RequestVM.company?.CommercialLicNo,


                        UnitNo = model.RequestVM.company?.AddressNavigation?.UnitNo,
                        FloorNo = model.RequestVM.company?.AddressNavigation?.FloorNo,

                        SessionCivilId = civilId,
                        SessionName = fullName
                    };
                    var response = await _helperUrlApi.PostDataToApi<PreApprovalRequestApiModel, ErrorMessage>(apiSetting, ModelApi);

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

            }
            return RedirectToAction("Index", "Home");
        }

        //[HttpGet]
        //public ActionResult RenewTourLicRequest(string id)
        //{

        //    try
        //    {
        //        VLictourism2 model = new VLictourism2();
        //        string _id = MyCrypto.Decode(id);

        //        model = _LicenseDetails(int.Parse(_id));

        //        RenewTourismLicensesRequestModel request = new RenewTourismLicensesRequestModel();
        //        request.UserCivilID = model.CivilId.ToString();
        //        request.UserName = model.UserName;
        //        request.CompanyCivilId = model.CompanyCivilId;
        //        request.HotelName = model.Name;
        //        request.DirCompanyAr = model.DirCompanyAr;
        //        request.OwnerCompanyAr = model.OwnerCompanyAr;
        //        request.OwnerCoCommercialNo = model.OwnerCoCommercialNo;
        //        request.OwnerCoAddress = model.OwnerCoAddress;
        //        request.AaliNumber = model.AaliNumber;
        //        request.CompanyActivity = model.CompanyActivity;
        //        request.ActivityCode = model.LicActivityTypeId.ToString();
        //        request.AreaSize = model.AreaSize;
        //        request.AreaChartNo = model.AreaChartNo;
        //        request.CommercialLicenseNo = model.CommercialLicenseNo;
        //        request.licenseNo = model.LicenseNo;


        //        request.licenseExpireDate = DateTime.Parse(model.LicenseExpireDate.ToString());
        //        request.licenseIssueDate = DateTime.Parse(model.LicenseIssueDate.ToString());
        //        request.licenceId = model.LicenceId.ToString();

        //        int ReNewLicAmount = 0;
        //        if (request.ActivityCode == "551011")
        //        {
        //            ReNewLicAmount = GeneralFunc.GetAmount(51);
        //        }
        //        else if (request.ActivityCode == "551020")
        //        {
        //            ReNewLicAmount = GeneralFunc.GetAmount(103);
        //        }
        //        else if (request.ActivityCode == "681015")
        //        {
        //            ReNewLicAmount = GeneralFunc.GetAmount(103);
        //        }

        //        request.ServiceAmount = ReNewLicAmount.ToString();

        //        return View(request);
        //    }
        //    catch (Exception ex)
        //    {
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        string fileName = controllerName + "_" + actionName + "_";

        //        string exId = ExceptionLog.LogException(ex, fileName);

        //        TempData["Ex"] = exId;
        //        throw;
        //    }

        //}


        #endregion

        #region لمن يهمه الأمر 
        //[HttpGet]
        //public async Task<ActionResult> WhoConcTourLicRequest(int id)
        //{

        //    try
        //    {

        //        int LicId = 0;
        //        if (int.TryParse(MyCrypto.Decode(id.ToString()), out LicId))
        //        {

        //            //var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        //            //var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        //            //var civilId = User.FindFirst("CivilId")?.Value;  // Custom claim
        //            //var fullName = User.FindFirst("FullName")?.Value;
        //            //var accountTypeId = User.FindFirst("AccouuntTypeId")?.Value;

        //            var apiSetting = _baseUrl + $"api/TourismFront/GetLicenceDetailsForWhoConc/{LicId}";



        //            var response = await _helperUrlApi.GetDataFromApi<WhoConcRequestVm>(apiSetting);

        //            return View(response);
        //        }
        //        else
        //        {
        //            return RedirectToAction("RequestsList");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        string fileName = controllerName + "_" + actionName + "_";

        //        string exId = ExceptionLog.LogException(ex, fileName);

        //        TempData["Ex"] = exId;
        //        throw;
        //    }

        //}
        //[HttpPost]
        //public async Task<ActionResult> WhoConcTourLicRequest(WhoConcRequestVm TourLicRequestResponseVM)
        //{

        //    try
        //    {
        //        //var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        //        //var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        //        //var civilId = User.FindFirst("CivilId")?.Value;  // Custom claim
        //        //var fullName = User.FindFirst("FullName")?.Value;
        //        //var accountTypeId = User.FindFirst("AccouuntTypeId")?.Value;
        //        var token = HttpContext.Session.GetString("UserToken");
        //        var userId = HttpContext.Session.GetString("UserId");
        //        var userName = HttpContext.Session.GetString("UserUserName");
        //        var civilId = HttpContext.Session.GetString("UserCivilId");
        //        var fullName = HttpContext.Session.GetString("UserFullName");
        //        var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
        //        Tuple<long, string> requestData = await _generalReqNo.GetRequestNo((int)RequestTypeEnum.Request, TourLicRequestResponseVM.ActivityType.ActivityCode);
        //        string reqNo = requestData.Item2;
        //        long SequenceNo = requestData.Item1;
        //        List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();

        //        if (accountTypeId == "100")
        //        {
        //            TourLicRequestResponseVM.LicencesVM.ApplicantId = userId;
        //            TourLicRequestResponseVM.LicencesVM.ApplicantCivilId = civilId;
        //        }
        //        else if (accountTypeId == "300")
        //        {
        //            TourLicRequestResponseVM.LicencesVM.MandoobId = userId;
        //            TourLicRequestResponseVM.LicencesVM.UserCivilId = civilId;
        //        }


        //        foreach (var file in TourLicRequestResponseVM.NamedFile)
        //        {
        //            if (file.File != null)
        //            {
        //                // Save each file to disk
        //                var savedFilePath = await SaveFileToDiskAsync(file.File, file.FieldName, _file, reqNo, file.IsRequired, file.FieldName, file.LabelName);

        //                filePath.Add(savedFilePath);
        //                // Perform any additional logic with the saved file path, if needed
        //                Console.WriteLine($"File saved at: {savedFilePath}");

        //            }
        //        }
        //        var ModelToApi = new PreApprovalRequestApiModel
        //        {
        //            reqno = reqNo,
        //            SequenceNo = SequenceNo,
        //            accountTypeId = accountTypeId,
        //            Amount = TourLicRequestResponseVM.LicencesInfo.FixedFees,
        //            LicNo = TourLicRequestResponseVM.LicencesVM.LicNo,
        //            LicencesName = TourLicRequestResponseVM.LicencesVM.LicName,
        //            ManId = TourLicRequestResponseVM.LicencesVM.ManagerId,
        //            SessionCivilId = civilId,
        //            SessionName = fullName,
        //            ActivityTypeId = TourLicRequestResponseVM.LicencesVM.ActiivityTypeId,
        //            AppCivilId = TourLicRequestResponseVM.LicencesVM.ApplicantCivilId,
        //            UserCivilID = TourLicRequestResponseVM.LicencesVM.UserCivilId,
        //            ReqtypeId = (int)RequestTypeEnum.WhoConc,
        //            saveResponseVMs = filePath,
        //            ManCivilId = TourLicRequestResponseVM.LicencesVM.ManagerCivilId,
        //            OwnerCompanyAr = fullName,
        //            UserName = fullName,
        //            RecordNo = TourLicRequestResponseVM.LicencesVM.RecordNo,
        //            CommercialLicNo = TourLicRequestResponseVM.LicencesVM.CommercialLicNo,

        //            MandoobId = TourLicRequestResponseVM.LicencesVM.MandoobId,
        //            AppId = TourLicRequestResponseVM.LicencesVM.ApplicantId

        //        };
        //        var apiSetting = _baseUrl + $"api/TourismFront/PostDataLicWhoConc";

        //        var response = await _helperUrlApi.PostDataToApi<PreApprovalRequestApiModel, PreApprovalRequestApiModel>(apiSetting, ModelToApi);



        //        return View(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        string fileName = controllerName + "_" + actionName + "_";

        //        string exId = ExceptionLog.LogException(ex, fileName);

        //        TempData["Ex"] = exId;
        //        throw;
        //    }

        //}
        #endregion
        #region إبداء الرأي للتجاره 
        [HttpGet]
        public async Task<ActionResult> ChooseMOCIRequest()
        {
            try
            {
                

                var apiSetting = _baseUrl + $"api/TourismFront/GetChooseWhichMOICLetter";



                var response = await _helperUrlApi.GetDataFromApi<MOICDropdownVM>(apiSetting);

                return View(response);

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
        [HttpGet]
        public async Task<ActionResult> GetLicencesListDropDown()
        {
            
            var civilId = HttpContext.Session.GetString("UserCivilId");
            

            var apiSetting = _baseUrl + $"api/TourismFront/GetLicencesDropDownPerUser?CivilId={civilId}";


            var result = await _helperUrlApi.GetDataFromApi<List<SelectListItem>>(apiSetting);
            return Json(result);


        }
        [HttpGet]
        public async Task<ActionResult> MOCIletterRequest(int ReqType, int ActivitiID, int? LicID)
        {
            

            var apiSetting = _baseUrl + $"api/TourismFront/GetLicenseDetailsMOIC?ReqType={ReqType}&ActivitiID={ActivitiID}";

            if (LicID.HasValue)
            {
                apiSetting += $"&LicId={LicID.Value}";
            }

            var response = await _helperUrlApi.GetDataFromApi<MoicRequestVM>(apiSetting);

            return View(response);

        }

        [HttpPost]
        public async Task<ActionResult> MOCIletterRequest(MoicRequestVM moicRequestVM)
        {
            try
            {
                var token = HttpContext.Session.GetString("UserToken");
                var userId = HttpContext.Session.GetString("UserId");
                var userName = HttpContext.Session.GetString("UserUserName");
                var civilId = HttpContext.Session.GetString("UserCivilId");
                var fullName = HttpContext.Session.GetString("UserFullName");
                var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
                Tuple<long, string> requestData = await _generalReqNo.GetRequestNo(moicRequestVM.ReqTypeId??0, moicRequestVM.ActivityCode);
                string reqNo = requestData.Item2;
                long SequenceNo = requestData.Item1;
                List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();

                //if (accountTypeId == "100")
                //{
                //    //moicRequestVM.LicencesVM.ApplicantId = userId;
                //    moicRequestVM.LicencesVM.ApplicantCivilId = civilId;
                //}
                //else if (accountTypeId == "300")
                //{
                //    moicRequestVM.LicencesVM.MandoobId = userId;
                //    moicRequestVM.LicencesVM.UserCivilId = civilId;
                //}


                foreach (var file in moicRequestVM.NamedFile)
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
                var ModelToApi = new PreApprovalRequestApiModel
                {
                    reqno = reqNo,
                    SequenceNo = SequenceNo,
                    accountTypeId = accountTypeId,  
                    LicNo = moicRequestVM.LicencesVM.LicNo,
                    LicId= moicRequestVM.LicId,
                    SessionCivilId=civilId,
                    SessionName=fullName,
                    LicencesName = moicRequestVM.LicencesVM.LicName,
                    ManId = moicRequestVM.LicencesVM.ManagerId,
                    ActivityTypeId = moicRequestVM.ActivityTypeId,
                    AppCivilId = moicRequestVM.LicencesVM.Applicant.CivilId,
                    UserCivilID = moicRequestVM.LicencesVM.UserCivilId,
                    ReqtypeId = moicRequestVM.ReqTypeId??0,
                    saveResponseVMs = filePath,
                    ManCivilId = moicRequestVM.LicencesVM.ManagerCivilId,
                    OwnerCompanyAr = moicRequestVM.LicencesVM.Company.OwnerCompanyAr,
                    CompanyCivilId=moicRequestVM.LicencesVM.Company.CompanyCivilId,
                    UserName = moicRequestVM.LicencesVM.Applicant.Name1,
                    RecordNo = moicRequestVM.LicencesVM.Company.RecordNo,
                    CommercialLicNo = moicRequestVM.LicencesVM.Company.CommercialLicNo,
                    MandoobId = moicRequestVM.LicencesVM.MandoobId,
                    CentralNoMOIc= moicRequestVM.LicencesVM.Company.CentralNoMoci,
                    
                    //AppId = moicRequestVM.LicencesVM.ApplicantId,
                    AaliNumber = moicRequestVM.AaliNumber,   
                    Area = moicRequestVM.Area,
                    BlockNo = moicRequestVM.BlockNo,
                    BuildingNo = moicRequestVM.BuildingNo, 
                    BuildingName = moicRequestVM.BuildingName,
                    Street = moicRequestVM.Street,
                    Governrate = moicRequestVM.Governrate,
                    UnitNo = moicRequestVM.UnitNo,
                    FloorNo = moicRequestVM.FloorNo,

                };
                var apiSetting = _baseUrl + $"api/TourismFront/PostDataLicMOIC";

                var response = await _helperUrlApi.PostDataToApi<PreApprovalRequestApiModel, ErrorMessage>(apiSetting, ModelToApi);
                if (response != null && response.Error == false)
                {
                    TempData["AlertTitle"] = "تأكـيد";
                    TempData["Message"] = "تم حفظ البيانات بنجاح";
                    TempData["AlertType"] = "Success";
                    return RedirectToAction("GetAllRequest", "Home");
                }
                else
                {
                    return View(response);
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
        #region طلب منتزهات وتنظيم وتأجير رحلات 

        [HttpGet]
        public async Task<ActionResult> TourLicActivitiesRequest(int id)
        {
            try
            {

                


                var apiSetting = _baseUrl + $"api/TourismFront/GetActivityWithService?id={id}";


                var response = await _helperUrlApi.GetDataFromApi<TourLicActivityVm>(apiSetting);



                return View(response);

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
        public async Task<ActionResult> TourLicActivitiesRequest(TourLicActivityVm TourLicRequestResponseVM)
        {

            try
            {
                var token = HttpContext.Session.GetString("UserToken");
                var userId = HttpContext.Session.GetString("UserId");
                var userName = HttpContext.Session.GetString("UserUserName");
                var civilId = HttpContext.Session.GetString("UserCivilId");
                var fullName = HttpContext.Session.GetString("UserFullName");
                var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
                Tuple<long, string> requestData = await _generalReqNo.GetRequestNo((int)RequestTypeEnum.Request, TourLicRequestResponseVM.ActivityType.ActivityCode);
                string reqNo = requestData.Item2;
                long SequenceNo = requestData.Item1;
                List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();

                //if (accountTypeId == "100")
                //{
                //   // TourLicRequestResponseVM.AppId = userId;
                //    TourLicRequestResponseVM.ApplicantCivilId = civilId;
                //}
                //else if (accountTypeId == "300")
                //{
                //    TourLicRequestResponseVM.MandoobId = userId;
                //    TourLicRequestResponseVM.UserCivilId = civilId;
                //}


                foreach (var file in TourLicRequestResponseVM.NamedFile)
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
                var ModelToApi = new PreApprovalRequestApiModel
                {
                    reqno = reqNo,
                    SequenceNo = SequenceNo,
                    accountTypeId = accountTypeId,
                    Amount = TourLicRequestResponseVM.LicencesInfo.FixedFees,
                    AaliNumber = TourLicRequestResponseVM.AaliNumber,
                    ActivityCode = TourLicRequestResponseVM.ActivityType.ActivityCode,
                    ActivityTypeId = TourLicRequestResponseVM.LicencesInfo.ActvityTypeId,
                    AppCivilId = TourLicRequestResponseVM.ApplicantCivilId,
                    AppName = TourLicRequestResponseVM.ApplicantUserName,
                    Area = TourLicRequestResponseVM.Area,
                    SessionCivilId = civilId,
                    SessionName = fullName,
                    
                    BlockNo = TourLicRequestResponseVM.BlockNo,
                    BuildingNo = TourLicRequestResponseVM.BuildingNo,
                    ManagerEmail = TourLicRequestResponseVM.ManagerEmail,
                    ManagerName = TourLicRequestResponseVM.ManagerName,
                    CompanyCivilId = TourLicRequestResponseVM.CompanyCivilId,
                    CompanyActivity = TourLicRequestResponseVM.CompanyActivity,
                    ManagerMobile = TourLicRequestResponseVM.ManagerMobile,
                    ReqtypeId = TourLicRequestResponseVM.LicencesInfo.ReqTypeId,
                    LicencesName = TourLicRequestResponseVM.LicName,
                    SalesManagerCivilId = TourLicRequestResponseVM.SalesManager.CivilId,
                    SalesManagerEmail = TourLicRequestResponseVM.SalesManager.Email,
                    SalesManagerName = TourLicRequestResponseVM.SalesManager.Name1,
                    SalesManagerPhone = TourLicRequestResponseVM.SalesManager.Phone,
                    MarketingManagerCivilId = TourLicRequestResponseVM.MarketingManager.CivilId,
                    MarketingManagerEmail = TourLicRequestResponseVM.MarketingManager.Email,
                    MarketingManagerName = TourLicRequestResponseVM.MarketingManager.Name1,
                    MarketingManagerPhone = TourLicRequestResponseVM.MarketingManager.Phone,
                    OperationManagerCivilId = TourLicRequestResponseVM.OperationsManager.CivilId,
                    OperationManagerEmail = TourLicRequestResponseVM.OperationsManager.Email,
                    OperationManagerName = TourLicRequestResponseVM.OperationsManager.Name1,
                    OperationManagerPhone = TourLicRequestResponseVM.OperationsManager.Phone,
                 
                    saveResponseVMs = filePath,

                    BuildingName = TourLicRequestResponseVM.BuildingName,
                    Street = TourLicRequestResponseVM.Street,
                    Governrate = TourLicRequestResponseVM.Governrate,
                    ManCivilId = TourLicRequestResponseVM.ManagerCivilId,
                    OwnerCompanyAr = TourLicRequestResponseVM.OwnerCompanyAr,
                    //UserName = fullName,
                    RecordNo = TourLicRequestResponseVM.RecordNo,
                    CommercialLicNo = TourLicRequestResponseVM.CommercialLicNo,
                    UnitNo = TourLicRequestResponseVM.UnitNo,
                    FloorNo = TourLicRequestResponseVM.FloorNo,
                    MandoobId = TourLicRequestResponseVM.MandoobId,
                    AppId = TourLicRequestResponseVM.AppId

                };
                var apiSetting = _baseUrl + $"api/TourismFront/PostDataLicActivity";

                var response = await _helperUrlApi.PostDataToApi<PreApprovalRequestApiModel, ErrorMessage>(apiSetting, ModelToApi);
                if (response != null && response.Error == false)
                {
                    TempData["AlertTitle"] = "تأكـيد";
                    TempData["Message"] = "تم حفظ البيانات بنجاح";
                    TempData["AlertType"] = "Success";
                    return RedirectToAction("GetAllRequest", "Home");
                }
                else
                {
                    return View(response);
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
        public async Task<ActionResult> UpdateLicActivitiescRequestDetails(TourLicActivityVm TourLicRequestResponseVM)
        {
            var token = HttpContext.Session.GetString("UserToken");
            var userId = HttpContext.Session.GetString("UserId");
            var userName = HttpContext.Session.GetString("UserUserName");
            var civilId = HttpContext.Session.GetString("UserCivilId");
            var fullName = HttpContext.Session.GetString("UserFullName");

            var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
            if (token != null)
            {
                try
                {
                    var apiSetting = _baseUrl + $"api/TourismFront/UpdateLicRequestDetails";
                    var ModelApi = new PreApprovalRequestApiModel
                    {
                        AaliNumber = TourLicRequestResponseVM.AaliNumber,
                        ActivityCode = TourLicRequestResponseVM.ActivityType.ActivityCode,
                        ActivityTypeId = TourLicRequestResponseVM.LicencesInfo.ActvityTypeId,
                        AppCivilId = TourLicRequestResponseVM.ApplicantCivilId,
                        AppName = TourLicRequestResponseVM.ApplicantUserName,
                        Area = TourLicRequestResponseVM.Area,
                        SessionCivilId = civilId,
                        SessionName = fullName,

                        BlockNo = TourLicRequestResponseVM.BlockNo,
                        BuildingNo = TourLicRequestResponseVM.BuildingNo,
                        ManagerEmail = TourLicRequestResponseVM.ManagerEmail,
                        ManagerName = TourLicRequestResponseVM.ManagerName,
                        CompanyCivilId = TourLicRequestResponseVM.CompanyCivilId,
                        CompanyActivity = TourLicRequestResponseVM.CompanyActivity,
                        ManagerMobile = TourLicRequestResponseVM.ManagerMobile,
                        ReqtypeId = TourLicRequestResponseVM.LicencesInfo.ReqTypeId,
                        LicencesName = TourLicRequestResponseVM.LicName,
                        SalesManagerCivilId = TourLicRequestResponseVM.SalesManager.CivilId,
                        SalesManagerEmail = TourLicRequestResponseVM.SalesManager.Email,
                        SalesManagerName = TourLicRequestResponseVM.SalesManager.Name1,
                        SalesManagerPhone = TourLicRequestResponseVM.SalesManager.Phone,
                        MarketingManagerCivilId = TourLicRequestResponseVM.MarketingManager.CivilId,
                        MarketingManagerEmail = TourLicRequestResponseVM.MarketingManager.Email,
                        MarketingManagerName = TourLicRequestResponseVM.MarketingManager.Name1,
                        MarketingManagerPhone = TourLicRequestResponseVM.MarketingManager.Phone,
                        OperationManagerCivilId = TourLicRequestResponseVM.OperationsManager.CivilId,
                        OperationManagerEmail = TourLicRequestResponseVM.OperationsManager.Email,
                        OperationManagerName = TourLicRequestResponseVM.OperationsManager.Name1,
                        OperationManagerPhone = TourLicRequestResponseVM.OperationsManager.Phone,

                      

                        BuildingName = TourLicRequestResponseVM.BuildingName,
                        Street = TourLicRequestResponseVM.Street,
                        Governrate = TourLicRequestResponseVM.Governrate,
                        ManCivilId = TourLicRequestResponseVM.ManagerCivilId,
                        OwnerCompanyAr = TourLicRequestResponseVM.OwnerCompanyAr,
                        //UserName = fullName,
                        RecordNo = TourLicRequestResponseVM.RecordNo,
                        CommercialLicNo = TourLicRequestResponseVM.CommercialLicNo,
                        UnitNo = TourLicRequestResponseVM.UnitNo,
                        FloorNo = TourLicRequestResponseVM.FloorNo,
                        MandoobId = TourLicRequestResponseVM.MandoobId,
                        AppId = TourLicRequestResponseVM.AppId
                    };
                    var response = await _helperUrlApi.PostDataToApi<PreApprovalRequestApiModel, ErrorMessage>(apiSetting, ModelApi);

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

            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #region SaveFile
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
        #endregion
        #region Update Old Attach
        public async Task<ActionResult> UpdateAttachment(NamedFile file, int attachId, string reqNo,long RequestId)
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
                FileSaveResponseVM=savedFile,
                RequestId= RequestId,
                AttachId= attachId
            };
            // Send new attachment info to API for DB update
            var apiUrl = _baseUrl + "api/TourismFront/Request/InsertUpdateAttachement";
            var result = await _helperUrlApi.PostDataToApi<UpdatedAttachVM, ErrorMessage>(apiUrl, SendData);

            if (result != null && !result.Error)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction("GetAllRequest", "Home");
            }
            
            TempData["ErrorMessage"] = result?.Message ?? "Unknown error.";
            return RedirectToAction("GetAllRequest", "Home");
        }
        // Add New Attachment dynamic

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
            var apiUrl = _baseUrl + "api/TourismFront/Request/InsertUpdateAttachement";
            var result = await _helperUrlApi.PostDataToApi<UpdatedAttachVM, ErrorMessage>(apiUrl, SendData);

            if (result != null && !result.Error)
            {
                return Ok(new { success = true, message = result.Message });
            }

            return BadRequest(result?.Message ?? "Unknown error.");
        }

        #endregion
        #endregion
        #region Renew
        //[HttpGet]
        //public async Task<ActionResult> RenewTourLicRequest(string id)
        //{
        //    try
        //    {
        //        int LicId = 0;
        //        if (int.TryParse(MyCrypto.Decode(id), out LicId))
        //        {


        //            var apiSetting = _baseUrl + $"api/TourismFront/GetLicenseDetailsForRenew?LicId={LicId}";

        //            ViewBag.PathAttachment = _file;

        //            var response = await _helperUrlApi.GetDataFromApi<RenewRequest>(apiSetting);

        //            return View(response);
        //        }
        //        else
        //        {
        //            return RedirectToAction("RequestsList");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        string fileName = controllerName + "_" + actionName + "_";

        //        string exId = ExceptionLog.LogException(ex, fileName);

        //        TempData["Ex"] = exId;
        //        throw;
        //    }
        //}

        //[HttpPost]
        //public async Task<ActionResult> RenewTourLicRequest(RenewRequest renewRequest)
        //{
        //    try
        //    {
        //        var token = HttpContext.Session.GetString("UserToken");
        //        var userId = HttpContext.Session.GetString("UserId");
        //        var userName = HttpContext.Session.GetString("UserUserName");
        //        var civilId = HttpContext.Session.GetString("UserCivilId");
        //        var fullName = HttpContext.Session.GetString("UserFullName");
        //        var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
        //        Tuple<long, string> requestData = await _generalReqNo.GetRequestNo((int)RequestTypeEnum.Renew, renewRequest.LicencesVM.ActivityTypesLookup.ActivityCode);
        //        string reqNo = requestData.Item2;
        //        long SequenceNo = requestData.Item1;
        //        List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();

        //        if (accountTypeId == "100")
        //        {
        //            renewRequest.LicencesVM.ApplicantId = userId;
        //            renewRequest.LicencesVM.ApplicantCivilId = civilId;
        //        }
        //        else if (accountTypeId == "300")
        //        {
        //            renewRequest.LicencesVM.MandoobId = userId;
        //            renewRequest.LicencesVM.UserCivilId = civilId;
        //        }


        //        foreach (var file in renewRequest.NamedFile)
        //        {
        //            if (file.File != null)
        //            {
        //                // Save each file to disk
        //                var savedFilePath = await SaveFileToDiskAsync(file.File, file.FieldName, _file, reqNo, file.IsRequired, file.FieldName, file.LabelName);

        //                filePath.Add(savedFilePath);
        //                // Perform any additional logic with the saved file path, if needed
        //                Console.WriteLine($"File saved at: {savedFilePath}");

        //            }
        //        }

        //        var ModelToApi = new PreApprovalRequestApiModel
        //        {
        //            reqno = reqNo,
        //            SequenceNo = SequenceNo,
        //            accountTypeId = accountTypeId,
        //            //Amount = TourLicRequestResponseVM.LicencesInfoVM.FixedFees,                
        //            ActivityCode = renewRequest.LicencesVM.ActivityTypesLookup.ActivityCode,
        //            ActivityTypeId = renewRequest.LicencesVM.ActiivityTypeId,
        //            AppCivilId = renewRequest.LicencesVM.ApplicantCivilId,
        //            UserCivilID = renewRequest.LicencesVM.UserCivilId,
        //            SessionCivilId = civilId,
        //            SessionName = fullName,
        //            CompanyId = renewRequest.LicencesVM.CompanyId,
        //            ManId=renewRequest.LicencesVM.ManagerId,
        //            BuildingId=renewRequest.LicencesVM.BuildingId,

        //            CompanyActivity = renewRequest.LicencesVM.ActivityTypeName,
        //            LicencesName = renewRequest.LicencesVM.LicName,
        //            PreApprove = renewRequest.LicencesVM.PreApprovalNo,
        //            saveResponseVMs = filePath,
        //            LicId = renewRequest.LicencesVM.LicId,
        //            LicNo=renewRequest.LicencesVM.LicNo,
        //            ManCivilId = renewRequest.LicencesVM.ManagerCivilId,
        //            OwnerCompanyAr = renewRequest.LicencesVM.Company.OwnerCompanyAr,
        //            UserName = fullName,
        //            CommercialLicNo = renewRequest.LicencesVM.CommercialLicNo,         
        //            MandoobId = renewRequest.LicencesVM.MandoobId,
        //            AppId = renewRequest.LicencesVM.ApplicantId

        //        };
        //        var apiSetting = _baseUrl + $"api/TourismFront/PostDataRenewRequest";

        //        var response = await _helperUrlApi.PostDataToApi<PreApprovalRequestApiModel, PreApprovalRequestApiModel>(apiSetting, ModelToApi);



        //        return RedirectToAction("LicencesList");
        //    }
        //    catch (Exception ex)
        //    {
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        string fileName = controllerName + "_" + actionName + "_";

        //        string exId = ExceptionLog.LogException(ex, fileName);

        //        TempData["Ex"] = exId;
        //        throw;
        //    }
        //}

        #endregion
        #region EndLicences
        //[HttpGet]
        //public async Task<ActionResult> EndLicencesTourLicRequest(string id)
        //{
        //    try
        //    {
        //        int LicId = 0;
        //        if (int.TryParse(MyCrypto.Decode(id), out LicId))
        //        {


        //            var apiSetting = _baseUrl + $"api/TourismFront/GetLicenseDetailsForEndLicences?LicId={LicId}";

        //            ViewBag.PathAttachment = _file;

        //            var response = await _helperUrlApi.GetDataFromApi<EndLicencesRequest>(apiSetting);

        //            return View(response);
        //        }
        //        else
        //        {
        //            return RedirectToAction("RequestsList");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        string fileName = controllerName + "_" + actionName + "_";

        //        string exId = ExceptionLog.LogException(ex, fileName);

        //        TempData["Ex"] = exId;
        //        throw;
        //    }
        //}

        //[HttpPost]
        //public async Task<ActionResult> EndLicencesTourLicRequest(EndLicencesRequest renewRequest)
        //{
        //    try
        //    {
        //        var token = HttpContext.Session.GetString("UserToken");
        //        var userId = HttpContext.Session.GetString("UserId");
        //        var userName = HttpContext.Session.GetString("UserUserName");
        //        var civilId = HttpContext.Session.GetString("UserCivilId");
        //        var fullName = HttpContext.Session.GetString("UserFullName");
        //        var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
        //        Tuple<long, string> requestData = await _generalReqNo.GetRequestNo((int)RequestTypeEnum.Renew, renewRequest.LicencesVM.ActivityTypesLookup.ActivityCode);
        //        string reqNo = requestData.Item2;
        //        long SequenceNo = requestData.Item1;
        //        List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();

        //        if (accountTypeId == "100")
        //        {
        //            renewRequest.LicencesVM.ApplicantId = userId;
        //            renewRequest.LicencesVM.ApplicantCivilId = civilId;
        //        }
        //        else if (accountTypeId == "300")
        //        {
        //            renewRequest.LicencesVM.MandoobId = userId;
        //            renewRequest.LicencesVM.UserCivilId = civilId;
        //        }


        //        foreach (var file in renewRequest.NamedFile)
        //        {
        //            if (file.File != null)
        //            {
        //                // Save each file to disk
        //                var savedFilePath = await SaveFileToDiskAsync(file.File, file.FieldName, _file, reqNo, file.IsRequired, file.FieldName, file.LabelName);

        //                filePath.Add(savedFilePath);
        //                // Perform any additional logic with the saved file path, if needed
        //                Console.WriteLine($"File saved at: {savedFilePath}");

        //            }
        //        }

        //        var ModelToApi = new PreApprovalRequestApiModel
        //        {
        //            reqno = reqNo,
        //            SequenceNo = SequenceNo,
        //            accountTypeId = accountTypeId,
        //            //Amount = TourLicRequestResponseVM.LicencesInfoVM.FixedFees,                
        //            ActivityCode = renewRequest.LicencesVM.ActivityTypesLookup.ActivityCode,
        //            ActivityTypeId = renewRequest.LicencesVM.ActiivityTypeId,
        //            AppCivilId = renewRequest.LicencesVM.ApplicantCivilId,
        //            UserCivilID = renewRequest.LicencesVM.UserCivilId,
        //            SessionCivilId = civilId,
        //            SessionName = fullName,
        //            CompanyId = renewRequest.LicencesVM.CompanyId,
        //            ManId = renewRequest.LicencesVM.ManagerId,
        //            BuildingId = renewRequest.LicencesVM.BuildingId,
        //            EndingReasonId=renewRequest.EndingReasonId,
        //            CompanyActivity = renewRequest.LicencesVM.ActivityTypeName,
        //            LicencesName = renewRequest.LicencesVM.LicName,
        //            PreApprove = renewRequest.LicencesVM.PreApprovalNo,
        //            saveResponseVMs = filePath,
        //            LicId = renewRequest.LicencesVM.LicId,
        //            LicNo = renewRequest.LicencesVM.LicNo,
        //            ManCivilId = renewRequest.LicencesVM.ManagerCivilId,
        //            OwnerCompanyAr = renewRequest.LicencesVM.Company.OwnerCompanyAr,
        //            UserName = fullName,
        //            CommercialLicNo = renewRequest.LicencesVM.CommercialLicNo,
        //            MandoobId = renewRequest.LicencesVM.MandoobId,
        //            AppId = renewRequest.LicencesVM.ApplicantId

        //        };
        //        var apiSetting = _baseUrl + $"api/TourismFront/PostDataEndLicencesRequest";

        //        var response = await _helperUrlApi.PostDataToApi<PreApprovalRequestApiModel, PreApprovalRequestApiModel>(apiSetting, ModelToApi);



        //        return RedirectToAction("LicencesList");
        //    }
        //    catch (Exception ex)
        //    {
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        string fileName = controllerName + "_" + actionName + "_";

        //        string exId = ExceptionLog.LogException(ex, fileName);

        //        TempData["Ex"] = exId;
        //        throw;
        //    }
        //}

        #endregion

        #region التصنيف وإعادة التصنيف
        //[HttpGet]
        //public async Task<ActionResult> ClassTourLicRequest(string id,int? requestTypeId)
        //{
        //    try
        //    {
        //        int LicId = 0;
        //        if (int.TryParse(MyCrypto.Decode(id), out LicId))
        //        {


        //            var apiSetting = _baseUrl + $"api/TourismFront/GetClassificationForm?LicId={LicId}";

        //            ViewBag.PathAttachment = _file;
        //            ViewBag.RequestTypeId = requestTypeId;
        //            var response = await _helperUrlApi.GetDataFromApi<ClassificationFormVM>(apiSetting);

        //            return View(response);
        //        }
        //        else
        //        {
        //            return RedirectToAction("RequestsList");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        string fileName = controllerName + "_" + actionName + "_";

        //        string exId = ExceptionLog.LogException(ex, fileName);

        //        TempData["Ex"] = exId;
        //        throw;
        //    }
        //}

        //[HttpPost]
        //public async Task<ActionResult> ClassTourLicRequest(ClassificationFormVM renewRequest)
        //{
        //    try
        //    {
        //        var token = HttpContext.Session.GetString("UserToken");
        //        var userId = HttpContext.Session.GetString("UserId");
        //        var userName = HttpContext.Session.GetString("UserUserName");
        //        var civilId = HttpContext.Session.GetString("UserCivilId");
        //        var fullName = HttpContext.Session.GetString("UserFullName");
        //        var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
        //        int reqType = renewRequest.RequestTypeId;

        //        Tuple<long, string> requestData = await _generalReqNo.GetRequestNo(reqType, renewRequest.LicencesVM.ActivityTypesLookup.ActivityCode);
        //        string reqNo = requestData.Item2;
        //        long SequenceNo = requestData.Item1;
        //        List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();

        //        if (accountTypeId == "100")
        //        {
        //            renewRequest.LicencesVM.ApplicantId = userId;
        //            renewRequest.LicencesVM.ApplicantCivilId = civilId;
        //        }
        //        else if (accountTypeId == "300")
        //        {
        //            renewRequest.LicencesVM.MandoobId = userId;
        //            renewRequest.LicencesVM.UserCivilId = civilId;
        //        }


        //        foreach (var file in renewRequest.NamedFile)
        //        {
        //            if (file.File != null)
        //            {
        //                // Save each file to disk
        //                var savedFilePath = await SaveFileToDiskAsync(file.File, file.FieldName, _file, reqNo, file.IsRequired, file.FieldName, file.LabelName);

        //                filePath.Add(savedFilePath);
        //                // Perform any additional logic with the saved file path, if needed
        //                Console.WriteLine($"File saved at: {savedFilePath}");

        //            }
        //        }

        //        var ModelToApi = new PreApprovalRequestApiModel
        //        {
        //            reqno = reqNo,
        //            SequenceNo = SequenceNo,
        //            accountTypeId = accountTypeId,
        //            ReqtypeId=reqType,
        //            //Amount = TourLicRequestResponseVM.LicencesInfoVM.FixedFees,                
        //            ActivityCode = renewRequest.LicencesVM.ActivityTypesLookup.ActivityCode,
        //            ActivityTypeId = renewRequest.LicencesVM.ActiivityTypeId,
        //            AppCivilId = renewRequest.LicencesVM.ApplicantCivilId,
        //            UserCivilID = renewRequest.LicencesVM.UserCivilId,
        //            SessionCivilId = civilId,
        //            SessionName = fullName,
        //            CompanyId = renewRequest.LicencesVM.CompanyId,
        //            ManId = renewRequest.LicencesVM.ManagerId,
        //            BuildingId = renewRequest.LicencesVM.BuildingId,
        //            EvaluationSelections= renewRequest.EvaluationSelections,
        //            CompanyActivity = renewRequest.LicencesVM.ActivityTypeName,
        //            LicencesName = renewRequest.LicencesVM.LicName,
        //            PreApprove = renewRequest.LicencesVM.PreApprovalNo,
        //            saveResponseVMs = filePath,
        //            LicId = renewRequest.LicencesVM.LicId,
        //            LicNo = renewRequest.LicencesVM.LicNo,
        //            ManCivilId = renewRequest.LicencesVM.ManagerCivilId,
        //            OwnerCompanyAr = renewRequest.LicencesVM.Company.OwnerCompanyAr,
        //            UserName = fullName,
        //            CommercialLicNo = renewRequest.LicencesVM.CommercialLicNo,
        //            MandoobId = renewRequest.LicencesVM.MandoobId,
        //            AppId = renewRequest.LicencesVM.ApplicantId,
        //            ClassificationId=renewRequest.ClassificationId,
        //            PreApproveId=renewRequest.LicencesVM.PreApprovalId
        //        };
        //        var apiSetting = _baseUrl + $"api/TourismFront/PostDataClassificationRequest";

        //        var response = await _helperUrlApi.PostDataToApi<PreApprovalRequestApiModel, PreApprovalRequestApiModel>(apiSetting, ModelToApi);



        //        return RedirectToAction("LicencesList");
        //    }
        //    catch (Exception ex)
        //    {
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        string fileName = controllerName + "_" + actionName + "_";

        //        string exId = ExceptionLog.LogException(ex, fileName);

        //        TempData["Ex"] = exId;
        //        throw;
        //    }
        //}
        #endregion
        #region التنازل
        //[HttpGet]
        //public async Task<ActionResult> RenouncementLicRequest(string id)
        //{
        //    try
        //    {
        //        int LicId = 0;
        //        if (int.TryParse(MyCrypto.Decode(id), out LicId))
        //        {


        //            var apiSetting = _baseUrl + $"api/TourismFront/GetLicenseDetailsForRenouncement?LicId={LicId}";

        //            ViewBag.PathAttachment = _file;

        //            var response = await _helperUrlApi.GetDataFromApi<EndLicencesRequest>(apiSetting);

        //            return View(response);
        //        }
        //        else
        //        {
        //            return RedirectToAction("RequestsList");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        string fileName = controllerName + "_" + actionName + "_";

        //        string exId = ExceptionLog.LogException(ex, fileName);

        //        TempData["Ex"] = exId;
        //        throw;
        //    }
        //}

        //[HttpPost]
        //public async Task<ActionResult> RenouncementLicRequest(RenouncementRequest renewRequest)
        //{
        //    try
        //    {
        //        var token = HttpContext.Session.GetString("UserToken");
        //        var userId = HttpContext.Session.GetString("UserId");
        //        var userName = HttpContext.Session.GetString("UserUserName");
        //        var civilId = HttpContext.Session.GetString("UserCivilId");
        //        var fullName = HttpContext.Session.GetString("UserFullName");
        //        var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
        //        Tuple<long, string> requestData = await _generalReqNo.GetRequestNo((int)RequestTypeEnum.Renew, renewRequest.LicencesVM.ActivityTypesLookup.ActivityCode);
        //        string reqNo = requestData.Item2;
        //        long SequenceNo = requestData.Item1;
        //        List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();

        //        if (accountTypeId == "100")
        //        {
        //            renewRequest.LicencesVM.ApplicantId = userId;
        //            renewRequest.LicencesVM.ApplicantCivilId = civilId;
        //        }
        //        else if (accountTypeId == "300")
        //        {
        //            renewRequest.LicencesVM.MandoobId = userId;
        //            renewRequest.LicencesVM.UserCivilId = civilId;
        //        }


        //        foreach (var file in renewRequest.NamedFile)
        //        {
        //            if (file.File != null)
        //            {
        //                // Save each file to disk
        //                var savedFilePath = await SaveFileToDiskAsync(file.File, file.FieldName, _file, reqNo, file.IsRequired, file.FieldName, file.LabelName);

        //                filePath.Add(savedFilePath);
        //                // Perform any additional logic with the saved file path, if needed
        //                Console.WriteLine($"File saved at: {savedFilePath}");

        //            }
        //        }

        //        var ModelToApi = new PreApprovalRequestApiModel
        //        {
        //            reqno = reqNo,
        //            SequenceNo = SequenceNo,
        //            accountTypeId = accountTypeId,
        //            //Amount = TourLicRequestResponseVM.LicencesInfoVM.FixedFees,                
        //            ActivityCode = renewRequest.LicencesVM.ActivityTypesLookup.ActivityCode,
        //            ActivityTypeId = renewRequest.LicencesVM.ActiivityTypeId,
        //            AppCivilId = renewRequest.LicencesVM.ApplicantCivilId,
        //            UserCivilID = renewRequest.LicencesVM.UserCivilId,
        //            SessionCivilId = civilId,
        //            SessionName = fullName,
        //            CompanyId = renewRequest.LicencesVM.CompanyId,
        //            ManId = renewRequest.LicencesVM.ManagerId,
        //            BuildingId = renewRequest.LicencesVM.BuildingId,
        //             NewMobile=renewRequest.NewMobile,
        //             NewCivilId=renewRequest.NewCivilId,
        //             NewEmail=renewRequest.NewEmail,
        //             NewUserName=renewRequest.NewUserName,
        //            CompanyActivity = renewRequest.LicencesVM.ActivityTypeName,
        //            LicencesName = renewRequest.LicencesVM.LicName,
        //            PreApprove = renewRequest.LicencesVM.PreApprovalNo,
        //            saveResponseVMs = filePath,
        //            LicId = renewRequest.LicencesVM.LicId,
        //            LicNo = renewRequest.LicencesVM.LicNo,
        //            ManCivilId = renewRequest.LicencesVM.ManagerCivilId,
        //            OwnerCompanyAr = renewRequest.LicencesVM.Company.OwnerCompanyAr,
        //            UserName = fullName,
        //            CommercialLicNo = renewRequest.LicencesVM.CommercialLicNo,
        //            MandoobId = renewRequest.LicencesVM.MandoobId,
        //            AppId = renewRequest.LicencesVM.ApplicantId

        //        };
        //        var apiSetting = _baseUrl + $"api/TourismFront/PostDataRenouncementRequest";

        //        var response = await _helperUrlApi.PostDataToApi<PreApprovalRequestApiModel, PreApprovalRequestApiModel>(apiSetting, ModelToApi);



        //        return RedirectToAction("LicencesList");
        //    }
        //    catch (Exception ex)
        //    {
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        string fileName = controllerName + "_" + actionName + "_";

        //        string exId = ExceptionLog.LogException(ex, fileName);

        //        TempData["Ex"] = exId;
        //        throw;
        //    }
        //}

        #endregion
        //#region بدل فاقد
        //[HttpGet]
        //public async Task<ActionResult> EndLicencesTourLicRequest(string id)
        //{
        //    try
        //    {
        //        int LicId = 0;
        //        if (int.TryParse(MyCrypto.Decode(id), out LicId))
        //        {
        //            var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        //            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        //            var civilId = User.FindFirst("CivilId")?.Value;  // Custom claim
        //            var fullName = User.FindFirst("FullName")?.Value;
        //            var accountTypeId = User.FindFirst("AccouuntTypeId")?.Value;

        //            var apiSetting = _baseUrl + $"api/TourismFront/GetLicenseDetails?id={LicId}";

        //            ViewBag.PathAttachment = _file;

        //            var response = await _helperUrlApi.GetDataFromApi<RenewRequest>(apiSetting);

        //            return View(response);
        //        }
        //        else
        //        {
        //            return RedirectToAction("RequestsList");
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        string fileName = controllerName + "_" + actionName + "_";

        //        string exId = ExceptionLog.LogException(ex, fileName);

        //        TempData["Ex"] = exId;
        //        throw;
        //    }
        //}

        //[HttpPost]
        //public async Task<ActionResult> EndLicencesTourLicRequest(RenewRequest renewRequest)
        //{
        //    try
        //    {
        //        var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        //        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        //        var civilId = User.FindFirst("CivilId")?.Value;  // Custom claim
        //        var fullName = User.FindFirst("FullName")?.Value;
        //        var accountTypeId = User.FindFirst("AccouuntTypeId")?.Value;
        //        Tuple<long, string> requestData = await _generalReqNo.GetReqNoTourLicRenew((int)RequestTypeEnum.Request, renewRequest.LicencesVM.ActivityTypesLookup.ActivityCode);
        //        string reqNo = requestData.Item2;
        //        long SequenceNo = requestData.Item1;
        //        List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();

        //        if (accountTypeId == "100")
        //        {
        //            renewRequest.LicencesVM.ApplicantId = userId;
        //            renewRequest.LicencesVM.ApplicantCivilId = civilId;
        //        }
        //        else if (accountTypeId == "300")
        //        {
        //            renewRequest.LicencesVM.MandoobId = userId;
        //            renewRequest.LicencesVM.UserCivilId = civilId;
        //        }


        //        foreach (var file in renewRequest.NamedFile)
        //        {
        //            if (file.File != null)
        //            {
        //                // Save each file to disk
        //                var savedFilePath = await SaveFileToDiskAsync(file.File, file.FieldName, _file, reqNo, file.IsRequired, file.FieldName, file.LabelName);

        //                filePath.Add(savedFilePath);
        //                // Perform any additional logic with the saved file path, if needed
        //                Console.WriteLine($"File saved at: {savedFilePath}");

        //            }
        //        }

        //        var ModelToApi = new PreApprovalRequestApiModel
        //        {
        //            reqno = reqNo,
        //            SequenceNo = SequenceNo,
        //            accountTypeId = accountTypeId,
        //            //Amount = TourLicRequestResponseVM.LicencesInfoVM.FixedFees,                
        //            ActivityCode = renewRequest.LicencesVM.ActivityTypesLookup.ActivityCode,
        //            ActivityTypeId = renewRequest.LicencesVM.ActiivityTypeId,
        //            AppCivilId = renewRequest.LicencesVM.ApplicantCivilId,
        //            UserCivilID = renewRequest.LicencesVM.UserCivilId,
        //            SessionCivilId = civilId,
        //            SessionName = fullName,
        //            CompanyId = renewRequest.LicencesVM.CompanyId ?? 0,
        //            CompanyActivity = renewRequest.LicencesVM.ActivityTypeName,
        //            LicencesName = renewRequest.LicencesVM.LicName,
        //            PreApprove = renewRequest.LicencesVM.PreApprovalNo,
        //            saveResponseVMs = filePath,
        //            LicId = renewRequest.LicencesVM.LicId,
        //            ManCivilId = renewRequest.LicencesVM.ManagerCivilId,
        //            OwnerCompanyAr = renewRequest.LicencesVM.Company.OwnerCompanyAr,
        //            UserName = fullName,
        //            CommercialLicNo = renewRequest.LicencesVM.CommercialLicNo,
        //            MandoobId = renewRequest.LicencesVM.MandoobId,
        //            AppId = renewRequest.LicencesVM.ApplicantId

        //        };
        //        var apiSetting = _baseUrl + $"api/TourismFront/PostDataRenewRequest";

        //        var response = await _helperUrlApi.PostDataToApi<PreApprovalRequestApiModel, PreApprovalRequestApiModel>(apiSetting, ModelToApi);



        //        return View(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //        string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //        string fileName = controllerName + "_" + actionName + "_";

        //        string exId = ExceptionLog.LogException(ex, fileName);

        //        TempData["Ex"] = exId;
        //        throw;
        //    }
        //}
        //#endregion
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


        #region ViewRequestsWithDetails
        public async Task<ActionResult> RequestsList()
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
                    var apiSetting = _baseUrl + $"api/TourismFront/GetAllRequestsForUser/{civilId}";


                var response = await _helperUrlApi.GetDataFromApi<List<RequestVM>>(apiSetting);

                return View(response);
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
        public async Task<ActionResult> PreApprovalDetails(string id)
        {
            try
            {

                int ReqId = 0;
                if (int.TryParse(MyCrypto.Decode(id), out ReqId))
                {

                    var token = HttpContext.Session.GetString("UserToken");
                    var userId = HttpContext.Session.GetString("UserId");
                    var userName = HttpContext.Session.GetString("UserUserName");
                    var civilId = HttpContext.Session.GetString("UserCivilId");
                    var fullName = HttpContext.Session.GetString("UserFullName");
                    var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");
                    var apiSetting = _baseUrl + $"api/TourismFront/GetPreApprovalDetails/{ReqId}";

                    ViewBag.PathAttachment = _file;

                    var response = await _helperUrlApi.GetDataFromApi<RequestFrontVM>(apiSetting);

                    return View(response);
                }
                else
                {
                    return RedirectToAction("LicenseList");
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
        public async Task<ActionResult> RequestDetails(string id)
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
                    return RedirectToAction("GetAllRequest", "Home");
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
        #region LicencesListWithDetails
        public async Task<ActionResult> LicencesList()
        
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
                    var apiSetting = _baseUrl + $"api/TourismFront/GetAllLicencesForUser/{civilId}";


                    var response = await _helperUrlApi.GetDataFromApi<LicenceDetailsForUserVM>(apiSetting);

                    return View(response);
                }
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



        }
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
                    var apiSetting = _baseUrl + $"api/TourismFront/GetLicenseDetails?id={LicId}";

                    ViewBag.PathAttachment = _file;

                    var response = await _helperUrlApi.GetDataFromApi<LicenceDetailsVM>(apiSetting);

                    return View(response);
                }
                else
                {
                    return RedirectToAction("GetAllRequest", "Home");
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


        public async Task<ActionResult> PreApprovalLicDetails(string id)
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
                    var apiSetting = _baseUrl + $"api/TourismFront/GetPreApprovalLicDetails?id={LicId}";

                    ViewBag.PathAttachment = _file;

                    var response = await _helperUrlApi.GetDataFromApi<LicenceDetailsVM>(apiSetting);

                    return View(response);
                }
                else
                {
                    return RedirectToAction("LicenseList");
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
        public async Task<ActionResult> ServiceDetails(string id,int requestTypeId, List<int>? selectedTransactionTypeIds)
        {

            int LicId = 0;
            if (int.TryParse(MyCrypto.Decode(id), out LicId))
            {
                var apiSetting = _baseUrl + $"api/TourismFront/GetServiceDetails?LicId={LicId}&ReqType={requestTypeId}";

          

            var response = await _helperUrlApi.GetDataFromApi<LicencesInfoVM>(apiSetting);
                response.SelectedTransactionTypeIds = selectedTransactionTypeIds;

            return View(response);
            }
            else
            {
                return RedirectToAction("GetAllRequest", "Home");
            }
        }
        [HttpGet]
        public async Task<ActionResult> HandleRequest(string id, int? requestTypeId, List<int>? selectedTransactionTypeIds)
        {
            int licId = int.TryParse(MyCrypto.Decode(id), out int decoded) ? decoded : 0;
            string queryParams = "";
            if (selectedTransactionTypeIds != null)
            {
               queryParams = string.Join("&", selectedTransactionTypeIds.Select(id => $"selectedTransactionTypeIds={id}"));
            }
            var requestType = (RequestTypeEnum)requestTypeId;
            var apiSetting = requestType switch
            {
                RequestTypeEnum.Renew => $"api/TourismFront/GetLicenseDetailsForRenew?LicId={licId}",
                RequestTypeEnum.EndLicences => $"api/TourismFront/GetLicenseDetailsForEndLicences?LicId={licId}",
                RequestTypeEnum.Renouncement => $"api/TourismFront/GetLicenseDetailsForRenouncement?LicId={licId}",
                RequestTypeEnum.WhoConc=> $"api/TourismFront/GetLicenceDetailsForWhoConc?LicId={licId}",
                RequestTypeEnum.ReplacementOfLost => $"api/TourismFront/GetLicenceDetailsForReplacementOfLost?LicId={licId}",

                RequestTypeEnum.ChangeData=> $"api/TourismFront/GetLicenceDetailsForChangeData?LicId={licId}&&{queryParams}",
                RequestTypeEnum.Classification or RequestTypeEnum.ReClassification => $"api/TourismFront/GetClassificationForm?LicId={licId}",
                _ => null
            };

            if (string.IsNullOrEmpty(apiSetting))
                return RedirectToAction("GetAllRequest", "Home");

            var result = await _helperUrlApi.GetDataFromApi<RequestBaseVM>(apiSetting); // You may cast dynamically

            ViewBag.RequestTypeId = requestTypeId;
            result.ReqtypeId = (int)requestType;
            result.SelectedTransactionTypeIds = selectedTransactionTypeIds;
            ViewBag.PathAttachment = _file;

            

            return requestType switch
            {
                RequestTypeEnum.Classification or RequestTypeEnum.ReClassification => View("ClassTourLicRequest", result),
                RequestTypeEnum.Renew => View("RenewTourLicRequest", result),
                RequestTypeEnum.Renouncement => View("RenouncementLicRequest", result),
                RequestTypeEnum.EndLicences => View("EndLicencesTourLicRequest", result),
                RequestTypeEnum.WhoConc => View("WhoConcTourLicRequest", result),
                RequestTypeEnum.ReplacementOfLost => View("ReplacementOfLostLicRequest", result),
                RequestTypeEnum.ChangeData=>View("ChangedataLicRequest",result),
                _ => View("DefaultRequestView", result)
            };
        }

        [HttpPost]
        public async Task<IActionResult> HandleRequest(RequestBaseVM model)
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

            //if (accountTypeId == AccountTypeEnum.Kuwaiti.ToString())
            //{
            //   // model.AppId = userId;
            //    model.AppCivilId = civilId;
            //}
            //else if (accountTypeId == AccountTypeEnum.User.ToString())
            //{
            //  //  model.MandoobId = userId;
            //    model.UserCivilID = civilId;
            //}

            // File processing
            var savedFiles = new List<FileSaveResponseVM>();
            foreach (var file in model.NamedFile)
            {
                if (file.File != null)
                    savedFiles.Add(await SaveFileToDiskAsync(file.File, file.FieldName, _file, model.reqno, file.IsRequired, file.FieldName, file.LabelName));
            }

            // Generate request number
            var reqInfo = await _generalReqNo.GetRequestNo(model.ReqtypeId, model.LicencesVM.ActivityTypesLookup.ActivityCode);
            model.reqno = reqInfo.Item2;
            model.SequenceNo = reqInfo.Item1;

            var apiRequest = BuildRequestApiModel(model, model.ReqtypeId, model.reqno, model.SequenceNo, savedFiles, civilId, fullName);

           
            string apiRoute = model.ReqtypeId switch
            {
                (int)RequestTypeEnum.Renew => "PostDataRenewRequest",
                (int)RequestTypeEnum.Classification => "PostDataClassificationRequest",
                (int)RequestTypeEnum.ReplacementOfLost => "PostDataReplacementOfLostRequest",
                (int)RequestTypeEnum.ReClassification => "PostDataClassificationRequest",
                (int)RequestTypeEnum.Renouncement => "PostDataRenouncementRequest",
                (int)RequestTypeEnum.EndLicences => "PostDataEndLicencesRequest",
                (int)RequestTypeEnum.WhoConc => "PostDataWhoConcRequest",
                (int)RequestTypeEnum.ChangeData=> "PostDataChangeDataRequest",
                _ => null
            };

            if (apiRoute == null)
                return BadRequest("Invalid request type.");

            var apiEndpoint = $"{_baseUrl}api/TourismFront/{apiRoute}";

            // ✅ Use the transformed model here
            var response=await _helperUrlApi.PostDataToApi<PreApprovalRequestApiModel, PreApprovalRequestApiModel>(apiEndpoint, apiRequest);

            if (response != null)
            {
                return RedirectToAction("GetAllRequest", "Home");
            }
            else
            {
                return RedirectToAction("LicencesList");
            }
        }
        private PreApprovalRequestApiModel BuildRequestApiModel(RequestBaseVM model, int requestTypeId, string reqNo, long sequenceNo, List<FileSaveResponseVM> files, string civilId, string fullName)
        {
            int? oldAddressId;

            var apiModel = new PreApprovalRequestApiModel
            {
                reqno = reqNo,
                SequenceNo = sequenceNo,
                accountTypeId = model.accountTypeId,
                LicencesName=model.LicencesVM.LicName,
                ActivityCode = model.LicencesVM.ActivityTypesLookup.ActivityCode,
                ActivityTypeId = model.LicencesVM.ActiivityTypeId,
                AppCivilId = model.LicencesVM.Applicant.CivilId,
                AppId=model.LicencesVM.ApplicantId,
                UserCivilID = model.UserCivilID,
                SessionCivilId = civilId,
                SessionName = fullName,
                CompanyId = model.LicencesVM.CompanyId,
                ManId = model.LicencesVM.ManagerId,
                BuildingId = model.LicencesVM.BuildingId,
                saveResponseVMs = files,
                LicId = model.LicencesVM.LicId,
                LicNo = model.LicencesVM.LicNo,
                ManCivilId = model.LicencesVM.ManagerCivilId,
                OwnerCompanyAr = model.LicencesVM.Company.OwnerCompanyAr,
                DirCompanyAr = model.LicencesVM.Company.DirCompanyAr,
                Amount=model.LicencesInfo.FixedFees,
               // UserName = fullName,
                CommercialLicNo = model.LicencesVM.CommercialLicNo,
                MandoobId = model.MandoobId,
                //AppId = model.AppId,
                ReqtypeId = requestTypeId,
                SalesManagerCivilId=model.LicencesVM.SalesManagerCivilId,
                SalesManagerId=model.LicencesVM.SalesManagerId,
                MarketingManagerCivilId=model.LicencesVM.MarketingManagerCivilId,
                MarketingManagerId = model.LicencesVM.MarketingManagerId,
                OperationManagerId=model.LicencesVM.OperationsManagerId,
                OperationManagerCivilId=model.LicencesVM.OperationsManagerCivilId,
                IssueDate=model.LicencesVM.IssueDate,
                ExpireDate=model.LicencesVM.ExpireDate,


            };

            // Add conditional properties
            switch ((RequestTypeEnum)requestTypeId)
            {
                case RequestTypeEnum.Renouncement:
                    apiModel.NewCivilId = model.NewCivilId;
                    apiModel.OldCivilId = model.LicencesVM.Applicant.CivilId;
                    apiModel.NewMobile = model.NewMobile;
                    apiModel.OldMobile = model.LicencesVM.Applicant.Phone;
                    apiModel.NewEmail = model.NewEmail;
                    apiModel.OldEmail = model.LicencesVM.Applicant.Email;
                    apiModel.NewUserName = model.NewUserName;
                    apiModel.OldUserName = model.LicencesVM.Applicant.Name1;
                    break;
                case RequestTypeEnum.EndLicences:
                    apiModel.EndingReasonId = model.EndingReasonId;
                    break;
                case RequestTypeEnum.Classification:
                case RequestTypeEnum.ReClassification:
                    apiModel.EvaluationSelections = model.EvaluationSelections;
                    apiModel.ClassificationId = model.ClassificationId;
                    apiModel.PreApproveId = model.PreApproveId;
                    break;
                case RequestTypeEnum.ChangeData:
                    apiModel.SelectedTransactionTypeIds = model.SelectedTransactionTypeIds;
                    if (model.SelectedTransactionTypeIds.Contains((int)TransactionTypesEnum.ChangeManager))
                    {
                        apiModel.NewManagerName = model.NewManagerName;
                        apiModel.OldManagerName = model.LicencesVM.Manager.Name1;
                        apiModel.NewManCivilId = model.NewManCivilId;
                        apiModel.OldManCivilId = model.LicencesVM.Manager.CivilId;
                        apiModel.NewManagerMobile = model.NewManagerMobile;
                        apiModel.OldManagerMobile = model.LicencesVM.Manager.Phone;
                        apiModel.NewManagerEmail = model.NewManagerEmail;

                        apiModel.OldManagerEmail = model.LicencesVM.Manager.Email;

                    }

                    if (model.SelectedTransactionTypeIds.Contains((int)TransactionTypesEnum.ChangeLicencesName))
                    {
                        apiModel.NewLicencesName = model.NewLicencesName;
                        apiModel.OldLicencesName = model.LicencesVM.LicName;
                    }

                    if (model.SelectedTransactionTypeIds.Contains((int)TransactionTypesEnum.ChangeAddress))
                    {
                        //if (model.LicencesVM.ActiivityTypeId == (int)ActivityTypeEnum.ApartmentHotel
                        //    || model.LicencesVM.ActiivityTypeId == (int)ActivityTypeEnum.Hotel
                        //    || model.LicencesVM.ActiivityTypeId == (int)ActivityTypeEnum.Resorts)
                        //{
                        //    var oldAddress = model.LicencesVM.Building?.AddressNavigation;

                        //    apiModel.OldAaliNumber = oldAddress?.AalliNo;
                        //    apiModel.OldGovernrate = oldAddress?.GovernorateArabic;
                        //    apiModel.OldArea = oldAddress?.Area;
                        //    apiModel.OldBlockNo = oldAddress?.BlockArabic;
                        //    apiModel.OldStreet = oldAddress?.StreetArabic;
                        //    apiModel.OldBuildingNo = oldAddress?.BuildingNo;
                        //    apiModel.OldBuildingName = oldAddress?.BuildingName;
                        //    apiModel.OldUnitNo = oldAddress?.UnitNo;
                        //    apiModel.OldAreaSize = oldAddress?.AreaSize;
                        //    apiModel.OldAreaChartNo = oldAddress?.AreaChartNo;
                        //    apiModel.OldFloorNo = oldAddress?.FloorNo;
                        //    apiModel.AddressId=oldAddress?.Id;
                        //}
                        //else
                        //{
                            var oldAddress = model.LicencesVM.Company?.AddressNavigation;

                            apiModel.OldAaliNumber = oldAddress?.AalliNo;
                            apiModel.OldGovernrate = oldAddress?.GovernorateArabic;
                            apiModel.OldArea = oldAddress?.Area;
                            apiModel.OldBlockNo = oldAddress?.BlockArabic;
                            apiModel.OldStreet = oldAddress?.StreetArabic;
                            apiModel.OldBuildingNo = oldAddress?.BuildingNo;
                            apiModel.OldBuildingName = oldAddress?.BuildingName;
                            apiModel.OldUnitNo = oldAddress?.UnitNo;
                            apiModel.OldAreaSize= oldAddress?.AreaSize;
                            apiModel.OldAreaChartNo = oldAddress?.AreaChartNo;
                            apiModel.OldFloorNo = oldAddress?.FloorNo;
                            apiModel.AddressId = oldAddress?.Id;
                        //}
                        apiModel.NewAaliNumber = model.NewAaliNumber;                  
                        apiModel.NewGovernrate = model.NewGovernrate;
                        apiModel.NewArea = model.NewArea;
                        apiModel.NewBlockNo = model.NewBlockNo;
                        apiModel.NewStreet = model.NewStreet;
                        apiModel.NewBuildingNo = model.NewBuildingNo;
                        apiModel.NewBuildingName = model.NewBuildingName;
                        apiModel.NewUnitNo = model.NewUnitNo;
                        apiModel.NewAreaSize = model.NewAreaSize;
                        apiModel.NewAreaChartNo = model.NewAreaChartNo;
                        apiModel.NewFloorNo = model.NewFloorNo;
                    }

                    if (model.SelectedTransactionTypeIds.Contains((int)TransactionTypesEnum.ChangeCompaneName))
                    {
                        apiModel.NewOwnerCompanyAr = model.NewOwnerCompanyAr;
                        apiModel.OldOwnerCompanyAr = model.LicencesVM.Company.OwnerCompanyAr;
                        apiModel.OldDirCompanyAr = model.LicencesVM.Company.DirCompanyAr;
                        apiModel.NewDirCompanyAr = model.NewDirCompanyAr;
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
                    return RedirectToAction("GetAllRequest", "Home");
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
                    ServiceAmount =(decimal) model.LicencesInfoVM.FixedFees, 
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
