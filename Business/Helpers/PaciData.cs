//using Business.ViewModel;
//using Microsoft.AspNetCore.Http;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;
//using Microsoft.Extensions.Configuration;
//using Microsoft.AspNetCore.Mvc;



//namespace Business.Helpers
//{
//    public class PaciData
//    {

//        public string attpath;
//        public static string token;
//        public static string statusCode;
//        public static string _baseUrl;
//        public static string _PaciapiUrl;
//        public static string _PaciInfoApiUrl;
//        public static string _PaciPassword;
//        public static string _PaciUsername;
//        public static string eserviceToken;
//        private readonly HttpClient _httpClient;
//        private readonly string _file;
//        private readonly HelperUrlApi _helperUrlApi;
      
//        public IHttpContextAccessor _httpContextAccessor;


//        public PaciData(IConfiguration configuration
//            , HttpClient httpClient, HelperUrlApi helperUrlApi, IHttpContextAccessor httpContextAccessor)
//        {
//            _baseUrl = configuration["ApiSettings:BaseUrl"];
//            _PaciapiUrl = configuration["PaciData:PaciAPI"];
//            _PaciInfoApiUrl = configuration["PaciData:PaciInfoAPI"];
//            _PaciUsername = configuration["PaciData:PaciAPIUserName"];
//            _PaciPassword = configuration["PaciData:PaciAPIPassword"];
//            eserviceToken = configuration["EserviceId"];
//            _file = configuration["Path:ResetPasswordRequestImage"];
//            _httpClient = httpClient;
//            _helperUrlApi = helperUrlApi;
           
//            _httpContextAccessor = httpContextAccessor;

//        }

//        #region Get Paci Data By CivilId
//        public string gettoken_Paci()
//        {
//            try
//            {

//                var config = new ClientConfigVM()
//                {
//                    grant_type = "password",
//                    username = _PaciUsername,
//                    password = _PaciPassword
//                };



//                var client = new RestClient(_PaciapiUrl + "GetToken");


//                var request = new RestRequest();
//                request.Method = RestSharp.Method.Post;
//                request.AddHeader("content-type", "application/x-www-form-urlencoded");

//                request.AddParameter("application/x-www-form-urlencoded", "grant_type=password&Username=" + config.username + "&Password=" + config.password, ParameterType.RequestBody);
//                var response = client.Execute(request);

//                string ysysy = response.ResponseStatus.ToString();
//                JObject jObject = JsonConvert.DeserializeObject<dynamic>(response.Content);
//                PaciTokenResultVM paciTokenResult = new PaciTokenResultVM();
//                paciTokenResult = JsonConvert.DeserializeObject<PaciTokenResultVM>(response.Content.ToString());
//                token = paciTokenResult.access_token;

//                return token;
//            }
//            catch (Exception ex)
//            {
//                return "Error : " + ex.Message;
//            }

//        }

//        public string gettoken_PaciInfo(string _token, string civilID)
//        {

//            var client = new RestClient(_PaciInfoApiUrl + "PACI/GetPersonDetails");


//            var request = new RestRequest();
//            request.Method = RestSharp.Method.Post;
//            request.AddHeader("content-type", "application/x-www-form-urlencoded");
//            request.AddHeader("Authorization", "Bearer " + _token);
//            request.AddParameter("application/x-www-form-urlencoded", "civilId= " + civilID, ParameterType.RequestBody);
//            var response = client.Execute(request);

//            string responsStatues = response.ResponseStatus.ToString();
//            JObject jObject = JsonConvert.DeserializeObject<dynamic>(response.Content);

//            if (responsStatues == "Completed")
//            {
//                PaciDataVM PaciData = new PaciDataVM();

//                PaciData.statusCode = jObject.Value<string>("statusCode").ToString();
//                statusCode = PaciData.statusCode;

//                return statusCode;
//            }
//            else
//            {
//                statusCode = "error code";
//                return statusCode;
//            }


//        }

//        public PaciDataVM Get_PACI_User_Info(string _token2, string civilID)
//        {
//            PaciDataVM PaciData = new PaciDataVM();

//            string keyapi = _PaciInfoApiUrl;
//            var client = new RestClient(keyapi + "PACI/GetPersonDetails");


//            var request = new RestRequest();
//            request.Method = RestSharp.Method.Post;
//            request.AddHeader("content-type", "application/x-www-form-urlencoded");
//            request.AddHeader("Authorization", "Bearer " + _token2);
//            request.AddParameter("application/x-www-form-urlencoded", "civilId= " + civilID, ParameterType.RequestBody);
//            var response = client.Execute(request);

//            string responsStatues = response.ResponseStatus.ToString();
//            JObject jObject = JsonConvert.DeserializeObject<dynamic>(response.Content);

//            string statusCode = "";
//            statusCode = jObject.Value<string>("statusCode").ToString();
//            if (responsStatues == "Completed")
//            {
//                if (statusCode == "900")
//                {
//                    PaciData.arFullName = jObject.Value<string>("arFullName").ToString();
//                    PaciData.enFullName = jObject.Value<string>("enFullName").ToString();
//                    PaciData.birthDate = jObject.Value<string>("birthDate").ToString();
//                    PaciData.email = jObject.Value<string>("email").ToString();
//                    PaciData.mobile = jObject.Value<string>("mobile").ToString();
//                    PaciData.sex = jObject.Value<string>("sex").ToString();
//                    PaciData.statusCode = jObject.Value<string>("statusCode").ToString();
//                    PaciData.disclaimer = jObject.Value<string>("disclaimer").ToString();
//                    PaciData.remainingHits = jObject.Value<string>("remainingHits").ToString();
//                    PaciData.timeStamp = jObject.Value<string>("timeStamp").ToString();
//                    PaciData.message = jObject.Value<string>("message").ToString();
//                    PaciData.environment = jObject.Value<string>("environment").ToString();
//                }
//                else
//                {
//                    PaciData.statusCode = jObject.Value<string>("statusCode").ToString();
//                    PaciData.disclaimer = jObject.Value<string>("disclaimer").ToString();
//                    PaciData.remainingHits = jObject.Value<string>("remainingHits").ToString();
//                    PaciData.timeStamp = jObject.Value<string>("timeStamp").ToString();
//                    PaciData.message = jObject.Value<string>("message").ToString();
//                    PaciData.environment = jObject.Value<string>("environment").ToString();
//                }
//            }
//            return PaciData;
//        }

//        public JsonResult GetUserDataPACI(string CivilID)
//        {
//            token = gettoken_Paci();
//            statusCode = gettoken_PaciInfo(token, CivilID);

//            PaciDataVM PACIUserData = new PaciDataVM();
//            var result = new { success = false, message = "", data = PACIUserData }; // Default response

//            if (statusCode == "900")
//            {
//                PACIUserData = Get_PACI_User_Info(token, CivilID);
//                result = new { success = true, message = "", data = PACIUserData }; // Success response
//            }
//            else
//            {
//                result = new { success = false, message = "الرقم المدني يجب أن يكون لكويتي", data = PACIUserData }; // Error message
//            }

//            return Json(result);

//        }
//        #endregion
//    }
//}
