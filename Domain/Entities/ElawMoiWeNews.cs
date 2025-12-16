using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ElawMoiWeNews
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? SmallDescription { get; set; }

    public string? Image { get; set; }

    public string? Description { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreatedDate { get; set; }
}
