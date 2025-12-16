using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class MoiEserviceRequestPaymentDetail
{
    public int Id { get; set; }

    public long? RequestId { get; set; }

    public string? UserId { get; set; }
    public string? AppCivilId { get;set; }

    public int? LicenceId { get; set; }

    public int? ServiceId { get; set; }

    public string? PaymentId { get; set; }

    public string? Result { get; set; }

    public string? TranId { get; set; }

    public string? Ref { get; set; }

    public string? Postdate { get; set; }

    public string? Auth { get; set; }

    public string? TrackId { get; set; }

    public decimal? TotalAmount { get; set; }

    public int? Payed { get; set; }

    public string? Token { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? Status { get; set; }

    public string? PaymentMethod { get; set; }

    public int? LicenseCategory { get; set; }
}
