using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class WorkFlowActionButtonWithSpec : Specification<WorkFlowActionButton>
    {
        public WorkFlowActionButtonWithSpec() :base()
        {
            Includes.Add(a => a.WorkFlow);
            Includes.Add(a => a.WorkFlow.Eservice);
            Includes.Add(a => a.WorkFlow.RequestStatusCurrent);
            Includes.Add(a => a.WorkFlow.RequestStatusNext);
            Includes.Add(a => a.WorkFlow.RequestsTypesLookup);
            Includes.Add(a => a.WorkFlow.TransactionTypesLookup);

        }
        public WorkFlowActionButtonWithSpec(int id) : base(w=>w.Id==id)
        {
            Includes.Add(a => a.WorkFlow);
            Includes.Add(a => a.WorkFlow.Eservice);
            Includes.Add(a => a.WorkFlow.RequestStatusCurrent);
            Includes.Add(a => a.WorkFlow.RequestStatusNext);
            Includes.Add(a => a.WorkFlow.RequestsTypesLookup);
            Includes.Add(a => a.WorkFlow.TransactionTypesLookup);

        }



    }
}
