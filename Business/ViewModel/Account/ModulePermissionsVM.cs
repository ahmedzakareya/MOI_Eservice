using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Account
{
    public class ModulePermissionsVM
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public List<PermissionVM> Permissions { get; set; } = new List<PermissionVM>();
    }


}
