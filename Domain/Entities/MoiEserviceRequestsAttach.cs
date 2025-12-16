using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class MoiEserviceRequestsAttach
{
    public long AttachId { get; set; }

    public string? AttachName { get; set; }

    public string? AttachPath { get; set; }

    public int? ServiceId { get; set; }
   

    public long? AttachRequestid { get; set; }

    public string? AttachStatus { get; set; }

    public string? AttachType { get; set; }

    public int? DocType { get; set; }

    public bool? IsMandatory { get; set; }
    public DateTime? UploadedDate { get; set; }
    public string? UploadedBy { get; set; }
    public bool? IsApproved { get; set; }
    public bool? IsDeleted { get; set; }
    public bool? IsLatest { get; set; }
    public long? ReplacedByAttachId { get; set; }
    public int? TransactionTypeId { get; set; }
    public string? AttachFlag { get; set; }
    [ForeignKey("AttachRequestid")]
    public virtual MoiEserviceLicensesRequest? MoiEserviceLicensesRequest { get; set; }
}
