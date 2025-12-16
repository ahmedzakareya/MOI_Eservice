using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class ChangeOwnerTransVM
    {
        public int Id { get; set; }

        public int? TransactionId { get; set; }

        public int ServiceId { get; set; }

        public string? LastUpdateUser { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public string? OldName { get; set; }

        public string? OldNationalityNo { get; set; }

        public string? OldBirthDate { get; set; }

        public string? OldCivilId { get; set; }

        public string? OldQualification { get; set; }

        public string? OldQualificationDate { get; set; }

        public string? OldQualificationCountry { get; set; }

        public string? OldProfessionalExperience { get; set; }

        public string? NewName { get; set; }

        public string? NewNationallityNo { get; set; }

        public string? NewBirthDate { get; set; }

        public string? NewCivilId { get; set; }

        public string? NewQualification { get; set; }

        public string? NewQualificationDate { get; set; }

        public string? NewQualificationCountry { get; set; }

        public string? NewProfessionalExperience { get; set; }

        public DateTime? NewExpiryDate { get; set; }

        public int? RenounceId { get; set; }

        public long? RequestId { get; set; }
        public string? OldMobile { get; set; }
        public string? OldEmail { get; set; }
        public string? NewMobile { get; set; }
        public string? NewEmail { get; set; }
        [ForeignKey("RequestId")]
        public virtual MoiEserviceLicensesRequest? MoiEserviceLicensesRequest { get; set; }
        [ForeignKey("LicencesId")]
        public virtual Licence? Licence { get; set; }

    }
}
