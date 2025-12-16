using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class MonMoiEservicePartiesSub
{
    public long Id { get; set; }

    public long? RequestId { get; set; }

    public int? PartySequenceNo { get; set; }

    public string? PartyNo { get; set; }

    public int? PartyAmount { get; set; }

    public DateTime? PartyStartDate { get; set; }

    public DateTime? PartyEndDate { get; set; }

    public DateTime? PartyStartTime { get; set; }

    public DateTime? PartyEndTime { get; set; }

    public int? PartyStatues { get; set; }

    public string? PartyNotes { get; set; }

    public string? PartyFinalNo { get; set; }
}
