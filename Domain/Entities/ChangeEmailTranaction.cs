using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ChangeEmailTranaction
{
    public int Id { get; set; }

    public long? TransactionId { get; set; }
    public int? LicenceId { get; set; }

    public int? RequestId { get; set; }

    public string? OldOwnerEmail { get; set; }

    public string? NewOwnerEmail { get; set; }

    public string? OldManagerEmail { get; set; }

    public string? NewmanagerEmail { get; set; }

    public DateTime? RequestDate { get; set; }

    public bool? Status { get; set; }
    public string? LastUpdateUser {  get; set; } 
}
