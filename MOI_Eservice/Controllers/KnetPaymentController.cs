using Business.Helpers;
using Business.ViewModel;
using Business.ViewModel.Tourism;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Data;
using System.ServiceModel;

namespace MOI_Eservice.Controllers
{
    public class KnetPaymentController : Controller
    {
        private readonly string _baseUrl;
        private readonly IConfiguration _config;
        private readonly HelperUrlApi _helperUrlApi;
        private readonly IWebHostEnvironment _env;

        public KnetPaymentController(IConfiguration configuration, HelperUrlApi helperUrlApi, IWebHostEnvironment env)
        {
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _config = configuration;
            _helperUrlApi = helperUrlApi;
            _env = env;
        }
        public IActionResult Index() => View();

        #region KnetCancel
        public async Task<ActionResult> KnetCancel(string RequestID)
        {
            try
            {
                ViewBag.ManagmentName = string.Empty;

                // Get user session data
                var token = HttpContext.Session.GetString("UserToken");
                var userId = HttpContext.Session.GetString("UserId");
                var userName = HttpContext.Session.GetString("UserUserName");
                var civilId = HttpContext.Session.GetString("UserCivilId");
                var fullName = HttpContext.Session.GetString("UserFullName");
                var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");

                // Initialize ePay SOAP client with binding & endpoint
                var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                var endpoint = new EndpointAddress(_config["PaymentGateway:EpayWsdl"]);
                var ePayService = new ePaySoapClient(binding, endpoint);

                // Get token
                string merchantToken = await ePayService.GetePayTokenAsync(
                    _config["PaymentGateway:EpayTourismUsername"],
                    _config["PaymentGateway:EpayTourismPassword"]
                );

                if (merchantToken == "0")
                {
                    ViewBag.Message = "فشل في الحصول على التوكن من بوابة الدفع";
                    return View();
                }


                var dataTable = await ePayService.GetePayPaymentDetailsAsync(RequestID); ;
                string output = JsonConvert.SerializeObject(dataTable);
                var paymentResponses = JsonConvert.DeserializeObject<List<PaymentResponse>>(output);
                var paymentResponse = paymentResponses?.FirstOrDefault();

                // Decode ReqID from encrypted string
                if (int.TryParse(MyCrypto.Decode(RequestID), out int reqId))
                {
                    string apiUrl = _baseUrl + $"api/PaymentFront/GetRequestDetails/{reqId}";

                    var response = await _helperUrlApi.GetDataFromApi<RequestFrontVM>(apiUrl);
                    return View(response);
                }



                return View(paymentResponse);
            }
            catch (Exception ex)
            {
                LogObject(ex, "KNetResultException");
                throw ex;
            }
        }
        #endregion 

        #region KnetResult
        public async Task<ActionResult> KnetResult(string RequestID)
        {
            try
            {
                ViewBag.ManagmentName = string.Empty;

                // Get user session data
                var token = HttpContext.Session.GetString("UserToken");
                var userId = HttpContext.Session.GetString("UserId");
                var userName = HttpContext.Session.GetString("UserUserName");
                var civilId = HttpContext.Session.GetString("UserCivilId");
                var fullName = HttpContext.Session.GetString("UserFullName");
                var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");

                // Initialize ePay SOAP client with binding & endpoint
                var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                var endpoint = new EndpointAddress(_config["PaymentGateway:EpayWsdl"]);
                var ePayService = new ePaySoapClient(binding, endpoint);

                // Get token
                string merchantToken = await ePayService.GetePayTokenAsync(
                    _config["PaymentGateway:EpayTourismUsername"],
                    _config["PaymentGateway:EpayTourismPassword"]
                );

                if (merchantToken == "0")
                {
                    ViewBag.Message = "فشل في الحصول على التوكن من بوابة الدفع";
                    return View();
                }
                var dataTable = await ePayService.GetePayPaymentDetailsAsync(RequestID); ;

                string output = JsonConvert.SerializeObject(dataTable);
                var paymentResponses = JsonConvert.DeserializeObject<List<PaymentResponse>>(output);
                var paymentResponse = paymentResponses?.FirstOrDefault();

                // Decode ReqID from encrypted string
                if (int.TryParse(MyCrypto.Decode(RequestID), out int reqId))
                {
                    string apiUrl = _baseUrl + $"api/PaymentFront/GetRequestDetails/{reqId}";

                    var response = await _helperUrlApi.GetDataFromApi<RequestFrontVM>(apiUrl);
                    return View(response);
                }




                return View(paymentResponse);
            }
            catch (Exception ex)
            {
                LogObject(ex, "KNetResultException");
                throw ex;
            }

        }
        #endregion

        #region KnetError
        public async Task<ActionResult> KnetError(string RequestID)
        {
            try
            {
                ViewBag.ManagmentName = string.Empty;

                // Get user session data
                var token = HttpContext.Session.GetString("UserToken");
                var userId = HttpContext.Session.GetString("UserId");
                var userName = HttpContext.Session.GetString("UserUserName");
                var civilId = HttpContext.Session.GetString("UserCivilId");
                var fullName = HttpContext.Session.GetString("UserFullName");
                var accountTypeId = HttpContext.Session.GetString("UserAccouuntTypeId");

                // Initialize ePay SOAP client with binding & endpoint
                var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
                var endpoint = new EndpointAddress(_config["PaymentGateway:EpayWsdl"]);
                var ePayService = new ePaySoapClient(binding, endpoint);

                // Get token
                string merchantToken = await ePayService.GetePayTokenAsync(
                    _config["PaymentGateway:EpayTourismUsername"],
                    _config["PaymentGateway:EpayTourismPassword"]
                );

                if (merchantToken == "0")
                {
                    ViewBag.Message = "فشل في الحصول على التوكن من بوابة الدفع";
                    return View();
                }

                // Fetch payment details
                var dataTable = await ePayService.GetePayPaymentDetailsAsync(RequestID);
                string output = JsonConvert.SerializeObject(dataTable);
                var paymentResponses = JsonConvert.DeserializeObject<List<PaymentResponse>>(output);
                var paymentResponse = paymentResponses?.FirstOrDefault();

                // Decode ReqID from encrypted string
                if (int.TryParse(MyCrypto.Decode(RequestID), out int reqId))
                {
                    string apiUrl = _baseUrl + $"api/PaymentFront/GetRequestDetails/{reqId}";

                    var response = await _helperUrlApi.GetDataFromApi<RequestFrontVM>(apiUrl);
                    return View(response);
                }

                // If decoding fails or no request found
                return View(paymentResponse);
            }
            catch (Exception ex)
            {
                LogObject(ex, "KNetResultException");
                ViewBag.Message = "حدث خطأ أثناء تحميل بيانات الدفع.";
                return View();
            }
        }
        #endregion

        #region Helper
        public void LogObject(object inputItemModel, string fileStartName)
        {
            string fileName = $"{fileStartName}_{DateTime.Now.Ticks}.json";

            // Save under wwwroot/Attachments/YYYY/MM/DD/
            string basePath = Path.Combine(_env.WebRootPath, "Attachments",
                DateTime.Now.Year.ToString(),
                DateTime.Now.Month.ToString("D2"),
                DateTime.Now.Day.ToString("D2"));

            if (!Directory.Exists(basePath))
                Directory.CreateDirectory(basePath);

            string output = JsonConvert.SerializeObject(inputItemModel, Formatting.Indented);
            string fullPath = Path.Combine(basePath, fileName);

            System.IO.File.WriteAllText(fullPath, output);
        }
        private async Task<bool> UpdatePaymentStatusAsync(PaymentResponse paymentResponse)
        {
            try
            {
                string apiUrl = _baseUrl + "api/PaymentFront/UpdatePayment";

                var updateResult = await _helperUrlApi.PostDataToApi<PaymentResponse, ErrorMessage>(apiUrl, paymentResponse);

                if (updateResult != null && !updateResult.Error)
                {
                    LogObject(updateResult, "KNetUpdateSuccess_");
                    return true;
                }

                LogObject(updateResult, "KNetUpdateFailed_");
                return false;
            }
            catch (Exception ex)
            {
                LogObject(ex, "KNetUpdateException_");
                return false;
            }
        }

        #endregion
    }
}
