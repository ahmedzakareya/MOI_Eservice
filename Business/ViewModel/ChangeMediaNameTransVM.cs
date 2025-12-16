using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class ChangeMediaNameTransVM
    {
        public long Id { get; set; }

        public long? RequestId { get; set; }

        public string? OldMediaName { get; set; }

        public string? NewMediaName { get; set; }

        public DateTime? RequestDate { get; set; }

        public bool? Status { get; set; }

        public long? TransactionId { get; set; }
        public string? LastUpdatedUser { get; set; }
    }
}
