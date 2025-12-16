using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Enums
{
   
    public enum ActivityTypeEnum
    {
        Hotel = 16,//فندق
        ApartmentHotel = 17,//شقق فندقية
        Resorts = 18,//منتجعات
        Parks = 19,//منتزهات
        Sailing = 20,//رحلات بحرية
        NewsService=21,
        ElectronicPress=22,
        ElectronicWeb=23,
        ElectronicElaw=24,
        ElectronicNews=25
    }
    public enum RequestTypeEnum
    {
        PreApprovementConvert = 4,
        PreApprovementNew = 30,

        AddMoIC = 18,
        ChangeAddressMOIC = 19,
        RenewMOIC = 20,
        DeleteMOIC = 21,
        RenewOrChangeMOIC = 22,
        Request = 1,
        Renew = 2,
        Classification = 8,
        ReClassification = 9,
        WhoConc = 17,
        ChangeData = 12,
        EndLicences = 3,
        Renouncement = 23,
        ReplacementOfLost = 24
    }
    public enum TransactionTypesEnum
    {
        [Display(Name = "طلب تعديل الترخيص : تغيير إسم الشركة")]
        ChangeCompaneName = 1,
        ChangeCommercialName = 2,
        [Display(Name = "طلب تعديل الترخيص : تغيير أسماء الشركاء")]
        ChangePartnerName = 3,
        [Display(Name = "طلب تعديل الترخيص : تغيير العنوان")]
        ChangeAddress = 4,
        [Display(Name = "طلب تعديل الترخيص : تغيير المدير المسئول")]
        ChangeManager = 9,
        ChangeActivity = 11,
        [Display(Name = "طلب تعديل الترخيص : تغيير إسم الترخيص")]
        ChangeLicencesName = 16,
        [Display(Name = "طلب تعديل الترخيص : تغيير نوع الترخيص")]

        ChangeLicencesType = 15,
        [Display(Name = "طلب تعديل الترخيص : تغيير الإيميل للمالك والمدير")]

        ChangeEmail = 14,
        [Display(Name = "طلب تعديل الترخيص : تغيير وسائل التواصل الإجتماعي")]

        ChangeSocialMedia = 13,
        [Display(Name = "طلب بدل فاقد")]
        ReplacementOfLost = 12,
    }
    public enum LicTypeEnum
    {
        Media_Organization_Individuals = 6,//مؤسسة إعلامية أشخاص
        Media_Organization_Company = 7,//مؤسسة إعلامية شركات
        Licensed_Media_Organization = 8,//مؤسسة إعلامية مرخصة
        Government_Entity = 5,//جهة حكومية
        Public_Benefit_Association = 4,//جمعية النفع العام
        Company = 2,//شركات
        Organization = 3,//مؤسسة 
        OrganizationOrPerson = 1,//مؤسسة أو شخص,
        PrintedNewspapersAndLicensedAVChannels=9//إصدار ترخيص صحف ورقية والقنوات مرئية والمسوعه المرخصه 
    }
    public enum licencesStatusEnum
    {
        Pending = 1,//معلقه
        Released = 2,//تم الاصدار
        Ending = 3,//تم الانها
        Updated = 4,//تم التحديث
        Changed = 5,//تم التعديل
        Refused=6
    }
    public enum CategoryClassificationEnum
    {
        Hotel = 2
        , Resorts = 2,
        HotelApartment = 1
    }
    public enum RequestStatusEnum
    {
        Received = 1,
        WaitingForReview = 2,
        WaitingForPayment = 5,
        FinalLicenseIssuingProcessing = 7,
        FinalLicenseIssued = 8,
        RequestDeclined = 9,
        CorrectData=17,
        WaitingApprovalCommittee=16,
        WaitingBankGuarantee=15,
        CriminalCase=14,
        PaymentAndFinalLicencesIssued=11 ,
        RequestRevokedToCompleteData  =12

    }

    public enum ServiceEnum
    {
        Elaw = 4,
        Tourism = 6,
        LocalPress = 3,
        Mosanafat = 5,
        publishing= 2,
    }
    public enum ServicePrefixPaymentEnum
    {
        Tourism = 11,
        Publishing = 22,
        Mosanafat = 33,
        Parties = 44,
        LocalPress = 55,
        Elaw = 0
    }

    public enum PermissionAdminEnum
    {
        View=3,
        Add=4,
        Delete=16,
        Edit=17
    }


    public enum SocialMediaEnum
    {
        Facebook=1,
        instegram=2,
        Website=3,
        Twitter=4
    }
    public enum ClassificationEnum
    {
        DeluxApart=6,
        FirstClassApart = 7,
        SecondClassApart = 8,
        OneStarHotel=1,
        TwoStarHotel = 2,
        ThreeStarHotel = 3,
        FourStarsHotel = 4,
        FiveStarsHotel = 5,
        NoClassificationHotel=10,
        NoClassificationApart=9,
        NoClassificationResort=11,
        FourStarsResort=12,
        FiveStarsResort=13,

    }

    public enum AccountTypeEnum
    {
        Kuwaiti=100,
        User=300
    }
}
