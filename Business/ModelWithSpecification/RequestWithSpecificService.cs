using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
    public class RequestWithSpecificService : Specification<MoiEserviceLicensesRequest>
    {
        public RequestWithSpecificService(int ServiceId,bool isRequest) : base(isRequest
                 ? (R => R.RequestId==ServiceId)
                 : (R => R.ServiceId ==ServiceId))
        {
            Includes.Add(m => m.ActivityTypeNavigation);
            Includes.Add(m => m.RequestStatusNavigation);
            ////Includes.Add(m => m.RequestsTypes);
            Includes.Add(m => m.LicenceTypeNavigation);
            Includes.Add(m => m.RequestsTypesNavigation);
            Includes.Add(m => m.company);
            Includes.Add(m => m.company.AddressNavigation);

            Includes.Add(m => m.Building);
            Includes.Add(m=>m.LicenceNavigation);
            Includes.Add(m => m.Manager);
            Includes.Add(m => m.Manager.QualificationsLookup);

            Includes.Add(m => m.Manager.AddressNavigation);

            Includes.Add(m => m.Transactions);
            Includes.Add(m => m.Building.AddressNavigation);
            Includes.Add(m => m.ApplicantPerson);
            Includes.Add(m => m.ApplicantPerson.QualificationsLookup);

            Includes.Add(m => m.ApplicantPerson.AddressNavigation);
            Includes.Add(m => m.OperationsManager);
            Includes.Add(m => m.SalesManager);
            Includes.Add(m => m.MarketingManager);
            OrderByDescSpec(x => x.RequestId);
            ApplyTake(50);

        }
        public RequestWithSpecificService(int ServiceId) : base(
               r=>r.ServiceId==ServiceId)
        {
            Includes.Add(m => m.ActivityTypeNavigation);
            Includes.Add(m => m.RequestStatusNavigation);
            ////Includes.Add(m => m.RequestsTypes);
            Includes.Add(m => m.LicenceTypeNavigation);
            Includes.Add(m => m.RequestsTypesNavigation);
            Includes.Add(m => m.company);
            Includes.Add(m => m.Building);
            Includes.Add(m => m.LicenceNavigation);
            Includes.Add(m => m.Manager);
            Includes.Add(m => m.Transactions);
            Includes.Add(m => m.Building.AddressNavigation);
            Includes.Add(m => m.company.AddressNavigation);
            Includes.Add(m => m.Manager.QualificationsLookup);
            Includes.Add(m => m.ApplicantPerson.QualificationsLookup);

            Includes.Add(m => m.OperationsManager);
            Includes.Add(m => m.SalesManager);
            Includes.Add(m => m.MarketingManager);
            Includes.Add(m => m.ApplicantPerson);
            //Includes.Add(m=>
            OrderByDescSpec(r => r.RequestId);

        }
        public RequestWithSpecificService(int serviceId, int requestTypeId, int activityId)
        : base(r =>
            r.ServiceId == serviceId &&
            r.ReqtypeId == requestTypeId &&
             r.ActivityTypeId == activityId) // Include ActivityTypeId only if provided
        {
            // Add includes for related entities to enable eager loading
            Includes.Add(m => m.ActivityTypeNavigation);  // Include ActivityTypeNavigation
            Includes.Add(m => m.RequestStatusNavigation); // Include RequestStatusNavigation
            Includes.Add(m => m.LicenceTypeNavigation);   // Include LicenceTypeNavigation
            Includes.Add(m => m.RequestsTypesNavigation); // Include RequestsTypesNavigation
            Includes.Add(m => m.company);                // Include Company
            Includes.Add(m => m.Building);               // Include Building
            Includes.Add(m => m.LicenceNavigation);      // Include LicenceNavigation
            Includes.Add(m => m.Manager);                // Include Manager
            Includes.Add(m => m.Transactions);           // Include Transactions
            Includes.Add(m => m.Building.AddressNavigation);
            Includes.Add(m => m.company.AddressNavigation);
            Includes.Add(m => m.Manager.QualificationsLookup);
            Includes.Add(m => m.ApplicantPerson.QualificationsLookup);

            Includes.Add(m => m.OperationsManager);
            Includes.Add(m => m.SalesManager);
            Includes.Add(m => m.MarketingManager);
            Includes.Add(m => m.ApplicantPerson);
            OrderByDescSpec(x => x.RequestId);
            ApplyTake(50);
        }
        public RequestWithSpecificService(int serviceId, int requestTypeId)
       : base(r =>
           r.ServiceId == serviceId &&
           r.ReqtypeId == requestTypeId 
           ) // Include ActivityTypeId only if provided
        {
            // Add includes for related entities to enable eager loading
            Includes.Add(m => m.ActivityTypeNavigation);  // Include ActivityTypeNavigation
            Includes.Add(m => m.RequestStatusNavigation); // Include RequestStatusNavigation
            Includes.Add(m => m.LicenceTypeNavigation);   // Include LicenceTypeNavigation
            Includes.Add(m => m.RequestsTypesNavigation); // Include RequestsTypesNavigation
            Includes.Add(m => m.company);                // Include Company
            Includes.Add(m => m.Building);               // Include Building
            Includes.Add(m => m.LicenceNavigation);      // Include LicenceNavigation
            Includes.Add(m => m.Manager);                // Include Manager
            Includes.Add(m => m.Transactions);           // Include Transactions
            Includes.Add(m => m.Building.AddressNavigation);
            Includes.Add(m => m.company.AddressNavigation);
            Includes.Add(m => m.Manager.QualificationsLookup);
            Includes.Add(m => m.ApplicantPerson.QualificationsLookup);

            Includes.Add(m => m.OperationsManager);
            Includes.Add(m => m.ApplicantPerson);
            Includes.Add(m => m.SalesManager);
            Includes.Add(m => m.MarketingManager);
            OrderByDescSpec(x => x.RequestId);
            ApplyTake(50);
        }
        public RequestWithSpecificService(int id, int serviceId, bool isLicenseId)
         : base(isLicenseId
                 ? (R => R.ServiceId == serviceId && R.LicenseId == id)
                 : (R => R.ServiceId == serviceId && R.RequestId == id))
        {
            Includes.Add(m => m.ActivityTypeNavigation);
            Includes.Add(m => m.RequestStatusNavigation);
            ////Includes.Add(m => m.RequestsTypes);
            Includes.Add(m => m.LicenceTypeNavigation);
            Includes.Add(m => m.RequestsTypesNavigation);
            Includes.Add(m => m.ApplicantPerson);
            Includes.Add(m => m.company);
            Includes.Add(m => m.Building);
            Includes.Add(m => m.Building.AddressNavigation);
            Includes.Add(m => m.company.AddressNavigation);
            Includes.Add(m => m.ApplicantPerson.QualificationsLookup);

            Includes.Add(m => m.LicenceNavigation);
            Includes.Add(m => m.Manager);
            Includes.Add(m => m.Manager.QualificationsLookup);

            Includes.Add(m => m.Transactions);
            Includes.Add(m => m.OperationsManager);
            Includes.Add(m => m.SalesManager);
            Includes.Add(m => m.MarketingManager);


        }
        public RequestWithSpecificService():base()
        {
            OrderByDescSpec(r => r.RequestId);
            AddSelect(r => r.Licno);
        }
        public RequestWithSpecificService(string CivilId)
            : base(r => r.AppCivilId == CivilId)
        {
            Includes.Add(m => m.ActivityTypeNavigation);
            Includes.Add(m => m.RequestStatusNavigation);
            ////Includes.Add(m => m.RequestsTypes);
            Includes.Add(m => m.LicenceTypeNavigation);
            Includes.Add(m => m.RequestsTypesNavigation);
            Includes.Add(m => m.ApplicantPerson);
            Includes.Add(m => m.LicencePreApprovNavigation);
            Includes.Add(m => m.company);
            Includes.Add(m => m.company.AddressNavigation);
            Includes.Add(m => m.Manager.QualificationsLookup);
            Includes.Add(m => m.ApplicantPerson.QualificationsLookup);

            Includes.Add(m => m.Building);
            Includes.Add(m => m.LicenceNavigation);
            Includes.Add(m => m.Building.AddressNavigation);
            Includes.Add(m => m.OperationsManager);
            Includes.Add(m => m.SalesManager);
            Includes.Add(m => m.MarketingManager);
            Includes.Add(m => m.Manager);
            Includes.Add(m => m.Transactions);
            OrderByDescSpec(x => x.RequestId);
            ApplyTake(50);
        }

        public RequestWithSpecificService(string CivilId,int ServiceId)
            :base(r=>r.AppCivilId==CivilId & r.ServiceId==ServiceId)
        {
            Includes.Add(m => m.ActivityTypeNavigation);
            Includes.Add(m => m.RequestStatusNavigation);
            ////Includes.Add(m => m.RequestsTypes);
            Includes.Add(m => m.LicenceTypeNavigation);
            Includes.Add(m => m.RequestsTypesNavigation);
            Includes.Add(m => m.ApplicantPerson);
            Includes.Add(m => m.company);
            Includes.Add(m => m.company.AddressNavigation);
            Includes.Add(m => m.ApplicantPerson.QualificationsLookup);

            Includes.Add(m => m.Building);
            Includes.Add(m => m.LicenceNavigation);
            Includes.Add(m => m.Building.AddressNavigation);
            Includes.Add(m => m.OperationsManager);
            Includes.Add(m => m.SalesManager);
            Includes.Add(m => m.MarketingManager);
            Includes.Add(m => m.Manager);
            Includes.Add(m => m.Transactions);
            OrderByDescSpec(x => x.RequestId);
            ApplyTake(50);
        }
        public RequestWithSpecificService(int serviceId, int requestTypeId, int?[] activityIds)
        : base(r =>
            r.ServiceId == serviceId &&
            r.ReqtypeId == requestTypeId &&
             activityIds.Contains(r.ActivityTypeId)) // Include ActivityTypeId only if provided
        {
            // Add includes for related entities to enable eager loading
            Includes.Add(m => m.ActivityTypeNavigation);  // Include ActivityTypeNavigation
            Includes.Add(m => m.RequestStatusNavigation); // Include RequestStatusNavigation
            Includes.Add(m => m.LicenceTypeNavigation);   // Include LicenceTypeNavigation
            Includes.Add(m => m.RequestsTypesNavigation); // Include RequestsTypesNavigation
            Includes.Add(m => m.company);    
            Includes.Add(m => m.company.AddressNavigation);
            Includes.Add(m => m.ApplicantPerson.QualificationsLookup);

            Includes.Add(m => m.Building);
            Includes.Add(m => m.ApplicantPerson);// Include Building
            Includes.Add(m => m.LicenceNavigation);      // Include LicenceNavigation
            Includes.Add(m => m.Manager);                // Include Manager
            Includes.Add(m => m.Transactions);
            Includes.Add(m => m.Building.AddressNavigation);
            Includes.Add(m => m.OperationsManager);
            Includes.Add(m => m.SalesManager);
            Includes.Add(m => m.MarketingManager);
            OrderByDescSpec(x => x.RequestId);
            ApplyTake(50);

        }

        public RequestWithSpecificService(int serviceId, List<int> requestTypeIds, List<int> activityIds = null)
        : base(r =>
            r.ServiceId == serviceId &&
            (requestTypeIds == null || requestTypeIds.Contains(r.ReqtypeId??0)) &&
            (activityIds == null || activityIds.Contains(r.ActivityTypeId ?? 0)))
        {
            // Add includes for eager loading
            Includes.Add(m => m.ActivityTypeNavigation);  // Include ActivityTypeNavigation
            Includes.Add(m => m.RequestStatusNavigation); // Include RequestStatusNavigation
            Includes.Add(m => m.LicenceTypeNavigation);   // Include LicenceTypeNavigation
            Includes.Add(m => m.RequestsTypesNavigation); // Include RequestsTypesNavigation
            Includes.Add(m => m.company);
            Includes.Add(m => m.company.AddressNavigation);
            Includes.Add(m => m.ApplicantPerson.QualificationsLookup);
            Includes.Add(m => m.Manager.QualificationsLookup);

            Includes.Add(m => m.Building);
            Includes.Add(m => m.ApplicantPerson);// Include Building
            Includes.Add(m => m.LicenceNavigation);      // Include LicenceNavigation
            Includes.Add(m => m.Manager);                // Include Manager
            Includes.Add(m => m.Transactions);           // Include Transactions
            Includes.Add(m => m.Building.AddressNavigation);
            Includes.Add(m => m.OperationsManager);
            Includes.Add(m => m.SalesManager);
            Includes.Add(m => m.MarketingManager);
            OrderByDescSpec(x => x.RequestId);
            ApplyTake(50);
        }

    }
}
