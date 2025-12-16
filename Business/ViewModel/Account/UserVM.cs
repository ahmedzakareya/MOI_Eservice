using Business.ViewModel.Dynamic;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Account
{
    public class UserVM
    {
        public MoiEserviceSysUserVM? User { get; set; }

        public int SysUserId { get; set; }

        public string? SectorName { get; set; }
        public string? DepartName { get; set; }
        public string? QismName { get; set; }
        public string? MoraqabaName { get; set; }

        public IEnumerable<SelectListItem>? Sectors { get; set; }

        public IEnumerable<SelectListItem>? Departments { get; set; }
        public IEnumerable<SelectListItem>? Muraqabas { get; set; }
        public IEnumerable<SelectListItem>? Qisms { get; set; }

       // public IEnumerable<Permission>? AvailablePermissions { get; set; }
        public IEnumerable<RoleVMV>? AvailableRoles { get; set; } // List of all permissions
        // List of all permissions
        //public List<int>? SelectedPermissionsIds { get; set; }
        //public IEnumerable<MenuItem>? MenuItems {  get; set; }   
        //public IEnumerable<Module>? Modules {  get; set; }   
        public List<int>? SelectedRolesIds { get; set; }

    }
    public class userWithSystemOption
    {
        public AspnetUserVM? aspnetUserVM { get; set; }
        public List<SystemOptionVM> SystemOptions { get; set; }
    }
    public class UserProfile
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Mobile { get; set;}
        public string? CivilId { get; set; }
    }
}
