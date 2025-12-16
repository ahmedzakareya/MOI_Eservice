using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Dynamic
{
    public class AttachRuleVM
    {

        public int Id { get; set; }

        public int ServiceId { get; set; }

       // public int ActivityTypeId { get; set; }
        public int RequestId { get; set; }
        public int RequestTypeId { get; set; }

        public int RequestStatusId { get; set; }
        public int? TransactionTypeId { get; set; }
        public bool IsMandatory { get; set; }
        public string? AttachName { get; set; }
    }
}
