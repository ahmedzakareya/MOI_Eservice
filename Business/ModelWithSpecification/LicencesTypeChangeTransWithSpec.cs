using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class LicencesTypeChangeTransWithSpec : Specification<LicenseTypeChangeTransaction>
    {
        public LicencesTypeChangeTransWithSpec(int ServiceId, long RequestId) : base(c => c.Requestid == RequestId && c.ServiceId == ServiceId)
        {
            Includes.Add(x => x.Transaction);
        }

    }
}
