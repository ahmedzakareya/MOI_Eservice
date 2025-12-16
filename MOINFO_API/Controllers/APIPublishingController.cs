using AutoMapper;
using AutoMapper.QueryableExtensions;
using Business.Enums;
using Business.Interfaces;
using Business.ModelWithSpecification;
using Business.ViewModel;
using Business.ViewModel.Dynamic;
using Business.ViewModel.Tourism;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TransactionEntity = Domain.Entities.Transaction;


namespace MOINFO_API.Controllers
{
    [Route("api/Publishing")]
    [ApiController]

    public class APIPublishingController : Controller
    {
        private readonly IUnitOfwork _unitOfwork;
        private readonly IUpdateDataService _updateDataService;
        private readonly IMapper _mapper;
        private readonly IDataFetchService _dataFetchService;

        public APIPublishingController(IUnitOfwork unitOfwork, IUpdateDataService updateDataService, IMapper mapper, IDataFetchService dataFetchService)
        {
            _unitOfwork = unitOfwork;
            _updateDataService = updateDataService;
            _mapper = mapper;
            _dataFetchService = dataFetchService;
        }

        [HttpGet]
        [Route("GetActivity")]
        public async Task<IEnumerable<ActivityTypeVM>> GetActivity([FromQuery] int ID)
        {
            var entities = await _unitOfwork
                .genericRepository<ActivityTypesLookup>()
                .GetByCondition(c => c.ServiceId == ID)
                .ToListAsync();

            var mapped = _mapper.Map<IEnumerable<ActivityTypesLookup>, IEnumerable<ActivityTypeVM>>(entities);

            return mapped;
        }


        [HttpGet]
        [Route("GetLicenseType")]
        public async Task<IEnumerable<LicencesTypeVM>> GetLicenseType()
        {
            var entities = await _unitOfwork
    .genericRepository<LicenceTypesLookup>()
    .GetByCondition(c => new[] { 2, 3, 4 }.Contains(c.Id))
    .ToListAsync();


            var mapped = _mapper.Map<IEnumerable<LicenceTypesLookup>, IEnumerable<LicencesTypeVM>>(entities);

            return mapped;
        }








        [HttpGet]
        [Route("GetActivities")]
        public async Task<IEnumerable<ActivityTypeVM>> GetActivities([FromQuery] int ID)
        {
            var entities = await _unitOfwork
                .genericRepository<ActivityTypesLookup>()
                .GetByCondition(c => c.ServiceId == ID)
                .ToListAsync();

            var mapped = _mapper.Map<IEnumerable<ActivityTypesLookup>, IEnumerable<ActivityTypeVM>>(entities);

            return mapped;
        }


        [HttpGet]
        [Route("GetTestablishContract")]
        public async Task<IEnumerable<TestablishContractVM>> GetTestablishContract()
        {
            var entities = await _unitOfwork
    .genericRepository<TestablishContract>()
    .GetByCondition(c => true)
    .ToListAsync();


            var mapped = _mapper.Map<IEnumerable<TestablishContract>, IEnumerable<TestablishContractVM>>(entities);

            return mapped;
        }

        [HttpGet]
        [Route("GetPesronTypes")]
        public async Task<IEnumerable<PesronTypeLookUpVM>> GetPesronTypes()
        {
            var entities = await _unitOfwork.genericRepository<PesronTypeLookUp>().GetAllAsync();

            var mapped = _mapper.Map<IEnumerable<PesronTypeLookUp>, IEnumerable<PesronTypeLookUpVM>>(entities);

            return mapped;
        }

        [HttpGet]
        [Route("GetQualificationsLookup")]
        public async Task<IEnumerable<QualificationsLookupVM>> GetQualificationsLookup()
        {
            var entities = await _unitOfwork.genericRepository<QualificationsLookup>().GetAllAsync();

            var mapped = _mapper.Map<IEnumerable<QualificationsLookup>, IEnumerable<QualificationsLookupVM>>(entities);

            return mapped;
        }




        [HttpGet]
        [Route("GetAttachmentForRequest")]
        public async Task<IEnumerable<Business.ViewModel.AttachRuleVM>> GetAttachmentForRequest(string ViewType)
        {
            var entities = await _unitOfwork.genericRepository<AttachRule>().GetByCondition(c => c.ViewType == ViewType && c.FlagView == "user")
    .ToListAsync();

            var mapped = _mapper.Map<IEnumerable<AttachRule>, IEnumerable<Business.ViewModel.AttachRuleVM>>(entities);

            return mapped;
        }

        [HttpGet]
        [Route("GetAttachmentForModify")]
        public async Task<IEnumerable<Business.ViewModel.AttachRuleVM>> GetAttachmentForModify(string ViewType)
        {
            var entities = await _unitOfwork.genericRepository<AttachRule>().GetByCondition(c => c.ViewType == ViewType && c.FlagView == "user" && c.ServiceId == (int)ServiceEnum.publishing)
    .ToListAsync();

            var mapped = _mapper.Map<IEnumerable<AttachRule>, IEnumerable<Business.ViewModel.AttachRuleVM>>(entities);

            return mapped;
        }


        [Route("AddNewCompanyRequest")]
        [HttpPost]
        public async Task<MoiEserviceLicensesRequestVM> AddNewCompanyRequest([FromBody] MoiEserviceLicensesRequestVM model)
        {
            try
            {
                await _unitOfwork.BeginTransactionAsync();

                var nextRequestId = await GetNextRequestIdForService(5);
                model.Reqno = "MOIECR" + nextRequestId;
                model.ServiceId =(int)ServiceEnum.publishing;

                // المالك
                var ownerPerson = new Person
                {
                    CivilId = model.person.CivilId,
                    Name1 = model.person.personName,
                    PersonTypeId = model.person.PersonTypeId,
                    BirthDate = model.person.BirthDate,
                    NationalityId = model.person.NationalityId,
                    NationaltiyNo = model.person.NationaltiyNo,
                    QualificationId = model.person.QualificationId,
                    QualificationDate = model.person.QualificationDate,
                    QualificationCountryId = model.person.QualificationCountryId,
                    CategoryId = 0
                };
                await _unitOfwork.genericRepository<Person>().Create(ownerPerson);
                await _unitOfwork.Complete();
                // المدير
                var managerPerson = new Person
                {
                    CivilId = model.manager.CivilId,
                    Name1 = model.manager.personName,
                    PersonTypeId = model.manager.PersonTypeId,
                    BirthDate = model.manager.BirthDate,
                    NationalityId = model.manager.NationalityId,
                    NationaltiyNo = model.manager.NationaltiyNo,
                    QualificationId = model.manager.QualificationId,
                    QualificationDate = model.manager.QualificationDate,
                    QualificationCountryId = model.manager.QualificationCountryId,
                    CategoryId = 1
                };
                await _unitOfwork.genericRepository<Person>().Create(managerPerson);
                await _unitOfwork.Complete();
                var govGis = await _unitOfwork.genericRepository<GovernoratesLookup>()
                    .GetByCondition(c => c.GisId == model.address.GovernateId).FirstOrDefaultAsync();
                var areaGis = await _unitOfwork.genericRepository<AreasLookup>()
                    .GetByCondition(c => c.GisAreaId == model.address.AreaId).FirstOrDefaultAsync();

                // العنوان
                var addressEntity = new Address
                {
                    AalliNo = model.address.AalliNo,
                    AreaId = areaGis.Id,
                    ServiceId = (int)ServiceEnum.publishing,
                    BuildingName = model.address.BuildingName,
                    FloorNo = model.address.FloorNo,
                    GovernateId = govGis.Id,
                    Name = model.Licname,
                    BuildingNo = model.address.BuildingNo,
                    StreetArabic = model.address.StreetArabic,
                    UnitNo = model.address.UnitNo,
                    GovernorateArabic = govGis.Name,
                    BlockArabic = model.address.BlockArabic,
                    Area = areaGis.Name
                };
                await _unitOfwork.genericRepository<Address>().Create(addressEntity);
                await _unitOfwork.Complete();
                // الشركة
                var companyEntity = new Company
                {
                    Name = model.Licname,
                    ServiceId = (int)ServiceEnum.LocalPress,
                    ActivityTypeId = model.ActivityTypeId,
                    AddressId = addressEntity.Id,
                    FaxNo = model.Company.FaxNo,
                    PhoneNo = model.Company.PhoneNo
                };
                await _unitOfwork.genericRepository<Company>().Create(companyEntity);
                await _unitOfwork.Complete();
                // الرخصة
                var licenseEntity = new Licence
                {
                    ActiivityTypeId = model.ActivityTypeId,
                    ActivityCode = model.ActivityCode,
                    LicTypeId = 2,
                    ApplicantId = ownerPerson.Id,
                    CompanyId = companyEntity.Id,
                    ManagerId = managerPerson.Id,
                    LicStatusId = 1,
                    EstablishingContract = model.testablishContract,
                    ApplicantCivilId = ownerPerson.CivilId,
                    ManagerCivilId = managerPerson.CivilId,
                    LicName = model.Licname
                };
                await _unitOfwork.genericRepository<Licence>().Create(licenseEntity);
                await _unitOfwork.Complete();

                var activity = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                    .GetByCondition(c => c.Id == model.ActivityTypeId).FirstOrDefaultAsync();

                var licenseInfo = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                    .GetByCondition(c => c.ActvityTypeId == model.ActivityTypeId && c.ServiceId == (int)ServiceEnum.publishing && c.ReqTypeId == 1)
                    .FirstOrDefaultAsync();
                var UserInfo = await _unitOfwork.genericRepository<AspNetUser>()
                    .GetByCondition(c => c.Id == model.Requesterid)
                    .FirstOrDefaultAsync();

                // الطلب
                long? Sequence = await GetNextRequestIdForService((int)ServiceEnum.publishing);
                var requestEntity = new MoiEserviceLicensesRequest
                {
                    SequenceNo = Sequence,
                    Reqno = model.Reqno,
                    ReqtypeId = 1,
                    ActivityType = activity?.NameAr,
                    ActivityTypeId = model.ActivityTypeId,
                    ActivityCode = model.ActivityCode,
                    Licowner = model.address.BuildingName,
                    Licname = model.Licname,
                    Licreqtime = DateTime.Now,
                    Requesterid = model.Requesterid,
                    LicStatusId = 1,
                    RequestAttach = "Yes",
                    Licamount = licenseInfo?.FixedFees ?? 0,
                    Licpaystatus = "0",
                    CategoryId = 1,
                    SectorId = 1,
                    LicenseId = licenseEntity.LicId,
                    CompanyId = companyEntity.Id,
                    LicTypeId = 2,
                    ServiceId = (int)ServiceEnum.publishing,
                    AppCivilId = ownerPerson.CivilId,
                    ManCivilId = managerPerson.CivilId,
                    RequestStatusId = 2,

                };
                await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(requestEntity);
                await _unitOfwork.Complete();
                // الشركاء
                foreach (var entity in model.Partners)
                {
                    var partner = new Partner
                    {
                        LicenseId = licenseEntity.LicId,
                        Name = entity.Name,
                        RequestId = requestEntity.RequestId,
                        ServiceId = (int)ServiceEnum.publishing,
                        LastUpdateDate = DateTime.Now,

                    };
                    await _unitOfwork.genericRepository<Partner>().Create(partner);
                }

                var transaction = new Domain.Entities.Transaction
                {
                    RequestDate = DateTime.Now,
                    LicenseId = licenseEntity.LicId,
                    TransTypeId = 1,
                    MotletterNo = model.CentralNoMoci,
                    MotletterDate = model.MociBookDate,
                    RequestId = requestEntity.RequestId
                };

                await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(transaction);

                await _unitOfwork.Complete();
                await _unitOfwork.CommitTransactionAsync();

                var result = _mapper.Map<MoiEserviceLicensesRequestVM>(requestEntity);
                return result;
            }
            catch (Exception)
            {
                await _unitOfwork.RollbackTransactionAsync();
                throw;
            }
        }

        [Route("AddModifyRequest")]
        [HttpPost]
        public async Task<MoiEserviceLicensesRequestVM> AddModifyRequest([FromBody] LicenseModifyModel model)
        {
            // ---------- Guards ----------
            if (model == null) throw new ArgumentNullException(nameof(model));
            if (model.TypeIds == null || !model.TypeIds.Any())
                throw new InvalidOperationException("TypeIds is required.");
            if (model.LiceID <= 0)
                throw new InvalidOperationException("LiceID is required.");

            var sequence = await GetNextRequestIdForService(5);

            // Collect all created IDs here (no behavior change, just collection)
            model.CreatedRequestIds ??= new List<long>();
            model.CreatedTransactionIds ??= new List<int>();

            await _unitOfwork.BeginTransactionAsync();
            try
            {
                // ---------- Load Core Data ----------
                var licence = await _unitOfwork.genericRepository<Licence>()
                    .GetByCondition(c => c.LicId == model.LiceID)
                    .FirstOrDefaultAsync();
                if (licence == null)
                    throw new InvalidOperationException($"License {model.LiceID} not found.");

                var company = await _unitOfwork.genericRepository<Company>()
                    .GetByCondition(c => c.Id == licence.CompanyId)
                    .FirstOrDefaultAsync();

                var activity = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                    .GetByCondition(c => c.Id == licence.ActiivityTypeId)
                    .FirstOrDefaultAsync();

                model.moiEserviceLicensesRequestVM ??= new MoiEserviceLicensesRequestVM();

                bool didWork = false;
                MoiEserviceLicensesRequest requestEntity = null;

                // ---------- Supported Types ----------
                if (model.TypeIds.Contains(1)) // Company Name Change
                {
                    if (model.transactions == null)
                        throw new InvalidOperationException("transactions is required for renewal.");

                    var requesterId = model.moiEserviceLicensesRequestVM.Requesterid;

                    var licenseInfoRenew = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                        .GetByCondition(c =>
                            c.ServiceId ==(int)ServiceEnum.publishing &&
                            c.ActvityTypeId == licence.ActiivityTypeId &&
                            c.TransTypeId == 1)
                        .FirstOrDefaultAsync();

                    var reqno = $"MOIECR{sequence}";

                    requestEntity = new MoiEserviceLicensesRequest
                    {
                        SequenceNo = sequence,
                        Reqno = reqno,
                        ReqtypeId = 12,
                        ActivityType = activity?.NameAr,
                        ActivityTypeId = licence.ActiivityTypeId,
                        ActivityCode = licence.ActivityCode,
                        Licowner = company?.OwnerName,
                        Licname = licence.LicName,
                        Licreqtime = DateTime.Now,
                        Requesterid = requesterId,
                        LicStatusId = 1,
                        RequestAttach = "Yes",
                        Licamount = licenseInfoRenew?.FixedFees ?? 0m,
                        Licpaystatus = "0",
                        CategoryId = 1,
                        SectorId = 1,
                        LicenseId = licence.LicId,
                        CompanyId = licence.CompanyId,
                        LicTypeId = 2,
                        ServiceId =(int)ServiceEnum.publishing,
                        AppCivilId = licence.ApplicantCivilId,
                        ManCivilId = licence.ManagerCivilId,
                        RequestStatusId = 2,
                    };

                    await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(requestEntity);

                    var saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist request (no rows affected).");

                    var transaction = new Domain.Entities.Transaction
                    {
                        RequestDate = DateTime.Now,
                        LicenseId = licence.LicId,
                        TransTypeId = 1,
                        MotletterNo = model.transactions.MotletterNo,
                        MotletterDate = model.transactions.MotletterDate,
                        RequestId = requestEntity.RequestId
                    };
                    await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(transaction);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist transaction (no rows affected).");

                    // >>> Collect IDs
                    model.CreatedRequestIds.Add(requestEntity.RequestId);
                    model.CreatedTransactionIds.Add(transaction.Id);

                    CompanyNameChangeTransaction companyNameChange = new CompanyNameChangeTransaction()
                    {
                        LicenceId = licence.LicId,
                        NewCompanyNameDir = model.CompanyNameChangeTransactionVM.NewCompanyNameDir,
                        OldCompnayNameDir = company.DirCompanyAr,
                        TransactionId = transaction.Id,
                        ServiceId = (int)ServiceEnum.publishing,
                        RequestId = Convert.ToInt32(requestEntity.RequestId),
                    };
                    await _unitOfwork.genericRepository<CompanyNameChangeTransaction>().Create(companyNameChange);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist license renewal (no rows affected).");

                    didWork = true;
                }

                if (model.TypeIds.Contains(2)) // Commercial Name Change
                {
                    if (model.transactions == null)
                        throw new InvalidOperationException("transactions is required for renewal.");

                    var requesterId = model.moiEserviceLicensesRequestVM.Requesterid;

                    var licenseInfoRenew = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                        .GetByCondition(c =>
                            c.ServiceId == (int)ServiceEnum.publishing &&
                            c.ActvityTypeId == licence.ActiivityTypeId &&
                            c.TransTypeId == 1)
                        .FirstOrDefaultAsync();

                    var reqno = $"MOIECR{sequence}";

                    requestEntity = new MoiEserviceLicensesRequest
                    {
                        SequenceNo = sequence,
                        Reqno = reqno,
                        ReqtypeId = 12,
                        ActivityType = activity?.NameAr,
                        ActivityTypeId = licence.ActiivityTypeId,
                        ActivityCode = licence.ActivityCode,
                        Licowner = company?.OwnerName,
                        Licname = licence.LicName,
                        Licreqtime = DateTime.Now,
                        Requesterid = requesterId,
                        LicStatusId = 1,
                        RequestAttach = "Yes",
                        Licamount = licenseInfoRenew?.FixedFees ?? 0m,
                        Licpaystatus = "0",
                        CategoryId = 1,
                        SectorId = 1,
                        LicenseId = licence.LicId,
                        CompanyId = licence.CompanyId,
                        LicTypeId = 2,
                        ServiceId = (int)ServiceEnum.publishing,
                        AppCivilId = licence.ApplicantCivilId,
                        ManCivilId = licence.ManagerCivilId,
                        RequestStatusId = 2,
                    };

                    await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(requestEntity);

                    var saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist request (no rows affected).");

                    var transaction = new Domain.Entities.Transaction
                    {
                        RequestDate = DateTime.Now,
                        LicenseId = licence.LicId,
                        TransTypeId = 2,
                        MotletterNo = model.transactions.MotletterNo,
                        MotletterDate = model.transactions.MotletterDate,
                        RequestId = requestEntity.RequestId
                    };
                    await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(transaction);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist transaction (no rows affected).");

                    // >>> Collect IDs
                    model.CreatedRequestIds.Add(requestEntity.RequestId);
                    model.CreatedTransactionIds.Add(transaction.Id);

                    CommercialNameChangeTransaction ComercialNameChange = new CommercialNameChangeTransaction()
                    {
                        NewCommercialName = model.CommercialNameChangeTransactionVm.NewCommercialName,
                        OldCommercialName = licence.LicName,
                        TransactionId = transaction.Id,
                        ServiceId = (int)ServiceEnum.publishing,
                        RequestId = Convert.ToInt32(requestEntity.RequestId),
                    };
                    await _unitOfwork.genericRepository<CommercialNameChangeTransaction>().Create(ComercialNameChange);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist license renewal (no rows affected).");

                    didWork = true;
                }

                if (model.TypeIds.Contains(3)) // Partners Change
                {
                    if (model.transactions == null)
                        throw new InvalidOperationException("transactions is required for renewal.");

                    var requesterId = model.moiEserviceLicensesRequestVM?.Requesterid;

                    var licenseInfoRenew = await _unitOfwork
                        .genericRepository<MoiEserviceLicenseInfo>()
                        .GetByCondition(c =>
                            c.ServiceId == (int)ServiceEnum.publishing &&
                            c.ActvityTypeId == licence.ActiivityTypeId &&
                            c.TransTypeId == 3)
                        .FirstOrDefaultAsync();

                    var reqno = $"MOIECR{sequence}";

                    var requestEntities = new MoiEserviceLicensesRequest
                    {
                        SequenceNo = sequence,
                        ReqtypeId = 12,
                        Reqno = reqno,
                        ActivityType = activity?.NameAr,
                        ActivityTypeId = licence.ActiivityTypeId,
                        ActivityCode = licence.ActivityCode,
                        Licowner = company?.OwnerName,
                        Licname = licence.LicName,
                        LicenseId = licence.LicId,
                        CompanyId = licence.CompanyId,
                        LicTypeId = 2,
                        ServiceId = (int)ServiceEnum.publishing,
                        AppCivilId = licence.ApplicantCivilId,
                        ManCivilId = licence.ManagerCivilId,
                        Licreqtime = DateTime.Now,
                        Requesterid = requesterId,
                        LicStatusId = 1,
                        RequestAttach = "Yes",
                        Licamount = licenseInfoRenew?.FixedFees ?? 0m,
                        Licpaystatus = "0",
                        CategoryId = 1,
                        SectorId = 1,
                        RequestStatusId = 2
                    };

                    await _unitOfwork
                        .genericRepository<MoiEserviceLicensesRequest>()
                        .Create(requestEntities);

                    var saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist request (no rows affected).");

                    var transaction = new Domain.Entities.Transaction
                    {
                        RequestDate = DateTime.Now,
                        LicenseId = licence.LicId,
                        TransTypeId = 3,
                        MotletterNo = model.transactions.MotletterNo,
                        MotletterDate = model.transactions.MotletterDate,
                        RequestId = requestEntities.RequestId
                    };

                    await _unitOfwork
                        .genericRepository<Domain.Entities.Transaction>()
                        .Create(transaction);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist transaction (no rows affected).");

                    // >>> Collect IDs
                    model.CreatedRequestIds.Add(requestEntities.RequestId);
                    model.CreatedTransactionIds.Add(transaction.Id);

                    if (model?.ChangeOldPartnerTransVM != null && model.ChangeOldPartnerTransVM.Any())
                    {
                        foreach (var item in model.ChangeOldPartnerTransVM)
                        {
                            var oldPartner = new PartnerOldChangeTransaction
                            {
                                TransactionId = transaction.Id,
                                ServiceId = (int)ServiceEnum.publishing,
                                OldPartner = item.OldPartner,
                                RequestId = requestEntities.RequestId,
                                LicencesId = licence.LicId,
                                PartnerIsActive = item.IsActive
                            };
                            await _unitOfwork
                                .genericRepository<PartnerOldChangeTransaction>()
                                .Create(oldPartner);
                        }
                        var savedOld = await _unitOfwork.Complete();
                        if (savedOld <= 0)
                            throw new InvalidOperationException("Failed to persist old partners (no rows affected).");
                    }

                    if (model?.changeNewPartnerTransVM != null && model.changeNewPartnerTransVM.Any())
                    {
                        foreach (var item in model.changeNewPartnerTransVM)
                        {
                            var newPartner = new PartnerNewChangeTransaction
                            {
                                TransactionId = transaction.Id,
                                ServiceId = (int)ServiceEnum.publishing,
                                LastUpdateUser = item.LastUpdateUser,
                                LastUpdateDate = item.LastUpdateDate ?? DateTime.Now,
                                PartId = item.PartId,
                                RequestId = requestEntities.RequestId,
                                LicencesId = licence.LicId,
                                NewPartner = item.NewPartner,
                            };
                            await _unitOfwork
                                .genericRepository<PartnerNewChangeTransaction>()
                                .Create(newPartner);
                        }

                        var savedNew = await _unitOfwork.Complete();
                        if (savedNew <= 0)
                            throw new InvalidOperationException("Failed to persist new partners (no rows affected).");
                    }

                    didWork = true;
                }

                if (model.TypeIds.Contains(4)) // Address Change
                {
                    if (model.transactions == null)
                        throw new InvalidOperationException("transactions is required for renewal.");

                    var requesterId = model.moiEserviceLicensesRequestVM.Requesterid;

                    var licenseInfoRenew = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                        .GetByCondition(c =>
                            c.ServiceId == (int)ServiceEnum.publishing &&
                            c.ActvityTypeId == licence.ActiivityTypeId &&
                            c.TransTypeId == 4)
                        .FirstOrDefaultAsync();

                    var reqno = $"MOIECR{sequence}";

                    requestEntity = new MoiEserviceLicensesRequest
                    {
                        SequenceNo = sequence,
                        Reqno = reqno,
                        ReqtypeId = 12,
                        ActivityType = activity?.NameAr,
                        ActivityTypeId = licence.ActiivityTypeId,
                        ActivityCode = licence.ActivityCode,
                        Licowner = company?.OwnerName,
                        Licname = licence.LicName,
                        Licreqtime = DateTime.Now,
                        Requesterid = requesterId,
                        LicStatusId = 1,
                        RequestAttach = "Yes",
                        Licamount = licenseInfoRenew?.FixedFees ?? 0m,
                        Licpaystatus = "0",
                        CategoryId = 1,
                        SectorId = 1,
                        LicenseId = licence.LicId,
                        CompanyId = licence.CompanyId,
                        LicTypeId = 2,
                        ServiceId = (int)ServiceEnum.publishing,
                        AppCivilId = licence.ApplicantCivilId,
                        ManCivilId = licence.ManagerCivilId,
                        RequestStatusId = 2,
                    };

                    await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(requestEntity);

                    var saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist request (no rows affected).");

                    var transaction = new Domain.Entities.Transaction
                    {
                        RequestDate = DateTime.Now,
                        LicenseId = licence.LicId,
                        TransTypeId = 4,
                        MotletterNo = model.transactions.MotletterNo,
                        MotletterDate = model.transactions.MotletterDate,
                        RequestId = requestEntity.RequestId
                    };
                    await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(transaction);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist transaction (no rows affected).");

                    // >>> Collect IDs
                    model.CreatedRequestIds.Add(requestEntity.RequestId);
                    model.CreatedTransactionIds.Add(transaction.Id);

                    var oldAddress = await _unitOfwork.genericRepository<Address>()
                        .GetByCondition(c => c.Id == company.AddressId)
                        .FirstOrDefaultAsync();

                    AddressChangeTransaction ChangeAddress = new AddressChangeTransaction()
                    {
                        AalliNoOld = oldAddress.AalliNo,
                        OldGovernorate = oldAddress.GovernorateArabic,
                        OldArea = oldAddress.Area,
                        OldBlock = oldAddress.BlockArabic,
                        OldStreet = oldAddress.StreetArabic,
                        OldFloor = oldAddress.FloorNo,
                        OldUnitNo = oldAddress.UnitNo,
                        OldOwnerName = oldAddress.BuildingName,

                        NewAddressAutoNo = model.AddressChangeTransactionVM.AalliNoNew,
                        AddressNew = model.AddressChangeTransactionVM.AddressNew,
                        NewGovernorate = model.AddressChangeTransactionVM.NewGovernorate,
                        NewArea = model.AddressChangeTransactionVM.NewArea,
                        NewBlock = model.AddressChangeTransactionVM.NewBlock,
                        NewStreet = model.AddressChangeTransactionVM.NewStreet,
                        NewFloor = model.AddressChangeTransactionVM.NewFloor,
                        NewUnitNo = model.AddressChangeTransactionVM.NewUnitNo,
                        NewOwnerName = model.AddressChangeTransactionVM.NewOwnerName,
                        NewBuildingName = model.AddressChangeTransactionVM.NewOwnerName,

                        TransactionId = transaction.Id,
                        ServiceId = (int)ServiceEnum.publishing,
                        RequestId = Convert.ToInt32(requestEntity.RequestId),
                    };
                    await _unitOfwork.genericRepository<AddressChangeTransaction>().Create(ChangeAddress);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist license renewal (no rows affected).");

                    didWork = true;
                }

                if (model.TypeIds.Contains(9)) // Manager Change
                {
                    if (model.transactions == null)
                        throw new InvalidOperationException("transactions is required for renewal.");

                    var requesterId = model.moiEserviceLicensesRequestVM.Requesterid;

                    var licenseInfoRenew = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                        .GetByCondition(c =>
                            c.ServiceId == (int)ServiceEnum.publishing &&
                            c.ActvityTypeId == licence.ActiivityTypeId &&
                            c.TransTypeId == 9)
                        .FirstOrDefaultAsync();

                    var reqno = $"MOIECR{sequence}";

                    requestEntity = new MoiEserviceLicensesRequest
                    {
                        SequenceNo = sequence,
                        Reqno = reqno,
                        ReqtypeId = 12,
                        ActivityType = activity?.NameAr,
                        ActivityTypeId = licence.ActiivityTypeId,
                        ActivityCode = licence.ActivityCode,
                        Licowner = company?.OwnerName,
                        Licname = licence.LicName,
                        Licreqtime = DateTime.Now,
                        Requesterid = requesterId,
                        LicStatusId = 1,
                        RequestAttach = "Yes",
                        Licamount = licenseInfoRenew?.FixedFees ?? 0m,
                        Licpaystatus = "0",
                        CategoryId = 1,
                        SectorId = 1,
                        LicenseId = licence.LicId,
                        CompanyId = licence.CompanyId,
                        LicTypeId = 2,
                        ServiceId = (int)ServiceEnum.publishing,
                        AppCivilId = licence.ApplicantCivilId,
                        ManCivilId = licence.ManagerCivilId,
                        RequestStatusId = 2,
                    };

                    await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(requestEntity);

                    var saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist request (no rows affected).");

                    var transaction = new Domain.Entities.Transaction
                    {
                        RequestDate = DateTime.Now,
                        LicenseId = licence.LicId,
                        TransTypeId = 9,
                        MotletterNo = model.transactions.MotletterNo,
                        MotletterDate = model.transactions.MotletterDate,
                        RequestId = requestEntity.RequestId
                    };
                    await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(transaction);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist transaction (no rows affected).");

                    // >>> Collect IDs
                    model.CreatedRequestIds.Add(requestEntity.RequestId);
                    model.CreatedTransactionIds.Add(transaction.Id);

                    var oldManager = await _unitOfwork.genericRepository<Person>()
                        .GetByCondition(c => c.Id == licence.ManagerId)
                        .FirstOrDefaultAsync();

                    TchangeManager ChangeManager = new TchangeManager()
                    {
                        ManagerOldname = oldManager?.Name1,
                        ManagerOldcountryid = oldManager.NationalityId,
                        ManagerOldcivilid = oldManager.CivilId,
                        ManagerOldbirthdate = oldManager.BirthDate.HasValue
                            ? DateOnly.FromDateTime(oldManager.BirthDate.Value)
                            : (DateOnly?)null,
                        ManagerOldqualificationid = oldManager.QualificationId,
                        ManagerNewcivilid = model.ChangeManagerTransVM?.ManagerNewcivilid,
                        ManagerNewname1 = model.ChangeManagerTransVM?.ManagerNewname,
                        ManagerNewbirthdate = model.ChangeManagerTransVM?.ManagerNewbirthdate,
                        //is DateTime dt
                        //    ? DateOnly.FromDateTime(dt)
                        //    : (DateOnly?)null,,
                        ManagerNewqualificationid = model.ChangeManagerTransVM?.ManagerNewqualificationid,
                        ManagerNewcountryid = model.ChangeManagerTransVM?.ManagerNewcountryid,

                        TransactionId = transaction.Id,
                        ServiceId = (int)ServiceEnum.publishing,
                        RequestId = Convert.ToInt32(requestEntity.RequestId),
                    };
                    await _unitOfwork.genericRepository<TchangeManager>().Create(ChangeManager);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist license renewal (no rows affected).");

                    didWork = true;
                }

                if (model.TypeIds.Contains(11)) // Activity Type Change
                {
                    if (model.transactions == null)
                        throw new InvalidOperationException("transactions is required for renewal.");

                    var requesterId = model.moiEserviceLicensesRequestVM.Requesterid;

                    var licenseInfoRenew = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                        .GetByCondition(c =>
                            c.ServiceId == (int)ServiceEnum.publishing &&
                            c.ActvityTypeId == licence.ActiivityTypeId &&
                            c.TransTypeId == 11)
                        .FirstOrDefaultAsync();

                    var reqno = $"MOIECR{sequence}";

                    requestEntity = new MoiEserviceLicensesRequest
                    {
                        SequenceNo = sequence,
                        Reqno = reqno,
                        ReqtypeId = 12,
                        ActivityType = activity?.NameAr,
                        ActivityTypeId = licence.ActiivityTypeId,
                        ActivityCode = licence.ActivityCode,
                        Licowner = company?.OwnerName,
                        Licname = licence.LicName,
                        Licreqtime = DateTime.Now,
                        Requesterid = requesterId,
                        LicStatusId = 1,
                        RequestAttach = "Yes",
                        Licamount = licenseInfoRenew?.FixedFees ?? 0m,
                        Licpaystatus = "0",
                        CategoryId = 1,
                        SectorId = 1,
                        LicenseId = licence.LicId,
                        CompanyId = licence.CompanyId,
                        LicTypeId = 2,
                        ServiceId = (int)ServiceEnum.publishing,
                        AppCivilId = licence.ApplicantCivilId,
                        ManCivilId = licence.ManagerCivilId,
                        RequestStatusId = 2,
                    };

                    await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(requestEntity);

                    var saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist request (no rows affected).");

                    var transaction = new Domain.Entities.Transaction
                    {
                        RequestDate = DateTime.Now,
                        LicenseId = licence.LicId,
                        TransTypeId = 11,
                        MotletterNo = model.transactions.MotletterNo,
                        MotletterDate = model.transactions.MotletterDate,
                        RequestId = requestEntity.RequestId
                    };
                    await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(transaction);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist transaction (no rows affected).");

                    // >>> Collect IDs
                    model.CreatedRequestIds.Add(requestEntity.RequestId);
                    model.CreatedTransactionIds.Add(transaction.Id);

                    ActivityChangeTypeTransaction ChangeActivity = new ActivityChangeTypeTransaction()
                    {
                        NewActivityType = model.ActivityChangeTransVM.NewActivityType,
                        OldActivityType = licence.ActiivityTypeId,
                        TransactionId = transaction.Id,
                        ServiceId =(int)ServiceEnum.publishing,
                        RequestId = Convert.ToInt32(requestEntity.RequestId),
                    };
                    await _unitOfwork.genericRepository<ActivityChangeTypeTransaction>().Create(ChangeActivity);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist license renewal (no rows affected).");

                    didWork = true;
                }

                if (model.TypeIds.Contains(12)) // Replacement of Lost
                {
                    if (model.transactions == null)
                        throw new InvalidOperationException("transactions is required for renewal.");

                    var requesterId = model.moiEserviceLicensesRequestVM.Requesterid;

                    var licenseInfoRenew = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                        .GetByCondition(c =>
                            c.ServiceId ==(int)ServiceEnum.publishing &&
                            c.ActvityTypeId == licence.ActiivityTypeId &&
                            c.TransTypeId == 12)
                        .FirstOrDefaultAsync();

                    var reqno = $"MOIECR{sequence}";

                    requestEntity = new MoiEserviceLicensesRequest
                    {
                        SequenceNo = sequence,
                        Reqno = reqno,
                        ReqtypeId = 12,
                        ActivityType = activity?.NameAr,
                        ActivityTypeId = licence.ActiivityTypeId,
                        ActivityCode = licence.ActivityCode,
                        Licowner = company?.OwnerName,
                        Licname = licence.LicName,
                        Licreqtime = DateTime.Now,
                        Requesterid = requesterId,
                        LicStatusId = 1,
                        RequestAttach = "Yes",
                        Licamount = licenseInfoRenew?.FixedFees ?? 0m,
                        Licpaystatus = "0",
                        CategoryId = 1,
                        SectorId = 1,
                        LicenseId = licence.LicId,
                        CompanyId = licence.CompanyId,
                        LicTypeId = 2,
                        ServiceId =(int)ServiceEnum.publishing,
                        AppCivilId = licence.ApplicantCivilId,
                        ManCivilId = licence.ManagerCivilId,
                        RequestStatusId = 2,
                    };

                    await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(requestEntity);

                    var saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist request (no rows affected).");

                    var transaction = new Domain.Entities.Transaction
                    {
                        RequestDate = DateTime.Now,
                        LicenseId = licence.LicId,
                        TransTypeId = 12,
                        MotletterNo = model.transactions.MotletterNo,
                        MotletterDate = model.transactions.MotletterDate,
                        RequestId = requestEntity.RequestId
                    };
                    await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(transaction);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist transaction (no rows affected).");

                    // >>> Collect IDs
                    model.CreatedRequestIds.Add(requestEntity.RequestId);
                    model.CreatedTransactionIds.Add(transaction.Id);

                    ReplacementOfLostTransaction ChangeActivity = new ReplacementOfLostTransaction()
                    {
                        ReqTransactionId = transaction.Id,
                        LicId = licence.LicId,
                        ServiceId =(int)ServiceEnum.publishing,
                        RequestId = Convert.ToInt32(requestEntity.RequestId),
                    };
                    await _unitOfwork.genericRepository<ReplacementOfLostTransaction>().Create(ChangeActivity);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist license renewal (no rows affected).");

                    didWork = true;
                }

                if (model.TypeIds.Contains(17)) // Renewal
                {
                    if (model.transactions == null)
                        throw new InvalidOperationException("transactions is required for renewal.");

                    var requesterId = model.moiEserviceLicensesRequestVM.Requesterid;

                    var licenseInfoRenew = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                        .GetByCondition(c =>
                            c.ServiceId ==(int)ServiceEnum.publishing &&
                            c.ActvityTypeId == licence.ActiivityTypeId &&
                            c.TransTypeId == 17)
                        .FirstOrDefaultAsync();

                    var reqno = $"MOIECR{sequence}";

                    requestEntity = new MoiEserviceLicensesRequest
                    {
                        SequenceNo = sequence,
                        Reqno = reqno,
                        ReqtypeId = 2,
                        ActivityType = activity?.NameAr,
                        ActivityTypeId = licence.ActiivityTypeId,
                        ActivityCode = licence.ActivityCode,
                        Licowner = company?.OwnerName,
                        Licname = licence.LicName,
                        Licreqtime = DateTime.Now,
                        Requesterid = requesterId,
                        LicStatusId = 1,
                        RequestAttach = "Yes",
                        Licamount = licenseInfoRenew?.FixedFees ?? 0m,
                        Licpaystatus = "0",
                        CategoryId = 1,
                        SectorId = 1,
                        LicenseId = licence.LicId,
                        CompanyId = licence.CompanyId,
                        LicTypeId = 2,
                        ServiceId =(int)ServiceEnum.publishing,
                        AppCivilId = licence.ApplicantCivilId,
                        ManCivilId = licence.ManagerCivilId,
                        RequestStatusId = 2,
                    };

                    await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(requestEntity);

                    var saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist request (no rows affected).");

                    var transaction = new Domain.Entities.Transaction
                    {
                        RequestDate = DateTime.Now,
                        LicenseId = licence.LicId,
                        TransTypeId = 17,
                        MotletterNo = model.transactions.MotletterNo,
                        MotletterDate = model.transactions.MotletterDate,
                        RequestId = requestEntity.RequestId
                    };
                    await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(transaction);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist transaction (no rows affected).");

                    // >>> Collect IDs
                    model.CreatedRequestIds.Add(requestEntity.RequestId);
                    model.CreatedTransactionIds.Add(transaction.Id);

                    var baseDate = licence.ExpireDate ?? DateTime.Today;

                    var newExpiry = baseDate
                        .AddYears(3)   // add 3 years
                        .AddDays(-1);  // subtract 1 day


                    var renew = new LicenseRenew
                    {
                        LicenseId = licence.LicId,
                        OldExpiryDate = licence.ExpireDate,
                        NewExpiryDate = newExpiry,
                        ReqTransId = transaction.Id
                    };
                    await _unitOfwork.genericRepository<LicenseRenew>().Create(renew);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist license renewal (no rows affected).");

                    didWork = true;
                }

                if (model.TypeIds.Contains(66)) // License Ending
                {
                    if (model.transactions == null)
                        throw new InvalidOperationException("transactions is required for renewal.");

                    var requesterId = model.moiEserviceLicensesRequestVM.Requesterid;

                    var licenseInfoRenew = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                        .GetByCondition(c =>
                            c.ServiceId ==(int)ServiceEnum.publishing &&
                            c.ActvityTypeId == licence.ActiivityTypeId &&
                            c.TransTypeId == 19)
                        .FirstOrDefaultAsync();

                    var reqno = $"MOIECR{sequence}";

                    requestEntity = new MoiEserviceLicensesRequest
                    {
                        SequenceNo = sequence,
                        Reqno = reqno,
                        ReqtypeId = 3,
                        ActivityType = activity?.NameAr,
                        ActivityTypeId = licence.ActiivityTypeId,
                        ActivityCode = licence.ActivityCode,
                        Licowner = company?.OwnerName,
                        Licname = licence.LicName,
                        Licreqtime = DateTime.Now,
                        Requesterid = requesterId,
                        LicStatusId = 1,
                        RequestAttach = "Yes",
                        Licamount = licenseInfoRenew?.FixedFees ?? 0m,
                        Licpaystatus = "0",
                        CategoryId = 1,
                        SectorId = 1,
                        LicenseId = licence.LicId,
                        CompanyId = licence.CompanyId,
                        LicTypeId = 2,
                        ServiceId =(int)ServiceEnum.publishing,
                        AppCivilId = licence.ApplicantCivilId,
                        ManCivilId = licence.ManagerCivilId,
                        RequestStatusId = 2,
                    };

                    await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().Create(requestEntity);

                    var saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist request (no rows affected).");

                    var transaction = new Domain.Entities.Transaction
                    {
                        RequestDate = DateTime.Now,
                        LicenseId = licence.LicId,
                        TransTypeId = 19,
                        MotletterNo = model.transactions.MotletterNo,
                        MotletterDate = model.transactions.MotletterDate,
                        RequestId = requestEntity.RequestId
                    };
                    await _unitOfwork.genericRepository<Domain.Entities.Transaction>().Create(transaction);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist transaction (no rows affected).");

                    // >>> Collect IDs
                    model.CreatedRequestIds.Add(requestEntity.RequestId);
                    model.CreatedTransactionIds.Add(transaction.Id);

                    LicenseEndingTransaction EndLicenseTransactoin = new LicenseEndingTransaction()
                    {
                        TransactionId = transaction.Id,
                        LicenseId = licence.LicId,
                        LicExpiredate = licence.ExpireDate,
                        EndReasonId = model.ReasonID,
                        ServiceId =(int)ServiceEnum.publishing,
                        RequestId = Convert.ToInt32(requestEntity.RequestId),
                    };
                    await _unitOfwork.genericRepository<LicenseEndingTransaction>().Create(EndLicenseTransactoin);

                    saved = await _unitOfwork.Complete();
                    if (saved <= 0)
                        throw new InvalidOperationException("Failed to persist license renewal (no rows affected).");

                    didWork = true;
                }

                if (!didWork)
                    throw new InvalidOperationException("No supported TypeIds were processed (expected 17 for renewal).");

                await _unitOfwork.CommitTransactionAsync();

                // Map & return (unchanged), just attach the collected IDs to result
                var result = _mapper.Map<MoiEserviceLicensesRequestVM>(requestEntity) ?? new MoiEserviceLicensesRequestVM();
                if (requestEntity != null)
                {
                    result.Reqno = requestEntity.Reqno;
                    result.ServiceId = requestEntity.ServiceId;
                }

                // >>> Attach all collected IDs to the returned VM
                result.CreatedRequestIds = model.CreatedRequestIds;
                result.CreatedTransactionIds = model.CreatedTransactionIds;

                return result;
            }
            catch (DbUpdateException)
            {
                await _unitOfwork.RollbackTransactionAsync();
                throw;
            }
            catch
            {
                await _unitOfwork.RollbackTransactionAsync();
                throw;
            }
        }



        private async Task<long?> GetNextRequestIdForService(int serviceId)
        {
            var requests = await _unitOfwork
                .genericRepository<MoiEserviceLicensesRequest>()
                .GetAll();

            var lastRequestId = requests
                .Where(x => x.ServiceId == serviceId)
                .Select(x => x.RequestId)
                .DefaultIfEmpty(0)
                .Max();

            return lastRequestId + 1;
        }


        [HttpGet]
        [Route("GetFlowVMAsync")]
        public async Task<WorkFlowVM> GetFlowVMAsync(int requestId)
        {
            try
            {
                const int serviceId =(int)ServiceEnum.publishing; // Fixed service id

                // Get request first
                var request = await _unitOfwork
                    .genericRepository<MoiEserviceLicensesRequest>()
                    .GetByCondition(x =>
                        x.RequestId == requestId &&
                        x.ServiceId == serviceId)
                    .FirstOrDefaultAsync();

                // If no request found, return null
                if (request == null)
                {
                    return null;
                }

                // Prepare workflow entity (initially null)
                WorkFlow entity = null;

                // Only for specific request types (1 or 2)
                if (request.ReqtypeId == 1 || request.ReqtypeId == 2)
                {
                    entity = await _unitOfwork
                        .genericRepository<WorkFlow>()
                        .GetByCondition(x =>
                            x.RequestTypeId == request.ReqtypeId &&
                            x.CurrentStatusId == request.RequestStatusId &&
                            x.ServiceId == serviceId)
                        .FirstOrDefaultAsync();
                }
                else
                {
                    // Get transaction for the request
                    var transaction = await _unitOfwork
                        .genericRepository<Domain.Entities.Transaction>()
                        .GetByCondition(x => x.RequestId == request.RequestId)
                        .FirstOrDefaultAsync();

                    // If no transaction found, cannot resolve workflow
                    if (transaction == null)
                    {
                        return null;
                    }

                    // Get workflow based on request type, status, transaction type and service
                    entity = await _unitOfwork
                        .genericRepository<WorkFlow>()
                        .GetByCondition(x =>
                            x.RequestTypeId == request.ReqtypeId &&
                            x.CurrentStatusId == request.RequestStatusId &&
                            x.TransactionTypeId == transaction.TransTypeId &&
                            x.ServiceId == serviceId)
                        .FirstOrDefaultAsync();
                }

                // If no workflow is found for the given request, return null
                if (entity == null)
                {
                    return null;
                }

                // Map entity to view model
                var vm = new WorkFlowVM
                {
                    TransactionTypeId = entity.TransactionTypeId,
                    CurrentStatusId = entity.CurrentStatusId,
                    // Keep your current logic for NextStatusId
                    NextStatusId = entity.CurrentStatusId
                };

                return vm;
            }
            catch (Exception ex)
            {
                // TODO: log exception (file, DB, logger, etc.)
                throw;
            }
        }



        [HttpPost]
        [Route("SaveAttachments")]
        public async Task<IActionResult> SaveAttachments([FromBody] List<AttachVM> attachments)
        {
            if (attachments == null || attachments.Count == 0)
                return BadRequest(new { Error = true, Message = "قائمة المرفقات فارغة." });

            try
            {
                foreach (var item in attachments)
                {
                    var entity = new MoiEserviceRequestsAttach
                    {
                        //AttachId = item.AttachId,
                        AttachName = item.AttachName,
                        AttachPath = item.AttachPath,
                        ServiceId =(int)ServiceEnum.publishing,
                        AttachRequestid = item.AttachRequestid,
                        AttachStatus = "OK",
                        AttachType = ".pdf",
                        DocType = item.DocType,
                        IsMandatory = item.IsMandatory,
                        UploadedDate = DateTime.Now,
                        UploadedBy = item.UploadedBy,
                        IsApproved = true,
                        IsDeleted = false,
                        TransactionTypeId = item.TransactionTypeId,

                    };

                    await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>().Create(entity);
                }

                await _unitOfwork.Complete();

                return Ok(new { Error = false, Message = "تم حفظ جميع المرفقات بنجاح." });
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error while saving attachments");
                return StatusCode(500, new { Error = true, Message = "حدث خطأ أثناء حفظ المرفقات." });
            }
        }



        [HttpGet]
        [Route("GetRequestDetails/{id}")]
        public async Task<RequestFrontVM> GetRequestDetails(long id)
        {
            //var SpecRequest = new RequestWithSpecificService((int)id, (int)ServiceEnum.Tourism, false);
            var SpecRequest = new RequestWithSpecificService((int)id, true);
            var RequestDetails = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                              .GetByIdWithSpec(SpecRequest);

            var RequestPerSeq = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                            .GetByCondition(x => x.SequenceNo == RequestDetails.SequenceNo).ToListAsync();

            var renew = new LicenseRenew();
            //var RenewVN = new RenewVM();

            // Keep a reference to the specific request used for the renew
            MoiEserviceLicensesRequest requestForRenew = null;
            MoiEserviceLicensesRequest requestForChangeCompany = null;// scope outside loop
            MoiEserviceLicensesRequest requestForChangeCommercialName = null;
            MoiEserviceLicensesRequest requestForChangeActivityName = null;
            MoiEserviceLicensesRequest requestForChangeManager = null;
            MoiEserviceLicensesRequest requestForChangePartners = null;


            // Explicit type for name-change transaction (nullable)
            CompanyNameChangeTransaction? companyNameChangeTransaction = null;
            CommercialNameChangeTransaction? commercialNameChangeTransaction = null;
            ActivityChangeTypeTransaction? activityChangeTypeTransaction = null;
            TchangeManager? tchangeManagerTransaction = null;
            List<PartnerNewChangeTransaction>? partnerNewChangeTransaction = null;
            List<PartnerOldChangeTransaction>? partnerOldChangeTransaction = null;
            foreach (var item in RequestPerSeq)
            {
                // التجديد
                if (item.ReqtypeId == 2)
                {
                    var RenewTransaction = await _unitOfwork.genericRepository< TransactionEntity>()
                            .GetByCondition(x => x.RequestId == item.RequestId).FirstOrDefaultAsync();

                    requestForRenew = await _unitOfwork
                        .genericRepository<MoiEserviceLicensesRequest>()
                        .GetByCondition(x => x.RequestId == item.RequestId)
                        .FirstOrDefaultAsync();



                    renew = await _unitOfwork.genericRepository<LicenseRenew>()
                            .GetByCondition(x => x.ReqTransId == RenewTransaction.Id).FirstOrDefaultAsync();


                }


                if (item.ReqtypeId == 12)
                {
                    var transactions = await _unitOfwork.genericRepository<TransactionEntity>()
                            .GetByCondition(x => x.RequestId == item.RequestId).ToListAsync();

                    foreach (var itemTransaction in transactions)
                    {
                        // تغيير اسم الشركة

                        if (itemTransaction.TransTypeId == 1)
                        {
                            companyNameChangeTransaction = await _unitOfwork.genericRepository<CompanyNameChangeTransaction>()
                                .GetByCondition(x => x.RequestId == itemTransaction.RequestId)
                                .FirstOrDefaultAsync();

                            requestForChangeCompany = await _unitOfwork
                        .genericRepository<MoiEserviceLicensesRequest>()
                        .GetByCondition(x => x.RequestId == item.RequestId)
                        .FirstOrDefaultAsync();
                        }
                        // تغيير الاسم التجاري
                        if (itemTransaction.TransTypeId == 2)
                        {
                            commercialNameChangeTransaction = await _unitOfwork.genericRepository<CommercialNameChangeTransaction>()
                                .GetByCondition(x => x.RequestId == itemTransaction.RequestId)
                                .FirstOrDefaultAsync();

                            requestForChangeCommercialName = await _unitOfwork
                        .genericRepository<MoiEserviceLicensesRequest>()
                        .GetByCondition(x => x.RequestId == item.RequestId)
                        .FirstOrDefaultAsync();
                        }
                        // دخول  وخروج الشركاء 
                        if (itemTransaction.TransTypeId == 3)
                        {
                            partnerNewChangeTransaction = await _unitOfwork.genericRepository<PartnerNewChangeTransaction>()
                                .GetByCondition(x => x.RequestId == itemTransaction.RequestId)
                                .ToListAsync();
                            partnerOldChangeTransaction = await _unitOfwork.genericRepository<PartnerOldChangeTransaction>()
                               .GetByCondition(x => x.RequestId == itemTransaction.RequestId)
                               .ToListAsync();

                            requestForChangePartners = await _unitOfwork
                        .genericRepository<MoiEserviceLicensesRequest>()
                        .GetByCondition(x => x.RequestId == item.RequestId)
                        .FirstOrDefaultAsync();
                        }
                        // تغيير النشاط
                        if (itemTransaction.TransTypeId == 11)
                        {
                            activityChangeTypeTransaction = await _unitOfwork.genericRepository<ActivityChangeTypeTransaction>()
                                .GetByCondition(x => x.RequestId == itemTransaction.RequestId)
                                .FirstOrDefaultAsync();

                            requestForChangeActivityName = await _unitOfwork
                        .genericRepository<MoiEserviceLicensesRequest>()
                        .GetByCondition(x => x.RequestId == item.RequestId)
                        .FirstOrDefaultAsync();

                        }
                        // تغيير المدير المسئول 
                        if (itemTransaction.TransTypeId == 9)
                        {
                            tchangeManagerTransaction = await _unitOfwork.genericRepository<TchangeManager>()
                                .GetByCondition(x => x.RequestId == itemTransaction.RequestId)
                                .FirstOrDefaultAsync();

                            requestForChangeManager = await _unitOfwork
                        .genericRepository<MoiEserviceLicensesRequest>()
                        .GetByCondition(x => x.RequestId == item.RequestId)
                        .FirstOrDefaultAsync();

                        }

                    }
                }

            }

            var PaymentPerRequest = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>()
                            .GetByCondition(x => x.RequestId == id).FirstOrDefaultAsync();

            var AttachmenttRequest = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>()
                           .GetByCondition(x => x.AttachRequestid == id && !(x.IsLatest == false && x.IsApproved == true)).ToListAsync();

            var UserApplicant = await _unitOfwork.genericRepository<AspNetUser>()
                          .GetByCondition(x => x.CivilId == RequestDetails.AppCivilId).FirstOrDefaultAsync();

            var license = await _unitOfwork.genericRepository<Licence>()
                          .GetByCondition(x => x.LicId == RequestDetails.LicenseId).FirstOrDefaultAsync();

            var partners = await _unitOfwork.genericRepository<Partner>()
                .GetByCondition(p => p.LicenseId == RequestDetails.LicenseId)
                .AsNoTracking()
                .ToListAsync();

            var qCountry = _unitOfwork.genericRepository<CountriesLookup>()
               .GetByCondition(_ => true)
               .AsNoTracking();

            var qManager = _unitOfwork.genericRepository<Person>()
                .GetByCondition(x => x.Id == license.ManagerId).Include(p => p.QualificationsLookup)
                .AsNoTracking();

            var mgrResult = await (
                from p in qManager
                join c in qCountry on p.NationalityId equals c.Id into gj
                from c in gj.DefaultIfEmpty()
                select new { Person = p, CountryName = c != null ? c.Name : null, QualificationName = p.QualificationsLookup.Name }
            ).FirstOrDefaultAsync();



            var manager = mgrResult?.Person;
            if (manager != null)
                manager.NationaliyName = mgrResult?.CountryName;
            manager.QualificationsLookup.Name = mgrResult?.QualificationName;

            var qPerson = _unitOfwork.genericRepository<Person>()
                .GetByCondition(x => x.Id == license.ManagerId).Include(p => p.QualificationsLookup)
                .AsNoTracking();

            var result = await (
                from p in qPerson
                join c in qCountry on p.NationalityId equals c.Id into gj

                from c in gj.DefaultIfEmpty()
                select new { Person = p, CountryName = c != null ? c.Name : null, QualificationName = p.QualificationsLookup.Name }
            ).FirstOrDefaultAsync();

            var person = result?.Person;
            person.NationaliyName = result?.CountryName;
            person.QualificationsLookup.Name = result?.QualificationName;




            List<string> allowedGroups = new();
            List<string> ViewType = new();



            var fileUploadConfigs = await _unitOfwork.genericRepository<AttachRule>()
               .GetByCondition(f => f.ServiceId == RequestDetails.ServiceId
               && f.RequestStatusId == RequestDetails.RequestStatusId
               && f.RequestTypeId == RequestDetails.ReqtypeId && f.FlagView == "user")
               .ToListAsync();

            // ===== Build RenewVM and inject its inner RequestDVM (minimal, non-breaking) =====
            var renewVM = _mapper.Map<LicenseRenew, RenewVM>(renew) ?? new RenewVM(); // map renew
            renewVM.RequestDetailsVM ??= new RequestDetailsVM();                       // ensure inner container

            var companyVM = _mapper.Map<CompanyNameChangeTransaction, CompanyTransVM>(companyNameChangeTransaction);
            if (companyVM != null)
            {
                companyVM.RequestDetailsVM ??= new RequestDetailsVM();
            }

            var CommercialVM = _mapper.Map<CommercialNameChangeTransaction, CommercialTransVM>(commercialNameChangeTransaction);
            if (CommercialVM != null)
            {
                CommercialVM.RequestDetailsVM ??= new RequestDetailsVM();

            }

            var ActivityChangeVM = _mapper.Map<ActivityChangeTypeTransaction, ActivityChangeTransVM>(activityChangeTypeTransaction);
            if (ActivityChangeVM != null)
            {
                ActivityChangeVM.OldActivityName = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                          .GetByCondition(x => x.Id == activityChangeTypeTransaction.OldActivityType).Select(c => c.NameAr).FirstOrDefaultAsync();
                ActivityChangeVM.NewActivityName = await _unitOfwork.genericRepository<ActivityTypesLookup>()
                          .GetByCondition(x => x.Id == activityChangeTypeTransaction.NewActivityType).Select(c => c.NameAr).FirstOrDefaultAsync();
                ActivityChangeVM.RequestDetailsVM ??= new RequestDetailsVM();

            }
            var TchangeManagerVM = _mapper.Map<TchangeManager, ChangeManagerTransVM>(tchangeManagerTransaction);
            if (TchangeManagerVM != null)
            {
                TchangeManagerVM.OldNationalityName = await _unitOfwork.genericRepository<CountriesLookup>()
                          .GetByCondition(x => x.Id == TchangeManagerVM.ManagerOldcountryid).Select(c => c.Name).FirstOrDefaultAsync();
                TchangeManagerVM.OldQualificationName = await _unitOfwork.genericRepository<QualificationsLookup>()
                          .GetByCondition(x => x.Id == TchangeManagerVM.ManagerOldqualificationid).Select(c => c.Name).FirstOrDefaultAsync();
                TchangeManagerVM.OldQualificationCountryName = await _unitOfwork.genericRepository<CountriesLookup>()
                          .GetByCondition(x => x.Id == TchangeManagerVM.ManagerNewqualificationCountryid).Select(c => c.Name).FirstOrDefaultAsync();
                TchangeManagerVM.NewNationality = await _unitOfwork.genericRepository<CountriesLookup>()
                          .GetByCondition(x => x.Id == TchangeManagerVM.ManagerNewcountryid).Select(c => c.Name).FirstOrDefaultAsync();
                TchangeManagerVM.NewQualificationName = await _unitOfwork.genericRepository<QualificationsLookup>()
                          .GetByCondition(x => x.Id == TchangeManagerVM.ManagerNewqualificationid).Select(c => c.Name).FirstOrDefaultAsync();

            }
            var ChangeNewPartnerTransVM = _mapper.Map<List<PartnerNewChangeTransaction>, List<ChangeNewPartnerTransVM>>(partnerNewChangeTransaction);
            if (ChangeNewPartnerTransVM != null)
            {
                // map the specific request and place it inside RenewVM -> RequestDetailsVM -> RequestDVM
                //ChangeNewPartnerTransVM.RequestDetailsVM.RequestDVM =
                //    _mapper.Map<MoiEserviceLicensesRequest, RequestVM>(requestForChangePartners);
            }
            var ChangeOldPartnerTransVM = _mapper.Map<List<PartnerOldChangeTransaction>, List<ChangeOldPartnerTransVM>>(partnerOldChangeTransaction);
            if (ChangeNewPartnerTransVM != null)
            {
                // map the specific request and place it inside RenewVM -> RequestDetailsVM -> RequestDVM
                //ChangeNewPartnerTransVM.RequestDetailsVM.RequestDVM =
                //    _mapper.Map<MoiEserviceLicensesRequest, RequestVM>(requestForChangePartners);
            }
            if (requestForChangeCompany != null)
            {
                companyVM.RequestDetailsVM.RequestDVM =
                    _mapper.Map<MoiEserviceLicensesRequest, RequestVM>(requestForChangeCompany);
            }
            if (requestForChangeCommercialName != null)
            {
                CommercialVM.RequestDetailsVM.RequestDVM =
                    _mapper.Map<MoiEserviceLicensesRequest, RequestVM>(requestForChangeCommercialName);
            }
            if (requestForChangeActivityName != null)
            {
                ActivityChangeVM.RequestDetailsVM.RequestDVM =
                    _mapper.Map<MoiEserviceLicensesRequest, RequestVM>(requestForChangeActivityName);
            }

            // ===============================================================================




            return new RequestFrontVM
            {
                RequestVM = _mapper.Map<MoiEserviceLicensesRequest, RequestVM>(RequestDetails),
                PaymentDetailsVM = _mapper.Map<MoiEserviceRequestPaymentDetail, PaymentDetailsVM>(PaymentPerRequest),
                attachVMs = _mapper.Map<IEnumerable<MoiEserviceRequestsAttach>, IEnumerable<AttachVM>>(AttachmenttRequest),
                AspnetUserVM = _mapper.Map<AspNetUser, AspnetUserVM>(UserApplicant),
                ApplicantPerson = _mapper.Map<Person, PersonVM>(person),
                fileUploadConfigs = _mapper.Map<List<AttachRule>, List<AddAttachmentsRulesVM>>(fileUploadConfigs),
                LicencesVM = _mapper.Map<Licence, LicencesVM>(license),
                ManagerPerson = _mapper.Map<Person, PersonVM>(manager),
                partnerVM = _mapper.Map<List<Partner>, List<PartnerVM>>(partners),
                RequestListVM = _mapper.Map<List<MoiEserviceLicensesRequest>, List<RequestVM>>(RequestPerSeq),

                // Use the prepared RenewVM that now carries RequestDetailsVM.RequestDVM inside it
                RenewRequest = renewVM,
                requestForRenew = _mapper.Map<MoiEserviceLicensesRequest, RequestVM>(requestForRenew),
                // Map entity -> VM; AutoMapper will return null if source is null
                CompanyTransVM = companyVM,
                CommercialTransVM = CommercialVM,
                ActivityChangeTransVM = ActivityChangeVM,
                ChangeManagerTransVM = TchangeManagerVM,
                ChangeNewPartnerTransVM = ChangeNewPartnerTransVM,
                ChangeOldPartnerTransVM = ChangeOldPartnerTransVM,
            };
        }



        [HttpGet]
        [Route("LicenseModifyModel")]
        public async Task<LicenseModifyModel> LicenseModifyModel(int licenseID)
        {
            // 1) Load license
            var license = await _unitOfwork.genericRepository<Licence>()
                .GetByCondition(x => x.LicId == licenseID)
                .FirstOrDefaultAsync();

            // 2) Build allowed TransTypeIds per license type
            var ids = (license?.LicTypeId == 2)
                ? new[] { 1, 2, 3, 4, 5, 9, 11, 12, 17, 19 }
                : new[] { 2, 4, 5, 11, 12, 17, 18, 19 };

            // If license is expiring soon, ensure (17) is included
            if (license?.ExpireDate != null && license.ExpireDate <= DateTime.Now.AddMonths(3))
            {
                if (!ids.Contains(17))
                    ids = ids.Append(17).ToArray();
            }

            var idSet = new HashSet<int>(ids);

            // 3) Load transaction types (VM list)
            var transactionTypes = ((await _unitOfwork
                    .genericRepository<TransactionTypesLookup>()
                    .GetAllAsync()) ?? Enumerable.Empty<TransactionTypesLookup>())
                .Where(t => idSet.Contains(t.Id))
                .ToList();

            var transactionVm = _mapper.Map<List<TransactionTypesLookupVM>>(transactionTypes)
                                ?? new List<TransactionTypesLookupVM>();

            // Optional: add "طلب إنهاء" (Id = 66) if present in allowed ids and missing in VM
            if (idSet.Contains(66) && !transactionVm.Any(t => t.Id == 66))
            {
                transactionVm.Add(new TransactionTypesLookupVM
                {
                    Id = 66,
                    NameAr = "طلب إنهاء",
                    IsAvailable = true
                });
            }

            // 4) Request statuses considered closed
            var closedStatuses = new HashSet<int?> { 8, 9, 10, 13 };

            // 5) Correct previous-request detection by Transaction.TransTypeId (nullable-safe)
            var latestActiveByTransType = await
            (
                from t in _unitOfwork.genericRepository<TransactionEntity>()
                                     .GetByCondition(x => x.LicenseId == licenseID)
                                     .AsNoTracking()
                join r in _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                                     .GetByCondition(_ => true)
                                     .AsNoTracking()
                    on t.RequestId equals r.RequestId
                // >>> FIX: guard null and use .Value
                where t.TransTypeId.HasValue
                      && idSet.Contains(t.TransTypeId.Value)
                      && !closedStatuses.Contains(r.RequestStatusId)
                orderby r.RequestId descending
                // >>> FIX: project TransTypeId as non-nullable int
                select new { TransTypeId = t.TransTypeId.Value, Request = r }
            ).ToListAsync();

            // 6) Keep the latest active request per TransTypeId
            var byType = latestActiveByTransType
                .GroupBy(x => x.TransTypeId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(y => y.Request).First()
                );

            // 7) Mark availability per transaction type VM
            foreach (var vm in transactionVm)
            {
                if (byType.TryGetValue(vm.Id, out var req))
                {
                    vm.IsAvailable = false;
                    vm.PreviousRequestID = req.RequestId;
                    vm.PreviousRequestNo = req.Reqno;
                }
                else
                {
                    vm.IsAvailable = true;
                    vm.PreviousRequestID = null;
                    vm.PreviousRequestNo = null;
                }
            }

            // 8) Map license details
            var licenseDetails = _mapper.Map<Licence, LicencesVM>(license);

            // 9) Return model
            return new LicenseModifyModel
            {
                LiceID = license?.LicId ?? licenseID,
                LicNo = license?.LicNo,
                transactionVm = transactionVm,
                LicenceDetailsVM = licenseDetails,
            };
        }




        [HttpGet("GetLicenseDetails/{id:int}")]
        public async Task<ActionResult<LicencesVM>> GetLicenseDetails(int id, CancellationToken ct)
        {
            if (id <= 0)
                return BadRequest("Bad Request");

            try
            {
                // 1) Base license -> LicencesVM
                var baseQuery = _unitOfwork
                    .genericRepository<Licence>()
                    .GetByCondition(x => x.LicId == id)
                    .AsNoTracking();

                var vm = await baseQuery
                    .ProjectTo<LicencesVM>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(ct);

                if (vm == null)
                    return NotFound();

                // Holders for related entities
                Company? company = null;
                Address? address = null;
                Person? manager = null;
                Person? applicant = null;

                // 2) Company & Address (if available)
                if (vm.CompanyId > 0)
                {
                    company = await _unitOfwork.genericRepository<Company>()
                        .GetByCondition(x => x.Id == vm.CompanyId)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(ct);

                    if (company?.AddressId > 0)
                    {
                        address = await _unitOfwork.genericRepository<Address>()
                            .GetByCondition(x => x.Id == company.AddressId)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(ct);
                    }
                }

                // 3) Manager + lookups (NationalityName, QualificationName, QualificationCountry)
                if (vm.ManagerId.HasValue && vm.ManagerId.Value > 0)
                {
                    manager = await _unitOfwork.genericRepository<Person>()
                        .GetByCondition(x => x.Id == vm.ManagerId.Value)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(ct);

                    if (manager != null)
                    {
                        // Pull only the names (efficient)
                        string? nationalityName = null;
                        if (manager.NationalityId > 0)
                        {
                            nationalityName = await _unitOfwork.genericRepository<CountriesLookup>()
                                .GetByCondition(c => c.Id == manager.NationalityId)
                                .AsNoTracking()
                                .Select(c => c.Name)
                                .FirstOrDefaultAsync(ct);
                        }

                        string? qualificationName = null;
                        if (manager.QualificationId > 0)
                        {
                            qualificationName = await _unitOfwork.genericRepository<QualificationsLookup>()
                                .GetByCondition(q => q.Id == manager.QualificationId)
                                .AsNoTracking()
                                .Select(q => q.Name)
                                .FirstOrDefaultAsync(ct);
                        }

                        string? qualificationCountryName = null;
                        if (manager.QualificationCountryId > 0)
                        {
                            qualificationCountryName = await _unitOfwork.genericRepository<CountriesLookup>()
                                .GetByCondition(c => c.Id == manager.QualificationCountryId)
                                .AsNoTracking()
                                .Select(c => c.Name)
                                .FirstOrDefaultAsync(ct);
                        }

                        // Map once to PersonVM, then set derived names
                        var managerVm = _mapper.Map<PersonVM>(manager);
                        managerVm.NationaliyName = nationalityName;
                        managerVm.QualificationName = qualificationName;
                        managerVm.QualificationCountry = qualificationCountryName;

                        // Assign ONCE; do not overwrite later
                        vm.Manager = managerVm;
                    }
                    else
                    {
                        vm.Manager = null;
                    }
                }
                else
                {
                    vm.Manager = null;
                }


                // 3) Manager + lookups (NationalityName, QualificationName, QualificationCountry)
                if (vm.ApplicantId.HasValue && vm.ApplicantId.Value > 0)
                {
                    applicant = await _unitOfwork.genericRepository<Person>()
                        .GetByCondition(x => x.Id == vm.ApplicantId.Value)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(ct);

                    if (applicant != null)
                    {
                        // Pull only the names (efficient)
                        string? nationalityName = null;
                        if (applicant.NationalityId > 0)
                        {
                            nationalityName = await _unitOfwork.genericRepository<CountriesLookup>()
                                .GetByCondition(c => c.Id == applicant.NationalityId)
                                .AsNoTracking()
                                .Select(c => c.Name)
                                .FirstOrDefaultAsync(ct);
                        }

                        string? qualificationName = null;
                        if (applicant.QualificationId > 0)
                        {
                            qualificationName = await _unitOfwork.genericRepository<QualificationsLookup>()
                                .GetByCondition(q => q.Id == applicant.QualificationId)
                                .AsNoTracking()
                                .Select(q => q.Name)
                                .FirstOrDefaultAsync(ct);
                        }

                        string? qualificationCountryName = null;
                        if (applicant.QualificationCountryId > 0)
                        {
                            qualificationCountryName = await _unitOfwork.genericRepository<CountriesLookup>()
                                .GetByCondition(c => c.Id == applicant.QualificationCountryId)
                                .AsNoTracking()
                                .Select(c => c.Name)
                                .FirstOrDefaultAsync(ct);
                        }

                        // Map once to PersonVM, then set derived names
                        var applicantVm = _mapper.Map<PersonVM>(applicant);
                        applicantVm.NationaliyName = nationalityName;
                        applicantVm.QualificationName = qualificationName;
                        applicantVm.QualificationCountry = qualificationCountryName;

                        // Assign ONCE; do not overwrite later
                        vm.Applicant = applicantVm;
                    }
                    else
                    {
                        vm.Applicant = null;
                    }
                }
                else
                {
                    vm.Applicant = null;
                }

                // 4) Partners -> PartnerVM list
                var partners = await _unitOfwork.genericRepository<Partner>()
                    .GetByCondition(x => x.LicenseId == vm.LicId)
                    .AsNoTracking()
                    .ToListAsync(ct);
                vm.partnerVM = _mapper.Map<List<Partner>, List<PartnerVM>>(partners ?? new List<Partner>());

                // 5) Ending reasons (entities -> VM)
                var reasonEntities = await _unitOfwork
                    .genericRepository<MoiEserviceLicEndingReason>()
                    .GetAllAsync();

                var reasonsList = _mapper.Map<List<MoiEserviceLicEndingReasonVM>>(
                    reasonEntities ?? Enumerable.Empty<MoiEserviceLicEndingReason>());
                vm.moiEserviceLicEndingReasonVM = reasonsList;


                var countryEntities = await _unitOfwork
    .genericRepository<CountriesLookup>()
    .GetAllAsync();

                var countries = _mapper.Map<List<CountriesLookupVM>>(
                    countryEntities ?? Enumerable.Empty<CountriesLookup>());


                var QualificationEntities = await _unitOfwork
    .genericRepository<QualificationsLookup>()
    .GetAllAsync();

                var Qualification = _mapper.Map<List<QualificationsLookupVM>>(
                    QualificationEntities ?? Enumerable.Empty<QualificationsLookup>());


                var ChangeAddessFees = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                                .GetByCondition(c => c.ActvityTypeId == vm.ActiivityTypeId && c.ServiceId ==(int)ServiceEnum.publishing && c.TransTypeId == 4)
                                .AsNoTracking()
                                .Select(c => c.FixedFees)
                                .FirstOrDefaultAsync(ct);
                var RenewFees = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                                .GetByCondition(c => c.ActvityTypeId == vm.ActiivityTypeId && c.ServiceId ==(int)ServiceEnum.publishing && c.TransTypeId == 17)
                                .AsNoTracking()
                                .Select(c => c.FixedFees)
                                .FirstOrDefaultAsync(ct);
                var ReplaceOfLostFees = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                                .GetByCondition(c => c.ActvityTypeId == vm.ActiivityTypeId && c.ServiceId ==(int)ServiceEnum.publishing && c.TransTypeId == 12)
                                .AsNoTracking()
                                .Select(c => c.FixedFees)
                                .FirstOrDefaultAsync(ct);

                var ChangePartnerFees = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                                .GetByCondition(c => c.ActvityTypeId == vm.ActiivityTypeId && c.ServiceId ==(int)ServiceEnum.publishing && c.TransTypeId == 3)
                                .AsNoTracking()
                                .Select(c => c.FixedFees)
                                .FirstOrDefaultAsync(ct);

                var changeCompanyNameFess = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                                .GetByCondition(c => c.ActvityTypeId == vm.ActiivityTypeId && c.ServiceId ==(int)ServiceEnum.publishing && c.TransTypeId == 1)
                                .AsNoTracking()
                                .Select(c => c.FixedFees)
                                .FirstOrDefaultAsync(ct);

                var ChangeCommercialNameFees = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                                .GetByCondition(c => c.ActvityTypeId == vm.ActiivityTypeId && c.ServiceId ==(int)ServiceEnum.publishing && c.TransTypeId == 2)
                                .AsNoTracking()
                                .Select(c => c.FixedFees)
                                .FirstOrDefaultAsync(ct);


                var ChangeActivityFees = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                                .GetByCondition(c => c.ActvityTypeId == vm.ActiivityTypeId && c.ServiceId ==(int)ServiceEnum.publishing && c.TransTypeId == 2)
                                .AsNoTracking()
                                .Select(c => c.FixedFees)
                                .FirstOrDefaultAsync(ct);

                var ChangeManagerFees = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                                .GetByCondition(c => c.ActvityTypeId == vm.ActiivityTypeId && c.ServiceId ==(int)ServiceEnum.publishing && c.TransTypeId == 9)
                                .AsNoTracking()
                                .Select(c => c.FixedFees)
                                .FirstOrDefaultAsync(ct);
                var RenouncementFees = await _unitOfwork.genericRepository<MoiEserviceLicenseInfo>()
                               .GetByCondition(c => c.ActvityTypeId == vm.ActiivityTypeId && c.ServiceId ==(int)ServiceEnum.publishing && c.TransTypeId == 18)
                               .AsNoTracking()
                               .Select(c => c.FixedFees)
                               .FirstOrDefaultAsync(ct);
                RequestFessVM requestFessVM = new RequestFessVM
                {
                    ChangeActivityFees = ChangeActivityFees,
                    ChangeAddessFees = ChangeAddessFees,
                    ChangeCommercialNameFees = ChangeCommercialNameFees,
                    changeCompanyNameFess = changeCompanyNameFess,
                    ChangeManagerFees = ChangeManagerFees,
                    ChangePartnerFees = ChangePartnerFees,
                    RenewFees = RenewFees,
                    RenouncementFees = RenouncementFees,
                    ReplaceOfLostFees = ReplaceOfLostFees,
                };

                // 6) Map company/address to their VMs last
                vm.Company = company != null ? _mapper.Map<Company, CompanyVM>(company) : null;
                vm.AddressVM = address != null ? _mapper.Map<Address, AddressVM>(address) : null;
                vm.countriesLookupVM = countries;
                vm.qualificationsLookupVM = Qualification;
                vm.RequestFessVM = requestFessVM;
                return Ok(vm);
            }
            catch (OperationCanceledException)
            {
                // Client canceled the request
                return StatusCode(StatusCodes.Status499ClientClosedRequest);
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error in GetLicenseDetails({Id})", id);
                return Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Internal Server Error");
            }
        }



        [HttpGet]
        [Route("GetRequests")]
        public async Task<IEnumerable<object>> GetRequestsWithLicNoAndStatus(int serviceId)
        {
            try
            {
                // Fetch all requests from the repository
                var requests = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>()
                    .GetAll();
                var requestsType = await _unitOfwork.genericRepository<RequestsTypesLookup>()
                   .GetAll();
                var requestsStatus = await _unitOfwork.genericRepository<RequestStatusLookup>()
                   .GetAll();
                // Project data including LicNo and StatusName
                var requestsWithDetails = requests
                    .Where(r => r.ServiceId ==(int)ServiceEnum.publishing)
                    .Select(r => new
                    {
                        r.RequestId,
                        r.Reqno,
                        r.ServiceId,
                        r.LicenseId,
                        r.ReqtypeId,
                        LicNo = r.LicenceNavigation?.LicNo,
                        ReqStatusName = r.RequestStatusNavigation?.NameAr,
                        Licname = r.Licname,
                        Licreqtime = r.Licreqtime,
                        ReqTypeName = requestsType
            .FirstOrDefault(t => t.Id == r.ReqtypeId)?.NameAr,
                        r.RequestStatusId,
                        r.Licpaystatus,
                        r.IsTradeApprovalLetter,
                        r.IsRenewTradeApprovalLetter,
                        r.LicrequestIsDeleted

                    })
                    .OrderByDescending(c => c.RequestId).ToList();

                return requestsWithDetails;
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an empty result
                return Enumerable.Empty<object>();
            }
        }


        [HttpGet]
        [Route("GetRequestById")]
        public async Task<IActionResult> GetRequestById(int id, int serviceId)
        {
            try
            {
                // Fetch the main request
                var spec = new RequestWithSpecificService(id, 5, false);
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

                var licencesMapped = new LicencesVM();
                //var userData = new AspNetUserVM();

                var userData = await FetchUserDataAsync(requestMapped.Requesterid);
                if (requestMapped.ReqtypeId == (int)RequestTypeEnum.ChangeData)
                {
                    var transactions = await FetchTransactionsAsync(requestMapped.RequestId, serviceId);
                    requestMapped.Transactions = transactions;
                }
                var attachMapped = await FetchAttachmentsAsync(requestMapped.RequestId, serviceId);
                var employeeLogMapped = await FetchEmployeeLogAsync(requestMapped.RequestId);
                //if (requestMapped.ReqtypeId == (int)RequestTypeEnum.ChangeData)
                //{
                //    var transactions = await FetchTransactionsAsync(requestMapped.RequestId, serviceId);
                //    requestMapped.Transactions = transactions;
                //}

                // Return consolidated result
                var paymentMapped = await FetchPaymentAsync(requestMapped.RequestId);
                var result = new RequestDetailsVM
                {
                    RequestDVM = requestMapped,
                    RequestTransactionVM = employeeLogMapped,
                    AspNetUserVM = userData,
                    //PersonApplicantVM = applicantMapped,
                    //PreApprovementVM = licencepreApprovementMapped,
                    //ManagerPersonVM = managerMapped,
                    attachmentVM = attachMapped,
                    //CompanyVM = companyMapped,
                    //RequestStatusVM = requestStatusMapped,
                    //IsFinalStatus = isFinalStatus,
                    //requestStatus = requestStatus,
                    //LicenceRenewVM = licencesRenew,
                    PaymentDetailsVM = paymentMapped,
                    LicencesVM = licencesMapped
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



        [HttpPost]
        [Route("UpdateRequest")]
        public async Task<bool> UpdateRequest([FromBody] UpdateRequestInput model)
        {
            try
            {
                // Begin transaction
                await _unitOfwork.BeginTransactionAsync();

                // --- Prepare repositories (single access point) ---
                var requestRepo = _unitOfwork.genericRepository<MoiEserviceLicensesRequest>();
                var workFlowRepo = _unitOfwork.genericRepository<WorkFlow>();
                var userRepo = _unitOfwork.genericRepository<MoiEserviceSysUser>();
                var typeLookupRepo = _unitOfwork.genericRepository<RequestsTypesLookup>();
                var requestTransactionRepo = _unitOfwork.genericRepository<RequestTransaction>();
                var domainTransactionRepo = _unitOfwork.genericRepository<Domain.Entities.Transaction>();
                var licenseRenewRepo = _unitOfwork.genericRepository<LicenseRenew>();
                var licenceRepo = _unitOfwork.genericRepository<Licence>();

                // 1) Get main request
                var request = await requestRepo
                    .GetByCondition(c => c.RequestId == model.RequestID)
                    .FirstOrDefaultAsync();

                if (request == null)
                {
                    // No request found for given RequestID
                    await _unitOfwork.RollbackTransactionAsync();
                    return false;
                }

                // 2) Only handle renew requests (ReqtypeId == 2)
                if (request.ReqtypeId == 2)
                {
                    // 2.1) Get workflow phase for current status
                    var phase = await workFlowRepo
                        .GetByCondition(c =>
                            c.ServiceId == (int)ServiceEnum.Mosanafat &&
                            c.RequestTypeId == request.ReqtypeId &&
                            c.CurrentStatusId == request.RequestStatusId)
                        .FirstOrDefaultAsync();

                    // 2.2) Get current user
                    var user = await userRepo
                        .GetByCondition(c => c.CivilId == model.CivilID)
                        .FirstOrDefaultAsync();

                    // 2.3) Get request type lookup
                    var typeLookup = await typeLookupRepo
                        .GetByCondition(c => c.Id == request.ReqtypeId)
                        .FirstOrDefaultAsync();

                    if (phase == null || user == null || typeLookup == null)
                    {
                        // Missing required data (workflow phase / user / type)
                        await _unitOfwork.RollbackTransactionAsync();
                        return false;
                    }

                    // 3) Update main request (status + notes)
                    request.RequestStatusId = phase.NextStatusId;
                    request.RequestNote = model.Notes;

                    requestRepo.Update(request);

                    // 4) Create RequestTransaction row
                    var requestTransaction = new RequestTransaction
                    {
                        RequestId = request.RequestId,
                        CivilIdUser = model.CivilID,
                        CreatedBy = user.Name,
                        CreatedDate = DateTime.Now,
                        ReqStatusId = phase.NextStatusId,
                        Notes = model.Notes,
                        ReqTypeId = request.ReqtypeId,
                        Status = typeLookup.NameAr,
                        ServiceId = (int)ServiceEnum.Mosanafat,
                        UpdatedDate = DateTime.UtcNow
                    };

                    await requestTransactionRepo.Create(requestTransaction);

                    // 5) If phase is final: update licence expiry from LicenseRenew
                    if (phase.FlagRequestStatus == "final")
                    {
                        // Get transaction for this request
                        var dbTransaction = await domainTransactionRepo
                            .GetByCondition(c => c.RequestId == model.RequestID)
                            .FirstOrDefaultAsync();

                        if (dbTransaction != null)
                        {
                            // Get related renew record
                            var licenseRenew = await licenseRenewRepo
                                .GetByCondition(c => c.ReqTransId == dbTransaction.Id)
                                .FirstOrDefaultAsync();

                            if (licenseRenew != null)
                            {
                                // Get licence and update expiry date
                                var licence = await licenceRepo
                                    .GetByCondition(c => c.LicId == licenseRenew.LicenseId)
                                    .FirstOrDefaultAsync();

                                if (licence != null)
                                {
                                    licence.ExpireDate = licenseRenew.NewExpiryDate;
                                    licenceRepo.Update(licence);
                                }
                            }
                        }
                    }

                    // Save changes for renew flow
                    await _unitOfwork.Complete();
                }

                // Commit transaction
                await _unitOfwork.CommitTransactionAsync();
                return true;
            }
            catch (Exception)
            {
                // Rollback on any error
                await _unitOfwork.RollbackTransactionAsync();
                throw;
            }
        }




        private async Task<AspnetUserVM> FetchUserDataAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return null;

            // Retrieve a single user record based on userId
            var user = await _unitOfwork
                .genericRepository<AspNetUser>().GetByCondition(r => r.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null)
                return null;

            // Retrieve the corresponding person record based on CivilId
            var person = await _unitOfwork
                .genericRepository<Person>().GetByCondition(r => r.CivilId == user.CivilId && r.NationalityId != null)
                .FirstOrDefaultAsync();

            // Map the data to AspNetUserVM
            var userVM = new AspnetUserVM
            {
                CivilId = user.CivilId,
                Id = user.Id,
                FullNameAr = user.FullNameAr,
                Email = user.Email,
                Mobile = user.Mobile,
                // Determine nationality name based on person data
                NationalityName = person != null
                    ? (person.NationalityId == 156 || person.NationalityId == 157 ? "كويتي" : "غير كويتي")
                    : "غير معرف" // If no matching person found
            };

            return userVM;
        }




        private async Task<IEnumerable<AttachVM>> FetchAttachmentsAsync(long requestId, int serviceId)
        {
            var spec = new AttachmentWithSpec(requestId, serviceId);
            var attachments = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>().GetTableWithSpec(spec);
            return _mapper.Map<IEnumerable<MoiEserviceRequestsAttach>, IEnumerable<AttachVM>>(attachments);
        }

        private async Task<List<TransactionVM>> FetchTransactionsAsync(long requestId, int serviceId)
        {
            var spec = new TransactionWithSpec(requestId, serviceId);
            var transactions = await _unitOfwork.genericRepository<Domain.Entities.Transaction>().GetTableWithSpec(spec);

            var transactionVMs = new List<TransactionVM>();

            foreach (var transaction in transactions)
            {
                var transactionVM = _mapper.Map<Domain.Entities.Transaction, TransactionVM>(transaction);

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
                else if (transaction.TransTypeId == (int)TransactionTypesEnum.ReplacementOfLost)
                {

                    var replacementDetails = await _unitOfwork.genericRepository<ReplacementOfLostTransaction>().GetByIdObject(r => r.ReqTransactionId == transaction.Id);
                    transactionVM.ReplacementOfLostTransVM = _mapper.Map<ReplacementOfLostTransaction, ReplacementOfLostTransVM>(replacementDetails);
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
                        NewStatusName = transaction.NewStatusName,
                        OldStatusName = transaction.OldStatusName,
                        RequestId = transaction.RequestId,
                        EmployeeId = transaction.EmployeeId,
                        OperationDate = transaction.OperationDate,
                        EmployeeCivilId = transaction.EmployeeCivilId,
                        Notes = transaction.Notes,
                        EmployeeName = user.Name
                    }).ToList();
        }

        private async Task<PaymentDetailsVM> FetchPaymentAsync(long? requestID)
        {
            try
            {
                var transactions = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>()
                .GetByCondition(r => r.RequestId == requestID)
                .FirstOrDefaultAsync();
                return _mapper.Map<MoiEserviceRequestPaymentDetail, PaymentDetailsVM>(transactions);
            }
            catch (Exception ex)
            {

                return null;
            }

        }




    }


}
