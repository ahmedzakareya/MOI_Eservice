using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class Licence
{
    public int LicId { get; set; }

    public string? LicNo { get; set; }

    public DateTime? IssueDate { get; set; }

    public DateTime? ExpireDate { get; set; }

    public DateTime? FirstCreationDate { get; set; }

    public int? LicStatusId { get; set; }

    public int? LicTypeId { get; set; }

    public string? LicName { get; set; }

    public int? ServiceId { get; set; }

    public string? LastUpdatedUser { get; set; }

    public DateTime? LastUpdatedDate { get; set; }

    public DateTime? LastRenewDate { get; set; }

    public string? Notes { get; set; }

    public string? CommercialLicNo { get; set; }

    public DateTime? ComIssueDate { get; set; }

    public DateTime? ComExpireDate { get; set; }

    public long? SequenceNo { get; set; }

    public int? ApplicantId { get; set; }
    public string? Licowner { get; set; }

    public string? MandoobId { get; set; }

    public int? PreApprovalId { get; set; }
    public int? EstablishingContract { get; set; }

    public bool? ApplicantConvicted { get; set; }

    public int? CompanyId { get; set; }

    public int? BuildingId { get; set; }

    public int? ManagerId { get; set; }

    public int? ParentLicenseId { get; set; }

    public string? PreApprovalNo { get; set; }

    public DateTime? ClassificationDate { get; set; }

    public int? ClassificationId { get; set; }

    public int? ActiivityTypeId { get; set; }
    public string? ActivityCode { get; set; }

    public string? ApplicantCivilId { get; set; }
    public string? MandoobCivilId { get; set; }

    public string? SalesManagerCivilId { get; set; }
    public string? MarketingManagerCivilId { get; set; }
    public string? OperationsManagerCivilId { get; set; }
    public int? SalesManagerId { get; set; }
    public int? MarketingManagerId { get; set; }
    public int? OperationsManagerId { get; set; }
    public string? ManagerCivilId { get; set; }
    public string? Location { get; set; }
    public int? LicenseNationality { get; set; }
    public DateTime? Motdate { get; set; }

    public string? RecordNo { get; set; }
    [ForeignKey("SalesManagerId")]
    public virtual Person? SalesManager { get; set; }
    [ForeignKey("MarketingManagerId")]
    public virtual Person? MarketingManager { get; set; }
    [ForeignKey("OperationsManagerId")]
    public virtual Person? OperationsManager { get; set; }
    [ForeignKey("PreApprovalId")]
    public virtual MoiPreApprovement? PreApprovement { get; set; }

    [ForeignKey("CompanyId")]

    public virtual Company? Company { get; set; }
    [ForeignKey("BuildingId")]
    public virtual Company? Building { get; set; }

    [ForeignKey("ManagerId")]
    public virtual Person? Manager { get; set; }
    [ForeignKey("ApplicantId")]
    public virtual Person? Applicant { get; set; }
    [ForeignKey("MandoobId")]
    public virtual AspNetUser? Mandoob { get; set; }

    [ForeignKey("LicStatusId")]

    public virtual LicenseStatusLookup? LicenseStatusLookup { get; set; }
    [ForeignKey("LicTypeId")]

    public virtual LicenceTypesLookup? LicenceTypesLookup { get; set; }

    [ForeignKey("ActiivityTypeId")]

    public virtual ActivityTypesLookup? ActivityTypesLookup { get; set; }

    [ForeignKey("ClassificationId")]
    public virtual MoiClassification? Classification { get; set; }

}
