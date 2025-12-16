using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class TransactionVM
    {
        public int Id { get; set; }

        public int? LicenseId { get; set; }

        public int ServiceId { get; set; }

        public int? TransTypeId { get; set; }

        public string? MotletterNo { get; set; }

        public string? Changes { get; set; }

        public bool? Commited { get; set; }

        public string? Notes { get; set; }

        public string? LastUpdateUser { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public long? RequestId { get; set; }

        public DateTime? MotletterDate { get; set; }

        public DateTime? RequestDate { get; set; }

        public string? UsercivilId { get; set; }
        public string? TransationTypeName { get; set; }
        public int? ReqStatusId { get; set; }

        public DateTime? TransDate { get; set; }
        [ForeignKey("TransTypeId")]
        public virtual TransactionTypesLookup? TransType { get; set; }
        [ForeignKey("LicenseId")]

        public virtual LicencesVM? Licence { get; set; }
        [ForeignKey("RequestId")]
        public virtual RequestVM? Request { get; set; }
        [ForeignKey("ReqStatusId")]
        public virtual RequestStatusVM? RequestStatus { get; set; }

        public virtual CompanyTransVM? CompanyTransVM { get; set; }
        public virtual ChangeManagerTransVM? ChangeManagerTransVM { get; set; }  
        public virtual AddressChangeTransVM? AddressChangeTransVM { get; set; }

        //public virtual ReplacementOfLostTransVM? ReplacementOfLostTransVM { get; set;}
        public virtual LicencesNameChangeTransactionVM? LicencesNameChangeTransactionVM { get; set; }
        public virtual ChangeLicencesTypeTransVM? ChangeLicencesTypeTransVM { get; set; }

        public virtual EmailChangeTransVM? EmailChangeTransVM { get; set; }
        public virtual IEnumerable<ChangeNewPartnerTransVM>? ChangeNewPartnerTransVMs { get; set; }

        public virtual IEnumerable<ChangeOldPartnerTransVM>? ChangeOldPartnerTransVMs { get; set; }


        public virtual IEnumerable<ChangeSocialMediaTransVM>? ChangeSocialMediaTransVM { get;  set; }
        public virtual ReplacementOfLostTransVM? ReplacementOfLostTransVM { get; set; }




        // is  available   for update 
        public bool? IsAvailableForUpdate { get; set; }
    }
}
