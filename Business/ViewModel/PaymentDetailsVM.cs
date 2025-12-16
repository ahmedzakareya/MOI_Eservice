using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class PaymentDetailsVM
    {
        public int Id { get; set; }

        public long? RequestId { get; set; }

        public string? UserId { get; set; }

        public int? LicenceId { get; set; }

        public int ServiceId { get; set; }

        public string? PaymentId { get; set; }

        public string? Result { get; set; }

        public string? TranId { get; set; }

        public string? Ref { get; set; }

        public string? Postdate { get; set; }

        public string? Auth { get; set; }

        public string? TrackId { get; set; }

        public decimal? TotalAmount { get; set; }

        public int? Payed { get; set; }

        public string? Token { get; set; }

        public DateTime? PaymentDate { get; set; }

        public string? Status { get; set; }

        public string? PaymentMethod { get; set; }

        public int? LicenseCategory { get; set; }
    }

    public class PaymentRequestModel
    {
        public long reqID { get; set; }
        public decimal ServiceAmount { get; set; }
        public string userDateName { get; set; }
        public string StrRequesterMobile { get; set; }
        public string StrRequesterEmail { get; set; }
        public int ServicePrefixPaymentId { get; set; }
        public int? LicId { get; set; }
       
        public string ApplicantId { get; set; }

        public string ApplicantCivilId { get; set; }
    }
}
