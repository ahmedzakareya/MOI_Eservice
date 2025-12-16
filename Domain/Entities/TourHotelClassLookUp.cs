using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class TourHotelClassLookUp
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? ClassBranchId { get; set; }

    public int? ClassTypeId { get; set; }

    public int? CategoryId { get; set; }

    public bool? Status { get; set; }

    [ForeignKey("ClassBranchId")]
    public virtual TourClassBranchLookUp? TourClassBranchLookUp { get; set; }

    [ForeignKey("ClassTypeId")]
    public virtual TourClassTypeLookUp? TourClassTypeLookUp { get; set; }
    //public virtual IEnumerable<TourEvaluationLookUp> TourEvaluationLookUp { get; set; }

}
