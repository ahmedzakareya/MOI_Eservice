using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class MoiEserviceLicenseInfo
{
    public int Id { get; set; }

    public int? ActvityTypeId { get; set; }
    public int? TransTypeId { get; set; }
    public int? LicTypeId { get; set; }

    //public int? EserviceTypeId { get; set; }
    public int? ReqTypeId { get; set; }

    public int? EserviceTypeBranchId { get; set; }

    public int? ServiceId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Conditions { get; set; }

    public string? RequiredDocuments { get; set; }

    public string? Measures { get; set; }

    public decimal? VariableFees { get; set; }

    public decimal? FixedFees { get; set; }

    public bool Status { get; set; }

    public int? Sort { get; set; }

    public string? Branch { get; set; }

    public string? Controller { get; set; }

    public string? Action { get; set; }

    public string? Url { get; set; }
    [ForeignKey("EserviceTypeBranchId")]
    public virtual EserviceTypeBranch? EserviceTypeBranch { get; set; }
    //[ForeignKey("EserviceTypeId")]
    //public virtual EserviceTypesLookup? EserviceTypesLookup { get; set; }
    [ForeignKey("ReqTypeId")]
    public virtual RequestsTypesLookup? RequestsTypesLookup { get; set; }
    [ForeignKey("LicTypeId")]
    public virtual LicenceTypesLookup? LicenceTypesLookup { get; set; }
    [ForeignKey("ActvityTypeId")]
    public virtual ActivityTypesLookup? ActivityTypesLookup { get; set; }
    [ForeignKey("TransTypeId")]
    public virtual TransactionTypesLookup? TransactionTypesLookup { get; set; }

}
