using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Domain.Entities;
[NotMapped]
public partial class AspNetUserRole : IdentityUserRole<string>
{
    //public int Id { get; set; }

    public string UserId { get; set; }

    public string RoleId { get; set; }

    public virtual AspNetUser User { get; set; } = null!;
    public virtual AspNetRole? Role { get; set; }

}
