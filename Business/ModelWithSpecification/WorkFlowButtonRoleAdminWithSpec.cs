using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class WorkFlowButtonRoleAdminWithSpec : Specification<WorkFlowButtonRoleAdmin>
    {
        public WorkFlowButtonRoleAdminWithSpec() :base()
        {
            Includes.Add(a => a.RoleAdmin);
            Includes.Add(a => a.WorkFlowActionButton);

        }
        public WorkFlowButtonRoleAdminWithSpec(int id) : base(w=>w.Id==id)  
        {
            Includes.Add(a => a.RoleAdmin);
            Includes.Add(a => a.WorkFlowActionButton);

        }


    }
}
