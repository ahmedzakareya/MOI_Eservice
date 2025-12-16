using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ActivityType
{
    public int Id { get; set; }

    public string? EserviceId { get; set; }

    public string? ActivityTypeName { get; set; }
}
