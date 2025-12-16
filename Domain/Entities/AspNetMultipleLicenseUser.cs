using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class AspNetMultipleLicenseUser
{
    public int Id { get; set; }

    public int? MultipleUserId { get; set; }

    public int? LicenseId { get; set; }

    public string? ServiceId { get; set; }
    public bool IsApproved { get; set; }
    public bool IsConfirmed { get; set; }

    public string? Note { get; set; }
    public string? AttachmentUrl { get; set; }
    [ForeignKey("LicenseId")]
    public virtual Licence? Licence { get; set; }
    [ForeignKey("MultipleUserId")]
    public virtual AspNetMultipleUser? AspNetMultipleUser { get; set; }
}
