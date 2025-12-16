using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class PaciDataVM
    {
        public string? arFullName { get; set; }
        public string? enFullName { get; set; }
        public string? birthDate { get; set; }
        public string? email { get; set; }
        public string? mobile { get; set; }
        public string? sex { get; set; }
        public string? statusCode { get; set; }                  
        public string? disclaimer { get; set; }
        public string? remainingHits { get; set; }
        public string? timeStamp { get; set; }
        public string? message { get; set; }
        public string? environment { get; set; }

    }
    public class ClientConfigVM
    {

        public string? grant_type { get; set; }
        public string? username { get; set; }
        public string? password { get; set; }

    }

    public class PaciTokenResultVM
    {
        public string? access_token { get; set; }
    }
}
