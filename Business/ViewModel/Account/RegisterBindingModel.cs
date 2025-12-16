using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Account
{
    public class RegisterBindingModel
    {
        public string UserID { get; set; }
        [Required]
        [Display(Name = "Civil ID")]
        public string CivilID { get; set; }

        [Required]
        [Display(Name = "Full Name Arabic")]
        public string FullNameAr { get; set; }

        [Required]
        [Display(Name = "Full Name English")]
        public string FullNameEn { get; set; }

        [Required]
        [Display(Name = "Mobile Number")]
        public string Mobile { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Display(Name = "Old Password")]
        //public string OldPassword { get; set; }

        //[Required]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        //[DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        //[DataType(DataType.Password)]
        //[Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Account Type")]
        public int? AccountTypeId { get; set; }
    }
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "أدخل الرقم المدني")]
        [Display(Name = "الرقم المدني")]
        [StringLength(12, ErrorMessage = "الرقم المدني 12 رقم باللغة الإنجليزية فقط", MinimumLength = 12)]
        [RegularExpression(@"^[1-3][0-9][0-9]((0[13578]|[1][02])(0[1-9]|[12][0-9]|3[01])|(0[469]|(11))(0[1-9]|[12][0-9]|30)|(02)(0[1-9]|1[0-9]|2[0-8]))[0-9]{5}$|(^[1-3](04|08|[2468][048]|[13579][26])|(300))(0229)[0-9]{5}$", ErrorMessage = "الرقم المدني غير صحيح")]

        public string CivilId { get; set; }

        [Required(ErrorMessage = "أدخل كلمة المرور")]
        [StringLength(100, ErrorMessage = "كلمة المرور لا تقل عن 8 أحرف تحتوي على حروف كبيرة وصغيرة وأرقام ورموز خاصة", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{""':;?/>.<,])(?!.*\s).{8,}$", ErrorMessage = "كلمة المرور لا تقل عن 8 أحرف تحتوي على حروف كبيرة وصغيرة وأرقام ورموز خاصة")]

        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
        public string? token { get; set; }
        public bool IsApplicant { get; set; }
        public bool IsDelegate { get; set; }
    }

    public class LoginBindingModel
    {
        [Required]
        [Display(Name = "Civil ID")]
        [RegularExpression("^[0-9]{12,12}$", ErrorMessage = "civil Id must be 12  Numeric")]
        public string CivilID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "password")]
        public string Password { get; set; }
    }


    public class ResetPasswordViewModel1
    {
        public int id { get; set; }

        [Required(ErrorMessage = "أدخل الرقم المدني")]
        [Display(Name = "الرقم المدني")]
        [StringLength(12, ErrorMessage = "الرقم المدني 12 رقم باللغة الإنجليزية فقط", MinimumLength = 12)]
        [RegularExpression(@"^[1-3][0-9][0-9]((0[13578]|[1][02])(0[1-9]|[12][0-9]|3[01])|(0[469]|(11))(0[1-9]|[12][0-9]|30)|(02)(0[1-9]|1[0-9]|2[0-8]))[0-9]{5}$|(^[1-3](04|08|[2468][048]|[13579][26])|(300))(0229)[0-9]{5}$", ErrorMessage = "الرقم المدني غير صحيح")]
        public string? CivilID { get; set; }


        [Required(ErrorMessage = "أدخل البريد الإلكتروني")]
        [Display(Name = "البريد الإلكتروني")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "أدخل بريد إلكتروني صحيح")]
        public string? Email { get; set; }
       

        //public Nullable<System.DateTime> DateAdded { get; set; }

        [Required(ErrorMessage = "أدخل كلمة المرور")]
        [StringLength(100, ErrorMessage = "كلمة المرور لا تقل عن 8 أحرف تحتوي على حروف كبيرة وصغيرة وأرقام ورموز خاصة", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{""':;?/>.<,])(?!.*\s).{8,}$", ErrorMessage = "كلمة المرور لا تقل عن 8 أحرف تحتوي على حروف كبيرة وصغيرة وأرقام ورموز خاصة")]
        public string? NewPass { get; set; }


        [Required(ErrorMessage = "أدخل رقم الجوال")]
        [Display(Name = "رقم الجوال")]
        [RegularExpression(@"^[4-69]\d{7}$", ErrorMessage = "أدخل رقم جوال كويتي صحيح، الأرقام بالإنجليزية فقط")]
        public string? Mobile { get; set; }
        [Required]
        public IFormFile? Image { get; set; }
        public string? AttachPath { get; set; }
       
    }

    public class ResetPasswordVM
    {
        public int id { get; set; }

        
        public string? CivilID { get; set; }


        
        public string? Email { get; set; }


        //public Nullable<System.DateTime> DateAdded { get; set; }

        
        public string? NewPass { get; set; }

        public string? Note { get; set; }


        public string? Mobile { get; set; }
        
        public string? AttachPath { get; set; }

    }


    public class RegisterViewModel
    {
        [Required(ErrorMessage = "أدخل الرقم المدني لصاحب الترخيص")]
        [Display(Name = "الرقم المدني لصاحب الترخيص")]
        [StringLength(12, ErrorMessage = "الرقم المدني 12 رقم باللغة الإنجليزية فقط", MinimumLength = 12)]
        [RegularExpression(@"^[1-3][0-9][0-9]((0[13578]|[1][02])(0[1-9]|[12][0-9]|3[01])|(0[469]|(11))(0[1-9]|[12][0-9]|30)|(02)(0[1-9]|1[0-9]|2[0-8]))[0-9]{5}$|(^[1-3](04|08|[2468][048]|[13579][26])|(300))(0229)[0-9]{5}$", ErrorMessage = "الرقم المدني غير صحيح")]
        public string CivilID { get; set; }

        [Required(ErrorMessage = "أدخل الاسم باللغة العربية")]
        [Display(Name = "الإسم باللغة العربية")]
        //[RegularExpression(@"^[\\u0621-\\u064A\\s]+$", ErrorMessage = "الاسم باللغة العربية فقط")]
        public string FullNameAr { get; set; }


        [Display(Name = "الإسم باللغة الإنجليزية")]
       // [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "الاسم باللغة الإنجليزية فقط")]
        public string FullNameEn { get; set; }

        [Required(ErrorMessage = "أدخل رقم الجوال")]
        [Display(Name = "رقم الجوال")]
        [RegularExpression(@"^[4-69]\d{7}$", ErrorMessage = "أدخل رقم جوال كويتي صحيح، الأرقام بالإنجليزية فقط")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "أدخل البريد الإلكتروني")]
        [Display(Name = "البريد الإلكتروني")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "أدخل بريد إلكتروني صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage = "أدخل كلمة المرور")]
        [StringLength(100, ErrorMessage = "كلمة المرور لا تقل عن 8 أحرف تحتوي على حروف كبيرة وصغيرة وأرقام ورموز خاصة", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{""':;?/>.<,])(?!.*\s).{8,}$", ErrorMessage = "كلمة المرور لا تقل عن 8 أحرف تحتوي على حروف كبيرة وصغيرة وأرقام ورموز خاصة")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "أدخل كلمة المرور مرة أخرى")]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }
    }
}
