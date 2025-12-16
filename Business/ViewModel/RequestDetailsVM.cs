using Business.ViewModel.ClassificationVM;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class RequestDetailsVM
    {
        public RequestVM? RequestDVM { get; set; }
        // this two to conditions 
        public bool IsFinalStatus { get; set; }
        public string? requestStatus { get; set; }
        public string? FlagRequestStatus { get; set; }
        public string? ClassificationName { get; set; }
        public int? ClassificationId { get; set; }
        public List<AllowedButtonVM>? AllowedButtons { get; set; }
        public ActivityChangeTransVM? ActivityChangeTransVM { get; set; }
        public IEnumerable<RequestStatusVM>? RequestStatusVM { get; set; }
        public ChangeOwnerTransVM? OwnerChangeTransVM { get; set; }
        public IEnumerable<RequestTransactionVM>? RequestTransactionVM { get; set; }
        public IEnumerable<SocialMediaVM>? socialMediaVMs { get; set; }
        public ChangeManagerTransVM? ManagerChangeTransVM { get; set; }
        public MediaChangeTransVM? MediaChangeTransVM { get; set; }
        public ChangeSocialMediaTransVM? ChangeSocialMediaTransVM { get; set; }
        public EmailChangeTransVM? EmailChangeTransVM { get; set; }

        public AddressChangeTransVM? AddressChangeTransVM { get; set; }
        public ChangeNewPartnerTransVM? ChangePartnerTransVM { get; set; }
        public RenewVM? LicenceRenewVM { get; set; }
       
        public EndLicencesTransVM? EndLicencesTransVM { get; set; } 
        public IEnumerable<PartnerVM>? PartnerVM { get; set; }
        public PersonVM? PersonApplicantVM { get; set; }=new PersonVM();
        public PersonVM? ManagerPersonVM { get; set; } = new PersonVM();
        public AspnetUserVM? Mandoob { get; set; }
        public TransactionVM? TransactionsVM { get; set; }
        public List<ClassificationBranchDetail>? ClassificationData { get; set; }
        public ReplacementOfLostTransVM? ReplacementOfLostTransVM { get; set; }
        public CommercialTransVM? CommercialTransVM { get; set; }
        public CompanyTransVM? CompanyTransVM { get; set; }
        public CompanyVM? CompanyVM { get; set; } = null;
        public BuildingVM? BuildingVM { get; set; }  
        public IEnumerable<AttachVM>? attachmentVM { get; set; }
        public AttachVM? attachmentFinal { get; set; }

        public PaymentDetailsVM? PaymentDetailsVM { get; set; }
        public LicencesVM? LicencesVM {  get; set; }  

        public PreApprovementVM? PreApprovementVM { get; set; }
        public AspnetUserVM? AspNetUserVM { get; set; }
    }
    public class AllowedButtonVM
    {
        public int Id { get; set; }
        public string? ButtonText { get; set; }
        public bool IsPermissionRequired { get; set; }
        public string? ReasonIfNotAllowed { get; set; } // e.g. "NoPermissionRequired", "PermissionRequired_ButDenied", etc.

        public int? NextStatusId { get; set; }
        public string? ActionKey { get; set; }
    }
}
