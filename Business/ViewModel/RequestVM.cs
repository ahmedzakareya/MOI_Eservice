using Business.ViewModel.ClassificationVM;
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
    public class RequestVM:BaseEntity
    {
        public long RequestId { get; set; }

        public string? Reqno { get; set; }
        public string? ServiceName { get; set; }
        public string? SessionCivilId { get; set; }
        public string? SessionFullNaame { get; set; }

        public int? ReqtypeId { get; set; }
        [DisplayName("المدير المسؤول نفس المالك")]
        public bool OwnerSameManager { get; set; }

        public DateTime? RequestModDate { get; set; }

        public string? Licno { get; set; }
        public int? PreApprovalId { get; set; }
        public string? ActivityType { get; set; }
        [DisplayName("صاحب الترخيص")]

        public string? Licowner { get; set; }
       

        public string? Licname { get; set; }

        public DateTime? Licexpiredate { get; set; }
        public DateTime? LicIssuedate { get; set; }
        public string? PreApprovalNo { get; set; }

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
        public int? AppId { get; set; }


        public int? ServiceId { get; set; }

    

        public bool? IsArchived { get; set; }

        public int? LicTypeId { get; set; }

        public string? MediaName { get; set; }

        public int? AddressIdMocI { get; set; }

        public int? BuildingId { get; set; }

        public string? AppCivilId { get; set; }
        public long? SequenceNo { get; set; }
        public string? ManCivilId { get; set; }

        public string? UserCivilId { get; set; }

        public int? LicStatusId { get; set; }
        public string? RequesterCivilId { get;set; }
        public string? ActivityName { get; set; }
        public string? LicTypeName {  get; set; }
        public string? ReqTypeName {  get; set; }   
        public string? ReqStatusName { get; set; }
        public string? SalesManagerCivilId { get; set; }
        public string? MarketingManagerCivilId { get; set; }
        public string? OperationsManagerCivilId { get; set; }
        public int? SalesManagerId { get; set; }
        public int? MarketingManagerId { get; set; }
        public int? OperationsManagerId { get; set; }

        [ForeignKey("SalesManagerId")]
        public virtual PersonVM? SalesManager { get; set; }
        [ForeignKey("MarketingManagerId")]
        public virtual PersonVM? MarketingManager { get; set; }
        [ForeignKey("OperationsManagerId")]
        public virtual PersonVM? OperationsManager { get; set; }
        //public List<TourClassBranchLookUp>? ClassificationData { get; set; }
        public List<AttachVM>? attachVMs { get; set; }
        [ForeignKey("BuildingId")]

        public virtual CompanyVM? Building { get; set; }

        [ForeignKey("CompanyId")]

        public virtual CompanyVM? company { get; set; }

        [ForeignKey("ManagerId")]
        public virtual PersonVM? Manager { get; set; }
        [ForeignKey("AppId")]
        public virtual PersonVM? ApplicantPerson { get; set; }

        [ForeignKey("ActivityTypeId")]

        public virtual ActivityTypeVM? ActivityTypeNavigation { get; set; }
        [ForeignKey("LicTypeId")]
        public virtual LicencesTypeVM? LicenceTypeNavigation { get; set; }
        [ForeignKey("RequestStatusId")]
        public virtual RequestStatusVM? RequestStatusNavigation { get; set; }
        [ForeignKey("LicenseId")]
        public virtual LicencesVM LicenceNavigation { get; set; }
        [ForeignKey("ReqtypeId")]
        public virtual RequestTypeVM? RequestsTypesNavigation { get; set; }

        public virtual IEnumerable<TransactionVM> Transactions { get; set; } = new List<TransactionVM>();

        [ForeignKey("PreApprovalId")]
        public virtual PreApprovementVM LicencePreApprovNavigation { get; set; }


        // 
        public WorkFlowVM? WorkFlowVM { get; set; }



    }
}
