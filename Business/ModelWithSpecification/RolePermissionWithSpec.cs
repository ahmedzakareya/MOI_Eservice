using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class RolePermissionWithSpec:Specification<RolePermissionAdmin>
    {
        
        public RolePermissionWithSpec():base()
        {
            Includes.Add(r => r.Permission);
            Includes.Add(r => r.Role);
        }
    }

    public class RoleWithSpec:Specification<RoleAdmin>
    {
        public RoleWithSpec():base()
        {
                Includes.Add(r=>r.RolePermissions);
            Includes.Add(r=>r.UserRoles);
            Includes.Add(r=>r.RolePermissions.Select(r=>r.Permission));
            //Includes.Add(r=>r.RolePermissions.Select(r =>r.Permission));
        }
    }
}
