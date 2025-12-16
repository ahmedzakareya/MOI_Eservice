using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class RequestStatusVM
    {
        public int Id { get; set; }

        public string? NameAr { get; set; }

        public string? NameEn { get; set; }

        public bool? Status { get; set; }

        public int? Sort { get; set; }

        public int? ServiceId { get; set; }
    }
}
