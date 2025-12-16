using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.HomePage
{
    public class LicencesInfoVM
    {
        public int Id { get; set; }

        public int? ActvityTypeId { get; set; }
        public List<int>? SelectedTransactionTypeIds { get; set; }

        public int? ReqTypeId { get; set; }
        public int? LicTypeId { get; set; }

        public int? LicId { get; set; }
        public int? EserviceTypeBranchId { get; set; }

        public int? ServiceId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Conditions { get; set; }

        public string? RequiredDocuments { get; set; }

        public string? Measures { get; set; }

        public decimal? VariableFees { get; set; }

        public decimal? FixedFees { get; set; }

        public bool? Status { get; set; }

        public int? Sort { get; set; }

        public string? Branch { get; set; }

        public string? Controller { get; set; }

        public string? Action { get; set; }

        public string? Url { get; set; }
        public string? ActivityType { get; set; }
        public string? RequestType { get; set; }
        public string? TransactionType { get; set; }
        public string? EServiceName { get; set; }

        public string? EserviceType { get; set; }
        public string? BranchType { get; set; }
        [ForeignKey("EserviceTypeBranchId")]
        public virtual EserviceTypeBranch EserviceTypeBranch { get; set; }
        [ForeignKey("EserviceTypeId")]
        public virtual EserviceTypesLookup EserviceTypesLookup { get; set; }
        [ForeignKey("ReqTypeId")]
        public virtual RequestsTypesLookup RequestsTypesLookup { get; set; }
        [ForeignKey("LicTypeId")]
        public virtual LicenceTypesLookup? LicenceTypesLookup { get; set; }
        [ForeignKey("ActvityTypeId")]
        public virtual ActivityTypesLookup ActivityTypesLookup { get; set; }
        [ForeignKey("TransTypeId")]
        public virtual TransactionTypesLookup TransactionTypesLookup { get; set; }
    }


    public class CreateLicencesInfo
    {
        public int Id { get; set; }

        public int? ActvityTypeId { get; set; }

        public int? ReqTypeId { get; set; }

        public int? EserviceTypeBranchId { get; set; }

        public int ServiceId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Conditions { get; set; }

        public string? RequiredDocuments { get; set; }

        public string? Measures { get; set; }

        public decimal? VariableFees { get; set; }

        public decimal? FixedFees { get; set; }

        public bool? Status { get; set; }

        public int? Sort { get; set; }

        public string? Branch { get; set; }

        public string? Controller { get; set; }

        public string? Action { get; set; }

        public string? Url { get; set; }
        public string? ActivityType { get; set; }
        public string? RequestType { get; set; }
        public string? TransactionType { get; set; }
        public string? EServiceName { get; set; }

        public string? EserviceType { get; set; }
        public string? BranchType { get; set; }
        public IEnumerable<SelectListItem>? Services { get; set; }
        public IEnumerable<SelectListItem>? ActivityTypes { get; set; }
        public IEnumerable<SelectListItem>? RequestTypes { get; set; }
        public IEnumerable<SelectListItem>? EserviceTypeBranch { get; set; }
        public IEnumerable<SelectListItem>? TransactionTypes { get; set; }
        public IEnumerable<Eservice>? ServicesModel { get; set; }
        public IEnumerable<TransactionTypesLookup>? transactionTypesModel { get; set; }
        public IEnumerable<ActivityTypesLookup>? ActivityTypesModel { get; set; }
        public IEnumerable<RequestsTypesLookup>? RequestTypesModel { get; set; }
        public IEnumerable<LicenceTypesLookup> LicenceTypesModel { get; set; }

        public IEnumerable<EserviceTypeBranch>? EserviceTypeBranchModel { get; set; }
    }

    public class LicenseEditViewModel
    {
        public MoiEserviceLicenseInfo License { get; set; }
        public IEnumerable<SelectListItem>? Services { get; set; }
        public IEnumerable<SelectListItem>? ActivityTypes { get; set; }
        public IEnumerable<SelectListItem>? LicencesTypes { get; set; }

        public IEnumerable<SelectListItem>? RequestTypes { get; set; }
        public IEnumerable<SelectListItem>? EserviceTypeBranch { get; set; }
        public IEnumerable<SelectListItem>? TransactionTypes { get; set; }
        public List<ActivityTypesLookup> ActivityTypesModel { get; set; }
        public List<LicenceTypesLookup> LicenceTypesModel { get; set; }

        public List<EserviceTypeBranch> EserviceTypeBranchModel { get; set; }
        public List<Eservice> ServicesModel { get; set; }
        public List<RequestsTypesLookup> RequestTypesModel { get; set; }
        public List<TransactionTypesLookup> TransactionTypesModel { get; set; }
    }


}
