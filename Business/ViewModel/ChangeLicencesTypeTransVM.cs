using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class ChangeLicencesTypeTransVM
    {
        public int Id { get; set; }

        public int? TransactionId { get; set; }

        public long? OldRequestid { get; set; }

        public long? NewRequestid { get; set; }

        public string? LicenseNo { get; set; }

        public bool? Status { get; set; }

        public string? OldCivilId { get; set; }

        public string? NewCivilId { get; set; }

        public string? LastUpdateUser { get; set; }

        public DateTime? LastUpdateDate { get; set; }
        public int? LicenceId { get; set; }
        public string? LicTypeOld { get; set; }

        public string? LicTypeNew { get; set; }
        [ForeignKey("LicenceId")]
        public virtual Licence Licence { get; set; }
    }
}

