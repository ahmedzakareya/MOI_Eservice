using Business.ModelWithSpecification;
using Business.ViewModel.Dynamic;
using Business.ViewModel.HomePage;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Tourism
{
    public class RequestFrontVM
    {
        public RequestVM? RequestVM { get; set; }
        public List< RequestVM>? RequestListVM { get; set; }
        public IEnumerable<AttachVM>? attachVMs { get; set; }
        public AspnetUserVM? AspnetUserVM { get; set; }
        public PersonVM? ApplicantPerson { get; set; }
        public PersonVM? ManagerPerson { get; set; }
        public List<PartnerVM>? partnerVM { get; set; }
        public LicencesVM? LicencesVM { get; set; }
        public string? PreApprovalExist { get; set; }
        public PaymentDetailsVM? PaymentDetailsVM { get; set; }
        public LicencesInfoVM? LicencesInfoVM { get; set; }

        public List<AddAttachmentsRulesVM>? fileUploadConfigs { get; set; }

        [Display(Name = "المرفقات")]
        //public List<IFormFile> ContractDocuments { get; set; }
        public List<NamedFile>? NamedFile { get; set; }
        public RenewVM? RenewRequest { get; set; }
        public CompanyTransVM? CompanyTransVM { get; set; }

        public CommercialTransVM? CommercialTransVM { get; set; }

        public ActivityChangeTransVM? ActivityChangeTransVM { get; set; }


        public ChangeManagerTransVM? ChangeManagerTransVM { get; set; }
        public List< ChangeNewPartnerTransVM>? ChangeNewPartnerTransVM { get; set; }
        public List<ChangeOldPartnerTransVM>? ChangeOldPartnerTransVM { get; set; }


        public CompanyVM? CompanyVM { get; set; }
        public AddressChangeTransVM? AddressChangeTransVM { get; set; }
        public RenouncementTransactionVM? ChangeOwnerTransVM { get; set; }
        public List< RequestTransactionVM>? TransactionVM { get; set; }

        public RequestVM? requestForRenew { get; set; }
        public MoiEserviceLicensesRequest? PartnerRequest { get; set; }
    }



}
