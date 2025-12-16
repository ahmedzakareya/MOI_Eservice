using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[NotMapped]
public partial class AspNetUserClaim:IdentityUserClaim<string>
{
    //public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string? ClaimType { get; set; }

    public string? ClaimValue { get; set; }
    [ForeignKey("UserId")]
    public virtual AspNetUser User { get; set; } = null!;
}
