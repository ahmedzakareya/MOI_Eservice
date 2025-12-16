using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ElawMoiWeLawRule
{
    public int Id { get; set; }

    public string? LawTitle { get; set; }

    public string? LawDescription { get; set; }

    public int? LawTypeId { get; set; }

    public string? LawTypeName { get; set; }

    public bool? Status { get; set; }

    public int? Sort { get; set; }
}
