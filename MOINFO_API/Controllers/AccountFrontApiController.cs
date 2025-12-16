using AutoMapper;
using Business.Interfaces;
using Business.ViewModel;
using Business.ViewModel.Account;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace MOINFO_API.Controllers
{
    //[Authorize]
    [Route("api/AccountFront")]
    public class AccountFrontApiController:BaseController
    {
        private readonly IUnitOfwork _unitOfwork;
        private readonly IMapper _mapper;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly SignInManager<AspNetUser> _signInManager;
        public IConfiguration Configuration { get; }
        public AccountFrontApiController(IUnitOfwork unitOfwork, IMapper mapper, IConfiguration configuration, UserManager<AspNetUser> userManager, SignInManager<AspNetUser> signInManager)
        {
            _unitOfwork = unitOfwork;
            _mapper = mapper;
            Configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [Route("RegisterUser")]
        [HttpPost]
        public async Task<IActionResult> RegisterUser(RegisterViewModel model)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                //bool passwordChanged = true;
                // Check if user already exists by CivilID
                //var existingUserByCivilID = await _userManager.FindByNameAsync(model.CivilID);
                var existingCivilId = await _unitOfwork.genericRepository<AspNetUser>().GetByIdObject(x=>x.CivilId==model.CivilID);
                if (existingCivilId != null)
                {
                    return BadRequest("A user with this CivilID already exists.");
                }

                // Check if user already exists by Email
                var existingUserByEmail = await _userManager.FindByEmailAsync(model.Email);
                if (existingUserByEmail != null)
                {
                    return BadRequest("A user with this email already exists.");
                }

                var user = new AspNetUser()
                {
                    UserName = model.CivilID,                  
                    CivilId = model.CivilID,
                    Email = model.Email,
                    EmailConfirmed = true,
                    PhoneNumber = model.Mobile,
                    PhoneNumberConfirmed = true,
                    Mobile = model.Mobile,
                    FullNameAr = model.FullNameAr,
                    FullNameEn = model.FullNameEn,

                  //  PasswordChanged = passwordChanged,
                    AccountTypeId = 100
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                var PersonExist = await _unitOfwork.genericRepository<Person>()
                            .GetByCondition(p => p.CivilId == model.CivilID).FirstOrDefaultAsync();
                if (PersonExist == null)
                {

                    var PersonAdd = new Person()
                    {
                        Name1 = model.FullNameAr,
                        Email = model.Email,
                        Phone = model.Mobile,
                        CivilId = model.CivilID,
                        NationaliyName="كويتي",

                    };
                    await _unitOfwork.genericRepository<Person>().Create(PersonAdd);
                    await _unitOfwork.Complete();
                }
                if (result.Succeeded)
                {
                    return Ok(new { message = "User registered successfully." });
                }
                if (!result.Succeeded)
                {
                    // Log the errors returned from Identity
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"CreateAsync Error: {error.Description}");
                    }
                    return BadRequest(result.Errors);  // Return the specific errors from Identity
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            return BadRequest();

        }


        [Route("LoginUser")]
        [HttpPost]
        
        public async Task<IActionResult> LoginUser([FromBody] LoginViewModel model)
        {
            try
            {
                // Validate the CivilId
                var existingUser = await _unitOfwork.genericRepository<AspNetUser>().GetByIdObject(x => x.CivilId == model.CivilId);
                if (existingUser == null)
                {
                    return BadRequest(new { message = "User with this Civil ID doesn't exist." });
                }
                //bool isDelegate = false;
                
                // Validate the password
                var passwordValid = await _userManager.CheckPasswordAsync(existingUser, model.Password);
                if (!passwordValid)
                {
                    return BadRequest(new { message = "Invalid password." });
                }

                // Check if the account is locked
                if (existingUser.LockoutEnabled && existingUser.LockoutEndDateUtc.HasValue && existingUser.LockoutEndDateUtc.Value > DateTime.UtcNow)
                {
                    return BadRequest(new { message = "Your account is locked. Please try again later." });
                }

                // If login is successful, sign the user in (you can skip this if you're using JWT tokens)
                //await _signInManager.SignInAsync(existingUser, isPersistent: false);
                
                bool isApplicant = await _unitOfwork.genericRepository<Licence>()
                                    .GetByCondition(x => x.ApplicantCivilId == existingUser.CivilId)
                                    .AnyAsync();
                var isDelegate = await _unitOfwork.genericRepository<AspNetMultipleUser>()
                    .GetByCondition(a => a.MandoobId == existingUser.Id)
                    .AnyAsync();

                // ✅ Generate token with new claims
                var token = GenerateJwtToken(existingUser, isDelegate,isApplicant);
                // Return a successful login response with a token (if using JWT)
                return Ok(new { message = "Login successful.", token = token });
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                return StatusCode(500, new { message = "An error occurred during login.", details = ex.Message });
            }
        }
        private string GenerateJwtToken(AspNetUser user,bool isDelegate,bool isApplicant)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim("CivilId", user.CivilId),
        new Claim("FullName", user.FullNameAr),
        new Claim("AccouuntTypeId",user.AccountTypeId.ToString()),
        new Claim("IsApplicant", isApplicant.ToString()),
       new Claim("IsDelegate", isDelegate.ToString())
        // You can add any other claims based on your needs
    };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration["JwtSettings:Key"]));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Configuration["JwtSettings:ValidIssuer"],
                audience: Configuration["JwtSettings:ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(int.Parse(Configuration["JwtSettings:DurationInDays"])),
                signingCredentials: signingCredentials
            );

            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }

        

        [Route("ResetPasswordUser")]
        [HttpPost]
        public async Task<IActionResult> ResetPasswordUser(ResetPasswordVM model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var userExist = await _unitOfwork.genericRepository<AspNetUser>()
                    .GetByCondition(a => a.CivilId == model.CivilID).FirstOrDefaultAsync();

                if (userExist != null)
                {
                    var RequestResetPassword = await _unitOfwork.genericRepository<ResetUserPassword>()
                                      .GetByCondition(r => r.UserCivilID == model.CivilID && r.Executed == false).FirstOrDefaultAsync();
                    if (RequestResetPassword == null)
                    {
                        // Save to DB
                        var resetRequest = new ResetUserPassword
                        {
                            UserCivilID = model.CivilID,
                            UserEmail = model.Email,
                            Mobile = model.Mobile,
                            UserNewPass = model.NewPass,
                            FilePath = model.AttachPath,
                            DateAdded = DateTime.Now,
                            Executed = false,
                            Status=false
                        };

                        await _unitOfwork.genericRepository<ResetUserPassword>().Create(resetRequest);
                        await _unitOfwork.Complete();

                        return Ok(new { message = "Reset request submitted successfully." });
                    }else
                    {
                        return Ok(new { message = "يوجد طلب تغيير كلمة السر" });
                    }
                }else
                {
                    return Ok(new { message = "لا يوجد مستخدم مسجل بهذا لرقم المدني " });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [Route("ContactUsUser")]
        [HttpPost]
        public async Task<IActionResult> ContactUsUser(ContactUsVM model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var entity = new ContactUs
            {
                FullNameAr = model.FullNameAr,
                FullNameEn = model.FullNameEn,
                Email = model.Email,
                Mobile = model.Mobile,
                Message = model.Message,
                CreatedOn = DateTime.Now,
                IsDeleted = false
            };
            await _unitOfwork.genericRepository<ContactUs>().Create(entity);
            await _unitOfwork.Complete();

            return Ok(new { message = "Contact request submitted", id = entity.Id });
            return Ok(model);
        }

        //[Route("ChangePasswordUser")]
        //[HttpPost]
        //public async Task<IActionResult> ChangePasswordUser(ChangePasswordVM model)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //            return BadRequest(ModelState);
        //        var userExist = await _unitOfwork.genericRepository<AspNetUser>()
        //            .GetByCondition(a => a.CivilId == model.CivilId).FirstOrDefaultAsync();

        //        if (userExist != null)
        //        {
        //            var RequestResetPassword = await _unitOfwork.genericRepository<ResetUserPassword>()
        //                              .GetByCondition(r => r.UserCivilID == model.CivilId && r.Executed == false).FirstOrDefaultAsync();
        //            if (RequestResetPassword == null)
        //            {
        //                // Save to DB
        //                var resetRequest = new ResetUserPassword
        //                {
        //                    UserCivilID = model.CivilID,
        //                    UserEmail = model.Email,
        //                    Mobile = model.Mobile,
        //                    UserNewPass = model.NewPass,
        //                    FilePath = model.AttachPath,
        //                    DateAdded = DateTime.Now,
        //                    Executed = false,
        //                    Status = false
        //                };

        //                await _unitOfwork.genericRepository<ResetUserPassword>().Create(resetRequest);
        //                await _unitOfwork.Complete();

        //                return Ok(new { message = "Reset request submitted successfully." });
        //            }
        //            else
        //            {
        //                return Ok(new { message = "يوجد طلب تغيير كلمة السر" });
        //            }
        //        }
        //        else
        //        {
        //            return Ok(new { message = "لا يوجد مستخدم مسجل بهذا لرقم المدني " });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}




    }
}
