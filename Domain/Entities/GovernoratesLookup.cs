using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class GovernoratesLookup
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? GisId { get; set; }

    public virtual ICollection<AreasLookup> AreasLookups { get; set; } = new List<AreasLookup>();
}
