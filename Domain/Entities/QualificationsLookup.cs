using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class QualificationsLookup
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<TchangeManager> TchangeManagers { get; set; } = new List<TchangeManager>();
}
