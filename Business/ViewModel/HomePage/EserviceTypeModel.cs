using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.HomePage
{
    public class EserviceTypeModel
    {
        [Required(ErrorMessage = "مطلوب")]
        public int Id { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        public string EserviceId { get; set; }
        public string EserviceTypeEn { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        public string EserviceTypeAr { get; set; }
        public string Url { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
