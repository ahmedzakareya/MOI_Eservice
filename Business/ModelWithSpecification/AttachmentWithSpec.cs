using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class AttachmentWithSpec:Specification<MoiEserviceRequestsAttach>
    {
        public AttachmentWithSpec(long RequestId,int serviceId) : base(x => x.AttachRequestid == RequestId&&x.ServiceId==serviceId)
        {

        }
        //public AttachmentWithSpec(long RequestId, int serviceId,bo) : base(x => x.AttachRequestid == RequestId && x.ServiceId == serviceId&&x.IsLatest==IsLatest)
        //{

        //}
    }
}
