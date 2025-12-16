using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Account
{
    public class MoiEserviceSysUserVM
    {
        public int SysUserId { get; set; }
        //[Required]
        public string? Username { get; set; }
        //[Required]
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? CivilId { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public int? UserType { get; set; }
        public int? UserRole { get; set; }
        public string? UserPasswordEncrypted { get; set; }
        public int? ServiceId { get; set; }
        public int? SectorId { get; set; }

        public int? DepId { get; set; }

        public int? MuraqabaId { get; set; }

        public int? QismId { get; set; }

       
        public virtual ICollection<AspNetUserRoleAdmin> UserRoles { get; set; } = new List<AspNetUserRoleAdmin>();


    }
    public class SysUserVM
    {

        [Required]
        public string Username { get; set; }

        [Required]

        public string UserPasswordEncrypted { get; set; }
        //public int? UserRole { get; set; }



        public bool Status { get; set; } = true;
        public int? ServiceId { get; set; }

    }
}
