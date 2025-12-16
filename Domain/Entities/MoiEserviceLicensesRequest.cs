using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class MoiEserviceLicensesRequest
{
    public long RequestId { get; set; }

    public string? Reqno { get; set; }

    public int? ReqtypeId { get; set; }

    public bool? OwnerSameManager { get; set; }

    public DateTime? RequestModDate { get; set; }
    public string? RequesterCivilId { get; set; }   
    public string? Licno { get; set; }
    public  int? PreApprovalId { get; set; } 
    public string? ActivityType { get; set; }

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
    public string? SalesManagerCivilId { get; set; }
    public string? MarketingManagerCivilId { get; set; }
    public string? OperationsManagerCivilId { get; set; }
    public int? SalesManagerId { get; set; }
    public int? MarketingManagerId { get; set; }
    public int? OperationsManagerId { get; set; }

    public long? SequenceNo { get; set; }
    public string? ManCivilId { get; set; }

    public string? MandoobCivilId { get; set; }

    public int? LicStatusId { get; set; }
    //public List<TourClassBranchLookUp>? ClassificationData { get; set; }
    public List<MoiEserviceRequestsAttach>? attachVMs { get; set; }
    [ForeignKey("BuildingId")]

    public virtual Company? Building { get; set; }

    [ForeignKey("CompanyId")]

    public virtual Company? company { get; set; }

    [ForeignKey("ManagerId")]
    public virtual Person? Manager { get; set; }
    [ForeignKey("SalesManagerId")]
    public virtual Person? SalesManager { get; set; }
    [ForeignKey("MarketingManagerId")]
    public virtual Person? MarketingManager { get; set; }
    [ForeignKey("OperationsManagerId")]
    public virtual Person? OperationsManager { get; set; }
    [ForeignKey("AppId")]
    public virtual Person? ApplicantPerson { get; set; }

    [ForeignKey("ActivityTypeId")]

    public virtual ActivityTypesLookup? ActivityTypeNavigation { get; set; }
    [ForeignKey("LicTypeId")]
    public virtual LicenceTypesLookup? LicenceTypeNavigation { get; set; }
    [ForeignKey("RequestStatusId")]
    public virtual RequestStatusLookup? RequestStatusNavigation { get; set; }
    [ForeignKey("LicenseId")]
    public virtual Licence LicenceNavigation { get; set; }
    [ForeignKey("ReqtypeId")]
    public virtual RequestsTypesLookup? RequestsTypesNavigation { get; set; }

    public virtual IEnumerable<Transaction> Transactions { get; set; } = new List<Transaction>();

    [ForeignKey("PreApprovalId")]
    public virtual MoiPreApprovement LicencePreApprovNavigation { get; set; }


}
