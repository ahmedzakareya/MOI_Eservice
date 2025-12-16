using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class RenouncementTransactionVM
    {
        public int Id { get; set; }

        public int? ReqTransactionId { get; set; }
        public int? LicencesId { get; set; }


        public int? ServiceId { get; set; }

        public string? LastUpdateUser { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public string? OldName { get; set; }

        public string? OldNationalityNo { get; set; }

        public DateTime? OldBirthDate { get; set; }

        public string? OldCivilId { get; set; }

        public string? OldQualification { get; set; }

        public DateTime? OldQualificationDate { get; set; }

        public string? OldQualificationCountry { get; set; }

        public string? OldProfessionalExperience { get; set; }

        public string? NewName { get; set; }

        public string? NewNationallityNo { get; set; }

        public DateTime? NewBirthDate { get; set; }

        public string? NewCivilId { get; set; }

        public string? NewQualification { get; set; }

        public DateTime? NewQualificationDate { get; set; }

        public string? NewQualificationCountry { get; set; }

        public string? NewProfessionalExperience { get; set; }

        public DateTime? NewExpiryDate { get; set; }
        public string? OldMobile { get; set; }
        public string? OldEmail { get; set; }
        public string? NewMobile { get; set; }
        public string? NewEmail { get; set; }
        public int? NewCountryID { get; set; }
        public int? NewQualificationId { get; set; }
        public int? NewqualificationCountryid { get; set; }


    }
}
