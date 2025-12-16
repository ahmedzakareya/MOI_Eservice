using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Business.ViewModel.Account;
using Business.ViewModel.Dynamic;
using Domain.Entities;
using Newtonsoft.Json;
using System.Text;
using Business.Helpers;
using Business.Enums;
using NuGet.Configuration;
using Microsoft.EntityFrameworkCore.Metadata;
using Business.ViewModel.HomePage;
using Business.ViewModel;



namespace MOI_Eservice.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DynamicController : Controller
    {
        private readonly string _baseUrl;

        private readonly HttpClient _httpClient;
        private readonly HelperUrlApi _helperUrlApi;

        public DynamicController(IConfiguration configuration, HttpClient httpClient, HelperUrlApi helperUrlApi)
        {
            _baseUrl = configuration["ApiSettings:BaseUrl"];

            _httpClient = httpClient;
            _helperUrlApi = helperUrlApi;
        }
        public async Task<ActionResult> Index()
        {
            return View();
        }
       
        #region WorkFlow Status
        [HttpGet]
        public async Task<IActionResult> AddWorkFlow()
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;

            // Fetch all data from the unified API
            var allData = await _helperUrlApi.GetDataFromApi<WorkFlowAllSystem>($"{apiSettings}GetAllData");

            if (allData == null)
            {
                return View("Error");
            }

            // Map the data to SelectListItem for the ViewModel
            var model = new WorkFlowVM
            {
                TransactionTypes=allData.transactionTypesLookups.Select(tt=>new SelectListItem
                {
                    Text=tt.NameAr,
                    Value=tt.Id.ToString()
                }).ToList(),
                Services = allData.Services.Select(s => new SelectListItem
                {
                    Text = s.EserviceName,
                    Value = s.ServiceId.ToString()
                }).ToList(),
                
                RequestTypes = allData.RequestTypes.Select(rt => new SelectListItem
                {
                    Text = rt.NameAr,
                    Value = rt.Id.ToString()
                }).ToList(),

                RequestStatus = allData.RequestStatuses.Select(rs => new SelectListItem
                {
                    Text = rs.NameAr,
                    Value = rs.Id.ToString()
                }).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkFlows()
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            
            // Fetch all data from the API
            //var allData = await GetDataFromApi<List<WorkFlowVM>>($"{apiSettings}GetAllWorkflows");
           var allData = await _helperUrlApi.GetDataFromApi<List<WorkFlowVM>>($"{apiSettings}GetAllWorkflows");

            if (allData == null)
            {
                return View("Error");
            }

            return View(allData);
        }
        [HttpGet]
        public async Task<IActionResult> DeleteWorkflow(int id)
        {
            // Get Workflow details from API
            var apiSettings = $"{_baseUrl}Dynamic/GetWorkflow/{id}";
            var workflowItem = await _helperUrlApi.GetDataFromApi<WorkFlowVM>(apiSettings);
            var apiSetting = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSetting;

            if (workflowItem == null)
            {
                TempData["ErrorMessage"] = "Workflow not found.";
                return RedirectToAction("GetAllWorkflows");
            }

            var model = new WorkFlowVM
            {
                Id = workflowItem.Id,
               // ActivityTypeId = workflowItem.ActivityTypeId,
                CurrentStatusId = workflowItem.CurrentStatusId,
                CurrentStatusName = workflowItem.CurrentStatusName,
                NextStatusName = workflowItem.NextStatusName,
                NextStatusId = workflowItem.NextStatusId,
                ServiceName = workflowItem.ServiceName,
                ServiceId = workflowItem.ServiceId,
                RequestTypeName = workflowItem.RequestTypeName,
                RequestTypeId = workflowItem.RequestTypeId,
                //ActivityTypeName = workflowItem.ActivityTypeName,
                FlagRequestStatus = workflowItem.FlagRequestStatus,
                Conditions = workflowItem.Conditions,
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateWorkflow(int id)
        {
            // Get Workflow details from API
            var apiSettings = $"{_baseUrl}Dynamic/GetWorkflow/{id}";
            var workflowItem = await _helperUrlApi.GetDataFromApi<WorkFlowVM>(apiSettings);
            var apiSetting = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSetting;

            if (workflowItem == null)
            {
                TempData["ErrorMessage"] = "Workflow not found.";
                return RedirectToAction("GetAllWorkflows");
            }
            ViewBag.EditRequestType = (int)RequestTypeEnum.ChangeData;

            var model = new WorkFlowVM
            {
                Id = workflowItem.Id,
              //  ActivityTypeId = workflowItem.ActivityTypeId,
                CurrentStatusId = workflowItem.CurrentStatusId,
                CurrentStatusName = workflowItem.CurrentStatusName,
                NextStatusName = workflowItem.NextStatusName,
                NextStatusId = workflowItem.NextStatusId,
                ServiceName = workflowItem.ServiceName,
                ServiceId = workflowItem.ServiceId,
                RequestTypeName = workflowItem.RequestTypeName,
                RequestTypeId = workflowItem.RequestTypeId,
               // ActivityTypeName = workflowItem.ActivityTypeName,
                FlagRequestStatus= workflowItem.FlagRequestStatus,
                Conditions = workflowItem.Conditions,
                RequestStatus=workflowItem.RequestStatus,
               // ActivityTypes=workflowItem.ActivityTypes,
                TransactionTypeName=workflowItem.TransactionTypeName,
                IsPermissionRequired=workflowItem.IsPermissionRequired,
                TransactionTypes = workflowItem.TransactionTypes,
                RequestTypes = workflowItem.RequestTypes,
                Services = workflowItem.Services    ,
                TransactionTypeId= workflowItem.TransactionTypeId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteWorkflow(WorkFlowVM workFlowVM)
        {
            // Call API to delete the workflow
            var apiSettings = $"{_baseUrl}Dynamic/DeleteMenuItem/{workFlowVM.Id}";
            var response = await _httpClient.DeleteAsync(apiSettings);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Workflow deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete workflow.";
            }

            return RedirectToAction("GetAllWorkflows");
        }
        [HttpPost]
        public async Task<IActionResult> CloneWorkflow(int id)
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            var url = $"{_baseUrl}Dynamic/GetWorkflow/{id}";
            var result = await _helperUrlApi.GetDataFromApi<WorkFlowVM>(url);

            if (result == null)
            {
                TempData["Error"] = "فشل في نسخ بيانات المسار";
                return RedirectToAction("GetAllWorkflow", "Dynamic", new { area = "Admin" });
            }

            // Create a copy
            var workflowToCopy = new WorkFlowVM
            {
                Id = 0, // new record
                ServiceId = result.ServiceId,
                RequestTypeId = result.RequestTypeId,
                CurrentStatusId = result.CurrentStatusId,
                NextStatusId = result.NextStatusId,
                FlagRequestStatus = result.FlagRequestStatus,
                Conditions = result.Conditions,
               
            };

            var response = await _helperUrlApi.PostDataToApi<WorkFlowVM, WorkFlowVM>(
                $"{apiSettings}AddWorkFlow", workflowToCopy
            );

            if (response == null)
            {
                TempData["Error"] = "فشل في إنشاء نسخة جديدة";
                return RedirectToAction("GetAllWorkflow", "Dynamic", new { area = "Admin" });
            }

            TempData["Success"] = "تم إنشاء نسخة جديدة بنجاح";
            return RedirectToAction("GetAllWorkflows", "Dynamic", new { area = "Admin" });
        }


        [HttpGet("activities/{serviceId}")]
        public async Task<IActionResult> GetActivityTypes(int serviceId)
        {
            var activities = await GetDataFromApi<List<SelectListItem>>($"{_baseUrl}Dynamic/activities/{serviceId}");
            return Ok(activities ?? new List<SelectListItem>());
        }
        #endregion
        #region WorkFlow Attachment
        [HttpGet]
        public async Task<IActionResult> GetAllAttachmentDynamic()
        {
            var apiUrl = $"{_baseUrl}Dynamic/GetAllAttachmentRules";
            var result = await _helperUrlApi.GetDataFromApi<List<AddWorkflowWithAttachmentsVM>>(apiUrl);
            ViewBag.ApiBaseUrl = $"{_baseUrl}Dynamic/";

            if (result == null)
            {
                TempData["ErrorMessage"] = "Failed to load attachment rules.";
                return View("Error");
            }

            return View(result);
        }
        [HttpGet]
        public async Task<IActionResult> AddWorkFlowAttach()
        {

            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;

            // Fetch all data from the unified API
            var allData = await _helperUrlApi.GetDataFromApi<WorkFlowAllSystem>($"{apiSettings}GetAllData");

            if (allData == null)
            {
                return View("Error");
            }

            // Map the data to SelectListItem for the ViewModel
            var model = new AddWorkflowWithAttachmentsVM
            {
                TransactionTypes = allData.transactionTypesLookups.Select(tt => new SelectListItem
                {
                    Text = tt.NameAr,
                    Value = tt.Id.ToString()
                }).ToList(),
                Services = allData.Services.Select(s => new SelectListItem
                {
                    Text = s.EserviceName,
                    Value = s.ServiceId.ToString()
                }).ToList(),

                RequestTypes = allData.RequestTypes.Select(rt => new SelectListItem
                {
                    Text = rt.NameAr,
                    Value = rt.Id.ToString()
                }).ToList(),

                RequestStatus = allData.RequestStatuses.Select(rs => new SelectListItem
                {
                    Text = rs.NameAr,
                    Value = rs.Id.ToString()
                }).ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteWorkflowAttach(int id)
        {
            // Get Workflow details from API
            var apiSettings = $"{_baseUrl}Dynamic/GetWorkflowAttach/{id}";
            var workflowItem = await _helperUrlApi.GetDataFromApi<AddWorkflowWithAttachmentsVM>(apiSettings);
            var apiSetting = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSetting;

            if (workflowItem == null)
            {
                TempData["ErrorMessage"] = "Workflow not found.";
                return RedirectToAction("GetAllWorkflows");
            }

            var model = new AddWorkflowWithAttachmentsVM
            {
                Id = workflowItem.Id,
                ActivityTypeId = workflowItem.ActivityTypeId,
                RequestStatusId = workflowItem.RequestStatusId,
                RequestStatusName = workflowItem.RequestStatusName,
                AttachName = workflowItem.AttachName,
                IsMandatory = workflowItem.IsMandatory,
                ServiceName = workflowItem.ServiceName,
                ServiceId = workflowItem.ServiceId,
                RequestTypeName = workflowItem.RequestTypeName,
                RequestTypeId = workflowItem.RequestTypeId,
                ActivityTypeName = workflowItem.ActivityTypeName,
                FlagView=workflowItem.FlagView,
                FieldName = workflowItem.FieldName,
                 AllowedFileTypes = workflowItem.AllowedFileTypes,
                 ViewTypeForAttach = workflowItem.ViewTypeForAttach,
                 Description = workflowItem.Description,
                 MaxFileSize = workflowItem.MaxFileSize,
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> UpdateWorkflowAttach(int id)
        {
            // Get Workflow details from API
            var apiSettings = $"{_baseUrl}Dynamic/GetWorkflowAttachWithDropDown/{id}";
            var workflowItem = await _helperUrlApi.GetDataFromApi<AddWorkflowWithAttachmentsVM>(apiSettings);
            var apiSetting = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSetting;

            if (workflowItem == null)
            {
                TempData["ErrorMessage"] = "Workflow not found.";
                return RedirectToAction("GetAllWorkflows");
            }

            var model = new AddWorkflowWithAttachmentsVM
            {
                Id = workflowItem.Id,
                ActivityTypeId = workflowItem.ActivityTypeId,
                RequestStatusId = workflowItem.RequestStatusId,
                RequestStatusName = workflowItem.RequestStatusName,
                AttachName = workflowItem.AttachName,
                IsMandatory = workflowItem.IsMandatory,
                ServiceName = workflowItem.ServiceName,
                ServiceId = workflowItem.ServiceId,
                RequestTypeName = workflowItem.RequestTypeName,
                RequestTypeId = workflowItem.RequestTypeId,
                TransactionTypes = workflowItem.TransactionTypes,
                RequestTypes = workflowItem.RequestTypes,
                Services = workflowItem.Services,
                RequestStatus=workflowItem.RequestStatus,
                ViewTypeForAttach = workflowItem.ViewTypeForAttach,
                AllowedFileTypes = workflowItem.AllowedFileTypes,
                FieldName = workflowItem.FieldName,
                FlagView= workflowItem.FlagView,
                MaxFileSize = workflowItem.MaxFileSize,
                Description = workflowItem.Description,
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> CloneWorkflowAttach(int id)
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            var url = $"{_baseUrl}Dynamic/GetWorkflowAttach/{id}";

            var result = await _helperUrlApi.GetDataFromApi<AddWorkflowWithAttachmentsVM>(url);

            if (result == null)
            {
                TempData["Error"] = "فشل في نسخ القاعدة";
                return RedirectToAction("GetAllAttachmentDynamic", "Dynamic", new { area = "Admin" });
            }

            var cloneModel = new AddWorkflowWithAttachmentsVM
            {
                Id = 0, // Reset ID
                AttachName = result.AttachName,
                IsMandatory = result.IsMandatory,
                ServiceId = result.ServiceId,
                RequestTypeId = result.RequestTypeId,
                TransactionTypeId = result.TransactionTypeId,
                RequestStatusId = result.RequestStatusId,
                FlagView = result.FlagView,
                FieldName= result.FieldName,
                AllowedFileTypes= result.AllowedFileTypes,
                ViewTypeForAttach = result.ViewTypeForAttach,
                ActivityTypeId= result.ActivityTypeId,
                Description= result.Description,
                MaxFileSize = result.MaxFileSize,
            };

            var response = await _helperUrlApi.PostDataToApi<AddWorkflowWithAttachmentsVM, object>(
                $"{apiSettings}AddAttachRule", cloneModel
            );

            if (response == null)
            {
                TempData["Error"] = "فشل في إنشاء نسخة جديدة";
                return RedirectToAction("GetAllAttachmentDynamic", "Dynamic", new { area = "Admin" });
            }

            TempData["Success"] = "تم إنشاء نسخة جديدة بنجاح";
            return RedirectToAction("GetAllAttachmentDynamic", "Dynamic", new { area = "Admin" });
        }

        #endregion
        #region GetApi
        // Helper Method to Fetch Data from API
        private async Task<T> GetDataFromApi<T>(string url)
        {
            try
            {
                _httpClient.BaseAddress = new Uri(_baseUrl);
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonData);
                }
                else
                {
                    Console.WriteLine($"Error fetching data from API: {url}, Status Code: {response.StatusCode}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while calling API: {url}, Exception: {ex.Message}");
                return default;
            }
        }

        #endregion
        #region GetRequestsTypes
        public async Task<IActionResult> GetRequestsTypes()
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var response = await _helperUrlApi.GetDataFromApi<List<RequestsTypesLookup>>($"{apiSettings}GetRequestTypes");

            return View(response);
        }


        #endregion

        #region GetActivityType
        public async Task<IActionResult> GetActivityType()
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var response = await _helperUrlApi.GetDataFromApi<List<ActivityTypesLookup>>($"{apiSettings}GetAllActivityTypes");

            return View(response);
        }


        #endregion
        #region GetEserviceType
        public async Task<IActionResult> GetEserviceType()
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var response = await _helperUrlApi.GetDataFromApi<List<EserviceTypesLookup>>($"{apiSettings}GetAllEserviceTypes");

            return View(response);
        }


        #endregion
        #region GetEservice
        public async Task<IActionResult> GetEservice()
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var response = await _helperUrlApi.GetDataFromApi<List<Eservice>>($"{apiSettings}GetAllEservice");

            return View(response);
        }


        #endregion
        #region GetLicencesInfo
        public async Task<IActionResult> GetLicencesInfo()
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var response = await _helperUrlApi.GetDataFromApi<List<MoiEserviceLicenseInfo>>($"{apiSettings}GetAllLicenseInfo");

            return View(response);
        }
        [HttpGet]
        public async Task<IActionResult> CreateLicencesInfo()
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var apiUrl = $"{_baseUrl}Dynamic/GetLicencesInfoDropDown";
            var dropdownData = await _helperUrlApi.GetDataFromApi<CreateLicencesInfo>(apiUrl);

            if (dropdownData == null)
                return View("Error");

            ViewBag.ActivityTypes = dropdownData.ActivityTypesModel.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.NameAr
            }).ToList();

            ViewBag.TypeBranches = dropdownData.EserviceTypeBranchModel?.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.EserviceTypeBranchAr
            }).ToList();

            ViewBag.Services = dropdownData.ServicesModel?.Select(s => new SelectListItem
            {
                Value = s.ServiceId.ToString(),
                Text = s.EserviceName
            }).ToList();

            ViewBag.RequestTypes = dropdownData.RequestTypesModel?.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.NameAr
            }).ToList();

            ViewBag.TransactionTypes = dropdownData.transactionTypesModel?.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.NameAr
            }).ToList();
            ViewBag.LicenceTypes = dropdownData.LicenceTypesModel?.Select(l => new SelectListItem
            {
                Value = l.Id.ToString(),
                Text = l.NameAr
            }).ToList();

            return View(new MoiEserviceLicenseInfo());
        }
        [HttpPost]
        public async Task<IActionResult> CreateLicencesInfo(MoiEserviceLicenseInfo model)
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var response = await _helperUrlApi.PostDataToApi<MoiEserviceLicenseInfo,MoiEserviceLicenseInfo>($"{apiSettings}AddLicenseInfo",model);

            return RedirectToAction("GetLicencesInfo","dynamic");
        }
        public async Task<IActionResult> EditLicencesInfo(int id)
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var url = $"{apiSettings}GetLicenseInfoWithDropDown?id={id}";
            var result = await _helperUrlApi.GetDataFromApi<LicenseEditViewModel>(url);

            if (result == null || result.License == null)
                return View("Error");

            // Prepare dropdown ViewBags
            ViewBag.ActivityTypes = result.ActivityTypesModel.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.NameAr,
                Selected = a.Id == result.License.ActvityTypeId
            }).ToList();

            ViewBag.TypeBranches = result.EserviceTypeBranchModel.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.EserviceTypeBranchAr,
                Selected = e.Id == result.License.EserviceTypeBranchId
            }).ToList();

            ViewBag.Services = result.ServicesModel.Select(s => new SelectListItem
            {
                Value = s.ServiceId.ToString(),
                Text = s.EserviceName,
                Selected = s.ServiceId == result.License.ServiceId
            }).ToList();

            ViewBag.RequestTypes = result.RequestTypesModel.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.NameAr,
                Selected = r.Id == result.License.ReqTypeId
            }).ToList();
            ViewBag.LicenceTypes = result.LicenceTypesModel?.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.NameAr,
                Selected = r.Id == result.License.LicTypeId
            }).ToList();
            ViewBag.TransactionTypes = result.TransactionTypesModel.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.NameAr,
                Selected = t.Id == result.License.TransTypeId
            }).ToList();

            return View(result.License);
        }
        [HttpPost]
        public async Task<IActionResult> EditLicencesInfo(MoiEserviceLicenseInfo model)
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var response = await _helperUrlApi.PostDataToApi<MoiEserviceLicenseInfo, MoiEserviceLicenseInfo>($"{apiSettings}UpdateLicenseInfo", model);

            return RedirectToAction("GetLicencesInfo");
        }

        [HttpPost]
        public async Task<IActionResult> CloneAndSave(int id)
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            var url = $"{apiSettings}GetLicenseInfoWithDropDown?id={id}";
            var result = await _helperUrlApi.GetDataFromApi<LicenseEditViewModel>(url);

            if (result == null || result.License == null)
                return View("Error");

            var licenseToCopy=new MoiEserviceLicenseInfo();

            // Clean fields to make it a new record
            licenseToCopy.Id = 0; // Remove ID
            licenseToCopy.Url = result.License.Url;
            licenseToCopy.Action = result.License.Action;
            licenseToCopy.Sort = result.License.Sort;
            licenseToCopy.Branch = result.License.Branch;
            licenseToCopy.ServiceId = result.License.ServiceId;
            licenseToCopy.ActvityTypeId = result.License.ActvityTypeId;
            licenseToCopy.Conditions = result.License.Conditions;
            licenseToCopy.Controller = result.License.Controller;
            licenseToCopy.Description = result.License.Description;
            licenseToCopy.EserviceTypeBranchId = result.License.EserviceTypeBranchId;
            licenseToCopy.FixedFees = result.License.FixedFees;
            licenseToCopy.ReqTypeId = result.License.ReqTypeId;
            licenseToCopy.RequiredDocuments = result.License.RequiredDocuments;
            licenseToCopy.LicTypeId = result.License.LicTypeId;
            licenseToCopy.TransTypeId = result.License.TransTypeId;
            licenseToCopy.VariableFees = result.License.VariableFees;
            licenseToCopy.Measures = result.License.Measures;
            licenseToCopy.Name = result.License.Name;
            licenseToCopy.Status = result.License.Status;
           

            // Call API to create new record
            var response = await _helperUrlApi.PostDataToApi<MoiEserviceLicenseInfo, MoiEserviceLicenseInfo>(
                $"{apiSettings}AddLicenseInfo", licenseToCopy
            );

            if (response == null)
            {
                TempData["Error"] = "فشل في نسخ البيانات";
                return RedirectToAction("GetLicencesInfo", "dynamic");
            }

            TempData["Success"] = "تم إنشاء نسخة جديدة بنجاح";
            return RedirectToAction("GetLicencesInfo", "dynamic");
        }


        #endregion
        #region GetEserviceTypeBranch
        public async Task<IActionResult> GetEserviceTypeBranch()
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var response = await _helperUrlApi.GetDataFromApi<List<EserviceTypeBranch>>($"{apiSettings}GetAllTypeBranch");

            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> CreateEserviceTypeBranch()
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var apiUrl = $"{_baseUrl}Dynamic/GetEserviceTypeBranchDropDown";
            var dropdownData = await _helperUrlApi.GetDataFromApi<EserviceTypeBranchViewModel>(apiUrl);

            if (dropdownData == null)
                return View("Error");

            ViewBag.ActivityTypes = dropdownData.ActivityTypes.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.NameAr
            }).ToList();

            ViewBag.RequestTypes = dropdownData.RequestTypes?.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.NameAr
            }).ToList();

           

            return View(new EserviceTypeBranch());
        }
   
        public async Task<IActionResult> EditEserviceTypeBranch(int id)
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var url = $"{apiSettings}GetEserviceTypeBranchWithIdDropDown?id={id}";
            var result = await _helperUrlApi.GetDataFromApi<EserviceTypeBranchViewModel>(url);

            if (result == null )
                return View("Error");

            // Prepare dropdown ViewBags
            ViewBag.ActivityTypes = result.ActivityTypes.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.NameAr,
                Selected = a.Id == result.Branch.ActivityTypesId
            }).ToList();

          

            ViewBag.RequestTypes = result.RequestTypes.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.NameAr,
                Selected = r.Id == result.Branch.RequestTypeId
            }).ToList();

            

            return View(result.Branch);
        }
        [HttpPost]
        public async Task<IActionResult> CloneEserviceTypeBranch(int id)
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            var url = $"{apiSettings}GetByIdTypeBranch?id={id}";
            var result = await _helperUrlApi.GetDataFromApi<EserviceTypeBranch>(url);

            if (result == null )
                return View("Error");

            var clone = new EserviceTypeBranch();

            // Reset fields for copy
            result.Id = 0;
            clone.Fees = result.Fees;
            clone.Sort = result.Sort;
            clone.Status = result.Status;
            clone.CreatedOn = DateTime.Now;
            clone.IsDeleted = false;
            clone.ActivityTypesId = result.ActivityTypesId;
            clone.RequestTypeId = result.RequestTypeId;
            clone.Url = result.Url;
            clone.EserviceTypeBranchEn = result.EserviceTypeBranchEn;
            clone.EserviceTypeBranchAr = result.EserviceTypeBranchAr;


            // Send as new item
            var response = await _helperUrlApi.PostDataToApi<EserviceTypeBranch, EserviceTypeBranch>(
                $"{apiSettings}CreateTypeBranch", clone
            );

            if (response == null)
            {
                TempData["Error"] = "فشل في نسخ الفرع";
                return RedirectToAction("GetEserviceTypeBranch", "dynamic");
            }

            TempData["Success"] = "تم إنشاء نسخة جديدة بنجاح";
            return RedirectToAction("GetEserviceTypeBranch", "dynamic");
        }

        #endregion


        #region GetValidEserviceCombinations
        public async Task<IActionResult> GetValidEserviceCombinations()
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var response = await _helperUrlApi.GetDataFromApi<List<ValidEserviceCombinations>>($"{apiSettings}GetValidEserviceCombinations");

            return View(response);
        }
        [HttpGet]
        public async Task<IActionResult> CreateValidEserviceCombinations()
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var apiUrl = $"{_baseUrl}Dynamic/GetValidEserviceDropDown";
            var dropdownData = await _helperUrlApi.GetDataFromApi<ValidEserviceHomePage>(apiUrl);

            if (dropdownData == null)
                return View("Error");

            ViewBag.ActivityTypes = dropdownData.ActivityTypesModel.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.NameAr
            }).ToList();

           

            ViewBag.RequestTypes = dropdownData.RequestTypesModel?.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.NameAr
            }).ToList();

            ViewBag.LicencesType = dropdownData.LicenceTypesLookup?.Select(t => new SelectListItem
            {
                Value = t.Id.ToString(),
                Text = t.NameAr
            }).ToList();

            return View(new ValidEserviceCombinations());
        }
        
        public async Task<IActionResult> EditValidEserviceCombinations(int id)
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var url = $"{apiSettings}GetValidEserviceWithIdDropDown?id={id}";
            var result = await _helperUrlApi.GetDataFromApi<ValidEserviceHomePage>(url);

            if (result == null || result.ValidEserviceCombinations == null)
                return View("Error");

            // Prepare dropdown ViewBags
            ViewBag.ActivityTypes = result.ActivityTypesModel.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.NameAr,
                Selected = a.Id == result.ValidEserviceCombinations.ActivityTypeId
            }).ToList();

            ViewBag.LicencesType = result.LicenceTypesLookup.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.NameAr,
                Selected = e.Id == result.ValidEserviceCombinations.LicenceTypeId
            }).ToList();

           

            ViewBag.RequestTypes = result.RequestTypesModel.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.NameAr,
                Selected = r.Id == result.ValidEserviceCombinations.RequestTypeId
            }).ToList();

           

            return View(result.ValidEserviceCombinations);
        }
        [HttpPost]
        public async Task<IActionResult> EditValidEserviceCombinations(ValidEserviceCombinations model)
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var response = await _helperUrlApi.PostDataToApi<ValidEserviceCombinations, ValidEserviceCombinations>($"{apiSettings}EditValidEserviceCombination", model);

            return RedirectToAction("GetValidEserviceCombinations");
        }

        #endregion

        #region  WorkFlowButtonAction
        [HttpGet]
        public async Task<IActionResult> GetAllWorkFlowActionButtons()
        {
            var apiUrl = $"{_baseUrl}Dynamic/GetAllWorkFlowActionButtons";
            var buttons = await _helperUrlApi.GetDataFromApi<List<WorkFlowActionButtonVM>>(apiUrl);
            return View(buttons);
        }

        // GET: Show Add Form
        [HttpGet]
        public async Task<IActionResult> AddWorkFlowActionButton()
        {
            // Load workflows for dropdown
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var workflows = await _helperUrlApi.GetDataFromApi<List<WorkFlowVM>>($"{_baseUrl}Dynamic/GetAllWorkflows");

            var model = new WorkFlowActionButtonVM
            {
                WorkFlows = workflows.Select(w => new SelectListItem
                {
                    Text = $"{w.ServiceName} - {w.RequestTypeName} ({w.CurrentStatusName} → {w.NextStatusName})",
                    Value = w.Id.ToString()
                }).ToList()
            };

            return View(model);
        }

        // POST: Add new WorkFlowActionButton
        //[HttpPost]
        //public async Task<IActionResult> AddWorkFlowActionButton(WorkFlowActionButtonVM model)
        //{

        //    var response = await _helperUrlApi.PostDataToApi<WorkFlowActionButtonVM, WorkFlowActionButtonVM>(
        //        $"{_baseUrl}Dynamic/AddWorkFlowActionButton", model);

        //    if (response != null)
        //        TempData["Success"] = "تمت الإضافة بنجاح";
        //    else
        //        TempData["Error"] = "فشلت عملية الإضافة";

        //    return RedirectToAction("GetAllWorkFlowActionButtons");
        //}

        // GET: Show Edit Form
        [HttpGet]
        public async Task<IActionResult> UpdateWorkFlowActionButton(int id)
        {
            var apiSettings = $"{_baseUrl}Dynamic/";
            ViewBag.ApiBaseUrl = apiSettings;
            var apiUrl = $"{_baseUrl}Dynamic/GetByIdWorkFlowActionButton/{id}";
            var button = await _helperUrlApi.GetDataFromApi<WorkFlowActionButtonVM>(apiUrl);

            var workflows = await _helperUrlApi.GetDataFromApiNewHttpClient<List<WorkFlowVM>>($"{_baseUrl}Dynamic/GetAllWorkflows");

            button.WorkFlows = workflows.Select(w => new SelectListItem
            {
                Text = $"{w.ServiceName} - {w.RequestTypeName} ({w.CurrentStatusName} → {w.NextStatusName})",
                Value = w.Id.ToString()
            }).ToList();

            return View(button);
        }

        // POST: Update WorkFlowActionButton
        [HttpPost]
        public async Task<IActionResult> UpdateWorkFlowActionButton(WorkFlowActionButton model)
        {
            var response = await _helperUrlApi.PostDataToApi<WorkFlowActionButton, object>(
                $"{_baseUrl}Dynamic/UpdateWorkFlowActionButton/{model.Id}", model);

            if (response != null)
                TempData["Success"] = "تم التعديل بنجاح";
            else
                TempData["Error"] = "فشل التعديل";

            return RedirectToAction("GetAllWorkFlowActionButtons");
        }

        // GET: Show Delete Confirmation
        [HttpGet]
        public async Task<IActionResult> DeleteWorkFlowActionButton(int id)
        {
            var apiUrl = $"{_baseUrl}Dynamic/DeleteWorkFlowActionButton/{id}";

            var result = await _helperUrlApi.PostDataToApi<object, ErrorMessage>(apiUrl, null);

            if (result != null && result.Error == false)
            {
                TempData["SuccessMessage"] = result.Message ?? "تم حذف الزر بنجاح";
            }
            else
            {
                TempData["ErrorMessage"] = result?.Message ?? "حدث خطأ أثناء الحذف";
            }

            return RedirectToAction("GetAllWorkFlowActionButtons");
        }


        // POST: Confirm Delete
        //[HttpPost]
        //public async Task<IActionResult> DeleteWorkFlowActionButtonConfirmed(int id)
        //{
        //    var response = await _helperUrlApi.PostDataToApi<object, object>(
        //        $"{_baseUrl}Dynamic/DeleteWorkFlowActionButton/{id}", null);

        //    if (response != null)
        //        TempData["Success"] = "تم الحذف بنجاح";
        //    else
        //        TempData["Error"] = "فشل الحذف";

        //    return RedirectToAction("GetAllWorkFlowActionButtons");
        //}
        #endregion
        #region WorkFlow RoleAdminButton
        [HttpGet]
        public async Task<IActionResult> GetAllWorkFlowButtonRoleAdmins()
        {
            var apiUrl = $"{_baseUrl}Dynamic/GetAllWorkFlowButtonRoleAdmin";
            var data = await _helperUrlApi.GetDataFromApi<List<WorkFlowButtonRoleAdmin>>(apiUrl);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> AddWorkFlowButtonRoleAdmin()
        {
            var buttons = await _helperUrlApi.GetDataFromApiNewHttpClient<List<WorkFlowActionButton>>($"{_baseUrl}Dynamic/GetAllWorkFlowActionButtons");
            var roles = await _helperUrlApi.GetDataFromApiNewHttpClient<List<RoleAdmin>>($"{_baseUrl}Roles/GetRoleAdmin");

            ViewBag.Buttons = buttons.Select(b => new SelectListItem { Text = b.ButtonText, Value = b.Id.ToString() }).ToList();
            ViewBag.Roles = roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() }).ToList();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkFlowButtonRoleAdmin(WorkFlowButtonRoleAdmin model)
        {
            var result = await _helperUrlApi.PostDataToApi<WorkFlowButtonRoleAdmin, object>($"{_baseUrl}Dynamic/AddWorkFlowButtonRoleAdmin", model);
            return RedirectToAction("GetAllWorkFlowButtonRoleAdmins");
        }

        [HttpGet]
        public async Task<IActionResult> UpdateWorkFlowButtonRoleAdmin(int id)
        {
            var item = await _helperUrlApi.GetDataFromApiNewHttpClient<WorkFlowButtonRoleAdmin>($"{_baseUrl}Dynamic/GetByIdWorkFlowButtonRoleAdmin/{id}");

            var buttons = await _helperUrlApi.GetDataFromApiNewHttpClient<List<WorkFlowActionButton>>($"{_baseUrl}Dynamic/GetAllWorkFlowActionButtons");
            var roles = await _helperUrlApi.GetDataFromApiNewHttpClient<List<RoleAdmin>>($"{_baseUrl}Roles/GetRoleAdmin");

            ViewBag.Buttons = buttons.Select(b => new SelectListItem { Text = b.ButtonText, Value = b.Id.ToString() }).ToList();
            ViewBag.Roles = roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() }).ToList();

            return View(item);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateWorkFlowButtonRoleAdmin(int id, WorkFlowButtonRoleAdmin model)
        {
            var result = await _helperUrlApi.PostDataToApi<WorkFlowButtonRoleAdmin, object>($"{_baseUrl}Dynamic/UpdateWorkFlowButtonRoleAdmin/{id}", model);
            return RedirectToAction("GetAllWorkFlowButtonRoleAdmins");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteWorkFlowButtonRoleAdmin(int id)
        {
            var result = await _helperUrlApi.PostDataToApi<object, object>($"{_baseUrl}Dynamic/DeleteWorkFlowButtonRoleAdmin/{id}", null);
            return RedirectToAction("GetAllWorkFlowButtonRoleAdmins");
        }

        #endregion

    }
}
