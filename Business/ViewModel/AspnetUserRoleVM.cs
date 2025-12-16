using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class AspnetUserRoleVM
    {
        public int Id { get; set; }
        public int UserId { get; set; } 

        public int RoleId { get; set; } 

        public List<int>? RoleIds { get; set; }

        public virtual MoiEserviceSysUser? User { get; set; }
        public virtual RoleAdmin? Role { get; set; }
    }
}
