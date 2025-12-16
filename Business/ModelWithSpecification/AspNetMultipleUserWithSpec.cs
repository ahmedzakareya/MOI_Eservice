using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class AspNetMultipleUserWithSpec : Specification<AspNetMultipleUser>
    {
        public AspNetMultipleUserWithSpec(string AspNetUserId,bool IsMainUser) :base(IsMainUser?
           ( r=>r.MainUserId == AspNetUserId):
            (r=>r.MandoobId==AspNetUserId))
        {
            Includes.Add(a => a.User);
            Includes.Add(a => a.Mandoob);

        }

        

    }
}
