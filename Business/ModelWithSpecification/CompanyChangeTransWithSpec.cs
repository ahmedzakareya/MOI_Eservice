using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class CompanyChangeTransWithSpec : Specification<CompanyNameChangeTransaction>
    {
        public CompanyChangeTransWithSpec(int ServiceId, long RequestId) : base(c => c.RequestId == RequestId && c.ServiceId == ServiceId)
        {
            Includes.Add(x => x.Transaction);
        }
        public CompanyChangeTransWithSpec(int TransactionId) : base(c => c.TransactionId==TransactionId)
        {
            Includes.Add(x => x.Transaction);
        }
    }
}
