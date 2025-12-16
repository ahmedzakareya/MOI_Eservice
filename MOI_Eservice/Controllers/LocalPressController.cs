using Business.Helpers;
using Business.ViewModel;
using Business.ViewModel.Dynamic;
using Microsoft.AspNetCore.Mvc;

namespace MOI_Eservice.Controllers
{
    public class LocalPressController : Controller
    {
        private readonly ILogger<LocalPressController> _logger;
        private readonly HelperUrlApi _helperUrlApi;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _configuration;
        public LocalPressController(IConfiguration configuration, ILogger<LocalPressController> logger, HelperUrlApi helperUrlApi, HttpClient httpClient)
        {
            _logger = logger;
            _helperUrlApi = helperUrlApi;
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _configuration = configuration;

        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateNewDailyRequest()
        {
            MoiEserviceLicensesRequestVM model = new MoiEserviceLicensesRequestVM()
            {

                countriesLookups = await FetchCountriesAsync(),
                attachRules = await FetchAttachmentsAsync("NewDailyRequest"),
                ActivityTypes = await FetchActivitiesAsync(3),
                licenceTypesLookupVMs =await FetchLicenseTypeAsync(),
                
            };
            return View(model);
        }


        public async Task<IActionResult> CreatePeriodicalRequest()
        {
            MoiEserviceLicensesRequestVM model = new MoiEserviceLicensesRequestVM()
            {

                countriesLookups = await FetchCountriesAsync(),
                attachRules = await FetchAttachmentsAsync("NewPeriodicalRequest"),
                ActivityTypes = await FetchActivitiesAsync(3),
                licenceTypesLookupVMs = await FetchLicenseTypeAsync(),
                scheduleReleaseTypesVMs = await FetchScheduleReleaseTypesAsync()

            };
            return View(model);
        }

        public async Task<IActionResult> CreateCertificateRequest()
        {
            MoiEserviceLicensesRequestVM model = new MoiEserviceLicensesRequestVM()
            {

                countriesLookups = await FetchCountriesAsync(),
                attachRules = await FetchAttachmentsAsync("NewPeriodicalRequest"),
                ActivityTypes = await FetchActivitiesAsync(3),
                licenceTypesLookupVMs = await FetchLicenseTypeAsync(),
                scheduleReleaseTypesVMs = await FetchScheduleReleaseTypesAsync()

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


        private async Task<List<Business.ViewModel.AttachRuleVM>> FetchAttachmentsAsync(string viewType)
        {
            try
            {
                var requestUrl = $"api/LocalPress/GetAttachmentForRequest?viewType={viewType}";

                var response = await _helperUrlApi.GetDataFromApi<List<Business.ViewModel.AttachRuleVM>>(requestUrl);
                return response ?? new List<Business.ViewModel.AttachRuleVM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching attachment rules.");
                return new List<Business.ViewModel.AttachRuleVM>();
            }
        }
        private async Task<List<ActivityTypeVM>> FetchActivitiesAsync(int id)
        {
            try
            {
                var requestUrl = $"api/LocalPress/GetActivity?ID={id}";

                var response = await _helperUrlApi.GetDataFromApi<List<ActivityTypeVM>>(requestUrl);
                return response ?? new List<ActivityTypeVM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching activity info.");
                return new List<ActivityTypeVM>();
            }
        }
        private async Task<List<LicenceTypesLookupVM>> FetchLicenseTypeAsync()
        {
            try
            {
                var requestUrl = $"api/LocalPress/GetLicensesType";

                var response = await _helperUrlApi.GetDataFromApi<List<LicenceTypesLookupVM>>(requestUrl);
                return response ?? new List<LicenceTypesLookupVM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching activity info.");
                return new List<LicenceTypesLookupVM>();
            }
        }
        private async Task<List<ScheduleReleaseTypesVM>> FetchScheduleReleaseTypesAsync()
        {
            try
            {
                var requestUrl = $"api/LocalPress/GetScheduleReleaseTypes";

                var response = await _helperUrlApi.GetDataFromApi<List<ScheduleReleaseTypesVM>>(requestUrl);
                return response ?? new List<ScheduleReleaseTypesVM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching activity info.");
                return new List<ScheduleReleaseTypesVM>();
            }
        }


    }
}
