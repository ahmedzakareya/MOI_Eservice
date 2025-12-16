using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Business.ViewModel.Dynamic
{
    public class ActivityTypeVM
    {
        public int Id { get; set; }

        public string? NameAr { get; set; }

        public int? MainLicenseId { get; set; }

        public int? ServiceId { get; set; }

        public string? ActivityCode { get; set; }

        public string? NameEn { get; set; }

        public List<SelectListItem> Activities2 { get; set; }

        public ActivityTypeVM()
        {
            Activities2 = new List<SelectListItem>(); // Make sure this is initialized
        }

    }
    public class ActivitySelectListItem
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string ActivityCode { get; set; }  // Custom data
    }
}
