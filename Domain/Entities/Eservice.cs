using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Eservice
{
    public string Id { get; set; } = null!;

    public string? EserviceName { get; set; }

    public string? EserviceNameAr { get; set; }

    public string? Url { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool IsDeleted { get; set; }

    public int? ServiceId { get; set; }
}
