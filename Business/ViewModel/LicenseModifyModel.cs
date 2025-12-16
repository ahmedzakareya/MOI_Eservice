using Business.ViewModel.Dynamic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    using System.Text.Json.Serialization;

    public class LicenseModifyModel
    {
        public int LiceID { get; set; }
        public string? LicNo { get; set; }
        public string? Name { get; set; }

        public IDictionary<int, decimal> FeesByTypeId { get; set; } = new Dictionary<int, decimal>();
        public List<TransactionTypesLookupVM>? transactionVm { get; set; }
        public List<int> TypeIds { get; set; } = new();

        public LicencesVM? LicenceDetailsVM { get; set; }
        public MoiEserviceLicensesRequestVM? moiEserviceLicensesRequestVM { get; set; }

        // تغييرات مختلفة
        public CompanyNameChangeTransaction? CompanyNameChangeTransactionVM { get; set; }
        public CommercialNameChangeTransaction? CommercialNameChangeTransactionVm { get; set; }
        public AddressChangeTransaction? AddressChangeTransactionVM { get; set; }
        public ActivityChangeTransVM? ActivityChangeTransVM { get; set; }
        public ChangeManagerTransVM? ChangeManagerTransVM { get; set; }
        public RenouncementTransactionVM? renouncementTransactionVM { get; set; }

        // دخول/خروج الشركاء
        [JsonPropertyName("ChangeOldPartnerTransVM")]
        public List<ChangeOldPartnerTransVM>? ChangeOldPartnerTransVM { get; set; }

        [JsonPropertyName("changeNewPartnerTransVM")]
        public List<ChangeNewPartnerTransVM>? changeNewPartnerTransVM { get; set; }

        public List<ActivityTypeVM>? activityTypeVMs { get; set; }
        public AddressVM? AddressVM { get; set; }
        public CompanyVM? companyVM { get; set; }
        public PersonVM? Manager { get; set; }
        public List<PartnerVM>? partnerVM { get; set; }
        public TransactionVM? transactions { get; set; }
        public List<AttachRuleVM>? attachRules { get; set; }
        public List<MoiEserviceLicEndingReasonVM>? ReasonsVM { get; set; }

        public List<CountriesLookupVM>? countriesLookupVM { get; set; }
        public List<QualificationsLookupVM>? qualificationsLookupVM { get; set; }
        public PersonVM? Applicant { get; set; }
        public RequestFessVM? RequestFessVM { get; set; }
        public int? ReasonID { get; set; }

        // Outputs to capture created IDs (from API response)
        public List<long> CreatedRequestIds { get; set; } = new();
        public List<int> CreatedTransactionIds { get; set; } = new();

        // Server-only attachments list used AFTER JSON save (we fill it on the server)
        [JsonIgnore]
        public List<AttachVM> attach { get; set; } = new();

        // ===== NEW: Per-transaction mapping (filled by server after save) =====
        public List<TxLinkVM> RequestLinks { get; set; } = new();                 // detailed list
        public Dictionary<int, long> RequestIdsByTypeId { get; set; } = new();    // typeId -> requestId
        public Dictionary<int, int> TransactionIdsByTypeId { get; set; } = new(); // typeId -> transactionId
    }


    public class TxLinkVM
    {
        public int TransactionTypeId { get; set; }
        public long RequestId { get; set; }
        public int TransactionId { get; set; }
    }


}
