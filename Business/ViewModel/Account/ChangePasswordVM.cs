using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Account
{
    public class ChangePasswordVM
    {
        [Required]
        
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور الحالية ")]
        public string? OldPassword { get; set; }

        public string? CivilId {  get; set; }    

        [Required]
        [StringLength(100, ErrorMessage = "كلمة المرور لا تقل عن حرفين ولا تزيد عن 8 أحرف ", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور الجديدة")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور الجديدة ")]
        [Compare("NewPassword", ErrorMessage = "كلمة المرور الجديدة و كلمة مرور التأكيد غير متطابقة ")]
        public string? ConfirmPassword { get; set; }
    }
}
