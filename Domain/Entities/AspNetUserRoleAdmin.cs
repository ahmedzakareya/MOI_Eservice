using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Domain.Entities;

public partial class AspNetUserRoleAdmin
{
    public int Id { get; set; }

    public int SysUserId { get; set; }

    public int RoleId { get; set; }
    [ForeignKey("SysUserId")]
    public virtual MoiEserviceSysUser SysUser { get; set; } = null!;
    [ForeignKey("RoleId")]
    public virtual RoleAdmin? Role { get; set; }

}
