using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class LicencesInfoWithSpec:Specification<MoiEserviceLicenseInfo>
    {
        public LicencesInfoWithSpec():base()
        {
            Includes.Add(l => l.ActivityTypesLookup);
            Includes.Add(l => l.EserviceTypeBranch);
            //Includes.Add(l => l.EserviceTypesLookup);
            Includes.Add(l => l.TransactionTypesLookup);
            Includes.Add(l => l.RequestsTypesLookup);
            Includes.Add(l => l.ActivityTypesLookup.Eservice);

        }
        public LicencesInfoWithSpec(int id) : base(x=>x.Id==id)
        {
            Includes.Add(l => l.ActivityTypesLookup);
            Includes.Add(l => l.EserviceTypeBranch);
            //Includes.Add(l => l.EserviceTypesLookup);
            Includes.Add(l => l.TransactionTypesLookup);
            Includes.Add(l => l.RequestsTypesLookup);
            Includes.Add(l => l.ActivityTypesLookup.Eservice);

        }
    }
}
