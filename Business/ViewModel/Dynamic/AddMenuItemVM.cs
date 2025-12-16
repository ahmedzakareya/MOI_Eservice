using Business.ViewModel.Account;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Dynamic
{
    public class AddMenuItemVM
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Url { get; set; }

        public int? ParentId { get; set; }

        public int? ModuleId { get; set; }

        public bool IsVisible { get; set; }
        public virtual ModuleVM? Module { get; set; }
 

        public IEnumerable<SelectListItem>? Modules { get; set; }
        public virtual ICollection<PermissionVM>? Permissions { get; set; }
        public string? ModuleName { get; set; }
       
    }
    public class EditMenuItemVM
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Url { get; set; }
        public int? ModuleId { get; set; }

        public bool IsVisible { get; set; }

    }
}
