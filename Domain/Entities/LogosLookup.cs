using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class LogosLookup
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Logo { get; set; }

    public string? Link { get; set; }

    public bool? Status { get; set; }

    public int? Sort { get; set; }
}
