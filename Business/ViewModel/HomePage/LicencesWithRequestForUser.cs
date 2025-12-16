using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.HomePage
{
    public class LicencesWithRequestForUser
    {
        public AspnetUserVM? AspnetUserVM { get; set; }
        public IEnumerable<AspnetUserVM>? Mandoob { get; set; }
        public Eservice Eservice { get; set; }
        public IEnumerable<RequestVM>? RequestVM { get; set; }
        public IEnumerable<LicencesVM>? licencesVMs { get; set; }
        public IEnumerable<PreApprovementVM>? preApprovementVMs { get; set; }
        //public MoiEserviceLicensesRequest 
        //RequestMosanafat,
        //        RequestTourism,
        //        RequestElaw,
        //        RequestPublishing,
        //        LicencesMosanafat,
        //        LicencesPublishing,
        //        LicencesElaw,
        //        LicencesTourism,
        //        applicant,
        //        MandoobInformation
    }
}
