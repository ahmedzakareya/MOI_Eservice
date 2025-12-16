using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class WorkFlowWithSpecification:Specification<WorkFlow>
    {
        public WorkFlowWithSpecification()
        {
            //Includes.Add(w => w.ActivityTypesLookup);
            Includes.Add(w => w.RequestStatusCurrent);
            Includes.Add(w => w.RequestStatusNext);
            Includes.Add(w => w.RequestsTypesLookup);
            Includes.Add(w => w.Eservice);

        }
    }
}
