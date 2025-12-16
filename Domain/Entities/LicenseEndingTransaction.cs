using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class LicenseEndingTransaction
{
    public int Id { get; set; }

    public long? RequestId { get; set; }

    public int? TransactionId { get; set; }

    public DateTime? LicExpiredate { get; set; }

    public int? EndReasonId { get; set; }
    public int? LicenseId { get; set; }

    public int? ServiceId { get; set; }
    public string? LastUpdateUser { get; set; }

    public DateTime? LastUpdateDate { get; set; }
    [ForeignKey("RequestId")]
    public virtual MoiEserviceLicensesRequest? MoiEserviceLicensesRequest { get; set; }
    [ForeignKey("LicenseId")]
    public virtual Licence? Licence { get; set; }
    [ForeignKey("EndReasonId")]
    public virtual MoiEserviceLicEndingReason? LicendingReason { get; set; }
}
