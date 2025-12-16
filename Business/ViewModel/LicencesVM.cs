using Business.ViewModel.Dynamic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class LicencesVM
    {
        public int LicId { get; set; }
        [DisplayName("رقم الترخيص")]
        public string? LicNo { get; set; }
        [DisplayName("تاريخ الإصدار")]

        public DateTime? IssueDate { get; set; }
        [DisplayName("تاريخ الإنتهاء")]

        public DateTime? ExpireDate { get; set; }
        public string? ServiceName { get; set; }

        public DateTime? FirstCreationDate { get; set; }

        public int? LicStatusId { get; set; }

        public int? LicTypeId { get; set; }
        [DisplayName("إسم الترخيص")]

        public string? LicName { get; set; }

        public int? ServiceId { get; set; }

        public string? LastUpdatedUser { get; set; }

        public DateTime? LastUpdatedDate { get; set; }

        public DateTime? LastRenewDate { get; set; }

        public string? Notes { get; set; }
        [DisplayName("الترخيص التجاري")]

        public string? CommercialLicNo { get; set; }

        public DateTime? ComIssueDate { get; set; }

        public DateTime? ComExpireDate { get; set; }

        public long? SequenceNo { get; set; }

        public int? ApplicantId { get; set; }
        public string? MandoobId { get; set; }


        public int? EstablishingContract { get; set; }

        public bool? ApplicantConvicted { get; set; }

        public int? CompanyId { get; set; }

        public int? BuildingId { get; set; }

        public int? ManagerId { get; set; }

        public int? ParentLicenseId { get; set; }
        public int PreApprovalId { get; set; }
        [DisplayName("رقم الموافقة المبدئية")]

        public string? PreApprovalNo { get; set; }

        public DateTime? ClassificationDate { get; set; }

        public int? ClassificationId { get; set; }

        public int? ActiivityTypeId { get; set; }
        public string? Licowner { get; set; }

        public string? ApplicantCivilId { get; set; }

        public string? ManagerCivilId { get; set; }
        public string? SalesManagerCivilId { get; set; }
        public string? MarketingManagerCivilId { get; set; }
        public string? OperationsManagerCivilId { get; set; }
        public int? SalesManagerId { get; set; }
        public int? MarketingManagerId { get; set; }
        public int? OperationsManagerId { get; set; }
       

        public DateTime? Motdate { get; set; }

        public string? RecordNo { get; set; }
       
        [DisplayName("نشاط الشركة")]

        public string? ActivityTypeName { get; set; }    
        public string? CompanyName { get; set; }    
        public string? LicStatusName {  get; set; }  
        public string? LicTypeName { get; set; }
       public string? UserCivilId { get; set; }  

        [ForeignKey("CompanyId")]

        public virtual CompanyVM? Company { get; set; }
        [ForeignKey("SalesManagerId")]
        public virtual PersonVM? SalesManager { get; set; }
        [ForeignKey("MarketingManagerId")]
        public virtual PersonVM? MarketingManager { get; set; }
        [ForeignKey("OperationsManagerId")]
        public virtual PersonVM? OperationsManager { get; set; }
        [ForeignKey("BuildingId")]
        public virtual CompanyVM? Building { get; set; }

        [ForeignKey("ManagerId")]
        public virtual PersonVM? Manager { get; set; }
        [ForeignKey("ApplicantId")]
        public virtual PersonVM? Applicant { get; set; }

        [ForeignKey("LicStatusId")]

        public virtual LicencesStatusVM? LicenseStatusLookup { get; set; }
        [ForeignKey("LicTypeId")]

        public virtual LicencesTypeVM? LicenceTypesLookup { get; set; }

        [ForeignKey("ActiivityTypeId")]

        public virtual ActivityTypeVM? ActivityTypesLookup { get; set; }


        [ForeignKey("ClassificationId")]
        public virtual MoiClassification? Classification { get; set; }


        public AddressVM? AddressVM { get; set; }
        public List<PartnerVM>? partnerVM { get; set; }
        public List<MoiEserviceLicEndingReasonVM>? moiEserviceLicEndingReasonVM { get; set; }
        public List<CountriesLookupVM>?   countriesLookupVM { get; set; }
        public List<QualificationsLookupVM>? qualificationsLookupVM { get; set; }

        // تغيير العنوان
        public AddressChangeTransVM?  AddressChangeTransVM { get; set; }

        public RenouncementTransactionVM? renouncementTransactionVM { get; set; }



        public RequestFessVM? RequestFessVM { get; set; }
    }
}
