using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class ReplacementOfLostTransaction
{
    public int Id { get; set; }

    public int? ReqTransactionId { get; set; }

    public int? ServiceId { get; set; }

    public string? LastUpdateUser { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public long? RequestId { get; set; }
    public int? LicId { get; set; }
    [ForeignKey("RequestId")]
    public virtual MoiEserviceLicensesRequest? MoiEserviceLicensesRequest { get; set; }
    [ForeignKey("LicId")]
    public virtual Licence? Licence { get; set; }

}
