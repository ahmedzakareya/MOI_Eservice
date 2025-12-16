using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class TransferTransaction
{
    public int Id { get; set; }

    public int? TransactionId { get; set; }

    public string? TransferType { get; set; }

    public string? PersonName1 { get; set; }

    public string? PersonName2 { get; set; }

    public string? PersonName3 { get; set; }

    public string? PersonName4 { get; set; }

    public string? CompanyName { get; set; }

    public string? PartnerNames { get; set; }

    public string? LastUpdateUser { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public string? CivilId { get; set; }

    public string? NationaltiyNo { get; set; }

    public string? QualificationDate { get; set; }

    public string? ProfessionalExperience { get; set; }

    public string? QualificationCountry { get; set; }

    public string? Qualification { get; set; }

    public string? CompanyCivilId { get; set; }

    public int? RequestId { get; set; }

    public string? Cname { get; set; }

    public string? OldLicenseeName { get; set; }

    public DateTime? BirthDate { get; set; }
}
