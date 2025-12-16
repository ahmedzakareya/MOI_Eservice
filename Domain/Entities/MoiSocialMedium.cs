using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class MoiSocialMedium
{
    public long Id { get; set; }

    public int? SocialType { get; set; }

    public long? Requestid { get; set; }

    public string? AccountSocial { get; set; }

    public int? LicenceId { get; set; }
}
