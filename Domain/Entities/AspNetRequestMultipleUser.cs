using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class AspNetRequestMultipleUser
{
    public int Id { get; set; }

    public string? MainUserId { get; set; }

    public string? MandoobId { get; set; }

    public string? RequestText { get; set; }

    public int? RoleId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public string? UpdatedBy { get; set; }
}
