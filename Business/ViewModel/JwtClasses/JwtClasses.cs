using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.JwtClasses
{
    public class JwtClasses
    {
        public class ApiResponse
        {
            public string token { get; set; }
            public User user { get; set; }
        }

        public class User
        {
            [JsonIgnore] // Optional: Include $id if necessary
            public string Id { get; set; }
            public string username { get; set; }
            public int serviceId { get; set; }
            public bool status { get; set; }
        }
    }
}
