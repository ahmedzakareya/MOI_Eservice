using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class MoiEservicesRequestTransaction
{
    public int TransReqId { get; set; }

    public long? RequestId { get; set; }

    public long? LicenseId { get; set; }

    public int? ReqStatusId { get; set; }
    
    public string? Activity { get; set; }

    public string? ReqStatusName { get; set; }


    public int? EmployeeId { get; set; }

    public string? Notes { get; set; }

    public DateTime? OperationDate { get; set; }

    public int? ServiceId { get; set; }
    public string? OldStatusName { get; set; }
    public string? NewStatusName { get; set; }
    public string? EmployeeCivilId { get; set; }
    [ForeignKey("EmployeeId")]
    public virtual MoiEserviceSysUser MoiEserviceSysUser { get; set; }

}
