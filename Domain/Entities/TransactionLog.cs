using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class TransactionLog
{
    public int Lid { get; set; }

    public int Id { get; set; }

    public string? DateTime { get; set; }

    public int? LicenseId { get; set; }

    public int ServiceId { get; set; }

    public int? TypeId { get; set; }

    public string? MotletterNo { get; set; }

    public string? MotletterDate { get; set; }

    public string? Changes { get; set; }

    public bool? Commited { get; set; }

    public string? Notes { get; set; }

    public string? LastUpdateUser { get; set; }

    public DateTime? LastUpdateDate { get; set; }
}
