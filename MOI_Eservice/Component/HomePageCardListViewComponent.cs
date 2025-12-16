using Business.Helpers;
using Business.ViewModel.HomePage;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace MOI_Eservice.Component
{
    public class HomePageCardListViewComponent : ViewComponent
    {
        private readonly HelperUrlApi _helperUrlApi;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public HomePageCardListViewComponent(IConfiguration configuration, HelperUrlApi helperUrlApi, HttpClient httpClient)
        {
            _helperUrlApi = helperUrlApi;
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var apiSettings = $"{_baseUrl}HomePage/";
            // Fetch data concurrently to reduce waiting time
            var task1 = _httpClient.GetAsync($"{apiSettings}GetActivityTypes");
            var task2 = _httpClient.GetAsync($"{apiSettings}GetLicencesInfos");
            var task3 = _httpClient.GetAsync($"{apiSettings}servicesWithCondition");

            // Wait for all tasks to complete
            await Task.WhenAll(task1, task2, task3);

            // Use JsonSerializer or JsonConvert to deserialize the data from response
            var activityTypes = await task1.Result.Content.ReadAsStringAsync();
            var eServiceLicenseInfo = await task2.Result.Content.ReadAsStringAsync();
            var eservices = await task3.Result.Content.ReadAsStringAsync();

            // Deserialize JSON data to your models
            var activityTypesList = JsonConvert.DeserializeObject<List<EserviceActvityTypeModel>>(activityTypes);
            var eServiceLicenseInfoList = JsonConvert.DeserializeObject<List<MoiEserviceLicenseInfo>>(eServiceLicenseInfo);
            var eservicesList = JsonConvert.DeserializeObject<List<EserviceViewModel>>(eservices);

            // Add the data to ViewData to pass it to the view
            ViewData["EservicesList"] = eservicesList;
            var jsonResponse = await _httpClient.GetStringAsync($"{apiSettings}GetServiceBranchTypes");
            var eserviceTypeBranches = JsonConvert.DeserializeObject<List<EserviceTypeBranchModel>>(jsonResponse);
            ViewData["EserviceTypeBranchesList"] = eserviceTypeBranches;
            ViewData["LicenceInfoList"] = eServiceLicenseInfoList;
            ViewData["ActvityTypesList"] = activityTypesList;
          
            var viewModel = new HomePageViewModel
            {
                ActvityTypes = JsonConvert.DeserializeObject<List<EserviceActvityTypeModel>>(activityTypes),
                LicenceInfoList = JsonConvert.DeserializeObject<List<MoiEserviceLicenseInfo>>(eServiceLicenseInfo),
                Eservices = JsonConvert.DeserializeObject<List<EserviceViewModel>>(eservices),
                EserviceTypeBranches = JsonConvert.DeserializeObject<List<EserviceTypeBranchModel>>(jsonResponse)
            };

            // Return the model to the view
            return View(viewModel);

            
        }
    }
}
