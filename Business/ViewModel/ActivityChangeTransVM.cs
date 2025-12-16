using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class ActivityChangeTransVM
    {
        public int Id { get; set; }

        public int? TransactionId { get; set; }

        public int? OldActivityType { get; set; }
        public string? OldActivityName { get; set; }

        public int? NewActivityType { get; set; }
        public string? NewActivityName { get; set; }


        public string? LastUpdateUser { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public int? RequestId { get; set; }
        public RequestDetailsVM? RequestDetailsVM { get; set; }
        public ActivityType? OldActivityTypeLookup { get; set; }
        public ActivityType? NewActivityTypeLookup { get; set; }
    }
}
