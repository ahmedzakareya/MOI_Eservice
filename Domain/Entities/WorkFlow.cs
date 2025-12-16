using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class WorkFlow
{
    public int Id { get; set; }

    public int? ServiceId { get; set; }

    public int? RequestTypeId { get; set; }

    public int? CurrentStatusId { get; set; }

    public int? NextStatusId { get; set; }
    public string? Conditions { get; set; }  
    
    public string? FlagRequestStatus { get; set; }
      public string? FlagRequestType { get; set; }

    public int? SortOrder { get; set; }
    public bool IsPermissionRequired { get; set; }

  //  public int? ActivityTypeId { get; set; }
    public int? TransactionTypeId { get; set; }
    [ForeignKey("TransactionTypeId")]
    public virtual TransactionTypesLookup TransactionTypesLookup { get; set; }  
  //  [ForeignKey("ActivityTypeId")]
   // public virtual ActivityTypesLookup ActivityTypesLookup { get; set; }
    [ForeignKey("CurrentStatusId")]
    public virtual RequestStatusLookup RequestStatusCurrent { get; set; }
    [ForeignKey("NextStatusId")]
    public virtual RequestStatusLookup RequestStatusNext { get; set; }
    [ForeignKey("ServiceId")]
    public virtual Eservice Eservice { get; set; }
    [ForeignKey("RequestTypeId")]
    public virtual RequestsTypesLookup RequestsTypesLookup { get; set; }

}
