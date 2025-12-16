using Business.Enums;
using Business.ViewModel;
using Microsoft.Extensions.Configuration;

using System.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers
{
    public class PaymentGatewayService
    {
        private readonly IConfiguration _config;
        private readonly string _username;
        private readonly string _password;
        private readonly string _wsdl;
        public PaymentGatewayService(IConfiguration config)
        {
            _config = config;
            _username = _config["PaymentGateway:EpayTourismUsername"];
            _password = _config["PaymentGateway:EpayTourismPassword"];
            _wsdl = _config["PaymentGateway:EpayWsdl"];
        }


        public async Task<string> GetPaymentLink(int serviceId, PaymentRequestModel request)
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
            var endpoint = new EndpointAddress(_wsdl);
            var client = new ePaySoapClient(binding, endpoint);
           
            var token =await  client.GetePayTokenAsync(_username, _password);
            if (token == "0") return string.Empty;

            string prefix = serviceId.ToString();
            string merchantRequestId = prefix + request.reqID;

         

         
           
         

            // ✅ Redirection URLs
            //string successUrl = $"https://yourdomain.com/KnetPayment/KnetResult?RequestID={MyCrypto.Encode(merchantRequestId)}";
            //string cancelUrl = $"https://yourdomain.com/KnetPayment/KnetCancel?RequestID={MyCrypto.Encode(merchantRequestId)}";
            //string errorUrl = $"https://yourdomain.com/KnetPayment/KnetError?RequestID={MyCrypto.Encode(merchantRequestId)}";

            // ✅ Update call (depending on your WSDL, may be overload or need a new method)
            //var payLink = await client.ProceedToePayPaymentWithReturnUrlsAsync(
            //    token,
            //    request.ServiceAmount.ToString(),
            //    merchantRequestId,
            //    request.userDateName,
            //    request.StrRequesterMobile,
            //    request.StrRequesterEmail,
            //    successUrl,
            //    errorUrl,
            //    cancelUrl);

            //return payLink;

            var payLink =await client.ProceedToePayPaymentAsync(
                token,
                request.ServiceAmount.ToString(),
                merchantRequestId,
                request.userDateName,
                request.StrRequesterMobile,
                request.StrRequesterEmail
                );

            return payLink;
        }

    }
}
