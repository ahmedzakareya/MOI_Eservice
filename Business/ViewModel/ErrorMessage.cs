using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class ErrorMessage
    {
        public Boolean Error { get; set; }
        public string? Status { get; set; }
        public string? Message { get; set; }
        public long? ID { get; set; }
    }
}
