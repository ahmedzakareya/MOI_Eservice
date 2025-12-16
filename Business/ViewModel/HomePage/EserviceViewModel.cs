using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.HomePage
{
    public class EserviceViewModel
    {
        [Required(ErrorMessage = "مطلوب")]
        public string Id { get; set; }
        public string EserviceName { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        public string EserviceNameAr { get; set; }
        public string Url { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
