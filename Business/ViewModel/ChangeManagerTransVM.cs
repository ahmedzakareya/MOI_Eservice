using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class ChangeManagerTransVM
    {
        public int ManagerId { get; set; }

        public string? ManagerLicno { get; set; }

        public int ServiceId { get; set; }

        public string? ManagerOldname { get; set; }

        public string? ManagerOldcivilid { get; set; }

        public int? ManagerOldcountryid { get; set; }

        public DateOnly? ManagerOldbirthdate { get; set; }

        public int? ManagerOldqualificationid { get; set; }

        public string? ManagerNewname { get; set; }

        public string? ManagerNewcivilid { get; set; }

        public int? ManagerNewcountryid { get; set; }

        public DateOnly? ManagerNewbirthdate { get; set; }

        public int? ManagerNewqualificationid { get; set; }
        public int? ManagerNewqualificationCountryid { get; set; }

        public string? ManagerBookno { get; set; }

        public DateOnly? ManagerBookdate { get; set; }

        public string? ManagerTradeletterAttach { get; set; }
        public int? OldManagerId { get; set; }

        public int? TransactionId { get; set; }

        public string? LastUpdateUser { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public string? OldNationaltiyNo { get; set; }

        public string? NewNationaltiyNo { get; set; }

        public int? RequestId { get; set; }

        public string? OldNationality { get; set; }

        public string? NewNationality { get; set; }

        public string? OldMobile { get; set; }

        public string? NewMobile { get; set; }

        public string? OldEmail { get; set; }

        public string? NewEmail { get; set; }

        public string? OldAddress { get; set; }

        public string? NewAddress { get; set; }
        public string? ManagerNewname1 { get; set; }
        public string? ManagerNewname2 { get; set; }
        public string? ManagerNewname3 { get; set; }
        public string? ManagerNewname4 { get; set; }
        public virtual QualificationsLookup? ManagerOldqualification { get; set; }
        [ForeignKey("TransactionId")]
        public virtual Transaction? Transaction { get; set; }


        public string?  OldNationalityName { get; set; }
        public string? OldQualificationName { get; set; }
        public string? OldQualificationCountryName { get; set; }

        public string? NewQualificationName { get; set; }

        public RequestDetailsVM? RequestDetailsVM { get; set; }


    }
}
