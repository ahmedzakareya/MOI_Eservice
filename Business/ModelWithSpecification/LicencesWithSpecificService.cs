using Azure.Core;
using Business.Enums;
using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class LicencesWithSpecificService:Specification<Licence>
    {
        public LicencesWithSpecificService(int ServiceId,bool isLicence):base(isLicence
                 ? (R => R.LicId == ServiceId && R.LicStatusId !=(int) licencesStatusEnum.Pending && R.LicStatusId != (int)licencesStatusEnum.Ending)
                 : (R => R.ServiceId == ServiceId&&R.LicStatusId!= (int)licencesStatusEnum.Pending && R.LicStatusId!= (int)licencesStatusEnum.Ending))
        {
            Includes.Add(l => l.Manager);
            Includes.Add(l => l.Company);

            Includes.Add(l => l.LicenceTypesLookup);
            Includes.Add(l => l.LicenseStatusLookup);
            Includes.Add(l => l.ActivityTypesLookup);
            Includes.Add(l => l.Classification);
            Includes.Add(l => l.PreApprovement);
            Includes.Add(l => l.Applicant);
            Includes.Add(l => l.SalesManager);
            Includes.Add(l => l.Applicant);
            Includes.Add(l => l.Company.AddressNavigation);
            Includes.Add(l => l.OperationsManager);
            Includes.Add(l => l.MarketingManager);
            Includes.Add(l => l.Mandoob);

            Includes.Add(l => l.Building);
            Includes.Add(l => l.Building.AddressNavigation);

        }
        public LicencesWithSpecificService(int id,int ServiceId) : base(
            R => R.LicId == id && R.ServiceId==ServiceId
            )
        {
            Includes.Add(l => l.Manager);
            Includes.Add(l => l.Company);
            Includes.Add(l => l.LicenceTypesLookup);
            Includes.Add(l => l.LicenseStatusLookup);
            Includes.Add(l => l.ActivityTypesLookup);
            Includes.Add(l => l.Classification);
            Includes.Add(l => l.Building);
            Includes.Add(l => l.PreApprovement);
            Includes.Add(l => l.Building.AddressNavigation);
            Includes.Add(l => l.Applicant);
            Includes.Add(l => l.SalesManager);
            Includes.Add(l => l.Applicant);
            Includes.Add(l => l.Company.AddressNavigation);
            Includes.Add(l => l.OperationsManager);
            Includes.Add(l => l.MarketingManager);
            Includes.Add(l => l.Mandoob);

        }
        public LicencesWithSpecificService(string CivilId, int LicStatus) : base(
            R => R.LicStatusId == LicStatus && R.ApplicantCivilId == CivilId
            )
        {
            Includes.Add(l => l.Manager);
            Includes.Add(l => l.Company);
            Includes.Add(l => l.LicenceTypesLookup);
            Includes.Add(l => l.LicenseStatusLookup);
            Includes.Add(l => l.ActivityTypesLookup);
            Includes.Add(l => l.Classification);
            Includes.Add(l => l.Building);
            Includes.Add(l => l.PreApprovement);

            Includes.Add(l => l.Building.AddressNavigation);
            Includes.Add(l => l.Applicant);
            Includes.Add(l => l.SalesManager);
            Includes.Add(l => l.Applicant);
            Includes.Add(l => l.Company.AddressNavigation);
            Includes.Add(l => l.OperationsManager);
            Includes.Add(l => l.MarketingManager);
            Includes.Add(l => l.Mandoob);

        }
        public LicencesWithSpecificService(string CivilId) : base(
            R =>  R.ApplicantCivilId == CivilId
            )
        {
            Includes.Add(l => l.Manager);
            Includes.Add(l => l.Company);
            Includes.Add(l => l.LicenceTypesLookup);
            Includes.Add(l => l.LicenseStatusLookup);
            Includes.Add(l => l.ActivityTypesLookup);
            Includes.Add(l => l.Classification);
            Includes.Add(l => l.Building);
            Includes.Add(l => l.PreApprovement);
            Includes.Add(l => l.Mandoob);

            Includes.Add(l => l.Building.AddressNavigation);
            Includes.Add(l => l.Applicant);
            Includes.Add(l => l.SalesManager);
            Includes.Add(l => l.Applicant);
            Includes.Add(l => l.Company.AddressNavigation);
            Includes.Add(l => l.OperationsManager);
            Includes.Add(l => l.MarketingManager);

        }
        
        public LicencesWithSpecificService(string CivilId, bool excludePendingAndEnding)
    : base(
        R => R.ApplicantCivilId == CivilId &&
            (!excludePendingAndEnding ||
             (R.LicStatusId != (int)licencesStatusEnum.Pending && R.LicStatusId != (int)licencesStatusEnum.Ending))
      )
        {
            Includes.Add(l => l.Manager);
            Includes.Add(l => l.Company);
            Includes.Add(l => l.LicenceTypesLookup);
            Includes.Add(l => l.LicenseStatusLookup);
            Includes.Add(l => l.ActivityTypesLookup);
            Includes.Add(l => l.Classification);
            Includes.Add(l => l.PreApprovement);
            Includes.Add(l => l.Applicant);
            Includes.Add(l => l.SalesManager);
            Includes.Add(l => l.Applicant);
            Includes.Add(l => l.Company.AddressNavigation);
            Includes.Add(l => l.OperationsManager);
            Includes.Add(l => l.MarketingManager);
            Includes.Add(l => l.Building);
            Includes.Add(l => l.Mandoob);

            Includes.Add(l => l.Building.AddressNavigation);

        }
        public LicencesWithSpecificService(string CivilId, int ServiceId,int LicStatus) : base(
            R => R.ServiceId == ServiceId && R.ApplicantCivilId == CivilId&& R.LicStatusId== LicStatus
            )
        {
            Includes.Add(l => l.Manager);
            Includes.Add(l => l.Company);
            Includes.Add(l => l.LicenceTypesLookup);
            Includes.Add(l => l.LicenseStatusLookup);
            Includes.Add(l => l.ActivityTypesLookup);
            Includes.Add(l => l.Classification);
            Includes.Add(l => l.Building);
            Includes.Add(l => l.PreApprovement);
            Includes.Add(l => l.Applicant);
            Includes.Add(l => l.SalesManager);
            Includes.Add(l => l.Applicant);
            Includes.Add(l => l.Company.AddressNavigation);
            Includes.Add(l => l.OperationsManager);
            Includes.Add(l => l.MarketingManager);
            Includes.Add(l => l.Mandoob);
            Includes.Add(l => l.Building.AddressNavigation);


        }
    }
}
