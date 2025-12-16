using Business.ViewModel.Dynamic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class PreApprovementVM
    {
        public int PreAppId { get; set; }
        public int? LinkedLicenseId { get; set; }
        public bool IsConsumed { get; set; }
        public int? BuildingId { get; set; }
        
        public int? CompanyId { get; set; }

        public int? ManagerId { get; set; }
        public string? Flag { get; set; }

        public string? AppId { get; set; }
        public string? UserId { get; set; }


        public long? RequestId { get; set; }

        public int? LicTypeId { get; set; }

        public string? ClassificationName { get; set; }

        public DateTime? ClassificationDate { get; set; }



        public DateTime? ComIssuingDate { get; set; }

        public DateTime? ComExpiryDate { get; set; }

        public int? ActivityTypeId { get; set; }

        public int? ReqStatusId { get; set; }

        public string? LicenseName { get; set; }

        public string? LicenseNo { get; set; }

        public DateTime? LicenseIssueDate { get; set; }

        public DateTime? LicenseExpireDate { get; set; }

        public int? ClassificationId { get; set; }

        public string? ApplicantCivilId { get; set; }
        public string? CommercialLicNo { get; set; }
        public string? RecordNo { get; set; }


        public string? ManagerCivilId { get; set; }

        public string? UserCivilId { get; set; }

        public int? LicStatusId { get; set; }
        public string? SalesManagerCivilId { get; set; }
        public string? MarketingManagerCivilId { get; set; }
        public string? OperationsManagerCivilId { get; set; }
        public int? SalesManagerId { get; set; }
        public int? MarketingManagerId { get; set; }
        public int? OperationsManagerId { get; set; }
      
        [ForeignKey("SalesManagerId")]
        public virtual PersonVM? SalesManager { get; set; }
        [ForeignKey("MarketingManagerId")]
        public virtual PersonVM? MarketingManager { get; set; }
        [ForeignKey("OperationsManagerId")]
        public virtual PersonVM? OperationsManager { get; set; }
        [ForeignKey("ManagerId")]
        public virtual PersonVM? Manager { get; set; }
        public virtual AspnetUserVM? User { get; set; }  
        [ForeignKey("CompanyId")]
        public virtual CompanyVM? Company { get; set; }
        [ForeignKey("BuildingId")]
        public virtual CompanyVM? Building { get; set; }
        [ForeignKey("LicTypeId")]
        public virtual LicencesTypeVM? LicenceTypesLookup { get; set; }
        [ForeignKey("ActivityTypeId")]
        public virtual ActivityTypeVM? ActivityTypesLookup { get; set; }
        [ForeignKey("ReqStatusId")]
        public virtual RequestStatusVM? RequestStatusLookup { get; set; }
        [ForeignKey("LicStatusId")]
        public virtual LicencesStatusVM? licenceStatusLookup { get; set; }
    }
}
