using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class ChangeSocialMediaTransVM
    {
        public int Id { get; set; }

        public int? RequestId { get; set; }

        public int? SocialMediaType { get; set; }

        public string? SocialMediaRequestType { get; set; }

        public string? OldAccountSocial_MediaName { get; set; }

        public string? NewAccountSocial_Media { get; set; }
        public DateTime? RequestDate { get; set; }

        public bool? Status { get; set; }

        public long? TransactionId { get; set; }
        [ForeignKey("SocialMediaType")]
        public virtual SocialTypeLookup? SocialMedialookup { get; set; }

    }
}
