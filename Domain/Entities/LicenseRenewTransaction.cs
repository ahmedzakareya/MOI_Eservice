using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class LicenseRenewTransaction
{
    public int Id { get; set; }

    public int? TransactionId { get; set; }

    public int? ServiceId { get; set; }

    public DateTime? LicExpiredate { get; set; }

    public DateTime? LicRenewDate { get; set; }

    public int? RequestId { get; set; }
    public int? RequestStatusId { get; set; }
}
