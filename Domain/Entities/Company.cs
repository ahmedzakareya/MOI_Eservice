using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class Company
{
    public int Id { get; set; }

    public int? ServiceId { get; set; }

    public string? Parcel { get; set; }

    public string? PhoneNo { get; set; }



    public string? CompanyCivilId { get; set; }

    public string? OwnerName { get; set; }

    public string? LastUpdateUser { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public string? CompanyNo { get; set; }

    public string? AddressAutoNo { get; set; }
    public string? CentralNoMoci { get; set; }
    public string? CompanyActivity { get; set; }

    public string? DirCompanyAr { get; set; }

    public string? DirCompanyEn { get; set; }

    public string? OwnerCompanyAr { get; set; }

    public string? OwnerCompanyEn { get; set; }
    

    public string? RecordNo { get; set; }

    public string? CommercialLicNo { get; set; }


    public string? Email { get; set; }

    public string? ActivityCode { get; set; }

    public int? ActivityTypeId { get; set; }

    public string? UnitType { get; set; }

    public int? AddressId { get; set; }

    public string? Name { get; set; }

    public bool? IsBuilding { get; set; }
    public string? FaxNo { get; set; }

    [ForeignKey("AddressId")]
    public virtual Address? AddressNavigation { get; set; }
    [ForeignKey("ActivityTypeId")]
    public virtual ActivityTypesLookup? ActivityTypeNavigation { get; set; }

}
