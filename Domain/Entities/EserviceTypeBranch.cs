using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class EserviceTypeBranch
    {

        public int Id { get; set; }
        //public int? EserviceTypeId { get; set; }
        public string? EserviceTypeBranchEn { get; set; }
        public string? EserviceTypeBranchAr { get; set; }
        public string? Url { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public decimal? Fees { get; set; }
        public int? ActivityTypesId { get; set; }
        public int? Sort { get; set; }
        public int? RequestTypeId { get; set; }
        public bool Status { get; set; }
        [ForeignKey("ActivityTypesId")]
        public virtual ActivityTypesLookup? ActivityTypesLookup { get; set; }
        [ForeignKey("RequestTypeId")]
        public virtual RequestsTypesLookup? RequestsTypesLookup { get; set; }

    }
}
