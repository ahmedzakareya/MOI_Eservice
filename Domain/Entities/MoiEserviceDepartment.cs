using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class MoiEserviceDepartment
{
    public int Id { get; set; }

    public int? SectorId { get; set; }

    public string? Name { get; set; }

    public string? NameEn { get; set; }

    public bool? Status { get; set; }

    public int? Sort { get; set; }
}
