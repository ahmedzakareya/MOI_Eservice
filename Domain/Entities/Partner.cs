using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class Partner
{
    public int Id { get; set; }

    public int? LicenseId { get; set; }
    public long? RequestId { get; set; }
    public int? ServiceId { get; set; }

    public string? Name { get; set; }

    public string? LastUpdateUser { get; set; }

    public DateTime? LastUpdateDate { get; set; }
    [ForeignKey("LicenseId")]
    public virtual Licence? Licence { get; set; }

    [ForeignKey("RequestId")]
    public virtual MoiEserviceLicensesRequest? Request { get; set; }
}
