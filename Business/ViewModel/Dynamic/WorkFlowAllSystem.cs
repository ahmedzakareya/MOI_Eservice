using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Dynamic
{
    public class WorkFlowAllSystem
    {
        public IEnumerable<Eservice>? Services { get; set; }
        public IEnumerable<TransactionTypesLookup>? transactionTypesLookups { get; set; }
       // public IEnumerable<ActivityTypesLookup>? ActivityTypes { get; set; }
        public IEnumerable<RequestsTypesLookup>? RequestTypes { get; set; }
        public IEnumerable<RequestStatusLookup>? RequestStatuses { get; set; }
        
    }
}
