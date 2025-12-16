using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class PersonApplicantWithSpec:Specification<Person>
    {
        public PersonApplicantWithSpec(string civilId,int serviceId):base(p=>p.CivilId==civilId)
        {
            Includes.Add(x=>x.AddressNavigation);
        }
    }
}
