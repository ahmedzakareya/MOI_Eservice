using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RequestTransaction
    {
        public int Id { get; set; }

        public long? RequestId { get; set; }

        public int? ReqTypeId { get; set; }
     public int? LicenseId { get; set; }
        public int? ReqStatusId { get; set; }

        public string? Notes { get; set; }
        public string? Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string? CivilIdUser { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? ServiceId {  get; set; }

        [ForeignKey("ReqTypeId")]

        public RequestsTypesLookup? RequestType { get; set; }
        [ForeignKey("ReqStatusId")]
        public RequestStatusLookup? RequestStatus { get; set; }
        [ForeignKey("RequestId")]
        public MoiEserviceLicensesRequest? Request { get; set; }
    }

}
