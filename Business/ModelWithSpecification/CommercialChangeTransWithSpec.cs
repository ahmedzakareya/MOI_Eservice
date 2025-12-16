using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class CommercialChangeTransWithSpec:Specification<CommercialNameChangeTransaction>
    {
        public CommercialChangeTransWithSpec(int ServiceId, long RequestId) : base(c => c.RequestId == RequestId && c.ServiceId == ServiceId) 
        {
                
        }
    }
}
