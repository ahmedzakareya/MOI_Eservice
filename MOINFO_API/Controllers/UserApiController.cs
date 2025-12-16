using AutoMapper;
using Business.Enums;
using Business.Helpers;
using Business.Interfaces;
using Business.ModelWithSpecification;
using Business.Repository;
using Business.ViewModel;
using Business.ViewModel.Account;
using Business.ViewModel.Hierarchy;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Data;

namespace MOINFO_API.Controllers
{
    [Route("Userapi")]
    public class UserApiController : BaseController
    {
        private readonly IUnitOfwork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly MenuHelper _menuHelper;
        private readonly string? _hierarchy;
        public UserApiController(IUnitOfwork unitOfwork, IConfiguration configuration, IMapper mapper, MenuHelper menuHelper)
        {
            _unitOfWork = unitOfwork;
            _mapper = mapper;
            _menuHelper = menuHelper;
            _hierarchy = configuration["Hierarachy:MoiInfoHierarchyURL"];
        }

        #region Hierarchy 
        [HttpGet("GetSectors")]
        public async Task<IEnumerable<SelectListItem>> GetSectors()
        {
            // Fetching sectors from external API
            var sectors = await _unitOfWork.FetchFromApiAsync<SectorVM>(_hierarchy + "MOINFOHierarchy/GetQitae");

            // Convert sectors to SelectListItems
            return sectors.Select(s => new SelectListItem
            {

                Value = s.ID.ToString(),  // Assuming SectorModel has a SectorID property
                Text = s.QitaeName             // Assuming SectorModel has a SectorName property
            }).ToList();

            //return Ok(sectorSelectList);
        }

        [HttpGet("GetDepartments/{sectorId}")]
        public async Task<IEnumerable<SelectListItem>> GetDepartments(int sectorId)
        {
            // Fetching departments by sectorId from external API
            var departments = await _unitOfWork.FetchFromApiAsync<DepartVM>(_hierarchy + $"MOINFOHierarchy/GetEdara?parantID={sectorId}");

            // Convert departments to SelectListItems
            return departments.Select(d => new SelectListItem
            {
                Value = d.ID.ToString(), // Assuming DepartmentModel has a DepartmentID property
                Text = d.EdaraName           // Assuming DepartmentModel has a DepartmentName property
            }).ToList();

            //return Ok(departmentSelectList);
        }
        [HttpGet("GetMoraqaba/{DepartId}")]
        public async Task<IEnumerable<SelectListItem>> GetMoraqaba(int DepartId)
        {
            // Fetching departments by sectorId from external API
            var Moraqaba = await _unitOfWork.FetchFromApiAsync<MuraqabaVM>(_hierarchy + $"MOINFOHierarchy/GetMuraqaba?parantID={DepartId}");

            // Convert departments to SelectListItems
            return Moraqaba.Select(d => new SelectListItem
            {
                Value = d.ID.ToString(), // Assuming DepartmentModel has a DepartmentID property
                Text = d.MuraqabaName
                // Assuming DepartmentModel has a DepartmentName property
            }).ToList();

            //return Ok(MoraqabaSelectList);
        }

        [HttpGet("GetQism")]
        public async Task<IEnumerable<SelectListItem>> GetQism([FromQuery] int MoraqabaId)
        {

            var Qism = await _unitOfWork.FetchFromApiAsync<QismVM>(_hierarchy + $"MOINFOHierarchy/GetQism?parantID={MoraqabaId}");


            return Qism.Select(d => new SelectListItem
            {
                Value = d.ID.ToString(),
                Text = d.QismName

            }).ToList();


        }
        [HttpGet("GetDepartNameByIdAsync")]
        public async Task<string> GetDepartNameByIdAsync([FromQuery] int departId)
        {
            try
            {
                var departNameById = await _unitOfWork.FetchByIdFromApiAsync<DepartVM>(_hierarchy + $"MOINFOHierarchy/GetEdaraName?ID={departId}");


                return departNameById.EdaraName;



            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        [HttpGet("GetSectorNameByIdAsync")]
        public async Task<string> GetSectorNameByIdAsync([FromQuery] int sectorId)
        {
            try
            {
                var sectorNameById = await _unitOfWork.FetchByIdFromApiAsync<SectorVM>(_hierarchy + $"MOINFOHierarchy/GetHierarchyName?ID={sectorId}");

                return sectorNameById.HierarchyName;



            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        [HttpGet("GetHierarchyNameByIdAsync")]
        public async Task<string> GetHierarchyNameByIdAsync([FromQuery] int hierarchyName)
        {
            try
            {
                var sectorNameById = await _unitOfWork.FetchByIdFromApiAsync<HierarchyVM>(_hierarchy + $"MOINFOHierarchy/GetHierarchyName?ID={hierarchyName}");

                return sectorNameById.HierarchyName;



            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// [HttpGet("GetQismNameByIdAsync")]
        /// </summary>
        /// <param name="sectorId"></param>
        /// <param name="departId"></param>
        /// <returns></returns>
        //public async Task<string> GetQismNameByIdAsync([FromQuery] int qism)
        //{
        //    try
        //    {
        //        var sectorNameById = await _unitOfWork.FetchByIdFromApiAsync<QismVM>(_hierarchy + $"MOINFOHierarchy/GetHierarchyName?ID={qism}");
        //        return sectorNameById.QismName;
        //    }
        //    catch (Exception)
        //    {
        //        return string.Empty;
        //    }
        //}
        [HttpGet("GetSectorAndDepartmentNamesAsync")]
        public async Task<(string SectorName, string DepartmentName)> GetSectorAndDepartmentNamesAsync([FromQuery] int sectorId, [FromQuery] int departId)
        {
            var sectorResponse = await _unitOfWork.FetchByIdFromApiAsync<SectorVM>(_hierarchy + $"MOINFOHierarchy/GetHierarchyName?ID={sectorId}");
            string sectorName = sectorResponse?.HierarchyName ?? "Unknown Sector";

            var departResponse = await _unitOfWork.FetchByIdFromApiAsync<DepartVM>(_hierarchy + $"MOINFOHierarchy/GetEdaraName?ID={departId}");
            string departmentName = departResponse?.EdaraName ?? "Unknown Department";

            return (sectorName, departmentName);
        }

        #endregion
        [HttpGet]
        [Route("GetUserWithAllPermission")]
        public async Task<IActionResult> GetUserWithAllPermission()
        {
            var spec = new SysUserWithSpec();
            var user = await _unitOfWork.genericRepository<MoiEserviceSysUser>().GetTableWithSpec(spec);
            return Ok(_mapper.Map<IEnumerable<MoiEserviceSysUser>, IEnumerable<MoiEserviceSysUserVM>>(user));
        }
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] UserVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var spec = new SysUserWithSpec(model.User.CivilId);
            var userExist = await _unitOfWork.genericRepository<MoiEserviceSysUser>()
                .GetByIdObject(s => s.CivilId == model.User.CivilId);
            if (userExist == null)
            {
                var user = new MoiEserviceSysUser
                {
                    CivilId = model.User.CivilId,
                    Name = model.User.Name,
                    Username = model.User.CivilId,
                    Mobile = model.User.Mobile,
                    Email = model.User.Email,
                    Password = model.User.Password,
                    UserPasswordEncrypted = model.User.UserPasswordEncrypted,
                    SectorId = model.User.SectorId,
                    DepId = model.User.DepId,
                    MuraqabaId = model.User.MuraqabaId,
                    QismId = model.User.QismId,
                    ServiceId = model.User.ServiceId,
                    Status = model.User.Status,

                };

                await _unitOfWork.genericRepository<MoiEserviceSysUser>().Create(user);
                var resultCreate = await _unitOfWork.Complete();

                if (resultCreate > 0)
                {
                    return Ok(new { message = "User created successfully!", userId = user.SysUserId });
                }
                else
                {
                    return StatusCode(500, "Failed to create user. Please try again.");
                }
            }
            else
            {
                return Ok(new { message = "User Already Exist!" });
            }

        }
       

        [HttpPost]
        [Route("AssignRoles")]
        public async Task<IActionResult> AssignRoles([FromBody] AspnetUserRoleVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                foreach (var roleId in model.RoleIds)
                {
                    // Check if the UserRole already exists to prevent duplicates
                    var existingRole = await _unitOfWork.genericRepository<AspNetUserRoleAdmin>()
                        .GetByIdObject(ur => ur.SysUserId == model.UserId && ur.RoleId == roleId);

                    if (existingRole == null)
                    {
                        var userRole = new AspNetUserRoleAdmin
                        {
                            SysUserId = model.UserId,
                            RoleId = roleId
                        };

                        await _unitOfWork.genericRepository<AspNetUserRoleAdmin>().Create(userRole);
                    }
                }
                // Save all changes at once
                var result = await _unitOfWork.Complete();

                if (result > 0)
                {
                    return Ok(new { message = "Roles assigned successfully!" });
                }
                else
                {
                    return StatusCode(500, "Failed to assign roles. Please try again.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var spec = new SysUserWithSpec(id);

            // Step 1: Retrieve user data based on the ID
            var userEdit = await _unitOfWork.genericRepository<MoiEserviceSysUser>().GetByIdWithSpec(spec);
            int sectorId = userEdit.SectorId ?? 0;
            int departId = userEdit.DepId ?? 0;
            int qismid = userEdit.QismId ?? 0;
            int muraqabaId = userEdit.MuraqabaId ?? 0;
            var sectorName = await GetHierarchyNameByIdAsync(sectorId);
            var departName = await GetHierarchyNameByIdAsync(departId);
            var qismName = await GetHierarchyNameByIdAsync(qismid);
            var muraqabaName = await GetHierarchyNameByIdAsync(muraqabaId);
            var allroles = await _unitOfWork.genericRepository<RoleAdmin>().GetAllAsync();

           

            
            var roles = await _unitOfWork.genericRepository<RoleAdmin>()
      .GetFilteredWithProjection(
          filter: null,
          selector: r => new RoleVMV
          {
              RoleId = r.Id,
              RoleName = r.Name
          }
      ).ToListAsync();

            var selectedRoles = await _unitOfWork.genericRepository<AspNetUserRoleAdmin>()
                .GetFilteredWithProjection(
                    filter: x => x.SysUserId == id,
                    selector: u => u.RoleId
                ).ToListAsync();

            var editUser = new UserVM
            {
                SectorName = sectorName,
                DepartName = departName,
                MoraqabaName = muraqabaName,
                QismName = qismName,
                Sectors = await GetSectors(),
                Departments = await GetDepartments(sectorId),
                Muraqabas = await GetMoraqaba(departId),
                Qisms = await GetQism(muraqabaId),
                User = _mapper.Map<MoiEserviceSysUser, MoiEserviceSysUserVM>(userEdit),
                AvailableRoles = roles, // This now uses RoleDTO
                SysUserId = userEdit.SysUserId,
                SelectedRolesIds = selectedRoles
            };

            return Ok(editUser);


        

        }


        [HttpPost]
        [Route("Edit")]
        public async Task<IActionResult> Edit([FromBody] UserVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var specuser = new SysUserWithSpec(model.User.SysUserId);
            var editUser = await _unitOfWork.genericRepository<MoiEserviceSysUser>().GetByIdWithSpec(specuser);
            if (editUser == null)
            {
                return NotFound();
            }
            else
            {
                //editUser.CivilId = model.User.CivilId;
                editUser.Name = model.User.Name;
                //editUser.Username = model.User.Username;
                editUser.Mobile = model.User.Mobile;
                editUser.Email = model.User.Email;
                editUser.SectorId = model.User.SectorId;
                editUser.DepId = model.User.DepId;
                editUser.MuraqabaId = model.User.MuraqabaId;
                editUser.QismId = model.User.QismId;
                

                // Update user's permissions and roles

                // Add new permissions

                await _unitOfWork.genericRepository<MoiEserviceSysUser>().Update(editUser);
                var resultUpdate = await _unitOfWork.Complete();

                if (resultUpdate > 0)
                {
                    return Ok(new { message = "User Updated successfully!", userId = editUser.SysUserId });
                }
                else
                {
                    return StatusCode(500, "Failed to create user. Please try again.");
                }
            }
        }

        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var spec = new SysUserWithSpec(id);

            // Step 1: Retrieve user data based on the ID
            var userEdit = await _unitOfWork.genericRepository<MoiEserviceSysUser>().GetByIdWithSpec(spec);
            int sectorId = userEdit.SectorId ?? 0;
            int departId = userEdit.DepId ?? 0;
            int qismid = userEdit.QismId ?? 0;
            int muraqabaId = userEdit.MuraqabaId ?? 0;
            var sectorName = await GetHierarchyNameByIdAsync(sectorId);
            var departName = await GetHierarchyNameByIdAsync(departId);
            var qismName = await GetHierarchyNameByIdAsync(qismid);
            var muraqabaName = await GetHierarchyNameByIdAsync(muraqabaId);
            var roles = await _unitOfWork.genericRepository<AspNetRole>().GetAllAsync();
            var permissions = await _unitOfWork.genericRepository<Permission>().GetAllAsync();

            var selectedroles = userEdit.AspNetUserRoles?
                                             .Select(p => p.RoleId)

                                             .ToList() ?? new List<int>();
                                   

            var editUser = new UserVM
            {
                SectorName = sectorName,
                DepartName = departName,
                MoraqabaName = muraqabaName,
                QismName = muraqabaName,
                Sectors = await GetSectors(),
                Departments = await GetDepartments(sectorId),
                Muraqabas = await GetMoraqaba(departId),
                Qisms = await GetQism(muraqabaId),
                User = _mapper.Map<MoiEserviceSysUser, MoiEserviceSysUserVM>(userEdit),
                //AvailableRoles = roles,
                //AvailablePermissions = permissions,
                SelectedRolesIds = selectedroles,
                //SelectedPermissionIds = selectedpermissions

            };

            return Ok(editUser);


        }
      

        [HttpPost]
        [Route("EditAssignRoleToUser")]
        public async Task<IActionResult> EditAssignRoleToUser([FromBody] UserAssignmentVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                int id = model.UserId ?? 0;
                var specuser = new SysUserWithSpec(id);
                var editUser = await _unitOfWork.genericRepository<MoiEserviceSysUser>().GetByIdWithSpec(specuser);
              
                var specuserrole = new AspNetUserRoleWithSpec(id);
                var puserInrole = await _unitOfWork.genericRepository<AspNetUserRoleAdmin>().GetTableWithSpec(specuserrole);
                foreach (var item in puserInrole)
                {
                    await _unitOfWork.genericRepository<AspNetUserRoleAdmin>().Delete(item);

                }
                await _unitOfWork.Complete();

                foreach (var roleId in model.RoleIds)
                {
                    // Check if the UserRole already exists to prevent duplicates
                    var existingRole = await _unitOfWork.genericRepository<AspNetUserRoleAdmin>()
                        .GetByIdObject(ur => ur.SysUserId == model.UserId && ur.RoleId == roleId);

                    if (existingRole == null)
                    {
                        var userRole = new AspNetUserRoleAdmin
                        {
                            SysUserId = editUser.SysUserId,
                            RoleId = roleId
                        };

                        await _unitOfWork.genericRepository<AspNetUserRoleAdmin>().Create(userRole);
                    }
                }
               
                 await _unitOfWork.Complete();
                



                return Ok(new { message = "Permissions and roles assigned successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }


        
        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var role = await _unitOfWork.genericRepository<RoleAdmin>()
                .GetFilteredWithProjection(
                filter:null,
                selector:r=>new RoleVMV
                {
                    RoleId=r.Id,
                    RoleName=r.Name
                }
                ).ToListAsync();
            return Ok(role);
        }
       
        [HttpGet]
        [Route("GetSpecificUserWithAllPermission")]
        public async Task<IActionResult> GetSpecificUserWithAllPermission(int id)
        {
            var spec = new SysUserWithSpec(id);


            var userEdit = await _unitOfWork.genericRepository<MoiEserviceSysUser>().GetByIdWithSpec(spec);
            int sectorId = userEdit.SectorId ?? 0;
            int departId = userEdit.DepId ?? 0;
            int qismid = userEdit.QismId ?? 0;
            int muraqabaId = userEdit.MuraqabaId ?? 0;
            var sectorName = await GetHierarchyNameByIdAsync(sectorId);
            var departName = await GetHierarchyNameByIdAsync(departId);
            var qismName = await GetHierarchyNameByIdAsync(qismid);
            var muraqabaName = await GetHierarchyNameByIdAsync(muraqabaId);
            var roles = await _unitOfWork.genericRepository<RoleAdmin>().GetAllAsync();
            var permissions = await _unitOfWork.genericRepository<PermissionAdmin>().GetAllAsync();

            var selectedroles = userEdit.AspNetUserRoles
                                             .Select(p => p.RoleId)


                                             .ToList() ?? new List<int>();

           

            var editUser = new UserVM
            {
                SectorName = sectorName,
                DepartName = departName,
                MoraqabaName = muraqabaName,
                QismName = muraqabaName,
                Sectors = await GetSectors(),
                Departments = await GetDepartments(sectorId),
                Muraqabas = await GetMoraqaba(departId),
                Qisms = await GetQism(muraqabaId),
                User = _mapper.Map<MoiEserviceSysUser, MoiEserviceSysUserVM>(userEdit),
                //AvailableRoles = roles,
                //AvailablePermissions = permissions,
                SelectedRolesIds = selectedroles,
               

            };

            return Ok(editUser);

        }

        [HttpGet]
        [Route("GetMenuItemsForUser")]
        public async Task<IActionResult> GetMenuItemsForUser(int userId)
        {
            var roleIds = await _unitOfWork.genericRepository<AspNetUserRoleAdmin>()
                .GetFilteredWithProjection(
                    filter: ur => ur.SysUserId == userId,
                    selector: ur => ur.RoleId
                ).ToListAsync();

            var permissionIds = await _unitOfWork.genericRepository<RolePermissionAdmin>()
                .GetFilteredWithProjection(
                    filter: rp => roleIds.Contains(rp.RoleId),
                    selector: rp => rp.PermissionAdminId
                ).ToListAsync();

            var menuItems = await _unitOfWork.genericRepository<MenuItem>()
                .GetFilteredWithProjection(
                    filter: mi => permissionIds.Contains(mi.ModuleId.Value), // Adjust based on your permission logic
                    selector: mi => new
                    {
                        mi.Id,
                        mi.Name,
                        mi.Url,
                        mi.IsVisible,
                        ModuleName = mi.Module.Name
                    },
                    includes: mi => mi.Module
                ).ToListAsync();

            return Ok(menuItems);
        }
 
        //[HttpGet]
        //[Route("GetUserMenuItems")]
        //public async Task<IActionResult> GetUserMenuItems()
        //{
        //    // Fetch user permissions from claims
        //    var userPermissions = User.Claims
        //        .Where(c => c.Type == "Permission")
        //        .Select(c => c.Value)
        //        .ToList();

        //    // Filter menu items based on permissions
        //    var menuItems = await _unitOfWork.genericRepository<MenuItem>()
        //        .GetAllAsync(mi => userPermissions.Contains($"{mi.ModuleId}_{mi.MenuItemId}_{mi.PermissionId}"));



        //    return Ok(menuItems);
        //}



    }
}
