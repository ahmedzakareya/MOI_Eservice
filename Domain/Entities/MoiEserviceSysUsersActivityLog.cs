using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class MoiEserviceSysUsersActivityLog
{
    public int Id { get; set; }

    public int? SysUserId { get; set; }

    public string? UserFullName { get; set; }

    public string? Section { get; set; }

    public string? Activity { get; set; }

    public DateTime? ActivityDate { get; set; }

    public int? ActivityItemId { get; set; }

    public string? ActivityItemName { get; set; }
    public string? ChangeLogs { get; set; } 

    public string? Note { get; set; }

    public int? RequestId { get; set; }
}
