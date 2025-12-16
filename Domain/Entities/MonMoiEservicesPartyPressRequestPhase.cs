using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class MonMoiEservicesPartyPressRequestPhase
{
    public int PhaseId { get; set; }

    public long? PhaseRequestid { get; set; }

    public long? PhaseLicenseid { get; set; }

    public string? PhaseStage { get; set; }

    public string? PhaseStageStatus { get; set; }

    public int? PhaseEmployeeid { get; set; }

    public string? PhaseNotes { get; set; }

    public DateTime? PhaseOperationdate { get; set; }
}
