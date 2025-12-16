using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain.Entities;

[NotMapped]
public partial class AspNetRole :IdentityRole
{
    

    public string Name { get; set; } = null!;

    //public bool? IsAdmin { get; set; }
    //[JsonIgnore]
    //public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

    //[JsonIgnore]
    //public virtual ICollection<AspNetUserRole> UserRoles { get; set; } = new List<AspNetUserRole>();
}
