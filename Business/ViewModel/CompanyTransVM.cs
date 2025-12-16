using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class CompanyTransVM
    {
        public int Id { get; set; }

        public int? ServiceId { get; set; }

        public int? TransactionId { get; set; }

        public string? OldCompnayNameDir { get; set; }

        public string? NewCompanyNameDir { get; set; }
        public string? OldCompnayNameOwner { get; set; }

        public string? NewCompanyNameOwner { get; set; }
        public string? LastUpdateUser { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public int? CompId { get; set; }

        public int? RequestId { get; set; }
        [ForeignKey("TransactionId")]
        public virtual TransactionVM? Transaction { get; set; }
        public RequestDetailsVM? RequestDetailsVM { get; set; }

    }
}
