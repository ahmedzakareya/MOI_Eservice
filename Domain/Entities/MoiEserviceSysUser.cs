using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public partial class MoiEserviceSysUser
{
    [Key]
    public int SysUserId { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Mobile { get; set; }

    public string? CivilId { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? ModifyDate { get; set; }

    public DateTime? LastLoginDate { get; set; }

    public int? UserType { get; set; }

    public int? UserRole { get; set; }

    public string? UserPasswordEncrypted { get; set; }

    public bool? SysUser { get; set; }

    public bool? RegUser { get; set; }

    public bool? Request { get; set; }

    public bool? Licnses { get; set; }

    public bool? LicnsesInfo { get; set; }

    public bool? Links { get; set; }

    public bool? Contact { get; set; }

    public bool? Parties { get; set; }

    public bool? Gisaddress { get; set; }

    public int? ServiceId { get; set; }

    public int? SectorId { get; set; }

    public int? DepId { get; set; }

    public int? MuraqabaId { get; set; }

    public int? QismId { get; set; }

    public string? ConfirmedPassword { get; set; }

    public virtual ICollection<AspNetUserRoleAdmin> AspNetUserRoles { get; set; } = new List<AspNetUserRoleAdmin>();
}
