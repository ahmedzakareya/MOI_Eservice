using Business.Helpers;
using Business.ViewModel;
using Business.ViewModel.HomePage;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;

namespace MOI_Eservice.Controllers
{
    //[Area("Home")]
    public class ChannelsController : Controller
    {
        private readonly ILogger<ChannelsController> _logger;
        private readonly HelperUrlApi _helperUrlApi;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public ChannelsController(IConfiguration configuration, ILogger<ChannelsController> logger, HelperUrlApi helperUrlApi, HttpClient httpClient)
        {
            _logger = logger;
            _helperUrlApi = helperUrlApi;
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];

        }
        public async Task<IActionResult> ChannelRepresentativeOfficeNewRequest()
        {
            MoiEserviceLicensesRequestVM model = new MoiEserviceLicensesRequestVM()
            {
                countriesLookups = await FetchCountriesAsync(),
                attachRules = await FetchAttachRulesAsync(33, 7, 1, 0)
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChannelRepresentativeOfficeNewRequest(MoiEserviceLicensesRequestVM model, List<int> AttachmentIds, List<string> FileNames)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                        .SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage)
                                        .ToList();

                return Json(new
                {
                    success = false,
                    message = "البيانات غير صالحة: " + string.Join(", ", errors)
                });
            }

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return Json(new
                {
                    success = false,
                    message = "يجب تسجيل الدخول قبل المتابعة"
                });
            }

            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                model.Requesterid = userId;
                var result = await AddNewChannelRequestAsync(model);

                if (result == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "فشل في حفظ البيانات"
                    });
                }

                return Json(new
                {
                    success = true,
                    message = "تم حفظ البيانات بنجاح",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "حدث خطأ أثناء حفظ البيانات: " + ex.Message
                });
            }
        }



        private async Task<MoiEserviceLicensesRequestVM> AddNewChannelRequestAsync(MoiEserviceLicensesRequestVM model)
        {
            try
            {
                string requestUrl = _baseUrl + "api/Channels/AddNewChannelRequest";

                var serializedData = JsonConvert.SerializeObject(model, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                var jsonContent = new StringContent(serializedData, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(requestUrl, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("API failed with status: " + response.StatusCode);
                    return null;
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<MoiEserviceLicensesRequestVM>(responseBody);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling AddNewChannelRequestAsync");
                return null;
            }
        }



        public async Task<IActionResult> AssociatedPresscCorrespondent()
        {
            MoiEserviceLicensesRequestVM model = new MoiEserviceLicensesRequestVM()
            {
                countriesLookups = await FetchCountriesAsync(),
                attachRules = await FetchAttachRulesAsync(34, 7, 1, 0)
            };
            return View(model);
        }

        private async Task<List<CountriesLookupVM>> FetchCountriesAsync()
        {
            try
            {
                var requestUrl = $"api/Channels/GetAllCountries";
                //HomePage/LicenseInfo?ID=176
                var response = await _helperUrlApi.GetDataFromApi<List<CountriesLookupVM>>(requestUrl);
                return response ?? new List<CountriesLookupVM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching license info.");
                return new List<CountriesLookupVM>();
            }
        }




        private async Task<List<AttachRuleVM>> FetchAttachRulesAsync(int activityTypeId, int serviceId, int requestTypeId, int requestStatusId)
        {
            try
            {
                var requestUrl = $"api/Channels/GetAttchRule?ActivityTypeId={activityTypeId}&ServiceId={serviceId}&RequestTypeId={requestTypeId}&RequestStatusId={requestStatusId}";
                var response = await _helperUrlApi.GetDataFromApi<List<AttachRuleVM>>(requestUrl);
                return response ?? new List<AttachRuleVM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching attachment rules.");
                return new List<AttachRuleVM>();
            }
        }

    }
}
