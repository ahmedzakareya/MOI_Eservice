using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class ChangeNewPartnerTransVM
    {
        public int Id { get; set; }

        public int? TransactionId { get; set; }

        public int? ServiceId { get; set; }

        public string? NewPartner { get; set; }

        public string? LastUpdateUser { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public int? PartId { get; set; }

        public int? RequestId { get; set; }
        public RequestDetailsVM? RequestDetailsVM { get; set; }

    }
}
