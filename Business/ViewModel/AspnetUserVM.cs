using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class AspnetUserVM
    {
        public string Id { get; set; }
        [Display(Name = "الرقم المدني للمالك")]

        public string UserName { get; set; }
        public string Email { get; set; }
        [Display(Name = "الرقم المدني للمالك")]

        public string? CivilId { get; set; }
        [Display(Name = "إسم مالك العقار")]

        public string? FullNameAr { get; set; }

        public string? FullNameEn { get; set; }

        public string? Mobile { get; set; }

        public string? OldPassword { get; set; }
        public string? Password { get; set; }


        public bool? SahelUser { get; set; }

        public bool? Profileupdate2023 { get; set; }

        public int? AccountTypeId { get; set; }
        public string? NationalityName { get; set; }
        public string? Licname { get; set; }
    }
}
