using Business.ViewModel.ClassificationVM;
using Business.ViewModel.Dynamic;
using Business.ViewModel.HomePage;
using Business.ViewModel.Tourism;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Elaw
{
    public class RequestLicPerPerson
    {
        public RequestVM? RequestVM { get; set; }
        
        public SocialMediaVM? socialMediaVM { get; set; }
        [DisplayName("فيسبوك")]
        public string? FacebookUrl { get; set; }
        [DisplayName("تويتر")]

        public string? Twitter {  get; set; }
        [DisplayName("الموقع الإلكتروني")]

        public string? website { get; set; }
        [DisplayName("الإنستجرام")]

        public string? Instagram { get; set; }
        [DisplayName("إسم الشريك الأول")]

        public string? Partner1 { get; set; }
        [DisplayName("إسم الشريك الثاني")]

        public string? Partner2 { get; set; }
        [DisplayName("إسم الشريك الثالث")]

        public string? Partner3 { get; set; }
        [DisplayName("إسم الشريك الرابع")]

        public string? Partner4 { get; set; }
        [DisplayName("إسم الشريك الخامس")]

        public string? Partner5 { get; set; }
        public string? Partner6 { get; set; }
        public int? ActivityTypeId { get; set; }
        public int? QualificationApplicantId { get; set; }
        public int? QualificationManagerId { get; set; }
        [DisplayName("المؤهل العلمي")]

        public List<SelectListItem>? QualificationSelectedList { get; set; }
        [DisplayName("إسم الوسيلة الإعلامية")]
        public string? LicName { get; set; }

        public AspnetUserVM? AspnetUserVM { get; set; }
        public PersonVM? PersonVM { get; set; }
        public LicencesInfoVM? LicencesInfoVM { get; set; }
        public List<SelectListItem>? ActivitySelectedList { get; set; }
        public List<AddAttachmentsRulesVM>? FileUploadConfigs { get; set; }
        public List<NamedFile>? NamedFile { get; set; }
        public List<FileSaveResponseVM>? saveResponseVMs { get; set; }
        public bool OwnerSameManager { get; set; }

    }
    public class MediaCheckResult
    {
        public bool exists { get; set; }
        public string message { get; set; }
    }
    public class RequestLicPerPersonApi
    {

     
        public int? LicTypeId { get; set; }
        public string? SessionName { get; set; }
        public string? SessionCivilId { get; set; }
        public string? Name1Applicant {  get; set; }
        public string? Name2Applicant { get; set; }
        public string? Name3Applicant { get; set; }
        public string? Name4Applicant { get; set; }

        public string? Name1Manager { get; set; }
        public string? Name2Manager { get; set; }
        public string? Name3Manager { get; set; }
        public string? Name4Manager { get; set; }

        public string? PhoneManager {  get; set; }  
        public string? EmailManager { get; set; }
        public string? NationalitynameManager {  get; set; }
        public string? NationalitynameApplicant { get; set; }

        public string? AaliNOApplicant { get; set; }
        public string? AreaApplicant { get; set; }
        public string? GovernateApplicant { get; set; }
        public string? BlockApplicant { get; set; }
        public string? StreetApplicant { get; set; }
        public string? BuildingNOApplicant { get; set; }
        public string? BuildingNameApplicant { get; set; }
        public string? UnitNOApplicant { get; set; }
        public string? FloorNOApplicant { get; set; }
        public string? AaliNOManager{ get; set; }
        public string? AreaManager{ get; set; }
        public string? GovernateManager{ get; set; }
        public string? BlockManager { get; set; }
        public string? StreetManager { get; set; }
        public string? BuildingNoManager { get; set; }
        public string? BuildingNameManager { get; set; }
        public string? UnitNOManager { get; set; }
        public string? FloorNOManager  { get; set; }

        public string? FacebookUrl { get; set; }
   

        public string? Twitter { get; set; }
   

        public string? website { get; set; }
 
        public string? Instagram { get; set; }
  
        public string? Email { get; set; }
  

        public string? CivilId { get; set; }



        public string? Mobile { get; set; }

        public int? ActivityTypeId { get; set; }
        public int? QualificationApplicantId { get; set; }
        public int? QualificationManagerId { get; set; }
       
   
    

        public string? Reqno { get; set; }
        
        public int? ReqtypeId { get; set; }
      
        public bool OwnerSameManager { get; set; }

      


    
    
 

        public string? Licowner { get; set; }

        public string? Licname { get; set; }

        public DateTime? Licreqtime { get; set; }

        public int? RequestStatusId { get; set; }

        


        public string? AppCivilId { get; set; }
        public long? SequenceNo { get; set; }
        public string? ManCivilId { get; set; }
        public int? LicStatusId { get; set; }


   
        public List<NamedFile>? NamedFile { get; set; }
        public List<FileSaveResponseVM>? saveResponseVMs { get; set; }
    }

    public class RequestLicPerCompanyApi
    {


        public int? LicTypeId { get; set; }

        public string? SessionName { get; set; }
        public string? SessionCivilId { get; set; }
        public string? Name1Manager { get; set; }
        public string? Name2Manager { get; set; }
        public string? Name3Manager { get; set; }
        public string? Name4Manager { get; set; }

        public string? PhoneManager { get; set; }
        public string? EmailManager { get; set; }
        public string? NationalitynameManager { get; set; }
        public string? CompanyName {  get; set; }    
        public string? CompanyCivilId { get; set; }
        public string? CompanyPhone { get; set; }
        public string? CompanyFax {  get; set; }
        public string? CompanyEmail {  get; set; }
        public string? PartnerName1 {  get; set; }
        public string? PartnerName2 { get; set; }
        public string? PartnerName3 { get; set; }
        public string? PartnerName4 { get; set; }

        public string? PartnerName5 { get; set; }

        public string? AaliNOCompany { get; set; }
        public string? AreaCompany { get; set; }
        public string? GovernateCompany { get; set; }
        public string? BlockCompany { get; set; }
        public string? StreetCompany { get; set; }
        public string? BuildingNOCompany { get; set; }
        public string? BuildingNameCompany { get; set; }
        public string? UnitNOCompany { get; set; }
        public string? FloorNOCompany { get; set; }
        public string? AaliNOManager { get; set; }
        public string? AreaManager { get; set; }
        public string? GovernateManager { get; set; }
        public string? BlockManager { get; set; }
        public string? StreetManager { get; set; }
        public string? BuildingNoManager { get; set; }
        public string? BuildingNameManager { get; set; }
        public string? UnitNOManager { get; set; }
        public string? FloorNOManager { get; set; }

        public string? FacebookUrl { get; set; }


        public string? Twitter { get; set; }


        public string? website { get; set; }

        public string? Instagram { get; set; }

        public string? Email { get; set; }


        public string? CivilId { get; set; }



        public string? Mobile { get; set; }

        public int? ActivityTypeId { get; set; }
        public int? QualificationApplicantId { get; set; }
        public int? QualificationManagerId { get; set; }




        public string? Reqno { get; set; }

        public int? ReqtypeId { get; set; }

        public bool OwnerSameManager { get; set; }








        public string? Licowner { get; set; }

        public string? Licname { get; set; }

        public DateTime? Licreqtime { get; set; }

        public int? RequestStatusId { get; set; }




        public string? AppCivilId { get; set; }
        public long? SequenceNo { get; set; }
        public string? ManCivilId { get; set; }
        public int? LicStatusId { get; set; }



        public List<NamedFile>? NamedFile { get; set; }
        public List<FileSaveResponseVM>? saveResponseVMs { get; set; }
    }

    public class PostRequestApiModel
    {
        public int? CompanyId { get; set; }
       public string? ManagerCivilid { get; set; }
        public int? LictypeId { get; set; }
        public string? LicName { get; set; }
        public string? LicOwner {  get; set; }  
        public string? NewApplicantName1 { get; set; }
        public string? NewApplicantName2 { get; set; }
        public string? NewApplicantName3 { get; set; }
        public string? NewApplicantName4 { get; set; }
        public string? NewPartner1 { get; set; }
        public string? NewPartner2 { get; set; }
        public string? NewPartner3 { get; set; }
        public string? NewPartner4 { get; set; }
        public string? NewPartner5 { get; set; }

        public string? NewAaliNoApplicant { get; set; }
        public string? NewAreaApplicant { get; set; }
      public int? NewQualificationApplicant { get; set; }

        public string? NewGovernateApplicant { get; set; }
        public string? NewBlockApplicant { get; set; }
        public string? NewStreetApplicant { get; set; }
        public string? NewFloorNoApplicant { get; set; }
        public string? NewUnitNoApplicant { get; set; }
        public string? NewBuildingNoApplicant { get; set; }
        public string? NewBuildingNameApplicant { get; set; }
        public string? NewCivilIdApplicant { get; set; }
        public string? NewEmailApplicant { get; set; }
        public string? NewMobileApplicant { get; set; }
        public string? OldApplicantName1 { get; set; }
        public string? OldApplicantName2 { get; set; }
        public string? OldApplicantName3 { get; set; }
        public string? OldApplicantName4 { get; set; }
        public string? OldAaliNoApplicant { get; set; }
        public string? OldAreaApplicant { get; set; }
        public int? OldQualificationApplicant { get; set; }

        public string? OldGovernateApplicant { get; set; }
        public string? OldBlockApplicant { get; set; }
        public string? OldStreetApplicant { get; set; }
        public string? OldFloorNoApplicant { get; set; }
        public string? OldUnitNoApplicant { get; set; }
        public string? OldBuildingNoApplicant { get; set; }
        public string? OldBuildingNameApplicant { get; set; }
        public string? OldCivilIdApplicant { get; set; }
        public string? OldEmailApplicant { get; set; }
        public string? OldMobileApplicant { get; set; }
        public string? NewManagerName1 { get; set; }
        public string? NewManagerName2 { get; set; }
        public string? NewManagerName3 { get; set; }
        public string? NewManagerName4 { get; set; }
        public string? NewAaliNoManager { get; set; }
        public string? NewAreaManager { get; set; }
        public int? NewQualificationManager { get; set; }

        public string? NewGovernateManager { get; set; }
        public string? NewBlockManager { get; set; }
        public string? NewStreetManager { get; set; }
        public string? NewFloorNoManager { get; set; }
        public string? NewUnitNoManager { get; set; }
        public string? NewBuildingNoManager { get; set; }
        public string? NewBuildingNameManager { get; set; }
        public string? NewCivilIdManager { get; set; }
        public string? NewEmailManager{ get; set; }
        public string? NewMobileManager { get; set; }
        public string? OldManagerName1 { get; set; }
        public string? OldManagerName2 { get; set; }
        public string? OldManagerName3 { get; set; }
        public string? OldManagerName4 { get; set; }
        public string? OldAaliNoManager { get; set; }
        public string? OldAreaManager { get; set; }
        public int? OldQualificationManager { get; set; }

        public string? OldGovernateManager { get; set; }
        public string? OldBlockManager { get; set; }
        public string? OldStreetManager { get; set; }
        public string? OldFloorNoManager { get; set; }
        public string? OldUnitNoManager { get; set; }
        public string? OldBuildingNoManager { get; set; }
        public string? OldBuildingNameManager { get; set; }
        public string? OldCivilIdManager { get; set; }
        public string? OldEmailManager { get; set; }
        public string? OldMobileManager { get; set; }
        public string? NewFacebook { get; set; }
        public string? NewInsta { get; set; }
        public string? NewWebSite { get; set; }
        public string? NewTwitter { get; set; }
        public string? OldLicencesName {  get; set; }   
        public int? ReqtypeId { get; set; }
        public int? EndingReasonId { get; set; }
       


        public int? CompanyUserId { get; set; }
    
        public int? ManId { get; set; }
        public int? LicId { get; set; }
        public long SequenceNo { get; set; }
      
        public decimal? Amount { get; set; }
       
        public List<FileSaveResponseVM>? saveResponseVMs { get; set; }
        public List<int>? SelectedTransactionTypeIds { get; set; }
        public string? accountTypeId { get; set; }
        public int? NewLicencesTpeId { get; set; }
        public int? OldLicencesTpeId { get; set; }


        public string? AppCivilId { get; set; }
        public string? MandoobId { get; set; }
        public int? AppId { get; set; }
        public int? EndingLicenseReason { get; set; }
        public int? ActivityTypeId { get; set; }
        [Display(Name = "الرقم المدني للمالك")]
        public string? UserCivilID { get; set; }
        public string? SessionName { get; set; }
        public string? SessionCivilId { get; set; }

        [Display(Name = "إسم مالك العقار")]
        public string? UserName { get; set; }


       

      
      
        // بيانات الرخصة الجديدة
        [DisplayName("إسم الترخيص الجديد")]

        public string? NewLicencesName { get; set; }

     
 

        public string? reqno { get; set; }

        public string? LicNo { get; set; }




    }
    public class RequestElawBaseVM
    {
        public int? CompanyId { get; set; }
 
        public int ReqtypeId { get; set; }
        public int? ActivityTypeId { get; set; }
        public List<LicencesTypeVM>? licencesTypes { get; set; }
        public List<int>? SelectedTransactionTypeIds { get; set; }
        public List<SocialMediaVM>? socialMediaVMs { get; set; }
        public List<PartnerVM>? PartnerVMs { get; set; }

        public LicencesVM? LicencesVM { get; set; }
        public List<SelectListItem>? EndingReasons { get; set; }
        public List<SelectListItem>? QualificationSelectedList { get; set; }

        // Applicant Info
        [DisplayName("الإسم الأول للمالك الجديد")]
        public string? NewApplicantName1 { get; set; }
        [DisplayName("الإسم الثاني للمالك الجديد")]

        public string? NewApplicantName2 { get; set; }
        [DisplayName("الإسم الثالث للمالك الجديد")]

        public string? NewApplicantName3 { get; set; }
        [DisplayName("الإسم الرابع للمالك الجديد")]

        public string? NewApplicantName4 { get; set; }
        [DisplayName("إسم الشريك الأول")]

        public string? NewPartner1 { get; set; }
        [DisplayName("إسم الشريك الثاني")]

        public string? NewPartner2 { get; set; }
        [DisplayName("إسم الشريك الثالث")]

        public string? NewPartner3 { get; set; }
        [DisplayName("إسم الشريك الرابع")]

        public string? NewPartner4 { get; set; }
        [DisplayName("إسم الشريك الخامس")]

        public string? NewPartner5 { get; set; }

        [DisplayName("الرقم المدني للمالك الجديد")]

        public string? NewCivilIdApplicant { get; set; }

        [DisplayName("البريد الإلكتروني للمالك الجديد")]

        public string? NewEmailApplicant { get; set; }

        [DisplayName("رقم الهاتف للمالك الجديد")]

        public string? NewMobileApplicant { get; set; }

        [DisplayName("الرقم الآلي للمالك الجديد")]

        public string? NewAaliNoApplicant { get; set; }

        [DisplayName("المنطقة")]

        public string? NewAreaApplicant { get; set; }

        [DisplayName("المؤهل")]

        public int? NewQualificationApplicant { get; set; }
        [DisplayName("المحافظة")]


        public string? NewGovernateApplicant { get; set; }

        [DisplayName("القطعة")]

        public string? NewBlockApplicant { get; set; }

        [DisplayName("الشارع")]

        public string? NewStreetApplicant { get; set; }

        [DisplayName("الدور")]

        public string? NewFloorNoApplicant { get; set; }
        [DisplayName("رقم الوحدة")]


        public string? NewUnitNoApplicant { get; set; }
        [DisplayName("رقم المبني")]


        public string? NewBuildingNoApplicant { get; set; }
        [DisplayName("إسم المبني")]

        public string? NewBuildingNameApplicant { get; set; }


        // Manager Info
        [DisplayName("الإسم الأول للمدير الجديد")]

        public string? NewManagerName1 { get; set; }
        [DisplayName("الإسم الثاني للمدير الجديد")]

        public string? NewManagerName2 { get; set; }
        [DisplayName("الإسم الثالث للمدير الجديد")]

        public string? NewManagerName3 { get; set; }
        [DisplayName("الإسم الرابع للمدير الجديد")]

        public string? NewManagerName4 { get; set; }


       


       // public string? NewEmailManager { get; set; }



        [DisplayName("الرقم الآلي الجديد")]

        public string? NewAaliNoManager { get; set; }

        [DisplayName("المنطقة")]

        public string? NewAreaManager { get; set; }

        [DisplayName("المؤهل")]

        public int? QualificationManagerId { get; set; }

        [DisplayName("المحافظة")]

        public string? NewGovernateManager { get; set; }

        [DisplayName("القطعة")]

        public string? NewBlockManager { get; set; }

        [DisplayName("الشارع")]

        public string? NewStreetManager { get; set; }

        [DisplayName("الدور")]

        public string? NewFloorNoManager { get; set; }

        [DisplayName("رقم الوحدة")]

        public string? NewUnitNoManager { get; set; }

        [DisplayName("رقم المبني")]

        public string? NewBuildingNoManager { get; set; }

        [DisplayName("إسم المبني")]

        public string? NewBuildingNameManager { get; set; }


        public string? NewAaliNo { get; set; }

        [DisplayName("المنطقة")]

        public string? NewArea { get; set; }

        
        [DisplayName("المحافظة")]

        public string? NewGovernate{ get; set; }

        [DisplayName("القطعة")]

        public string? NewBlock { get; set; }

        [DisplayName("الشارع")]

        public string? NewStreet { get; set; }

        [DisplayName("الدور")]

        public string? NewFloorNo { get; set; }

        [DisplayName("رقم الوحدة")]

        public string? NewUnitNo { get; set; }

        [DisplayName("رقم المبني")]

        public string? NewBuildingNo { get; set; }

        [DisplayName("إسم المبني")]

        public string? NewBuildingName { get; set; }


        // Social Media
        [DisplayName("Facebook")]

        public string? NewFacebook { get; set; }
        public string? OldFacebook { get; set; }
        [DisplayName("Instagram")]

        public string? NewInsta { get; set; }
        public string? OldInsta { get; set; }
        [DisplayName("Website")]

        public string? NewWebSite { get; set; }
        public string? OldWebSite { get; set; }
        [DisplayName("Twitter")]

        public string? NewTwitter { get; set; }
        public string? OldTwitter { get; set; }

        // Licence Type
        [DisplayName("نوع الترخيص الجديد")]

        public int? NewLicencesTpeId { get; set; }
        public int? OldLicencesTpeId { get; set; }

        // General Info
        public string? AppCivilId { get; set; }
        public string? MandoobId { get; set; }
        public string? AppId { get; set; }
  
        public string? accountTypeId { get; set; }
        public string? SessionName { get; set; }
        public string? SessionCivilId { get; set; }

 

        public string? CompanyCivilId { get; set; }

        [DisplayName("رقم الهاتف للمدير الجديد")]

        public string? NewManagerMobile { get; set; }
        [DisplayName("البريد الإلكتروني للمدير الجديد")]

        public string? NewManagerEmail { get; set; }

        [DisplayName("الرقم المدني للمدير الجديد")]

        public string? NewManangerCivilId { get; set; }
 
        public string? NewLicencesName { get; set; }
     

        // Address change
        //public string? NewAaliNumber { get; set; }
        //public string? OldAaliNumber { get; set; }

        //public string? NewArea { get; set; }
        //public string? OldArea { get; set; }

        //public string? NewGovernrate { get; set; }
        //public string? OldGovernrate { get; set; }

        //public string? NewBlockNo { get; set; }
        //public string? OldBlockNo { get; set; }

        //public string? NewStreet { get; set; }
        //public string? OldStreet { get; set; }

        //public string? NewBuildingName { get; set; }
        //public string? OldBuildingName { get; set; }

        //public string? NewBuildingNo { get; set; }
        //public string? OldBuildingNo { get; set; }

        //public string? NewUnitNo { get; set; }
        //public string? OldUnitNo { get; set; }

        //public string? NewFloorNo { get; set; }
        //public string? OldFloorNo { get; set; }

        public int? ManId { get; set; }
        public string? LicNo { get; set; }
        public int? LicId { get; set; }
        public string? LicencesName { get; set; }

        

        public string? NameCompany { get; set; }
        public string? reqno { get; set; }
        public long SequenceNo { get; set; }

        public List<AddAttachmentsRulesVM>? FileUploadConfigs { get; set; }
        public List<NamedFile>? NamedFile { get; set; }
        public List<FileSaveResponseVM>? saveResponseVMs { get; set; }

       
        public int? EndingReasonId { get; set; }

        public LicencesInfoVM? LicencesInfo { get; set; }
        public AspnetUserVM? AspnetUserVM { get; set; }
    }

}
