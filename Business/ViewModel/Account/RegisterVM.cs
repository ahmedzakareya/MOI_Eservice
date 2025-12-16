using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Account
{
    public class RegisterVM
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
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "الاسم باللغة الإنجليزية فقط")]
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
}
