using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Account
{
    public class RolePermissionVM
    {
        public int Id { get; set; }

        public int? RoleId { get; set; }
        public int ModuleId { get; set; }
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public string MenuItemUrl { get; set; }
        public int? MenuItemParentId { get; set; }
        public string ModuleName { get; set; }
        public virtual IEnumerable<Module> Module { get; set; }

        public virtual IEnumerable<MenuItem> MenuItem { get; set; }
        public int? PermissionId { get; set; }
        [ForeignKey("PermissionId")]
        public virtual Permission? Permission { get; set; }
        [ForeignKey("RoleId")]
        public virtual RoleAdmin? Role { get; set; }
    }
    public class RolePermissionVMV
    {
        public int Id { get; set; }

        public int? RoleId { get; set; }
        public int ModuleId { get; set; }
        public int MenuItemId { get; set; }
        public string MenuItemName { get; set; }
        public string MenuItemUrl { get; set; }
        public int? MenuItemParentId { get; set; }

        public virtual Module Module { get; set; }

        public virtual MenuItem MenuItem { get; set; }
        public int? PermissionId { get; set; }

        public virtual List<PermissionAdmin>? Permissions { get; set; }
        [ForeignKey("RoleId")]
        public virtual RoleAdmin? Role { get; set; }
    }
}
