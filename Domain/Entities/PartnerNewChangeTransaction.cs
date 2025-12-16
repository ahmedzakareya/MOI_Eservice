using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class PartnerNewChangeTransaction
{
    public int Id { get; set; }

    public int? TransactionId { get; set; }

    public int? ServiceId { get; set; }

    public string? NewPartner { get; set; }

    public string? LastUpdateUser { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public int? PartId { get; set; }

    public long? RequestId { get; set; }
    public int? LicencesId { get; set; }
}
