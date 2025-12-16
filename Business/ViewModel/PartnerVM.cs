using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class PartnerVM
    {
        public int Id { get; set; }
        public int? LicenseId { get; set; }
        public string Name { get; set; }
        public string? LastUpdateUser { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public bool IsActive { get; set; }
    }
}
