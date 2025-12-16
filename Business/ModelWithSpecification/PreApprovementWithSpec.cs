using Business.Enums;
using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class PreApprovementWithSpec:Specification<MoiPreApprovement>
    {
       
        public PreApprovementWithSpec(int licenceId, bool IsLicence)
            :base(IsLicence
                 ? (R => R.PreAppId == licenceId) 
                 : (R => R.RequestId == licenceId))
        {
            Includes.Add(l => l.Applicant);

            Includes.Add(l => l.Company);
            Includes.Add(l => l.Company.AddressNavigation);
            Includes.Add(l => l.SalesManager);
            Includes.Add(l => l.MarketingManager);
            Includes.Add(l => l.OperationsManager);
            Includes.Add(l => l.LicenceTypesLookup);
            // Includes.Add(l => l.LicenseStatusLookup)
            Includes.Add(l => l.ActivityTypesLookup);
            Includes.Add(l => l.RequestStatusLookup);
            Includes.Add(l => l.Building);
            Includes.Add(l => l.Manager);
            Includes.Add(l => l.Building.AddressNavigation);
            Includes.Add(l => l.Mandoob);
            Includes.Add(l => l.Request);
        }

        public PreApprovementWithSpec()
           : base(p=>p.LicStatusId==(int) licencesStatusEnum.Released)
        {
            Includes.Add(l => l.Applicant);

            Includes.Add(l => l.Company);
            Includes.Add(l => l.Company.AddressNavigation);
            Includes.Add(l => l.SalesManager);
            Includes.Add(l => l.MarketingManager);
            Includes.Add(l => l.OperationsManager);
            Includes.Add(l => l.LicenceTypesLookup);
            Includes.Add(l => l.Manager);
            // Includes.Add(l => l.LicenseStatusLookup)
            Includes.Add(l => l.ActivityTypesLookup);
            Includes.Add(l => l.RequestStatusLookup);
            Includes.Add(l => l.Mandoob);
            Includes.Add(l => l.Request);
        }


        public PreApprovementWithSpec(string LicNo):base(x=>x.LicenseNo== LicNo)
        {
            Includes.Add(l => l.Company);
            Includes.Add(l => l.Company.AddressNavigation);

            Includes.Add(l => l.LicenceTypesLookup);
            Includes.Add(l => l.LicenseStatusLookup);
            Includes.Add(l => l.ActivityTypesLookup);
            Includes.Add(l => l.RequestStatusLookup);
            Includes.Add(l => l.Building);
            Includes.Add(l => l.Manager);
            Includes.Add(l => l.Building.AddressNavigation);
            Includes.Add(l => l.Applicant);
            Includes.Add(l => l.SalesManager);
            Includes.Add(l => l.MarketingManager);
            Includes.Add(l => l.OperationsManager);


            Includes.Add(l => l.Mandoob);   
            Includes.Add(l => l.Request);

        }
        public PreApprovementWithSpec(string AppCivilId,int LicStatusId) : base(x => x.ApplicantCivilId == AppCivilId&& x.LicStatusId== LicStatusId)
        {
            Includes.Add(l => l.Company);
            Includes.Add(l => l.LicenceTypesLookup);
            Includes.Add(l => l.LicenseStatusLookup);
            Includes.Add(l => l.ActivityTypesLookup);
            Includes.Add(l => l.RequestStatusLookup);
            Includes.Add(l => l.Company.AddressNavigation);
            Includes.Add(l => l.SalesManager);
            Includes.Add(l => l.MarketingManager);
            Includes.Add(l => l.OperationsManager);
            Includes.Add(l => l.Building);
            Includes.Add(l => l.Manager);
            Includes.Add(l => l.Building.AddressNavigation);
            Includes.Add(l => l.Applicant);
            Includes.Add(l => l.Mandoob);
            Includes.Add(l => l.Request);

        }

    }
}
