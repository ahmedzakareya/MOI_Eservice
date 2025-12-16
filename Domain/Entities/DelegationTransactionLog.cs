using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class DelegationTransactionLog
{
    public int Id { get; set; }

    public string? LicenseId { get; set; }

    public string? ServiceId { get; set; }

    public DateTime? CreateAt { get; set; }

    public string? TransactionType { get; set; }

    public string? CreateBy { get; set; }

    public string? MandoobId { get; set; }
}
