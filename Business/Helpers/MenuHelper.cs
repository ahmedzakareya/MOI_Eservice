using Business.Interfaces;
using Business.ModelWithSpecification;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers
{
    public  class MenuHelper
    {
        private readonly IUnitOfwork _unitOfwork;

        public  MenuHelper(IUnitOfwork unitOfwork)
        {
            _unitOfwork = unitOfwork;
        }
        #region Menu
        //public async Task<IEnumerable<MenuItem>> GetUserMenuAsync(string username)
        //{

        //    var spec = new UserPermissionwithSpec(username, true);
        //    var userPermissions = await _unitOfwork.genericRepository<UserPermission>().GetTableWithSpecService(spec);
        //    var permissionIds = userPermissions.Where(u => u.PermissionId.HasValue).Select(u => u.PermissionId.Value).ToList();
        //    // Get menu items that match any of the user's permissions
        //    var specmenuItem = new MenuItemWithPermissionsSpec(permissionIds);
        //    var menuItems = await _unitOfwork.genericRepository<MenuItem>().GetTableWithSpec(specmenuItem);


        //    return menuItems;
        //}

        #endregion
    }
}
