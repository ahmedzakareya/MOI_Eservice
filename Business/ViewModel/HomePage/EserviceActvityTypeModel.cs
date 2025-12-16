using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.HomePage
{
    public class EserviceActvityTypeModel
    {
     public int Id { get; set; }

    public string? NameAr { get; set; }

    public int? MainLicenseId { get; set; }

    public int? ServiceId { get; set; }

    public string? ActivityCode { get; set; }

    public string? NameEn { get; set; }
     public string EserviceUrl { get; set; }
     public string? EserviceName {  get; set; }  
     public string? EserviceId { get; set; }
    public Eservice? Eservice { get; set; }
    }
}
