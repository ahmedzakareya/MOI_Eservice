using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class TourClassBranchLookUp
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? ClassId { get; set; }
    //[ForeignKey("ClassId")]
    //public virtual MoiClassification? Calssification { get; set; }
    //[NotMapped]
    public virtual ICollection<TourHotelClassLookUp>? TourHotelClassLookUp { get; set; }


}
