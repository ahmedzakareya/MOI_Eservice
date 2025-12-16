using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class ChangePersonVM
    {
        public int Id { get; set; }

        public int? TransactionId { get; set; }

        public int ServiceId { get; set; }

        public string? OldPersonName { get; set; }

        public string? NewPersonName { get; set; }

        public string? LastUpdateUser { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public int? CompId { get; set; }

        public int? RequestId { get; set; }
    }
}
