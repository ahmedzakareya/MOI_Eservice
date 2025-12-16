using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class ValidEserviceCominationWithSpec:Specification<ValidEserviceCombinations>
    {
        public ValidEserviceCominationWithSpec():base()
        {
            Includes.Add(x => x.ActivityTypesLookup);
            Includes.Add(x => x.RequestsTypesLookup);
            Includes.Add(x => x.LicenceTypesLookup);
        }
        public ValidEserviceCominationWithSpec(int id) : base(x=>x.Id==id)
        {
            Includes.Add(x => x.ActivityTypesLookup);
            Includes.Add(x => x.RequestsTypesLookup);
            Includes.Add(x => x.LicenceTypesLookup);
        }
    }
}
