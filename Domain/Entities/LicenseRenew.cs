using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class LicenseRenew
{
    public int Id { get; set; }

    public int? LicenseId { get; set; }

    public int? ServiceId { get; set; }

    public string? OldExpiryDateOld { get; set; }

    public string? NewExpiryDateOld { get; set; }

    public string? LastUpdateUser { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public int? ReqTransId { get; set; }

    public DateTime? NewExpiryDate { get; set; }

    public DateTime? OldExpiryDate { get; set; }
    public int? RequestStatusId { get; set; }
}
