using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class MenuItemWithPermissionsSpec : Specification<MenuItem>
    {
        //public MenuItemWithPermissionsSpec()
        //: base(menu => menu.MenuItemPermissions.Any(mp => mp.PermissionId.HasValue && userPermissions.Contains(mp.PermissionId.Value)))
        //{
        //    // Add sorting by Sort property
        //    OrderByAsc(menu => menu.Sort);

        //    // Optionally, if you need to include related data, you can use AddInclude
        //    AddInclude(menu => menu.MenuItemPermissions);  // Assuming you want to load permissions along with the menu item
        //}
    }
}
