using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class CommercialTransVM
    {
        public int Id { get; set; }

        public int? TransactionId { get; set; }

        public int ServiceId { get; set; }

        public string? OldCommercialName { get; set; }

        public string? NewCommercialName { get; set; }

        public string? LastUpdateUser { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public int? ComId { get; set; }

        public int? RequestId { get; set; }
        public RequestDetailsVM? RequestDetailsVM { get; set; }
    }
}
