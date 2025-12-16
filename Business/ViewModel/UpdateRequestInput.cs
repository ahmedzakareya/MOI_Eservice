using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class UpdateRequestInput
    {
        public int RequestID { get; set; }
        public string Notes { get; set; }
        public string CivilID { get; set; }
    }
}
