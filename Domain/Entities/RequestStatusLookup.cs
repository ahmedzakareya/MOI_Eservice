using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class RequestStatusLookup
{
    public int Id { get; set; }

    public string? NameAr { get; set; }

    public string? NameEn { get; set; }

    public bool? Status { get; set; }

    public int? Sort { get; set; }

    public int? ServiceId { get; set; }

    public bool? ForReNew { get; set; }

    public bool? ForNew { get; set; }
}
