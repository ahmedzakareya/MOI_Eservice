using Business.Enums;
using Business.Helpers;
using Business.ViewModel;
using Business.ViewModel.Dynamic;
using Business.ViewModel.Tourism;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace MOI_Eservice.Controllers
{
    public class PublishingController : Controller
    {
        private readonly ILogger<PublishingController> _logger;
        private readonly HelperUrlApi _helperUrlApi;
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly IConfiguration _configuration;


        public PublishingController(IConfiguration configuration, ILogger<PublishingController> logger, HelperUrlApi helperUrlApi, HttpClient httpClient)
        {
            _logger = logger;
            _helperUrlApi = helperUrlApi;
            _httpClient = httpClient;
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _configuration = configuration;

        }





        public async Task<IActionResult> PressNewRequestCompany()
        {
            MoiEserviceLicensesRequestVM model = new MoiEserviceLicensesRequestVM()
            {

                LicenTypeName = "شركة",
                countriesLookups = await FetchCountriesAsync(),
                attachRules = await FetchAttachmentsAsync("NewCompanyRequest"),
                ActivityTypes = await FetchActivitiesAsync((int) ServiceEnum.LocalPress),
                testablishContracts = await FetchTestablishContractAsync(),
                pesronTypeLookUp = await FetchPesronTypesAsync(),
                qualificationsLookups = await FetchQualificationsAsync(),

            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> PressNewRequestCompany(MoiEserviceLicensesRequestVM model)
        {
            List<PartnerVM> partners = new();

            if (Request.Form.TryGetValue("PartnersJson", out var sv) && sv.Count > 0)
            {
                // take the last non-empty value (in case the field is posted multiple times)
                string partnersJson = sv.Reverse().FirstOrDefault(v => !string.IsNullOrWhiteSpace(v));

                if (!string.IsNullOrWhiteSpace(partnersJson))
                {
                    try
                    {
                        partners = JsonConvert.DeserializeObject<List<PartnerVM>>(partnersJson) ?? new();
                    }
                    catch (JsonReaderException)
                    {
                        ModelState.AddModelError("", "حدث خطأ أثناء معالجة قائمة الشركاء (صيغة JSON غير صحيحة).");
                    }
                }
            }

            model.Partners = partners;

            var userId = HttpContext.Session.GetString("UserId");
            model.Requesterid = userId;

            var result = await SendCompanyRequestAsync(model);

            if (result != null)
            {
                var uploadedFiles = Request.Form.Files;
                var attachInfoValues = Request.Form["AttachInfo"];

                var attachmentList = new List<AttachVM>();

                var baseFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", "Publishing");

                var reqnoFolderName = string.IsNullOrWhiteSpace(result.Reqno) ? $"REQ_{result.RequestId}" : result.Reqno;

                var invalid = Path.GetInvalidFileNameChars();
                var safeReqno = new string(reqnoFolderName.Where(ch => !invalid.Contains(ch)).ToArray());

                var reqnoFolderPath = Path.Combine(baseFolder, safeReqno);
                Directory.CreateDirectory(reqnoFolderPath); // ينشئ إن لم يكن موجودًا

                foreach (var attachInfo in attachInfoValues)
                {
                    var parts = attachInfo.Split('|');
                    if (parts.Length != 3) continue;

                    var attachId = parts[0];
                    var originalFileName = parts[1];
                    var attachLabel = parts[2];

                    var uploadedFile = uploadedFiles.FirstOrDefault(f => f.FileName == originalFileName);
                    if (uploadedFile == null || uploadedFile.Length == 0) continue;

                    var ext = Path.GetExtension(uploadedFile.FileName)?.ToLowerInvariant();
                    if (string.IsNullOrEmpty(ext)) ext = ".bin";

                    var uniqueName = $"{result.Reqno}_{Guid.NewGuid()}{ext}";

                    var filePath = Path.Combine(reqnoFolderPath, uniqueName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(stream);
                    }

                    var relativePath = Path.Combine(safeReqno, uniqueName).Replace("\\", "/");

                    attachmentList.Add(new AttachVM
                    {
                        AttachId = long.Parse(attachId),
                        AttachName = attachLabel,
                        AttachPath = relativePath,
                        UploadedDate = DateTime.Now,
                        UploadedBy = User.Identity?.Name ?? "System",
                        IsApproved = false,
                        IsDeleted = false,
                        IsLatest = true,
                        AttachRequestid = result.RequestId
                    });
                }



                model.attach = attachmentList;

                await _helperUrlApi.PostDataToApi<List<AttachVM>, ErrorMessage>(
                    "api/Publishing/SaveAttachments",
                    model.attach
                );
                TempData["SuccessMessage"] = "تم إرسال الطلب بنجاح";
                return RedirectToAction("GetAllRequest", "Home");
            }
            else
            {
                return View(model);
            }
        }



        public async Task<IActionResult> PublishNewRequest()
        {
            MoiEserviceLicensesRequestVM model = new MoiEserviceLicensesRequestVM()
            {

                LicenTypeName = "شركة",
                countriesLookups = await FetchCountriesAsync(),
                attachRules = await FetchAttachmentsAsync("NewCompanyRequest"),
                ActivityTypes = await FetchActivitiesAsync(5),
                testablishContracts = await FetchTestablishContractAsync(),
                pesronTypeLookUp = await FetchPesronTypesAsync(),
                qualificationsLookups = await FetchQualificationsAsync(),

            };
            return View(model);
        }



        private async Task<MoiEserviceLicensesRequestVM?> SendCompanyRequestAsync(MoiEserviceLicensesRequestVM model)
        {
            try
            {
                var apiUrl = $"{_baseUrl}api/Publishing/AddNewCompanyRequest";

                var result = await _helperUrlApi.PostDataToApi<MoiEserviceLicensesRequestVM, MoiEserviceLicensesRequestVM>(apiUrl, model);

                if (result != null && result.RequestId > 0)
                {
                    TempData["SuccessMessage"] = "تم تنفيذ العملية بنجاح";
                    return result;
                }
                else
                {
                    TempData["ErrorMessage"] = "حدث خطأ أثناء تنفيذ الطلب";
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while sending company request to API.");
                TempData["ErrorMessage"] = "حدث خطأ غير متوقع أثناء الاتصال بالخادم.";
                return null;
            }
        }



        // personal REQUEST 
        public async Task<IActionResult> PressNewRequestPersonal()
        {
            MoiEserviceLicensesRequestVM model = new MoiEserviceLicensesRequestVM()
            {

                LicenTypeName = "مؤسسة",
                countriesLookups = await FetchCountriesAsync(),
                attachRules = await FetchAttachmentsAsync("NewPersonalRequestPublishing"),
                ActivityTypes = await FetchActivitiesAsync((int) ServiceEnum.publishing),
                pesronTypeLookUp = await FetchPesronTypesAsync(),
                qualificationsLookups = await FetchQualificationsAsync(),

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

        private async Task<List<ActivityTypeVM>> FetchActivitiesAsync(int id)
        {
            try
            {
                var requestUrl = $"api/Publishing/GetActivity?ID={id}";

                var response = await _helperUrlApi.GetDataFromApi<List<ActivityTypeVM>>(requestUrl);
                return response ?? new List<ActivityTypeVM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching activity info.");
                return new List<ActivityTypeVM>();
            }
        }

        private async Task<List<TestablishContractVM>> FetchTestablishContractAsync()
        {
            try
            {
                var requestUrl = $"api/Publishing/GetTestablishContract";

                var response = await _helperUrlApi.GetDataFromApi<List<TestablishContractVM>>(requestUrl);
                return response ?? new List<TestablishContractVM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Testablish Contract info.");
                return new List<TestablishContractVM>();
            }
        }

        private async Task<List<PesronTypeLookUpVM>> FetchPesronTypesAsync()
        {
            try
            {
                var requestUrl = $"api/Publishing/GetPesronTypes";

                var response = await _helperUrlApi.GetDataFromApi<List<PesronTypeLookUpVM>>(requestUrl);
                return response ?? new List<PesronTypeLookUpVM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Pesron Types info.");
                return new List<PesronTypeLookUpVM>();
            }
        }


        private async Task<List<QualificationsLookupVM>> FetchQualificationsAsync()
        {
            try
            {
                var requestUrl = $"api/Publishing/GetQualificationsLookup";

                var response = await _helperUrlApi.GetDataFromApi<List<QualificationsLookupVM>>(requestUrl);
                return response ?? new List<QualificationsLookupVM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching Qualifications info.");
                return new List<QualificationsLookupVM>();
            }
        }

        private async Task<List<Business.ViewModel.AttachRuleVM>> FetchAttachRulesAsync(int activityTypeId, int serviceId, int requestTypeId, int requestStatusId)
        {
            try
            {
                var requestUrl = $"api/Channels/GetAttchRule?ActivityTypeId={activityTypeId}&ServiceId={serviceId}&RequestTypeId={requestTypeId}&RequestStatusId={requestStatusId}";
                var response = await _helperUrlApi.GetDataFromApi<List<Business.ViewModel.AttachRuleVM>>(requestUrl);
                return response ?? new List<Business.ViewModel.AttachRuleVM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching attachment rules.");
                return new List<Business.ViewModel.AttachRuleVM>();
            }
        }

        private async Task<List<Business.ViewModel.AttachRuleVM>> FetchAttachmentsAsync(string viewType)
        {
            try
            {
                var requestUrl = $"api/Publishing/GetAttachmentForRequest?viewType={viewType}";

                var response = await _helperUrlApi.GetDataFromApi<List<Business.ViewModel.AttachRuleVM>>(requestUrl);
                return response ?? new List<Business.ViewModel.AttachRuleVM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching attachment rules.");
                return new List<Business.ViewModel.AttachRuleVM>();
            }
        }

        public async Task<IActionResult> RequestDetails(string id)
        {
            try
            {
                int ReqID = 0;
                if (int.TryParse(MyCrypto.Decode(id), out ReqID))
                {
                    ViewBag.PathAttachment = "Files/Publishing/";

                    var apiSetting = _baseUrl + $"api/Publishing/GetRequestDetails/{ReqID}";



                    var response = await _helperUrlApi.GetDataFromApi<RequestFrontVM>(apiSetting);

                    return View(response);
                }
                else
                {
                    return RedirectToAction("RequestsList");
                }

            }
            catch (Exception ex)
            {
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                string fileName = controllerName + "_" + actionName + "_";

                string exId = ExceptionLog.LogException(ex, fileName);

                TempData["Ex"] = exId;
                throw;
            }
        }



        private async Task<LicenseModifyModel> FetchLicenseModifyModelAsync(int licenseID)
        {
            try
            {
                var requestUrl = $"api/Publishing/LicenseModifyModel?licenseID={licenseID}";
                var response = await _helperUrlApi.GetDataFromApi<LicenseModifyModel>(requestUrl);
                return response ?? new LicenseModifyModel
                {
                    LiceID = licenseID,
                    transactionVm = new List<TransactionTypesLookupVM>(),

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching LicenseModifyModel for licenseID {LicenseID}", licenseID);
                return new LicenseModifyModel
                {
                    LiceID = licenseID,

                    transactionVm = new List<TransactionTypesLookupVM>()
                };
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadTempFiles()
        {
            try
            {
                if (!Request.HasFormContentType)
                    return Json(new { ok = true, tempKey = (string?)null });

                var form = Request.Form;
                var files = form.Files;

                if (files == null || files.Count == 0)
                    return Json(new { ok = true, tempKey = (string?)null });

                string tempKey = form["TempKey"].FirstOrDefault();
                if (string.IsNullOrWhiteSpace(tempKey))
                    tempKey = Guid.NewGuid().ToString("N");

                var baseTemp = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", "TempUploads", tempKey);
                Directory.CreateDirectory(baseTemp);

                foreach (var f in files)
                {
                    if (f.Length <= 0) continue;
                    var ext = Path.GetExtension(f.FileName) ?? "";
                    if (!ext.Equals(".pdf", StringComparison.OrdinalIgnoreCase)) continue;

                    var safeName = $"{Guid.NewGuid():N}{ext}";
                    var fullPath = Path.Combine(baseTemp, safeName);
                    using (var fs = System.IO.File.OpenWrite(fullPath))
                        await f.CopyToAsync(fs);

                    var metaPath = fullPath + ".meta.txt";
                    await System.IO.File.WriteAllTextAsync(metaPath, f.FileName);
                }

                return Json(new { ok = true, tempKey });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { ok = false, message = ex.Message });
            }
        }



        [HttpPost]
        public async Task<IActionResult> LicenseModifyRequest([FromBody] LicenseModifyModel model)
        {
            var tempKey = Request.Headers["X-Attach-TempKey"].ToString();
            var attachInfoJson64 = Request.Headers["X-Attach-Info-Json64"].ToString();
            List<string> attachInfoFromPreUpload = new List<string>();
            if (!string.IsNullOrWhiteSpace(attachInfoJson64))
            {
                try
                {
                    var bytes = Convert.FromBase64String(attachInfoJson64);
                    var json = Encoding.UTF8.GetString(bytes);
                    attachInfoFromPreUpload = JsonConvert.DeserializeObject<List<string>>(json) ?? new List<string>();
                }
                catch { /* ignore */ }
            }

            if (model == null)
            {
                Response.StatusCode = 400;
                return Json(new { ok = false, message = "No data received." });
            }

            if (model.ChangeOldPartnerTransVM == null || model.changeNewPartnerTransVM == null)
            {
                Request.EnableBuffering();
                string raw;
                using (var reader = new StreamReader(Request.Body, Encoding.UTF8, false, leaveOpen: true))
                    raw = await reader.ReadToEndAsync();
                Request.Body.Position = 0;

                try
                {
                    var root = JObject.Parse(raw);

                    if (model.ChangeOldPartnerTransVM == null &&
                        root.TryGetValue("ChangeOldPartnerTransVM", StringComparison.OrdinalIgnoreCase, out JToken oldTok) &&
                        oldTok is JArray)
                    {
                        model.ChangeOldPartnerTransVM =
                            JsonConvert.DeserializeObject<List<ChangeOldPartnerTransVM>>(oldTok.ToString())
                            ?? new List<ChangeOldPartnerTransVM>();
                    }

                    if (model.changeNewPartnerTransVM == null &&
                        root.TryGetValue("changeNewPartnerTransVM", StringComparison.OrdinalIgnoreCase, out JToken newTok) &&
                        newTok is JArray)
                    {
                        model.changeNewPartnerTransVM =
                            JsonConvert.DeserializeObject<List<ChangeNewPartnerTransVM>>(newTok.ToString())
                            ?? new List<ChangeNewPartnerTransVM>();
                    }
                }
                catch { /* ignore */ }
            }

            // ============ 3) Normalize ============
            model.ChangeOldPartnerTransVM ??= new List<ChangeOldPartnerTransVM>();
            model.changeNewPartnerTransVM ??= new List<ChangeNewPartnerTransVM>();
            model.FeesByTypeId ??= new Dictionary<int, decimal>();

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(kv => kv.Value?.Errors.Any() == true)
                    .ToDictionary(kv => kv.Key, kv => kv.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

                Response.StatusCode = 422;
                return Json(new { ok = false, message = "Validation failed.", errors });
            }

            try
            {
                var userId = HttpContext.Session.GetString("UserId");
                model.moiEserviceLicensesRequestVM ??= new MoiEserviceLicensesRequestVM();
                model.moiEserviceLicensesRequestVM.Requesterid = userId;

                var apiResult = await SendModifyRequestAsync(model);
                if (apiResult == null)
                {
                    Response.StatusCode = 502;
                    return Json(new { ok = false, message = "فشل استدعاء خدمة الإنشاء (API)." });
                }


                var apiJson = JsonConvert.SerializeObject(apiResult);
                var jo = JObject.Parse(apiJson);

                var mapToken = jo.SelectToken("RequestIdsByTypeId") ?? jo.SelectToken("requestIdsByTypeId");
                var requestIdByType = new Dictionary<int, long>();

                if (mapToken is JObject mapObj)
                {
                    foreach (var p in mapObj.Properties())
                    {
                        if (int.TryParse(p.Name, out var typeId))
                        {
                            var val = p.Value?.Value<long?>() ?? 0;
                            if (val > 0) requestIdByType[typeId] = val;
                        }
                    }
                }

                long defaultRequestId =
                    jo.SelectToken("RequestId")?.Value<long?>() ??
                    jo.SelectToken("requestId")?.Value<long?>() ??
                    0;

                if (defaultRequestId == 0)
                {
                    var created = jo.SelectToken("CreatedRequestIds") as JArray
                               ?? jo.SelectToken("createdRequestIds") as JArray;
                    if (created != null && created.Count > 0)
                        defaultRequestId = created[0]?.Value<long?>() ?? 0;
                }

                if (defaultRequestId == 0 && requestIdByType.Count > 0)
                    defaultRequestId = requestIdByType.Values.First();

                long ResolveRequestIdForTx(int? txTypeId)
                {
                    if (txTypeId.HasValue && requestIdByType.TryGetValue(txTypeId.Value, out var rid))
                        return rid;
                    return defaultRequestId;
                }

                try
                {
                    var PublishingBase = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", "Publishing");
                    var folderName = string.IsNullOrWhiteSpace((string?)jo.SelectToken("Reqno") ?? (string?)jo.SelectToken("reqno"))
                        ? $"REQ_{(jo.SelectToken("RequestId")?.Value<long?>() ?? jo.SelectToken("requestId")?.Value<long?>() ?? 0)}"
                        : ((string?)jo.SelectToken("Reqno") ?? (string?)jo.SelectToken("reqno"))!;
                    var safeFolder = new string(folderName.Where(ch => !Path.GetInvalidFileNameChars().Contains(ch)).ToArray());
                    var finalFolder = Path.Combine(PublishingBase, safeFolder);
                    Directory.CreateDirectory(finalFolder);

                    var attachList = new List<AttachVM>();

                    if (!string.IsNullOrWhiteSpace(tempKey) && attachInfoFromPreUpload.Count > 0)
                    {
                        var tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", "TempUploads", tempKey);
                        if (Directory.Exists(tempFolder))
                        {
                            var metaFiles = Directory.GetFiles(tempFolder, "*.meta.txt", SearchOption.TopDirectoryOnly);

                            foreach (var info in attachInfoFromPreUpload)
                            {
                                // الشكل: attachId|originalFileName|label|transactionTypeId?
                                var parts = (info ?? string.Empty).Split('|');
                                if (parts.Length < 3) continue;

                                var attachIdStr = parts[0];
                                var originalFileName = parts[1];
                                var attachLabel = parts[2];

                                int? txTypeId = null;
                                if (parts.Length >= 4 && int.TryParse(parts[3], out var txx))
                                    txTypeId = txx;

                                if (!long.TryParse(attachIdStr, out var attachId))
                                    continue;

                                var matchMeta = metaFiles.FirstOrDefault(m =>
                                {
                                    var metaText = System.IO.File.ReadAllText(m).Trim();
                                    return string.Equals(metaText, originalFileName, StringComparison.OrdinalIgnoreCase);
                                });
                                if (matchMeta == null) continue;

                                var pdfPath = matchMeta[..^(".meta.txt".Length)];
                                if (!System.IO.File.Exists(pdfPath)) continue;

                                var ext = Path.GetExtension(pdfPath) ?? ".pdf";
                                var uniqueName = $"{safeFolder}_{Guid.NewGuid():N}{ext}";
                                var finalPath = Path.Combine(finalFolder, uniqueName);
                                System.IO.File.Copy(pdfPath, finalPath, overwrite: false);

                                var relativePath = Path.Combine(safeFolder, uniqueName).Replace("\\", "/");

                                var requestIdForThisFile = ResolveRequestIdForTx(txTypeId);

                                attachList.Add(new AttachVM
                                {
                                    AttachId = attachId,
                                    AttachName = attachLabel,
                                    AttachPath = relativePath,
                                    UploadedDate = DateTime.Now,
                                    UploadedBy = User.Identity?.Name ?? "System",
                                    IsApproved = false,
                                    IsDeleted = false,
                                    IsLatest = true,
                                    AttachRequestid = requestIdForThisFile,
                                    TransactionTypeId = txTypeId
                                });
                            }

                            if (attachList.Count > 0)
                            {
                                await _helperUrlApi.PostDataToApi<List<AttachVM>, ErrorMessage>(
                                    "api/Publishing/SaveAttachments", attachList);
                            }

                            try { Directory.Delete(tempFolder, true); } catch { }
                        }
                    }
                    else if (Request.HasFormContentType)
                    {
                        var form = Request.Form;
                        var uploadedFiles = form.Files;
                        var infos = form["AttachInfo"];

                        if (uploadedFiles != null && uploadedFiles.Count > 0 && infos.Count > 0)
                        {
                            foreach (var info in infos)
                            {
                                var parts = (info ?? string.Empty).Split('|');
                                if (parts.Length < 3) continue;

                                var attachIdStr = parts[0];
                                var originalFileName = parts[1];
                                var attachLabel = parts[2];

                                int? txTypeId = null;
                                if (parts.Length >= 4 && int.TryParse(parts[3], out var txx))
                                    txTypeId = txx;

                                var file = uploadedFiles.FirstOrDefault(f => f.FileName == originalFileName);
                                if (file == null || file.Length == 0) continue;

                                if (!long.TryParse(attachIdStr, out var attachId)) continue;

                                var ext = Path.GetExtension(file.FileName) ?? ".pdf";
                                var uniqueName = $"{safeFolder}_{Guid.NewGuid():N}{ext}";
                                var finalPath = Path.Combine(finalFolder, uniqueName);
                                using (var fs = System.IO.File.OpenWrite(finalPath))
                                    await file.CopyToAsync(fs);

                                var relativePath = Path.Combine(safeFolder, uniqueName).Replace("\\", "/");

                                var requestIdForThisFile = ResolveRequestIdForTx(txTypeId);

                                attachList.Add(new AttachVM
                                {
                                    AttachId = attachId,
                                    AttachName = attachLabel,
                                    AttachPath = relativePath,
                                    UploadedDate = DateTime.Now,
                                    UploadedBy = User.Identity?.Name ?? "System",
                                    IsApproved = false,
                                    IsDeleted = false,
                                    IsLatest = true,
                                    AttachRequestid = requestIdForThisFile,
                                    TransactionTypeId = txTypeId
                                });
                            }

                            if (attachList.Count > 0)
                            {
                                await _helperUrlApi.PostDataToApi<List<AttachVM>, ErrorMessage>(
                                    "api/Publishing/SaveAttachments", attachList);
                            }
                        }
                    }
                }
                catch (Exception exAttach)
                {
                    Console.WriteLine("[AttachmentsFinalize] " + exAttach.Message);
                }

                var redirect = Url.Action("LicenseModifySuccess", "Publishing", new { reqno = (string?)jo.SelectToken("Reqno") ?? (string?)jo.SelectToken("reqno") });
                return Json(new { ok = true, message = "Saved successfully.", redirectUrl = redirect, data = apiResult });
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { ok = false, message = "Server error while calling API.", detail = ex.Message });
            }
        }



        [HttpPost]
        [RequestSizeLimit(10_000_000)] // optional: allow up to 10 MB
        public async Task<IActionResult> UploadLicenseModifyAttachments()
        {
            if (!Request.HasFormContentType)
                return Json(new { ok = false, message = "FormData content type expected." });

            var form = Request.Form;
            var uploadedFiles = form.Files;
            var attachInfoValues = form["AttachInfo"];
            var reqno = form["Reqno"].FirstOrDefault() ?? "";
            var requestIdStr = form["DefaultRequestId"].FirstOrDefault();
            long.TryParse(requestIdStr, out var requestId);

            if (uploadedFiles == null || uploadedFiles.Count == 0)
                return Json(new { ok = false, message = "No files uploaded." });

            var baseFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Files", "Publishing");
            var folderName = string.IsNullOrWhiteSpace(reqno) ? $"REQ_{requestId}" : reqno;
            var safeReqno = new string(folderName.Where(ch => !Path.GetInvalidFileNameChars().Contains(ch)).ToArray());
            var reqnoFolderPath = Path.Combine(baseFolder, safeReqno);
            Directory.CreateDirectory(reqnoFolderPath);

            var attachmentList = new List<AttachVM>();

            foreach (var attachInfo in attachInfoValues)
            {
                var parts = (attachInfo ?? string.Empty).Split('|');
                if (parts.Length < 3) continue;

                var attachIdStr = parts[0];
                var originalFileName = parts[1];
                var attachLabel = parts[2];
                var uploadedFile = uploadedFiles.FirstOrDefault(f => f.FileName == originalFileName);
                if (uploadedFile == null) continue;

                var ext = Path.GetExtension(uploadedFile.FileName) ?? ".pdf";
                var uniqueName = $"{safeReqno}_{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(reqnoFolderPath, uniqueName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await uploadedFile.CopyToAsync(stream);

                var relativePath = Path.Combine(safeReqno, uniqueName).Replace("\\", "/");
                if (!long.TryParse(attachIdStr, out var attachId)) continue;

                attachmentList.Add(new AttachVM
                {
                    AttachId = attachId,
                    AttachName = attachLabel,
                    AttachPath = relativePath,
                    UploadedDate = DateTime.Now,
                    UploadedBy = User.Identity?.Name ?? "System",
                    IsApproved = false,
                    IsDeleted = false,
                    IsLatest = true,
                    AttachRequestid = requestId
                });
            }

            // save to API
            if (attachmentList.Count > 0)
            {
                await _helperUrlApi.PostDataToApi<List<AttachVM>, ErrorMessage>(
                    "api/Publishing/SaveAttachments",
                    attachmentList
                );
            }

            return Json(new { ok = true, uploaded = attachmentList.Count });
        }


        private async Task<MoiEserviceLicensesRequestVM?> SendModifyRequestAsync(LicenseModifyModel model)
        {
            // Same style you used for SaveAttachments / SendCompanyRequestAsync
            // Adjust the path if your API route differs.
            const string endpoint = "api/Publishing/AddModifyRequest";
            try
            {
                var result = await _helperUrlApi.PostDataToApi<LicenseModifyModel, MoiEserviceLicensesRequestVM>(
                    endpoint,
                    model
                );
                return result;
            }
            catch
            {
                return null;
            }
        }



        [HttpGet]
        [Route("Publishing/LicenseModifyModelAjax")]
        public async Task<IActionResult> LicenseModifyModelAjax(int licenseID)
        {
            try
            {
                var model = await FetchLicenseModifyModelAsync(licenseID);

                if (model == null)
                    return NotFound(new { success = false, error = "لم يتم العثور على بيانات الترخيص." });

                return Json(new
                {
                    success = true,
                    data = model
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching LicenseModifyModel for licenseID {LicenseID}", licenseID);
                return StatusCode(500, new { success = false, error = "حدث خطأ غير متوقع أثناء جلب البيانات." });
            }
        }





        [HttpGet]
        public async Task<IActionResult> LicenseModifyRequest(int licenseID, int[] typeIds)
        {
            if (typeIds == null || typeIds.Length == 0)
            {
                TempData["Msg"] = "يجب اختيار معاملة واحدة على الأقل.";
                return RedirectToAction("GetAllLicenses");
            }

            var licenseVM = await FetchLicenseDetailsAsync(licenseID);

            string? ViewTypeFor(int t) => t switch
            {
                1 => "ChangeCompanyName",     // تغيير اسم الشركة
                2 => "ChangeCommercialName",  // تغيير الاسم التجاري
                3 => "PartnersInOut",         // دخول/خروج الشركاء
                4 => "ChangeAddress",         // تغيير العنوان
                9 => "ChangeManager",         // تغيير المدير المسؤول
                11 => "ChangeActivity",
                17 => "Renew", // التجديد
                19 => "EndLicense",
                77 => "Renouncement",  // التنازل
                // تغيير النشاط
                _ => null
            };

            const string BaseAttachmentKey = "UpdateRequest";

            var keys = typeIds
                .Select(ViewTypeFor)
                .Where(k => !string.IsNullOrWhiteSpace(k))
                .Append(BaseAttachmentKey) // ← إضافة المرفقات العامة
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            var fetched = (keys.Length > 0)
                ? await Task.WhenAll(keys.Select(k => FetchAttachmentsAsync(k!)))
                : Array.Empty<List<Business.ViewModel.AttachRuleVM>>();

            var mergedAttachments = fetched
    .Where(list => list != null)
    .SelectMany(list => list!)
    .GroupBy(a => new { Trans = (a.TransactionTypeId ?? 0), Id = a.Id, Name = a.AttachName ?? string.Empty })
    .Select(g => g.First())
    .ToList();

            var vm = new LicenseModifyModel
            {
                LiceID = licenseVM.LicId,
                TypeIds = typeIds.ToList(),
                activityTypeVMs = await FetchActivitiesAsync(5),
                LicenceDetailsVM = licenseVM,
                companyVM = licenseVM.Company,
                Manager = licenseVM.Manager,
                partnerVM = licenseVM.partnerVM,
                attachRules = mergedAttachments,
                ReasonsVM = licenseVM.moiEserviceLicEndingReasonVM,
                countriesLookupVM = licenseVM.countriesLookupVM,
                qualificationsLookupVM = licenseVM.qualificationsLookupVM,
                Applicant = licenseVM.Applicant,
                RequestFessVM = licenseVM.RequestFessVM,



            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken] // expects the token in header: RequestVerificationToken
        public ActionResult LicenseModifyRequestAjax(LicenseModifyModel model)
        {
            if (model == null)
            {
                Response.StatusCode = 400;
                return Json(new { ok = false, message = "No data received." });
            }

            if (!ModelState.IsValid)
            {
                // Collect model state errors to send back to the client
                var errors = ModelState
                    .Where(kv => kv.Value.Errors.Any())
                    .ToDictionary(
                        kv => kv.Key,
                        kv => kv.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                Response.StatusCode = 422; // Unprocessable Entity
                return Json(new { ok = false, message = "Validation failed.", errors });
            }

            try
            {
                // TODO: use your UnitOfWork/DbContext to persist the request
                // Example: iterate fees
                if (model.FeesByTypeId != null)
                {
                    foreach (var fee in model.FeesByTypeId)
                    {
                        int typeId = fee.Key;
                        decimal amount = fee.Value;
                        // TODO: save fee
                    }
                }

                // TODO: handle sub-models if not null (examples)
                // if (model.CompanyNameChangeTransactionVM != null) { ... }
                // if (model.AddressChangeTransactionVM != null) { ... }
                // if (model.partnerVM != null && model.partnerVM.Any()) { ... }

                // _unitOfWork.Complete();

                return Json(new
                {
                    ok = true,
                    message = "Saved successfully.",
                    redirectUrl = Url.Action("LicenseModifySuccess", "License")
                });
            }
            catch (System.Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { ok = false, message = "Server error.", detail = ex.Message });
            }
        }


        private async Task<LicencesVM?> FetchLicenseDetailsAsync(int id)
        {
            try
            {
                var url = $"api/Publishing/GetLicenseDetails/{id}";
                return await _helperUrlApi.GetDataFromApi<LicencesVM>(url);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching license details for id {Id}", id);
                return null;
            }
        }
        #region PACI
       
        #endregion
    }
}
