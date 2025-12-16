using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class CompanyVM
    {
        public int Id { get; set; }

        public int? ServiceId { get; set; }

        public string? Parcel { get; set; }
        [Display(Name = "رقم الهاتف")]

        public string? PhoneNo { get; set; }
        [Display(Name = "رقم المدني للجهة")]

        public string? CompanyCivilId { get; set; }
        [Display(Name = "إسم مالك العقار")]

        public string? OwnerName { get; set; }
        [Display(Name = "إسم الشركة")]

        public string? Name { get; set; }
        [Display(Name = "الإيميل")]

        public string? Email { get; set; }
        public string? LastUpdateUser { get; set; }
        [DisplayName("الرقم المركزي")]
        public string? CentralNoMoci { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        [Display(Name = "رقم الفاكس")]

        public string? CompanyNo { get; set; }

        public string? AddressAutoNo { get; set; }
        [Display(Name = "نشاط الشركة")]

        public string? CompanyActivity { get; set; }
        [Display(Name = "الشركة المديرة")]

        public string? DirCompanyAr { get; set; }

        public string? DirCompanyEn { get; set; }
        [Display(Name = "الشركة المالكة")]

        public string? OwnerCompanyAr { get; set; }

        public string? OwnerCompanyEn { get; set; }
        [Display(Name = "رقم السجل التجاري")]

        public string? RecordNo { get; set; }
        [Display(Name = "رقم الترخيص التجاري")]

        public string? CommercialLicNo { get; set; }
       
        [Display(Name = "رمز النشاط")]

        public string? ActivityCode { get; set; }

        public int? ActivityTypeId { get; set; }

        public string? UnitType { get; set; }

        public int? AddressId { get; set; }
        public string? FaxNo { get; set; }

        [ForeignKey("AddressId")]
        public virtual AddressVM? AddressNavigation { get; set; }
        [ForeignKey("ActivityTypeId")]
        public virtual ActivityTypesLookup? ActivityTypeNavigation { get; set; }
    }
}
