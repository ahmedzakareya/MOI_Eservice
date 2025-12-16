using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class MonMoiEservicePartySubscriber
{
    public int Id { get; set; }

    public int? Nationality { get; set; }

    public string? CivilIdorPassportNo { get; set; }

    public string? Name { get; set; }

    public int? RequestId { get; set; }

    public string? Job { get; set; }

    public int? PartyId { get; set; }

    public string? AttachmentFile { get; set; }
}
