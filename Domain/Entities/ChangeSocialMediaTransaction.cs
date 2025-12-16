using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class ChangeSocialMediaTransaction
{
    public int Id { get; set; }

    public int? RequestId { get; set; }

    public int? SocialMediaType { get; set; }

    public string? SocialMediaRequestType { get; set; }

    public string? OldAccountSocial_MediaName { get; set; }

    public string? NewAccountSocial_Media { get; set; }

    public DateTime? RequestDate { get; set; }

    public bool? Status { get; set; }
    public int? LicenceId {  get; set; } 
    public long? TransactionId { get; set; }
    [ForeignKey("SocialMediaType")]
    public virtual SocialTypeLookup? SocialMedialookup { get; set; }
   
}
