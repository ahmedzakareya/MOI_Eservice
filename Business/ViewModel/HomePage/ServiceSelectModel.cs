using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.HomePage
{
    public class ServiceSelectModel
    {

        public List<EserviceViewModel> Eservices { get; set; }
        public List<EserviceActvityTypeModel> ActvityTypes { get; set; }
        public List<EserviceTypeModel> EserviceTypes { get; set; }
        public List<EserviceTypeBranchModel> EserviceTypeBranches { get; set; }
    }
}
