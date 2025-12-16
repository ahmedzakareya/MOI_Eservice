using Business.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NuGet.Common;
using RestSharp;
using System.Net.Http.Headers;
using System.Net.Http;
using Business.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Configuration;
using Business.ViewModel.Account;


namespace MOI_Eservice.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class UserController : Controller
    {
        private readonly HttpClient _httpClient;

        private readonly string _baseUrl;
        private readonly string _PaciapiUrl;
        private readonly string _PaciInfoApiUrl;
        private readonly string _PaciPassword;
        private readonly string _PaciUsername;
        private readonly string _hierarchy;
        public string token;
        public string statusCode;
        public UserController(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;


            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _PaciapiUrl = configuration["PaciData:PaciAPI"];
            _PaciInfoApiUrl = configuration["PaciData:PaciInfoAPI"];
            _PaciUsername = configuration["PaciData:PaciAPIUserName"];
            _PaciPassword = configuration["PaciData:PaciAPIPassword"];
            _hierarchy = configuration["Hierarachy:MoiInfoHierarchyURL"];
        }
        public async Task<IActionResult> Index()
        {
            var apiSettings = _baseUrl + $"Userapi/";
            ViewBag.ApiBaseUrl = apiSettings;
            _httpClient.BaseAddress = new Uri(_baseUrl);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var response = await _httpClient.GetAsync(_baseUrl + $"Userapi/GetUserWithAllPermission");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MoiEserviceSysUserVM>>(jsonData);

                return View(responseData);
            }
            else
            {

                return RedirectToAction("ErrorPage", "Home");
            }
            return View();
        }
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
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
        [HttpGet("GetUserDataPACI/{civilId}")]
        public JsonResult GetUserDataPACI(string CivilID)
        {
            token = gettoken_Paci();
            statusCode = gettoken_PaciInfo(token, CivilID);

            PaciDataVM PACIUserData = new PaciDataVM();

            if (statusCode == "900")
            {

                PACIUserData = Get_PACI_User_Info(token, CivilID);
            }
            return Json(PACIUserData);

        }
        #endregion
        public async Task<ActionResult> AllUserWithPermission()
        {



            var loggedInUser = HttpContext.Session.GetString("AdminUserData");
            dynamic userData = System.Text.Json.JsonSerializer.Deserialize<dynamic>(loggedInUser);
            //int serviceId = userData.GetProperty("ServiceId").GetInt32();

            if (loggedInUser != null)
            {


                // Access the ServiceId property
                //var serviceId = userData.ServiceId;

                _httpClient.BaseAddress = new Uri(_baseUrl);

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var requestUrl = _baseUrl + $"api/GetUserWithPermission";

                var response = await _httpClient.GetAsync($"api/GetUserWithPermission");

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
        public async Task<ActionResult> AllRoleWithPermission()
        {

            var loggedInUser = HttpContext.Session.GetString("AdminUserData");
            dynamic userData = System.Text.Json.JsonSerializer.Deserialize<dynamic>(loggedInUser);
            //int serviceId = userData.GetProperty("ServiceId").GetInt32();

            if (loggedInUser != null)
            {


                // Access the ServiceId property
                //var serviceId = userData.ServiceId;

                _httpClient.BaseAddress = new Uri(_baseUrl);

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var requestUrl = _baseUrl + $"api/GetRoleWithPermission";

                var response = await _httpClient.GetAsync($"api/GetRoleWithPermission");

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


  

        //Get Sector from my Api
        private async Task<List<SelectListItem>> GetSectorsSelectListAsync()
        {
            // Call your API endpoint to get sectors

            _httpClient.BaseAddress = new Uri(_baseUrl);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

           
            var response = await _httpClient.GetAsync(_baseUrl+$"Userapi/GetSectors");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var sectorSelectList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SelectListItem>>(jsonData);

               
                return sectorSelectList;

              

               
            }
            else
            {
                // Handle the error accordingly (you can return an empty list or throw an exception)
                return new List<SelectListItem>();
            }
        }
        public async Task<IActionResult> CreateUser()
        {
            var apiSettings = _baseUrl + $"Userapi/";
            ViewBag.ApiBaseUrl = apiSettings; // Pass the API base URL to the view

            var sectors = await GetSectorsSelectListAsync();

            var model = new UserVM
            {
                Sectors = sectors // Assuming you have a property to hold departments
            };

            return View(model);
        }
        public async Task<IActionResult> ViewUser(int id)
        {
            var apiSettings = _baseUrl + $"Userapi/";
            ViewBag.ApiBaseUrl = apiSettings;

            _httpClient.BaseAddress = new Uri(_baseUrl);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var response = await _httpClient.GetAsync(_baseUrl + $"Userapi/GetSpecificUserWithAllPermission?id={id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<UserVM>(jsonData);

                return View(responseData);
            }
            else
            {

                return RedirectToAction("ErrorPage", "Home");
            }
          
        }

        public async Task<IActionResult> EditUser(int id)
        {
            var apiSettings = _baseUrl + $"Userapi/";
            ViewBag.ApiBaseUrl = apiSettings;

            _httpClient.BaseAddress = new Uri(_baseUrl);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var response = await _httpClient.GetAsync(_baseUrl + $"Userapi/Edit?id={id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<UserVM>(jsonData);

                return View(responseData);
            }
            else
            {

                return RedirectToAction("ErrorPage", "Home");
            }

        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            var apiSettings = _baseUrl + $"Userapi/";
            ViewBag.ApiBaseUrl = apiSettings;

            _httpClient.BaseAddress = new Uri(_baseUrl);

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


            var response = await _httpClient.GetAsync(_baseUrl + $"Userapi/Delete?id={id}");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var responseData = Newtonsoft.Json.JsonConvert.DeserializeObject<UserVM>(jsonData);

                return View(responseData);
            }
            else
            {

                return RedirectToAction("ErrorPage", "Home");
            }

        }




    }
}
