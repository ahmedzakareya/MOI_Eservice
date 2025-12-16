using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class TransactionTypesLookupVM
    {
        public int Id { get; set; }

        public string? NameAr { get; set; }

        public string? NameEn { get; set; }


        // الطلبات  لهذا النوع 
        public bool? IsAvailable { get; set; }
        public long? PreviousRequestID { get; set; }
        public string? PreviousRequestNo { get; set; }
    }
}
