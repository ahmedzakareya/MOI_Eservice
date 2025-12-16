using Azure;
using Business.Enums;
using Business.Helpers;
using Business.ViewModel;
using Business.ViewModel.Dynamic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using NuGet.Packaging;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace MOI_Eservice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ElawController : Controller
    {

        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;
        private readonly HelperUrlApi _helperUrlApi;
        private readonly string _file;
        private readonly ILogger<ElawController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly GenerateLicNo _generateLicNo;

        public ElawController(IConfiguration configuration, ILogger<ElawController> logger, IWebHostEnvironment env, GenerateLicNo generateLicNo, HttpClient httpClient, HelperUrlApi helperUrlApi)
        {
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _httpClient = httpClient;
            _helperUrlApi = helperUrlApi;
            _logger = logger;
            _env = env;
            _file = configuration["Path:Elaw"];

            _generateLicNo = generateLicNo;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Statistics()
        {
            var apiSettings = _baseUrl + $"api/AdminElaw/GetAllStatistics";
            var  statistics = await _helperUrlApi.GetDataFromApi<StatisticsViewModel>(apiSettings);

            return View(statistics);
        }
        #region Request 
        //AllRequest
        private async Task<List<RequestVM>> FetchRequestsAsync(int serviceId, List<RequestTypeEnum> requestTypes)
        {
            try
            {
                // Build request types parameter
                var requestTypeIds = string.Join(",", requestTypes.Select(rt => (int)rt));
                var requestUrl = $"api/AdminElaw/GetAllRequest?serviceId={serviceId}&requestTypes={requestTypeIds}";

               

                // Fetch data using the helper method
                var response = await _helperUrlApi.GetDataFromApi<List<RequestVM>>(requestUrl);

                return response ?? new List<RequestVM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching requests.");
                return new List<RequestVM>();
            }
        }
        public async Task<IActionResult> HandleRequests(string viewName, List<RequestTypeEnum> requestTypes)
        {
            try
            {
                // Check user authentication
                if (!HttpContext.User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Login", "Account");
                }

                // Extract ServiceId from user claims
                var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
                if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
                {
                    return RedirectToAction("ErrorPage", "Home");
                }

                // Fetch requests using the existing FetchRequestsAsync method
                var requests = await FetchRequestsAsync((int)ServiceEnum.Elaw, requestTypes);

                // Return the specified view with the retrieved data
                return View(viewName, requests);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling requests.");
                return RedirectToAction("ErrorPage", "Home");
            }
        }
        public async Task<IActionResult> ChangeDataRequests()
        {
            return await HandleRequests(
                "ChangeDataRequests",
                new List<RequestTypeEnum> { RequestTypeEnum.ChangeData }
               
            );
        }
        public async Task<IActionResult> GetRenewRequest()
        {
            return await HandleRequests(
                "GetRenewRequest",
                new List<RequestTypeEnum> { RequestTypeEnum.Renew }
            );
        }
        public async Task<IActionResult> AllRequest()
        {
            return await HandleRequests(
                "GetAllRequests",
                new List<RequestTypeEnum> { RequestTypeEnum.Request }
            );
        }

        //public async Task<ActionResult> AllRequest()
        //{

        //    var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
        //    if (!loggedInUser)
        //    {
        //        // Handle case where user is not logged in
        //        return RedirectToAction("Login", "Account");
        //    }
        //    var user= User.Claims.Where(u=>u.Type==ClaimTypes.UserData).FirstOrDefault().Value;
        //    var ServiceId = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
        //    var requestUrl = $"api/Elaw/GetAllRequest?ServiceId={(int)ServiceEnum.Elaw}";
        //    var request = await _helperUrlApi.GetDataFromApi<List<RequestVM>>(requestUrl);
       
        //    return View(request);
        //}

        public async Task<ActionResult> GetRequestDetails(int id,int licTypeId)
        {
            var requestUrl = $"api/AdminElaw/GetRequestById?requestId={id}&licTypeId={licTypeId}";
            var request = await _helperUrlApi.GetDataFromApi<RequestDetailsVM>(requestUrl);
           
            return View(request);

        }

        public async Task<ActionResult> GetRequestChangeDetails(int id, int licTypeId)
        {
            var requestUrl = $"api/AdminElaw/GetRequestById?requestId={id}&licTypeId={licTypeId}";
            var request = await _helperUrlApi.GetDataFromApi<RequestDetailsVM>(requestUrl);
            //_logger.LogInformation($"RequestDetails: {JsonConvert.SerializeObject(request)}");
            Console.WriteLine(request);
            return View(request);

        }
        #endregion
        #region Forms
        public async Task<IActionResult> AddForm()
        {
            var apiSettings = _baseUrl + $"api/AdminElaw/GetForms";

            var getform = await _helperUrlApi.GetDataFromApi<List<FormsViewModel>>(apiSettings);
            if (getform == null)
            {
                getform = new List<FormsViewModel>(); // Initialize an empty list if no data is returned

            }

            return View(getform);
        }

        [HttpPost]
        public async Task<IActionResult> AddForm(IFormFile UploadedFile, string fileName)
        {
            if (UploadedFile == null || UploadedFile.Length == 0)
            {
                ModelState.AddModelError("UploadedFile", "Please upload a valid file.");
                return View("Index"); // Or redirect back with validation errors
            }

            try
            {
                var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
                if (!loggedInUser)
                {
                    // Handle case where user is not logged in
                    return RedirectToAction("Login", "Account");
                }
                var serviceIdClaim = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
                if (string.IsNullOrEmpty(serviceIdClaim) || !int.TryParse(serviceIdClaim, out int serviceId))
                {
                    return RedirectToAction("ErrorPage", "Home");
                }

                var apiSettings = _baseUrl + $"api/AdminElaw/SaveForms";
                string pathForm = Path.Combine(_file, "Forms");
                // Use the existing function to save the file
                var response = await SaveFileToDiskAsync(UploadedFile, fileName, pathForm, null);

                //// Save form details to the database
                //SaveFormToDatabase(ModelName, response.FilePath);
                var requestData = new FormsViewModel
                {
                    FormPath = response.FilePath,
                    FormName = response.FileName,
                    ServiceId = (int)ServiceEnum.Elaw,
                    IsDeleted = false,
                    FormType = ".pdf"
                };
                var requesttoapi = await _helperUrlApi.PostDataToApi<FormsViewModel, FormsViewModel>(
                           apiSettings,
                           requestData
                       );
                TempData["SuccessMessage"] = "Form added successfully!";
                return RedirectToAction("AddForm");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving the file. Please try again.";
                Console.WriteLine(ex.Message); // Log the exception
            }

            return View("AddForm");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteForm(int formId)
        {
            var apiSettings = _baseUrl + $"api/AdminElaw/DeleteForm/{formId}";

            var form = _helperUrlApi.GetDataFromApi<FormsViewModel>(apiSettings);

            return RedirectToAction("AddForm");
        }
        #endregion
        #region News
        [HttpGet]
        public async Task<IActionResult> AddNews()
        {
            var apiSettings = _baseUrl + $"api/AdminElaw/GetNews";
            var model = new NewsVM
            {
                NewsList =await _helperUrlApi.GetDataFromApi<List<NewsItem>>(apiSettings) 
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddNews(NewsVM model)
        {
            var apiSettings = _baseUrl + $"api/AdminElaw/AddOrEditNews";
            var images =await UploadFile(model.Image);
            model.FilePath = images.FilePath;
            model.FileName = images.FileName;
            var requestData = new NewsItem
            {
                Description=model.Description,
                Id=model.Id,
                Image=images.FileName,
                SmallDescription=model.SmallDescription,
                Title=model.Title,
                CreatedDate=DateTime.Now,
                Status=true,
            };
            Console.WriteLine(requestData);
            var addnews = await _helperUrlApi.PostDataToApi<NewsItem, NewsItem>(apiSettings, requestData);
            return RedirectToAction("AddNews");
        }
        #endregion
        #region Link
        [HttpGet]
        public async Task<IActionResult> Addlink()
        {
            var apiSettings = _baseUrl + $"api/AdminElaw/GetAllLinks";
            var model = new LinksVM
            {
                LinksList = await _helperUrlApi.GetDataFromApi<List<AddLinksVM>>(apiSettings)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddLink(LinksVM model)
        {
            var apiSettings = _baseUrl + $"api/AdminElaw/AddOrEditLink";
         
            var requestData = new AddLinksVM
            {
                
                Id = model.Id,
               IsDeleted=false,
               Link=model.Link,
               
               Name=model.Name,
               Sort=model.Sort,
                Status = model.Status,
            };
            Console.WriteLine(requestData);
            var addnews = await _helperUrlApi.PostDataToApi<AddLinksVM, AddLinksVM>(apiSettings, requestData);
            return RedirectToAction("Addlink");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteLink(int id)
        {
            var apiSettings = _baseUrl + $"api/AdminElaw/DeleteLink/{id}";

            
            var addnews = await _helperUrlApi.DeleteDataFromApi<LinksVM>(apiSettings);
            return RedirectToAction("Addlink");
        }
        #endregion
        #region Conditions
        public async Task<IActionResult> AddConitions()
        {
            var apiSettings = _baseUrl + $"api/Dynamic/GetLicencesTypes";
            var LicencesType = await _helperUrlApi.GetDataFromApi<List<LicencesTypeVM>>(apiSettings);
           var licenceTypes = LicencesType.Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = l.NameAr
            }).ToList();
            //ViewBag.LicencesType = LicencesType;
            return View(LicencesType);
        }
        [HttpPost]
        public async Task<IActionResult> AddConditions(ConditionVM model)
        {
            var apiSettings = _baseUrl + $"api/AdminElaw/AddConditions";
            var PostCondition=await _helperUrlApi.PostDataToApi<ConditionVM,ConditionVM>(apiSettings, model);

          return  RedirectToAction("AddConitions");
        }

        #endregion
        #region Licences
        public async Task<ActionResult> AllLicences()
        {


            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var user = User.Claims.Where(u => u.Type == ClaimTypes.UserData).FirstOrDefault().Value;
            var ServiceId = HttpContext.User.Claims.FirstOrDefault(u => u.Type == "ServiceId")?.Value;
            var licencesUrl = $"api/AdminElaw/GetAllLicences?ServiceId={(int)ServiceEnum.Elaw}";
            var licences = await _helperUrlApi.GetDataFromApi<List<LicencesVM>>(licencesUrl);

            return View(licences);

          
        }

        public async Task<ActionResult> GetLicencesDetails(int id)
        {
            var licenceUrl = $"api/AdminElaw/GetLicencesDetails?licid={id}";
            var licence = await _helperUrlApi.GetDataFromApi<LicenceDetailsVM>(licenceUrl);
            return View(licence);

        }
        #endregion
        #region SaveData
        public async Task<ActionResult> SaveData([FromForm] SaveDataViewModel model)
        {
            // Extract page name from the referrer
            var referrer = HttpContext.Request.Headers["Referer"].ToString();
            string pageName = string.IsNullOrEmpty(referrer)
                ? "Unknown"
                : new Uri(referrer).AbsolutePath;
            pageName = pageName.Substring(pageName.LastIndexOf("/") + 1);
           
            // Retrieve user data from session
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            string logs = "";
            if (model.ChangeLogs != null)
            {
                logs = string.Join(", ", model.ChangeLogs);  // You can change the separator as needed
            }
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var username = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;
           
            var userid = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.UserData)?.Value;
            // Initialize API settings
            var apiSettings = _baseUrl + "api/AdminElaw";
            ViewBag.ApiBaseUrl = apiSettings;
            var apisettingsDynamic = _baseUrl + "Dynamic";
            // Initialize default values
            string licNo = "";
            long SequenceNo = 0;
            string nextStatusName = "";
            List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();
            int licStatusId = (int)licencesStatusEnum.Pending;

            try
            {
                // Create a new HttpClient instance for each request
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                   
                    UpdatedRequestVM requestData;
                    // Fetch the next status dynamically from the workflow
                    if (model.Action == "RequestStatusbutton")
                    {
                        var workflowResponse = await client.GetAsync($"{apisettingsDynamic}/GetNextStatus?serviceId={(int)ServiceEnum.Elaw}&activityTypeId={model.ActivityTypeId}&requestTypeId={model.ReqTypeId}&currentStatusId={model.ReqStatusId}");
                        if (!workflowResponse.IsSuccessStatusCode)
                        {
                            return StatusCode((int)workflowResponse.StatusCode, "Failed to fetch next status");
                        }

                        var workflowData = await workflowResponse.Content.ReadAsStringAsync();
                        var workflowResult = JsonConvert.DeserializeObject<dynamic>(workflowData);





                        if (workflowResult.success == false)
                        {
                            return BadRequest(workflowResult.message.ToString());
                        }
                        Console.WriteLine($"Conditions: {workflowResult.conditions}");
                        Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");
                        // Extract conditions JSON as a string
                        //if(workflowResult.)
                        string conditionsJson = workflowResult.conditions?.ToString() ?? string.Empty;

                        // Declare variables outside the if block
                        string requestStatusValue = null;
                        string requestTypeValue = null;

                        // Check if conditionsJson is not null or empty before proceeding
                        if (!string.IsNullOrEmpty(conditionsJson))
                        {
                            Console.WriteLine($"Conditions: {workflowResult.conditions}");
                            Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");

                            // Deserialize conditions JSON
                            var conditions = JsonConvert.DeserializeObject<List<Condition>>(conditionsJson);

                            // Instantiate the evaluator
                            ConditionEvaluator conditionEvaluator = new ConditionEvaluator();

                            // Evaluate the conditions and extract values
                            var (areConditionsMet, extractedValues) = conditionEvaluator.Evaluate(conditionsJson, new
                            {
                                RequestStatus = "final", 
                                
                            });

                            // Extract specific values
                            requestStatusValue = extractedValues.ContainsKey("RequestStatus") ? extractedValues["RequestStatus"] : null;

                            Console.WriteLine($"RequestStatusValue: {requestStatusValue}");
                           
                        }

                        // Now you can use requestStatusValue and requestTypeValue outside the if block
                        Console.WriteLine($"Outside If - RequestStatusValue: {requestStatusValue}");
                        Console.WriteLine($"Outside If - RequestTypeValue: {requestTypeValue}");

                        // Extract the next status
                        int nextStatusId = workflowResult.nextStatusId;
                        nextStatusName = workflowResult.nextStatusName;
                        string flag = workflowResult.flag;

                        if (flag == "LicencesNo"|| flag == "final")
                        {
                          var licenseResult = await _generateLicNo.GenerateUniqueLicenseNumberElaw((int)ServiceEnum.Elaw,model.ReqTypeId, model.ActivityTypeId);
                            licNo = licenseResult.Item2;
                            SequenceNo = licenseResult.Item1;
                        }
                        // Additional logic for specific statuses
                        if ( model.files != null && model.files.Count > 0)
                        {
                            foreach (var file in model.files)
                            {
                                if (file.Files != null)
                                {
                                    // Save each file to disk
                                    var savedFilePath = await SaveFileToDiskAsync(file.Files, file.filename, _file, model.ReqNo);
                                    savedFilePath.IsRequired = file.ismandatory;
                                    savedFilePath.FieldName = file.fieldname;
                                    filePath.Add(savedFilePath);
                                    // Perform any additional logic with the saved file path, if needed
                                    Console.WriteLine($"File saved at: {savedFilePath}");

                                }
                            }
                        }
                        // Create request data
                        requestData = new UpdatedRequestVM
                        {
                            LicNo = licNo,
                            StatusId = nextStatusId,
                            RequestId = model.RequestId,
                            SequenceNo=SequenceNo,
                            //FilePath = filePath.FilePath,
                            //FileName = filePath.FileName,
                            saveResponseVMs = filePath,
                            ReqTypeId = model.ReqTypeId,
                            LicStatusId = licStatusId,
                            Note = model.Note,
                            NameUser = username,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            requestStatusValue = requestStatusValue,
                            requestTypeValue = requestTypeValue,
                            Flag= flag,
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                            selectedAttachments = model.selectedAttachments,
                            uncheckedAttachments = model.uncheckedAttachments,

                            AttachmentStates = model.allAttachmentsState,


                        };
                    }
                    else if (model.Action == "CorrectData")
                    {
                        
       
                        requestData = new UpdatedRequestVM
                        {                           
                            StatusId =(int)RequestStatusEnum.CorrectData,
                            RequestId = model.RequestId,
                            saveResponseVMs = filePath,
                            ReqTypeId = model.ReqTypeId,
                            LicStatusId = licStatusId,
                            Note = model.Note,
                            NameUser = username,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                            selectedAttachments = model.selectedAttachments,
                            uncheckedAttachments = model.uncheckedAttachments,
                            AttachmentStates = model.allAttachmentsState

                        };
                    }
                    else
                    {
                        requestData = new UpdatedRequestVM
                        {
                            LicNo = licNo,
                            StatusId = model.ReqStatusId,
                            RequestId = model.RequestId,
                            ReqTypeId = model.ReqTypeId,
                            LicStatusId = (int)licencesStatusEnum.Refused,
                            Note = model.Note,
                            NameUser = username,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                            selectedAttachments = model.selectedAttachments,
                            uncheckedAttachments = model.uncheckedAttachments,
                            AttachmentStates = model.allAttachmentsState

                        };
                    }
                    // Send API request to update request status
                    var jsonContent = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var updateResponse = await client.PostAsync($"{apiSettings}/UpdateRequestStatus", content);

                    if (!updateResponse.IsSuccessStatusCode)
                    {
                        return StatusCode((int)updateResponse.StatusCode, "Failed to update request status");
                    }

                    var responseData = await updateResponse.Content.ReadAsStringAsync();
                    //var updatedRequest = JsonConvert.DeserializeObject<dynamic>(responseData);

                    // Return the updated request data
                    return Ok(new
                    {
                        UpdatedRequest = responseData,
                        Message = $"Status updated to {nextStatusName}"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while accessing the Index page.");

                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        public async Task<ActionResult> SaveTransData([FromForm] SaveDataViewModelTransactonType model)
        {
            // Extract page name from the referrer
            var referrer = HttpContext.Request.Headers["Referer"].ToString();
            string pageName = string.IsNullOrEmpty(referrer)
                ? "Unknown"
                : new Uri(referrer).AbsolutePath;
            pageName = pageName.Substring(pageName.LastIndexOf("/") + 1);

            // Retrieve user data from session
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            string logs = "";
            if (model.ChangeLogs != null)
            {
                logs = string.Join(", ", model.ChangeLogs);  // You can change the separator as needed
            }
            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var username = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;

            var userid = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.UserData)?.Value;
            // Initialize API settings
            var apiSettings = _baseUrl + "api/AdminElaw";
            ViewBag.ApiBaseUrl = apiSettings;
            var apisettingsDynamic = _baseUrl + "Dynamic";
            // Initialize default values
            string licNo = "";
            long SequenceNo = 0;
            string nextStatusName = "";
            List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();
            //int licStatusId = (int)licencesStatusEnum.Pending;

            try
            {
                // Create a new HttpClient instance for each request
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                    UpdatedRequestVM requestData;
                    // Fetch the next status dynamically from the workflow
                    if (model.Action == "RequestStatusbutton")
                    {
                        var workflowResponse = await client.GetAsync($"{apisettingsDynamic}/GetNextStatus?serviceId={(int)ServiceEnum.Elaw}&activityTypeId={model.ActivityTypeId}&requestTypeId={model.ReqTypeId}&currentStatusId={model.ReqStatusId}");
                        if (!workflowResponse.IsSuccessStatusCode)
                        {
                            return StatusCode((int)workflowResponse.StatusCode, "Failed to fetch next status");
                        }

                        var workflowData = await workflowResponse.Content.ReadAsStringAsync();
                        var workflowResult = JsonConvert.DeserializeObject<dynamic>(workflowData);





                        if (workflowResult.success == false)
                        {
                            return BadRequest(workflowResult.message.ToString());
                        }
                        Console.WriteLine($"Conditions: {workflowResult.conditions}");
                        Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");
                        // Extract conditions JSON as a string
                        //if(workflowResult.)
                        string conditionsJson = workflowResult.conditions?.ToString() ?? string.Empty;

                        // Declare variables outside the if block
                        string requestStatusValue = null;
                        string requestTypeValue = null;

                        // Check if conditionsJson is not null or empty before proceeding
                        if (!string.IsNullOrEmpty(conditionsJson))
                        {
                            Console.WriteLine($"Conditions: {workflowResult.conditions}");
                            Console.WriteLine($"Type: {workflowResult.conditions?.GetType()}");

                            // Deserialize conditions JSON
                            var conditions = JsonConvert.DeserializeObject<List<Condition>>(conditionsJson);

                            // Instantiate the evaluator
                            ConditionEvaluator conditionEvaluator = new ConditionEvaluator();

                            // Evaluate the conditions and extract values
                            var (areConditionsMet, extractedValues) = conditionEvaluator.Evaluate(conditionsJson, new
                            {
                                RequestStatus = "final",

                            });

                            // Extract specific values
                            requestStatusValue = extractedValues.ContainsKey("RequestStatus") ? extractedValues["RequestStatus"] : null;

                            Console.WriteLine($"RequestStatusValue: {requestStatusValue}");

                        }

                        // Now you can use requestStatusValue and requestTypeValue outside the if block
                        Console.WriteLine($"Outside If - RequestStatusValue: {requestStatusValue}");
                        Console.WriteLine($"Outside If - RequestTypeValue: {requestTypeValue}");

                        // Extract the next status
                        int nextStatusId = workflowResult.nextStatusId;
                        nextStatusName = workflowResult.nextStatusName;
                        string flag = workflowResult.flag;

                        if (flag == "LicencesNo"||flag=="final")
                        {
                            var licenseResult = await _generateLicNo.GenerateUniqueLicenseNumberElaw((int)ServiceEnum.Elaw, model.ReqTypeId, model.ActivityTypeId);

                            licNo = licenseResult.Item2;
                            SequenceNo = licenseResult.Item1;
                        }
                        // Additional logic for specific statuses
                        if (model.files != null && model.files.Count > 0)
                        {
                            foreach (var file in model.files)
                            {
                                if (file.Files != null)
                                {
                                    // Save each file to disk
                                    var savedFilePath = await SaveFileToDiskAsync(file.Files, file.filename, _file, model.ReqNo);
                                    savedFilePath.IsRequired = file.ismandatory;
                                    savedFilePath.FieldName = file.fieldname;
                                    filePath.Add(savedFilePath);
                                    // Perform any additional logic with the saved file path, if needed
                                    Console.WriteLine($"File saved at: {savedFilePath}");

                                }
                            }
                        }
                        // Create request data
                        requestData = new UpdatedRequestVM
                        {
                            LicNo = licNo,
                            StatusId = nextStatusId,
                            RequestId = model.RequestId,
                            //FilePath = filePath.FilePath,
                            //FileName = filePath.FileName,
                            saveResponseVMs = filePath,
                            ReqTypeId = model.ReqTypeId,
                            //LicStatusId = licStatusId,
                            Note = model.Note,
                            NameUser = username,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            requestStatusValue = requestStatusValue,
                            requestTypeValue = requestTypeValue,
                            Flag = flag,
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                            selectedAttachments = model.selectedAttachments,
                            uncheckedAttachments = model.uncheckedAttachments,
                            TransId=model.TransactionId,
                            TransTypeId=model.transTypeId,
                            AttachmentStates = model.AttachmentStates,


                        };
                    }
                    else if (model.Action == "CorrectData")
                    {


                        requestData = new UpdatedRequestVM
                        {
                            StatusId = (int)RequestStatusEnum.CorrectData,
                            RequestId = model.RequestId,
                            saveResponseVMs = filePath,
                            ReqTypeId = model.ReqTypeId,
                            //LicStatusId = licStatusId,
                            Note = model.Note,
                            NameUser = username,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            TransId = model.TransactionId,
                            TransTypeId = model.transTypeId,
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                            selectedAttachments = model.selectedAttachments,
                            uncheckedAttachments = model.uncheckedAttachments,
                            AttachmentStates = model.AttachmentStates

                        };
                    }
                    else
                    {
                        requestData = new UpdatedRequestVM
                        {
                            LicNo = licNo,
                            StatusId = model.ReqStatusId,
                            RequestId = model.RequestId,
                            ReqTypeId = model.ReqTypeId,
                            LicStatusId = (int)licencesStatusEnum.Refused,
                            Note = model.Note,
                            NameUser = username,
                            ActionName = pageName,
                            UserId = int.Parse(userid),
                            ChangeLogs = model.ChangeLogs,
                            Action = model.Action,
                            selectedAttachments = model.selectedAttachments,
                            uncheckedAttachments = model.uncheckedAttachments,
                            AttachmentStates = model.AttachmentStates,
                            TransId = model.TransactionId,
                            TransTypeId = model.transTypeId,
                            
                        };
                    }
                    // Send API request to update request status
                    var jsonContent = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var updateResponse = await client.PostAsync($"{apiSettings}/UpdateRequestStatus", content);

                    if (!updateResponse.IsSuccessStatusCode)
                    {
                        return StatusCode((int)updateResponse.StatusCode, "Failed to update request status");
                    }

                    var responseData = await updateResponse.Content.ReadAsStringAsync();
                    //var updatedRequest = JsonConvert.DeserializeObject<dynamic>(responseData);

                    // Return the updated request data
                    return Ok(new
                    {
                        UpdatedRequest = responseData,
                        Message = $"Status updated to {nextStatusName}"
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while accessing the Index page.");

                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
        #endregion
        #region Function Attachment
        #region this for additional attachment
        [HttpPost]
        public async Task<ActionResult> SaveFile([FromForm] fileAttach file)
        {
            var referrer = HttpContext.Request.Headers["Referer"].ToString();
            string pageName = string.IsNullOrEmpty(referrer)
                ? "Unknown"
                : new Uri(referrer).AbsolutePath;
            pageName = pageName.Substring(pageName.LastIndexOf("/") + 1);
            int serviceId = (int)ServiceEnum.Elaw;
            // Retrieve user data from session
            var loggedInUser = HttpContext.User.Identity.IsAuthenticated == true;
            string logs = "";

            if (!loggedInUser)
            {
                // Handle case where user is not logged in
                return RedirectToAction("Login", "Account");
            }
            var username = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Name)?.Value;

            var userid = HttpContext.User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.UserData)?.Value;
            // Initialize API settings
            var apiSettings = _baseUrl + "api/AdminTourism";
            ViewBag.ApiBaseUrl = apiSettings;
            var apisettingsDynamic = _baseUrl + "Dynamic";


            List<FileSaveResponseVM> filePath = new List<FileSaveResponseVM>();

            List<string> fileNames = new List<string>();
            ViewBag.ApiBaseUrl = apiSettings;
            if (loggedInUser)
            {
                using (var client = new HttpClient())
                {
                    foreach (var fileAttach in file.files)
                    {
                        if (fileAttach.Files != null)
                        {
                            // Save each file to disk
                            var savedFilePath = await SaveFileToDiskAsync(fileAttach.Files, fileAttach.filename, _file, file.ReqNo);

                            filePath.Add(savedFilePath);
                            fileNames.Add(fileAttach.filename);
                            Console.WriteLine($"File saved at: {savedFilePath}");

                        }
                    }
                    List<string> changeLogs = new List<string>
                        {
                            "Files uploaded: " + string.Join(", ", fileNames)
                        };
                    var requestData = new UpdatedRequestVM
                    {

                        RequestId = file.RequestId ?? 0,

                        saveResponseVMs = filePath,

                        ServiceId = serviceId,

                        NameUser = username,
                        ActionName = pageName,
                        UserId = int.Parse(userid),

                        ChangeLogs = changeLogs,
                        Action = "AddAdditionalFiles",


                    };
                    var jsonContent = JsonConvert.SerializeObject(requestData);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var updateResponse = await client.PostAsync($"{apiSettings}/SaveAttachmentAdditional", content);

                    if (!updateResponse.IsSuccessStatusCode)
                    {
                        return StatusCode((int)updateResponse.StatusCode, "Failed to update request status");
                    }

                    var responseData = await updateResponse.Content.ReadAsStringAsync();
                    //var updatedRequest = JsonConvert.DeserializeObject<dynamic>(responseData);

                    // Return the updated request data
                    return Ok(new
                    {
                        UpdatedRequest = responseData,

                    });
                }
            }
            else
            {
                return RedirectToAction("Login", "Account");

            }
            return null;
        }
        #endregion
        public async Task<FileSaveResponseVM> SaveFileToDiskAsync(IFormFile file, string fileNameFromFile, string relativePath, string? reqNo)
        {
            string filepath = Path.Combine(_env.WebRootPath, relativePath);
            string uploadsFolder;
            if (!string.IsNullOrEmpty(reqNo))
            {
                uploadsFolder = Path.Combine(_env.WebRootPath, relativePath, reqNo);
            }
            else
            {
                uploadsFolder = Path.Combine(_env.WebRootPath, relativePath);
            }

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder); // Create directory if it doesn't exist
            }
            string fileName;

            // Generate the sequence number and file name
            if (!string.IsNullOrEmpty(reqNo))
            {
                string _Reqno = reqNo + "/AttachNo-";
                Random random = new Random();
                int sequenceNumber = random.Next(100, 1000); // Generating a random number for sequence
                fileName = $"{_Reqno}{sequenceNumber}.pdf"; // AttachNo-{sequenceNumber}.pdf
            }
            else
            {
                fileName = $"{fileNameFromFile}.pdf";
            }
            string filePath = Path.Combine(filepath, fileName);
            string filePathWithoutSlash = filePath.Replace("/", "\\"); // Replace / with backslash for Windows compatibility

            try
            {
                // Save the file asynchronously
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream); // Copy file to the disk
                }

                return new FileSaveResponseVM
                {
                    FilePath = fileName,
                    FileName = fileNameFromFile
                };
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine("Error: " + ex.Message);
                throw; // Rethrow exception or handle accordingly
            }


        }
        #region For image News
        [HttpPost]
        public async Task<FileSaveResponseVM> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return new FileSaveResponseVM
                {
                    Success = false,
                    Message = "File not provided or empty."
                };
            }

            // Generate a unique file name
            string fileName = $"{DateTime.UtcNow.Day}{DateTime.UtcNow.Second}{DateTime.UtcNow.Millisecond}{Path.GetExtension(file.FileName)}";

            // Define the upload directory path (from configuration)
            string uploadDir = Path.Combine(_env.WebRootPath,_file, "News");

            // Ensure the directory exists
            if (!Directory.Exists(uploadDir))
            {
                Directory.CreateDirectory(uploadDir);
            }

            // Full file path for saving
            string filePath = Path.Combine(uploadDir, fileName).Replace("\\", "/");

            // Relative path for database or API (normalized for web usage)
            string relativePath = Path.Combine("News", fileName).Replace("\\", "/");

            try
            {
                // Save the file to the specified path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Return file details
                return new FileSaveResponseVM
                {
                    Success = true,
                    FileName = fileName,
                    FilePath = relativePath // Normalized for web usage
                };
            }
            catch (Exception ex)
            {
                // Return error response
                return new FileSaveResponseVM
                {
                    Success = false,
                    Message = "File upload failed.",
                    Error = ex.Message
                };
            }
        }


        #endregion
    }

    #endregion
}

