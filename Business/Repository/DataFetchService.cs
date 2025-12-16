using AutoMapper;
using Business.Enums;
using Business.Interfaces;
using Business.ModelWithSpecification;
using Business.ViewModel;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class DataFetchService : IDataFetchService
    {
        private readonly IUnitOfwork _unitOfwork;
        private readonly IMapper _mapper;

        public DataFetchService(IUnitOfwork unitOfwork, IMapper mapper)
        {
            _unitOfwork = unitOfwork;
            _mapper = mapper;
        }
        public async Task<MoiEserviceLicensesRequest> GetRequest(int requestId)
        {
            var spec = new RequestWithSpecificService(requestId, true);
            return await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().GetByIdWithSpec(spec);
        }
        public async Task<PersonVM> FetchManagerDataAsync(string? managerCivilId, int serviceId)
        {
            if (managerCivilId == null) return null;

            var spec = new ManagerApplicantWithSpec(managerCivilId.ToString(), serviceId);
            var manager = await _unitOfwork.genericRepository<Person>().GetByIdWithSpec(spec);
            return _mapper.Map<Person, PersonVM>(manager);
        }

        public async Task<PersonVM> FetchApplicantDataAsync(string? CivilId, int? serviceId)
        {
            if (CivilId == null) return null;

            //var spec = new ManagerApplicantWithSpec(CivilId, serviceId ?? 0);
            var applicant = await _unitOfwork.genericRepository<Person>().GetByCondition(x=>x.CivilId==CivilId).FirstOrDefaultAsync();
            return _mapper.Map<Person, PersonVM>(applicant);
        }
        public async Task<AspnetUserVM> FetchMandoobDataAsync(int LicId)
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
        public async Task<IEnumerable<AttachVM>> FetchAttachmentsAsync(long requestId, int serviceId)
        {
            //var spec = new AttachmentWithSpec(requestId, serviceId);
            // var attachments = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>().GetTableWithSpec(spec);
            var attachments = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>()
                 .GetByCondition(a => a.AttachRequestid == requestId && !(a.IsLatest == false && a.IsApproved == true) && a.ServiceId == serviceId)
                 .ToListAsync();
            return _mapper.Map<IEnumerable<MoiEserviceRequestsAttach>, IEnumerable<AttachVM>>(attachments);
        }

        public async Task<PaymentDetailsVM> FetchPaymentsAsync(long requestId, int serviceId)
        {
            var spec = new PaymentDetailsWithSpec(serviceId, requestId);
            var payments = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>().GetByIdWithSpec(spec);
            return _mapper.Map<MoiEserviceRequestPaymentDetail, PaymentDetailsVM>(payments);
        }
        public async Task<IEnumerable<RequestTransactionVM>> FetchEmployeeLogAsync(long requestId)
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
        public async Task<IEnumerable<RequestStatusVM>> FetchRequestStatusAsync()
        {
            var statuses = await _unitOfwork.genericRepository<RequestStatusLookup>().GetAll();
            return _mapper.Map<IEnumerable<RequestStatusLookup>, IEnumerable<RequestStatusVM>>(statuses);
        }

        public async Task<CompanyVM> FetchCompanyDataAsync(int? companyId, int serviceId)
        {
            if (companyId == null) return null;

            var spec = new CompanyWithSpec(companyId.Value, serviceId);
            var company = await _unitOfwork.genericRepository<Company>().GetByIdWithSpec(spec);
            return _mapper.Map<Company, CompanyVM>(company);
        }

        public async Task<AddressVM> FetchAddressForCompanyDataAsync(int? AddresssId, int serviceId)
        {
            if (AddresssId == null) return null;

            var AddressCompany = await _unitOfwork.genericRepository<Address>().GetbyId(AddresssId);
            return _mapper.Map<Address, AddressVM>(AddressCompany);
        }

        private async Task<LicencesVM> FetchLicenceDataAsync(int licenseId, int serviceId)
        {
            if (licenseId == null) return null;

            var spec = new LicencesWithSpecificService(licenseId, serviceId);
            var licence = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(spec);
            return _mapper.Map<Licence, LicencesVM>(licence);
        }

        public async Task<IEnumerable<SocialMediaVM>> FetchSocialMediaDataAsync(long requestId)
        {
            var social=new SocialWithSpec(requestId,true);
            var spec=await _unitOfwork.genericRepository<MoiSocialMedia>().GetTableWithSpecService(social);

            return _mapper.Map< IEnumerable<MoiSocialMedia>, IEnumerable<SocialMediaVM>>(spec);
        }
        public async Task<IEnumerable<PartnerVM>> FetchPartnerDataAsync(int? licenseId, int serviceId)
        {
            if (licenseId == null) return null;

            var spec = new PartnerWithSpec(licenseId.Value, serviceId);
            var partners = await _unitOfwork.genericRepository<Partner>().GetTableWithSpec(spec);
            return _mapper.Map<IEnumerable<Partner>, IEnumerable<PartnerVM>>(partners);
        }
        public async Task<List<TransactionVM>> FetchTransactionsAsync(long requestId, int serviceId)
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
                //else if (transaction.TransTypeId == (int)TransactionTypesEnum.ReplacementOfLost)
                //{
                //    var replaceSpec = new ReplacementOfLostChangeTransWithSpec(serviceId, requestId);
                //    var replacementDetails = await _unitOfwork.genericRepository<ReplacementOfLostTransaction>().GetByIdWithSpec(replaceSpec);
                //    transactionVM.ReplacementOfLostTransVM = _mapper.Map<ReplacementOfLostTransaction, ReplacementOfLostTransVM>(replacementDetails);
                //}
                else if (transaction.TransTypeId == (int)TransactionTypesEnum.ChangeLicencesName)
                {
                    var licenceNameSpec = new LicencesNameChangeTransWithSpec(serviceId,requestId);
                    var changeLicencesNameDetails = await _unitOfwork.genericRepository<LicencesNameChangeTransaction>().GetByIdWithSpec(licenceNameSpec);
                    transactionVM.LicencesNameChangeTransactionVM = _mapper.Map<LicencesNameChangeTransaction, LicencesNameChangeTransactionVM>(changeLicencesNameDetails);
                }
                else if (transaction.TransTypeId == (int)TransactionTypesEnum.ChangeEmail)
                {
                    var emailSpec = new EmailChangeTransWithSpec(serviceId, requestId);
                    var changeEmailDetails = await _unitOfwork.genericRepository<ChangeEmailTranaction>().GetByIdWithSpec(emailSpec);
                    transactionVM.EmailChangeTransVM = _mapper.Map<ChangeEmailTranaction, EmailChangeTransVM>(changeEmailDetails);
                }
                else if (transaction.TransTypeId == (int)TransactionTypesEnum.ChangeSocialMedia)
                {
                    var socialmedia = new SocialMediaChangeTransWithSpec(requestId);
                    var changeSocialMediaDetails = await _unitOfwork.genericRepository<ChangeSocialMediaTransaction>().GetTableWithSpecService(socialmedia);
                    transactionVM.ChangeSocialMediaTransVM = _mapper.Map<IEnumerable<ChangeSocialMediaTransaction>, IEnumerable<ChangeSocialMediaTransVM>>(changeSocialMediaDetails);
                }
                else if (transaction.TransTypeId == (int)TransactionTypesEnum.ChangePartnerName)
                {
                    var newpartSpec = new PartnerChangeTransNewWithSpec(serviceId, requestId);
                    var newPartnerTransactions = await _unitOfwork.genericRepository<PartnerNewChangeTransaction>()
                                                        .GetTableWithSpec(newpartSpec);
                    var oldpartSpec=new PartnerChangeTransOldWithSpec(serviceId, requestId);
                    var oldPartnerTransactions = await _unitOfwork.genericRepository<PartnerOldChangeTransaction>()
                                                                .GetTableWithSpec(oldpartSpec);


                    // Assign lists of partners to the transaction view model
                    transactionVM.ChangeNewPartnerTransVMs = _mapper.Map<IEnumerable<PartnerNewChangeTransaction>, IEnumerable<ChangeNewPartnerTransVM>>(newPartnerTransactions);
                    transactionVM.ChangeOldPartnerTransVMs = _mapper.Map<IEnumerable<PartnerOldChangeTransaction>, IEnumerable<ChangeOldPartnerTransVM>>(oldPartnerTransactions);
                }
                else if (transaction.TransTypeId == (int)TransactionTypesEnum.ChangeLicencesType)
                {
                    var licenceTpeSpec=new LicencesTypeChangeTransWithSpec(serviceId,requestId);    
                    var changeLicencesTypeDetails = await _unitOfwork.genericRepository<LicenseTypeChangeTransaction>().GetByIdWithSpec(licenceTpeSpec);
                    transactionVM.ChangeLicencesTypeTransVM = _mapper.Map<LicenseTypeChangeTransaction, ChangeLicencesTypeTransVM>(changeLicencesTypeDetails);
                }


                transactionVMs.Add(transactionVM);
            }

            return transactionVMs;
        }


    }
}
