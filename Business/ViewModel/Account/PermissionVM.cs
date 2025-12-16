using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Account
{
    public class PermissionVM
    {
        public int Id { get; set; }

        public string? NameAr { get; set; }

     public bool Selected { get; set; }


        //public int ModuleId { get; set; }
        //public virtual Module? Module { get; set; }
        //public IEnumerable<SelectListItem>? Modules { get; set; }
        public virtual ICollection<RolePermissionAdmin> RolePermissions { get; set; } = new List<RolePermissionAdmin>();
        //public string? ModuleName { get; set; } = string.Empty;
      
    }
}
