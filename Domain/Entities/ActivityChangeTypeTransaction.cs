using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ActivityChangeTypeTransaction
{
    public int Id { get; set; }

    public int? TransactionId { get; set; }

    public int? OldActivityType { get; set; }

    public int? NewActivityType { get; set; }

    public string? LastUpdateUser { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public int? RequestId { get; set; }

    public int? ServiceId { get; set; }
}
