using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Account
{
    public class UserAssignmentVM
    {
        public int? UserId { get; set; }
      
        public List<int>? RoleIds { get; set; }
       
        public List<AspnetUserRoleVM>? RoleModel { get; set; }
    }
}
