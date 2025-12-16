using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Permission
{
    public int Id { get; set; }

    public string? NameAr { get; set; }

    public string? Description { get; set; }

    public string? NameEn { get; set; }

    //public int ModuleId { get; set; }
    //public virtual Module Module { get; set; }
    //public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>(); 

    public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
