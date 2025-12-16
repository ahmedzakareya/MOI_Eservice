using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class PaymentResponse
    {
        public int id { get; set; }
        public string RequestID { get; set; }
        public string RequestDate { get; set; }
        public string MOIMerchantID { get; set; }
        public string MerchantRequestID { get; set; }
        public string Result { get; set; }
        public string PaymentID { get; set; }
        public string TranID { get; set; }
        public string Ref { get; set; }
        public string Postdate { get; set; }
        public string Auth { get; set; }
        public string TrackID { get; set; }
        public decimal TotalAmount { get; set; }
        public int Payed { get; set; }
        public string Token { get; set; }
        public string Status { get; set; }
        public string PayeeName { get; set; }
        public string PayeeMobile { get; set; }
        public string PayeeEmail { get; set; }
        public object ErrorText { get; set; }
        public object ErrorNo { get; set; }
    }
}
