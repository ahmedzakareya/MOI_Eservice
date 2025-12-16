using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using Business.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
//using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using NuGet.Common;
using System.Drawing.Drawing2D;
using RestSharp;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Business.ViewModel.Account;
using Business.ViewModel.Dynamic;
using Business.Helpers;




namespace MOI_Eservice.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize]

    public class HomeController : Controller
    {
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _clientFactory;
        private readonly HelperUrlApi _helperUrlApi;

        public HomeController(IConfiguration configuration, HttpClient httpClient, IHttpClientFactory clientFactory,HelperUrlApi helperUrlApi)
        {
            _baseUrl = configuration["ApiSettings:BaseUrl"];

            _httpClient = httpClient;
            _clientFactory = clientFactory;
            _helperUrlApi = helperUrlApi;
        }
        #region Index
        public async Task<IActionResult> Index()
         {
            // Retrieve token and user-related session data
            string token = HttpContext.Session.GetString("Token");
            string username = HttpContext.Session.GetString("AdminUsername");
            int? serviceId = HttpContext.Session.GetInt32("AdminServiceId");
            string status = HttpContext.Session.GetString("AdminStatus");

            // Assign session data to ViewBag for use in the view
            ViewBag.Username = username;
            ViewBag.ServiceId = serviceId;

            // Ensure required session variables are present
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(username) || serviceId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            try
            {
                // Set up HttpClient for API call
                _httpClient.BaseAddress = new Uri(_baseUrl);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Include the Bearer token in the Authorization header
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                // Build the request URL
                var requestUrl = $"{_baseUrl}api/AccountAdminApi/GetAllStatistics?ServiceId={serviceId}";

                // Make the API call
                var response = await _httpClient.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Parse the response content into the StatisticsViewModel
                    var jsonData = await response.Content.ReadAsStringAsync();
                    var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<StatisticsViewModel>(jsonData);

                    // Pass the data to the view
                    return View(responseData);
                }
                else
                {
                    // Handle non-success status codes
                    Console.WriteLine($"API call failed: {response.StatusCode}");
                    return RedirectToAction("ErrorPage", "Home");
                }
            }
            catch (Exception ex)
            {
                // Log the exception and redirect to the error page
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return RedirectToAction("ErrorPage", "Home");
            }
        }
        
        //public async Task<IActionResult> Index()
        //{
        //    // Retrieve token and user-related session data
        //    string token = HttpContext.Session.GetString("Token");

        //    //if (!string.IsNullOrEmpty(token))
        //    //{
        //    //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //    //}
        //    string username = HttpContext.Session.GetString("Username");
        //    int? serviceId = HttpContext.Session.GetInt32("ServiceId");
        //    string status = HttpContext.Session.GetString("Status");
        //    ViewBag.username = username;
        //    ViewBag.serviceId = serviceId;

        //    // Ensure session variables are present
        //    if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(username) || serviceId == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    try
        //    {


        //        // Access the ServiceId property
        //        //var serviceId = userData.ServiceId;

        //        _httpClient.BaseAddress = new Uri(_baseUrl);

        //        _httpClient.DefaultRequestHeaders.Clear();
        //        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        var requestUrl = _baseUrl + $"api/GetAllStatistics?ServiceId={serviceId}";

        //        var response = await _httpClient.GetAsync($"api/GetAllStatistics?ServiceId={serviceId}");

        //        if (response.IsSuccessStatusCode)

        //        {
        //            // Read response content as a list of RequestVM
        //            var jsonData = await response.Content.ReadAsStringAsync();
        //            var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<StatisticsViewModel>(jsonData);

        //            return View(responseData);
        //        }
        //        else
        //        {

        //            return RedirectToAction("ErrorPage", "Home");
        //        }

        //    }catch(Exception ex)
        //    {
        //        return RedirectToAction("ErrorPage", "Home");

        //    }

        //}
        #endregion
        #region Request 
        //AllRequest
        public async Task<ActionResult> AllRequest()
        {



            var loggedInUser = HttpContext.Session.GetString("AdminUsername");
            var token = HttpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            //dynamic userData = System.Text.Json.JsonSerializer.Deserialize<dynamic>(loggedInUser);
            int? serviceId = HttpContext.Session.GetInt32("AdminServiceId");

            if (loggedInUser != null)
            {


                // Access the ServiceId property
                //var serviceId = userData.ServiceId;

                _httpClient.BaseAddress = new Uri(_baseUrl);

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var requestUrl = _baseUrl + $"api/GetAllRequest?ServiceId={serviceId}";

                var response = await _httpClient.GetAsync($"api/GetAllRequest?ServiceId={serviceId}");

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
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region Licences
        public async Task<ActionResult> AllLicences()
        {

            var loggedInUser = HttpContext.Session.GetString("AdminUsername");
            var loggedInUserToken = HttpContext.Session.GetString("Token");

            //dynamic userData = System.Text.Json.JsonSerializer.Deserialize<dynamic>(loggedInUser);
            int? serviceId = HttpContext.Session.GetInt32("AdminServiceId");

            if (loggedInUser != null)
            {

               // int serviceId = userData.GetProperty("ServiceId").GetInt32();

                _httpClient.BaseAddress = new Uri(_baseUrl);

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.GetAsync($"api/GetAllLicences?ServiceId={serviceId}");



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
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region Get Specific Request 
        public async Task<ActionResult> RequestDetails(int? ID)
        {
            var loggedInUser = HttpContext.Session.GetString("AdminUsername");
            var loggedInUserToken = HttpContext.Session.GetString("AdminToken");

            //dynamic userData = System.Text.Json.JsonSerializer.Deserialize<dynamic>(loggedInUser);
            int? serviceId = HttpContext.Session.GetInt32("AdminServiceId");


            if (loggedInUser != null)
            {
                using (var client = new HttpClient())
                {

                    _httpClient.BaseAddress = new Uri(_baseUrl);

                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await _httpClient.GetAsync($"api/GetRequestById?id={ID}&serviceId={serviceId}");

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
        #region Get Specific Licences 
        public async Task<ActionResult> LicencesDetails(int? ID)
        {
            var loggedInUser = HttpContext.Session.GetString("Username");
            var loggedInUserToken = HttpContext.Session.GetString("Token");

            //dynamic userData = System.Text.Json.JsonSerializer.Deserialize<dynamic>(loggedInUser);
            int? serviceId = HttpContext.Session.GetInt32("ServiceId");


            if (loggedInUser != null)
            {
                using (var client = new HttpClient())
                {

                    _httpClient.BaseAddress = new Uri(_baseUrl);

                    _httpClient.DefaultRequestHeaders.Clear();
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await _httpClient.GetAsync($"api/GetLicencesById?id={ID}&serviceId={serviceId}");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonData = await response.Content.ReadAsStringAsync();

                        var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<LicenceDetailsVM>(jsonData);


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

    }



}
