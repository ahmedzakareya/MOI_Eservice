using Business.ViewModel;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IDataFetchService
    {
        Task<PersonVM> FetchManagerDataAsync(string? managerCivilId, int serviceId);
        Task<MoiEserviceLicensesRequest> GetRequest(int requestId);
        Task<PersonVM> FetchApplicantDataAsync(string? CivilId, int? serviceId);
         Task<AspnetUserVM> FetchMandoobDataAsync(int LicId);
        Task<IEnumerable<AttachVM>> FetchAttachmentsAsync(long requestId, int serviceId);
        Task<PaymentDetailsVM> FetchPaymentsAsync(long requestId, int serviceId);
        Task<IEnumerable<RequestStatusVM>> FetchRequestStatusAsync();
        Task<IEnumerable<RequestTransactionVM>> FetchEmployeeLogAsync(long requestId);
        Task<CompanyVM> FetchCompanyDataAsync(int? companyId, int serviceId);
        Task<AddressVM> FetchAddressForCompanyDataAsync(int? AddresssId, int serviceId);
        Task<IEnumerable<SocialMediaVM>> FetchSocialMediaDataAsync(long requestId);
        Task<IEnumerable<PartnerVM>> FetchPartnerDataAsync(int? licenseId, int serviceId);
        Task<List<TransactionVM>> FetchTransactionsAsync(long requestId, int serviceId);
    }
}
