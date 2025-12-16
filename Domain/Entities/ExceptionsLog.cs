using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class ExceptionsLog
{
    public int Id { get; set; }

    public string? Controller { get; set; }

    public string? Action { get; set; }

    public string? Method { get; set; }

    public string? Message { get; set; }

    public string? ApplicationName { get; set; }

    public string? Exception { get; set; }

    public DateTime? Created { get; set; }

    public bool? Solved { get; set; }

    public DateTime? SolvedOn { get; set; }

    public string? DeveloperNotes { get; set; }

    public string? Status { get; set; }
}
