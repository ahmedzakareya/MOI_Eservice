using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class AreasLookup
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? GisAreaId { get; set; }

    public string? GisAreaText { get; set; }

    public int GovernorateId { get; set; }

    public virtual GovernoratesLookup Governorate { get; set; } = null!;
}
