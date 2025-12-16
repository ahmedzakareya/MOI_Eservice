using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class LicenceRenewTransVM
    {
        public int Id { get; set; }

        public int? TransactionId { get; set; }

        public int ServiceId { get; set; }

        public DateTime? LicExpiredate { get; set; }

        public DateTime? LicRenewDate { get; set; }

        public int? RequestId { get; set; }
    }
}
