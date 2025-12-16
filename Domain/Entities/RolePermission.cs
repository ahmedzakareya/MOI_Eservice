using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class RolePermission
{
    public int Id { get; set; }

    public string RoleId { get; set; }

    public int PermissionId { get; set; }
   // public int MenuItemId { get; set; }
    //public int ModuleId { get; set; }

    public virtual Permission? Permission { get; set; }
    //public virtual MenuItem? MenuItem { get; set; }
    //public virtual Module? Module { get; set; }

    public virtual AspNetRole? Role { get; set; }
}
