using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class Transaction
{
    public int Id { get; set; }

    public int? LicenseId { get; set; }

    public int? ServiceId { get; set; }

    public int? TransTypeId { get; set; }

    public string? MotletterNo { get; set; }

    public string? Changes { get; set; }

    public bool? Commited { get; set; }

    public string? Notes { get; set; }

    public string? LastUpdateUser { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public long? RequestId { get; set; }

    public DateTime? MotletterDate { get; set; }

    public DateTime? RequestDate { get; set; }

    public string? UsercivilId { get; set; }

    public int? ReqStatusId { get; set; }

    public DateTime? TransDate { get; set; }

    [ForeignKey("TransTypeId")]
    public virtual TransactionTypesLookup? TransType { get; set; }
    [ForeignKey("LicenseId")]

    public virtual Licence? Licence { get; set; }
    [ForeignKey("RequestId")]
    public virtual MoiEserviceLicensesRequest? Request { get; set; }
    [ForeignKey("ReqStatusId")]
    public virtual RequestStatusLookup? RequestStatus { get; set; }


    //public virtual CompanyNameChangeTransaction CompanyNameChangeTransaction { get; set; }
   
    //public virtual AddressChangeTransaction AddressChangeTransaction { get; set; }
    //public virtual TchangeManager TchangeManager { get; set; }


}
