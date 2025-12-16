using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class TestablishContract
{
    public int EsId { get; set; }

    public string? EsTitle { get; set; }

    public int? ServiceId { get; set; }

    public int? RequestId { get; set; }
}
