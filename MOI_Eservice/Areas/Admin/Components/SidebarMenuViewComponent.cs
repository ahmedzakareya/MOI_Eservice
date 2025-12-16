using Business.ViewModel.Account;
using Business.ViewModel.Dynamic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MOI_Eservice.Areas.Admin.Components
{
    public class SidebarMenuViewComponent : ViewComponent
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _baseUrl;


        public SidebarMenuViewComponent(IConfiguration configuration, IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _baseUrl = configuration["ApiSettings:BaseUrl"];

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            string token = HttpContext.Session.GetString("Token");

            if (string.IsNullOrEmpty(token))
            {
                // Return an empty menu if token is missing
                return View(new List<RolePermissionVM>());
            }

            try
            {
                var client = _clientFactory.CreateClient();
                client.BaseAddress = new Uri(_baseUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var fullUrl = new Uri(client.BaseAddress, "Dynamic/GetDynamicMenuItems");
                Console.WriteLine("Full API URL: " + fullUrl);

                // Fetch menu items from API
                var response = await client.GetAsync("Dynamic/GetDynamicMenuItems");

                if (response.IsSuccessStatusCode)
                {
                    var menuJson = await response.Content.ReadAsStringAsync();
                    var modulesWithMenuItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ModuleVM>>(menuJson);
                    return View(modulesWithMenuItems);
                }
                else
                {
                    Console.WriteLine($"Failed to fetch menu items: {response.StatusCode}");
                    return View(new List<RolePermissionVM>());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return View(new List<RolePermissionVM>());
            }
        }
    }
}
