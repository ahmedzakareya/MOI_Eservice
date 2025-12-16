using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class CompanyNameChangeTransaction
{
    public int Id { get; set; }

    public int? ServiceId { get; set; }

    public int? TransactionId { get; set; }
    public int? LicenceId { get; set; }
    public string? OldCompnayNameDir { get; set; }

    public string? NewCompanyNameDir { get; set; }
    public string? OldCompnayNameOwner { get; set; }

    public string? NewCompanyNameOwner { get; set; }
    public string? LastUpdateUser { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public int? CompId { get; set; }

    public int? RequestId { get; set; }
    [ForeignKey("TransactionId")]
    public virtual Transaction? Transaction { get; set; }
}
