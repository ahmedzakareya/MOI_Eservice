using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class LicencesNameChangeTransactionVM
    {

        public int Id { get; set; }
        public int? RequestId { get; set; }
        public int TransactionId {  get; set; } 
        public string LicencesNameOld { get; set; }

        public string LicencesNameNew { get; set; }
        public int? LicencesId { get; set; }
    }
}
