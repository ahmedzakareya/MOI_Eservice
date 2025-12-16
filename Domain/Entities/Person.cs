using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class Person
{
    public int Id { get; set; }

    public string? Name1 { get; set; }

    public string? Name2 { get; set; }

    public string? Name3 { get; set; }

    public string? Name4 { get; set; }
    public string? PersonName { get; set; }


    public string? CivilId { get; set; }

    public int? NationalityId { get; set; }

    public string? NationaltiyNo { get; set; }

    public int? QualificationId { get; set; }

    public string? QualificationDateOld { get; set; }

    public int? QualificationCountryId { get; set; }

    public int? Experience { get; set; }

    public int? CategoryId { get; set; }

    public string? LastUpdateUser { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public string? CompanyNo { get; set; }

    public DateTime? BirthDate { get; set; }

    public DateTime? QualificationDate { get; set; }

    public int? PersonTypeId { get; set; }

    public string? Education { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? AddressId { get; set; }

    public string? NationaliyName { get; set; }
    public bool? IsApplicant { get; set; }

    public int? ServiceId { get; set; }
    [ForeignKey("AddressId")]
    public virtual Address? AddressNavigation { get; set; }
    [ForeignKey("QualificationId")]
    public virtual QualificationsLookup? QualificationsLookup { get; set; }

}
