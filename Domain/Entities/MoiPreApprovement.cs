using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class MoiPreApprovement
{
    public int PreAppId { get; set; }

    public int? BuildingId { get; set; }
    public DateTime? ConsumedDate { get; set; }  
    public int? CompanyId { get; set; }
    public string? Flag { get; set; } 
    public int? ManagerId { get; set; }

    public int? AppId { get; set; }
    public string? MandoobId { get; set; }


    public long? RequestId { get; set; }

    public int? LicTypeId { get; set; }
    public int? LinkedLicenseId { get; set; }
    public bool IsConsumed { get; set; }    
    public string? ClassificationName { get; set; }

    public DateTime? ClassificationDate { get; set; }

  

    public DateTime? ComIssuingDate { get; set; }

    public DateTime? ComExpiryDate { get; set; }

    public int? ActivityTypeId { get; set; }

    public int? ReqStatusId { get; set; }

    public string? LicenseName { get; set; }

    public string? LicenseNo { get; set; }

    public DateTime? LicenseIssueDate { get; set; }

    public DateTime? LicenseExpireDate { get; set; }
    public string? SalesManagerCivilId { get; set; }
    public string? MarketingManagerCivilId { get; set; }
    public string? OperationsManagerCivilId { get; set; }
    public int? SalesManagerId { get; set; }
    public int? MarketingManagerId { get; set; }
    public int? OperationsManagerId { get; set; }
    public int? ReqTypeId { get; set; }

    public int? ClassificationId { get; set; }

    public string? ApplicantCivilId { get; set; }
    public string? CommercialLicNo { get; set; }
    public string? RecordNo { get; set; }

    public long? SequenceNo { get; set; }
    public string? ManagerCivilId { get; set; }

    public string? MandoobCivilId { get; set; }

    public int? LicStatusId { get; set; }
    [ForeignKey("SalesManagerId")]
    public virtual Person? SalesManager { get; set; }
    [ForeignKey("MarketingManagerId")]
    public virtual Person? MarketingManager { get; set; }
    [ForeignKey("OperationsManagerId")]
    public virtual Person? OperationsManager { get; set; }
    [ForeignKey("RequestId")]
    public virtual MoiEserviceLicensesRequest? Request { get; set; }
    [ForeignKey("ManagerId")]
    public virtual Person Manager { get; set; }
    [ForeignKey("AppId")]
    public virtual Person Applicant { get; set; }
    [ForeignKey("MandoobId")]
    public virtual AspNetUser Mandoob { get; set; }

    [ForeignKey("CompanyId")]
    public virtual Company Company { get; set; }
    [ForeignKey("BuildingId")]
    public virtual Company Building { get; set; }
    [ForeignKey("LicTypeId")]
    public virtual LicenceTypesLookup LicenceTypesLookup { get; set; }
    [ForeignKey("ActivityTypeId")]
    public virtual ActivityTypesLookup ActivityTypesLookup { get; set; }
    [ForeignKey("ReqStatusId")]
    public virtual RequestStatusLookup RequestStatusLookup { get; set; }
    [ForeignKey("LicStatusId")]
    public virtual LicenseStatusLookup LicenseStatusLookup { get; set; }


}
