using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public partial class TourEvaluationListHotel
{
    public int Id { get; set; }

    public long? RequestId { get; set; }

    public int? LicId { get; set; }

    public string? ClassificationName { get; set; }

    public int? ClassificationId { get; set; }

    public int? HotelClassId { get; set; }

    public int? EvalitemId { get; set; }
    [ForeignKey("EvalitemId")]

    public virtual TourEvaluationLookUp? TourEvaluationLookUp { get; set; }
    [ForeignKey("RequestId")]
    public virtual MoiEserviceLicensesRequest? MoiEserviceLicensesRequest { get; set; }
    [ForeignKey("LicId")]

    public virtual Licence? Licence { get; set; }
    [ForeignKey("ClassificationId")]

    public virtual MoiClassification? MoiClassification { get; set; }
    [ForeignKey("HotelClassId")]

    public virtual TourHotelClassLookUp? TourHotelClassLookUp { get; set; } 


}
