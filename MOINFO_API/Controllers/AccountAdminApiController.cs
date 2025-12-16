using AutoMapper;
using Azure.Core;
using Business.Enums;
using Business.Helpers;
using Business.Interfaces;
using Business.ModelWithSpecification;
using Business.Repository;
using Business.ViewModel;
using Business.ViewModel.Account;
using Domain.Entities;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static Business.ViewModel.JwtClasses.JwtClasses;

namespace MOINFO_API.Controllers
{
    //[Authorize(AuthenticationSchemes = "SysUserJwt")]
    [Route("api/AccountAdminApi")]
    public class AccountAdminApiController : BaseController
    {
        private readonly IUnitOfwork _unitOfwork;
        private readonly EmailService _emailService;
        private readonly IMapper _mapper;
        private readonly UserManager<AspNetUser> _userManager;
        //private readonly SignInManager<AspNetUser> _signInManager;
        public IConfiguration Configuration { get; }
        public AccountAdminApiController(IUnitOfwork unitOfwork, EmailService emailService,
            IMapper mapper,IConfiguration configuration, UserManager<AspNetUser> userManager, SignInManager<AspNetUser> signInManager)
        {
            _unitOfwork = unitOfwork;
            _emailService = emailService;
            _mapper = mapper;
            Configuration = configuration;
           // _signInManager = signInManager;
            _userManager = userManager; 
        }
        #region EmployeeBackend
        #region SysUser
        private static readonly byte[] key = Encoding.UTF8.GetBytes("0123456789abcdef");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("1234567890abcdef");



        //private static string Encrypt(string plainText)
        //{
        //    using (Aes aesAlg = Aes.Create())
        //    {
        //        aesAlg.Key = key;
        //        aesAlg.IV = IV;

        //        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        //        using (MemoryStream msEncrypt = new MemoryStream())
        //        {
        //            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        //            {
        //                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
        //                {
        //                    swEncrypt.Write(plainText);
        //                }
        //            }
        //            return Convert.ToBase64String(msEncrypt.ToArray());
        //        }
        //    }
        //}
        private static string EncryptToBase64Url(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key; // Ensure key is 16, 24, or 32 bytes for AES
                aesAlg.IV = IV;   // Ensure IV is 16 bytes for AES

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    // Convert to Base64Url
                    byte[] encryptedBytes = msEncrypt.ToArray();
                    return ConvertToBase64Url(encryptedBytes);
                }
            }
        }

        // Helper Method for Base64Url Encoding
        private static string ConvertToBase64Url(byte[] input)
        {
            return Convert.ToBase64String(input) // Convert to Base64
                .TrimEnd('=')                    // Remove padding characters
                .Replace('+', '-')               // Replace '+' with '-'
                .Replace('/', '_');              // Replace '/' with '_'
        }
        private static string Decrypt(string cipherText)
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = IV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }

        //[HttpPost]
        //[Route("GetSysUserForLogin")]
        //public async Task<IActionResult> GetSysUserForLogin([FromBody] SysUserVM model)
        //{
        //	// Validate model
        //	if (!ModelState.IsValid)
        //	{
        //		return BadRequest(ModelState); // Return a bad request if the model is not valid
        //	}

        //	try
        //	{
        //		// Encrypt the password
        //		string PassWord = Encrypt(model.UserPasswordEncrypted);

        //		// Prepare the specification with encrypted password
        //		var Spec = new SysUserWithSpec(model.Username, PassWord, model?.Status);

        //		// Fetch data using the repository
        //		var result = await _unitOfwork.genericRepository<MoiEserviceSysUser>().GetByIdWithSpec(Spec);

        //		// Transform the result to the desired view model



        //		// Check if the result is null and return appropriate status
        //		if (result != null)
        //		{
        //			var resultlist = new SysUserVM
        //			{
        //			Username = result.Username,
        //			UserPasswordEncrypted = PassWord,
        //			ServiceId = result.ServiceId,
        //			Status = true
        //			};
        //			return Ok(resultlist);  // Return the result with HTTP 200 OK status
        //		}
        //		else
        //		{
        //			return NotFound("User not found.");  // Return 404 if user is not found
        //		}
        //	}
        //	catch (Exception ex)
        //	{
        //		// Log the exception and return a 500 error
        //		return StatusCode(500, $"Internal server error: {ex.Message}");
        //	}
        //}
        //[HttpPost]
        //[Route("GetSysUserForLogin")]
        //public async Task<IActionResult> GetSysUserForLogin([FromBody] SysUserVM model)
        //{
        //    // Validate model
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState); // Return a bad request if the model is not valid
        //    }

        //    try
        //    {
        //        // Encrypt the password
        //        string encryptedPassword = Encrypt(model.UserPasswordEncrypted);

        //        // Prepare the specification with encrypted password
        //        var Spec = new SysUserWithSpec(model.Username, encryptedPassword, model?.Status);

        //        // Fetch data using the repository
        //        var result = await _unitOfwork.genericRepository<MoiEserviceSysUser>().GetByIdWithSpec(Spec);

        //        if (result == null)
        //        {
        //            return NotFound("Invalid username or password."); // Return 404 if user is not found
        //        }

        //        // Fetch user roles and permissions
        //        //var role = await _unitOfwork.genericRepository<AspNetRole>().GetFirstOrDefaultAsync(r => r.RoleId == result.RoleId);
        //        //var rolePermissions = await _unitOfwork.genericRepository<RolePermission>()
        //        //    .GetAll(rp => rp.RoleId == result.RoleId);
        //        var RoleUser = await _unitOfwork.genericRepository<AspNetUserRole>()
        //                     .GetFilteredWithProjection(
        //                            filter:u=>u.UserId==result.UserId,
        //                            selector:r=>r.RoleId
        //                         ).ToListAsync();
        //        var Roles = await _unitOfwork.genericRepository<AspNetRole>()
        //                          .GetFilteredWithProjection(
        //                            filter:r=>RoleUser.Contains(r.Id),
        //                            selector:r=>r.Name
        //                             ).ToListAsync();

        //        var rolePermissions = await _unitOfwork.genericRepository<RolePermission>()
        //                              .GetFilteredWithProjection(
        //                                  filter:x=>RoleUser.Contains(x.RoleId),
        //                                  selector: c => new RolePermissionVM
        //                                  {
        //                                      PermissionId = c.PermissionId,
        //                                      MenuItemId = c.MenuItemId,
        //                                      ModuleId = c.ModuleId
        //                                  }
        //                                   ).ToListAsync();


        //        var claims = new List<Claim>
        //                        {
        //                            new Claim(ClaimTypes.Name, result.Username),
        //                            new Claim(ClaimTypes.NameIdentifier, result.UserId.ToString())
        //                        };

        //        // Add a separate claim for each role
        //        foreach (var roleName in Roles) // Assuming `result.Roles` is a collection of roles assigned to the user
        //        {
        //            claims.Add(new Claim(ClaimTypes.Role, roleName));
        //        }
        //        foreach (var permission in rolePermissions)
        //        {
        //            claims.Add(new Claim("Permission", $"{permission.ModuleId}_{permission.MenuItemId}_{permission.PermissionId}"));
        //        }

        //        // Create claims identity
        //        var claimsIdentity = new ClaimsIdentity(claims, "CustomAuth");
        //        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        //        // Sign in user
        //        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        //        //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
        //        foreach (var claim in claims)
        //        {
        //            Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
        //        }
        //        //Console.WriteLine(HttpContext.SignInAsync(claimsPrincipal));
        //        // Return user data for frontend
        //        var userVm = new SysUserVM
        //        {
        //            Username = result.Username,
        //            ServiceId = result.ServiceId,
        //            Status = true,
        //            //Claims = claims.Select(c => new { c.Type, c.Value }) // Return all claims

        //        };

        //        return Ok(userVm); // Return user details with HTTP 200 OK
        //        //return Ok(new
        //        //{
        //        //    User= result,
        //        //    Claims = claims.Select(c => new { c.Type, c.Value })
        //        //});
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception and return a 500 error
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}
        //[HttpPost]
        //[Route("GetSysUserForLogin")]
        //public async Task<IActionResult> GetSysUserForLogin([FromBody] SysUserVM model)
        //{
        //    // Validate model
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    try
        //    {
        //        // Encrypt the password
        //        string encryptedPassword = EncryptToBase64Url(model.UserPasswordEncrypted);

        //        // Prepare the specification with encrypted password
        //        var Spec = new SysUserWithSpec(model.Username,encryptedPassword, model?.Status);

        //        // Fetch user data
        //        var result = await _unitOfwork.genericRepository<MoiEserviceSysUser>().GetByIdWithSpec(Spec);

        //        if (result == null)
        //        {
        //            return NotFound("Invalid username or password.");
        //        }

        //        // Fetch user roles and permissions
        //        var RoleUser = await _unitOfwork.genericRepository<AspNetUserRole>()
        //            .GetFilteredWithProjection(
        //                filter: u => u.UserId == result.UserId,
        //                selector: r => r.RoleId
        //            ).ToListAsync();

        //        var Roles = await _unitOfwork.genericRepository<AspNetRole>()
        //            .GetFilteredWithProjection(
        //                filter: r => RoleUser.Contains(r.Id),
        //                selector: r => r.Name
        //            ).ToListAsync();

        //        var rolePermissions = await _unitOfwork.genericRepository<RolePermission>()
        //            .GetFilteredWithProjection(
        //                filter: x => RoleUser.Contains(x.RoleId),
        //                selector: c => new RolePermissionVM
        //                {
        //                    PermissionId = c.PermissionId,
        //                    MenuItemId = c.MenuItemId,
        //                    ModuleId = c.ModuleId
        //                }
        //            ).ToListAsync();

        //        // Generate claims
        //        var claims = new List<Claim>
        //                        {
        //                            new Claim(ClaimTypes.Email, result.Email),
        //                            new Claim(ClaimTypes.Name, result.Name),

        //                            //new Claim(ClaimTypes., Guid.NewGuid().ToString())
        //                        };

        //        foreach (var roleName in Roles)
        //        {
        //            claims.Add(new Claim(ClaimTypes.Role, roleName));
        //        }

        //        //foreach (var permission in rolePermissions)
        //        //{
        //        //    claims.Add(new Claim("Permission", $"{permission.ModuleId}_{permission.MenuItemId}_{permission.PermissionId}"));
        //        //}

        //        // Generate JWT token
        //        var token = GenerateJwtToken(claims);

        //        // Return token and user data
        //        return Ok(new
        //        {
        //            Token = token,
        //            User = new
        //            {
        //                Username = result.Username,
        //                ServiceId = result.ServiceId,
        //                Status = true
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}

        // Helper: Generate JWT Token
        //private string GenerateJwtToken(List<Claim> claims)
        //{


        //    var authkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:Key"]));
        //    var creds = new SigningCredentials(authkey, SecurityAlgorithms.HmacSha256Signature);
        //    //var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
        //    //var claimsIdentity = new ClaimsIdentity(claims);
        //    var token = new JwtSecurityToken(
        //        issuer: Configuration["JwtSettings:ValidIssuer"],          
        //        audience: Configuration["JwtSettings:ValidAudience"],
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(double.Parse(Configuration["JwtSettings:DurationInDays"])),
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
        [HttpPost]
        [Route("GetSysUserForLogin")]
        public async Task<IActionResult> GetSysUserForLogin([FromBody] SysUserVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Encrypt password
                string encryptedPassword = EncryptToBase64Url(model.UserPasswordEncrypted);

                // Query the database for the user
                var spec = new SysUserWithSpec(model.Username, encryptedPassword, model?.Status);
                var user = await _unitOfwork.genericRepository<MoiEserviceSysUser>().GetByIdWithSpec(spec);

                if (user == null)
                {
                    return NotFound("Invalid username or password.");
                }

                // Fetch roles and permissions
                var roleIds = await _unitOfwork.genericRepository<AspNetUserRoleAdmin>()
                    .GetFilteredWithProjection(u => u.SysUserId == user.SysUserId, r => r.RoleId)
                    .ToListAsync();

                var roles = await _unitOfwork.genericRepository<RoleAdmin>()
                    .GetFilteredWithProjection(r => roleIds.Contains(r.Id), r => r.Name)
                    .ToListAsync();

                var permissions = await _unitOfwork.genericRepository<RolePermissionAdmin>()
                    .GetFilteredWithProjection(p => roleIds.Contains(p.RoleId), p => new RolePermissionVM
                    {
                        PermissionId = p.PermissionAdminId,
                        MenuItemId = p.MenuItemId,
                        ModuleId = p.ModuleId
                    })
                    .ToListAsync();

                // Generate claims
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.UserData,user.SysUserId.ToString()),
            new Claim("ServiceId",user.ServiceId.ToString())
        };

                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
                claims.AddRange(permissions.Select(p =>
                    new Claim("Permission", $"{p.ModuleId}_{p.MenuItemId}_{p.PermissionId}")));
                var claimsIdentity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                
                // Generate JWT token
                var token = GenerateJwtToken(claims);
                Console.WriteLine("Generated Token: " + token);

                // Return token and user data
                return Ok(new
                {
                    //Token = $"Bearer {token}",
                    Token=token,
                    User = new
                    {
                        Username = user.Username,
                        ServiceId = user.ServiceId,
                        Status = true
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private string GenerateJwtToken(IEnumerable<Claim> claims)
        {
            var key = Configuration["JwtSettings:Key"];
            var issuer = Configuration["JwtSettings:ValidIssuer"];
            var audience = Configuration["JwtSettings:ValidAudience"];
            var durationInDays = int.Parse(Configuration["JwtSettings:DurationInDays"]);

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(durationInDays),
                signingCredentials: signingCredentials
            );

            var handler = new JwtSecurityTokenHandler();

            // Ensure token is valid Base64Url
            string jwt = handler.WriteToken(token);

            return jwt;
        }
        [HttpGet]
        [Route("VerifyToken")]
        public IActionResult VerifyToken()
        {
            var token = HttpContext.Session.GetString("AdminToken");

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing.");
            }

            // Simulate token validation or call your API to verify
            var isTokenValid = true; // Replace with actual token validation logic
            return isTokenValid ? Ok() : Unauthorized();
        }

        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {

            var user =await _unitOfwork.genericRepository<MoiEserviceSysUser>().GetbyId(model.Id);
            // Encrypt password
            string encryptedPassword = EncryptToBase64Url(model.NewPassword);
            user.UserPasswordEncrypted = encryptedPassword;
            user.Password = model.NewPassword;
            _unitOfwork.genericRepository<MoiEserviceSysUser>().Update(user);
            _unitOfwork.Complete();

            return Ok();
        }
        #endregion

        #region GetStatistics
        //--------------------Get All Statistics----------
        [HttpGet]
        [Route("GetAllStatistics")]
        public async Task<StatisticsViewModel> GetAllStatistics(int ServiceId)
        {
            var reqRepo = _unitOfwork.genericRepository<MoiEserviceLicensesRequest>();
            var transRepo = _unitOfwork.genericRepository<Transaction>();
            var licRepo = _unitOfwork.genericRepository<Licence>();

            var model = new StatisticsViewModel
            {
                PreApprovementConvert = await reqRepo.Count(p => p.ReqtypeId == (int)RequestTypeEnum.PreApprovementConvert && p.ServiceId == ServiceId),
                PreApprovementNew = await reqRepo.Count(p => p.ReqtypeId == (int)RequestTypeEnum.PreApprovementNew && p.ServiceId == ServiceId),
                WhoConc = await reqRepo.Count(p => p.ReqtypeId == (int)RequestTypeEnum.WhoConc && p.ServiceId == ServiceId),
                AllLicences = await licRepo.Count(p => p.LicStatusId == (int)licencesStatusEnum.Released && p.ServiceId == ServiceId),
                AllRequests = await reqRepo.Count(r => r.ServiceId == ServiceId),
                NewRequests = await reqRepo.Count(r => r.RequestStatusId == (int)RequestStatusEnum.Received && r.ServiceId == ServiceId),
                ChangeRequest = await reqRepo.Count(p => p.ReqtypeId == (int)RequestTypeEnum.ChangeData && p.ServiceId == ServiceId),
                ChangeCompanyName = await transRepo.Count(c => c.TransTypeId == (int)TransactionTypesEnum.ChangeCompaneName && c.ServiceId == ServiceId),
                ChangeAddress = await transRepo.Count(c => c.TransTypeId == (int)TransactionTypesEnum.ChangeAddress && c.ServiceId == ServiceId),
                ChangeOwnerRequest = await reqRepo.Count(c => c.ReqtypeId == (int)RequestTypeEnum.Renouncement && c.ServiceId == ServiceId),
                EndLicenseRequests = await reqRepo.Count(c => c.ReqtypeId == (int)RequestTypeEnum.EndLicences && c.ServiceId == ServiceId),
                RenewRequests = await reqRepo.Count(c => c.ReqtypeId == (int)RequestTypeEnum.Renew && c.ServiceId == ServiceId),
                ChangeManagerRequests = await transRepo.Count(c => c.TransTypeId == (int)TransactionTypesEnum.ChangeManager && c.ServiceId == ServiceId),
                ChangeActivityRequests = await transRepo.Count(c => c.TransTypeId == (int)TransactionTypesEnum.ChangeActivity && c.ServiceId == ServiceId),
                // You can add more below, or uncomment if needed
                ReplacementOfLostRequests = await reqRepo.Count(r => r.ReqtypeId == (int)RequestTypeEnum.ReplacementOfLost && r.ServiceId == ServiceId),
            };

            return model;
        }

        #endregion
        #region GetAllRequest
        [HttpGet]
        [Route("GetAllRequest")]
        public async Task<IEnumerable<RequestVM>> GetAllRequest(int ServiceId)
        {


            var specification = new RequestWithSpecificService(ServiceId, false);
            var allRequestTask = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().GetTableWithSpecService(specification);

            var DataMapped = _mapper.Map<IEnumerable<RequestVM>>(allRequestTask.AsEnumerable());
            //return _mapper.Map<IEnumerable<MoiEserviceLicensesRequest>, IEnumerable<RequestVM>>(allRequestTask);
            return DataMapped;
        }
        #endregion
        #region GetAllLicences
        [HttpGet]
        [Route("GetAllLicences")]
        public async Task<IEnumerable<LicencesVM>> GetAllLicences(int ServiceId)
        {
            var specification = new LicencesWithSpecificService(ServiceId, false);
            var licence = await _unitOfwork.genericRepository<Licence>().GetTableWithSpecService(specification);

            return _mapper.Map<IEnumerable<Licence>, IEnumerable<LicencesVM>>(licence.AsEnumerable());
        }
        #endregion
        #region Licences By specific id

        [HttpGet]
        [Route("GetLicencesById")]
        public async Task<dynamic> GetLicencesById(int id, int serviceId)
        {
            try
            {
                var Spec = new LicencesWithSpecificService(id, serviceId);

                var licence = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(Spec);

                var licenceMapped = _mapper.Map<Licence, LicencesVM>(licence);

                if (licenceMapped != null)
                {

                    var requests = new RequestWithSpecificService(id, serviceId, true);
                    var RequestLicences = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().GetTableWithSpec(requests);
                    var requestMapped = _mapper.Map<IEnumerable<MoiEserviceLicensesRequest>, IEnumerable<RequestVM>>(RequestLicences);


                    //var Applicant = new PersonApplicantWithSpec(licenceMapped.ApplicantCivilId, serviceId);
                    var ApplicantRepo = await _unitOfwork.genericRepository<Person>().GetByCondition(x=>x.CivilId== licenceMapped.ApplicantCivilId).FirstOrDefaultAsync();
                    var applicantMapped = _mapper.Map<Person, PersonVM>(ApplicantRepo);
                    int managerId = licenceMapped.ManagerId ?? 0; // Default to 0 if null

                    var ManagerPerson = new ManagerApplicantWithSpec(managerId, serviceId);
                    var manager = await _unitOfwork.genericRepository<Person>().GetByIdWithSpec(ManagerPerson);
                    var managerMapped = _mapper.Map<Person, PersonVM>(manager);
                    var allAttachments = new List<AttachVM>();

                    foreach (var item in requestMapped)
                    {
                        long RequestAttachId = item.RequestId;

                        var attachPerrequest = new AttachmentWithSpec(RequestAttachId, serviceId);
                        var AttachRepo = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>().GetTableWithSpec(attachPerrequest);
                        var attachMapped = _mapper.Map<IEnumerable<MoiEserviceRequestsAttach>, IEnumerable<AttachVM>>(AttachRepo);
                        allAttachments.AddRange(attachMapped);

                    }
                    //var PreApprove = new PreApprovementWithSpec(requestMapped.RequestId);
                    //var preApprovementRepo = await _unitOfwork.genericRepository<TourMoiEserviceTourismPreApprovement>().GetByIdWithSpec(PreApprove);
                    //var preApprovementMapped = _mapper.Map<TourMoiEserviceTourismPreApprovement, PreApprovementVM>(preApprovementRepo);

                    var PartnerPerrequest = new PartnerWithSpec(licenceMapped.LicId, serviceId);
                    var PartnerRepo = await _unitOfwork.genericRepository<Partner>().GetTableWithSpec(PartnerPerrequest);
                    var PartnerMapped = _mapper.Map<IEnumerable<Partner>, IEnumerable<PartnerVM>>(PartnerRepo);




                    int companyid = licenceMapped.CompanyId ?? 0;
                    var Company = new CompanyWithSpec(companyid, serviceId);
                    var companyRepo = await _unitOfwork.genericRepository<Company>().GetByIdWithSpec(Company);
                    var companyMapped = _mapper.Map<Company, CompanyVM>(companyRepo);


                    //You can return both the request and transactions together
                    return new LicenceDetailsVM
                    {
                        RequestsDVM = requestMapped,
                        PersonApplicantVM = applicantMapped,
                        LicencesVM = licenceMapped,
                        ManagerPersonVM = managerMapped,
                        attachmentVM = allAttachments,
                        PartnerVM = PartnerMapped,
                        CompanyVM = companyMapped,
                    };
                }
                else
                {
                    return new ErrorMessage()
                    {
                        Error = true,
                        Status = "Failure",
                        Message = "No data Found",
                    };

                }
            }
            catch (Exception ex)
            {


                //LogManager.Instance.AddErrorLog(ex);
                return new ErrorMessage()
                {
                    Error = true,
                    Status = "Failure",
                    Message = ex.Message,
                };
            }

        }
        #endregion
        #region Request By specific id

        [HttpGet]
        [Route("GetRequestById")]
        public async Task<dynamic> GetRequestById(int id, int serviceId)
        {
            try
            {
                var Spec = new RequestWithSpecificService(id, serviceId, false);

                var request = await _unitOfwork.genericRepository<MoiEserviceLicensesRequest>().GetByIdWithSpec(Spec);

                var requestMapped = _mapper.Map<MoiEserviceLicensesRequest, RequestVM>(request);

                if (request != null)
                {
                    var TransSpec = new TransactionWithSpec(id);
                    // Retrieve the transactions that match the RequestId
                    var transactions = await _unitOfwork.genericRepository<Transaction>()
                                                        .GetByIdWithSpec(TransSpec);

                    var transactionMapped = _mapper.Map<Transaction, TransactionVM>(transactions);
                    var PaymentSpec = new PaymentDetailsWithSpec(id, serviceId);
                    var payment = await _unitOfwork.genericRepository<MoiEserviceRequestPaymentDetail>().GetByIdWithSpec(PaymentSpec);
                    var paymentMapped = _mapper.Map<MoiEserviceRequestPaymentDetail, PaymentDetailsVM>(payment);
                    int licencesId = requestMapped.LicenseId ?? 0;
                    var licences = new LicencesWithSpecificService(licencesId, serviceId);
                    var licence = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licences);
                    var licenceMapped = _mapper.Map<Licence, LicencesVM>(licence);

                   // var Applicant = new PersonApplicantWithSpec(licenceMapped.ApplicantCivilId, serviceId);
                    var ApplicantRepo = await _unitOfwork.genericRepository<Person>().GetByCondition(x=>x.CivilId==requestMapped.AppCivilId).FirstOrDefaultAsync();
                    var applicantMapped = _mapper.Map<Person, PersonVM>(ApplicantRepo);
                    int managerId = requestMapped.ManagerId ?? 0; // Default to 0 if null

                    var ManagerPerson = new ManagerApplicantWithSpec(managerId, serviceId);
                    var manager = await _unitOfwork.genericRepository<Person>().GetByIdWithSpec(ManagerPerson);
                    var managerMapped = _mapper.Map<Person, PersonVM>(manager);
                    long RequestAttachId = requestMapped.RequestId;

                    var attachPerrequest = new AttachmentWithSpec(RequestAttachId, serviceId);
                    var AttachRepo = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>().GetTableWithSpec(attachPerrequest);
                    var attachMapped = _mapper.Map<IEnumerable<MoiEserviceRequestsAttach>, IEnumerable<AttachVM>>(AttachRepo);
                    if (requestMapped.ReqtypeId == 5)
                    {
                        int licId = requestMapped.LicenseId ?? 0;
                        var PreApprove = new PreApprovementWithSpec(licId, true);
                        var preApprovementRepo = await _unitOfwork.genericRepository<MoiPreApprovement>().GetByIdWithSpec(PreApprove);
                        var preApprovementMapped = _mapper.Map<MoiPreApprovement, PreApprovementVM>(preApprovementRepo);
                    }
                    var PartnerPerrequest = new PartnerWithSpec(licenceMapped.LicId, serviceId);
                    var PartnerRepo = await _unitOfwork.genericRepository<Partner>().GetTableWithSpec(PartnerPerrequest);
                    var PartnerMapped = _mapper.Map<IEnumerable<Partner>, IEnumerable<PartnerVM>>(PartnerRepo);


                    var ChangeActivity = new ActivityChangeTransWithSpec(serviceId, requestMapped.RequestId);
                    var ActivityChangeRepo = await _unitOfwork.genericRepository<ActivityChangeTypeTransaction>().GetByIdWithSpec(ChangeActivity);
                    var ActivityChangeMapped = _mapper.Map<ActivityChangeTypeTransaction, ActivityChangeTransVM>(ActivityChangeRepo);

                    var ChaneManager = new ManagerChangeTransWithSpec(serviceId, requestMapped.RequestId);
                    var ChaneManagerRepo = await _unitOfwork.genericRepository<TchangeManager>().GetByIdWithSpec(ChaneManager);
                    var ChaneManagerMapped = _mapper.Map<TchangeManager, ChangeManagerTransVM>(ChaneManagerRepo);

                    var changeAddress = new AddressChangeTransWithSpec(serviceId, requestMapped.RequestId);
                    var changeAddressRepo = await _unitOfwork.genericRepository<AddressChangeTransaction>().GetByIdWithSpec(changeAddress);
                    var changeAddressMapped = _mapper.Map<AddressChangeTransaction, AddressChangeTransVM>(changeAddressRepo);


                    var changeEmail = new EmailChangeTransWithSpec(serviceId, requestMapped.RequestId);
                    var changeEmailRepo = await _unitOfwork.genericRepository<ChangeEmailTranaction>().GetByIdWithSpec(changeEmail);
                    var changeEmailMapped = _mapper.Map<ChangeEmailTranaction, EmailChangeTransVM>(changeEmailRepo);

                    var changePartner = new PartnerChangeTransWithSpec(serviceId, requestMapped.RequestId);
                    var changePartnerRepo = await _unitOfwork.genericRepository<PartnerOldChangeTransaction>().GetByIdWithSpec(changePartner);
                    var changePartnerMapped = _mapper.Map<PartnerOldChangeTransaction, ChangeNewPartnerTransVM>(changePartnerRepo);

                    var changeMadia = new MediaNameChangeTransWithSpec(requestMapped.RequestId);
                    var changeMediaRepo = await _unitOfwork.genericRepository<ChangeMediaNameTransaction>().GetByIdWithSpec(changeMadia);
                    var changeMediaMapped = _mapper.Map<ChangeMediaNameTransaction, MediaChangeTransVM>(changeMediaRepo);


                    int companyid = licenceMapped.CompanyId ?? 0;
                    var Company = new CompanyWithSpec(companyid, serviceId);
                    var companyRepo = await _unitOfwork.genericRepository<Company>().GetByIdWithSpec(Company);
                    var companyMapped = _mapper.Map<Company, CompanyVM>(companyRepo);


                    var changeSocialMedia = new SocialMediaChangeTransWithSpec(requestMapped.RequestId);
                    var changeSocialMediaRepo = await _unitOfwork.genericRepository<ChangeSocialMediaTransaction>().GetByIdWithSpec(changeSocialMedia);
                    var changeSocialMediaMapped = _mapper.Map<ChangeSocialMediaTransaction, ChangeSocialMediaTransVM>(changeSocialMediaRepo);

                    var changeOwner = new OwnerChangeTransWithSpec(serviceId, requestMapped.RequestId);
                    var changeOwnerRepo = await _unitOfwork.genericRepository<RenouncementTransaction>().GetByIdWithSpec(changeOwner);
                    var changeownerMapped = _mapper.Map<RenouncementTransaction, ChangeOwnerTransVM>(changeOwnerRepo);

                    var changeCommercialName = new CommercialChangeTransWithSpec(serviceId, requestMapped.RequestId);
                    var changeCommercialRepo = await _unitOfwork.genericRepository<CommercialNameChangeTransaction>().GetByIdWithSpec(changeCommercialName);
                    var changeCommercialMapped = _mapper.Map<CommercialNameChangeTransaction, CommercialTransVM>(changeCommercialRepo);

                    var changeCompanyName = new CompanyChangeTransWithSpec(serviceId, requestMapped.RequestId);
                    var changeCompanyRepo = await _unitOfwork.genericRepository<CompanyNameChangeTransaction>().GetByIdWithSpec(changeCompanyName);
                    var changeCompanyMapped = _mapper.Map<CompanyNameChangeTransaction, CompanyTransVM>(changeCompanyRepo);


                    var Renew = new RenewWithSpec(licencesId, serviceId);
                    var RenewRepo = await _unitOfwork.genericRepository<LicenseRenew>().GetByIdWithSpec(Renew);
                    var RenewMapped = _mapper.Map<LicenseRenew, RenewVM>(RenewRepo);
                    //var Mandoob = await FetchMandoobDataAsync(requestMapped.LicenseId);
                    //You can return both the request and transactions together
                    return new RequestDetailsVM
                    {
                        RequestDVM = requestMapped,
                        TransactionsVM = transactionMapped,
                        PaymentDetailsVM = paymentMapped,
                        PersonApplicantVM = applicantMapped,
                        LicencesVM = licenceMapped,
                        ManagerPersonVM = managerMapped,
                        attachmentVM = attachMapped,
                        ActivityChangeTransVM = ActivityChangeMapped,
                        PartnerVM = PartnerMapped,
                        ChangePartnerTransVM = changePartnerMapped,
                        EmailChangeTransVM = changeEmailMapped,
                        AddressChangeTransVM = changeAddressMapped,
                        ManagerChangeTransVM = ChaneManagerMapped,
                        ChangeSocialMediaTransVM = changeSocialMediaMapped,
                        MediaChangeTransVM = changeMediaMapped,
                        OwnerChangeTransVM = changeownerMapped,
                        CommercialTransVM = changeCommercialMapped,
                        CompanyTransVM = changeCompanyMapped,
                        LicenceRenewVM = RenewMapped,
                        CompanyVM = companyMapped,


                    };
                }
                else
                {
                    return new ErrorMessage()
                    {
                        Error = true,
                        Status = "Failure",
                        Message = "No data Found",
                    };

                }
            }
            catch (Exception ex)
            {


                //LogManager.Instance.AddErrorLog(ex);
                return new ErrorMessage()
                {
                    Error = true,
                    Status = "Failure",
                    Message = ex.Message,
                };
            }

        }
        #endregion
        #region
        [HttpGet]
        [Route("GetUserWithPermission")]
        public async Task<IEnumerable<SysUserVM>> GetUserWithPermission()
        {

            var specification = new SysUserWithSpec();
            var allRequestTask = await _unitOfwork.genericRepository<MoiEserviceSysUser>().GetTableWithSpecService(specification);

            var DataMapped = _mapper.Map<IEnumerable<SysUserVM>>(allRequestTask.AsEnumerable());
            //return _mapper.Map<IEnumerable<MoiEserviceLicensesRequest>, IEnumerable<RequestVM>>(allRequestTask);
            return DataMapped;
        }
        [HttpGet]
        [Route("GetRoleWithPermission")]
        public async Task<IEnumerable<RolePermissionVM>> GetRoleWithPermission()
        {

            var specification = new RolePermissionWithSpec();
            var allRequestTask = await _unitOfwork.genericRepository<RolePermissionAdmin>().GetTableWithSpecService(specification);

            var DataMapped = _mapper.Map<IEnumerable<RolePermissionVM>>(allRequestTask.AsEnumerable());
            //return _mapper.Map<IEnumerable<MoiEserviceLicensesRequest>, IEnumerable<RequestVM>>(allRequestTask);
            return DataMapped;
        }
        #endregion
        #region User Register

        private ErrorMessage CheckExist(string Email, string CivilId)
        {

            var checkCivilandEmail = _unitOfwork.genericRepository<AspNetUser>().GetByCondition(m => m.CivilId == CivilId && m.Email == Email);
            if (checkCivilandEmail.Count() == 0)
            {
                var checkcivil = _unitOfwork.genericRepository<AspNetUser>().GetByCondition(m => m.CivilId == CivilId);
                if (checkcivil.Count() == 0)
                {


                    var checkEmail = _unitOfwork.genericRepository<AspNetUser>().GetByCondition(m => m.Email == Email);


                    if (checkEmail.Count() == 0)
                    {
                        return new ErrorMessage()
                        {
                            Error = false,
                            Status = "Success",
                            Message = "No data found",
                        };

                    }
                    else
                    {
                        return new ErrorMessage()
                        {
                            Error = true,
                            Status = "Fail",
                            Message = "This email address is already being used",
                        };

                    }

                }
                else
                {
                    return new ErrorMessage()
                    {
                        Error = true,
                        Status = "Fail",
                        Message = "This Civil ID is already being used",
                    };

                }

            }
            else
            {
                return new ErrorMessage()
                {
                    Error = true,
                    Status = "Fail",
                    Message = "This email address and Civil ID  is already being used",
                };


            }
        }

        [HttpGet]
        [Route("ResetPasswordUserInAdmin")]
        public async Task<IActionResult> ResetPasswordUserInAdmin(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
              

                    var RequestResetPassword = await _unitOfwork.genericRepository<ResetUserPassword>()
                                      .GetByCondition(r => r.Id==id).FirstOrDefaultAsync();
                var contactMapped = _mapper.Map<ResetUserPassword, ResetUserPasswordVM>(RequestResetPassword);

                return Ok(contactMapped);
               
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [HttpGet]
        [Route("AllResetPasswordUserInAdmin")]
        public async Task<IActionResult> AllResetPasswordUserInAdmin()
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                var RequestResetPassword = await _unitOfwork.genericRepository<ResetUserPassword>()
                                  .GetByCondition(r => r.Executed==false).ToListAsync();
                var contactMapped = _mapper.Map<List<ResetUserPassword>, List<ResetUserPasswordVM>>(RequestResetPassword);

                return Ok(contactMapped);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        //    [HttpPost("ExecuteResetPassword")]
        //    public async Task<IActionResult> ExecuteResetPassword([FromBody] ResetUserPasswordVM model)
        //    {
        //        if (string.IsNullOrEmpty(model.UserEmail))
        //            return BadRequest("User email is required.");

        //        var entity = await _unitOfwork.genericRepository<ResetUserPassword>().GetbyId(model.Id);
        //        if (entity == null)
        //            return NotFound("Request not found.");

        //        // Update entity with execution details
        //        entity.Executed = true;
        //        entity.ExecutedOn = DateTime.UtcNow;
        //        entity.Note = model.Note;
        //        entity.Status = model.Status; // "Accepted" or "Rejected"
        //        await _unitOfwork.genericRepository<ResetUserPassword>().UpdateAsync(entity);
        //        await _unitOfwork.Complete();

        //        // Prepare email message
        //        string subject = model.Status == false
        //            ? "تم تنفيذ طلب إعادة تعيين كلمة السر"
        //            : "تم رفض طلب إعادة تعيين كلمة السر";

        //        var placeholders = new Dictionary<string, string>
        //{
        //    {"{{CivilID}}", model.UserCivilID},
        //    {"{{Mobile}}", model.Mobile},
        //    {"{{NewPassword}}", model.UserNewPass ?? "----"},
        //    {"{{Note}}", model.Note ?? ""}
        //};

        //        string templatePath = model.Status == true
        //            ? "templates/reset-password.html"
        //            : "templates/reset-password-rejected.html";

        //        var body = _emailService.PrepareEmailBody(Path.Combine(Directory.GetCurrentDirectory(), templatePath), placeholders);

        //        var emailSent = await _emailService.SendEmail(model.UserEmail, subject, body);

        //        return Ok(new { message = "Request processed and email sent.", status = model.Status });
        //    }
        [Route("ExecuteResetPassword")]
        [HttpPost]
        public async Task<IActionResult> ExecuteResetPassword(ResetUserPasswordVM model)
        {
            try
            {
                //if (!ModelState.IsValid)
                //    return BadRequest(ModelState);

                var userExist = await _unitOfwork.genericRepository<AspNetUser>()
                    .GetByCondition(a => a.CivilId == model.UserCivilID).FirstOrDefaultAsync();

                if (userExist == null)
                    return Ok(new { message = "لا يوجد مستخدم مسجل بهذا الرقم المدني" });

                //var existingResetRequest = await _unitOfwork.genericRepository<ResetUserPassword>()
                //    .GetByCondition(r => r.UserCivilID == model.UserCivilID && r.Executed == false)
                //    .FirstOrDefaultAsync();

                //if (existingResetRequest != null)
                //    return Ok(new { message = "يوجد طلب تغيير كلمة السر قيد التنفيذ" });

                // Reset the password using Identity
                string token = await _userManager.GeneratePasswordResetTokenAsync(userExist);
                var resetResult = await _userManager.ResetPasswordAsync(userExist, token, model.UserNewPass);

                if (!resetResult.Succeeded)
                {
                    var errors = resetResult.Errors.Select(e => e.Description).ToList();
                    return BadRequest(new { message = "فشل في تحديث كلمة السر", errors });
                }
                var resetRequest = await _unitOfwork.genericRepository<ResetUserPassword>()
                    .GetByCondition(r => r.Id == model.Id).FirstOrDefaultAsync();
                // Save reset request record

                //resetRequest.UserCivilID = model.UserCivilID;
                //resetRequest.UserEmail = model.UserEmail;
                //resetRequest.Mobile = model.Mobile;
                resetRequest.ProcessedBy = model.ProcessedBy;
                resetRequest.ProcessedByName = model.ProcessedByName;
                resetRequest.Note = model.Note;
                //resetRequest.UserNewPass = model.UserNewPass;
                ///resetRequest.FilePath = model.FilePath;
                    //resetRequest.DateAdded = DateTime.Now;
                resetRequest.Executed = true;
                resetRequest.ExecutedOn = DateTime.UtcNow;
                resetRequest.Status = true;
               

                await _unitOfwork.genericRepository<ResetUserPassword>().Update(resetRequest);
                await _unitOfwork.Complete();
                //string decrypted = EncryptionHelper.Decrypt(encryptedPassword);
                // Prepare email message
                string subject = "تم تنفيذ طلب إعادة تعيين كلمة السر";

                var placeholders = new Dictionary<string, string>
{
    {"{{CivilID}}", model.UserCivilID},
    {"{{Mobile}}", model.Mobile},
    //{"{{NewPassword}}", model.UserNewPass}, // or hide it for security: "----"
    {"{{Note}}", model.Note ?? ""}
};

                string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "templates/reset-password.html");
                string body = _emailService.PrepareEmailBody(templatePath, placeholders);
                bool emailSent = await _emailService.SendEmail(model.UserEmail, subject, body);

                return Ok(true);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"حدث خطأ في الخادم: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("AllContactUsUserInAdmin")]
        public async Task<IActionResult> AllContactUsUserInAdmin()
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                var RequestCotactUs = await _unitOfwork.genericRepository<ContactUs>()
                                  .GetByCondition(c=>c.IsReplayed==false).ToListAsync();
                var contactMapped = _mapper.Map<List<ContactUs>, List<ContactUsVM>>(RequestCotactUs);

                return Ok(contactMapped);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet]
        [Route("AllContactUsUserInAdminById")]
        public async Task<IActionResult> AllContactUsUserInAdminById(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);


                var RequestResetPassword = await _unitOfwork.genericRepository<ContactUs>()
                                  .GetByCondition(c => c.Id == id).FirstOrDefaultAsync();
                var contactMapped=_mapper.Map<ContactUs,ContactUsVM>(RequestResetPassword);
                return Ok(contactMapped);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPost("SendContactReply")]
        public async Task<IActionResult> SendContactReply([FromBody] ContactReplyVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = await _unitOfwork.genericRepository<ContactUs>().GetByCondition(x => x.Id == model.Id).FirstOrDefaultAsync();
            if (entity == null)
                return NotFound();

            entity.Note = model.Note;
            entity.IsReplayed = true;
            await _unitOfwork.Complete();

            var placeholders = new Dictionary<string, string>
    {
        {"{{FullName}}", model.FullNameAr },
        {"{{Note}}", model.Note }
    };

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "templates/email-reply.html");
            var body = _emailService.PrepareEmailBody(templatePath, placeholders);

            var result = await _emailService.SendEmail(model.Email, "الرد على طلب التواصل", body);

            return Ok(result);
        }
        //[HttpPost]
        //[Route("Register")]
        //public async Task<IActionResult> Register(RegisterBindingModel model)
        //{
        //    try
        //    {

        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //        bool passwordChanged = true;
        //        // Check if user already exists by CivilID
        //        var existingUserByCivilID = await _userManager.FindByNameAsync(model.CivilID);
        //        if (existingUserByCivilID != null)
        //        {
        //            return BadRequest("A user with this CivilID already exists.");
        //        }

        //        // Check if user already exists by Email
        //        var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
        //        if (existingUserByEmail != null)
        //        {
        //            return BadRequest("A user with this email already exists.");
        //        }


        //        var user = new AspNetUser()
        //        {
        //            UserName = model.CivilID,
        //            CivilId = model.CivilID,
        //            Email = model.Email,
        //            EmailConfirmed = true,
        //            PhoneNumber = model.Mobile,
        //            PhoneNumberConfirmed = true,
        //            Mobile = model.Mobile,
        //            FullNameAr = model.FullNameAr,
        //            FullNameEn = model.FullNameEn,
        //            //OldPassword = model.OldPassword,
        //           // PasswordChanged = passwordChanged,
        //            AccountTypeId = 100
        //        };

        //      IdentityResult result = await _userManager.CreateAsync(user, model.Password);

        //        if (result.Succeeded)
        //        {
        //            return Ok();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //    return BadRequest();

        //}
        //[HttpPost]
        //[Route("Login")]
        //public async Task<IActionResult> LoginUser(LoginBindingModel model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        // Check if user already exists by CivilID
        //        var existingUserByCivilID = await _userManager.FindByNameAsync(model.CivilID);
        //        if (existingUserByCivilID == null)
        //        {
        //            return BadRequest("Invalid CivilID or password.");
        //        }

        //        // Check if the password is correct
        //        var isPasswordValid = await _userManager.CheckPasswordAsync(existingUserByCivilID, model.Password);
        //        if (!isPasswordValid)
        //        {
        //            return BadRequest("Invalid CivilID or password.");
        //        }

        //        // Generate the JWT token for valid users
        //        var claims = new List<Claim>
        //                    {
        //                        new Claim(ClaimTypes.Name, existingUserByCivilID.UserName),
        //                        new Claim(ClaimTypes.Email, existingUserByCivilID.Email),
        //                        new Claim(ClaimTypes.UserData, existingUserByCivilID.Id)
        //                        // Add any other claims if necessary
        //                    };

        //        var token = GenerateJwtToken(claims);

        //        // Store the token in the session (or pass it to the client)
        //        HttpContext.Session.SetString("Token", token);

        //        // Return the token and user info
        //        return Ok(new
        //        {
        //            Token = token,
        //            User = new
        //            {
        //                Username = existingUserByCivilID.UserName,
        //                Email = existingUserByCivilID.Email
        //                // Add any other user information if needed
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}
        #endregion
        #endregion
        #region Admin Delegate

        [HttpGet]
        [Route("GetPendingDelegations")]
        public async Task<IActionResult> GetPendingDelegations()
        {

            var pendingSpec = new AspNetMultipleLicencesUserWithSpec(false,false);
            var pending = await _unitOfwork.genericRepository<AspNetMultipleLicenseUser>()
                           .GetTableWithSpec(pendingSpec);

            var result = pending.Select(x => new PendingDelegationVM
            {
                Id = x.Id,
                IsApproved=x.IsApproved,
                LicenseName = x.Licence?.LicName,
                DelegateName = x.AspNetMultipleUser?.Mandoob?.FullNameAr,
                MainUserName = x.AspNetMultipleUser?.User?.FullNameAr,
                AttachmentUrl=x.AttachmentUrl
            });

            return Ok(result);
        }

        [HttpGet]
        [Route("GetDelegationById")]
        public async Task<IActionResult> GetDelegationById(int id)
        {

            var pendingSpec = new AspNetMultipleLicencesUserWithSpec(id);
            var pending = await _unitOfwork.genericRepository<AspNetMultipleLicenseUser>()
                           .GetByIdWithSpec(pendingSpec);

            var result =  new PendingDelegationVM
            {
                Id = pending.Id,
                IsApproved = pending.IsApproved,
                LicenseName = pending.Licence?.LicName,
                DelegateName = pending.AspNetMultipleUser?.Mandoob?.FullNameAr,
                MainUserName = pending.AspNetMultipleUser?.User?.FullNameAr,
                AttachmentUrl = pending.AttachmentUrl
            };

            return Ok(result);
        }

        [HttpPost("ApproveMandoobDelegation")]
        public async Task<IActionResult> ApproveMandoobDelegation([FromBody] PendingDelegationVM data)
        {
            
           

            var record = await _unitOfwork.genericRepository<AspNetMultipleLicenseUser>()
                .GetByCondition(x=>x.Id==data.Id).FirstOrDefaultAsync();

            if (record == null)
            {
                return NotFound(new ErrorMessage
                {
                    Error = true,
                    Status = "404",
                    Message = "السجل غير موجود"
                });
            }

            record.IsApproved = data.IsApproved;
            record.Note = data.Note;
            record.IsConfirmed = true;
            await _unitOfwork.genericRepository<AspNetMultipleLicenseUser>().Update(record);
            await _unitOfwork.Complete();
            if (data.IsApproved == true)
            {
                var licences = await _unitOfwork.genericRepository<Licence>()
                      .GetByCondition(l => l.LicId == record.LicenseId).FirstOrDefaultAsync();
                var mandoobId = await _unitOfwork.genericRepository<AspNetMultipleUser>()
                          .GetByCondition(m => m.Id == record.Id).FirstOrDefaultAsync();
                var aspnetUser = await _unitOfwork.genericRepository<AspNetUser>()
                        .GetByCondition(a => a.Id == mandoobId.MandoobId).FirstOrDefaultAsync();
                licences.MandoobCivilId = aspnetUser.CivilId;
                licences.MandoobId = aspnetUser.Id;
                await _unitOfwork.genericRepository<Licence>().Update(licences);
                await _unitOfwork.Complete();
            }

            return Ok(new ErrorMessage
            {
                Error = false,
                Status = "200",
                Message = data.IsApproved ? "تمت الموافقة" : "تم الرفض",
                ID = data.Id
            });
        }

        #endregion

    }
}
