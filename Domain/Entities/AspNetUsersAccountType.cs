using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class AspNetUsersAccountType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? TypeCode { get; set; }
}
