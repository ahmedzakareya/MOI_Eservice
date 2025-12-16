using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class MoiSocialMedia
{
    public long Id { get; set; }

    public int? SocialType { get; set; }

    public long? Requestid { get; set; }

    public string? AccountSocial { get; set; }
    public int? LicenceId { get; set; }
    [ForeignKey("SocialType")]
    public virtual SocialTypeLookup SocialTypeLookup { get; set; }
}
