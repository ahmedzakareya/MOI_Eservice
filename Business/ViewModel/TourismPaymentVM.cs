using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class TourismPaymentVM
    {
        public int? reqID { get; set; }

        public string LicID { get; set; }

        public string Reqno { get; set; }

        public int? requesterID { get; set; }

        [Display(Name = "تاريخ تقديم الطلب")]
        public string LicrequestLicreqtime { get; set; }


        [Display(Name = "نوع الطلب")]
        public string LicrequestLictype { get; set; }


        [Display(Name = "حالة الطلب")]
        public string ReqStatus { get; set; }


        [Display(Name = "اسم صاحب الترخيص")]
        public string userDateName { get; set; }

        [Display(Name = "المبلغ")]
        public decimal ServiceAmount { get; set; }

        public string StrRequesterEmail { get; set; }

        public string StrRequesterMobile { get; set; }
    }
}
