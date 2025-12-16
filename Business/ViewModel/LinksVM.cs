using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class LinksVM
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Link { get; set; }

        public bool Status { get; set; }
        public bool? IsDeleted { get; set; }

        public int? Sort { get; set; }
        public List<AddLinksVM> LinksList { get; set; }
    }
    public class AddLinksVM
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Link { get; set; }

        public bool Status { get; set; }
        public bool? IsDeleted { get; set; }

        public int? Sort { get; set; }
     
    }
}
