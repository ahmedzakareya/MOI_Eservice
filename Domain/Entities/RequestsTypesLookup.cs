using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class RequestsTypesLookup
{
    public int Id { get; set; }

    public string? NameAr { get; set; }

    public string? NameEn { get; set; }

    public int? ServiceId { get; set; }

    public string? Code { get; set; }

    public bool Status { get; set; }

    public int? Sort { get; set; }
}
