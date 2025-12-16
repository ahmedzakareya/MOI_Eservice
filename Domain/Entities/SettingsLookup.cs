using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class SettingsLookup
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? MetaDescription { get; set; }

    public string? MetaKeyword { get; set; }

    public string? TopPhone { get; set; }

    public string? TopEmail { get; set; }

    public string? FaceBook { get; set; }

    public string? Instagram { get; set; }

    public string? Twitter { get; set; }

    public string? Youtube { get; set; }

    public int? LicensePeriod { get; set; }

    public int? ServiceId { get; set; }
}
