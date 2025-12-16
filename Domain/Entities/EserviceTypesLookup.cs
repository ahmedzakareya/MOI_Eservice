using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class EserviceTypesLookup
{
    public int Id { get; set; }

    public string? EserviceId { get; set; }

    public string? EserviceTypeEn { get; set; }

    public string? EserviceTypeAr { get; set; }

    public string? Url { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool? IsDeleted { get; set; }
}
