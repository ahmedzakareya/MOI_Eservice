using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class RolePermissionAdmin
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public int PermissionAdminId { get; set; }
    public int MenuItemId { get; set; }
    public int ModuleId { get; set; }

    [ForeignKey("PermissionAdminId")]
    public virtual PermissionAdmin? Permission { get; set; }
    public virtual MenuItem? MenuItem { get; set; }
    public virtual Module? Module { get; set; }
    [ForeignKey("RoleId")]
    public virtual RoleAdmin? Role { get; set; }
}
