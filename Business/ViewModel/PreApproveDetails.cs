using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class PreApproveDetails
    {
        public PreApprovementVM? PreApprovementVM { get; set; }
        public List<AttachVM>? attachVMs { get; set; }
        public PersonVM? Applicant { get; set; }

        public AspnetUserVM? Mandoob { get; set; }

    }
}
