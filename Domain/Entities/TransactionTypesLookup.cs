using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class TransactionTypesLookup
{
    public int Id { get; set; }

    public string? NameAr { get; set; }

    public string? NameEn { get; set; }

   // public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
