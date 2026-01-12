using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class CompleteLookup
    {
        public int ID { get; set; }
        public int? LicenseTypeID { get; set; }
        public int? RequestTypeID { get; set; }
        public int? TransactionTypeID { get; set; }
        public string? FieldNameDB { get; set; }
        public string? FieldValue { get;  set; }
        public string? FieldType { get; set; }
    }
}
