using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;
[NotMapped]
public partial class AspNetUserLogin : IdentityUserLogin<string>
{
    public string LoginProvider { get; set; } = null!;

    public string ProviderKey { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public virtual AspNetUser User { get; set; } = null!;
}
