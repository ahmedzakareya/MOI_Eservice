using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class PartnerWithSpec:Specification<Partner>
    {
        public PartnerWithSpec(int LicenceId,int serviceId):base(p=>p.LicenseId==LicenceId&&p.ServiceId==serviceId) 
        {

        }    
    }
}
