using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.HomePage
{
    public class EserviceTypeBranchModel
    {
        [Required(ErrorMessage = "مطلوب")]
        public int Id { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        public int EserviceTypeId { get; set; }
        public string EserviceTypeBranchEn { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        public string EserviceTypeBranchAr { get; set; }
        public string Url { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public int? ActivityTypesId { get; set; }
    }
    public class EserviceTypeBranchViewModel
    {
        public EserviceTypeBranch Branch { get; set; }

        public List<ActivityTypesLookup> ActivityTypes { get; set; }
        public List<RequestsTypesLookup> RequestTypes { get; set; }
    }
}
