using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class AspNetMultipleLicencesUserWithSpec : Specification<AspNetMultipleLicenseUser>
    {
        public AspNetMultipleLicencesUserWithSpec() :base()
        {
            Includes.Add(a => a.AspNetMultipleUser);
            Includes.Add(a => a.Licence);
            Includes.Add(a => a.AspNetMultipleUser.User);
            Includes.Add(a => a.AspNetMultipleUser.Mandoob);



        }
        public AspNetMultipleLicencesUserWithSpec(bool IsApproved,bool IsConfirmed) : base(a=>a.IsApproved==IsApproved&&a.IsConfirmed==IsConfirmed)
        {
            Includes.Add(a => a.AspNetMultipleUser);
            Includes.Add(a => a.Licence);
            Includes.Add(a => a.AspNetMultipleUser.User);
            Includes.Add(a => a.AspNetMultipleUser.Mandoob);



        }

        public AspNetMultipleLicencesUserWithSpec(int id) : base(a => a.Id == id)
        {
            Includes.Add(a => a.AspNetMultipleUser);
            Includes.Add(a => a.Licence);
            Includes.Add(a => a.AspNetMultipleUser.User);
            Includes.Add(a => a.AspNetMultipleUser.Mandoob);



        }


    }
}
