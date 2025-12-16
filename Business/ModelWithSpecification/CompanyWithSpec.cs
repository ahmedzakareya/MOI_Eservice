using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class CompanyWithSpec:Specification<Company>
    {
        public CompanyWithSpec(int id,int serviceid):base(c=>c.Id==id && c.ServiceId == serviceid)
        {
            Includes.Add(c => c.AddressNavigation);
        }
    }
}
