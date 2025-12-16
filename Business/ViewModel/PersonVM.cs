using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class PersonVM
    {
        public int Id { get; set; }

        public string? personName { get; set; }
        [Display(Name = "الإسم الأول")]

        public string? Name1 { get; set; }
        [Display(Name = "الإسم الثاني")]

        public string? Name2 { get; set; }
        [Display(Name = "الإسم الثالث")]

        public string? Name3 { get; set; }
        [Display(Name = "الإسم الرابع")]

        public string? Name4 { get; set; }
        public string? BirthDateOld { get; set; }
        [Display(Name = "الرقم المدني")]

        public string? CivilId { get; set; }
        public int? NationalityId { get; set; }
        public string? NationaltiyNo { get; set; }
        public int? QualificationId { get; set; }
        public string? QualificationDateOld { get; set; }
        public int? QualificationCountryId { get; set; }
        public string? ProfessionalExperience { get; set; }
        public int? CategoryId { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string? CompanyNo { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? QualificationDate { get; set; }
        public int? PersonTypeId { get; set; }
        [Display(Name = "الجنسية")]

        public string? NationaliyName { get; set; }
        public string? QualificationName { get; set; }
        public string? QualificationCountry { get; set; }
        public string? Messaage { get; set; }
        public bool? IsApplicant { get; set; }

        // transation
        public string? MotletterNo { get; set; }
        public string? MotletterDate { get; set; }

        


        public string? Education { get; set; }
        [Display(Name = "رقم جوال ")]

        public string? Phone { get; set; }
        [Display(Name = "البريد الالكتروني ")]

        public string? Email { get; set; }

        public int? AddressId { get; set; }

        [ForeignKey("QualificationId")]

        public virtual QualificationsLookup? QualificationsLookup { get; set; }
        [ForeignKey("AddressId")]
        public virtual AddressVM? AddressNavigation { get; set; }


        public PersonVM? oldPersonModel { get; set; }
        
    }
}
