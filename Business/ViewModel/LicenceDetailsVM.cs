using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class LicenceDetailsVM
    {
        public IEnumerable<RequestVM>? RequestsDVM { get; set; }
        public List<int?> RequestTypesId { get; set; }
        public List<int>? SelectedTransactionTypeIds { get; set; }
        public List<EnumOptionVM>? TransactionTypeOptions { get; set; }

        public bool IsRenewable { get; set; }
        public IEnumerable<PartnerVM>? PartnerVM { get; set; }
        public PersonVM? PersonApplicantVM { get; set; }
        public PersonVM? ManagerPersonVM { get; set; }

        public AspnetUserVM? Mandoob { get; set; }
        public CompanyVM? CompanyVM { get; set; } = null;
        public IEnumerable<AttachVM>? attachmentVM { get; set; }
        public LicencesVM? LicencesVM { get; set; }

        public PreApprovementVM? PreApprovementVM { get; set; }
    }
    public class EnumOptionVM
    {
        public int Value { get; set; }
        public string Text { get; set; }
    }
    public class LicenceDetailsForUserVM
    {
        public IEnumerable<PreApprovementVM>? PreApprovementVM { get; set; }
        public IEnumerable<LicencesVM>? LicencesVM { get; set; }

    }
}
