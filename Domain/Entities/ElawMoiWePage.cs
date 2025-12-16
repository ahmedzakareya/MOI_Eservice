using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ElawMoiWePage
{
    public int Id { get; set; }

    public int? Pid { get; set; }

    public string? Description { get; set; }

    public string? DescriptionEn { get; set; }
}
