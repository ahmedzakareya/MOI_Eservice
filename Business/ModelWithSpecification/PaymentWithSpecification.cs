using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class PaymentWithSpecification:Specification<MoiEserviceRequestPaymentDetail>
    {
        public PaymentWithSpecification(long RequestId):base(x=>x.RequestId==RequestId)
        {
                
        }
    }
}
