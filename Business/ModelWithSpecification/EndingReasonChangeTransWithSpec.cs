using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class EndingReasonChangeTransWithSpec :Specification<LicenseEndingTransaction>
    {
        public EndingReasonChangeTransWithSpec( int ServiceId,long RequestId) : base(c =>c.ServiceId==ServiceId &&c.RequestId == RequestId)
        {
            Includes.Add(l => l.Licence);
            Includes.Add(l => l.MoiEserviceLicensesRequest);
            Includes.Add(l => l.LicendingReason);

        }
        public EndingReasonChangeTransWithSpec( long RequestId) : base(c =>c.RequestId == RequestId)
        {
            Includes.Add(l => l.Licence);
            Includes.Add(l => l.MoiEserviceLicensesRequest);
            Includes.Add(l => l.LicendingReason);

        }
    }
}
