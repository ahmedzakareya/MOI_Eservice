using AutoMapper;
using Azure.Core;
using Business.Enums;
using Business.Helpers;
using Business.Interfaces;
using Business.ModelWithSpecification;
using Business.Repository;
using Business.ViewModel;
using Business.ViewModel.ClassificationVM;
using Business.ViewModel.Dynamic;
using Business.ViewModel.Tourism;
using Castle.Components.DictionaryAdapter.Xml;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;
using static System.Collections.Specialized.BitVector32;

namespace MOINFO_API.Controllers
{
    [Route("api/AdminTourism")]
    public class APIAdminTourismController : BaseController
    {
        private readonly IUnitOfwork _unitOfwork;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly GenerateLicNo _generateLicNo;
        private readonly EmailService _emailService;
        private readonly IDataFetchService _dataFetchService;
        private readonly IUpdateDataService _updateDataService;
        private readonly ILogger<APIAdminTourismController> _logger;

        public APIAdminTourismController(IUnitOfwork unitOfwork, IConfiguration configuration
            , IMapper mapper, GenerateLicNo generateLicNo, ILogger<APIAdminTourismController> logger, EmailService emailService, IDataFetchService dataFetchService, IUpdateDataService updateDataService)
        {
            _unitOfwork = unitOfwork;
            _configuration = configuration;
            _mapper = mapper;
            _generateLicNo = generateLicNo;
            _emailService = emailService;
            _dataFetchService = dataFetchService;
            _updateDataService = updateDataService;
            _logger = logger;
        }

        #region Backend
        #region GetStatistics
        //--------------------Get All Statistics----------
        //[HttpGet]
        //[Route("GetAllStatistics")]
        //public async Task<StatisticsViewModel> GetAllStatistics(int ServiceId)
        //{
        //    int[] ReStats = new int[] { 1, 2, 3, 4 };
        //    // var specification = new RequestWithSpecificService(ServiceId);
        //    StatisticsViewModel model = new StatisticsViewModel()
        //    {
        //        PreApprovementConvert = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Count(p => p.ReqtypeId == (int)RequestTypeEnum.PreApprovementConvert),
        //        PreApprovementNew = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Count(p => p.ReqtypeId == (int)RequestTypeEnum.PreApprovementNew),

        //        WhoConc = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Count(p => p.ReqtypeId == (int)RequestTypeEnum.WhoConc),
        //        AllLicences = await _unitOfwork.genericRepository<Licence>().Count(p => p.LicStatusId == (int)licencesStatusEnum.Released && p.ServiceId == (int)ServiceEnum.Tourism),
        //        AllRequests = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Count(r => ReStats.Contains(r.RequestStatusId.Value) && r.ServiceId == ServiceId),
        //        NewRequests = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Count(r => r.RequestStatusId == (int ) RequestStatusEnum.WaitingForReview && r.ServiceId == (int)ServiceEnum.Tourism),
        //        ChangeRequest = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Count(p => p.ReqtypeId == (int)RequestTypeEnum.ChangeData && p.ServiceId == (int)ServiceEnum.Tourism),
        //        ChangeCompanyName = await _unitOfwork.genericRepository<Transaction>().Count(c => c.TransTypeId == (int)TransactionTypesEnum.ChangeCompaneName && c.ServiceId == (int)ServiceEnum.Tourism),
        //        //ChangeCommercialName = await _unitOfwork.genericRepository<Transaction>().Count(c => c.TransTypeId == 2 && c.ServiceId == ServiceId),
        //        //ChangePartner = await _unitOfwork.genericRepository<Transaction>().Count(c => c.TransTypeId == 3 && c.ServiceId == ServiceId),
        //        ChangeAddress = await _unitOfwork.genericRepository<Transaction>().Count(c => c.TransTypeId == (int)TransactionTypesEnum.ChangeAddress && c.ServiceId == (int)ServiceEnum.Tourism),
        //        ChangeOwnerRequest = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Count(c => c.ReqtypeId == (int)RequestTypeEnum.Renouncement && c.ServiceId == (int)ServiceEnum.Tourism),
        //        EndLicenseRequests = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Count(c => c.ReqtypeId == (int)RequestTypeEnum.EndLicences && c.ServiceId == (int)ServiceEnum.Tourism),
        //        RenewRequests = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Count(c => c.ReqtypeId == (int)RequestTypeEnum.Renew && c.ServiceId == (int)ServiceEnum.Tourism),
        //        ChangeManagerRequests = await _unitOfwork.genericRepository<Transaction>().Count(c => c.TransTypeId == (int)TransactionTypesEnum.ChangeManager && c.ServiceId == (int)ServiceEnum.Tourism),
        //        ChangeActivityRequests = await _unitOfwork.genericRepository<Transaction>().Count(c => c.TransTypeId == (int)TransactionTypesEnum.ChangeActivity && c.ServiceId == (int)ServiceEnum.Tourism),
        //        //ReplacementOfLostRequests = await _unitOfwork.genericRepository<Transaction>().Count(c => c.TransTypeId == (int)TransactionTypesEnum.re && c.ServiceId == ServiceId),

        //    };


        //    return model;
        //}
        [HttpGet]
        [Route("GetAllStatistics")]
        public async Task<StatisticsViewModel> GetAllStatistics()
        {
            int serviceId = (int)ServiceEnum.Tourism;


            #region Sum Money Hotel and apartmet and resort
            int?[] activityListRes = new int?[]
{
    (int)ActivityTypeEnum.Hotel,
    (int)ActivityTypeEnum.ApartmentHotel,
    (int)ActivityTypeEnum.Resorts
};
            List<long?> requestIdHoteAppaResors = await _unitOfwork
    .genericRepository<MoiEserviceLicensesRequest>()
    .GetByCondition(r =>
        activityListRes.Contains(r.ActivityTypeId) &&
        r.ReqtypeId == (int)RequestTypeEnum.Request &&
        r.ServiceId == serviceId &&
        r.Licpaystatus == "1")
    .Select(r => (long?)r.RequestId) // ensure nullable
    .ToListAsync();

            var SumLicHoteAppaResor = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>()
                   .GetByCondition(p => requestIdHoteAppaResors.Contains(p.RequestId) &&
        p.Payed == 1 &&
        p.Result == "CAPTURED").Select(p => p.TotalAmount).SumAsync();
            #endregion

            #region Sum Money Park and Sailing
            int?[] activityListActivities = new int?[]
{
    (int)ActivityTypeEnum.Sailing,
    (int)ActivityTypeEnum.Parks,

};
            List<long?> requestIdSailingandParks = await _unitOfwork
    .genericRepository<MoiEserviceLicensesRequest>()
    .GetByCondition(r =>
        activityListActivities.Contains(r.ActivityTypeId) &&
         r.ReqtypeId == (int)RequestTypeEnum.Request &&
        r.ServiceId == serviceId &&
        r.Licpaystatus == "1")
    .Select(r => (long?)r.RequestId) // ensure nullable
    .ToListAsync();

            var SumLicActivities = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>()
                   .GetByCondition(p => requestIdSailingandParks.Contains(p.RequestId) &&
        p.Payed == 1 &&
        p.Result == "CAPTURED").Select(p => p.TotalAmount).SumAsync();
            #endregion
            #region Sum Money Classification

            List<long?> requestIdClassification = await _unitOfwork
    .genericRepository<MoiEserviceLicensesRequest>()
    .GetByCondition(r =>
        (r.ReqtypeId == (int)RequestTypeEnum.Classification || r.ReqtypeId == (int)RequestTypeEnum.ReClassification) &&
        r.ServiceId == serviceId &&
        r.Licpaystatus == "1")
    .Select(r => (long?)r.RequestId) // ensure nullable
    .ToListAsync();

            var SumClassification = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>()
                   .GetByCondition(p => requestIdClassification.Contains(p.RequestId) &&
        p.Payed == 1 &&
        p.Result == "CAPTURED").Select(p => p.TotalAmount).SumAsync();
            #endregion
            #region Sum Money Replacement

            List<long?> requestIdReplacement = await _unitOfwork
    .genericRepository<MoiEserviceLicensesRequest>()
    .GetByCondition(r =>
        (r.ReqtypeId == (int)RequestTypeEnum.ReplacementOfLost) &&
        r.ServiceId == serviceId &&
        r.Licpaystatus == "1")
    .Select(r => (long?)r.RequestId) // ensure nullable
    .ToListAsync();

            var SumReplacement = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>()
                   .GetByCondition(p => requestIdReplacement.Contains(p.RequestId) &&
        p.Payed == 1 &&
        p.Result == "CAPTURED").Select(p => p.TotalAmount).SumAsync();
            #endregion
            #region Sum Money Renew

            List<long?> requestIdRenew = await _unitOfwork
    .genericRepository<MoiEserviceLicensesRequest>()
    .GetByCondition(r =>
        (r.ReqtypeId == (int)RequestTypeEnum.Renew) &&
        r.ServiceId == serviceId &&
        r.Licpaystatus == "1")
    .Select(r => (long?)r.RequestId) // ensure nullable
    .ToListAsync();

            var SumRenew = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>()
                   .GetByCondition(p => requestIdRenew.Contains(p.RequestId) &&
        p.Payed == 1 &&
        p.Result == "CAPTURED").Select(p => p.TotalAmount).SumAsync();
            #endregion
            #region Sum Money Edit

            List<long?> requestIdEdit = await _unitOfwork
    .genericRepository<MoiEserviceLicensesRequest>()
    .GetByCondition(r =>
        (r.ReqtypeId == (int)RequestTypeEnum.ChangeData) &&
        r.ServiceId == serviceId &&
        r.Licpaystatus == "1")
    .Select(r => (long?)r.RequestId) // ensure nullable
    .ToListAsync();

            var SumEdit = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>()
                   .GetByCondition(p => requestIdEdit.Contains(p.RequestId) &&
        p.Payed == 1 &&
        p.Result == "CAPTURED").Select(p => p.TotalAmount).SumAsync();
            #endregion
            #region Sum Money WhoConc

            List<long?> requestIdWhoConc = await _unitOfwork
    .genericRepository<MoiEserviceLicensesRequest>()
    .GetByCondition(r =>
        (r.ReqtypeId == (int)RequestTypeEnum.WhoConc) &&
        r.ServiceId == serviceId &&
        r.Licpaystatus == "1")
    .Select(r => (long?)r.RequestId) // ensure nullable
    .ToListAsync();

            var SumWhoConc = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>()
                   .GetByCondition(p => requestIdWhoConc.Contains(p.RequestId) &&
        p.Payed == 1 &&
        p.Result == "CAPTURED").Select(p => p.TotalAmount).SumAsync();
            #endregion

            #region classification
            var classificationList = await _unitOfwork.genericRepository<MoiClassification>()
    .GetAllAsync();

            var licenses = await _unitOfwork.genericRepository<Licence>()
                .GetByCondition(l => l.LicStatusId == (int)licencesStatusEnum.Released).ToListAsync();

            var classificationStats = Enumerable.GroupJoin<
    MoiClassification, Licence, int?, ClassificationCountVM>(
    classificationList,                          // outer
    licenses,                                    // inner
    c => c.ClassifyId,                           // outer key selector
    l => l.ClassificationId,                       // inner key selector
    (c, licGroup) => new ClassificationCountVM   // result selector
    {
        Id = c.ClassifyId,
        Name = c.ClassifiyName,
        Count = licGroup.Count()
    }).ToList();

            #endregion
            var reqRepo = _unitOfwork.genericRepository<MoiEserviceLicensesRequest>();
            var transRepo = _unitOfwork.genericRepository<Transaction>();
            var licRepo = _unitOfwork.genericRepository<Licence>();

            var model = new StatisticsViewModel
            {
                AllLicences = await licRepo.Count(p => p.LicStatusId == (int)licencesStatusEnum.Released && p.ServiceId == serviceId),
                LicencesWillEnd = await licRepo.Count(l => l.ExpireDate < DateTime.Now.AddMonths(-1) && l.ServiceId == serviceId),
                LicencesEnded = await licRepo.Count(l => l.ExpireDate < DateTime.Now && l.ServiceId == serviceId),
                LicencesActive = await licRepo.Count(l => l.ExpireDate < DateTime.Now && l.ServiceId == serviceId),
                SumLicHoteAppaResor = SumLicHoteAppaResor,
                SumLicActivities = SumLicActivities,
                SumLicClassify = SumClassification,
                SumLicEdit = SumEdit,
                SumLicRenew = SumRenew,
                SumLicReplacement = SumReplacement,
                SumLicWhoConc = SumWhoConc,
                LicencesHotel = await licRepo.Count(h => h.ActiivityTypeId == (int)ActivityTypeEnum.Hotel),
                LicencesApartmentHotel = await licRepo.Count(h => h.ActiivityTypeId == (int)ActivityTypeEnum.ApartmentHotel),
                LicencesResort = await licRepo.Count(h => h.ActiivityTypeId == (int)ActivityTypeEnum.Resorts),
                LicencesParks = await licRepo.Count(h => h.ActiivityTypeId == (int)ActivityTypeEnum.Parks),
                LicencesSailing = await licRepo.Count(h => h.ActiivityTypeId == (int)ActivityTypeEnum.Sailing),

                ClassificationStats = classificationStats,
                //AllRequests = await reqRepo.Count(r => activeStatuses.Contains(r.RequestStatusId.Value) && r.ServiceId == serviceId),
                NewRequests = await reqRepo.Count(r => r.RequestStatusId == (int)RequestStatusEnum.WaitingForReview && r.ServiceId == serviceId),
                AllRequestActive = await reqRepo.Count(r => r.RequestStatusId != (int)RequestStatusEnum.FinalLicenseIssued && r.ServiceId == serviceId),
                RefusedRequest = await reqRepo.Count(r => r.RequestStatusId == (int)RequestStatusEnum.RequestDeclined && r.ServiceId == serviceId),
                // Pull values from the lookup table dynamically (optional: cache for performance)
                ChangeRequest = await reqRepo.Count(p => p.ReqtypeId == (int)RequestTypeEnum.ChangeData && p.ServiceId == serviceId), // طلب تعديل البيانات
                RenewRequests = await reqRepo.Count(p => p.ReqtypeId == (int)RequestTypeEnum.Renew && p.ServiceId == serviceId),   // تجديد
                EndLicenseRequests = await reqRepo.Count(p => p.ReqtypeId == (int)RequestTypeEnum.EndLicences && p.ServiceId == serviceId), // إنهاء
                ChangeOwnerRequest = await reqRepo.Count(p => p.ReqtypeId == (int)RequestTypeEnum.Renouncement && p.ServiceId == serviceId), // تنازل
                PreApprovementNew = await reqRepo.Count(p => p.ReqtypeId == (int)RequestTypeEnum.PreApprovementNew && p.ServiceId == serviceId), // طلب موافقة على إنشاء عقار
                PreApprovementConvert = await reqRepo.Count(p => p.ReqtypeId == (int)RequestTypeEnum.PreApprovementConvert && p.ServiceId == serviceId), // طلب موافقة على تحويل عقار
                ReplacementOfLostRequests = await reqRepo.Count(r => r.ReqtypeId == (int)RequestTypeEnum.ReplacementOfLost && r.ServiceId == serviceId),
                ChangeManagerRequests = await transRepo.Count(p => p.TransTypeId == (int)TransactionTypesEnum.ChangeManager && p.ServiceId == serviceId),
                ChangeAddress = await transRepo.Count(p => p.TransTypeId == (int)TransactionTypesEnum.ChangeAddress && p.ServiceId == serviceId),
                WithoutClassification = await reqRepo.Count(c => c.ReqtypeId == (int)RequestTypeEnum.ReClassification || c.ReqtypeId == (int)RequestTypeEnum.Classification),
                WhoConc = await reqRepo.Count(w => w.ReqtypeId == (int)RequestTypeEnum.WhoConc && w.ServiceId == serviceId),
                ChangeActivityRequests = await transRepo.Count(p => p.TransTypeId == (int)TransactionTypesEnum.ChangeActivity && p.ServiceId == serviceId),
                ChangeCompanyName = await transRepo.Count(p => p.TransTypeId == (int)TransactionTypesEnum.ChangeCompaneName && p.ServiceId == serviceId),

            };

            return model;
        }

        [HttpGet]
        [Route("GetLicensesByClassification")]
        public async Task<IActionResult> GetLicensesByClassification(int id)
        {
            var result = await _unitOfwork.genericRepository<Licence>()
                .GetFilteredWithProjection(
                    l => l.Classification.ClassifyId == id,
                    l => new
                    {
                        licNo = l.LicNo,
                        licName = l.LicName,
                        expireDate = l.ExpireDate.Value.ToString("yyyy-MM-dd"),
                        classification = l.Classification.ClassifiyName
                    }
                ).ToListAsync();

            return Ok(result);
        }

        [HttpGet("GetTotalCollectedAmountByPeriod")]
        public async Task<IActionResult> GetTotalCollectedAmountByPeriod(DateTime fromDate, DateTime toDate)
        {
            // Example: Replace with your actual service/repository logic
            var total = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>()
                .GetByCondition(
                    x => x.PaymentDate >= fromDate && x.PaymentDate <= toDate
                ).ToListAsync();

            decimal? totalAmount = total.Sum(x => x.TotalAmount); // Replace 'AmountCollected' with your field

            return Ok(totalAmount);
        }
        #endregion
        #region PreAprovmentRequest
        [HttpGet]
        [Route("GetRequests")]
        public async Task<IEnumerable<RequestVM>> GetRequests(int serviceId, string requestTypes, string activityTypeNames = null)
        {
            try
            {
                // Parse request type IDs
                var requestTypeIds = requestTypes.Split(',').Select(int.Parse).ToList();

                // Parse activity type names to IDs (if provided)
                var activityTypeIds = string.IsNullOrEmpty(activityTypeNames)
                    ? null
                    : activityTypeNames.Split(',').Select(int.Parse).ToList();

                IEnumerable<MoiEserviceLicensesRequest> requests;

                try
                {
                    // Instantiate the specification
                    var requestSpec = new RequestWithSpecificService(serviceId, requestTypeIds, activityTypeIds);

                    // Fetch data using the specification
                    requests = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                        .GetTableWithSpec(requestSpec);

                    _logger.LogInformation($"Fetched {requests.Count()} requests successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error fetching requests");
                    return Enumerable.Empty<RequestVM>();
                }

                _logger.LogInformation("Mapping requests to ViewModel...");
                //var mappedRequests = _mapper.Map<List<RequestVM>>(requests);
                List<RequestVM> mappedRequests;

                try
                {
                    _logger.LogInformation("Starting AutoMapper mapping of requests...");
                    mappedRequests = _mapper.Map<List<RequestVM>>(requests);
                    _logger.LogInformation("Successfully mapped requests to RequestVM.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Error occurred while mapping requests to RequestVM.");
                    throw; // Re-throw to bubble up or return a failed result as needed
                }
                // Attach transactions if ChangeData is included
                if (requestTypeIds.Contains((int)RequestTypeEnum.ChangeData))
                {
                    foreach (var request in mappedRequests)
                    {
                        try
                        {
                            var transactionSpec = new TransactionWithSpec(serviceId);
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

                            _logger.LogInformation($"Fetched {filteredTransactions.Count()} transactions for requestId: {request.RequestId}.");

                            request.Transactions = filteredTransactions.ToList();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error fetching transactions for requestId: {request.RequestId}");
                            request.Transactions = new List<TransactionVM>();
                        }
                    }
                }

                return mappedRequests;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRequests");
                return Enumerable.Empty<RequestVM>();
            }
        }


        [HttpGet]
        [Route("GetAllLicences")]
        public async Task<IEnumerable<LicencesVM>> GetAllLicences(int ServiceId)
        {
            var specification = new LicencesWithSpecificService(ServiceId, false);
            var licence = await _unitOfwork.genericRepository<Licence>().GetTableWithSpecService(specification);

            return _mapper.Map<IEnumerable<Licence>, IEnumerable<LicencesVM>>(licence.AsEnumerable());
        }

        [HttpGet]
        [Route("GetLicenceById")]
        public async Task<LicenceDetailsVM> GetLicenceById(int licId)
        {


            var requestForLicence = new RequestWithSpecificService(licId, true);
            var request = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().GetTableWithSpec(requestForLicence);
            var attachmentVM = new List<AttachVM>();
            foreach (var item in request)
            {
                var attach = await FetchAttachmentsAsync(item.RequestId, (int)ServiceEnum.Tourism);
                attachmentVM.AddRange(attach);
            }
            var licencesWithSpec = new LicencesWithSpecificService(licId, (int)ServiceEnum.Tourism);
            var licence = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesWithSpec);
            var applicant = await _dataFetchService.FetchApplicantDataAsync(licence.ApplicantCivilId, (int)ServiceEnum.Tourism);
            var licenceMapped = _mapper.Map<Licence, LicencesVM>(licence);
            var mandoob = await FetchMandoobDataAsync(licId);
            return new LicenceDetailsVM
            {
                LicencesVM = licenceMapped,
                attachmentVM = attachmentVM,
                PersonApplicantVM = applicant,
                Mandoob = mandoob,
            };
        }


        [HttpPost]
        [Route("UpdateLicenceData")]
        public async Task<dynamic> UpdateLicenceData(PreApprovalRequestApiModel model)
        {


            var licence = await _unitOfwork.genericRepository<Licence>()
                .GetByCondition(l => l.LicId == model.LicId).FirstOrDefaultAsync();


            if (licence == null) return NotFound();



            // Company
            var Company = await _unitOfwork.genericRepository<Company>()
                .GetByCondition(c => c.Id == licence.CompanyId).FirstOrDefaultAsync();


            Company.DirCompanyAr = model.DirCompanyAr;
            Company.OwnerCompanyAr = model.OwnerCompanyAr;
            Company.RecordNo = model.RecordNo;
            Company.CompanyCivilId = model.CompanyCivilId;
            Company.CommercialLicNo = model.CommercialLicNo;

            // Company Address (Parks or Sailing)
            if (Company.AddressId != null)
            {
                var addr = await _unitOfwork.genericRepository<Address>()
                    .GetByCondition(a => a.Id == Company.AddressId).FirstOrDefaultAsync();

                addr.AalliNo = model.AaliNumber;
                addr.Area = model.Area;
                addr.GovernorateArabic = model.Governrate;
                addr.BlockArabic = model.BlockNo;
                addr.StreetArabic = model.Street;
                addr.BuildingName = model.BuildingName;
                addr.BuildingNo = model.BuildingNo;
                addr.FloorNo = model.FloorNo;
                addr.UnitNo = model.UnitNo;
                addr.AreaChartNo = model.AreaChartNo;
                addr.AreaSize = model.AreaSize;
                await _unitOfwork.genericRepository<Address>().Update(addr);
            }
            else
            {
                var NewAddress = new Address
                {
                    AalliNo = model.AaliNumber,
                    Area = model.Area,
                    GovernorateArabic = model.Governrate,
                    BlockArabic = model.BlockNo,
                    StreetArabic = model.Street,
                    BuildingName = model.BuildingName,
                    BuildingNo = model.BuildingNo,
                    FloorNo = model.FloorNo,
                    UnitNo = model.UnitNo,
                    AreaChartNo = model.AreaChartNo,
                    AreaSize = model.AreaSize,
                };
                await _unitOfwork.genericRepository<Address>().Create(NewAddress);
                await _unitOfwork.Complete();
                Company.AddressId = NewAddress.Id;
                await _unitOfwork.genericRepository<Company>().Update(Company);


            }

            await _unitOfwork.genericRepository<Company>().Update(Company);


            var applicant = await _unitOfwork.genericRepository<Person>()
     .GetByCondition(b => b.CivilId == licence.ApplicantCivilId)
     .FirstOrDefaultAsync();
            int AppId = 0;
            if (applicant == null)
            {
                applicant = new Person
                {
                    CivilId = model.AppCivilId,
                    Name1 = model.AppName,
                    Phone = model.AppPhone,
                    Email = model.AppEmail
                };
                await _unitOfwork.genericRepository<Person>().Create(applicant);
                await _unitOfwork.Complete();
                AppId = applicant.Id;
            }
            else
            {
                applicant.Name1 = model.AppName;
                applicant.Phone = model.AppPhone;
                applicant.Email = model.AppEmail;
                applicant.CivilId = model.AppCivilId;
                await _unitOfwork.genericRepository<Person>().Update(applicant);
                AppId = applicant.Id;

            }

            // Manager
            var manager = await _unitOfwork.genericRepository<Person>()
                .GetByCondition(b => b.CivilId == licence.ManagerCivilId)
                .FirstOrDefaultAsync();
            int ManagerId = 0;
            if (manager == null)
            {
                manager = new Person
                {
                    CivilId = model.ManCivilId,
                    Email = model.ManagerEmail,
                    Phone = model.ManagerMobile,
                    Name1 = model.ManagerName
                };
                await _unitOfwork.genericRepository<Person>().Create(manager);
                await _unitOfwork.Complete();
                ManagerId = manager.Id;
            }
            else
            {
                manager.CivilId = model.ManCivilId;
                manager.Email = model.ManagerEmail;
                manager.Phone = model.ManagerMobile;
                manager.Name1 = model.ManagerName;
                await _unitOfwork.genericRepository<Person>().Update(manager);
                ManagerId = manager.Id;

            }

            // Sales Manager
            var salesManager = await _unitOfwork.genericRepository<Person>()
                .GetByCondition(b => b.CivilId == licence.SalesManagerCivilId)
                .FirstOrDefaultAsync();
            int SalesManagerId = 0;
            if (salesManager == null)
            {
                salesManager = new Person
                {
                    Name1 = model.SalesManagerName,
                    CivilId = model.SalesManagerCivilId,
                    Email = model.SalesManagerEmail,
                    Phone = model.SalesManagerPhone
                };
                await _unitOfwork.genericRepository<Person>().Create(salesManager);
                await _unitOfwork.Complete();
                SalesManagerId = salesManager.Id;
            }
            else
            {
                salesManager.Name1 = model.SalesManagerName;
                salesManager.CivilId = model.SalesManagerCivilId;
                salesManager.Email = model.SalesManagerEmail;
                salesManager.Phone = model.SalesManagerPhone;
                await _unitOfwork.genericRepository<Person>().Update(salesManager);
                SalesManagerId = salesManager.Id;

            }

            // Marketing Manager
            var marketingManager = await _unitOfwork.genericRepository<Person>()
                .GetByCondition(b => b.CivilId == licence.MarketingManagerCivilId)
                .FirstOrDefaultAsync();
            int MarketingManagerId = 0;
            if (marketingManager == null)
            {
                marketingManager = new Person
                {
                    Name1 = model.MarketingManagerName,
                    CivilId = model.MarketingManagerCivilId,
                    Email = model.MarketingManagerEmail,
                    Phone = model.MarketingManagerPhone
                };
                await _unitOfwork.genericRepository<Person>().Create(marketingManager);
                await _unitOfwork.Complete();
                MarketingManagerId = marketingManager.Id;

            }
            else
            {
                marketingManager.Name1 = model.MarketingManagerName;
                marketingManager.CivilId = model.MarketingManagerCivilId;
                marketingManager.Email = model.MarketingManagerEmail;
                marketingManager.Phone = model.MarketingManagerPhone;
                await _unitOfwork.genericRepository<Person>().Update(marketingManager);
                MarketingManagerId = marketingManager.Id;
            }

            // Operations Manager
            var operationsManager = await _unitOfwork.genericRepository<Person>()
                .GetByCondition(b => b.CivilId == licence.OperationsManagerCivilId)
                .FirstOrDefaultAsync();
            int OperationManagerId = 0;
            if (operationsManager == null)
            {
                operationsManager = new Person
                {
                    Name1 = model.OperationManagerName,
                    CivilId = model.OperationManagerCivilId,
                    Email = model.OperationManagerEmail,
                    Phone = model.OperationManagerPhone
                };
                await _unitOfwork.genericRepository<Person>().Create(operationsManager);
                await _unitOfwork.Complete();
                OperationManagerId = operationsManager.Id;
            }
            else
            {
                operationsManager.Name1 = model.OperationManagerName;
                operationsManager.CivilId = model.OperationManagerCivilId;
                operationsManager.Email = model.OperationManagerEmail;
                operationsManager.Phone = model.OperationManagerPhone;
                await _unitOfwork.genericRepository<Person>().Update(operationsManager);
                OperationManagerId = operationsManager.Id;


            }

            // Mandoob (AspNetUser)
            var mandoob = await _unitOfwork.genericRepository<AspNetUser>()
                .GetByCondition(b => b.CivilId == model.UserCivilID)
                .FirstOrDefaultAsync();
            string MandoobId = "";
            if (mandoob == null)
            {
                mandoob = new AspNetUser
                {
                    UserName = model.UserName,
                    CivilId = model.UserCivilID,
                    Email = model.MandoobEmail,
                    Mobile = model.MandoobPhone
                };
                await _unitOfwork.genericRepository<AspNetUser>().Create(mandoob);
                try
                {
                    await _unitOfwork.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during saving changes: " + ex.Message);
                    _logger.LogError(ex, "An error occurred while completing the unit of work.");

                    // Optional: log stack trace
                    Console.WriteLine("Stack Trace: " + ex.StackTrace);
                }
                MandoobId = mandoob.Id;

            }
            else
            {
                mandoob.UserName = model.UserName;
                mandoob.CivilId = model.UserCivilID;
                mandoob.Email = model.MandoobEmail;
                mandoob.Mobile = model.MandoobPhone;
                await _unitOfwork.genericRepository<AspNetUser>().Update(mandoob);
                MandoobId = mandoob.Id;

            }
            //Insert licences to table LicencesMandoob 
            var LicencesMandoob = await _unitOfwork.genericRepository<AspNetMultipleLicenseUser>()
                              .GetByCondition(a => a.LicenseId == model.LicId).FirstOrDefaultAsync();
            var applicantUserInAsp = await _unitOfwork.genericRepository<AspNetUser>()
                  .GetByCondition(a => a.CivilId == model.AppCivilId).FirstOrDefaultAsync();
            string ApplicantIdInAspNetUser = "";
            if (applicantUserInAsp == null)
            {
                applicantUserInAsp = new AspNetUser
                {
                    UserName = model.AppName,
                    CivilId = model.AppCivilId,
                    Email = model.AppEmail,
                    Mobile = model.AppEmail
                };
                await _unitOfwork.genericRepository<AspNetUser>().Create(applicantUserInAsp);
                try
                {
                    await _unitOfwork.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during saving changes: " + ex.Message);
                    _logger.LogError(ex, "An error occurred while completing the unit of work.");

                    // Optional: log stack trace
                    Console.WriteLine("Stack Trace: " + ex.StackTrace);
                }
                ApplicantIdInAspNetUser = applicantUserInAsp.Id;

            }
            else
            {
                ApplicantIdInAspNetUser = applicantUserInAsp.Id;
            }
            if (LicencesMandoob == null)
            {
                //var MandoobExistInAnotherLice = await _unitOfwork.genericRepository<AspNetMultipleUser>()
                //             .GetByCondition(a =>  a.MandoobId == MandoobId).FirstOrDefaultAsync();
                //if(MandoobExistInAnotherLice==null)
                //{
                var newMultipleUser = new AspNetMultipleUser
                {
                    MandoobId = MandoobId,
                    IsActive = true,
                    MainUserId = ApplicantIdInAspNetUser,

                };

                await _unitOfwork.genericRepository<AspNetMultipleUser>().Create(newMultipleUser);
                await _unitOfwork.Complete();

                // Step 2: Create new AspNetMultipleLicenseUser assignment
                var newAssignment = new AspNetMultipleLicenseUser
                {
                    LicenseId = model.LicId,
                    MultipleUserId = newMultipleUser.Id,
                    IsApproved = true,
                    IsConfirmed = true,
                    Note = "إضافة بواسطة الأدمن",
                    ServiceId = ServiceEnum.Tourism.ToString(),


                };

                await _unitOfwork.genericRepository<AspNetMultipleLicenseUser>().Create(newAssignment);
                await _unitOfwork.Complete();
                //}
            }
            else
            {
                var MandoobExist = await _unitOfwork.genericRepository<AspNetMultipleUser>()
                                 .GetByCondition(a => a.Id == LicencesMandoob.MultipleUserId && a.MandoobId == MandoobId).FirstOrDefaultAsync();
                if (MandoobExist == null)
                {
                    var newMultipleUser = new AspNetMultipleUser
                    {
                        MandoobId = MandoobId,
                        IsActive = true,
                        MainUserId = ApplicantIdInAspNetUser,

                    };

                    await _unitOfwork.genericRepository<AspNetMultipleUser>().Create(newMultipleUser);
                    await _unitOfwork.Complete();

                    // Step 2: Create new AspNetMultipleLicenseUser assignment
                    var newAssignment = new AspNetMultipleLicenseUser
                    {
                        LicenseId = model.LicId,
                        MultipleUserId = newMultipleUser.Id,
                        IsApproved = true,
                        IsConfirmed = true,
                        Note = "إضافة بواسطة الأدمن",
                        ServiceId = ServiceEnum.Tourism.ToString(),


                    };

                    await _unitOfwork.genericRepository<AspNetMultipleLicenseUser>().Create(newAssignment);
                    await _unitOfwork.Complete();
                }
            }



            // Basic Licence 
            licence.LicNo = model.LicNo;
            licence.LicName = model.LicencesName;
            licence.CommercialLicNo = model.CommercialLicNo;
            licence.RecordNo = model.RecordNo;
            licence.IssueDate = model.IssueDate;
            licence.ExpireDate = model.ExpireDate;
            licence.ApplicantCivilId = model.AppCivilId;
            licence.ManagerCivilId = model.ManCivilId;
            licence.SalesManagerCivilId = model.SalesManagerCivilId;
            licence.MarketingManagerCivilId = model.MarketingManagerCivilId;
            licence.OperationsManagerCivilId = model.OperationManagerCivilId;
            licence.MandoobCivilId = model.UserCivilID;
            licence.ApplicantId = AppId;
            licence.SalesManagerId = SalesManagerId;
            licence.ManagerId = ManagerId;
            licence.OperationsManagerId = OperationManagerId;
            licence.MarketingManagerId = MarketingManagerId;
            licence.MandoobId = MandoobId;

            await _unitOfwork.genericRepository<Licence>().Update(licence);

            await _unitOfwork.Complete();


            return Ok(new ErrorMessage()
            {
                Error = false,
                Message = "Updated Successfully",
                Status = "Complete"
            });
        }

        [HttpGet]
        [Route("GetAllLicencesPreApprove")]
        public async Task<IEnumerable<PreApprovementVM>> GetAllLicencesPreApprove()
        {
            var specification = new PreApprovementWithSpec();
            var licence = await _unitOfwork.genericRepository<MoiPreApprovement>().GetTableWithSpecService(specification);

            return _mapper.Map<IEnumerable<MoiPreApprovement>, IEnumerable<PreApprovementVM>>(licence.AsEnumerable());
        }

        [HttpGet]
        [Route("GetLicencePreApproveById")]
        public async Task<PreApproveDetails> GetLicencePreApproveById(int licId)
        {


            var requestForLicence = new RequestWithSpecificService(licId, true);
            var request = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().GetTableWithSpec(requestForLicence);
            var attachmentVM = new List<AttachVM>();
            foreach (var item in request)
            {
                var attach = await FetchAttachmentsAsync(item.RequestId, (int)ServiceEnum.Tourism);
                attachmentVM.AddRange(attach);
            }
            var licencesWithSpec = new PreApprovementWithSpec(licId, false);
            var licence = await _unitOfwork.genericRepository<MoiPreApprovement>().GetByIdWithSpec(licencesWithSpec);

            var licenceMapped = _mapper.Map<MoiPreApprovement, PreApprovementVM>(licence);
            // var LicencesSpec= new mul
            var Mandoob = await FetchMandoobDataAsync(licId);
            var applicant = await FetchApplicantDataAsync(licence.ApplicantCivilId, null);
            return new PreApproveDetails
            {
                PreApprovementVM = licenceMapped,
                attachVMs = attachmentVM,
                Applicant = applicant,
                Mandoob = Mandoob

            };
        }


        [HttpGet]
        [Route("GetRequestById")]
        public async Task<IActionResult> GetRequestById(int id, int serviceId,int userId)
        {
            try
            {
                // Fetch the main request
                var spec = new RequestWithSpecificService(id, serviceId, false);
                var request = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().GetByIdWithSpec(spec);

                if (request == null)
                {
                    return NotFound(new ErrorMessage
                    {
                        Error = true,
                        Status = "Failure",
                        Message = "No data found"
                    });
                }

                var requestMapped = _mapper.Map<MoiEserviceLicensesRequest, RequestVM>(request);
                var licencepreApprovementMapped = new PreApprovementVM();
                var licencesMapped = new LicencesVM();
                var buildingMapped = new BuildingVM();
                var AddressBuilding = new AddressVM();
                var ChangeOwner = new ChangeOwnerTransVM();
                var EndLicences = new EndLicencesTransVM();
                var ReplacementTrans = new ReplacementOfLostTransVM();
                // Fetch related data
                if (requestMapped.ReqtypeId == (int)RequestTypeEnum.PreApprovementConvert || requestMapped.ReqtypeId == (int)RequestTypeEnum.PreApprovementNew)
                {
                    licencepreApprovementMapped = await FetchLicencePreApproveDataAsync(requestMapped.LicenseId, serviceId);
                }
                if ((requestMapped.ActivityTypeId == (int)ActivityTypeEnum.Hotel || requestMapped.ActivityTypeId == (int)ActivityTypeEnum.ApartmentHotel))
                {

                    buildingMapped = await FetchBuildingDataAsync(requestMapped.BuildingId, serviceId);
                    if (buildingMapped != null)
                    {
                        AddressBuilding = await FetchAddressForBuldingDataAsync(buildingMapped.AddressId, serviceId);
                    }

                }
                var AllowedButton = new List<AllowedButtonVM>();
                var excludedTypes = new[]
{
    (int)RequestTypeEnum.PreApprovementNew,
    (int)RequestTypeEnum.PreApprovementConvert
};

                if (!excludedTypes.Contains((int)requestMapped.ReqtypeId))
                {
                    licencesMapped = await FetchLicenceDataAsync(requestMapped.LicenseId ?? 0, serviceId);
                    AllowedButton = await GetAllowedButtonsPerRequest(request.RequestId, userId);

                }
                //if (
                //    requestMapped.ReqtypeId != (int)RequestTypeEnum.PreApprovementConvert)
                //{
                //    licencesMapped = await FetchLicenceDataAsync(requestMapped.LicenseId ?? 0, serviceId);
                //     AllowedButton = await GetAllowedButtons(request.RequestId, request.RequestStatusId ?? 0, userId);

                //}
                //if (requestMapped.ReqtypeId != (int)RequestTypeEnum.PreApprovementNew )
                //{
                //    licencesMapped = await FetchLicenceDataAsync(requestMapped.LicenseId ?? 0, serviceId);
                //    AllowedButton = await GetAllowedButtons(request.RequestId, request.RequestStatusId ?? 0, userId);

                //}
                var licencesRenew = await FetchLicenceRenewDataAsync(requestMapped.LicenseId, serviceId);
                var applicantMapped = await FetchApplicantDataAsync(requestMapped.AppCivilId, serviceId);
                var managerMapped = await FetchManagerDataAsync(requestMapped.ManCivilId, serviceId);

                //var partnerMapped = await FetchPartnerDataAsync(requestMapped.LicenseId, serviceId);

                var attachMapped = await FetchAttachmentsAsync(requestMapped.RequestId, serviceId);
                var requestStatusMapped = await FetchRequestStatusAsync();
                var companyMapped = await FetchCompanyDataAsync(requestMapped.CompanyId, serviceId);
                var employeeLogMapped = await FetchEmployeeLogAsync(requestMapped.RequestId);
                var PaymentMapped = await FetchPaymentsAsync(requestMapped.RequestId, serviceId);
                var (isFinalStatus, requestStatus, requestStatusFlag) = await IsFinalCycleStatusAsync(requestMapped.ActivityTypeId ?? 0, requestMapped.ReqtypeId ?? 0, serviceId, requestMapped.RequestStatusId ?? 0);
                ClassificationResponse classification = null;
                if (requestMapped.ReqtypeId == (int)RequestTypeEnum.Classification || requestMapped.ReqtypeId == (int)RequestTypeEnum.ReClassification)
                {
                    // Await the asynchronous method to fetch classification data
                    classification = await FetchAllClassificationByRequestId(requestMapped.RequestId, requestMapped.ActivityTypeId ?? 0);
                }
                if (requestMapped.ReqtypeId == (int)RequestTypeEnum.Renouncement)
                {
                    ChangeOwner = await FetchOwnerTransRequestId(requestMapped.RequestId);
                }
                if (requestMapped.ReqtypeId == (int)RequestTypeEnum.ReplacementOfLost)
                {
                    ReplacementTrans = await FetchReplaceMentRequestId(requestMapped.RequestId);
                }
                if (requestMapped.ReqtypeId == (int)RequestTypeEnum.EndLicences)
                {
                    EndLicences = await FetchEndLicencesTransForRequest(requestMapped.RequestId);
                }
                if (requestMapped.ReqtypeId == (int)RequestTypeEnum.ChangeData)
                {
                    var transactions = await FetchTransactionsAsync(requestMapped.RequestId, serviceId);
                    requestMapped.Transactions = transactions;
                }
                var Mandoob = await FetchMandoobDataAsync(requestMapped.LicenseId ?? 0);
               
                // Return consolidated result
                var result = new RequestDetailsVM
                {
                    RequestDVM = requestMapped ?? new RequestVM(),
                    RequestTransactionVM = employeeLogMapped ?? new List<RequestTransactionVM>(),
                    PersonApplicantVM = applicantMapped ?? new PersonVM(),
                    PreApprovementVM = licencepreApprovementMapped ?? new PreApprovementVM(),
                    ManagerPersonVM = managerMapped ?? new PersonVM(),
                    attachmentVM = attachMapped ?? new List<AttachVM>(),
                    CompanyVM = companyMapped ?? new CompanyVM(),
                    BuildingVM = buildingMapped ?? new BuildingVM(),
                    RequestStatusVM = requestStatusMapped,
                    IsFinalStatus = isFinalStatus, // Include final status flag
                    requestStatus = requestStatus,
                    FlagRequestStatus = requestStatusFlag,
                    AllowedButtons=AllowedButton,
                    LicenceRenewVM = licencesRenew ?? new RenewVM(),
                    EndLicencesTransVM = EndLicences ?? new EndLicencesTransVM(),
                    OwnerChangeTransVM = ChangeOwner ?? new ChangeOwnerTransVM(),
                    ReplacementOfLostTransVM = ReplacementTrans ?? new ReplacementOfLostTransVM(),
                    LicencesVM = licencesMapped ?? new LicencesVM(),
                    ClassificationData = classification?.Branches ?? new List<ClassificationBranchDetail>(),
                    ClassificationName = classification?.ClassificationName ?? "",
                    ClassificationId=classification?.ClassificationId,
                    PaymentDetailsVM = PaymentMapped ?? new PaymentDetailsVM(),
                    Mandoob = Mandoob
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorMessage
                {
                    Error = true,
                    Status = "Failure",
                    Message = ex.Message
                });
            }
        }

        private async Task<List<AllowedButtonVM>> GetAllowedButtonsPerRequest(long requestId, int userId)
        {
            var allowedButtons = new List<AllowedButtonVM>();

            var request = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                .GetbyId(requestId);
            if (request == null) return allowedButtons;

            var workflow = await _unitOfwork.genericRepository<WorkFlow>()
                .GetByCondition(w =>
                    w.CurrentStatusId == request.RequestStatusId &&
                    w.RequestTypeId == request.ReqtypeId &&
                    w.ServiceId == request.ServiceId
                ).FirstOrDefaultAsync();

            if (workflow == null) return allowedButtons;

            var buttons = await _unitOfwork.genericRepository<WorkFlowActionButton>()
                .GetByCondition(b => b.WorkFlowId == workflow.Id)
                .ToListAsync();

            // ✅ إذا لم يكن يتطلب صلاحية → نرجع زرار افتراضي لو مش موجود أصلاً
            if (workflow.IsPermissionRequired != true)
            {
                if (!buttons.Any())
                {
                    var nextStatus = await _unitOfwork.genericRepository<RequestStatusLookup>()
                        .GetByCondition(s => s.Id == workflow.NextStatusId)
                        .FirstOrDefaultAsync();

                    return new List<AllowedButtonVM>
            {
                new AllowedButtonVM
                {
                    Id = 0,
                    ButtonText = nextStatus?.NameAr ?? "الانتقال التالي",
                    NextStatusId = workflow.NextStatusId,
                    ActionKey = "RequestStatusbutton",
                    IsPermissionRequired = false,
                    ReasonIfNotAllowed = "NoPermissionRequired"
                }
            };
                }

                return buttons.Select(b => new AllowedButtonVM
                {
                    Id = b.Id,
                    ButtonText = b.ButtonText,
                    NextStatusId = workflow.NextStatusId,
                    ActionKey = b.ActionKey,
                    IsPermissionRequired = false,
                    ReasonIfNotAllowed = "NoPermissionRequired"
                }).ToList();
            }

            // ✅ لو يتطلب صلاحية: نتحقق من صلاحية المستخدم
            var userRoleId = await _unitOfwork.genericRepository<AspNetUserRoleAdmin>()
                .GetByCondition(r => r.SysUserId == userId)
                .Select(r => r.RoleId)
                .FirstOrDefaultAsync();

            if (userRoleId == 0 || userRoleId == null)
                return allowedButtons;

            var allowedButtonIds = await _unitOfwork.genericRepository<WorkFlowButtonRoleAdmin>()
                .GetByCondition(p => p.RoleAdminId == userRoleId)
                .Select(p => p.WorkFlowActionButtonId)
                .ToListAsync();

            return buttons
                .Where(b => allowedButtonIds.Contains(b.Id))
                .Select(b => new AllowedButtonVM
                {
                    Id = b.Id,
                    ButtonText = b.ButtonText,
                    NextStatusId = workflow.NextStatusId,
                    ActionKey = b.ActionKey,
                    IsPermissionRequired = true,
                    ReasonIfNotAllowed = "PermissionGranted"
                }).ToList();
        }


        private async Task<List<AllowedButtonVM>> GetAllowedButtons(long requestId, int nextStatusId, int userId)
        {
            var allowedButtons = new List<AllowedButtonVM>();

            var request = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                .GetbyId(requestId);
            if (request == null) return allowedButtons;

            var workflow = await _unitOfwork.genericRepository<WorkFlow>()
                .GetByCondition(w =>
                    w.NextStatusId == nextStatusId &&
                    w.RequestTypeId == request.ReqtypeId &&
                    w.ServiceId == request.ServiceId)
                .FirstOrDefaultAsync();
            if (workflow == null) return allowedButtons;

            var buttons = await _unitOfwork.genericRepository<WorkFlowActionButton>()
                .GetByCondition(b => b.WorkFlowId == workflow.Id)
                .ToListAsync();

            // If no permission required, show all buttons
            if (workflow.IsPermissionRequired == false)
            {
                allowedButtons = buttons.Select(b => new AllowedButtonVM
                {
                    Id = b.Id,
                    ButtonText = b.ButtonText,
                    NextStatusId = workflow.NextStatusId,
                    ActionKey = b.ActionKey,
                    IsPermissionRequired = false
                }).ToList();

                return allowedButtons;
            }

            // Else: permission is required → filter based on user's role
            var userRole = await _unitOfwork.genericRepository<AspNetUserRoleAdmin>()
                .GetByCondition(r => r.SysUserId == userId)
                .Select(r => r.RoleId)
                .FirstOrDefaultAsync();

            if (userRole == 0) return allowedButtons; // No role = no permission

            var allowedButtonIds = await _unitOfwork.genericRepository<WorkFlowButtonRoleAdmin>()
                .GetByCondition(p => p.RoleAdminId == userRole)
                .Select(p => p.WorkFlowActionButtonId)
                .ToListAsync();

            allowedButtons = buttons
                .Where(b => allowedButtonIds.Contains(b.Id))
                .Select(b => new AllowedButtonVM
                {
                    Id = b.Id,
                    ButtonText = b.ButtonText,
                    NextStatusId = workflow.NextStatusId,
                    ActionKey = b.ActionKey,
                    IsPermissionRequired = true
                }).ToList();

            return allowedButtons;
        }



        private async Task<ChangeOwnerTransVM> FetchOwnerTransRequestId(long requestId)
        {
            var SpecChangeOwner = new OwnerChangeTransWithSpec((int)ServiceEnum.Tourism, requestId);
            var ChangeOwner = await _unitOfwork.genericRepository<RenouncementTransaction>().GetByIdWithSpec(SpecChangeOwner);
            return _mapper.Map<RenouncementTransaction, ChangeOwnerTransVM>(ChangeOwner);
        }
        private async Task<ReplacementOfLostTransVM> FetchReplaceMentRequestId(long requestId)
        {
            var SpecReplacement = new ReplacementOfLostChangeTransWithSpec((int)ServiceEnum.Tourism, requestId);
            var Replacement = await _unitOfwork.genericRepository<ReplacementOfLostTransaction>().GetByIdWithSpec(SpecReplacement);
            return _mapper.Map<ReplacementOfLostTransaction, ReplacementOfLostTransVM>(Replacement);
        }
        private async Task<EndLicencesTransVM> FetchEndLicencesTransForRequest(long requestId)
        {
            var Specendlicences = new EndingReasonChangeTransWithSpec((int)ServiceEnum.Tourism, requestId);
            var endlicences = await _unitOfwork.genericRepository<LicenseEndingTransaction>().GetByIdWithSpec(Specendlicences);
            return _mapper.Map<LicenseEndingTransaction, EndLicencesTransVM>(endlicences);
        }

        [HttpGet]
        [Route("FetchAllClassificationByRequestId")]
        public async Task<ClassificationResponse> FetchAllClassificationByRequestId(long requestId, int ActivityTypeId)
        {
            // Fetch all evaluations first to avoid concurrent database access
            int categoryId = 0;

            if (ActivityTypeId == (int)ActivityTypeEnum.Hotel || ActivityTypeId == (int)ActivityTypeEnum.Resorts)
            {
                categoryId = (int)CategoryClassificationEnum.Resorts;
            }
            else
            {
                categoryId = (int)CategoryClassificationEnum.HotelApartment;
            }
            //Fetch all class branches and their related hotel classes


            var evaluationsList = await _unitOfwork.genericRepository<TourEvaluationListHotel>()
                .GetByCondition(e => e.RequestId == requestId).ToListAsync();
            var selectedClassificationId = evaluationsList
    .Where(e => e.ClassificationId.HasValue)
    .Select(e => e.ClassificationId.Value)
    .FirstOrDefault();
    //        var classificationName = await _unitOfwork.genericRepository<MoiClassification>()
    //.GetFilteredWithProjection(
    //    filter: c => c.ClassifyId == selectedClassificationId,
    //    selector: c => c.ClassifiyName&&c.ClassifyId
    //).FirstOrDefaultAsync();
            var classification = await _unitOfwork.genericRepository<MoiClassification>()
                .GetByCondition(c => c.ClassifyId == selectedClassificationId).FirstOrDefaultAsync();
            // استرجاع التقييمات (ممتاز، جيد، سيئ)
            var evaluationTypes = await _unitOfwork.genericRepository<TourEvaluationLookUp>()
        .GetFilteredWithProjection(
            filter: null,
            selector: x => new { x.Id, x.Name }
        ).ToListAsync();

            var classBranches = await _unitOfwork.genericRepository<TourClassBranchLookUp>()
             .GetFilteredWithProjection(
               filter: x => x.ClassId == categoryId,
               selector: x => new ClassificationBranchDetail
               {
                   BranchId = x.Id,
                   BranchName = x.Name,
                   HotelClasses = x.TourHotelClassLookUp
                       .Where(t => t.CategoryId == categoryId)
                       .Select(hotelClass => new HotelClassDetail
                       {
                           HotelClassId = hotelClass.Id,
                           HotelClassName = hotelClass.Name,
                           CategoryId = hotelClass.CategoryId,
                           Status = hotelClass.Status,
                           ClassType = hotelClass.TourClassTypeLookUp != null ? new ClassTypeDetail
                           {
                               ClassTypeId = hotelClass.TourClassTypeLookUp.Id,
                               ClassTypeName = hotelClass.TourClassTypeLookUp.Name
                           } : null,
                           // الآن نقوم بربط التقييمات مع الفئة الفندقية بعد تحميل التقييمات
                           Evaluations = new List<EvaluationDetail>()
                       }).ToList()
               }).ToListAsync();




            foreach (var branch in classBranches)
            {
                foreach (var hotelClass in branch.HotelClasses)
                {
                    foreach (var evaluation in evaluationTypes)
                    {
                        //var isSelected = await _unitOfwork.genericRepository<TourEvaluationListHotel>()
                        //    .GetFilteredWithProjection(
                        //        filter: ev => ev.ClassItemId == hotelClass.HotelClassId && ev.EvalitemId == evaluation.Id,
                        //        selector: ev => ev.Id
                        //    ).AnyAsync();  
                        var isSelected = evaluationsList
                     .Any(ev => ev.HotelClassId == hotelClass.HotelClassId && ev.EvalitemId == evaluation.Id);
                        // Set the IsSelected flag for the evaluation
                        hotelClass.Evaluations.Add(new EvaluationDetail
                        {
                            EvaluationId = evaluation.Id,
                            EvaluationName = evaluation.Name,
                            IsSelected = isSelected
                        });
                    }
                }
            }

            return new ClassificationResponse
            {
                ClassificationName = classification?.ClassifiyName,
                ClassificationId=classification.ClassifyId,
                Branches = classBranches
            };
        }

        private async Task<(bool, string, string)> IsFinalCycleStatusAsync(int ActivityTypeId, int RequestTypeId, int serviceId, int Requeststatusid)
        {
            var workflow = await _unitOfwork.genericRepository<WorkFlow>()
                .GetByCondition(w => w.RequestTypeId == RequestTypeId
                                  //&& w.ActivityTypeId == ActivityTypeId
                                  && w.ServiceId == serviceId
                                  && w.NextStatusId == Requeststatusid)
                .FirstOrDefaultAsync();

            string conditionMet = null; // لمعرفة الشرط الذي تحقق
            string FlagrequestStatus = "";

            if (workflow != null && workflow.Conditions != "" && workflow.Conditions != null && workflow.FlagRequestStatus != null)
            {
                var conditions = JsonConvert.DeserializeObject<List<Condition>>(workflow.Conditions);

                if (conditions.Any(c =>
                    c.Field.Equals("RequestStatus", StringComparison.OrdinalIgnoreCase) &&
                    c.Value.ToString().Equals("final", StringComparison.OrdinalIgnoreCase)))
                {
                    conditionMet = "final";
                }

                if (conditions.Any(c =>
                    c.Field.Equals("RequestStatus", StringComparison.OrdinalIgnoreCase) &&
                    c.Value.ToString().Equals("Payment", StringComparison.OrdinalIgnoreCase)))
                {
                    conditionMet = "Payment";
                }

                if (!string.IsNullOrEmpty(conditionMet) || !string.IsNullOrEmpty(FlagrequestStatus))
                {
                    return (true, conditionMet, "No FlagrequestStatus"); // الشرط تحقق مع تحديد نوعه
                }
            }
            else if (workflow != null && workflow.FlagRequestStatus != "" && workflow.FlagRequestStatus != null && workflow.Conditions == "")
            {
                FlagrequestStatus = workflow.FlagRequestStatus;
                return (true, "No Condition met", FlagrequestStatus);
            }
            else if (workflow != null && workflow.FlagRequestStatus == "" && workflow.FlagRequestStatus == null && workflow.Conditions != null && workflow.Conditions != "")
            {
                var conditions = JsonConvert.DeserializeObject<List<Condition>>(workflow.Conditions);

                if (conditions.Any(c =>
                    c.Field.Equals("RequestStatus", StringComparison.OrdinalIgnoreCase) &&
                    c.Value.ToString().Equals("final", StringComparison.OrdinalIgnoreCase)))
                {
                    conditionMet = "final";
                }

                if (conditions.Any(c =>
                    c.Field.Equals("RequestStatus", StringComparison.OrdinalIgnoreCase) &&
                    c.Value.ToString().Equals("Payment", StringComparison.OrdinalIgnoreCase)))
                {
                    conditionMet = "Payment";
                }

                if (!string.IsNullOrEmpty(conditionMet))
                {
                    return (true, conditionMet, "No Flag Request status"); // الشرط تحقق مع تحديد نوعه
                }
            }

            return (false, "No condition met", "No RequestStatus Flag"); // لا يوجد شرط متحقق
        }

        private async Task<PreApprovementVM> FetchLicencePreApproveDataAsync(int? licenseId, int serviceId)
        {
            if (licenseId == null) return null;

            var spec = new PreApprovementWithSpec(licenseId.Value, true);
            var licence = await _unitOfwork.genericRepository<MoiPreApprovement>().GetByIdWithSpec(spec);
            return _mapper.Map<MoiPreApprovement, PreApprovementVM>(licence);
        }
        private async Task<LicencesVM> FetchLicenceDataAsync(int licenseId, int serviceId)
        {
            if (licenseId == null) return null;

            var spec = new LicencesWithSpecificService(licenseId, serviceId);
            var licence = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(spec);
            return _mapper.Map<Licence, LicencesVM>(licence);
        }
        private async Task<RenewVM> FetchLicenceRenewDataAsync(int? licenseId, int serviceId)
        {
            if (licenseId == null) return null;

            var spec = new RenewWithSpec(licenseId.Value, serviceId);
            var licence = await _unitOfwork.genericRepository<LicenseRenew>().GetByIdWithSpec(spec);
            return _mapper.Map<LicenseRenew, RenewVM>(licence);
        }


        private async Task<List<TransactionVM>> FetchTransactionsAsync(long requestId, int serviceId)
        {
            var spec = new TransactionWithSpec(requestId, serviceId);
            var transactions = await _unitOfwork.genericRepository<Transaction>().GetTableWithSpec(spec);

            var transactionVMs = new List<TransactionVM>();

            foreach (var transaction in transactions)
            {
                var transactionVM = _mapper.Map<Transaction, TransactionVM>(transaction);

                // Check transaction type and fetch additional details
                if (transaction.TransTypeId == (int)TransactionTypesEnum.ChangeCompaneName)
                {
                    var CompanychangeWithSpecSpec = new CompanyChangeTransWithSpec(transaction.Id);
                    var CompanyDetails = await _unitOfwork.genericRepository<CompanyNameChangeTransaction>().GetByIdWithSpec(CompanychangeWithSpecSpec);
                    transactionVM.CompanyTransVM = _mapper.Map<CompanyNameChangeTransaction, CompanyTransVM>(CompanyDetails);
                }
                else if (transaction.TransTypeId == (int)TransactionTypesEnum.ChangeAddress)
                {
                    var changeAddressSpec = new AddressChangeTransWithSpec(transaction.Id);
                    var changeAddressDetails = await _unitOfwork.genericRepository<AddressChangeTransaction>().GetByIdWithSpec(changeAddressSpec);
                    transactionVM.AddressChangeTransVM = _mapper.Map<AddressChangeTransaction, AddressChangeTransVM>(changeAddressDetails);
                }
                else if (transaction.TransTypeId == (int)TransactionTypesEnum.ChangeManager)
                {
                    var changeManagerSpec = new ManagerChangeTransWithSpec(transaction.Id);
                    var changeManagerDetails = await _unitOfwork.genericRepository<TchangeManager>().GetByIdWithSpec(changeManagerSpec);
                    transactionVM.ChangeManagerTransVM = _mapper.Map<TchangeManager, ChangeManagerTransVM>(changeManagerDetails);
                }
                else if (transaction.TransTypeId == (int)TransactionTypesEnum.ChangeLicencesName)
                {

                    var changeLicencesNameDetails = await _unitOfwork.genericRepository<LicencesNameChangeTransaction>().GetByIdObject(r => r.TransactionId == transaction.Id);
                    transactionVM.LicencesNameChangeTransactionVM = _mapper.Map<LicencesNameChangeTransaction, LicencesNameChangeTransactionVM>(changeLicencesNameDetails);
                }


                transactionVMs.Add(transactionVM);
            }

            return transactionVMs;
        }

        private async Task<IEnumerable<PartnerVM>> FetchPartnerDataAsync(int? licenseId, int serviceId)
        {
            if (licenseId == null) return null;

            var spec = new PartnerWithSpec(licenseId.Value, serviceId);
            var partners = await _unitOfwork.genericRepository<Partner>().GetTableWithSpec(spec);
            return _mapper.Map<IEnumerable<Partner>, IEnumerable<PartnerVM>>(partners);
        }

        private async Task<PersonVM> FetchManagerDataAsync(string? managerCivilId, int serviceId)
        {
            if (managerCivilId == null) return null;

            var spec = new ManagerApplicantWithSpec(managerCivilId.ToString(), serviceId);
            var manager = await _unitOfwork.genericRepository<Person>().GetByIdWithSpec(spec);
            return _mapper.Map<Person, PersonVM>(manager);
        }
        private async Task<PersonVM> FetchApplicantDataAsync(string? CivilId, int? serviceId)
        {
            if (CivilId == null) return null;

            // var spec = new ManagerApplicantWithSpec(CivilId, serviceId??0);
            var applicant = await _unitOfwork.genericRepository<Person>().GetByCondition(x => x.CivilId == CivilId).FirstOrDefaultAsync();


            return _mapper.Map<Person, PersonVM>(applicant);
        }
        private async Task<AspnetUserVM> FetchMandoobDataAsync(int LicId)
        {
            // Step 1: Get the relation between the license and the multiple user
            var licenceMapping = await _unitOfwork.genericRepository<AspNetMultipleLicenseUser>()
                                                  .GetByCondition(l => l.LicenseId == LicId)
                                                  .FirstOrDefaultAsync();

            if (licenceMapping == null)
                return null;

            // Step 2: Get the MultipleUser entry by the mapping's user ID
            var multipleUser = await _unitOfwork.genericRepository<AspNetMultipleUser>()
                                                .GetByCondition(u => u.Id == licenceMapping.Id) // should be UserId, not Id
                                                .FirstOrDefaultAsync();

            if (multipleUser == null)
                return null;

            // Step 3: Get the actual user (mandoob) by MandoobId
            var mandoob = await _unitOfwork.genericRepository<AspNetUser>()
                                           .GetByCondition(u => u.Id == multipleUser.MandoobId)
                                           .FirstOrDefaultAsync();

            if (mandoob == null)
                return null;

            // Step 4: Map to view model
            return _mapper.Map<AspNetUser, AspnetUserVM>(mandoob);
        }
        private async Task<IEnumerable<AttachVM>> FetchAttachmentsAsync(long requestId, int serviceId)
        {
            var spec = new AttachmentWithSpec(requestId, serviceId);
            var attachments = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>().GetTableWithSpec(spec);
            return _mapper.Map<IEnumerable<MoiEserviceRequestsAttach>, IEnumerable<AttachVM>>(attachments);
        }
        private async Task<PaymentDetailsVM> FetchPaymentsAsync(long requestId, int serviceId)
        {

            var spec = new PaymentDetailsWithSpec(serviceId, requestId);
            var payments = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>().GetByIdWithSpec(spec);
            return _mapper.Map<MoiEserviceRequestPaymentDetail, PaymentDetailsVM>(payments);
        }
        private async Task<IEnumerable<RequestStatusVM>> FetchRequestStatusAsync()
        {
            var statuses = await _unitOfwork.genericRepository<RequestStatusLookup>().GetAll();
            return _mapper.Map<IEnumerable<RequestStatusLookup>, IEnumerable<RequestStatusVM>>(statuses);
        }

        private async Task<CompanyVM> FetchCompanyDataAsync(int? companyId, int serviceId)
        {
            if (companyId == null) return null;

            var spec = new CompanyWithSpec(companyId.Value, serviceId);
            var company = await _unitOfwork.genericRepository<Company>().GetByIdWithSpec(spec);
            return _mapper.Map<Company, CompanyVM>(company);
        }
        private async Task<BuildingVM> FetchBuildingDataAsync(int? companyId, int serviceId)
        {
            if (companyId == null) return null;

            var spec = new CompanyWithSpec(companyId.Value, serviceId);
            var company = await _unitOfwork.genericRepository<Company>().GetByIdWithSpec(spec);
            return _mapper.Map<Company, BuildingVM>(company);
        }
        private async Task<AddressVM> FetchAddressForBuldingDataAsync(int? AddresssId, int serviceId)
        {
            if (AddresssId == null) return null;

            var AddressBuilding = await _unitOfwork.genericRepository<Address>().GetbyId(AddresssId);
            return _mapper.Map<Address, AddressVM>(AddressBuilding);
        }
        private async Task<IEnumerable<RequestTransactionVM>> FetchEmployeeLogAsync(long requestId)
        {
            var transactions = await _unitOfwork.genericRepository<MoiEservicesRequestTransaction>()
                .GetByCondition(r => r.RequestId == requestId)
                .ToListAsync();

            var users = await _unitOfwork.genericRepository<MoiEserviceSysUser>().GetAll();

            return (from transaction in transactions
                    join user in users on transaction.EmployeeId equals user.SysUserId
                    select new RequestTransactionVM
                    {

                        ReqStatusName = transaction.ReqStatusName,
                        Activity = transaction.Activity,
                        RequestId = transaction.RequestId,
                        EmployeeId = transaction.EmployeeId,
                        OperationDate = transaction.OperationDate,
                        EmployeeCivilId = transaction.EmployeeCivilId,
                        Notes = transaction.Notes,
                        EmployeeName = user.Name
                    }).ToList();
        }

        //[HttpGet]
        //[Route("FetchAllClassification")]
        //public async Task<IActionResult> FetchAllClassification(int RequestId)
        //{
        //    var classBranches = await _unitOfwork.genericRepository<TourClassBranchLookUp>()
        //             .GetFilteredWithProjection(
        //                 filter: null,
        //                 selector: x => new {
        //                     BranchId = x.Id,
        //                     BranchName = x.Name,
        //                     HotelClasses = x.TourHotelClassLookUp.Select(hotelClass => new
        //                     {
        //                         HotelClassId = hotelClass.Id,
        //                         HotelClassName = hotelClass.Name,
        //                         CategoryId = hotelClass.CategoryId,
        //                         Status = hotelClass.Status,
        //                         ClassType = hotelClass.TourClassTypeLookUp != null ? new
        //                         {
        //                             ClassTypeId = hotelClass.TourClassTypeLookUp.Id,
        //                             ClassTypeName = hotelClass.TourClassTypeLookUp.Name
        //                         } : null
        //                     }).ToList()
        //                 }).ToListAsync();
        //    return Ok(classBranches);

        //}

        [HttpGet]
        [Route("FetchAllClassification")]
        public async Task<IActionResult> FetchAllClassification()
        {
            // Fetch all evaluations first to avoid concurrent database access
            var evaluations = await _unitOfwork.genericRepository<TourEvaluationLookUp>().GetAll();

            // Fetch all class branches and their related hotel classes
            var classBranches = await _unitOfwork.genericRepository<TourClassBranchLookUp>()
                .GetFilteredWithProjection(
                    filter: null,
                    selector: x => new ClassificationBranchDetail
                    {
                        BranchId = x.Id,
                        BranchName = x.Name,
                        HotelClasses = x.TourHotelClassLookUp.Select(hotelClass => new HotelClassDetail
                        {
                            HotelClassId = hotelClass.Id,
                            HotelClassName = hotelClass.Name,
                            CategoryId = hotelClass.CategoryId,
                            Status = hotelClass.Status,
                            ClassType = hotelClass.TourClassTypeLookUp != null ? new ClassTypeDetail
                            {
                                ClassTypeId = hotelClass.TourClassTypeLookUp.Id,
                                ClassTypeName = hotelClass.TourClassTypeLookUp.Name
                            } : null,
                            Evaluations = evaluations.Select(evaluation => new EvaluationDetail
                            {
                                EvaluationId = evaluation.Id,
                                EvaluationName = evaluation.Name,
                                IsSelected = false // This can be modified later based on the requestId
                            }).ToList()
                        }).ToList()
                    }).ToListAsync();

            return Ok(classBranches);
        }

        [HttpPost]
        [Route("SaveAttachmentAdditional")]
        public async Task<IActionResult> SaveAttachmentAdditional(UpdatedRequestVM updatedRequestVM)
        {
            var request = await GetRequest(updatedRequestVM.RequestId);

            MoiEserviceRequestsAttach RequestAttach = null; // Declare outside the if block
            var Employee = await _unitOfwork.genericRepository<MoiEserviceSysUser>().GetByIdObject(s => s.SysUserId == updatedRequestVM.UserId);
            await _updateDataService.HandleAttachmentsAsync(updatedRequestVM, request, Employee, updatedRequestVM.ServiceId ?? 0);
            var userLog = await SaveUserLog(Employee, "AddAttachmentAdditional", updatedRequestVM);

            await _unitOfwork.Complete();
            return Ok(new
            {
                userLog = userLog
            });
        }

        #region Forms
        [HttpPost]
        [Route("UpdateMoicForm")]
        public async Task<IActionResult> UpdateMoicForm(UpdateMoicViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await UpdateAddressForMoic(model);
            await UpdateCompanyForMoic(model);

            return Ok();
        }

        #endregion

        #region Update Data After Request Status 
        [HttpPost]
        [Route("UpdateRequestStatus")]
        public async Task<IActionResult> UpdateRequestStatus(UpdatedRequestVM updatedRequestVM)
        {
            ///--------------Get Rquest --------------------////
            #region Get Request
            // Get the request using the provided specification
            var request = await GetRequest(updatedRequestVM.RequestId);

            #endregion
            MoiPreApprovement preApprovement = null;
            string Changes = "";
            Licence licence = null;
            // Declare the variables outside the block
            DateTime? IssueDate = null;
            DateTime? ExpireDate = null;


            ///----------Get Payment Per Request-------------///
            #region Get Payment
            if (updatedRequestVM.ReqTypeId != (int)RequestTypeEnum.PreApprovementConvert || updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.PreApprovementNew)
            {

            }
            #endregion
            #region Save Attachment



            MoiEserviceRequestsAttach RequestAttach = null; // Declare outside the if block
            var Employee = await _unitOfwork.genericRepository<MoiEserviceSysUser>().GetByIdObject(s => s.SysUserId == updatedRequestVM.UserId);
            //Get New RequestStatus
            var RequestStatusNew = await _unitOfwork.genericRepository<RequestStatusLookup>().GetByIdObject(r => r.Id == updatedRequestVM.StatusId);
            await _updateDataService.HandleAttachmentsAsync(updatedRequestVM, request, Employee, (int)ServiceEnum.Tourism);

            #endregion
            #region AddAction
            var action = "";
            action = updatedRequestVM.Action switch
            {
                "SendNotifyToUser" => "إرسال ملاحظات للمراجع",
                "RefuseRequestbutton" => "تم رفض المعاملة",
                "RequestStatusbutton" => $"تم تغيير حالة الطلب إلي {RequestStatusNew.NameAr}",
                _ => ""
            };
            #endregion
            #region Save Request Transaction(Add New Request Transaction Every Step)

            var requestTransaction = await SaveRequestTransaction(request, action, RequestStatusNew.NameAr, updatedRequestVM, Employee, (int)ServiceEnum.Tourism);

            #endregion
            #region Save UserLog(Add New Row Log For User Every Step)
            MoiEserviceSysUsersActivityLog userLog = null;

            try
            {
                userLog = await SaveUserLog(Employee, action, updatedRequestVM);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            #endregion
            var (isbool, requeststatus, requestStatusFlag) = await IsFinalCycleStatusAsync(request.ActivityTypeId ?? 0, request.ReqtypeId ?? 0, request.ServiceId ?? 0, updatedRequestVM.StatusId);
            var specRequestNew = new RequestWithSpecificService(updatedRequestVM.RequestId, true);
            var requestNew = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().GetByIdWithSpec(specRequestNew);
            var requesttype = await _unitOfwork.genericRepository<RequestsTypesLookup>().GetFilteredWithProjection(
                                 filter: x => x.Id == updatedRequestVM.ReqTypeId,
                                 selector: x => new
                                 {
                                     NameAr = x.NameAr
                                 }
                                   ).FirstOrDefaultAsync();
            if (updatedRequestVM.ReqTypeId != (int)RequestTypeEnum.ChangeData)
            {
                request.RequestStatusId = updatedRequestVM.StatusId;
            }
            request.RequestNote = updatedRequestVM.Note;
            request.Licno = updatedRequestVM.LicNo;
            if (updatedRequestVM.Flag != "final")
            {
                request.PreApprovalNo = updatedRequestVM.PreApprovalNo;
            }
            if ((updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.PreApprovementConvert || updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.PreApprovementNew) && updatedRequestVM.LicNo != "" && updatedRequestVM.Flag != "final")
            {
                if (updatedRequestVM.Flag == "LicencesNo")
                {
                    request.PreApprovalNo = updatedRequestVM.PreApprovalNo;

                    request.LicIssuedate = DateTime.Now;
                }
            }
            //--------------------------Important Read----------------------------
            //this is handle preapprovemnet in preapprovement page not related to update preapprove when update licences licences
            if (request.ReqtypeId == (int)RequestTypeEnum.PreApprovementNew || request.ReqtypeId == (int)RequestTypeEnum.PreApprovementConvert)
            {
                await HandlePreApprovementUpdate(updatedRequestVM, Employee);
            }
            if (request.ReqtypeId == (int)RequestTypeEnum.Request)
            {
                (IssueDate, ExpireDate) = await CalculateIssueAndExpireDatesAsync(request.RequestId);
                await UpdateLicencesAsync(request, updatedRequestVM, IssueDate, ExpireDate, Employee);
            }
            if (request.ReqtypeId == (int)RequestTypeEnum.Renew)
            {
                (IssueDate, ExpireDate) = await CalculateIssueAndExpireDatesAsync(request.RequestId);
                await HandleRenewal(updatedRequestVM, request, ExpireDate, IssueDate, Employee);
            }
            if (request.ReqtypeId == (int)RequestTypeEnum.Renouncement)
            {
                await HandleRenouncement(updatedRequestVM, request);
            }
            if (request.ReqtypeId == (int)RequestTypeEnum.EndLicences)
            {
                await HandleEndingLicenses(updatedRequestVM, request);
            }
            TourEvaluationListHotel existingEvaluation = null;

            try
            {
                if (request.ReqtypeId == (int)RequestTypeEnum.Classification || request.ReqtypeId == (int)RequestTypeEnum.ReClassification)
                {
                    await HandleClassificationEvaluations(updatedRequestVM, request);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            if (updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.ChangeData)
            {

                await HandleChangeData(updatedRequestVM, Employee);


            }

            //if (RequestAttach != null)
            //{
            //    await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>().Create(RequestAttach);
            //}
            var requestUpdated = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Update(request);
            Console.WriteLine("updated", requestUpdated);
            var result = await _unitOfwork.Complete();
            var AllowedButton = await GetAllowedButtonsPerRequest(updatedRequestVM.RequestId,  updatedRequestVM.UserId ?? 0);

            if (result > 0)
            {
                Console.WriteLine($"Status ID: {updatedRequestVM.StatusId}");
                Console.WriteLine($"License No: {updatedRequestVM.LicNo}");
                Console.WriteLine($"Attachment Path: {updatedRequestVM.FilePath}");
                Console.WriteLine($"Attachment Name: {updatedRequestVM.FileName}");
                Console.WriteLine($"Status Name: {RequestStatusNew.NameAr}");
                Console.WriteLine($"Username: {updatedRequestVM.NameUser}");
                //await SendStatusUpdateEmail(request, action, requesttype.NameAr, ExpireDate, IssueDate);

                return Ok(new
                {
                    statusid = updatedRequestVM.StatusId,
                    issuedatePreApprove = request.LicIssuedate,
                    preApprovalNo = request.PreApprovalNo,
                    licno = updatedRequestVM.LicNo ?? "",
                    attachpath = updatedRequestVM.FilePath ?? string.Empty,
                    attachname = updatedRequestVM.FileName ?? "",
                    statusname = RequestStatusNew.NameAr,
                    requestnew = requestNew,
                    expireDate = ExpireDate ?? null,
                    issueDate = IssueDate ?? null,
                    username = updatedRequestVM.NameUser ?? "Anonymous",
                    requestTransaction = requestTransaction,
                    userLog = userLog,
                    transId = updatedRequestVM.TransId,
                    transTypeId = updatedRequestVM.TransTypeId,
                    isFinalStatus = isbool,
                    requestStatus = requeststatus,
                    AllowedButton= AllowedButton,
                    HotelClassEvaluations = updatedRequestVM.HotelClassEvaluations,
                    showUploadFinalLicenseButtonForTransaction = updatedRequestVM.showUploadFinalLicenseButtonForTransaction,

                });

            }
            else
            {
                return StatusCode(500, "An error occurred while updating the request status");

            }

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
                           .GetByCondition(l => l.ReqTypeId == (int)RequestTypeEnum.ChangeData && l.ServiceId == Request.ServiceId).FirstOrDefaultAsync();
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
                                if (item.TransTypeId == (int)TransactionTypesEnum.ChangeCompaneName)
                                {
                                    await HandleCompanyNameChangeAsync(Request.RequestId, Employee, updatedRequestVM);
                                }
                                if (item.TransTypeId == (int)TransactionTypesEnum.ChangeManager)
                                {
                                    await HandleManagerChangeAsync(Request, Employee, updatedRequestVM);
                                }
                                if (item.TransTypeId == (int)TransactionTypesEnum.ChangeAddress)
                                {
                                    await HandleAddressChangeAsync(Request.RequestId, updatedRequestVM);
                                }
                                if (item.TransTypeId == (int)TransactionTypesEnum.ChangeLicencesName)
                                {
                                    await HandleLicenseNameChangeAsync(Request, updatedRequestVM);
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

        [HttpGet]
        [Route("GetUserDetailsByCivilId")]
        public async Task<dynamic> GetUserDetailsByCivilId(string civilId)
        {
            var userdetails = await _unitOfwork.genericRepository<AspNetUser>()
                          .GetFilteredWithProjection(
                           filter: x => x.CivilId == civilId,
                           selector: x => new
                           {
                               CivilId = x.CivilId,
                               UserName = x.UserName,
                               PhoneNumber = x.PhoneNumber,
                               Email = x.Email,
                               FullNameAr = x.FullNameAr,
                               Mobile = x.Mobile
                           }).FirstOrDefaultAsync();

            if (userdetails != null)
            {
                return userdetails;
            }
            else
            {
                return new ErrorMessage()
                {
                    Error = true,
                    Status = "Failure",
                    Message = "No data Found",
                };
            }

        }
        private async Task HandleRenewal(UpdatedRequestVM updatedRequestVM, MoiEserviceLicensesRequest request, DateTime? ExpireDate, DateTime? IssueDate, MoiEserviceSysUser employee)
        {
            if (updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.Renew)
            {
                var renewSpec = new RenewWithSpec(request.LicenseId ?? 0, request.ServiceId ?? 0);
                var Renew = await _unitOfwork.genericRepository<LicenseRenew>().GetByIdWithSpec(renewSpec);
                //var licencesSpec = new LicencesWithSpecificService(request.LicenseId ?? 0, request.ServiceId ?? 0);
                // var Licences = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
                var Licences = await _unitOfwork.genericRepository<Licence>()
                     .GetByCondition(l => l.LicId == request.LicenseId&&l.ServiceId==request.ServiceId).FirstOrDefaultAsync();
                Renew.RequestStatusId = updatedRequestVM.StatusId;

                DateTime? ExpireDateold = ExpireDate ?? Licences.ExpireDate;
                DateTime? renewDateTrans = updatedRequestVM.requestStatusValue != "final" ? Licences.ExpireDate : ExpireDate;

                if (updatedRequestVM.requestStatusValue == "final" || updatedRequestVM.Flag == "final")
                {
                    Renew.LastUpdateDate = DateTime.Now;
                    Renew.OldExpiryDate = Licences.ExpireDate ?? default(DateTime);
                    Renew.NewExpiryDate = ExpireDate ?? default(DateTime);
                    Renew.LastUpdateUser = employee.Name;
                    Renew.ServiceId = request.ServiceId;
                    Licences.ExpireDate = ExpireDate;
                    Licences.IssueDate = IssueDate;
                    Licences.LastRenewDate = DateTime.Now;
                    Licences.LicStatusId = (int)licencesStatusEnum.Released;
                    
                }

                var renewtransaction = new LicenseRenewTransaction
                {
                    LicExpiredate = ExpireDateold,
                    RequestId = (int)request.RequestId,
                    LicRenewDate = renewDateTrans,
                    RequestStatusId = updatedRequestVM.StatusId,
                    ServiceId = request.ServiceId
                };

                await _unitOfwork.genericRepository<LicenseRenewTransaction>().Create(renewtransaction);
                await _unitOfwork.genericRepository<Licence>().Update(Licences);
                await _unitOfwork.genericRepository<LicenseRenew>().Update(Renew);
            }
        }
        private async Task HandleRenouncement(UpdatedRequestVM updatedRequestVM, MoiEserviceLicensesRequest request)
        {
            if (updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.Renouncement && updatedRequestVM.requestStatusValue == "final")
            {
                var renouncementSpec = new OwnerChangeTransWithSpec(request.ServiceId ?? 0, request.RequestId);
                var renouncement = await _unitOfwork.genericRepository<RenouncementTransaction>().GetByIdWithSpec(renouncementSpec);
                var personNew = await _unitOfwork.genericRepository<Person>().GetByIdObject(p => p.CivilId == renouncement.NewCivilId);

                //request.AppCivilId = renouncement.NewCivilId;
                //var licencesSpec = new LicencesWithSpecificService(request.LicenseId ?? 0, request.ServiceId ?? 0);
                //var licences = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
                var licences = await _unitOfwork.genericRepository<Licence>()
                    .GetByCondition(l => l.LicId == request.LicenseId && l.ServiceId == request.ServiceId).FirstOrDefaultAsync();
                if (updatedRequestVM.Flag == "final")
                {
                    licences.ApplicantCivilId = renouncement.NewCivilId;
                    licences.ApplicantId = personNew.Id;

                    await _unitOfwork.genericRepository<Licence>().Update(licences);
                }
            }
        }
        private async Task HandleEndingLicenses(UpdatedRequestVM updatedRequestVM, MoiEserviceLicensesRequest request)
        {
            if (updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.EndLicences && updatedRequestVM.requestStatusValue == "final")
            {
                var licenEndingWithSpec = new EndingReasonChangeTransWithSpec(request.RequestId);
                var licenEnding = await _unitOfwork.genericRepository<LicenseEndingTransaction>().GetByIdWithSpec(licenEndingWithSpec);
                // var licencesSpec = new LicencesWithSpecificService(request.LicenseId ?? 0, request.ServiceId ?? 0);
                // var licences = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
                var licences = await _unitOfwork.genericRepository<Licence>()
                     .GetByCondition(l => l.LicId == request.LicenseId && l.ServiceId == request.ServiceId).FirstOrDefaultAsync();
                if (updatedRequestVM.Flag == "final")
                {
                    licences.LicStatusId = (int)licencesStatusEnum.Ending;

                    await _unitOfwork.genericRepository<Licence>().Update(licences);
                }
            }
        }
        private async Task HandleCompanyNameChangeAsync(long requestId, MoiEserviceSysUser employee, UpdatedRequestVM updatedRequestVM)
        {
            var companytrans = await _unitOfwork.genericRepository<CompanyNameChangeTransaction>().GetByIdObject(c => c.RequestId == requestId);
            companytrans.LastUpdateDate = DateTime.UtcNow;
            companytrans.LastUpdateUser = employee.Name;

            if (updatedRequestVM.Flag == "final")
            {
                var company = await _unitOfwork.genericRepository<Company>().GetByIdObject(c => c.Id == companytrans.CompId);
                company.DirCompanyAr = companytrans.NewCompanyNameDir;
                company.OwnerCompanyAr = companytrans.NewCompanyNameOwner;
                await _unitOfwork.genericRepository<Company>().Update(company);
            }
        }
        private async Task HandleManagerChangeAsync(MoiEserviceLicensesRequest request, MoiEserviceSysUser employee, UpdatedRequestVM updatedRequestVM)
        {
            var managertrans = await _unitOfwork.genericRepository<TchangeManager>().GetByIdObject(c => c.RequestId == request.RequestId);
            var personmanager = await _unitOfwork.genericRepository<Person>().GetByIdObject(p => p.CivilId == managertrans.ManagerNewcivilid);

            var licencesSpec = new LicencesWithSpecificService(request.LicenseId ?? 0, request.ServiceId ?? 0);
            var licences = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);

            if (updatedRequestVM.Flag == "final")
            {
                //request.ManCivilId = personmanager.CivilId;
                //request.ManagerId = personmanager.Id;
                licences.ManagerId = personmanager.Id;
                licences.ManagerCivilId = personmanager.CivilId;
                await _unitOfwork.genericRepository<Licence>().Update(licences);
            }
        }
        private async Task HandleAddressChangeAsync(long requestId, UpdatedRequestVM updatedRequestVM)
        {
            var addresstrans = await _unitOfwork.genericRepository<AddressChangeTransaction>().GetByIdObject(c => c.RequestId == requestId);
            var address = await _unitOfwork.genericRepository<Address>().GetByIdObject(a => a.Id == addresstrans.AddId);

            if (updatedRequestVM.Flag == "final")
            {
                address.Area = addresstrans.NewArea;
                address.AreaSize = addresstrans.AreaSizeNew;
                address.AalliNo = addresstrans.AalliNoNew;
                address.AreaChartNo = addresstrans.AreaChartNoNew;
                address.BlockArabic = addresstrans.NewBlock;
                address.BuildingName = addresstrans.NewBuildingName;
                address.StreetArabic = addresstrans.NewStreet;
                address.GovernorateArabic = addresstrans.NewGovernorate;
                address.FloorNo = addresstrans.NewFloor;
                await _unitOfwork.genericRepository<Address>().Update(address);
            }
        }
        private async Task HandleLicenseNameChangeAsync(MoiEserviceLicensesRequest request, UpdatedRequestVM updatedRequestVM)
        {
            var LicencesNametrans = await _unitOfwork.genericRepository<LicencesNameChangeTransaction>().GetByIdObject(c => c.RequestId == request.RequestId);
            var licencesSpec = new LicencesWithSpecificService(request.LicenseId ?? 0, request.ServiceId ?? 0);
            var licences = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);

            if (updatedRequestVM.Flag == "final")
            {
                //request.Licname = LicencesNametrans.LicencesNameNew;
                licences.LicName = LicencesNametrans.LicencesNameNew;
                await _unitOfwork.genericRepository<Licence>().Update(licences);
            }
        }
        private async Task HandleClassificationEvaluations(UpdatedRequestVM updatedRequestVM, MoiEserviceLicensesRequest request)
        {
            if (updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.Classification || updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.ReClassification)
            {
                foreach (var evaluation in updatedRequestVM.HotelClassEvaluations)
                {
                    //var existingEvaluation = _unitOfwork.genericRepository<TourEvaluationListHotel>()
                    //    .GetFilteredWithProjection(
                    //        filter: e => e.ClassItemId == evaluation.HotelClassId && e.RequestId == updatedRequestVM.RequestId,
                    //        selector: e => new TourEvaluationListHotel
                    //        {
                    //            EvalitemId = e.EvalitemId,
                    //            ClassItemId = e.ClassItemId,
                    //            ClassificationId = e.ClassificationId,
                    //            RequestId = e.RequestId,
                    //            Id=e.Id
                    //        })
                    //    .FirstOrDefault();

                    var existingEvaluation = await _unitOfwork.genericRepository<TourEvaluationListHotel>()
                          .GetByCondition(t => t.RequestId == updatedRequestVM.RequestId && t.HotelClassId == evaluation.HotelClassId)
                          .FirstOrDefaultAsync();

                    if (existingEvaluation == null)
                    {
                        var newEvaluation = new TourEvaluationListHotel
                        {
                            HotelClassId = evaluation.HotelClassId,
                            EvalitemId = evaluation.EvaluationId,
                            RequestId = updatedRequestVM.RequestId,
                            LicId = request.LicenseId,
                            ClassificationId = updatedRequestVM.ClassificationId  // 👈 Make sure this exists in your viewmodel
                        };

                        await _unitOfwork.genericRepository<TourEvaluationListHotel>().Create(newEvaluation);
                    }
                    else
                    {
                        existingEvaluation.EvalitemId = evaluation.EvaluationId;
                        existingEvaluation.ClassificationId = updatedRequestVM.ClassificationId; // 👈 Optional if already set

                        await _unitOfwork.genericRepository<TourEvaluationListHotel>().UpdateAsync(existingEvaluation);
                    }
                }



                var licence = await _unitOfwork.genericRepository<Licence>().GetByCondition(l => l.LicId == request.LicenseId)
                    .FirstOrDefaultAsync();

                if (updatedRequestVM.Flag == ("final"))
                {
                    var payment = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>
                        ().GetByCondition(p => p.RequestId == updatedRequestVM.RequestId && p.Payed == 1).FirstOrDefaultAsync();
                    var finalClassificationId = updatedRequestVM.HotelClassEvaluations
                             .Where(e => e.ClassificationId != null)
                             .Select(e => e.ClassificationId.Value)
                             .FirstOrDefault();

                    licence.ClassificationId = updatedRequestVM.ClassificationId;
                    licence.ClassificationDate = payment.PaymentDate;
                    await _unitOfwork.genericRepository<Licence>().UpdateAsync(licence);

                }
            }
        }
        private async Task SendStatusUpdateEmail(MoiEserviceLicensesRequest request, string ReqStatus, string requestType, DateTime? ExpireDate, DateTime? IssueDate)
        {
            try
            {
                var UserInformation = await GetUserDetailsByCivilId(request.AppCivilId);
                // Prepare dynamic email content
                var placeholders = new Dictionary<string, string>
        {
            { "{customer_name}", UserInformation.FullNameAr },
            { "{request_no}", request.Reqno },
            { "{phone_no}", UserInformation.Mobile },
            { "{request_type}", requestType },
            { "{request_status}", ReqStatus },
            { "{request_date}", request.Licreqtime?.ToString("yyyy-MM-dd") },
             { "{request_licno}", request.Licno ?? "N/A" },
           { "{request_lictype}", request.LicenceTypeNavigation.NameAr ?? "N/A" },
           { "{request_owner}", request.Licowner ?? "N/A" },

            { "{issue_date}", IssueDate?.ToString("yyyy-MM-dd") },
            { "{expire_date}",ExpireDate?.ToString("yyyy-MM-dd") },
        };

                // Prepare the email body using the template
                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "EmailTemplates", "RequestStatusTemplate.html");
                string emailBody = _emailService.PrepareEmailBody(templatePath, placeholders);
                // Remove rows dynamically based on conditions
                if (string.IsNullOrEmpty(request.Licno))
                {
                    emailBody = RemoveHtmlRow(emailBody, "conditional-license-no");
                }
                if (!IssueDate.HasValue)
                {
                    emailBody = RemoveHtmlRow(emailBody, "conditional-license-issue-date");
                }
                if (!ExpireDate.HasValue)
                {
                    emailBody = RemoveHtmlRow(emailBody, "conditional-license-expire-date");
                }

                // Send the email to the user
                bool isEmailSent = await _emailService.SendEmail(UserInformation.Email, "Request Status Update", emailBody);
                if (isEmailSent)
                {
                    Console.WriteLine("Email sent successfully.");
                }
                else
                {
                    Console.WriteLine("Email failed.");
                }
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
        private string RemoveHtmlRow(string htmlContent, string rowId)
        {
            string pattern = $@"<tr id=""{rowId}"">.*?</tr>";
            return System.Text.RegularExpressions.Regex.Replace(htmlContent, pattern, string.Empty, System.Text.RegularExpressions.RegexOptions.Singleline);
        }
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
                    if (request.ActivityTypeId == (int)ActivityTypeEnum.Hotel)
                    {
                        ExpireDate = IssueDate.Value.AddYears(3).AddDays(-1);
                    }
                    else
                    {
                        ExpireDate = IssueDate.Value.AddYears(1).AddDays(-1);
                    }
                }
            }

            return (IssueDate, ExpireDate);
        }
        private async Task HandleAttachmentsAsync(UpdatedRequestVM updatedRequestVM, MoiEserviceLicensesRequest request, MoiEserviceSysUser employee)
        {
            // Handle new attachment if a file is provided
            if (updatedRequestVM.saveResponseVMs != null)
            {
                foreach (var item in updatedRequestVM.saveResponseVMs)
                {


                    var newAttachment = new MoiEserviceRequestsAttach
                    {
                        AttachName = item.FileName,
                        AttachPath = item.FilePath,
                        AttachRequestid = request.RequestId,
                        ServiceId = (int)ServiceEnum.Tourism,
                        UploadedBy = employee.Name,
                        UploadedDate = DateTime.Now,
                        IsMandatory = true,
                        IsApproved = true,

                        AttachType = ".pdf"
                    };

                    await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>().Create(newAttachment);
                }
            }

            // Update existing attachments based on the provided states
            var existingAttachments = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>()
                .GetByCondition(a => a.AttachRequestid == request.RequestId).ToListAsync();

            if (updatedRequestVM.AttachmentStates != null)
            {
                var updatedAttachmentStates = updatedRequestVM.AttachmentStates;  // Attachment states from the ViewModel

                foreach (var attachment in existingAttachments)
                {
                    var updatedState = updatedAttachmentStates.FirstOrDefault(x => x.AttachmentId == attachment.AttachId);

                    if (updatedState != null)
                    {
                        if (updatedState.State == "checked" && (attachment.IsApproved == null || attachment.IsApproved == false))
                        {
                            // Update the attachment approval state to true
                            attachment.IsApproved = true;
                        }
                        else if (updatedState.State == "unchecked" && attachment.IsApproved == true)
                        {
                            // Update the attachment approval state to false
                            attachment.IsApproved = false;
                        }
                    }
                }

                // Save the updated attachment records
                await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>().UpdateRange(existingAttachments);
            }
        }

        private async Task<MoiEservicesRequestTransaction> SaveRequestTransaction(MoiEserviceLicensesRequest request, string action, string statusName, UpdatedRequestVM updatedRequestVM, MoiEserviceSysUser employee, int serviceId)
        {
            var requestTransaction = new MoiEservicesRequestTransaction
            {
                RequestId = request.RequestId,
                OperationDate = DateTime.UtcNow,
                ServiceId = serviceId,
                LicenseId = request.LicenseId,
                Notes = updatedRequestVM.Note,
                ReqStatusName = statusName,
                ReqStatusId = updatedRequestVM.StatusId,
                EmployeeCivilId = updatedRequestVM.NameUser,
                Activity = action,
                EmployeeId = employee.SysUserId
            };

            await _unitOfwork.genericRepository<MoiEservicesRequestTransaction>().Create(requestTransaction);
            return requestTransaction; // Return the created transaction
        }

        private async Task<MoiEserviceSysUsersActivityLog> SaveUserLog(MoiEserviceSysUser employee, string action, UpdatedRequestVM updatedRequestVM)
        {
            var userLogCreate = new MoiEserviceSysUsersActivityLog
            {
                UserFullName = employee.Name,
                SysUserId = employee.SysUserId,
                ActivityDate = DateTime.UtcNow,
                Note = updatedRequestVM.Note,
                Section = updatedRequestVM.ActionName,
                Activity = action,
                ChangeLogs = string.Join(", ", updatedRequestVM.ChangeLogs)
            };

            await _unitOfwork.genericRepository<MoiEserviceSysUsersActivityLog>().Create(userLogCreate);
            return userLogCreate; // Return the created user log
        }
        private async Task HandlePreApprovementUpdate(UpdatedRequestVM updatedRequestVM, MoiEserviceSysUser employee)
        {
            if (updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.PreApprovementConvert ||
                updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.PreApprovementNew)
            {
                long? Sequence = updatedRequestVM.SequenceNo;
                var specPreapprovement = new PreApprovementWithSpec(updatedRequestVM.RequestId, false);
                var preApprovement = await _unitOfwork.genericRepository<MoiPreApprovement>().GetByIdWithSpec(specPreapprovement);
                preApprovement.ReqStatusId = updatedRequestVM.StatusId;

                if (updatedRequestVM.Flag == "LicencesNo")
                {
                    //preApprovement.IsConsumed = true;
                    //preApprovement.
                    //preApprovement.LicStatusId = (int)licencesStatusEnum.Released;

                    preApprovement.LicenseNo = updatedRequestVM.PreApprovalNo;
                    preApprovement.LicenseIssueDate = DateTime.Now;
                    preApprovement.SequenceNo = Sequence;

                }
                if (updatedRequestVM.Flag == "final")
                {
                    preApprovement.IsConsumed = false;
                    preApprovement.LicStatusId = (int)licencesStatusEnum.Released;

                }
                await _unitOfwork.genericRepository<MoiPreApprovement>().Update(preApprovement);
            }
        }

        private async Task<MoiEserviceLicensesRequest> GetRequest(int requestId)
        {
            var spec = new RequestWithSpecificService(requestId, true);
            return await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().GetByIdWithSpec(spec);
        }
        private async Task UpdateLicencesAsync(MoiEserviceLicensesRequest request, UpdatedRequestVM updatedRequestVM, DateTime? IssueDate, DateTime? ExpireDate, MoiEserviceSysUser employee)
        {
            //Licence licence = null;
            // var licenceSpec = new LicencesWithSpecificService(request.LicenseId ?? 0, true);
            /// licence = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licenceSpec);
            var lic = await _unitOfwork.genericRepository<Licence>()
            .GetByCondition(l => l.LicId == request.LicenseId).FirstOrDefaultAsync();
            if (updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.Request)
            {
                if (updatedRequestVM.requestStatusValue == "final" && updatedRequestVM.requestTypeValue != "Preapprove")
                {


                    lic.LicNo = updatedRequestVM.LicNo ?? "";
                    lic.IssueDate = IssueDate;
                    lic.LicStatusId = (int)licencesStatusEnum.Released;
                    lic.ExpireDate = ExpireDate;

                    await _unitOfwork.genericRepository<Licence>().Update(lic);
                }
            }
            //if(updatedRequestVM.ReqTypeId==(int)RequestTypeEnum.Renew)
            //{
            //    if(updatedRequestVM.Flag =="PreFinal")
            //    {
            //        licence.IssueDate = IssueDate;
            //        licence.LicStatusId = updatedRequestVM.LicStatusId;
            //        licence.ExpireDate = ExpireDate;
            //    }
            //}
        }

        private async Task<CompanyVM> UpdateCompanyForMoic(UpdateMoicViewModel companyChanged)
        {
            var company = await _unitOfwork.genericRepository<Company>().GetbyId(companyChanged.CompanyVM.Id);
            var companymapped = companyChanged.CompanyVM;
            _mapper.Map(companymapped, company);
            _unitOfwork.genericRepository<Company>().Update(company);
            await _unitOfwork.Complete();
            return companymapped;

        }
        private async Task<AddressVM> UpdateAddressForMoic(UpdateMoicViewModel addressChanged)
        {
            var address = await _unitOfwork.genericRepository<Address>().GetbyId(addressChanged.AddressVM.Id);
            var addressmapped = addressChanged.AddressVM;
            _mapper.Map(addressmapped, address);
            _unitOfwork.genericRepository<Address>().Update(address);
            await _unitOfwork.Complete();
            return addressmapped;

        }
        #endregion

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


        #endregion
        #endregion


    }
}
