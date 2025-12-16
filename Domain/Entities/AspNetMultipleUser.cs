using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class AspNetMultipleUser
{
    public int Id { get; set; }

    public string? MainUserId { get; set; }

    public string? MandoobId { get; set; }

    public bool? IsActive { get; set; }
    [ForeignKey("MainUserId")]
    public virtual AspNetUser? User { get; set; }
    [ForeignKey("MandoobId")]

    public virtual AspNetUser? Mandoob {  get; set; }
    public virtual ICollection<AspNetMultipleLicenseUser> DelegatedLicenses { get; set; } = new List<AspNetMultipleLicenseUser>();

}
