using Business.ViewModel.Dynamic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Account
{
    public class RoleVM
    {
        

      public int? RoleId { get; set; }

        public bool? IsAdmin { get; set; }

        public string PermissionAction {  get; set; }
		public List<int> PermissionIds { get; set; } // List of permissions assigned to this role
        public List<CombinedAssignmentVM> CombinedAssignments { get; set; } = new List<CombinedAssignmentVM>();
        public string MenuItemName { get; set; }
        public int? Id { get; set; }
        public string Name { get; set; }
        public int UserCount { get; set; } // Count of users in this role

        public IEnumerable<ModuleVM> Modules { get; set; } = new List<ModuleVM>(); // Associated modules
        public ICollection<PermissionVM> Permissions { get; set; } = new List<PermissionVM>(); // Associated permissions
        public IEnumerable<AddMenuItemVM> MenuItems { get; set; } = new List<AddMenuItemVM>(); // Associated menu items
      
        public string ModuleName { get; set; }
        public virtual ICollection<RolePermissionAdmin> RolePermissions { get; set; } = new List<RolePermissionAdmin>();


        public virtual ICollection<AspNetUserRoleAdmin> UserRoles { get; set; } = new List<AspNetUserRoleAdmin>();
        public List<string> PermissionsName { get; set; } = new List<string>();
      
    }
	public class RoleVMV
    {
		public int RoleId { get; set; }
		public string RoleName { get; set; }
        public int UserCount { get; set; }
        public List<MoiEserviceSysUserVM> userVMs { get; set; }
        public List<ModuleVM> Modules { get; set; }
       
	}

    public class RoleVMUpdate
    {
        public List<CombinedAssignmentVM> CombinedAssignments { get; set; } = new List<CombinedAssignmentVM>();
        public int RoleId { get; set; }
        public string RoleName { get; set; }



    }
    public class RoleWithModulesDTO
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<ModuleWithMenuItemsDTO> Modules { get; set; }
    }

    public class ModuleWithMenuItemsDTO
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public List<MenuItemWithPermissionsDTO> MenuItems { get; set; }
    }

    public class MenuItemWithPermissionsDTO
    {
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public string MenuItemUrl { get; set; }
        public bool IsVisible { get; set; }
        public List<PermissionDTO> Permissions { get; set; }
    }

    public class PermissionDTO
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
    }
    public class ModuleVMV
	{
		public int ModuleId { get; set; }

		public string ModuleName { get; set; }
		public List<MenuItemVMV> MenuItems { get; set; } = new List<MenuItemVMV>();
	}

	public class MenuItemVMV
	{
		public int MenuItemId { get; set; }
		public string MenuItemName { get; set; }
		public string MenuItemUrl { get; set; }
		public bool IsVisible { get; set; }
		public List<PermissionVMV> Permissions { get; set; } = new List<PermissionVMV>();
	}

	public class PermissionVMV
	{
		public int PermissionId { get; set; }
		public string PermissionName { get; set; }
	}

	public class AddRoleWithpermission
    {
        

        public string Name { get; set; } = null!;

        public bool? IsAdmin { get; set; }


        public List<CombinedAssignmentVM> CombinedAssignments { get; set; } = new List<CombinedAssignmentVM>();

    }
}
