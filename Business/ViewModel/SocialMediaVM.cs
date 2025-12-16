using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class SocialMediaVM
    {
        public long Id { get; set; }

        public int? SocialType { get; set; }

        public long? Requestid { get; set; }
        public int? LicenceId { get; set; }
        public string? AccountSocial { get; set; }
        [ForeignKey("SocialType")]
        public virtual SocialTypeLookup SocialTypeLookup { get; set; }
    }
}
