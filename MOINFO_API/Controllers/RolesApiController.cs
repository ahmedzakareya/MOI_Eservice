using AutoMapper;
using Business.Interfaces;
using Business.ModelWithSpecification;
using Business.Repository;
using Business.ViewModel.Account;
using Business.ViewModel.Dynamic;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Security.Claims;

namespace MOINFO_API.Controllers
{
    [Route("Roles")]
    public class RolesApiController : BaseController
    {
        private readonly IUnitOfwork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public RolesApiController(IUnitOfwork unitOfWork, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        #region Modules


        [HttpGet]
        [Route("GetModules")]
        public async Task<IActionResult> GetModules()
        {
            var modules = await _unitOfWork.genericRepository<Module>().GetAllAsync();
           //var modulesMapped = _mapper.Map<IEnumerable<Module>, IEnumerable<ModuleVM>>(modules);
           var getModules=modules.Select( x=> new ModuleVM
           {
               Description = x.Description,
               Name = x.Name,
               Id = x.Id,
           }).ToList();  
            return Ok(getModules);
        }

        [HttpGet]
        [Route("GetModule")]
        public async Task<IActionResult> GetModule(int id)
        {
            var module = await _unitOfWork.genericRepository<Module>().GetbyId(id);
            if (module == null) return NotFound();

            return Ok(module);
        }

        [HttpPost]
        [Route("AddModule")]
        public async Task<IActionResult> AddModule([FromBody] Module module)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _unitOfWork.genericRepository<Module>().Create(module);
            await _unitOfWork.Complete();
            return Ok(new { message = "Module created successfully!" });
        }

        [HttpPost]
        [Route("EditModule")]
        public async Task<IActionResult> EditModule([FromBody] Module module)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingModule = await _unitOfWork.genericRepository<Module>().GetbyId(module.Id);
            if (existingModule == null) return NotFound();

            existingModule.Name = module.Name;
            existingModule.Description = module.Description;
            await _unitOfWork.genericRepository<Module>().Update(existingModule);
            await _unitOfWork.Complete();
            return Ok(new { message = "Module updated successfully!" });
        }

        [HttpPost]
        [Route("DeleteModule")]
        public async Task<IActionResult> DeleteModule([FromBody] Module module)
        {
            var modulebyId = await _unitOfWork.genericRepository<Module>().GetbyId(module.Id);
            if (modulebyId == null) return NotFound();

            await _unitOfWork.genericRepository<Module>().Delete(modulebyId);
            await _unitOfWork.Complete();
            return Ok(new { message = "Module deleted successfully!" });
        }


        #endregion

        #region MenuItem




        [HttpGet]
        [Route("GetMenuItems")]
        public async Task<IActionResult> GetMenuItems()
        {
            var menuItems = await _unitOfWork.genericRepository<MenuItem>()
                .GetFilteredWithProjection(
                    filter:null,
                    selector: mi => new
                    {
                        mi.Id,
                        mi.Name,
                        mi.Url,
                        mi.IsVisible,
                        ModuleName = mi.Module.Name,
                        Permissions = mi.Module.RolePermissions
                    .Where(rp => rp.MenuItemId == mi.Id)
                    .Select(rp => new
                    {
                        PermissionId = rp.PermissionAdminId,
                        PermissionName = rp.Permission.NameAr,
                        RoleId = rp.RoleId,
                        RoleName = rp.Role.Name
                    })
                    .ToList(),
                    }
                   //mi => mi.Module, mi => mi.Module.RolePermissions.Select(rp => rp.Permission) // Include RolePermissions and Permission

                ).ToListAsync();
            // var menuItemMapped =await _mapper.Map<IEnumerable<MenuItem>,IEnumerable<AddMenuItemVM>>(menuItems);

            return Ok(menuItems);
        }

        [HttpGet]
        [Route("GetMenuItemById/{id}")]
        public async Task<IActionResult> GetMenuItemById(int id)
        {
            var menuItem = await _unitOfWork.genericRepository<MenuItem>()
                .GetFilteredWithProjection(
                    filter: mi => mi.Id == id,
                    selector: mi => new
                    {
                        mi.Id,
                        mi.Name,
                        mi.Url,
                        mi.IsVisible,
                        mi.ModuleId,
                        ModuleName = mi.Module.Name,
                        Permissions = mi.Module.RolePermissions
                            .Where(rp => rp.MenuItemId == mi.Id) // Filter RolePermissions for this MenuItem
                            .Select(rp => new
                            {
                                PermissionId = rp.PermissionAdminId,
                                PermissionName = rp.Permission.NameAr
                            }).ToList()
                    }
                    //includes: mi => mi.Module.RolePermissions.Select(rp => rp.Permission) // Include RolePermissions and Permission
                ).FirstOrDefaultAsync();

            if (menuItem == null) return NotFound();

            return Ok(menuItem);
        }


        [HttpPost]
        [Route("AddMenuItem")]
        public async Task<IActionResult> AddMenuItem([FromBody] AddMenuItemVM menuItemvm)
        {

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var menuItem = new MenuItem
            {
                IsVisible = menuItemvm.IsVisible,
                Name = menuItemvm.Name,
                Url = menuItemvm.Url,
                ModuleId = menuItemvm.ModuleId,

            };
            await _unitOfWork.genericRepository<MenuItem>().Create(menuItem);
            await _unitOfWork.Complete();
            return Ok(new { message = "Menu item created successfully!" });
        }

        [HttpPost]
        [Route("EditMenuItem")]
        public async Task<IActionResult> EditMenuItem([FromBody] EditMenuItemVM menuItem)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingMenuItem = await _unitOfWork.genericRepository<MenuItem>().GetbyId(menuItem.Id);
            if (existingMenuItem == null) return NotFound();

            existingMenuItem.Name = menuItem.Name;
            existingMenuItem.Url = menuItem.Url;
            existingMenuItem.ModuleId = menuItem.ModuleId;
            existingMenuItem.IsVisible = menuItem.IsVisible;
            await _unitOfWork.genericRepository<MenuItem>().Update(existingMenuItem);
            await _unitOfWork.Complete();
            return Ok(new { message = "Menu item updated successfully!" });
        }

        [HttpPost]
        [Route("DeleteMenuItem/{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var menuItem = await _unitOfWork.genericRepository<MenuItem>().GetbyId(id);
            if (menuItem == null) return NotFound();

            await _unitOfWork.genericRepository<MenuItem>().Delete(menuItem);
            await _unitOfWork.Complete();
            return Ok(new { message = "Menu item deleted successfully!" });
        }



        #endregion
        #region Permission
        [HttpGet]
        [Route("GetPermissions")]
        public async Task<IActionResult> GetPermissions()
        {
            var permissions = await _unitOfWork.genericRepository<Permission>()
                .GetFilteredWithProjection(
                    selector: p => new
                    {
                        p.Id,
                        p.NameAr,
                        //ModuleName = p.Module.Name
                    }
                  //includes: p => p.Module
                ).ToListAsync();

            return Ok(permissions);
        }

        [HttpGet]
        [Route("GetPermissionById/{id}")]
        public async Task<IActionResult> GetPermissionById(int id)
        {
            var permission = await _unitOfWork.genericRepository<Permission>()
                           .GetFilteredWithProjection(
                                filter: f => f.Id == id,
                               selector: mi => new
                               {
                                   mi.Id,
                                   mi.NameAr,
                                   mi.NameEn,

                                   //mi.ModuleId,
                                   //ModuleName = mi.Module.Name,

                               }
                             //includes: mi => mi.Module
                           ).FirstOrDefaultAsync();
            if (permission == null) return NotFound();
            var modules = await _unitOfWork.genericRepository<Module>().GetAllAsync();

            return Ok(permission);


        }

        [HttpPost]
        [Route("AddPermission")]
        public async Task<IActionResult> AddPermission([FromBody] PermissionVM permission)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var permissioncreate = new Permission
            {
                //ModuleId = permission.ModuleId,
                NameAr = permission.NameAr
            };
            await _unitOfWork.genericRepository<Permission>().Create(permissioncreate);
            await _unitOfWork.Complete();
            return Ok(new { message = "Permission created successfully!" });
        }

        [HttpPost]
        [Route("EditPermission")]
        public async Task<IActionResult> EditPermission([FromBody] PermissionVM permission)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var existingPermission = await _unitOfWork.genericRepository<Permission>().GetbyId(permission.Id);
            if (existingPermission == null) return NotFound();

            existingPermission.NameAr = permission.NameAr;
            //existingPermission.ModuleId = permission.ModuleId;
            await _unitOfWork.genericRepository<Permission>().Update(existingPermission);
            await _unitOfWork.Complete();
            return Ok(new { message = "Permission updated successfully!" });
        }

        [HttpPost]
        [Route("DeletePermission/{id}")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var getpermission = await _unitOfWork.genericRepository<Permission>().GetbyId(id);
            if (getpermission == null) return NotFound();

            await _unitOfWork.genericRepository<Permission>().Delete(getpermission);
            await _unitOfWork.Complete();
            return Ok(new { message = "Permission deleted successfully!" });
        }
        #endregion
        #region Roles
        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var rolesWithModules = await _unitOfWork.genericRepository<RoleAdmin>()
                                            .GetFilteredWithProjection(
                                                filter: null,
                                                selector: r => new RoleVMV
                                                {
                                                    UserCount= r.UserRoles.Count,
                                                    RoleId = r.Id,
                                                    RoleName = r.Name,
                                                    Modules = r.RolePermissions
                                                        .GroupBy(rp => new { rp.ModuleId, rp.Module.Name })
                                                        .Select(g => new ModuleVM
                                                        {
                                                            Id = g.Key.ModuleId,
                                                            Name = g.Key.Name,
                                                            MenuItems = g.GroupBy(mi => new { mi.MenuItemId, mi.MenuItem.Name, mi.MenuItem.Url, mi.MenuItem.IsVisible })
                                                                .Select(miGroup => new AddMenuItemVM
                                                                {
                                                                    Id = miGroup.Key.MenuItemId,
                                                                    Name = miGroup.Key.Name,
                                                                    Url = miGroup.Key.Url,
                                                                    IsVisible = miGroup.Key.IsVisible,
                                                                    Permissions = miGroup.Select(mip => new PermissionVM
                                                                    {
                                                                        Id = mip.PermissionAdminId,
                                                                        NameAr = mip.Permission.NameAr
                                                                    }).ToList()
                                                                }).ToList()
                                                        }).ToList()
                                                }
                                            ).ToListAsync();


            var rolesMapper = _mapper.Map<List<RoleVMV>>(rolesWithModules);
            return Ok(rolesWithModules);
        }

        [HttpGet]
        [Route("GetRoleAdmin")]
        public async Task<IActionResult> GetRoleAdmin()
        {
            var role = await _unitOfWork.genericRepository<RoleAdmin>().GetAllAsync();
            return Ok(role);
        }


        [HttpPost]
        [Route("AddRoleWithHisPermission")]
        public async Task<IActionResult> AddRoleWithHisPermission([FromBody] AddRoleWithpermission roleVM)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                // Step 1: Create the Role
                var role = new RoleAdmin
                {
                    IsAdmin = roleVM.IsAdmin,
                    Name = roleVM.Name
                };
                _unitOfWork.genericRepository<RoleAdmin>().Create(role);
                await _unitOfWork.Complete();


                // Step 2: Create combined RolePermissions
                var rolePermissions = roleVM.CombinedAssignments.Select(assignment => new RolePermissionAdmin
                {
                    RoleId = role.Id,
                    PermissionAdminId = assignment.PermissionId,
                    ModuleId = assignment.ModuleId,
                    MenuItemId = assignment.MenuItemId
                }).ToList();

                await _unitOfWork.genericRepository<RolePermissionAdmin>().AddRange(rolePermissions);

                await _unitOfWork.Complete();

                return Ok(new { message = "Role created successfully with combined permissions!" });
            }catch(Exception ex)
            {
                return BadRequest(ex);
            }
        }


        //[HttpGet]
        //[Route("GetModulesWithMenuItemsAndPermission")]
        //public async Task<IActionResult> GetModulesWithMenuItemsAndPermission()
        //{
        //    var modules = await _unitOfWork.genericRepository<Module>()
        //        .GetFilteredWithProjection(
        //            selector: m => new
        //            {
        //                ModuleId = m.Id,
        //                ModuleName = m.Name,
        //                MenuItems = m.MenuItems.Select(mi => new
        //                {
        //                    MenuItemId = mi.Id,
        //                    MenuItemName = mi.Name
        //                })
        //            },
        //            includes: m => m.MenuItems
        //        ).ToListAsync();

        //    var permissions=await _unitOfWork.genericRepository<Permission>().GetAllAsync();

        //    return Ok(new
        //    {
        //        modules = modules,
        //        permissions = permissions
        //    });
        //}
        [HttpGet]
        [Route("GetModulesWithMenuItemsAndPermission")]
        public async Task<IActionResult> GetModulesWithMenuItemsAndPermission()
        {
            try
            {
                // Fetch modules with menu items
                var modules = await _unitOfWork.genericRepository<Module>()
                    .GetFilteredWithProjection(
                        selector: m => new ModuleVM
                        {
                            Id = m.Id,
                            Name = m.Name,
                            MenuItems = m.MenuItems.Select(mi => new AddMenuItemVM
                            {
                                Id = mi.Id,
                                Name = mi.Name
                            }).ToList()
                        },
                        includes: m => m.MenuItems // Include related MenuItems
                    ).ToListAsync();

                // Fetch permissions
                var permissions = await _unitOfWork.genericRepository<PermissionAdmin>()
                    .GetFilteredWithProjection(
                        selector: p => new PermissionVM
                        {
                            Id = p.Id,
                            NameAr = p.NameAr
                        }
                    ).ToListAsync();

                // Return data
                return Ok(new
                {
                    modules = modules,
                    permissions = permissions
                });
            }
            catch (Exception ex)
            {
                // Log and return error
                return StatusCode(500, new { message = "An error occurred while fetching data.", error = ex.Message });
            }
        }


        [HttpPost]
        [Route("AddRoleWithPermissions")]
        public async Task<IActionResult> AddRoleWithPermissions([FromBody] RoleVM roleVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Create Role
            var role = new RoleAdmin
            {
                Name = roleVm.Name,
                IsAdmin = roleVm.IsAdmin
            };
            await _unitOfWork.genericRepository<RoleAdmin>().Create(role);
            await _unitOfWork.Complete();

            // Create RolePermissions
            var rolePermissions = roleVm.RolePermissions.Select(p => new RolePermissionAdmin
            {
                RoleId = role.Id, // Use the newly created role's ID
                MenuItemId = p.MenuItemId,
                ModuleId=p.ModuleId,
                PermissionAdminId = p.PermissionAdminId
            }).ToList();

            await _unitOfWork.genericRepository<RolePermissionAdmin>().AddRange(rolePermissions);
            await _unitOfWork.Complete();

            return Ok(new { message = "Role created successfully!" });
        }



        [HttpGet]
		[Route("GetModulesWithMenuItemsAndPermissions")]
		public async Task<IActionResult> GetModulesWithMenuItemsAndPermissions(int roleId)
		{
			var rolePermissions = await _unitOfWork.genericRepository<RolePermissionAdmin>()
				.GetFilteredWithProjection(
					filter: rp => rp.RoleId == roleId,
					selector: rp => rp.PermissionAdminId
				).ToListAsync();

			var modulesWithMenuItems = await _unitOfWork.genericRepository<Module>()
				.GetFilteredWithProjection(
					selector: m => new
					{
						ModuleId = m.Id,
						ModuleName = m.Name,
						MenuItems = m.MenuItems.Select(mi => new
						{
							MenuItemId = mi.Id,
							MenuItemName = mi.Name,
							mi.Url,
							mi.IsVisible,
							HasAccess = rolePermissions.Contains(m.Id) // Check if role has permission for this module
						}).ToList()
					},
					includes: m => m.MenuItems
				).ToListAsync();

			return Ok(modulesWithMenuItems);
		}


      

        [HttpPost]
        [Route("UpdateRoleWithPermissions")]
        public async Task<IActionResult> UpdateRoleWithPermissions([FromBody] RoleVMUpdate roleVm)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);




           await _unitOfWork.genericRepository<RolePermissionAdmin>()
                                      .DeleteRange(x => x.RoleId == roleVm.RoleId);

            // Create RolePermissions
            var rolePermissions = roleVm.CombinedAssignments.Select(p => new RolePermissionAdmin
            {
                RoleId = roleVm.RoleId, // Use the newly created role's ID
                MenuItemId = p.MenuItemId,
                ModuleId = p.ModuleId,
                PermissionAdminId = p.PermissionId
            }).ToList();

            await _unitOfWork.genericRepository<RolePermissionAdmin>().AddRange(rolePermissions);
            await _unitOfWork.Complete();

            return Ok(new { message = "Role created successfully!" });
        }

        [HttpDelete]
        [Route("DeleteRole{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _unitOfWork.genericRepository<RoleAdmin>().GetbyId(id);
            if (role == null) return NotFound();

            await _unitOfWork.genericRepository<RoleAdmin>().Delete(role);
            await _unitOfWork.Complete();
            return Ok(new { message = "Role deleted successfully!" });
        }

        [HttpGet]
        [Route("GetRoleById")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            try
            {
                var UserFromRole = await _unitOfWork.genericRepository<AspNetUserRoleAdmin>()
                    .GetFilteredWithProjection(
                    filter:u=>u.RoleId==id,
                    selector:u=>u.SysUserId
                    ).ToListAsync();
                var assignedPermissions =await _unitOfWork.genericRepository<RolePermissionAdmin>().
                    GetFilteredWithProjection(
                     filter: r => r.RoleId== id,
                     selector:rp=>rp.PermissionAdminId
                    ).ToListAsync();
    
                var roleDetails = await _unitOfWork.genericRepository<RoleAdmin>()
                    .GetFilteredWithProjection(
                        filter: r => r.Id == id,
                        selector: r => new RoleVMV
                        {
                            UserCount = r.UserRoles.Count,
                            RoleId = r.Id,
                            RoleName = r.Name,
                            userVMs = r.UserRoles.Select(u => new MoiEserviceSysUserVM
                            {
                                SysUserId = u.SysUserId,
                                Username=u.SysUser.Username,
                                Name=u.SysUser.Name,
                                LastLoginDate = u.SysUser.LastLoginDate,
                                CreateDate = u.SysUser.CreateDate
                               
                            }).ToList(),
                            
                            Modules = r.RolePermissions
                                                        .GroupBy(rp => new { rp.ModuleId, rp.Module.Name })
                                                        .Select(g => new ModuleVM
                                                        {
                                                            Id = g.Key.ModuleId,
                                                            Name = g.Key.Name,
                                                            MenuItems = g.GroupBy(mi => new { mi.MenuItemId, mi.MenuItem.Name, mi.MenuItem.Url, mi.MenuItem.IsVisible })
                                                                .Select(miGroup => new AddMenuItemVM
                                                                {
                                                                    Id = miGroup.Key.MenuItemId,
                                                                    Name = miGroup.Key.Name,
                                                                    Url = miGroup.Key.Url,
                                                                    IsVisible = miGroup.Key.IsVisible,
                                                                    Permissions = miGroup.Select(mip => new PermissionVM
                                                                    {
                                                                        Id = mip.PermissionAdminId,
                                                                        NameAr = mip.Permission.NameAr,
                                                                        Selected = assignedPermissions.Contains(mip.PermissionAdminId)
                                                                    }).ToList()
                                                                }).ToList()
                                                        }).ToList()
                        }
                    ).FirstOrDefaultAsync();


                if (roleDetails == null)
                {
                    return NotFound(new { message = "Role not found" });
                }

                return Ok(roleDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving role details", error = ex.Message });
            }
        }



        [HttpPost]
        [Route("UpdateRolePermission")]
        public async Task<IActionResult> UpdateRolePermission(int RoleId, List<string> modulePermissionPairs)
        {
            var Rolepermission = _unitOfWork.genericRepository<RolePermissionAdmin>()
                                       .DeleteRange(x => x.RoleId == RoleId);

            await _unitOfWork.Complete();
            var newRolePermissions = modulePermissionPairs
                        .Select(pair => pair.Split(','))
                        .Where(parts => parts.Length == 2 &&
                                        int.TryParse(parts[0], out _) &&
                                        int.TryParse(parts[1], out _))
                        .Select(parts => new RolePermissionAdmin
                        {
                            RoleId = RoleId,

                            PermissionAdminId = int.Parse(parts[1])
                        });

            await _unitOfWork.genericRepository<RolePermissionAdmin>().AddRange(newRolePermissions);
            await _unitOfWork.Complete();
            return Ok(new { Message = "RolePermission Added Successfully" });
        }
        [HttpGet]
        [Route("GetUserPermissions")]
        public async Task<IActionResult> GetUserPermissionsAsync(int userId)
        {
            try
            {
                // Step 1: Retrieve roles assigned to the user
                var userRoles = await _unitOfWork.genericRepository<AspNetUserRoleAdmin>()
                    .GetFilteredWithProjection(
                        filter: ur => ur.SysUserId == userId,
                        selector: ur => ur.RoleId
                    ).ToListAsync();

                if (userRoles == null || !userRoles.Any())
                {
                    return Ok(new { message = "No roles found for the user.", permissions = new List<object>() });
                }

                var roleIds = userRoles;

                // Step 2: Fetch role permissions for these roles
                var rolePermissions = await _unitOfWork.genericRepository<RolePermissionAdmin>()
                    .GetFilteredWithProjection(
                        filter: rp => roleIds.Contains(rp.RoleId),
                        selector: rp => new
                        {
                            RoleId = rp.RoleId,
                            PermissionId = rp.PermissionAdminId,
                            PermissionName = rp.Permission.NameAr,
                            MenuItemId = rp.MenuItemId,
                            MenuItemName = rp.MenuItem.Name, // Assuming navigation to MenuItem
                            ModuleId = rp.ModuleId,
                            ModuleName = rp.Module.Name    // Assuming navigation to Module
                        }
                    ).ToListAsync();

                // Step 3: Group and format the data
                var groupedPermissions = rolePermissions
                    .GroupBy(rp => rp.ModuleId)
                    .Select(moduleGroup => new
                    {
                        ModuleId = moduleGroup.Key,
                        ModuleName = moduleGroup.FirstOrDefault()?.ModuleName,
                        MenuItems = moduleGroup
                            .GroupBy(rp => rp.MenuItemId)
                            .Select(menuItemGroup => new
                            {
                                MenuItemId = menuItemGroup.Key,
                                MenuItemName = menuItemGroup.FirstOrDefault()?.MenuItemName,
                                Permissions = menuItemGroup.Select(rp => new
                                {
                                    rp.PermissionId,
                                    rp.PermissionName
                                }).ToList()
                            }).ToList()
                    }).ToList();

                // Step 4: Return the grouped result
                return Ok(new
                {
                    UserId = userId,
                    Permissions = groupedPermissions
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving permissions.", error = ex.Message });
            }
        }



        #endregion
        #region Get Menu For UserRole

        [HttpGet]
        [Route("GetModulesAndMenuItemsForUser")]
        public async Task<IActionResult> GetModulesAndMenuItemsForUser(int userId)
        {
            // Step 1: Fetch the user's roles
            var userRoles = await _unitOfWork.genericRepository<AspNetUserRoleAdmin>()
                .GetFilteredWithProjection(
                    filter: ur => ur.SysUserId == userId,
                    selector: ur => ur.RoleId
                ).ToListAsync();

            var roleIds = userRoles.Select(r => r).ToList();

            // Step 2: Fetch permissions associated with these roles
            var permissions = await _unitOfWork.genericRepository<RolePermissionAdmin>()
                .GetFilteredWithProjection(
                    filter: rp => roleIds.Contains(rp.RoleId),
                    selector: rp => new
                    {
                        rp.PermissionAdminId,
                        ModuleId = rp.ModuleId,
                        MenuItemId = rp.MenuItemId
                    }
                ).ToListAsync();

            var permissionIds = permissions.Select(p => p.PermissionAdminId).ToList();
            var moduleIds = permissions.Select(p => p.ModuleId).ToList();
            var menuItemIds = permissions.Select(p => p.MenuItemId).ToList();

            // Step 3: Fetch modules and menu items for these permissions
            var modulesWithMenuItems = await _unitOfWork.genericRepository<Module>()
                .GetFilteredWithProjection(
                    filter: m => moduleIds.Contains(m.Id), // Filter modules by permission
                    selector: module => new
                    {
                        ModuleId = module.Id,
                        ModuleName = module.Name,
                        MenuItems = module.MenuItems
                            .Where(mi => menuItemIds.Contains(mi.Id)) // Filter menu items by permission
                            .Select(mi => new
                            {
                                MenuItemId = mi.Id,
                                MenuItemName = mi.Name,
                                Url = mi.Url,
                                IsVisible = mi.IsVisible
                            }).ToList()
                    },
                    module => module.MenuItems // Include MenuItems
                ).ToListAsync();

            return Ok(modulesWithMenuItems);
        }

        #endregion


    }
}
