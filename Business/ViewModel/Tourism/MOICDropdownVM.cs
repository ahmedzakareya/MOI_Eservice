using Business.ViewModel.Dynamic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Tourism
{
    public class MOICDropdownVM
    {
        public List<SelectListItem> ActivityTypes { get; set; }
        public List<SelectListItem> RequestTypes { get; set; }
    }
    public class MoicRequestVM
    {
        public int? CompanyId { get; set; }
        public int? buildingId { get; set; }

        public int? managerId { get; set; }
        
        public int? CompanyUserId { get; set; }

        [Display(Name = "الرقم الالي")]
        public string? AaliNumber { get; set; }


        [Display(Name = "المحافظة")]
        public string? Governrate { get; set; }

        [Display(Name = "المنطقة")]
        public string? Area { get; set; }

        [Display(Name = "القطعة")]
        public string? BlockNo { get; set; }

        [Display(Name = "الشارع")]
        public string? Street { get; set; }

        [Display(Name = "المبنى")]
        public string? BuildingName { get; set; }

        [Display(Name = "رقم المبني")]
        public string? BuildingNo { get; set; }
        [Display(Name = "الدور")]
        public string? FloorNo { get; set; }
        [Display(Name = "رقم الوحدة")]
        public string? UnitNo { get; set; }




        [Display(Name = "إسم مالك العقار")]
        public string UserName { get; set; }


        


      

        [Display(Name = "رمز النشاط")]
        public string ActivityCode { get; set; }


        

       


        [Display(Name = "نوع الطلب")]
        public string? ReqType { get; set; }
        public int? ReqTypeId {  get; set; } 
        public int? ActivityTypeId { get; set; }

        public int? LicId { get; set; }

        [DisplayName("نوع الطلب ")]
        public string? ReqTypeName { get; set; }
        [DisplayName("نشاط الشركة ")]

        public string? ActivityName { get; set; } 

        public string? reqno { get; set; }
        public List<AddAttachmentsRulesVM>? fileUploadConfigs { get; set; }

        [Display(Name = "المرفقات")]
        public List<NamedFile> NamedFile { get; set; }

        public LicencesVM? LicencesVM { get; set; }
    }
}
