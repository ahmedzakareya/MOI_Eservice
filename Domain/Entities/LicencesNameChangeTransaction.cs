using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class LicencesNameChangeTransaction
    {
        public int Id { get; set; }
        public int? RequestId { get; set; }
    public int TransactionId { get; set; }
        public string LicencesNameOld { get; set; }
       public int? ServiceId { get; set; }
        public string LicencesNameNew { get; set; }
        public int? LicencesId { get; set; }
        [ForeignKey("TransactionId")]
        public virtual Transaction Transaction { get; set; }
    }
}
