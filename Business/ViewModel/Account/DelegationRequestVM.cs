using Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Account
{
    public class DelegationRequestVM
    {
        public int Id { get; set; }

        public string? MainUserId { get; set; }

        public string? MandoobId { get; set; }

        public bool? IsActive { get; set; }
        [ForeignKey("MainUserId")]
        public virtual AspNetUser? User { get; set; }
        [ForeignKey("MandoobId")]

        public virtual AspNetUser? Mandoob { get; set; }
    }

    public class RegisterDelegateVM
    {
        public string? MandoobCivilId { get; set; }
        public string? ApplicantCivilId { get; set; }

        public string? FullNameAr { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public int? AccountTypeId { get; set; }

        public List<LicenseAssignmentVM> Licenses { get; set; } = new();
    }
    public class RegisterApiDelegateVM
    {
        public string? MandoobCivilId { get; set; }
        public string? ApplicantCivilId { get; set; }

        public string? FullNameAr { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "كلمة المرور غير متطابقة")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public int? AccountTypeId { get; set; }

        public List<LicenseApiAssignmentVM> Licenses { get; set; } = new();
    }
    
    public class LicenseAssignmentVM
    {
        public int Id { get; set; }
        public string? LicName { get; set; } 
        public string? LicNo { get; set; }
        public int? ServiceId { get; set; }
        public string? ServiceName { get; set; } 

        public bool IsSelected { get; set; }
        public IFormFile? FilePath { get; set; }

        public string? AttachmentUrl { get; set; }
    }
    public class LicenseApiAssignmentVM
    {
        public int Id { get; set; }
        public string? LicName { get; set; }
        public int? ServiceId { get; set; }
        public string? ServiceName { get; set; }

        public bool IsSelected { get; set; }
      

        public string? AttachmentUrl { get; set; }
    }
    public class PendingDelegationVM
    {
        public int Id { get; set; }
        public string? LicenseName { get; set; }
        public string? DelegateName { get; set; }
        public string? MainUserName { get; set; }
        public string? AttachmentUrl { get; set; }
        public string? Note { get; set; }

        public bool IsApproved { get; set; }
    }

}
