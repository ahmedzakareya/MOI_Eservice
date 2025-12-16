using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ChangeMediaNameTransaction
{
    public long Id { get; set; }

    public long? RequestId { get; set; }

    public string? OldMediaName { get; set; }

    public string? NewMediaName { get; set; }

    public DateTime? RequestDate { get; set; }

    public bool? Status { get; set; }

    public long? TransactionId { get; set; }
    public string? LastUpdatedUser { get; set; }
}
