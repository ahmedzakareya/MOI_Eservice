using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Business.ViewModel.Channels
{
    public   class ChannelsPersonsVM
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? CivilID { get; set; }
        public string? Passport { get; set; }
        public int Nationality { get; set; }
        public int RequestID { get; set; }
        [JsonIgnore]

        public IFormFile? WorkFile { get; set; }
        [JsonIgnore]

        public IFormFile? CriminalFile { get; set; }
    }
}
