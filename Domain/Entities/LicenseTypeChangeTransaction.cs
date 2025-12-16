using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class LicenseTypeChangeTransaction
{
    public int Id { get; set; }

    public int? TransactionId { get; set; }

    public long? Requestid { get; set; }

    public long? NewRequestid { get; set; }

    public string? LicenseNo { get; set; }

    public bool? Status { get; set; }

    public string? OldCivilId { get; set; }

    public string? NewCivilId { get; set; }

    public string? LastUpdateUser { get; set; }
    public int? LicTypeNewId { get; set; }
    public int? LicTypeOldId { get; set; }
    public DateTime? LastUpdateDate { get; set; }
    public int? LicenceId { get; set; }
    public string? LicTypeOld { get; set; }
    public int? ServiceId { get; set; }
    public string? LicTypeNew { get; set; }
    [ForeignKey("LicenceId")]
    public virtual Licence Licence { get; set; }
    [ForeignKey("TransactionId")]
    public virtual Transaction? Transaction { get; set; }
}
