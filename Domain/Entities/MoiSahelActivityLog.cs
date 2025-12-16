using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class MoiSahelActivityLog
{
    public long Id { get; set; }

    public int ServiceId { get; set; }

    public string? IdentifierValue { get; set; }

    public string? Status { get; set; }

    public int? ActivityItemId { get; set; }

    public string? ActivityItemName { get; set; }

    public DateTime? ActivityDate { get; set; }

    public string? Note { get; set; }
}
