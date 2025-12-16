using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class RenewVM
    {
        public int Id { get; set; }

        public int? LicenseId { get; set; }

        public int? ServiceId { get; set; }

        public string? OldExpiryDateOld { get; set; }

        public string? NewExpiryDateOld { get; set; }

        public string? LastUpdateUser { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public int? TransactionId { get; set; }

        public DateTime? NewExpiryDate { get; set; }

        public DateTime? OldExpiryDate { get; set; }
        public int? RequestStatusId { get; set; }
        public bool? PaymentStatus { get; set; }
        public RequestDetailsVM? RequestDetailsVM { get; set; }
    }
}

