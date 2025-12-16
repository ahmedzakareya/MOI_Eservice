using Business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.CombinedView
{
    public class CombinedRequestVM
    {
        public List<RequestVM>? Requests { get; set; }
        public RequestTypeEnum RequestType { get; set; }
        public List<ActivityTypeEnum>? ActivityTypes { get; set; }
    }
}
