using AutoMapper;
using Business.Helpers;
using Business.Interfaces;
using Business.ViewModel.Dynamic;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MOI_Eservice.Component
{
    public class CopyrightViewComponent : ViewComponent
    {
        private readonly HelperUrlApi _helperUrlApi;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public CopyrightViewComponent(IConfiguration configuration, HelperUrlApi helperUrlApi, HttpClient httpClient)
        {
            _helperUrlApi = helperUrlApi;
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var apiSettings = $"{_baseUrl}SystemOptions/GetAllOptions/";
            var model=await _helperUrlApi.GetDataFromApiNewHttpClient<List<SystemOptionVM>>(apiSettings);  

           
            return View("Default", model);
        }
    }
}

