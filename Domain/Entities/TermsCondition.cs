using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class TermsCondition
{
    public long Id { get; set; }

    public int? ConditionTypeId { get; set; }

    public string? TypeCondition { get; set; }

    public string? ConditionDesc { get; set; }

    public bool? Status { get; set; }

    public int? Sort { get; set; }
}
