using Business.Interfaces;
using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class SocialWithSpec: Specification<MoiSocialMedia>
    {
        public SocialWithSpec(long requestId,bool IsRequest)
            :base(IsRequest ?
                 ( s=>s.Requestid==requestId):
                  (s=>s.LicenceId==requestId))
        {
            Includes.Add(s => s.SocialTypeLookup);
        }
        public SocialWithSpec(int licenceId,int SocialMediaType)
            : base(s=>s.LicenceId==licenceId&&s.SocialType==SocialMediaType)
        {
            Includes.Add(s => s.SocialTypeLookup);
        }
    }
}
