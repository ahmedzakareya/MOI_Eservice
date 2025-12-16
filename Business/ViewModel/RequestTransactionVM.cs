using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class RequestTransactionVM
    {
        public int TransReqId { get; set; }

        public long? RequestId { get; set; }

        public long? LicenseId { get; set; }

        public int? ReqStatusId { get; set; }
       


        public string? ReqStatusName { get; set; }
        public string? Activity { get; set; }


        public int? EmployeeId { get; set; }

        public string? Notes { get; set; }

        public DateTime? OperationDate { get; set; }

        public int? ServiceId { get; set; }

        public string? EmployeeCivilId { get; set; }
        public string? EmployeeName { get; set; }

        public string? OldStatusName { get; set; }
        public string? NewStatusName { get; set; }
    }
}
