using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class EmailChangeTransVM
    {
        public int Id { get; set; }

        public long? TransactionId { get; set; }

        public int? RequestId { get; set; }

        public string? OldOwnerEmail { get; set; }

        public string? NewOwnerEmail { get; set; }

        public string? OldManagerEmail { get; set; }

        public string? NewmanagerEmail { get; set; }

        public DateTime? RequestDate { get; set; }

        public bool? Status { get; set; }

        public string? LastUpdateUser { get; set; }
    }
}
