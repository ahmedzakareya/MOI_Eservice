using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class AspNetUserRoleWithSpec:Specification<AspNetUserRoleAdmin>
    {
        public AspNetUserRoleWithSpec(int SysUserId) :base(r=>r.SysUserId == SysUserId)
        {
        }

    }
}
