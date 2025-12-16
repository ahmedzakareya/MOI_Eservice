using Business.Helpers;
using Business.ViewModel.Dynamic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace MOI_Eservice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SystemOptionController : Controller
    {
        private readonly HelperUrlApi _helperUrlApi;
        private readonly string _baseUrl;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public SystemOptionController(IConfiguration configuration, HelperUrlApi helperUrlApi, IWebHostEnvironment webHostEnvironment)
        {
            _helperUrlApi = helperUrlApi;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var apiSettings = $"{_baseUrl}SystemOptions/";
            ViewBag.ApiBaseUrl = apiSettings;
            var requestUrl = $"SystemOptions/GetAllOptions";
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var response = await _helperUrlApi.GetDataFromApi<List<SystemOptionVM>>(requestUrl);

            return View(response);
        }
        // CREATE: Display the create form
        public IActionResult CreateSystemOptions()
        {
            var model = new SystemOptionVM
            {
              
                IsActive = true ,
                IsDeleted=false,
                IsHidden=false,
                IsReadOnly = false  
            };
            return View(model);
        }

       

        // CREATE: Post data to the API to create a new SystemOption
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSystemOptions(SystemOptionVM model, IFormFile FileUpload)
        {
            if (ModelState.IsValid)
            {
                if (model.ControlType == "Image" && FileUpload != null && FileUpload.Length > 0)
                {
                    // Set folder to assets/images (under wwwroot)
                    string folderName = "assets/images";
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, folderName);

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);

                    // Generate a unique name
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(FileUpload.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Save the file to wwwroot/assets/images
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await FileUpload.CopyToAsync(stream);
                    }

                    // Store relative path to use in <img src="@Url.Content(...)">
                    model.Value = $"/{folderName}/{uniqueFileName}";
                }
                var requestUrl = $"{_baseUrl}SystemOptions/HandlePost";
                var response = await _helperUrlApi.PostDataToApi<SystemOptionVM, SystemOptionVM>(requestUrl, model,"create");

                if (response != null)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Error creating system option.");
            }

            return View(model);
        }

        // EDIT: Display edit form
        public async Task<IActionResult> EditSystemOptions(int id)
        {
            var requestUrl = $"SystemOptions/GetSystemOption?id={id}";
            var response = await _helperUrlApi.GetDataFromApi<SystemOptionVM>(requestUrl);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }
        public async Task<IActionResult> DeleteSystemOptions(int id)
        {
            var requestUrl = $"SystemOptions/GetSystemOption?id={id}";
            var response = await _helperUrlApi.GetDataFromApi<SystemOptionVM>(requestUrl);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        // EDIT: Post data to the API to update a SystemOption
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSystemOptions(SystemOptionVM model)
        {
            if (ModelState.IsValid)
            {
                var requestUrl = "SystemOptions/HandlePost";
                var response = await _helperUrlApi.PostDataToApi<SystemOptionVM, SystemOptionVM>(requestUrl, model,"update");

                if (response != null)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Error updating system option.");
            }

            return View(model);
        }

        // DELETE: Post data to delete a system option
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSystemOptions(SystemOptionVM model)
        {
            var requestUrl = $"SystemOptions/HandlePost";
            var response = await _helperUrlApi.PostDataToApi<object, bool>(requestUrl, new { },"delete");

            if (response)
            {
                return RedirectToAction(nameof(Index));
            }

            return View("Error");
        }

       
    }
}
