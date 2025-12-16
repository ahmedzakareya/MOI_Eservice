using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class ManagerChangeTransWithSpec : Specification<TchangeManager>
    {
        public ManagerChangeTransWithSpec(int ServiceId, long RequestId) : base(c => c.RequestId == RequestId && c.ServiceId == ServiceId)
        {
            Includes.Add(x => x.Transaction);
            Includes.Add(x => x.AddressChangeTransaction);
            Includes.Add(x => x.ManagerOldqualification);
            Includes.Add(x => x.ManagerNewqualification);
            Includes.Add(x => x.OldManager);


        }
        public ManagerChangeTransWithSpec(int TransactionId) : base(c => c.TransactionId==TransactionId)
        {
            Includes.Add(x => x.Transaction);
            Includes.Add(x => x.AddressChangeTransaction);
            Includes.Add(x => x.ManagerOldqualification);
            Includes.Add(x => x.ManagerNewqualification);
            Includes.Add(x => x.OldManager);
        }
    }
}
