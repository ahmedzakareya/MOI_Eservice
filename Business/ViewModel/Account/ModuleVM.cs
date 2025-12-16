using Business.ViewModel.Dynamic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Account
{
    public class ModuleVM
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }
        public virtual ICollection<RolePermissionVM> Permissions { get; set; } = new List<RolePermissionVM>();
        public virtual ICollection<AddMenuItemVM> MenuItems { get; set; } = new List<AddMenuItemVM>();

    }
}
