using Business.Helpers;
using Business.ViewModel.Account;
using Business.ViewModel.Dynamic;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http;
using System.Reflection;
using System.Text;


namespace MOI_Eservice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly HelperUrlApi _helperUrlApi;
        private readonly string _baseUrl;


        public RolesController(HelperUrlApi helperUrlApi, IConfiguration configuration, HttpClient httpClient) 
        {
            _baseUrl = configuration["ApiSettings:BaseUrl"];

            _helperUrlApi = helperUrlApi;
        }
        #region Role
        //GetRoles
        public async Task<IActionResult> Index()     
        {
            var apiSettings = $"{_baseUrl}Roles/GetRoles";
            var RoleList = await _helperUrlApi.GetDataFromApi<List<RoleVMV>>(apiSettings);
            var apiSetting = $"{_baseUrl}Roles/";
            ViewBag.ApiBaseUrl = apiSetting;
            return View(RoleList);
        }
        //Add Roles
        public async Task<IActionResult> AddRole()
        {
            var apiSettings = $"{_baseUrl}Roles/AddRole";
            var RoleList = await _helperUrlApi.GetDataFromApi<RoleVM>(apiSettings);
            var apiSetting = $"{_baseUrl}Roles/";
            ViewBag.ApiBaseUrl = apiSetting;
            return View(RoleList);
        }
        //Get By Id Role
        public async Task<IActionResult> RoleDetails(int id)
        {
            var apiSettings = $"{_baseUrl}Roles/GetRoleById?id={id}";
            var RoleList = await _helperUrlApi.GetDataFromApi<RoleVMV>(apiSettings);
            var apiSetting = $"{_baseUrl}Roles/";
            ViewBag.ApiBaseUrl = apiSetting;
            return View(RoleList);
        }
        
        // Edit Roles
        public async Task<IActionResult> RoleEdit(int id)
        {
            var apiSettings = $"{_baseUrl}Roles/GetRoleById?id={id}";
            var RoleList = await _helperUrlApi.GetDataFromApi<RoleVMV>(apiSettings);
            //var apiSettings = $"{_baseUrl}Roles/GetRoles";
            //var RoleList = await _helperUrlApi.GetDataFromApi<List<RoleVMV>>(apiSettings);
            var apiSetting = $"{_baseUrl}Roles/";
            ViewBag.ApiBaseUrl = apiSetting;
            return View(RoleList);
        }

        #endregion
        #region Module
        public async Task<IActionResult> GetModules()
        {
            var apiSettings = $"{_baseUrl}Roles/GetModules";
            var RoleList = await _helperUrlApi.GetDataFromApi<List<ModuleVM>>(apiSettings);
            var apiSettingForView = $"{_baseUrl}Roles/";
            ViewBag.ApiBaseUrl = apiSettingForView;
            return View(RoleList);
        }

        [HttpGet]
        public IActionResult AddModule()
        {
            var apiSetting = $"{_baseUrl}Roles/";
            ViewBag.ApiBaseUrl = apiSetting;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditModule(int id)
        {
            var apiSettings = $"{_baseUrl}Roles/GetModule?id={id}";
            var ModuleList = await _helperUrlApi.GetDataFromApi<ModuleVM>(apiSettings);
            var apiSetting = $"{_baseUrl}Roles/";
            ViewBag.ApiBaseUrl = apiSetting;
            return View(ModuleList);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteModule(int id)
        {
            var apiSettings = $"{_baseUrl}Roles/GetModule?id={id}";
            var ModuleList = await _helperUrlApi.GetDataFromApi<ModuleVM>(apiSettings);
            var apiSettingForView = $"{_baseUrl}Roles/";
            ViewBag.ApiBaseUrl = apiSettingForView;
            return View(ModuleList);
        }
        #endregion
        
        #region MenuItem

        [HttpGet]
        public async Task<IActionResult> GetMenuItems()
        {
            var apiSettings = $"{_baseUrl}Roles/";
            ViewBag.ApiBaseUrl = apiSettings;

            // Fetch all data from the API
            var allData = await _helperUrlApi.GetDataFromApi<List<AddMenuItemVM>>($"{apiSettings}GetMenuItems");

            if (allData == null)
            {
                return View("Error");
            }

            return View(allData);
        }

        #endregion

        #region Permission
        [HttpGet]
        public async Task<IActionResult> GetPermissions()
        {
            var apiSettings = $"{_baseUrl}Roles/";
            ViewBag.ApiBaseUrl = apiSettings;

            // Fetch all data from the API
            var allData = await _helperUrlApi.GetDataFromApi<List<PermissionVM>>($"{apiSettings}GetPermissions");

            if (allData == null)
            {
                return View("Error");
            }

            return View(allData);
        }

        #endregion
    }
}
