using Business.ViewModel.Channels;
using Business.ViewModel.Dynamic;
using Business.ViewModel.HomePage;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class MoiEserviceLicensesRequestVM
    {
        public long RequestId { get; set; }

        public string? Reqno { get; set; }
        public string? LicenTypeName { get; set; }
        public int? testablishContract { get; set; }

        public int? ReqtypeId { get; set; }

        public bool? OwnerSameManager { get; set; }

        public DateTime? RequestModDate { get; set; }

        public string? Licno { get; set; }

        public string? ActivityType { get; set; }

        public string? Licowner { get; set; }

        public string? Licname { get; set; }

        public DateTime? Licexpiredate { get; set; }

        public DateTime? Licreqtime { get; set; }

        public string? Requesterid { get; set; }

        public string? RequestNote { get; set; }

        public int? RequestStatusId { get; set; }

        public string? RequestAttach { get; set; }

        public int? LicenseId { get; set; }

        public decimal? Licamount { get; set; }

        public string? Licpaystatus { get; set; }

        public int? CategoryId { get; set; }

        public int? SectorId { get; set; }

        public int? ActivityTypeId { get; set; }

        public string? CompletionDocs { get; set; }

        public string? ActivityCode { get; set; }

        public bool? IsTradeApprovalLetter { get; set; }

        public bool? IsRenewTradeApprovalLetter { get; set; }

        public bool? LicrequestIsDeleted { get; set; }

        public int? CompanyId { get; set; }

        public int? ManagerId { get; set; }

        public int? ServiceId { get; set; }

        public string? CentralNoMoci { get; set; }
        public DateTime? MociBookDate { get; set; }


        public bool? IsArchived { get; set; }

        public int? LicTypeId { get; set; }

        public string? MediaName { get; set; }

        public int? AddressIdMocI { get; set; }

        public int? BuildingId { get; set; }

        public string? AppCivilId { get; set; }

        public string? ManCivilId { get; set; }

        public string? UserCivilId { get; set; }

        public int? LicStatusId { get; set; }

        public int? LicNationality { get; set; }
        public string? LicLanguage { get; set; }


        public string? LicLocation { get; set; }

        public List<CountriesLookupVM>? countriesLookups { get; set; }

        public List<AttachRuleVM>? attachRules { get; set; }
        public List<ChannelsPersonsVM>?  channelsPersons { get; set; }
        public List<AttachVM>? attach { get; set; }
        public LicenceDetailsVM? licence { get; set; }
        public PersonVM? person { get; set; }

        public PersonVM? manager { get; set; }

        public  AddressVM ? address { get; set; }
        public List<PesronTypeLookUpVM>? pesronTypeLookUp { get; set; }
        public List<TestablishContractVM>? testablishContracts { get; set; }
        public List<QualificationsLookupVM>? qualificationsLookups { get; set; }
        public List<EserviceActvityTypeModel>? eserviceActvityTypes { get; set; }
        public List<ActivityTypeVM>? ActivityTypes { get; set; }
        public List<PartnerVM>? Partners { get; set; }
        public List<LicenceTypesLookupVM>? licenceTypesLookupVMs { get; set; }
        public CompanyVM? Company { get; set; }

        public List<ScheduleReleaseTypesVM>? scheduleReleaseTypesVMs { get; set; }



        [JsonIgnore]

        public List<IFormFile> UploadedFiles { get; set; } = new();
        // =======================
        // Added minimal outputs:
        // =======================
        // Collect all created request IDs within the same API call
        public List<long> CreatedRequestIds { get; set; } = new();

        // Collect all created transaction IDs within the same API call
        public List<int> CreatedTransactionIds { get; set; } = new();
        public List<TxLinkVM> RequestLinks { get; set; } = new();

        // جديد: قواميس اختصار للوصول مباشرة حسب النوع
        public Dictionary<int, long> RequestIdsByTypeId { get; set; } = new();
        public Dictionary<int, int> TransactionIdsByTypeId { get; set; } = new();

    }
}
