using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ValidEserviceCombinations
    {
        public int Id { get; set; }
        public int? ActivityTypeId { get; set; }
        public int? RequestTypeId { get; set; }
        public int? LicenceTypeId { get; set; }
        public bool IsAllowed { get; set; }
        [ForeignKey("ActivityTypeId")]
        public virtual ActivityTypesLookup? ActivityTypesLookup { get; set; }
        [ForeignKey("RequestTypeId")]
        public virtual RequestsTypesLookup? RequestsTypesLookup { get; set; }
        [ForeignKey("LicenceTypeId")]
        public virtual LicenceTypesLookup? LicenceTypesLookup { get; set; }
    }
}
