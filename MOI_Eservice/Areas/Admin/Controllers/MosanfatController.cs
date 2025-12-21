using Business.Enums;
using Business.Helpers;
using Business.ViewModel;
using Business.ViewModel.Dynamic;
using Business.ViewModel.Tourism;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MOINFO_API.Controllers;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using static System.Net.WebRequestMethods;


namespace MOI_Eservice.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class MosanfatController : Controller
    {
        private readonly string _baseUrl;
        private readonly string _file;
        private readonly HttpClient _httpClient;
        private readonly GenerateLicNo _generateLicNo;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _env;
        private readonly HelperUrlApi _helperUrlApi;

        public MosanfatController(IConfiguration configuration, HttpClient httpClient, GenerateLicNo generateLicNo
            , ILogger<TourismController> logger, IWebHostEnvironment env, HelperUrlApi helperUrlApi)
        {
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _file = configuration["Path:Mosanafat"];
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

            var apiSettings = _baseUrl + "api/Mosanafat";
            ViewBag.ApiBaseUrl = apiSettings;

            try
            {
                using (var client = new HttpClient())
                {
                    // Configure HTTP client
                    client.BaseAddress = new Uri(_baseUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Call the API
                    var response = await client.GetAsync($"api/Mosanafat/GetRequests?serviceId={serviceId}");

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

        #endregion


        [HttpGet]
        public async Task<IActionResult> GetFlowVM(int requestId)
        {
            // Check authentication
            if (!User.Identity.IsAuthenticated)
            {
                return new JsonResult(new { success = false, message = "Unauthenticated" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }

            // Get ServiceId from claims (if you still need it)
            var serviceIdClaim = User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
            {
                return new JsonResult(new { success = false, message = "ServiceId is missing" })
                {
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }







            try
            {
                using (var client = new HttpClient())
                {
                    // Configure HttpClient
                    client.BaseAddress = new Uri(_baseUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    // Call APIMosanafat API using requestId
                    var response = await client.GetAsync(
                        $"api/Mosanafat/GetFlowVMAsync?requestId={requestId}");

                    if (!response.IsSuccessStatusCode)
                    {
                        return new JsonResult(new { success = false, message = "API call failed" })
                        {
                            StatusCode = (int)response.StatusCode
                        };
                    }

                    var jsonData = await response.Content.ReadAsStringAsync();

                    // Deserialize to WorkFlowVM
                    var flowVm = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkFlowVM>(jsonData);

                    // Optional: لو حابب تتعامل مع حالة عدم وجود Workflow
                    // if (flowVm == null)
                    // {
                    //     return new JsonResult(new { success = false, message = "Workflow not found" });
                    // }

                    return new JsonResult(new
                    {
                        success = true,
                        data = flowVm
                    });
                }
            }
            catch
            {
                return new JsonResult(new { success = false, message = "Exception occurred" })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }








        [HttpGet]
        public async Task<IActionResult> LoadFlowVM(int requestId)
        {
            // Auth check
            if (!User.Identity.IsAuthenticated)
                return Unauthorized(new { success = false, message = "Unauthenticated" });

            // Guard
            if (requestId <= 0)
                return BadRequest(new { success = false, message = "Invalid requestId" });

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl.TrimEnd('/') + "/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    // Call API
                    var url = $"api/Mosanafat/FetchWorkFlowVM?requestID={requestId}";
                    var response = await client.GetAsync(url);

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        return NotFound(new { success = false, message = "Workflow/Request not found" });

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        var badMsg = await response.Content.ReadAsStringAsync();
                        return BadRequest(new { success = false, message = badMsg });
                    }

                    if (!response.IsSuccessStatusCode)
                        return StatusCode((int)response.StatusCode,
                            new { success = false, message = "API call failed" });

                    var json = await response.Content.ReadAsStringAsync();
                    var flowVm = Newtonsoft.Json.JsonConvert.DeserializeObject<WorkFlowVM>(json);

                    return Ok(new { success = true, data = flowVm });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { success = false, message = "Exception occurred", detail = ex.Message });
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
                    "api/Mosanafat/UpdateRequest",
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





        #region Get Specific Request 
        //public async Task<ActionResult> RequestDetails(int? ID)
        //{
        //    var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
        //    if (!loggedInUser)
        //    {
        //        // Handle case where user is not logged in
        //        return RedirectToAction("Login", "Account");
        //    }
        //    var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
        //    if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
        //    {
        //        return RedirectToAction("ErrorPage", "Home");
        //    }

        //    var apiSettings = _baseUrl + $"api/Mosanafat";
        //    var dynamicUrl = _baseUrl + $"Dynamic";
        //    ViewBag.ApiBaseUrl = apiSettings;
        //    ViewBag.DynamicUrlApi = dynamicUrl;
        //    ViewBag.RequestTypes = JsonConvert.SerializeObject(Enum.GetValues(typeof(RequestTypeEnum))
        //                            .Cast<RequestTypeEnum>()
        //                            .ToDictionary(e => e.ToString(), e => (int)e));

        //    if (loggedInUser != null)
        //    {
        //        using (var client = new HttpClient())
        //        {

        //            _httpClient.BaseAddress = new Uri(_baseUrl);

        //            _httpClient.DefaultRequestHeaders.Clear();
        //            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //            var response = await _httpClient.GetAsync($"api/Mosanafat/GetRequestById?id={ID}&serviceId={serviceId}");

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var jsonData = await response.Content.ReadAsStringAsync();

        //                var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestDetailsVM>(jsonData);


        //                return View(responseData);
        //            }
        //            else
        //            {

        //                return RedirectToAction("ErrorPage", "Home");
        //            }

        //        }
        //    }
        //    else
        //    {
        //        return RedirectToAction("Login", "Account");

        //    }
        //}



        public async Task<ActionResult> RequestDetails(int? ID)
        {
            // check login
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated;
            if (!loggedInUser)
            {
                return RedirectToAction("Login", "Account");
            }

            var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            if (!ID.HasValue)
            {
                return RedirectToAction("ErrorPage", "Home");
            }

            // prepare URLs for the View (كما هي)
            var apiSettings = _baseUrl + $"api/Mosanafat";
            var dynamicUrl = _baseUrl + $"Dynamic";
            ViewBag.ApiBaseUrl = apiSettings;
            ViewBag.DynamicUrlApi = dynamicUrl;
            ViewBag.RequestTypes = JsonConvert.SerializeObject(
                Enum.GetValues(typeof(RequestTypeEnum))
                    .Cast<RequestTypeEnum>()
                    .ToDictionary(e => e.ToString(), e => (int)e)
            );
            ViewBag.PathAttachment = "Files/Mosanafat/";
            // call new API: GetRequestDetails/{id}
            _httpClient.BaseAddress = new Uri(_baseUrl);
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            // NOTE: using the new endpoint that returns RequestFrontVM
            var response = await _httpClient.GetAsync($"api/Mosanafat/GetRequestDetails/{ID.Value}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();

                // important: deserialize to RequestFrontVM instead of RequestDetailsVM
                var responseData = JsonConvert.DeserializeObject<RequestFrontVM>(jsonData);

                return View(responseData);
            }
            else
            {
                return RedirectToAction("ErrorPage", "Home");
            }
        }
        #endregion




     
       

    }
}
