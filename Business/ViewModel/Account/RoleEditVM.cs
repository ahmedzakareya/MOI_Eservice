using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Account
{
    public class RoleEditVM
    {
        public RoleDetailsVM Role { get; set; }
        public List<ModulePermissionsVM> AllModulesWithPermissions { get; set; }
    }
}
