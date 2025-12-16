using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class LicencesNameChangeTransWithSpec : Specification<LicencesNameChangeTransaction>
    {
        public LicencesNameChangeTransWithSpec(int ServiceId, long RequestId) : base(c => c.RequestId == RequestId && c.ServiceId == ServiceId)
        {
            Includes.Add(x => x.Transaction);
        }

    }
}
