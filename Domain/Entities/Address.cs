using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Address
{
    public int Id { get; set; }

    public string? Area { get; set; }

    public string? GovernorateArabic { get; set; }

    public string? BlockArabic { get; set; }

    public string? StreetArabic { get; set; }

    public string? City { get; set; }

    public string? FloorNo { get; set; }

    public string? AalliNo { get; set; }

    public int? ServiceId { get; set; }

    public string? Address1 { get; set; }

    public string? Name { get; set; }

    public string? UnitNo { get; set; }

    public string? ActivityCode { get; set; }

    public int? ActivityTypeId { get; set; }

    public int? ClassificationId { get; set; }

    public string? AreaSize { get; set; }

    public string? AreaChartNo { get; set; }

    public int? AreaId { get; set; }

    public int? GovernateId { get; set; }

    public string? BuildingName { get; set; }

    public string? BuildingNo { get; set; }
}
