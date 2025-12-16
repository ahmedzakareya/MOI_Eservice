using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Module
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
   // public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>(); // Permissions in this Module
    public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>(); // Menu items in this Module

    // Navigation property for RolePermission
    public virtual ICollection<RolePermissionAdmin> RolePermissions { get; set; } = new List<RolePermissionAdmin>();

}
