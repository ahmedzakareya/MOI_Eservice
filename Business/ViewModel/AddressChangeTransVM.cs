using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class AddressChangeTransVM
    {
        public int Id { get; set; }

        public int? TransactionId { get; set; }

        public int? ServiceId { get; set; }

        public string? OldGovernorate { get; set; }

        public string? OldArea { get; set; }

        public string? OldBlock { get; set; }

        public string? OldStreet { get; set; }
        public string? AreaSizeOld { get; set; }
        public string? AreaSizeNew { get; set; }
        public string? AalliNoOld { get; set; }
        public string? AalliNoNew { get; set; }
        public string? AreaChartNoOld { get; set; }
        public string? AreaChartNoNew { get; set; }

        public string? OldBuilding { get; set; }

        public string? OldFloor { get; set; }

        public string? OldOwnerName { get; set; }

        public string? OldPhoneNo { get; set; }

        public string? OldFaxNo { get; set; }

        public string? NewGovernorate { get; set; }

        public string? NewArea { get; set; }

        public string? NewBlock { get; set; }

        public string? NewStreet { get; set; }

        public string? NewBuilding { get; set; }

        public string? NewFloor { get; set; }

        public string? NewOwnerName { get; set; }

        public string? NewPhoneNo { get; set; }

        public string? NewFaxNo { get; set; }

        public string? LastUpdateUser { get; set; }

        public DateTime? LastUpdateDate { get; set; }

        public string? NewAddressAutoNo { get; set; }

        public string? OldAddressAutoNo { get; set; }

        public int? AddId { get; set; }

        public string? AddressNew { get; set; }

        public string? AddressOld { get; set; }

        public int? RequestId { get; set; }
        [ForeignKey("TransactionId")]
        public virtual TransactionVM Transaction { get; set; }
    }
}
