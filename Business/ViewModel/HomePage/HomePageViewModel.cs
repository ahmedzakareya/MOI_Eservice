using Business.ViewModel.Dynamic;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.HomePage
{
    public class HomePageViewModel
    {
        public List<EserviceViewModel>? Eservices { get; set; }
        public List<EserviceActvityTypeModel>? ActvityTypes { get; set; }
        public List<EserviceTypeModel>? EserviceTypes { get; set; }
        public List<EserviceTypeBranchModel>? EserviceTypeBranches { get; set; }
     
        public List<MoiEserviceLicenseInfo>? LicenceInfoList { get; set; }
        public List<LicencesInfoVM>? LicencesInfoVM { get; set; }
        public Dictionary<string, string>? SystemOptions { get; set; } // <== add this
        public List<SystemOptionVM>? SystemOptionVMs { get; set; }
        public List<HomeCardViewModel>? Cards { get; set; }


    }

    public class HomeCardViewModel
    {
        public int ActivityTypeId { get; set; }
        public int RequestTypeId { get; set; }
        public string? ActivityTypeName { get; set; }
        public string? EserviceBranchDisplayName { get; set; }

        public string? EserviceName { get; set; }
        public string? Url { get; set; }
        public string ActivityName { get; set; }
        public string BranchName { get; set; }
        public List<LicencesInfoVM> Licenses { get; set; }
    }
}
