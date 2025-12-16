using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class FormsViewModel
    {

        public int Id { get; set; }

        public int ServiceId { get; set; }

        public string? FormName { get; set; }

        public string? FormPath { get; set; }

        public string? FormStatus { get; set; }

        public string? FormType { get; set; }

        public int? DocType { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
