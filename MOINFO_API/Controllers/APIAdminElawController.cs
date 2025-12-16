using AutoMapper;
using Azure.Core;
using Business.Enums;
using Business.Interfaces;
using Business.ModelWithSpecification;
using Business.Repository;
using Business.ViewModel;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace MOINFO_API.Controllers
{
    [Route("api/AdminElaw")]
    public class APIAdminElawController : BaseController
    {
        private readonly IUnitOfwork _unitOfwork;
        private readonly IUpdateDataService _updateDataService;
        private readonly IMapper _mapper;
        private readonly IDataFetchService _dataFetchService;

        public APIAdminElawController(IUnitOfwork unitOfwork, IUpdateDataService updateDataService, IMapper mapper, IDataFetchService dataFetchService)
        {
            _unitOfwork = unitOfwork;
            _updateDataService = updateDataService;
            _mapper = mapper;
            _dataFetchService = dataFetchService;
        }
        #region GetStatistics
        //--------------------Get All Statistics----------
        [HttpGet]
        [Route("GetAllStatistics")]
        public async Task<StatisticsViewModel> GetAllStatistics(int ServiceId)
        {
            int[] activeRequestStatuses = { 1, 2, 3, 4 };

            var reqRepo = _unitOfwork.genericRepository<MoiEserviceLicensesRequest>();
            var transRepo = _unitOfwork.genericRepository<Transaction>();
            var licRepo = _unitOfwork.genericRepository<Licence>();

            var model = new StatisticsViewModel
            {
                AllLicences = await licRepo.Count(p => p.LicStatusId == (int)licencesStatusEnum.Released && p.ServiceId == (int)ServiceEnum.Elaw),

                AllRequests = await reqRepo.Count(r => activeRequestStatuses.Contains(r.RequestStatusId.Value) && r.ServiceId == (int)ServiceEnum.Elaw),

                NewRequests = await reqRepo.Count(r => r.RequestStatusId == (int)RequestStatusEnum.Received && r.ServiceId == (int)ServiceEnum.Elaw),

                ChangeRequest = await reqRepo.Count(p => p.ReqtypeId == (int)RequestTypeEnum.ChangeData && p.ServiceId == (int)ServiceEnum.Elaw),

                ChangeOwnerRequest = await reqRepo.Count(c => c.ReqtypeId == (int)RequestTypeEnum.Renouncement && c.ServiceId == (int)ServiceEnum.Elaw),

                EndLicenseRequests = await reqRepo.Count(c => c.ReqtypeId == (int)RequestTypeEnum.EndLicences && c.ServiceId == (int)ServiceEnum.Elaw),

                RenewRequests = await reqRepo.Count(c => c.ReqtypeId == (int)RequestTypeEnum.Renew && c.ServiceId == (int)ServiceEnum.Elaw),

                ChangePartner = await transRepo.Count(c => c.TransTypeId == (int)TransactionTypesEnum.ChangePartnerName && c.ServiceId == ServiceId),

                ChangeAddress = await transRepo.Count(c => c.TransTypeId == (int)TransactionTypesEnum.ChangeAddress && c.ServiceId == (int)ServiceEnum.Elaw),

                ChangeManagerRequests = await transRepo.Count(c => c.TransTypeId == (int)TransactionTypesEnum.ChangeManager && c.ServiceId == (int)ServiceEnum.Elaw),

                ChangeSocialMedia = await transRepo.Count(c => c.TransTypeId == (int)TransactionTypesEnum.ChangeSocialMedia && c.ServiceId == (int)ServiceEnum.Elaw),

                ChangeEmail = await transRepo.Count(c => c.TransTypeId == (int)TransactionTypesEnum.ChangeEmail && c.ServiceId == (int)ServiceEnum.Elaw),

                ChangeLicencesType = await transRepo.Count(c => c.TransTypeId == (int)TransactionTypesEnum.ChangeLicencesType && c.ServiceId == (int)ServiceEnum.Elaw),

                ChangeLicencesName = await transRepo.Count(c => c.TransTypeId == (int)TransactionTypesEnum.ChangeLicencesName && c.ServiceId == (int)ServiceEnum.Elaw),
            };

            return model;
        }
        #endregion


        [Route("GetAllRequest")]
        [HttpGet]
        public async Task<IEnumerable<RequestVM>> GetAllRequest(int ServiceId, string requestTypes)
        {
            var requestTypeIds = requestTypes.Split(',').Select(int.Parse).ToList();
            var requestspec = new RequestWithSpecificService(ServiceId, requestTypeIds);
            var Requests = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().GetTableWithSpecService(requestspec);
            var RequestMapped = _mapper.Map<IEnumerable<MoiEserviceLicensesRequest>, IEnumerable<RequestVM>>(Requests);
            if (requestTypeIds.Contains((int)RequestTypeEnum.ChangeData))
            {
                foreach (var request in RequestMapped)
                {
                    var transactionSpec = new TransactionWithSpec(ServiceId);
                    var transactions = await _unitOfwork.genericRepository<Transaction>().GetTableWithSpec(transactionSpec);
                    var filteredTransactions = transactions
                                     .Where(t => t.RequestId == request.RequestId)
                                     .Select(t => new TransactionVM
                                     {
                                         Id = t.Id,
                                         LicenseId = t.LicenseId,
                                         ServiceId = t.ServiceId ?? 0,
                                         TransTypeId = t.TransTypeId,
                                         MotletterNo = t.MotletterNo,
                                         Changes = t.Changes,
                                         Commited = t.Commited,
                                         Notes = t.Notes,
                                         LastUpdateUser = t.LastUpdateUser,
                                         LastUpdateDate = t.LastUpdateDate,
                                         RequestId = t.RequestId,
                                         MotletterDate = t.MotletterDate,
                                         RequestDate = t.RequestDate,
                                         UsercivilId = t.UsercivilId,
                                         ReqStatusId = t.ReqStatusId,
                                         TransDate = t.TransDate
                                     });

                    // Assign mapped transactions to the request
                    request.Transactions = filteredTransactions.ToList();
                }
            }

            return RequestMapped;
        }
        [Route("GetAllLicences")]
        [HttpGet]
        public async Task<IEnumerable<LicencesVM>> GetAllLicences(int ServiceId)
        {

            var Licencesspec = new LicencesWithSpecificService(ServiceId, false);
            var Licences = await _unitOfwork.genericRepository<Licence>().GetTableWithSpecService(Licencesspec);
            var LicencesMapped = _mapper.Map<IEnumerable<Licence>, IEnumerable<LicencesVM>>(Licences);

            return LicencesMapped;
        }

        [Route("GetRequestById")]
        [HttpGet]
        public async Task<RequestDetailsVM> GetRequestById(int requestId, int licTypeId)
        {
            var requestIdSpec = new RequestWithSpecificService(requestId, (int)ServiceEnum.Elaw, false);
            var request = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().GetByIdWithSpec(requestIdSpec);
            var requestMapped = _mapper.Map<MoiEserviceLicensesRequest, RequestVM>(request);
            var manager = await _dataFetchService.FetchManagerDataAsync(request.ManCivilId, (int)ServiceEnum.Elaw);
            var socialMedia = await _dataFetchService.FetchSocialMediaDataAsync(request.RequestId);

            var partner = await _dataFetchService.FetchPartnerDataAsync(request.LicenseId, (int)ServiceEnum.Elaw);
            var attach = await _dataFetchService.FetchAttachmentsAsync(requestId, (int)ServiceEnum.Elaw);
            var applicantPerson = new PersonVM();
            var applicantcompany = new CompanyVM();
            var employeeLogMapped = await _dataFetchService.FetchEmployeeLogAsync(requestMapped.RequestId);

            if (requestMapped.ReqtypeId == (int)RequestTypeEnum.ChangeData)
            {
                var transactions = await _dataFetchService.FetchTransactionsAsync(requestMapped.RequestId, (int)ServiceEnum.Elaw);
                requestMapped.Transactions = transactions;
            }
            if (licTypeId == (int)LicTypeEnum.Media_Organization_Individuals || licTypeId == (int)LicTypeEnum.Media_Organization_Company)
            {
                applicantPerson = await _dataFetchService.FetchApplicantDataAsync(request.AppCivilId, (int)ServiceEnum.Elaw);
                if (licTypeId == (int)LicTypeEnum.Media_Organization_Company)
                {
                    applicantcompany = await _dataFetchService.FetchCompanyDataAsync(request.CompanyId, (int)ServiceEnum.Elaw);

                }

            }
            else
            {
                applicantcompany = await _dataFetchService.FetchCompanyDataAsync(request.CompanyId, (int)ServiceEnum.Elaw);
            }
            //var licences=await _dataFetchService.FetchLicenceDataAsync(request.)
            return new RequestDetailsVM
            {
                RequestDVM = requestMapped,
                RequestTransactionVM = employeeLogMapped,
                CompanyVM = applicantcompany,
                PersonApplicantVM = applicantPerson,
                PartnerVM = partner,
                socialMediaVMs = socialMedia ?? null,
                attachmentVM = attach ?? null,
            };
        }


        [Route("GetLicencesDetails")]
        [HttpGet]
        public async Task<LicenceDetailsVM> GetLicencesDetails(int licid)
        {
            var LicIdSpec = new LicencesWithSpecificService(licid, (int)ServiceEnum.Elaw);
            var licence = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(LicIdSpec);
            var requestForLicence = new RequestWithSpecificService(licid, true);
            var request = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().GetTableWithSpec(requestForLicence);
            var attachmentVM = new List<AttachVM>();
            foreach (var item in request)
            {
                var attach = await _dataFetchService.FetchAttachmentsAsync(item.RequestId, (int)ServiceEnum.Elaw);
                attachmentVM.AddRange(attach);
            }
            var LicenceMapped = _mapper.Map<Licence, LicencesVM>(licence);
            var Applicant = await _dataFetchService.FetchApplicantDataAsync(licence.ApplicantCivilId, (int)ServiceEnum.Elaw);
            return new LicenceDetailsVM
            {
                LicencesVM = LicenceMapped,
                attachmentVM = attachmentVM,
                PersonApplicantVM = Applicant
            };
        }

        [Route("UpdateRequestStatus")]
        [HttpPost]
        public async Task<IActionResult> UpdateRequestStatus(UpdatedRequestVM updatedRequestVM)
        {

            using var transaction = _unitOfwork.BeginTransaction();
            try
            {
                #region Intialize Variable
                MoiEserviceRequestsAttach RequestAttach = null; // Declare outside the if block
                #endregion
                #region Get Request
                // Get the request using the provided specification
                var request = await _dataFetchService.GetRequest(updatedRequestVM.RequestId);
                DateTime? IssueDate=null;
                DateTime? ExpireDate = null;
                #endregion
                #region Get Employee
                var Employee = await _unitOfwork.genericRepository<MoiEserviceSysUser>().GetByIdObject(s => s.SysUserId == updatedRequestVM.UserId);
                #endregion
                #region GetRequestStatusNew
                var RequestStatusNew = await _unitOfwork.genericRepository<RequestStatusLookup>().GetByIdObject(r => r.Id == updatedRequestVM.StatusId);
                #endregion

                #region Handle change data
                //if (updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.ChangeData)
                //{
                //    foreach (var item in request.Transactions)
                //    {

                //        if (item.TransTypeId == (int)TransactionTypesEnum.ChangeManager)
                //        {
                //            await _updateDataService.HandleManagerChangeAsync(request, Employee, updatedRequestVM);
                //        }
                //        if (item.TransTypeId == (int)TransactionTypesEnum.ChangeAddress)
                //        {
                //            await _updateDataService.HandleAddressChangeAsync(request.RequestId, updatedRequestVM);
                //        }
                //        if (item.TransTypeId == (int)TransactionTypesEnum.ChangeLicencesName)
                //        {
                //            await _updateDataService.HandleLicenseNameChangeAsync(request, updatedRequestVM);
                //        }
                //        if (item.TransTypeId == (int)TransactionTypesEnum.ChangePartnerName)
                //        {
                //            await _updateDataService.HandlePartnerChangeAsync(request, Employee, updatedRequestVM);
                //        }
                //        if (item.TransTypeId == (int)TransactionTypesEnum.ChangeLicencesType)
                //        {
                //            await _updateDataService.HandleLicenseNameChangeAsync(request, updatedRequestVM);
                //        }
                //        if (item.TransTypeId == (int)TransactionTypesEnum.ChangeEmail)
                //        {
                //            await _updateDataService.HandleEmailChangeAsync(request, Employee, updatedRequestVM);
                //        }
                //        if (item.TransTypeId == (int)TransactionTypesEnum.ChangeSocialMedia)
                //        {
                //            await _updateDataService.HandleSocialMediaNameChangeAsync(request, Employee, updatedRequestVM);
                //        }
                //    }
                //}

                if (updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.ChangeData)
                {

                    await HandleChangeData(updatedRequestVM, Employee);


                }

                //if (RequestAttach != null)
                #endregion
                #region AddAction
                var action = "";
                action = updatedRequestVM.Action switch
                {
                    "CorrectData" => "تصحيح بيانات",
                    "RefuseRequestbutton" => "تم رفض المعاملة",
                    "RequestStatusbutton" => $"تم تغيير حالة الطلب إلي {RequestStatusNew.NameAr}",
                    _ => ""
                };
                #endregion
                #region Save Request Transaction(Add New Request Transaction Every Step)

                var requestTransaction = await _updateDataService.SaveRequestTransaction(request, action, RequestStatusNew.NameAr, updatedRequestVM, Employee, (int)ServiceEnum.Elaw);

                #endregion
                #region Calculate Expire Date
                //(DateTime? IssueDate, DateTime? ExpireDate) = await CalculateIssueAndExpireDatesAsync(request.RequestId);
                #endregion
                #region Renew
                if (request.ReqtypeId == (int)RequestTypeEnum.Renew)
                {
                    ( IssueDate, ExpireDate) = await CalculateIssueAndExpireDatesAsync(request.RequestId);
                    await _updateDataService.HandleRenewal(updatedRequestVM, request, ExpireDate, IssueDate, Employee);
                }
                #endregion
                #region Save UserLog(Add New Row Log For User Every Step)

                var userLog = await _updateDataService.SaveUserLog(Employee, action, updatedRequestVM);

                #endregion
                #region Attachment
                await _updateDataService.HandleAttachmentsAsync(updatedRequestVM, request, Employee, (int)ServiceEnum.Elaw);

                #endregion
                #region IsFinal Or have Condition 
                var (isbool, requeststatus, requestStatusFlag) = await _updateDataService.IsFinalCycleStatusAsync(/*request.ActivityTypeId ?? 0,*/ request.ReqtypeId ?? 0, request.ServiceId ?? 0, updatedRequestVM.StatusId);

                #endregion
                #region New Request
                var specRequestNew = new RequestWithSpecificService(updatedRequestVM.RequestId, true);
                var requestNew = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().GetByIdWithSpec(specRequestNew);
                var requesttype = await _unitOfwork.genericRepository<RequestsTypesLookup>().GetFilteredWithProjection(
                                     filter: x => x.Id == updatedRequestVM.ReqTypeId,
                                     selector: x => new
                                     {
                                         NameAr = x.NameAr
                                     }
                                       ).FirstOrDefaultAsync();
                #endregion
                #region Update Licence
                try
                {

                    if (request.ReqtypeId == (int)RequestTypeEnum.Request)
                    {
                        ( IssueDate,  ExpireDate) = await CalculateIssueAndExpireDatesAsync(request.RequestId);
                        var licence = UpdateLicencesAsync(request, updatedRequestVM, IssueDate, ExpireDate, Employee, updatedRequestVM.SequenceNo ?? 0);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                #endregion

                #region Update Request
                if (updatedRequestVM.ReqTypeId != (int)RequestTypeEnum.ChangeData)
                {
                    request.RequestStatusId = updatedRequestVM.StatusId;
                }
                if (request.RequestStatusId == (int)RequestStatusEnum.FinalLicenseIssued)
                {
                    request.Licno = updatedRequestVM.LicNo;
                    request.Licexpiredate = ExpireDate;
                    request.LicIssuedate = IssueDate;
                }
                request.RequestNote = updatedRequestVM.Note;
                
                var requestUpdated = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Update(request);

                #endregion
                #region SaveData
                var result = await _unitOfwork.Complete();
                #endregion
                #region Send Data and Ok to controller 
                if (result > 0)
                {
                    Console.WriteLine($"Status ID: {updatedRequestVM.StatusId}");
                    Console.WriteLine($"License No: {updatedRequestVM.LicNo}");
                    Console.WriteLine($"Attachment Path: {updatedRequestVM.FilePath}");
                    Console.WriteLine($"Attachment Name: {updatedRequestVM.FileName}");
                    Console.WriteLine($"Status Name: {RequestStatusNew.NameAr}");
                    Console.WriteLine($"Username: {updatedRequestVM.NameUser}");
                    //await SendStatusUpdateEmail(request, action, requesttype.NameAr, ExpireDate, IssueDate);
                    transaction.Commit();
                    return Ok(new
                    {
                        statusid = updatedRequestVM.StatusId,
                        licno = updatedRequestVM.LicNo ?? "",
                        attachpath = updatedRequestVM.FilePath ?? string.Empty,
                        attachname = updatedRequestVM.FileName ?? "",
                        statusname = RequestStatusNew.NameAr,
                        transId = updatedRequestVM.TransId,
                        transTypeId = updatedRequestVM.TransTypeId,
                        requestnew = requestNew,
                        expireDate = ExpireDate ?? null,
                        issueDate = IssueDate ?? null,
                        username = updatedRequestVM.NameUser ?? "Anonymous",
                        requestTransaction = requestTransaction,
                        userLog = userLog,
                        isFinalStatus = isbool,
                        flag = updatedRequestVM.Flag,
                        requestStatus = requeststatus,
                        updatedRequestVM = updatedRequestVM

                    });


                }
                else
                {
                    // 🔴 If save failed, rollback
                    transaction.Rollback();
                    return StatusCode(500, "An error occurred while updating the request status");
                }
            }
            catch (Exception ex)
            {
                // 🔴 Rollback if anything goes wrong
                transaction.Rollback();
                Console.WriteLine("Error in UpdateRequestStatus: " + ex.Message);
                return StatusCode(500, "Transaction failed: " + ex.Message);
            }

            #endregion

        }
        private async Task HandleChangeData(UpdatedRequestVM updatedRequestVM, MoiEserviceSysUser Employee)
        {
            var Request = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                        .GetByCondition(r => r.RequestId == updatedRequestVM.RequestId).FirstOrDefaultAsync();
            if (Request != null)
            {
                var alltransInThisRequest = await _unitOfwork.genericRepository<Transaction>()
                    .GetByCondition(t => t.RequestId == updatedRequestVM.RequestId).ToListAsync();
                var LicencesInfoToGetAmount = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                           .GetByCondition(l => l.ReqTypeId == (int)RequestTypeEnum.ChangeData&&l.ServiceId==Request.ServiceId).FirstOrDefaultAsync();
                if (alltransInThisRequest != null)
                {
                    var transactionChangeByAdmin = await _unitOfwork.genericRepository<Transaction>()
                        .GetByCondition(t => t.Id == updatedRequestVM.TransId).FirstOrDefaultAsync();
                    var requeststatus = await _unitOfwork.genericRepository<RequestStatusLookup>()
                        .GetByCondition(r => r.Id == updatedRequestVM.StatusId).FirstOrDefaultAsync();
                    if (transactionChangeByAdmin != null)
                    {
                        transactionChangeByAdmin.ReqStatusId = updatedRequestVM.StatusId;
                        transactionChangeByAdmin.Changes = " تم تحديث حالة الطلب إلي" + requeststatus.NameAr;
                        transactionChangeByAdmin.Notes = updatedRequestVM.Note;

                    }
                    int RejectRequest = alltransInThisRequest.Count(r => r.ReqStatusId == (int)RequestStatusEnum.RequestDeclined);
                    int PaymentRequest = alltransInThisRequest.Count(r => r.ReqStatusId == (int)RequestStatusEnum.WaitingForPayment);
                    int SubmitRequest = alltransInThisRequest.Count(r => r.ReqStatusId == (int)RequestStatusEnum.FinalLicenseIssued);
                    bool allTransactionsWaitingForPayment = alltransInThisRequest.All(t =>
                    t.ReqStatusId == (int)RequestStatusEnum.WaitingForPayment ||
                    t.ReqStatusId == (int)RequestStatusEnum.RequestDeclined
                     );
                    bool allTransactionsReadyForProcessing = alltransInThisRequest.All(t =>
    t.ReqStatusId == (int)RequestStatusEnum.FinalLicenseIssuingProcessing
);
                    bool allTransactionsReject = alltransInThisRequest.All(t =>
                    t.ReqStatusId == (int)RequestStatusEnum.RequestDeclined

                     );
                    bool allTransactionsFinal = alltransInThisRequest.All(t =>
                    t.ReqStatusId == (int)RequestStatusEnum.FinalLicenseIssued

                     );
                    if (allTransactionsWaitingForPayment)
                    {
                        Request.RequestStatusId = (int)RequestStatusEnum.WaitingForPayment;
                        Request.Licamount = (LicencesInfoToGetAmount.FixedFees) * PaymentRequest;
                        Request.RequestNote = $"عدد المعاملات المقبولة: {PaymentRequest} | المرفوضة: {RejectRequest}";

                    }
                    else if (allTransactionsReject)
                    {
                        Request.RequestStatusId = (int)RequestStatusEnum.RequestDeclined;
                        Request.Licamount = 0;
                        Request.RequestNote = "تم رفض جميع التعديلات.";

                    }
                    else if (allTransactionsFinal)
                    {
                        Request.RequestStatusId = (int)RequestStatusEnum.FinalLicenseIssued;
                        Request.RequestNote = "تم إصدار الرخصة بعد إتمام جميع التعديلات.";
                    }
                    else if (allTransactionsReadyForProcessing)
                    {
                        Request.RequestNote = "جاري تنفيذ التعديلات قبل إصدار الرخصة النهائية.";

                        if (updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.ChangeData)
                        {
                            foreach (var item in alltransInThisRequest)
                            {
                                if (item.TransTypeId == (int)TransactionTypesEnum.ChangeManager)
                                {
                                    await _updateDataService.HandleManagerChangeAsync(Request, Employee, updatedRequestVM);
                                }
                                if (item.TransTypeId == (int)TransactionTypesEnum.ChangeAddress)
                                {
                                    await _updateDataService.HandleAddressChangeAsync(Request.RequestId, updatedRequestVM);
                                }
                                if (item.TransTypeId == (int)TransactionTypesEnum.ChangeLicencesName)
                                {
                                    await _updateDataService.HandleLicenseNameChangeAsync(Request, updatedRequestVM);
                                }
                                if (item.TransTypeId == (int)TransactionTypesEnum.ChangePartnerName)
                                {
                                    await _updateDataService.HandlePartnerChangeAsync(Request, Employee, updatedRequestVM);
                                }
                                if (item.TransTypeId == (int)TransactionTypesEnum.ChangeLicencesType)
                                {
                                    await _updateDataService.HandleLicenseNameChangeAsync(Request, updatedRequestVM);
                                }
                                if (item.TransTypeId == (int)TransactionTypesEnum.ChangeEmail)
                                {
                                    await _updateDataService.HandleEmailChangeAsync(Request, Employee, updatedRequestVM);
                                }
                                if (item.TransTypeId == (int)TransactionTypesEnum.ChangeSocialMedia)
                                {
                                    await _updateDataService.HandleSocialMediaNameChangeAsync(Request, Employee, updatedRequestVM);
                                }
                            }
                        }

                        // بعد تنفيذ التعديلات نغير حالة الطلب
                        Request.RequestStatusId = (int)RequestStatusEnum.FinalLicenseIssued;
                        Request.RequestNote += " | تم إصدار الرخصة بعد تنفيذ التعديلات.";
                    }
                    else
                    {
                        Request.RequestNote = "لا تزال بعض التعديلات قيد المراجعة.";
                    }
                    updatedRequestVM.showUploadFinalLicenseButtonForTransaction = true;

                    // await _unitOfwork.Complete();
                    await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Update(Request);
                    await _unitOfwork.genericRepository<Transaction>().Update(transactionChangeByAdmin);

                }
            }
        }

        #region Save Forms
        [Route("SaveForms")]
        [HttpPost]
        public async Task<IActionResult> SaveForms(FormsViewModel model)
        {
            var form = new Form();
            _mapper.Map(model, form);
            _unitOfwork.genericRepository<Form>().Create(form);
            await _unitOfwork.Complete();
            return Ok(form);
        }

        [HttpGet]
        [Route("GetForms")]
        public async Task<IActionResult> GetForms()
        {
            var forms = await _unitOfwork.genericRepository<Form>().GetAll();
            var formMapped = _mapper.Map<IEnumerable<Form>, IEnumerable<FormsViewModel>>(forms).Where(x => x.IsDeleted == false);
            return Ok(formMapped);
        }
        [HttpPost]
        [Route("AddForm")]
        public async Task<IActionResult> AddForm(FormsViewModel model)
        {
            var form = new Form();
            _mapper.Map(model, form);
            var formAdded = _unitOfwork.genericRepository<Form>().Create(form);
            await _unitOfwork.Complete();
            return Ok(model);
        }
        [HttpPost]
        [Route("DeleteForm/{formId}")]
        public async Task<IActionResult> DeleteForm(int formId)
        {
            var formbyId = _unitOfwork.genericRepository<Form>().GetbyId(formId);
            var form = new Form()
            {
                IsDeleted = true
            };


            var formAdded = _unitOfwork.genericRepository<Form>().Update(form);
            return Ok();
        }
        #endregion
        #region AddNews
        [Route("GetNews")]
        [HttpGet]
        public async Task<IActionResult> GetNews()
        {
            var news = await _unitOfwork.genericRepository<ElawMoiWeNews>().GetAll();
            var newsMapper = _mapper.Map<IEnumerable<ElawMoiWeNews>, IEnumerable<NewsItem>>(news);
            return Ok(newsMapper);
        }
        [HttpPost]
        [Route("AddOrEditNews")]
        public async Task<IActionResult> AddOrEditNews(NewsItem model)
        {
            if (model == null)
            {
                return BadRequest("Invalid data.");
            }

            // Check if this is an Add or Edit operation
            if (model.Id == 0)
            {
                // Add operation
                var newNews = new ElawMoiWeNews();
                _mapper.Map(model, newNews); // Map the NewsItem model to your database entity
                await _unitOfwork.genericRepository<ElawMoiWeNews>().Create(newNews);
                await _unitOfwork.Complete();

                return Ok(newNews);
            }
            else
            {
                // Edit operation
                var existingNews = await _unitOfwork.genericRepository<ElawMoiWeNews>().GetbyId(model.Id);

                if (existingNews == null)
                {
                    return NotFound($"News with Id {model.Id} not found.");
                }

                // Update existing news
                _mapper.Map(model, existingNews); // Map the updated properties
                _unitOfwork.genericRepository<ElawMoiWeNews>().Update(existingNews);
                await _unitOfwork.Complete();

                return Ok(existingNews);
            }
        }

        #endregion
        #region AddLinks
        // GET: Links
        [HttpGet]
        [Route("GetAllLinks")]
        public async Task<IActionResult> GetAllLinks()
        {
            var links = await _unitOfwork.genericRepository<LinksLookup>().GetAllAsync();
            return Ok(links);
        }

        // POST: Add or Edit Link
        [HttpPost]
        [Route("AddOrEditLink")]
        public async Task<IActionResult> AddOrEditLink(AddLinksVM model)
        {

            if (model.Id == 0)
            {
                var link = new LinksLookup();
                _mapper.Map(model, link);
                await _unitOfwork.genericRepository<LinksLookup>().Create(link);
                await _unitOfwork.Complete();
            }
            else
            {
                // Edit an existing link
                var existingLink = await _unitOfwork.genericRepository<LinksLookup>().GetbyId(model.Id);
                _mapper.Map(model, existingLink);
                await _unitOfwork.genericRepository<LinksLookup>().Update(existingLink);
                await _unitOfwork.Complete();
            }



            return Ok(model);
        }

        // DELETE: Delete Link
        [HttpPost]
        [Route("DeleteLink")]
        public async Task<IActionResult> DeleteLink(int id)
        {
            var link = await _unitOfwork.genericRepository<LinksLookup>().GetbyId(id);
            if (link != null)
            {
                link.IsDeleted = true;
                await _unitOfwork.genericRepository<LinksLookup>().Update(link);
                await _unitOfwork.Complete();
            }
            return Ok();
        }
        #endregion
        #region Add Condition
        [HttpPost]
        [Route("AddConditions")]
        public async Task<IActionResult> AddConditions(ConditionVM model)
        {
            return Ok();
        }
        #endregion
        private async Task<(DateTime? IssueDate, DateTime? ExpireDate)> CalculateIssueAndExpireDatesAsync(long requestId)
        {
            var request = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                       .GetFilteredWithProjection(
                 filter: x => x.RequestId == requestId,
                 selector: x => new
                 {
                     ActivityTypeId = x.ActivityTypeId
                 }).FirstOrDefaultAsync();
            DateTime? IssueDate = null;
            DateTime? ExpireDate = null;

            var specPayment = new PaymentWithSpecification(requestId);
            var PaymentPerRequest = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>().GetByIdWithSpec(specPayment);

            if (PaymentPerRequest != null && PaymentPerRequest.Payed == 1)
            {
                IssueDate = PaymentPerRequest.PaymentDate;

                if (IssueDate.HasValue)
                {

                    ExpireDate = IssueDate.Value.AddYears(10).AddDays(-1);

                }
            }

            return (IssueDate, ExpireDate);
        }
        private async Task UpdateLicencesAsync(MoiEserviceLicensesRequest request, UpdatedRequestVM updatedRequestVM, DateTime? IssueDate, DateTime? ExpireDate, MoiEserviceSysUser employee, long SequenceNo)
        {
            if (updatedRequestVM.StatusId == (int)RequestStatusEnum.FinalLicenseIssued)
            {
                //Licence licence = null;
                //long updatedSequence = SequenceNo++;
                //var licenceSpec = new LicencesWithSpecificService(request.LicenseId ?? 0, true);
                try
                {
                    var licence = await _unitOfwork.genericRepository<Licence>()
                        .GetByCondition(l => l.LicId == request.LicenseId).FirstOrDefaultAsync();

                    licence.LicNo = updatedRequestVM.LicNo ?? "";
                    licence.IssueDate = IssueDate;
                    licence.SequenceNo = updatedRequestVM.SequenceNo;
                    licence.LicStatusId = (int)licencesStatusEnum.Released;
                    licence.ExpireDate = ExpireDate;

                    await _unitOfwork.genericRepository<Licence>().Update(licence);
                    await _unitOfwork.Complete();
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
    }
}

