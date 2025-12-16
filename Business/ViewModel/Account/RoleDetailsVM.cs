using Business.ViewModel.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Account
{
    public class RoleDetailsVM
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<PermissionVM> Permissions { get; set; } = new List<PermissionVM>();
        public List<ModuleVM> moduleVMs { get; set; }
        public List<EditMenuItemVM> editMenuItemVMs { get; set; }
        public List<UserAssignedVM> AssignedUsers { get; set; } = new List<UserAssignedVM>();
    }
}
