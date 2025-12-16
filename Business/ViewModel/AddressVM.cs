using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class AddressVM
    {
        public int Id { get; set; }
        [Display(Name = "المنطقة")]

        public string? Area { get; set; }
        [Display(Name = "المحافظة")]

        public string? GovernorateArabic { get; set; }
        [Display(Name = "القطعة")]

        public string? BlockArabic { get; set; }
        [Display(Name = "الشارع")]

        public string? StreetArabic { get; set; }

        public string? City { get; set; }
        [Display(Name = "الدور")]

        public string? FloorNo { get; set; }
        [Display(Name = "الرقم الالي")]

        public string? AalliNo { get; set; }

        public int? ServiceId { get; set; }

        public string? Address1 { get; set; }
       

        public string? Name { get; set; }
        [Display(Name = "رقم الوحدة")]

        public string? UnitNo { get; set; }

        public string? ActivityCode { get; set; }

        public int? ActivityTypeId { get; set; }

        public int? ClassificationId { get; set; }
        [Display(Name = "مساحة الارض")]

        public string? AreaSize { get; set; }
        [Display(Name = "رقم المخطط المساحي")]

        public string? AreaChartNo { get; set; }

        public int? AreaId { get; set; }

        public int? GovernateId { get; set; }
        [Display(Name = "إسم المبني")]

        public string? BuildingName { get; set; }
        [Display(Name = "رقم المبني")]

        public string? BuildingNo { get; set; }
        public int Amount { get; set; }

    }
}
