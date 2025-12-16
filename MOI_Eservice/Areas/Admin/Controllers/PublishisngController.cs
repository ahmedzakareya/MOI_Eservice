using Business.Helpers;
using Business.ViewModel;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;


namespace MOI_Eservice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PublishingController : Controller
    {
        private readonly string _baseUrl;
        private readonly string _file;
        private readonly HttpClient _httpClient;
        private readonly GenerateLicNo _generateLicNo;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;
        private readonly HelperUrlApi _helperUrlApi;

        public PublishingController(IConfiguration configuration, HttpClient httpClient, GenerateLicNo generateLicNo
            , ILogger<PublishingController> logger, IWebHostEnvironment env, HelperUrlApi helperUrlApi)
        {
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _file = configuration["Path:Publishing"];
            _httpClient = httpClient;
            _generateLicNo = generateLicNo;
            _logger = logger;
            _env = env;
            _helperUrlApi = helperUrlApi;
        }
        public IActionResult Index()
        {
            string loggedInUser = HttpContext.Session.GetString("UserData");

            return View();
        }
        #region Request 
        //AllRequest
        public async Task<ActionResult> Tourism_AllRequest()
        {

            

            var loggedInUser = HttpContext.Session.GetString("UserData");
            dynamic userData = JsonSerializer.Deserialize<dynamic>(loggedInUser);

            if (loggedInUser != null)
            {
               
                int serviceId= userData.GetProperty("ServiceId").GetInt32();

                // Access the ServiceId property
                //var serviceId = userData.ServiceId;

                _httpClient.BaseAddress = new Uri(_baseUrl);

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var requestUrl = _baseUrl + "/" + $"api/GetAllRequest?ServiceId={serviceId}";

                var response = await _httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)

                {
                    // Read response content as a list of RequestVM
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RequestVM>>(jsonData);

                    return View(responseData);
                }
                else
                {

                    return RedirectToAction("ErrorPage", "Home");
                }

            }
            return View();
        }
        #endregion

        #region Licences
        public async Task<ActionResult> Tourism_AllLicences()
        {


            var loggedInUser = HttpContext.Session.GetString("UserData");
            dynamic userData = JsonSerializer.Deserialize<dynamic>(loggedInUser);
            if (loggedInUser != null)
            {

                int serviceId = userData.GetProperty("ServiceId").GetInt32();

                _httpClient.BaseAddress = new Uri(_baseUrl);

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var requestUrl = _baseUrl + "/" + $"api/GetAllLicences?ServiceId={serviceId}";

                var response = await _httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)

                {
                    // Read response content as a list of RequestVM
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LicencesVM>>(jsonData);

                    return View(responseData);
                }
                else
                {

                    return RedirectToAction("ErrorPage", "Home");
                }

            }
            return View();
        }
        #endregion

        #region Get Specific Request 
        public async Task<ActionResult> RequestDetails(int? ID)
        {
            var loggedInUser = HttpContext.Session.GetString("UserData");
            dynamic userData = JsonSerializer.Deserialize<dynamic>(loggedInUser);
            int serviceId = userData.GetProperty("ServiceId").GetInt32();

            if (loggedInUser != null)
            {
                using (var client = new HttpClient())
                {

                    _httpClient.BaseAddress = new Uri(_baseUrl);

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync($"GetRequestById?id={ID}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();

                        var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RequestVM>>(jsonData);


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
                return RedirectToAction("Login", "Home");

            }
        }
        #endregion
        #region Get Specific Licences 
        public async Task<ActionResult> LicencesDetails(int? ID)
        {
            var loggedInUser = HttpContext.Session.GetString("UserData");
            dynamic userData = JsonSerializer.Deserialize<dynamic>(loggedInUser);
            int serviceId = userData.GetProperty("ServiceId").GetInt32();

            if (loggedInUser != null)
            {
                using (var client = new HttpClient())
                {

                    _httpClient.BaseAddress = new Uri(_baseUrl);

                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.GetAsync($"GetLicencesById?id={ID}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();

                        var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LicencesVM>>(jsonData);


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
                return RedirectToAction("Login", "Home");

            }
        }
        #endregion



        public async Task<ActionResult> GetAllRequests()

        {
            // Check if the user is logged in
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // Extract ServiceId from claims
            var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            var apiSettings = _baseUrl + "api/Publishing";
            ViewBag.ApiBaseUrl = apiSettings;

            try
            {
                using (var client = new HttpClient())
                {
                    // Configure HTTP client
                    client.BaseAddress = new Uri(_baseUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Call the API
                    var response = await client.GetAsync($"api/Publishing/GetRequests?serviceId={serviceId}");

                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response data
                        var jsonData = await response.Content.ReadAsStringAsync();
                        var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<RequestVM>>(jsonData);

                        return View(responseData);
                    }
                    else
                    {
                        // Redirect to an error page if the API call fails
                        return RedirectToAction("ErrorPage", "Home");
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return RedirectToAction("ErrorPage", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> HandleRequestStatus(
     int requestId,
     string actionType,
     string note,
     string requestType,
     string requestNo)
        {
            try
            {
                // 1) Get Civil ID from Session (must be the real CivilId)
                var civilId = HttpContext.Session.GetString("AdminUsername"); // <-- غيّرها من AdminUsername

                if (string.IsNullOrWhiteSpace(civilId))
                {
                    return Json(new
                    {
                        success = false,
                        message = "لا يوجد رقم مدني للمستخدم في الجلسة."
                    });
                }

                // 2) Prepare input model for API
                var input = new UpdateRequestInput
                {
                    RequestID = requestId,
                    Notes = note,
                    CivilID = civilId
                };

                // 3) Call API
                var apiResult = await _helperUrlApi.PostDataToApi<UpdateRequestInput, bool>(
                    "api/Publishing/UpdateRequest",
                    input
                );

                // 4) Handle API result
                if (!apiResult)
                {
                    return Json(new
                    {
                        success = false,
                        message = "لم يتم تحديث حالة الطلب، برجاء مراجعة بيانات المستخدم أو مسار الطلب."
                    });
                }

                // 5) Success
                return Json(new
                {
                    success = true,
                    message = $"تم تعديل حالة الطلب رقم {requestNo} بنجاح."
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    success = false,
                    message = "حدث خطأ غير متوقع أثناء تعديل حالة الطلب."
                });
            }
        }



    }
}
