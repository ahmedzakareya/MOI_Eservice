using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class RenewWithSpec:Specification<LicenseRenew>
    {
        public RenewWithSpec(int LicenseId,int serviceId) :base(l=>l.LicenseId==LicenseId&&l.ServiceId==serviceId)
        {
                
        }
    }
}
