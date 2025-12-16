using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class CommercialNameChangeTransaction
{
    public int Id { get; set; }

    public int? TransactionId { get; set; }

    public int? ServiceId { get; set; }

    public string? OldCommercialName { get; set; }

    public string? NewCommercialName { get; set; }

    public string? LastUpdateUser { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public int? ComId { get; set; }

    public int? RequestId { get; set; }
}
