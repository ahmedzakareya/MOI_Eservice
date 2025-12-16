using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Form
{
    public int Id { get; set; }

    public int ServiceId { get; set; }

    public string? FormName { get; set; }

    public string? FormPath { get; set; }

    public string? FormStatus { get; set; }

    public string? FormType { get; set; }

    public int? DocType { get; set; }

    public bool? IsDeleted { get; set; }
}
