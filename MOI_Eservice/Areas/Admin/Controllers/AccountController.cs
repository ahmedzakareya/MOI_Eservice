using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
//using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Drawing.Drawing2D;
using RestSharp;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Business.ViewModel.Account;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Text;
using Azure;
using static Business.ViewModel.JwtClasses.JwtClasses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Net.Http.Headers;
using Business.Helpers;
using Business.ViewModel.Dynamic;
using System.Security.Claims;
using Humanizer;
using Business.ViewModel.AddressPaciModel;
using Business.ViewModel;




namespace MOI_Eservice.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly HelperUrlApi _helperUrlApi;
        private readonly HttpClient _httpClient;
       
        private readonly string _baseUrl;
        private readonly string _PaciapiUrl;
        private readonly string _PaciInfoApiUrl;
        private readonly string _PaciPassword;
        private readonly string _PaciUsername;
        private readonly string _hierarchy;
        public string token;
        public string statusCode;
        private readonly string GetPACIAddressURL;
        private readonly string GetPACIUserTourism;
        private readonly string GetPACIPasswordTourism;
        private readonly string GetTokenURL;
        public IHttpContextAccessor _HttpContextAccessor { get; set; }


        public AccountController(IConfiguration configuration,HelperUrlApi helperUrlApi, HttpClient httpClient,IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _helperUrlApi = helperUrlApi;
            _httpClient = httpClient;
            _HttpContextAccessor = httpContextAccessor;
            GetPACIAddressURL = configuration["PaciAddressData:GetPACIAddressURL"];
            GetPACIUserTourism = configuration["PaciAddressData:GetPACIUserTourism"];
            GetPACIPasswordTourism = configuration["PaciAddressData:GetPACIPasswordTourism"];
            GetTokenURL = configuration["PaciAddressData:GetTokenURL"];
            _baseUrl = configuration["ApiSettings:BaseUrl"];
             _PaciapiUrl = configuration["PaciData:PaciAPI"];
             _PaciInfoApiUrl = configuration["PaciData:PaciInfoAPI"];
             _PaciUsername = configuration["PaciData:PaciAPIUserName"];
             _PaciPassword = configuration["PaciData:PaciAPIPassword"];
            _hierarchy = configuration["Hierarachy:MoiInfoHierarchyURL"];
        }
        public ActionResult Login()
        {
            return View();
        }
        #region Old Login
        //[HttpPost]
        //      public async Task<JsonResult> Login(SysUserVM model)
        //      {
        //	var url = _baseUrl + "Dynamic/GetDynamicMenuItems";
        //          ViewBag.ApiBaseUrl = url;

        //          if (!ModelState.IsValid)
        //	{
        //		return Json(new { success = false, responseText = "Invalid model data" });
        //	}

        //	model.Status = true;

        //	try
        //	{
        //		// Get the login data from the API
        //		SysUserVM result = await GetLoginDataAsync(model);

        //		if (result != null)
        //		{
        //			// Store result in session as JSON string
        //			 HttpContext.Session.SetString("UserData", JsonConvert.SerializeObject(result));
        //			HttpContext.Session.SetString("ApiBaseUrl", url);
        //                  return Json(new { success = true, redirectTo = Url.Action("Index", "Home") , ViewBag.ApiBaseUrl});
        //			///static redirection to service
        //                  /// Handle redirection based on ServiceId
        //                  ///  switch (result.ServiceId)
        //			///{
        //			///	case 5:
        //			///		return Json(new { success = true, redirectTo = Url.Action("Index", "Mosanafat") });
        //			///	case 6:
        //			///		return Json(new { success = true, redirectTo = Url.Action("Index", "Tourism") });
        //			///	case 4:
        //			///		return Json(new { success = true, redirectTo = Url.Action("Index", "Elaw") });
        //			///	case 2:
        //			///		return Json(new { success = true, redirectTo = Url.Action("Index", "Publishing") });
        //			///	default:
        //			///		return Json(new { success = true, responseText = "Unknown service type" });
        //			///}
        //		}
        //		else
        //		{
        //			// Return error message if login data is invalid
        //			return Json(new { success = false, responseText = "تأكد من اسم المستخدم وكلمة المرور" });
        //		}
        //	}
        //	catch (Exception ex)
        //	{
        //		// Handle exceptions and return error message
        //		return Json(new { success = false, responseText = $"An error occurred: {ex.Message}" });
        //	}



        //}
        //      public async Task<SysUserVM> GetLoginDataAsync(SysUserVM model)
        //      {
        //          // Set base URL for the API
        //          _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");

        //          _httpClient.DefaultRequestHeaders.Clear();
        //          _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //          try
        //          {
        //              model.Status = true;
        //              // Serialize the model to JSON
        //              var jsonData = JsonConvert.SerializeObject(model);
        //              var content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");

        //              // Send POST request to the API
        //              var response = await _httpClient.PostAsync("GetSysUserForLogin", content);

        //              if (response.IsSuccessStatusCode)
        //              {
        //                  // Deserialize the response content
        //                  var responseContent = await response.Content.ReadAsStringAsync();
        //                  SysUserVM result = JsonConvert.DeserializeObject<SysUserVM>(responseContent);

        //                  return result;
        //              }
        //              else
        //              {
        //                  // Handle error response
        //                  var errorResponse = await response.Content.ReadAsStringAsync();
        //                  throw new Exception($"API Error: {errorResponse}");
        //              }
        //          }
        //          catch (Exception ex)
        //          {
        //              throw new Exception($"An error occurred: {ex.Message}", ex);
        //          }
        //      }
        #endregion
        [HttpPost]
        public async Task<JsonResult> Login(SysUserVM model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, responseText = "Invalid model data" });
            }

            model.Status = true;

            try
            {
                // Call API to authenticate the user
                var result = await GetLoginDataAsync(model);
                Console.WriteLine(result);
                if (result == null)
                {
                    return Json(new { success = false, responseText = "Invalid response from API." });
                }

                // Extract token and user details
                string token = result["token"];
                string username = result["user"]["username"].ToString();
                int serviceId = int.Parse(result["user"]["serviceId"].ToString());
                bool status = bool.Parse(result["user"]["status"].ToString());
                //string UserData=

                // Ensure token includes "Bearer" prefix
                //if (!token.StartsWith("Bearer "))
                //{
                //    token = $"Bearer {token}";
                //}

                // Store details in session
                _HttpContextAccessor.HttpContext.Session.SetString("Token", token);
                _HttpContextAccessor.HttpContext.Session.SetString("AdminUsername", username);
                _HttpContextAccessor.HttpContext.Session.SetInt32("AdminServiceId", serviceId);
                _HttpContextAccessor.HttpContext.Session.SetString("AdminStatus", status.ToString());

                Console.WriteLine("Token stored in session: " + token);

                // Create an HTTP request with the Bearer token
                var request = new HttpRequestMessage(HttpMethod.Get, "/Home/Index");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Log the Authorization header
                Console.WriteLine("Authorization Header Sent: " + request.Headers.Authorization?.ToString());

                // Redirect to the Home page
                return Json(new { success = true, redirectTo = Url.Action("Index", "Home") });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return Json(new { success = false, responseText = $"An error occurred: {ex.Message}" });
            }
        }
        public ActionResult logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Home");


        }
        private async Task<dynamic> GetLoginDataAsync(SysUserVM model)
        {
           
            var apiSettings = $"{_baseUrl}api/AccountAdminApi/";
            ViewBag.ApiBaseUrl = apiSettings;

            // Set the base address and headers for HttpClient
            _httpClient.BaseAddress = new Uri(apiSettings);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Retrieve token from session if needed
            string token = _HttpContextAccessor.HttpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine("Authorization Header: " + _httpClient.DefaultRequestHeaders.Authorization);
            }

            try
            {
                // Serialize the model to JSON
                var jsonData = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                // Make the POST request
                var response = await _httpClient.PostAsync("GetSysUserForLogin", content);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize and return the response content
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<dynamic>(responseContent);
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API Error: {errorResponse}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetLoginDataAsync error: {ex.Message}");
                throw new Exception($"An error occurred: {ex.Message}", ex);
            }
        }

        // Helper: Call API for authentication
        //private async Task<dynamic> GetLoginDataAsync(SysUserVM model)
        //{
        //    _httpClient.BaseAddress = new Uri("https://localhost:7095/api/");
        //    _httpClient.DefaultRequestHeaders.Clear();
        //    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    string token = _HttpContextAccessor.HttpContext.Session.GetString("Token");

        //    try
        //    {
        //        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //        Console.WriteLine("Authorization Header: " + _httpClient.DefaultRequestHeaders.Authorization);

        //        // Prepare the content for the POST request
        //        var jsonData = JsonConvert.SerializeObject(model);
        //        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
        //        var response = await _httpClient.PostAsync("GetSysUserForLogin", content);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseContent = await response.Content.ReadAsStringAsync();


        //            var tokenFromresponse = JsonConvert.DeserializeObject<dynamic>(responseContent);


        //            return JsonConvert.DeserializeObject<dynamic>(responseContent);
        //        }
        //        else
        //        {
        //            var errorResponse = await response.Content.ReadAsStringAsync();
        //            throw new Exception($"API Error: {errorResponse}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"An error occurred: {ex.Message}", ex);
        //    }
        //}
        //[HttpPost]
        //public async Task<JsonResult> Login(SysUserVM model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Json(new { success = false, responseText = "Invalid model data" });
        //    }

        //    model.Status = true;

        //    try
        //    {
        //        Call API to authenticate user
        //       var result = await GetLoginDataAsync(model);

        //        string token = result["token"].ToString();
        //        string username = result["user"]["username"].ToString();
        //        int serviceId = int.Parse(result["user"]["serviceId"].ToString());
        //        bool status = bool.Parse(result["user"]["status"].ToString());

        //        Store token in session
        //        _HttpContextAccessor.HttpContext.Session.SetString("Token", token);
        //        _HttpContextAccessor.HttpContext.Session.SetString("Username", username);
        //        _HttpContextAccessor.HttpContext.Session.SetInt32("ServiceId", serviceId);
        //        _HttpContextAccessor.HttpContext.Session.SetString("Status", status.ToString());

        //        Explicitly construct a request with the Bearer token
        //        var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7095/api/GetDynamicMenuItems");
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //        var response = await _httpClient.SendAsync(request);

        //        if (!response.IsSuccessStatusCode)
        //        {
        //            return Json(new { success = false, responseText = "Authorization failed. Please check your credentials." });
        //        }

        //        Console.WriteLine("Authorization Header Sent: " + request.Headers.Authorization?.ToString());

        //        if (result != null)
        //        {
        //            Success, redirect to home
        //            return Json(new { success = true, redirectTo = Url.Action("Index", "Home") });
        //        }
        //        else
        //        {
        //            return Json(new { success = false, responseText = "Invalid username or password." });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, responseText = $"An error occurred: {ex.Message}" });
        //    }
        //}
        //private async Task<dynamic> GetLoginDataAsync(SysUserVM model)
        //{
        //    try
        //    {
        //        Prepare request for API authentication

        //       var jsonData = JsonConvert.SerializeObject(model);
        //        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

        //        Explicitly use HttpRequestMessage to include token(if available)
        //            var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7095/api/GetSysUserForLogin");
        //        request.Content = content;

        //        var response = await _httpClient.SendAsync(request);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseContent = await response.Content.ReadAsStringAsync();
        //            return JsonConvert.DeserializeObject<dynamic>(responseContent);
        //        }
        //        else
        //        {
        //            var errorResponse = await response.Content.ReadAsStringAsync();
        //            throw new Exception($"API Error: {errorResponse}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"An error occurred: {ex.Message}", ex);
        //    }
        //}
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


        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            HttpContext.Session.Remove("LoggedInUser");
            return RedirectToAction("Login");
        }

        public ActionResult ErrorPage()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var apiSettings = $"{_baseUrl}api/AccountAdminApi/ChangePassword";
          var user =  User.Claims.Where(c => c.Type == ClaimTypes.UserData).FirstOrDefault().Value;
          model.Id=int.Parse(user);
           var changePassword=await _helperUrlApi.PostDataToApi<ChangePasswordVM,ChangePasswordVM>(apiSettings,model);

            return RedirectToAction("Login");
        }
        [HttpGet]
        public async Task<IActionResult> AllResetPasswordUser()
        {
            var apiSettings = $"{_baseUrl}api/AccountAdminApi/AllResetPasswordUserInAdmin";
            var GetResetUserPassword = await _helperUrlApi.GetDataFromApi<List<ResetUserPasswordVM>>(apiSettings);
            return View(GetResetUserPassword);
        }

        [HttpGet]
        public async Task<IActionResult> ResetPasswordUser(int id)
        {
            var apiSettings = $"{_baseUrl}api/AccountAdminApi/ResetPasswordUserInAdmin?id={id}";
            ViewBag.ResetPasswordPath = _configuration["Path:ResetPasswordRequestImage"];
           
            var GetResetUserPassword = await _helperUrlApi.GetDataFromApi<ResetUserPasswordVM>(apiSettings);
            return View(GetResetUserPassword);  
        }

        [HttpPost]
        public async Task<IActionResult> UpdateResetRequest(ResetUserPasswordVM model, string status)
        {
            //if (!ModelState.IsValid)
            //    return View(model);

            var apiUrl = _baseUrl+ "api/AccountAdminApi/ExecuteResetPassword";
            var username = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;
            var ServiceId = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;

            var userid = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.UserData)?.Value;
            model.ProcessedBy = int.Parse(userid);
            model.ProcessedByName = username;
            var result = await _helperUrlApi.PostDataToApi<ResetUserPasswordVM, bool>(apiUrl, model);

            if (result)
            {
                TempData["SuccessMessage"] = "تم تحديث حالة الطلب بنجاح وتم إرسال بريد إلكتروني";
            }
            else
            {
                TempData["ErrorMessage"] = "فشل في إرسال البريد الإلكتروني أو تحديث الطلب.";
            }

            return RedirectToAction("AllResetPasswordUser","Account");
        }

        [HttpGet]
        public async Task<IActionResult> AllContactUsUser()
        {
            var apiSettings = $"{_baseUrl}api/AccountAdminApi/AllContactUsUserInAdmin";
            var GetResetUserPassword = await _helperUrlApi.GetDataFromApi<List<ContactUsVM>>(apiSettings);
            return View(GetResetUserPassword);
        }

        [HttpGet]
        public async Task<IActionResult> ContactUsUser(int id)
        {
            var apiSettings = $"{_baseUrl}api/AccountAdminApi/AllContactUsUserInAdminById?id={id}";
            var GetResetUserPassword = await _helperUrlApi.GetDataFromApi<ContactUsVM>(apiSettings);
            return View(GetResetUserPassword);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateNote(ContactReplyVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "حدث خطأ في البيانات المرسلة.";
                return RedirectToAction("ContactUsUser");
            }
            var username = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;
            var ServiceId = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;

            var userid = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.UserData)?.Value;
            model.ProcessedBy = int.Parse(userid);
            model.ProcessedByName = username;
            string apiEndpoint = _baseUrl+ $"api/AccountAdminApi/SendContactReply";
            var response = await _helperUrlApi.PostDataToApi<ContactReplyVM, bool>(apiEndpoint, model);

            if (response)
                TempData["SuccessMessage"] = "تم إرسال الرد بنجاح.";
            else
                TempData["ErrorMessage"] = "فشل في إرسال الرد عبر البريد الإلكتروني.";

            return RedirectToAction("ContactUsUser");
        }


        [HttpGet]
        public async Task<IActionResult> GetAllMandoobPending()
        {
            var apiSettings = $"{_baseUrl}api/AccountAdminApi/GetPendingDelegations";
            var GetPendingDelegation = await _helperUrlApi.GetDataFromApi<List<PendingDelegationVM>>(apiSettings);
            return View(GetPendingDelegation);
        }

        [HttpGet]
        public async Task<IActionResult> ViewDelegationDetails(int id)
        {
            var apiUrl = $"{_baseUrl}api/AccountAdminApi/GetDelegationById?id={id}";
            var delegation = await _helperUrlApi.GetDataFromApi<PendingDelegationVM>(apiUrl);

            if (delegation == null)
                return NotFound();

            return View (delegation);
        }
        [HttpPost]
        public async Task<IActionResult> ApproveMandoobDelegation(int id, bool approve, string? note)
        {
            var apiUrl = $"{_baseUrl}api/AccountAdminApi/ApproveMandoobDelegation";

            var data = new PendingDelegationVM
            {
                Id = id,
                IsApproved = approve,
                Note = note
            };

            var result = await _helperUrlApi.PostDataToApi<PendingDelegationVM, ErrorMessage>(apiUrl, data);

            if (result != null && result.Error == false)
            {
                TempData["SuccessMessage"] = result.Message ?? "تم تنفيذ العملية بنجاح";
            }
            else
            {
                TempData["ErrorMessage"] = result?.Message ?? "حدث خطأ أثناء تنفيذ الطلب";
            }

            return RedirectToAction("GetAllMandoobPending");
        }






        [HttpGet]
        public async Task<IActionResult> GetAddressDataFromPaci(string id)
        {
            string token = "";
            JObject jObject = await GetTokenAsync("GISmosanafat", "GISmosanafatmoinfo@2023");

            token = jObject.Value<string>("access_token");

            returnAddressValues paciData = await GetPaciAddressDataAsync(token, id);

            JObject paciLocation = null;

            try
            {
                if (paciData != null)
                {
                    paciLocation = await GetPaciAddressLocationAsync(token, paciData.lon, paciData.lat);
                }
            }
            catch (Exception ex)
            {
                // Optional: log the exception or handle it
            }

            return Json(paciData);
        }
        public async Task<returnAddressValues> GetPaciAddressDataAsync(string token, string paciNo)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var content = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string, string>("pacino", paciNo)
        });

                var response = await client.PostAsync("https://api.media.gov.kw/OtherMinistries/api/paci/gis/Addressinfobypacino", content);

                var responseContent = await response.Content.ReadAsStringAsync();

                var addressData = JsonConvert.DeserializeObject<returnAddressValues>(responseContent);

                if (addressData != null)
                {
                    string fullAddress = "المنطقة : " + addressData.neighborhoodarabic + " - " +
                                         "القطعة  : " + addressData.blockarabic + " - " +
                                         "الشارع : " + addressData.streetarabic + " - " +
                                         "القسيمة : " + addressData.parcelarabic + " - " +
                                         " : إسم المبني " + addressData.buildingnamearabic + " - " +
                                         " الدور :  " + addressData.floor_no + " - " +
                                         " رقم الوحدة :  " + addressData.unit_no + " - " +
                                         " الرقم الالي للعنوان :  " + paciNo;

                    addressData.FullAddressText = fullAddress;
                }

                return addressData;
            }
        }
        public async Task<JObject> GetPaciAddressLocationAsync(string token, string longitude, string latitude)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var content = new FormUrlEncodedContent(new[]
                {
            new KeyValuePair<string, string>("longitude", longitude),
            new KeyValuePair<string, string>("latitude", latitude)
        });

                var response = await client.PostAsync("https://api.media.gov.kw/OtherMinistries/api/paci/gis/InspectorLocation", content);

                var responseContent = await response.Content.ReadAsStringAsync();

                JObject jObject = JsonConvert.DeserializeObject<JObject>(responseContent);

                return jObject;
            }
        }

        public async Task<JObject> GetTokenAsync(string username, string password)
        {
            var config = new Dictionary<string, string>
    {
        { "grant_type", "password" },
        { "Username", username },
        { "Password", password }
    };

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders
                      .Accept
                      .Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

                var content = new FormUrlEncodedContent(config);

                var response = await client.PostAsync("https://api.media.gov.kw/OtherMinistries/api/GetToken", content);

                var responseContent = await response.Content.ReadAsStringAsync();

                JObject jObject = JsonConvert.DeserializeObject<JObject>(responseContent);

                return jObject;
            }
        }




        public class returnAddressValues
        {
            public string governoratearabic { get; set; }
            public string governorateid { get; set; }
            public string blockarabic { get; set; }
            public string neighborhoodarabic { get; set; }
            public string neighborhoodid { get; set; }
            public string floor_no { get; set; }
            public string parcelarabic { get; set; }
            public string streetarabic { get; set; }
            public string buildingnamearabic { get; set; }
            public string longitude { get; set; }
            public string latitude { get; set; }
            public string Location { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
            public string unit_no { get; set; }
            public string FullAddressText { get; set; }
            public string buildingtypearabic { get; set; }
        }

    }



}
