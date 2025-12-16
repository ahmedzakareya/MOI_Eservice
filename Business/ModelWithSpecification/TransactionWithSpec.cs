using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class TransactionWithSpec:Specification<Transaction>
    {
        public TransactionWithSpec(int serviceId):base(x=>x.ServiceId==serviceId)
        { 
        
        }

        public TransactionWithSpec(long RequestId, long serviceId) : base(x => x.ServiceId == serviceId&& x.RequestId==RequestId)
        {
            Includes.Add(t => t.TransType);
        }
    }
}
