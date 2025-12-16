using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class ManagerApplicantWithSpec:Specification<Person>
    {
        public ManagerApplicantWithSpec(int personId,int serviceId):base(p=>p.Id==personId)
        {
            Includes.Add(x => x.AddressNavigation);
        }
        public ManagerApplicantWithSpec(string civilId, int serviceId) : base(p => p.CivilId == civilId)
        {
            Includes.Add(x => x.AddressNavigation);
        }
    }
}
