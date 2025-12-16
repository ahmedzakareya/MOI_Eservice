using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using RestSharp;
using static MOI_Eservice.Controllers.AccountController;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web;    
using Microsoft.AspNetCore.Authorization;
using Business.ViewModel.Account;
using Business.Helpers;
using Business.ViewModel;
using Business.ViewModel.AddressPaciModel;
using System.Text.Json;
using Azure;
using Business.Enums;
using static Business.ViewModel.JwtClasses.JwtClasses;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Business.ViewModel.Dynamic;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;




namespace MOI_Eservice.Controllers
{
    //[Authorize(AuthenticationSchemes = "UserScheme")]

    public class AccountController : Controller
    {
        public string attpath;
        public static string token;
        public static string statusCode;
        public static string _baseUrl;
        public static string _PaciapiUrl;
        public static string _PaciInfoApiUrl;
        public static string _PaciPassword;
        public static string _PaciUsername;
        public static string eserviceToken;
        private readonly HttpClient _httpClient;
        private readonly string _file;
        private readonly HelperUrlApi _helperUrlApi;
        private readonly IWebHostEnvironment _env;
        public IHttpContextAccessor _httpContextAccessor;
     

        public AccountController(IConfiguration configuration
            ,HttpClient httpClient,HelperUrlApi helperUrlApi, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _PaciapiUrl = configuration["PaciData:PaciAPI"];
            _PaciInfoApiUrl = configuration["PaciData:PaciInfoAPI"];
            _PaciUsername = configuration["PaciData:PaciAPIUserName"];
            _PaciPassword = configuration["PaciData:PaciAPIPassword"];
            eserviceToken = configuration["EserviceId"];
            _file = configuration["Path:ResetPasswordRequestImage"];
            _httpClient = httpClient;
            _helperUrlApi = helperUrlApi;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            
        }

      

        //#region My Profile

        //[CustomAuthFilter]
        //public ActionResult MyProfile()
        //{
        //    try
        //    {
        //        AspNetUser user = Session["MoiUser"] as AspNetUser;

        //        string userType = string.Empty;
        //        if (user != null)
        //        {
        //            userType = GetAccountTypeCentralAll(int.Parse(user.AccountTypeId));
        //        }

        //        ViewBag.userType = userType;

        //        return View(user);
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
        //[ValidateAntiForgeryToken]
        //[CustomAuthFilter]
        //public ActionResult MyProfile(AspNetUser user)
        //{

        //    try
        //    {
        //        var moiUser = Session["MoiUser"] as AspNetUser;

        //        ProfileModel model = new ProfileModel
        //        {
        //            Mobile = user.mobile,
        //            FullNameAr = user.fullNameAr,
        //            FullNameEn = user.fullNameEn,
        //            PhoneNumber = user.mobile,
        //            Email = user.email,
        //        };

        //        try
        //        {
        //            bool result = ApiWrapper.Post("Profile/UpdateProfile", moiUser.access_token, model);
        //            if (result == true)
        //            {
        //                moiUser.mobile = user.mobile;
        //                moiUser.fullNameAr = user.fullNameAr;
        //                moiUser.fullNameEn = user.fullNameEn;
        //                moiUser.email = user.email;

        //                Session["MoiUser"] = moiUser;

        //                return RedirectToAction("MyAccount", "Home");
        //            }
        //            else
        //            {
        //                return View(model);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
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
        //#endregion

        #region Login

        [AllowAnonymous]
        public ActionResult Login(/*string returnUrl*/)
        {
            try
            {
               
                return View();
            }
            catch (Exception ex)
            {

                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                string fileName = controllerName + "_" + actionName + "_";

                
                throw;
            }

        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Login(LoginViewModel model/*, string returnUrl*/)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return View(model);
        //        }
        //        var apiSettings = _baseUrl + $"api/AccountFront/LoginUser";

        //        ViewBag.ApiBaseUrl = apiSettings;



        //            // Make the POST request
        //            var response =await  _helperUrlApi.PostDataToApi<LoginViewModel, LoginViewModel>(apiSettings, model);


        //       // Console.WriteLine(result);
        //        if (response == null)
        //        {
        //            return Json(new { success = false, responseText = "Invalid response from API." });
        //        }


        //        var token=response.token;


        //        Console.WriteLine("Token stored in session: " + token);


        //        var request = new HttpRequestMessage(HttpMethod.Get, "/Home/Index");
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //        var handler = new JwtSecurityTokenHandler();
        //        var jwtToken = handler.ReadJwtToken(token);
        //        // Log the Authorization header
        //        Console.WriteLine("Authorization Header Sent: " + request.Headers.Authorization?.ToString());
        //        var userName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        //        var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        //        var civilId = jwtToken.Claims.FirstOrDefault(c => c.Type == "CivilId")?.Value;
        //        var fullName = jwtToken.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
        //        var accountTypeId = jwtToken.Claims.FirstOrDefault(c => c.Type == "AccouuntTypeId")?.Value;
        //        // Optionally, store the token in the session or a cookie to use for subsequent requests
        //        HttpContext.Session.SetString("UserToken", token);
        //        HttpContext.Session.SetString("UserId", userId); 

        //        HttpContext.Session.SetString("UserAccouuntTypeId", accountTypeId);
        //        HttpContext.Session.SetString("UserFullName", fullName);
        //        HttpContext.Session.SetString("UserCivilId", civilId);

        //        // Or use localStorage or cookies
        //        //string redirectUrl = Url.Link("Default", new { controller = "Home", action = "Index" });
        //        //string redirectUrl = $"{Request.Scheme}://{Request.Host}{Url.Action("Index", "Home")}";
        //        // Redirect to Home page or another protected page
        //        //return Json(new { success = true, redirectToUrl = Url.Action("Index", "Home", new { token = token }) });
        //        //string redirectUrl = Url.Action("Index", "Home");
        //        //string baseUrl = $"{Request.Scheme}://{Request.Host}";
        //        //string redirectUrl = $"{baseUrl}/Home/Index";
        //        //var urlHelper = n(Request.RequestContext);

        //        // Get the full URL using the controller, action, and query parameters
        //        string redirectUrl = $"{Request.Scheme}://{Request.Host}/Home/Index";
        //        // Perform a server-side redirect
        //        HttpContext.Response.Redirect(redirectUrl);  // Redirects the user to the specified URL

        //        // Return null since the redirect is already performed
        //        return Json(new { success = true, redirectToUrl = redirectUrl });

        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.Contains("The user name or password is incorrect"))
        //        {
        //            ModelState.AddModelError("", "اسم المستخدم أو كلمة المرور غير صحيحة");
        //        }
        //        else
        //        {

        //            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //            string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
        //            string fileName = controllerName + "_" + actionName + "_";

        //            string exId = ExceptionLog.LogException(ex, fileName);

        //            TempData["Ex"] = exId;
        //            throw;
        //        }

        //        return View(model);

        //    }
        //}
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                var apiSettings = _baseUrl + $"api/AccountFront/LoginUser";
                var response = await _helperUrlApi.PostDataToApi<LoginViewModel, LoginViewModel>(apiSettings, model);

                if (response == null || string.IsNullOrEmpty(response.token))
                {
                    ModelState.AddModelError("", "رقم المدني أو كلمة السر غير صحيحة");
                    return View(model);
                }

                var token = response.token;
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var userId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var fullName = jwtToken.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
                var civilId = jwtToken.Claims.FirstOrDefault(c => c.Type == "CivilId")?.Value;
                var accountTypeId = jwtToken.Claims.FirstOrDefault(c => c.Type == "AccouuntTypeId")?.Value;
                var isDelegate = jwtToken.Claims.FirstOrDefault(c => c.Type == "IsDelegate")?.Value;
                var isApplicant = jwtToken.Claims.FirstOrDefault(c => c.Type == "IsApplicant")?.Value;

                HttpContext.Session.SetString("UserIsDelegate", isDelegate);
                HttpContext.Session.SetString("UserIsApplicant", isApplicant);
                HttpContext.Session.SetString("UserToken", token);
                HttpContext.Session.SetString("UserId", userId);
                HttpContext.Session.SetString("UserAccouuntTypeId", accountTypeId);
                HttpContext.Session.SetString("UserFullName", fullName);
                HttpContext.Session.SetString("UserCivilId", civilId);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "حدث خطأ أثناء محاولة تسجيل الدخول. يرجى المحاولة لاحقًا.");

                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                string fileName = controllerName + "_" + actionName + "_";

                string exId = ExceptionLog.LogException(ex, fileName);
                TempData["Ex"] = exId;

                return View(model);
            }
        }

        #endregion
        #region LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            try
            {
                // إزالة كل القيم من الجلسة
                HttpContext.Session.Clear();

                // أو يمكنك استخدام:
                // HttpContext.Session.Remove("UserToken");
                // HttpContext.Session.Remove("UserId");
                // HttpContext.Session.Remove("UserAccouuntTypeId");
                // HttpContext.Session.Remove("UserFullName");
                // HttpContext.Session.Remove("UserCivilId");

                // إعادة التوجيه لصفحة تسجيل الدخول أو الرئيسية
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                string fileName = controllerName + "_" + actionName + "_";

                string exId = ExceptionLog.LogException(ex, fileName);
                TempData["Ex"] = exId;

                // إعادة التوجيه إلى صفحة الخطأ أو تسجيل الدخول
                return RedirectToAction("Login", "Account");
            }
        }
        #endregion

        #region Register
        #region Get Paci Data By CivilId
        public string gettoken_Paci()
        {
            try
            {

                var config = new ClientConfigVM()
                {
                    grant_type = "password",
                    username = _PaciUsername,
                    password = _PaciPassword
                };



                var client = new RestClient(_PaciapiUrl + "GetToken");


                var request = new RestRequest();
                request.Method = RestSharp.Method.Post;
                request.AddHeader("content-type", "application/x-www-form-urlencoded");

                request.AddParameter("application/x-www-form-urlencoded", "grant_type=password&Username=" + config.username + "&Password=" + config.password, ParameterType.RequestBody);
                var response = client.Execute(request);

                string ysysy = response.ResponseStatus.ToString();
                JObject jObject = JsonConvert.DeserializeObject<dynamic>(response.Content);
                PaciTokenResultVM paciTokenResult = new PaciTokenResultVM();
                paciTokenResult = JsonConvert.DeserializeObject<PaciTokenResultVM>(response.Content.ToString());
                token = paciTokenResult.access_token;

                return token;
            }
            catch (Exception ex)
            {
                return "Error : " + ex.Message;
            }

        }

        public string gettoken_PaciInfo(string _token, string civilID)
        {

            var client = new RestClient(_PaciInfoApiUrl + "PACI/GetPersonDetails");


            var request = new RestRequest();
            request.Method = RestSharp.Method.Post;
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", "Bearer " + _token);
            request.AddParameter("application/x-www-form-urlencoded", "civilId= " + civilID, ParameterType.RequestBody);
            var response = client.Execute(request);

            string responsStatues = response.ResponseStatus.ToString();
            JObject jObject = JsonConvert.DeserializeObject<dynamic>(response.Content);

            if (responsStatues == "Completed")
            {
                PaciDataVM PaciData = new PaciDataVM();

                PaciData.statusCode = jObject.Value<string>("statusCode").ToString();
                statusCode = PaciData.statusCode;

                return statusCode;
            }
            else
            {
                statusCode = "error code";
                return statusCode;
            }


        }

        public PaciDataVM Get_PACI_User_Info(string _token2, string civilID)
        {
            PaciDataVM PaciData = new PaciDataVM();

            string keyapi = _PaciInfoApiUrl;
            var client = new RestClient(keyapi + "PACI/GetPersonDetails");


            var request = new RestRequest();
            request.Method = RestSharp.Method.Post;
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("Authorization", "Bearer " + _token2);
            request.AddParameter("application/x-www-form-urlencoded", "civilId= " + civilID, ParameterType.RequestBody);
            var response = client.Execute(request);

            string responsStatues = response.ResponseStatus.ToString();
            JObject jObject = JsonConvert.DeserializeObject<dynamic>(response.Content);

            string statusCode = "";
            statusCode = jObject.Value<string>("statusCode").ToString();
            if (responsStatues == "Completed")
            {
                if (statusCode == "900")
                {
                    PaciData.arFullName = jObject.Value<string>("arFullName").ToString();
                    PaciData.enFullName = jObject.Value<string>("enFullName").ToString();
                    PaciData.birthDate = jObject.Value<string>("birthDate").ToString();
                    PaciData.email = jObject.Value<string>("email").ToString();
                    PaciData.mobile = jObject.Value<string>("mobile").ToString();
                    PaciData.sex = jObject.Value<string>("sex").ToString();
                    PaciData.statusCode = jObject.Value<string>("statusCode").ToString();
                    PaciData.disclaimer = jObject.Value<string>("disclaimer").ToString();
                    PaciData.remainingHits = jObject.Value<string>("remainingHits").ToString();
                    PaciData.timeStamp = jObject.Value<string>("timeStamp").ToString();
                    PaciData.message = jObject.Value<string>("message").ToString();
                    PaciData.environment = jObject.Value<string>("environment").ToString();
                }
                else
                {
                    PaciData.statusCode = jObject.Value<string>("statusCode").ToString();
                    PaciData.disclaimer = jObject.Value<string>("disclaimer").ToString();
                    PaciData.remainingHits = jObject.Value<string>("remainingHits").ToString();
                    PaciData.timeStamp = jObject.Value<string>("timeStamp").ToString();
                    PaciData.message = jObject.Value<string>("message").ToString();
                    PaciData.environment = jObject.Value<string>("environment").ToString();
                }
            }
            return PaciData;
        }
       
        public JsonResult GetUserDataPACI(string CivilID)
        {
            token = gettoken_Paci();
            statusCode = gettoken_PaciInfo(token, CivilID);

            PaciDataVM PACIUserData = new PaciDataVM();
            var result = new { success = false, message = "", data = PACIUserData }; // Default response

            if (statusCode == "900")
            {
                PACIUserData = Get_PACI_User_Info(token, CivilID);
                result = new { success = true, message = "", data = PACIUserData }; // Success response
            }
            else
            {
                result = new { success = false, message = "الرقم المدني يجب أن يكون لكويتي", data = PACIUserData }; // Error message
            }

            return Json(result);

        }
        #endregion





        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            try
            {
                model.FullNameEn = model.FullNameAr;
                var paciResponse = GetUserDataPACI(model.CivilID);

                var jsonString = System.Text.Json.JsonSerializer.Serialize(paciResponse.Value);
                using var doc = JsonDocument.Parse(jsonString);

                var root = doc.RootElement;

                bool success = root.GetProperty("success").GetBoolean();
                string message = root.GetProperty("message").GetString();
                if (success)
                {
                    var requestData = new RegisterViewModel
                    {
                        CivilID=model.CivilID,
                        ConfirmPassword=model.ConfirmPassword,
                        Email=model.Email,
                        FullNameAr = model.FullNameAr,
                        FullNameEn = model.FullNameAr,
                        Mobile = model.Mobile,
                        Password = model.Password
                        
                    };

                    var apiSettings = _baseUrl + $"api/AccountFront/RegisterUser";

                    var sendToApi = await _helperUrlApi.PostDataToApi<RegisterViewModel, RegisterViewModel>
                        (apiSettings, requestData);
                    if (sendToApi == null)
                    {
                        TempData["ErrorMessage"] = "An error occurred while registering. Please try again."; // You can customize this message
                        return View(model);
                    }

                    // If successful, redirect to login or another action
                    TempData["SuccessMessage"] = "User registered successfully.";
                    return RedirectToAction("Login"); // Redirect to login page
                }
                
                return View();
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
        #region ResetPassword
        public async Task<ActionResult> ResetPassword()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel1 model)
        {
            var apiEndpoint = _baseUrl + "api/AccountFront/ResetPasswordUser";
            ViewBag.ApiBaseUrl = apiEndpoint;

            if (model.Image != null)
            {
                // Save the uploaded image
                var saveResult = await SaveImageToDiskAsync(model.Image, _file);

                if (saveResult != null)
                {
                    model.AttachPath = saveResult.FileName;
                    Console.WriteLine($"File saved: {saveResult.FilePath}");
                }
            }

            var responseSend = new ResetPasswordVM
            {
                CivilID=model.CivilID,
                AttachPath=model.AttachPath,
                Email=model.Email,
                NewPass=model.NewPass,
                Mobile = model.Mobile   
            };

            var response = await _helperUrlApi.PostDataToApi<ResetPasswordVM, Dictionary<string, string>>(apiEndpoint, responseSend);

            if (response != null && response.ContainsKey("message"))
            {
                TempData["SuccessMessage"] = response["message"];
            }
            else
            {
                TempData["ErrorMessage"] = "حدث خطأ يرجى المحاولة لاحقًا.";
            }
            return View(); // optionally pass data or feedback via ViewBag or Model
        }

        //Save Image For Reset Password
        public async Task<FileSaveResponseVM> SaveImageToDiskAsync(IFormFile file, string relativePath)
        {
            try
            {
                // Ensure target folder exists
                string folderPath = Path.Combine(_env.WebRootPath, relativePath);
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                // Generate unique filename
                string extension = Path.GetExtension(file.FileName);
                string fileName = $"{Guid.NewGuid():N}{extension}";

                // Create full path and save
                string fullPath = Path.Combine(folderPath, fileName);
                await using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);

                return new FileSaveResponseVM
                {
                    FilePath = Path.Combine(relativePath, fileName).Replace("\\", "/"),
                    FileName = fileName
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Image saving failed: {ex.Message}");
                throw;
            }
        }



        #endregion
        #region ContactUs

        public async Task<ActionResult> ContactUs()
        {
            var apiEndpoint = _baseUrl + "SystemOptions/GetAllOptions";
            var response=await _helperUrlApi.GetDataFromApi<List<SystemOptionVM>>(apiEndpoint);
            var viewModel = new ContactUsPageViewModel
            {
                contactUsVM = new ContactUsVM(),
                SystemOptionVM = response
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<ActionResult> ContactUs(ContactUsVM contactUsVM)
        {
            var apiSettings = _baseUrl + $"api/AccountFront/ContactUsUser";

            ViewBag.ApiBaseUrl = apiSettings;

            contactUsVM.CreatedOn = DateTime.Now;
            contactUsVM.IsDeleted = false;
            contactUsVM.IsReplayed = false;
            contactUsVM.FullNameEn = contactUsVM.FullNameAr;
            contactUsVM.Status=contactUsVM.Status;
            contactUsVM.Note = "";

            // Make the POST request
            var response = await _helperUrlApi.PostDataToApi<ContactUsVM, ContactUsVM>(apiSettings, contactUsVM);


            return View();
        }

        #endregion


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
        }


    }
}

