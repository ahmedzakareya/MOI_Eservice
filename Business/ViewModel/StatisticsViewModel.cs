using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class StatisticsViewModel
    {
        [DisplayName("طلبات إنشاءالموافقة المبدئية ")]
        public int? PreApprovementNew { get; set; }
        [DisplayName("طلبات تحويل عقار ")]
        public int? PreApprovementConvert { get; set; }
        [DisplayName(" كل الطلبات النشطة ")]
        public int? AllRequestActive { get; set; }
        [DisplayName(" كل الطلبات  ")]
        public int? AllRequests { get; set; }

        [DisplayName("طلب إصدار ترخيص تشغيلي")]
        public decimal? SumLicHoteAppaResor { get; set; }
        [DisplayName("طلب إصدار أنشطة سياحية")]
        public decimal? SumLicActivities { get; set; }
        [DisplayName("طلب لمن يهمه الأمر")]
        public decimal? SumLicWhoConc { get; set; }
        [DisplayName("طلب بدل فاقد ")]
        public decimal? SumLicReplacement { get; set; }
        [DisplayName("طلب تصنيف")]
        public decimal? SumLicClassify { get; set; }
        [DisplayName("طلب تعديل")]
        public decimal? SumLicEdit { get; set; }
        [DisplayName("طلب تجديد")]
        public decimal? SumLicRenew { get; set; }
        [DisplayName(" الطلبات المرفوضة ")]
        public int? RefusedRequest { get; set; }
        [DisplayName("كل التراخيص")]
        public int? AllLicences { get; set;}
        [DisplayName(" التراخيص النشطة")]
        public int? LicencesActive { get; set; }
        [DisplayName(" التراخيص  سوف تنتهي ")]
        public int? LicencesWillEnd { get; set; }

        [DisplayName(" فندق ")]
        public int? LicencesHotel { get; set; }
        [DisplayName(" شقق فندقية ")]
        public int? LicencesApartmentHotel { get; set; }
        [DisplayName(" منتجعات ")]
        public int? LicencesResort { get; set; }
        [DisplayName(" تنظيم وتأجير الرحلات السياحية والبرية والبحرية والإرشاد السياحي الداخلي ")]
        public int? LicencesSailing { get; set; }
        [DisplayName(" منتزهات الإستجمام والشواطيء والسواحل  ")]
        public int? LicencesParks { get; set; }
        [DisplayName(" التراخيص المنتهية")]
        public int? LicencesEnded { get; set; }

        [DisplayName(" الطلبات الجديدة")]
        public int? NewRequests { get; set; }
        [DisplayName(" طلبات التعديل")]
        public int? ChangeRequest { get; set; }
        [DisplayName(" طلبات تغيير إسم الشركة")]

        public int? ChangeCompanyName { get; set; }
        [DisplayName(" طلبات لمن يهمه الأمر")]

        public int? WhoConc { get; set; }
        [DisplayName(" طلبات تغيير الإسم التجاري")]

        public int? ChangeCommercialName { get; set; }
        [DisplayName(" طلبات تغيير إسم الشريك")]

        public int? ChangePartner { get; set; }
        [DisplayName(" طلبات تغيير العنوان")]

        public int? ChangeAddress { get; set; }
        [DisplayName(" طلبات تغيير نوع الترخيص")]

        public int? ChangeLicencesType { get; set; }
        [DisplayName(" طلبات تغيير إسم الترخيص")]

        public int? ChangeLicencesName{ get; set; }
        // التنازل
        [DisplayName(" طلبات تغيير المالك")]

        public int? ChangeOwnerRequest { get; set; }
        [DisplayName(" طلبات الإنهاء")]

        public int? EndLicenseRequests { get; set; }
        [DisplayName(" طلبات التجديد")]

        public int? RenewRequests { get; set; }
        [DisplayName(" طلبات تغيير المدير")]

        public int? ChangeManagerRequests { get; set; }
        [DisplayName(" طلبات تغيير النشاط")]

        public int? ChangeActivityRequests { get; set; }
        [DisplayName(" طلبات بدل فاقد")]

        public int? ReplacementOfLostRequests { get; set; }
        [DisplayName(" طلبات تغيير البريد الإلكتروني")]

        public int? ChangeEmail { get; set; }
        [DisplayName(" طلبات تغيير وسائل التواصل الإجتماعي")]

        public int? ChangeSocialMedia { get; set; }


        [DisplayName("طلبات الحفلات")]
        public int? PartiesRequests { get; set; }

        public int? WithoutClassification { get; set; }
        public int? OneStar { get; set; }
        public int? TwoStar { get; set; }
        public int? FourStar { get; set; }
        public int? ThreeStars { get; set; }
        public int? FiveStars { get; set; }
        public int? Delux { get; set; }
        public int? FirstClass { get; set; }
        public int? SecondClass { get; set; }

        public List<ClassificationCountVM>? ClassificationStats { get; set; }

        public int? ServiceId { get; set; }
       
        public List<RequestVM>? RequestModel { get; set; }
    }

    public class ClassificationCountVM
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public int? Count { get; set; }
    }
}
