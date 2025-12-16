using Business.Helpers;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Configuration;

namespace MOI_Eservice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AttachForFrontDynamicController : Controller
    {
        private readonly HelperUrlApi _helperUrlApi;
        private readonly string _baseUrl;

        // Inject HelperUrlApi into the constructor
        public AttachForFrontDynamicController(IConfiguration configuration,HelperUrlApi helperUrlApi)
        {
            _helperUrlApi = helperUrlApi;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
        }

        // GET: Admin/FileUploadConfigurations
        public async Task<IActionResult> Index()
        {
            var apisettings = _baseUrl + $"api/AttachDynamicAdmin/GetFileUploadConfigurations";
            var fileConfigs = await _helperUrlApi.GetDataFromApi<List<FileUploadConfigurationsFront>>(apisettings);

            if (fileConfigs == null)
            {
                return View(new List<FileUploadConfigurationsFront>()); 
            }

            return View(fileConfigs); 
        }

        // GET: Admin/FileUploadConfigurations/Create
        public async Task<IActionResult> Create()
        {
            var statusApiUrl = _baseUrl + "Dynamic/GetRequestStatus";
            var statusList = await _helperUrlApi.GetDataFromApi<List<RequestStatusLookup>>(statusApiUrl);
            ViewBag.StatusList = statusList.Select(rs => new SelectListItem
            {
                Text = rs.NameAr,       // ✅ Correct property
                Value = rs.Id.ToString()
            }).ToList();
            return View();
        }

        // POST: Admin/FileUploadConfigurations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FileUploadConfigurationsFront model)
        {
            if (ModelState.IsValid)
            {
                var apisettings = _baseUrl + $"api/AttachDynamicAdmin/PostFileUploadConfig";
                var response = await _helperUrlApi.PostDataToApi<FileUploadConfigurationsFront, FileUploadConfigurationsFront>(apisettings, model);

                if (response != null)
                {
                    return RedirectToAction(nameof(Index)); // Redirect to Index on successful creation
                }

                ModelState.AddModelError("", "Unable to save the configuration.");
            }

            return View(model);
        }

        // GET: Admin/FileUploadConfigurations/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var apisettings = _baseUrl + $"api/AttachDynamicAdmin/GetFileUploadConfig";

            var fileConfig = await _helperUrlApi.GetDataFromApi<FileUploadConfigurationsFront>($"{apisettings}/{id}");
            var statusApi = _baseUrl + "Dynamic/GetRequestStatus";
            var requestStatuses = await _helperUrlApi.GetDataFromApiNewHttpClient<List<RequestStatusLookup>>(statusApi);
            ViewBag.requestStatuses = requestStatuses.Select(rs => new SelectListItem
            {
                Text = rs.NameAr,       // ✅ Correct property
                Value = rs.Id.ToString()
            }).ToList();
            if (fileConfig == null)
            {
                return NotFound();
            }

            return View(fileConfig); // Return the file configuration to be edited
        }

        // POST: Admin/FileUploadConfigurations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, FileUploadConfigurationsFront model)
        {
            var apisettings = _baseUrl + $"api/AttachDynamicAdmin/PutFileUploadConfig";

            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var response = await _helperUrlApi.PostDataToApi<FileUploadConfigurationsFront, FileUploadConfigurationsFront>(apisettings, model);

                if (response != null)
                {
                    return RedirectToAction("Index"); // Redirect to Index on successful update
                }

                ModelState.AddModelError("", "Unable to update the configuration.");
            }

            return View(model);
        }

        // GET: Admin/FileUploadConfigurations/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var apisettings = _baseUrl + $"api/AttachDynamicAdmin/GetFileUploadConfig";

            var fileConfig = await _helperUrlApi.GetDataFromApi<FileUploadConfigurationsFront>($"{apisettings}/{id}");

            if (fileConfig == null)
            {
                return NotFound();
            }

            return View(fileConfig); // Show confirmation page
        }

        // POST: Admin/FileUploadConfigurations/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var apisettings = _baseUrl + $"api/AttachDynamicAdmin/DeleteFileUploadConfig";

            var isDeleted = await _helperUrlApi.DeleteDataFromApi<FileUploadConfigurationsFront>($"{apisettings}/{id}");

            if (isDeleted)
            {
                return RedirectToAction(nameof(Index)); // Redirect to Index on successful delete
            }

            ModelState.AddModelError("", "Unable to delete the configuration.");
            return RedirectToAction(nameof(Index)); // If failed, redirect to Index
        }
        // GET: Admin/FileUploadConfigurations/Copy/5
        [HttpPost]
        public async Task<IActionResult> CloneConfig(int id)
        {
            var getUrl = $"{_baseUrl}api/AttachDynamicAdmin/GetFileUploadConfig/{id}";
            var config = await _helperUrlApi.GetDataFromApi<FileUploadConfigurationsFront>(getUrl);

            if (config == null)
            {
                TempData["Error"] = "فشل في تحميل الإعداد لنسخه";
                return RedirectToAction("Index");
            }

            // Create a new object with Id = 0 and optionally modify the label
            var newConfig = new FileUploadConfigurationsFront
            {
                Id = 0,
                Label = config.Label ,
                ViewType = config.ViewType,
                MaxFileSize = config.MaxFileSize,
                AllowedFileTypes = config.AllowedFileTypes,
                IsRequired = config.IsRequired,
                //Order = config.Order,
                FieldName = config.FieldName,
                // ...add any other fields that should be copied
            };

            var postUrl = $"{_baseUrl}api/AttachDynamicAdmin/PostFileUploadConfig";
            var response = await _helperUrlApi.PostDataToApi<FileUploadConfigurationsFront, FileUploadConfigurationsFront>(postUrl, newConfig);

            if (response == null)
            {
                TempData["Error"] = "فشل في إنشاء نسخة جديدة";
            }
            else
            {
                TempData["Success"] = "تم إنشاء نسخة جديدة بنجاح";
            }

            return RedirectToAction("Index");
        }


    }
}
