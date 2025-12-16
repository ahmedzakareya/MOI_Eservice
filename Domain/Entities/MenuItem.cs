using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class MenuItem
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Url { get; set; }

    public int? ParentId { get; set; }

    public int? ModuleId { get; set; }
   
    public bool IsVisible { get; set; }
    [ForeignKey("ModuleId")]
    public virtual Module Module { get; set; }
    public virtual MenuItem? Parent { get; set; }
    public virtual ICollection<RolePermissionAdmin>? Permissions { get; set; }

    public virtual ICollection<MenuItem> Children { get; set; } = new List<MenuItem>();
}
