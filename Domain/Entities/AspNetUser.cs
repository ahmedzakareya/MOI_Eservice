using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

using System.Security.Claims;
using System.Threading.Tasks;



namespace Domain.Entities;
/*:IdentityUser<string>*/
[NotMapped]
public partial class AspNetUser:IdentityUser
{
    
    public DateTime? LockoutEndDateUtc { get; set; }

    public string? CivilId { get; set; }

    public string? FullNameAr { get; set; }

    public string? FullNameEn { get; set; }

    public string? Mobile { get; set; }

    public string? OldPassword { get; set; }

    public bool? SahelUser { get; set; }

    public bool? Profileupdate2023 { get; set; }

    public int? AccountTypeId { get; set; }
    
   // public bool IsApplicant { get; set; }


    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();
   
}
