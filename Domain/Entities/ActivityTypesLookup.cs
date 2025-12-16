using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ActivityTypesLookup
{
    public int Id { get; set; }

    public string? NameAr { get; set; }

    public int? MainLicenseId { get; set; }

    public int? ServiceId { get; set; }

    public string? ActivityCode { get; set; }

    public string? NameEn { get; set; }

    public string? EserviceId { get; set; }

    public Eservice? Eservice { get; set; }
}
