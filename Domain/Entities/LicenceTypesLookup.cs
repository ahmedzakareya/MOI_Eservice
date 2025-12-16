using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class LicenceTypesLookup
{
    public int Id { get; set; }

    public string? NameAr { get; set; }

    public string? NameEn { get; set; }

    public virtual ICollection<Licence> Licences { get; set; } = new List<Licence>();
}
