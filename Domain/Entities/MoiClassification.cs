using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class MoiClassification
{
    public int ClassifyId { get; set; }

    public string? ClassifiyName { get; set; }

    public int? CalssifiyType { get; set; }
    public int? ActivityTypeId { get; set; }
}
