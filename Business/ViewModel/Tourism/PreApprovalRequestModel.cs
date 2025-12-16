using Business.ViewModel.ClassificationVM;
using Business.ViewModel.Dynamic;
using Business.ViewModel.HomePage;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Tourism
{
    public class PreApprovalRequestModel
    {
        public int CompanyId { get; set; }
        public int ReqtypeId { get; set; }  
        public int? CompanyUserId { get; set; }
        public string? accountTypeId { get; set; }
        public string? LicenseNo { get; set; }

        public string? AppCivilId { get; set; }
        public string? MandoobId { get; set; }
        public string? AppId { get; set; }
        public int? ActivityTypeId {  get; set; }   
        [Display(Name = "الرقم المدني للمالك")]
        public string? UserCivilID { get; set; }


        [Display(Name = "إسم مالك العقار")]
        public string? UserName { get; set; }


        [Display(Name = "رقم المدني للجهة")]
        public string? CompanyCivilId { get; set; }


        [Display(Name = "الشركة المديرة")]
        public string? DirCompanyAr { get; set; }



        [Display(Name = "الشركة المالكة")]
        public string? OwnerCompanyAr { get; set; }


        [Display(Name = "رقم السجل التجاري")]
        public string? RecordNo { get; set; }
        [Display(Name = "رقم الترخيص التجاري")]
        public string? CommercialLicNo { get; set; }


        [Display(Name = "العنوان")]
        public string? OwnerCoAddress { get; set; }

        [Display(Name = "نشاط الشركة")]
        public string? CompanyActivity { get; set; }

        [Display(Name = "رمز النشاط")]
        public string? ActivityCode { get; set; }

        [Display(Name = "مساحة الارض")]
        public string? AreaSize { get; set; }

        [Display(Name = "رقم المخطط المساحي")]
        public string? AreaChartNo { get; set; }

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

        //-------- Manager Details ----------------
        [Display(Name = "الرقم المدني للمدير")]
        public string? ManCivilId { get; set; }

        [Display(Name = "اسم للمدير")]
        public string? ManagerName { get; set; }

        [Display(Name = "رقم جوال للمدير")]
        public string? ManagerMobile { get; set; }

        [Display(Name = "البريد الالكتروني للمدير")]
        public string? ManagerEmail { get; set; }
        [Display(Name = "الرقم المدني لمدير المبيعات")]
        public string? SalesManagerCivilId { get; set; }

        [Display(Name = "اسم مدير المبيعات")]
        public string? SalesManagerName { get; set; }

        [Display(Name = "رقم جوال مدير المبيعات")]
        public string? SalesManagerMobile { get; set; }

        [Display(Name = "البريد الالكتروني لمدير المبيعات")]
        public string? SalesManagerEmail { get; set; }
        [Display(Name = "الرقم المدني لمدير التسويق")]
        public string? MarketingManagerCivilId { get; set; }

        [Display(Name = "اسم مدير التسويق")]
        public string? MarketingManagerName { get; set; }

        [Display(Name = "رقم جوال مدير التسويق")]
        public string? MarketingManagerMobile { get; set; }

        [Display(Name = "البريد الالكتروني لمدير التسويق")]
        public string? MarketingManagerEmail { get; set; }
        [Display(Name = "الرقم المدني لمدير العمليات")]
        public string? OperationManagerCivilId { get; set; }

        [Display(Name = "اسم مدير العمليات")]
        public string? OperationManagerName { get; set; }

        [Display(Name = "رقم جوال مدير العمليات")]
        public string? OperationManagerMobile { get; set; }

        [Display(Name = "البريد الالكتروني لمدير المبيعات")]
        public string? OperationManagerEmail { get; set; }
        [DisplayName("نوع النشاط")]
        public string? ActivityName { get; set; }
        //----------------------------------------

        public List<SelectListItem>? Activities { get; set; }
        public LicencesInfoVM LicencesInfoVM { get; set; }
        public string? reqno { get; set; }

        //public string? _Reqno { get; set; }

        public List<AddAttachmentsRulesVM>? fileUploadConfigs { get; set; }

        [Display(Name = "المرفقات")]
        //public List<IFormFile> ContractDocuments { get; set; }
        public List<NamedFile> NamedFile { get; set; }
        //public Dictionary<string, List<IFormFile>> ContractDocuments { get; set; }
       

        //----------- Attachement Table ---------------
        // public List<MoiEserviceRequestsAttachModel>? PreApprovAttachmnt { get; set; }


        [ForeignKey("RequestId")]
        public virtual RequestVM? Request { get; set; }
        [ForeignKey("ManagerId")]
        public virtual PersonVM? Manager { get; set; }
        [ForeignKey("AppId")]
        public virtual PersonVM? AppUser { get; set; }
        [ForeignKey("MandoobId")]
        public virtual AspNetUser? Mandoob { get; set; }

        [ForeignKey("CompanyId")]
        public virtual CompanyVM? Company { get; set; }
        [ForeignKey("BuildingId")]
        public virtual CompanyVM? Building { get; set; }
        [ForeignKey("LicTypeId")]
        public virtual LicencesTypeVM? LicenceTypesLookup { get; set; }
        [ForeignKey("ActivityTypeId")]
        public virtual ActivityTypeVM? ActivityTypesLookup { get; set; }
        [ForeignKey("ReqStatusId")]
        public virtual RequestStatusVM? RequestStatusLookup { get; set; }
        [ForeignKey("LicStatusId")]
        public virtual LicencesStatusVM? LicenseStatusLookup { get; set; }
    }
    public class NamedFile
    {
        public string FieldName { get; set; }
        public string Flag { get; set; }
        public string LabelName { get; set; }
        public IFormFile File { get; set; }
        public bool IsRequired { get; set; }
    }

    public class CheckPreAprroval
    {
        public string? CivilId { get; set; }
        public int? id { get; set; }
        public string? PreApprove { get; set; }
    }
    public class PreApprovalApiModel
    {
        
        public int ReqtypeId { get; set; }
    
       

        

     
        public string? ManCivilId { get; set; }

        public long SequenceNo { get; set; }
        public string? UnitNo { get; set; }
        public string? FloorNo { get; set; }
 
        
        public List<FileSaveResponseVM>? saveResponseVMs { get; set; }
     
        public string? accountTypeId { get; set; }
       
        public string? AppCivilId { get; set; }
        public string? MandoobId { get; set; }
        public int? AppId { get; set; }

        public int? ActivityTypeId { get; set; }
        [Display(Name = "الرقم المدني للمالك")]
        public string? UserCivilID { get; set; }
       

        public string? SessionName { get; set; }
        public string? SessionCivilId { get; set; }

        [Display(Name = "إسم مالك العقار")]
        public string? UserName { get; set; }


        [Display(Name = "رقم المدني للجهة")]
        public string? CompanyCivilId { get; set; }


        [Display(Name = "الشركة المديرة")]
        public string? DirCompanyAr { get; set; }

        

        [Display(Name = "الشركة المالكة")]
        public string? OwnerCompanyAr { get; set; }


        [Display(Name = "رقم السجل التجاري")]
        public string? RecordNo { get; set; }
        [Display(Name = "رقم الترخيص التجاري")]
        public string? CommercialLicNo { get; set; }


        [Display(Name = "العنوان")]
        public string? OwnerCoAddress { get; set; }

        [Display(Name = "نشاط الشركة")]
        public string? CompanyActivity { get; set; }

        [Display(Name = "رمز النشاط")]
        public string? ActivityCode { get; set; }

        

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
        public string? AreaSize { get; set; }
        public string? AreaChartNo { get; set; }

        //-------- Manager Details ----------------

        [Display(Name = "اسم للمدير")]
        public string? ManagerName { get; set; }

        [Display(Name = "رقم جوال للمدير")]
        public string? ManagerMobile { get; set; }

        [Display(Name = "البريد الالكتروني للمدير")]
        public string? ManagerEmail { get; set; }

        //----------------------------------------
        

        public string? reqno { get; set; }

      




    }
    public class PreApprovalRequestApiModel
    {
        public int? AppId { get; set; }
        public int? ManId { get; set; }
        public int? LicId { get; set; }
        public long? RequestId { get; set; }
        public int? EndingLicenseReason { get; set; }
        public string? MandoobEmail { get;set; }
        public string? MandoobPhone { get; set; }
        public int? PreApproveId { get; set; }
        public int? SalesManagerId { get; set; }
        public int? MarketingManagerId { get; set; }
        public int? OperationManagerId { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpireDate { get; set; }

        public int? ActivityTypeId { get; set; }
        public int? CompanyId { get; set; }
        public int? BuildingId { get; set; }
        public int? AddressId { get; set; }
        public string? CentralNoMOIc { get; set; }
        public int? ClassificationId { get; set; }
        public int? ReqtypeId { get; set; }
        public int? EndingReasonId { get; set; }
        public string? NewUserName { get; set; }
        public string? OldUserName { get; set; }
        public int? licencesInfoId { get; set; }
        public string? NewCivilId { get; set; }
        public string? OldCivilId { get; set; }

        public string? NewEmail { get; set; }
        public string? OldEmail { get; set; }


        public string? NewMobile { get; set; }
        public string? OldMobile { get; set; }

        public int? CompanyUserId { get; set; }
        public string? ManCivilId { get; set; }
       
        public long SequenceNo {  get; set; }   
        public string? UnitNo { get; set; }
        public string? FloorNo { get; set; }
        public decimal? Amount { get; set; }
        public string? LicencesName { get; set; }
        public List<FileSaveResponseVM>? saveResponseVMs { get; set; }
        public List<int>? SelectedTransactionTypeIds { get; set; }
        public string? accountTypeId { get; set; }
        public string? PreApprove { get; set; }
        public string? SalesManagerCivilId { get; set; }
        public string? SalesManagerName { get; set; }
        public string? SalesManagerEmail { get; set; }
        public string? SalesManagerPhone { get; set; }
        public string? MarketingManagerCivilId { get; set; }
        public string? MarketingManagerName { get; set; }
        public string? MarketingManagerEmail { get; set; }
        public string? MarketingManagerPhone { get; set; }
        public string? OperationManagerCivilId { get; set; }
        public string? OperationManagerName { get; set; }
        public string? OperationManagerEmail { get; set; }
        public string? OperationManagerPhone { get; set; }
        public string? AppCivilId { get; set; }
        public string? AppEmail { get; set; }
        public string? AppPhone { get; set; }
        public string? AppName { get; set; }

        public string? MandoobId { get; set; }
      
      
        public string? UserCivilID { get; set; }
        public string? SessionName { get; set; }
        public string? SessionCivilId { get; set; }

        [Display(Name = "إسم مالك العقار")]
        public string? UserName { get; set; }


        [Display(Name = "رقم المدني للجهة")]
        public string? CompanyCivilId { get; set; }


        [Display(Name = "الشركة المديرة")]
        public string? DirCompanyAr { get; set; }

        //بيانات المدير الجديدة 
        [DisplayName("إسم المدير الجديد")]
        public string? NewManagerName { get; set; }
        public string? OldManagerName { get; set; }

        [DisplayName("رقم الهاتف المدير الجديد")]

        public string? NewManagerMobile { get; set; }
        public string? OldManagerMobile { get; set; }

        [DisplayName("البريد الإلكتروني المدير الجديد")]

        public string? NewManagerEmail { get; set; }
        public string? OldManagerEmail { get; set; }

        [DisplayName("الرقم المدني للمدير الجديد")]

        public string? NewManCivilId { get; set; }
        public string? OldManCivilId { get; set; }

        //بيانات الشركة الجديدة
        [DisplayName("إسم الشركة المالكة الجديد")]

        public string? NewOwnerCompanyAr { get; set; }
        public string? OldOwnerCompanyAr { get; set; }

        [DisplayName("إسم الشركة المديرة الجديد")]

        public string? NewDirCompanyAr { get; set; }
        public string? OldDirCompanyAr { get; set; }

        // بيانات الرخصة الجديدة
        [DisplayName("إسم الترخيص الجديد")]

        public string? NewLicencesName { get; set; }
        public string? OldLicencesName { get; set; }


        //بيانات العنوان الجديدة
        [DisplayName("الرقم الآلي الجديد")]

        public string? NewAaliNumber { get; set; }
        public string? OldAaliNumber { get; set; }

        [DisplayName("المنطقة ")]

        public string? NewArea { get; set; }
        public string? OldArea { get; set; }

        [DisplayName("المحافظة")]

        public string? NewGovernrate { get; set; }
        public string? OldGovernrate { get; set; }

        [DisplayName("القطعة")]

        public string? NewBlockNo { get; set; }
        public string? OldBlockNo { get; set; }

        [DisplayName("الشارع")]

        public string? NewStreet { get; set; }
        public string? OldStreet { get; set; }

        [DisplayName("إسم المبني")]

        public string? NewBuildingName { get; set; }
        public string? OldBuildingName { get; set; }

        [DisplayName("رقم المبني")]

        public string? NewBuildingNo { get; set; }
        public string? OldBuildingNo { get; set; }

        [DisplayName("رقم الوحدة")]

        public string? NewUnitNo { get; set; }
        public string? OldUnitNo { get; set; }

        [DisplayName("الدور")]

        public string? NewFloorNo { get; set; }
        public string? OldFloorNo { get; set; }


        [Display(Name = "الشركة المالكة")]
        public string? OwnerCompanyAr { get; set; }


        [Display(Name = "رقم السجل التجاري")]
        public string? RecordNo { get; set; }
        [Display(Name = "رقم الترخيص التجاري")]
        public string? CommercialLicNo { get; set; }


        [Display(Name = "العنوان")]
        public string? OwnerCoAddress { get; set; }

        [Display(Name = "نشاط الشركة")]
        public string? CompanyActivity { get; set; }

        [Display(Name = "رمز النشاط")]
        public string? ActivityCode { get; set; }

        [Display(Name = "مساحة الارض")]
        public string? NewAreaSize { get; set; }
        public string? OldAreaSize { get; set; }


        [Display(Name = "رقم المخطط المساحي")]
        public string? NewAreaChartNo { get; set; }
        public string? OldAreaChartNo { get; set; }


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
        public string? AreaSize { get; set; }
        public string? AreaChartNo { get; set; }   

        //-------- Manager Details ----------------

        [Display(Name = "اسم للمدير")]
        public string? ManagerName { get; set; }

        [Display(Name = "رقم جوال للمدير")]
        public string? ManagerMobile { get; set; }

        [Display(Name = "البريد الالكتروني للمدير")]
        public string? ManagerEmail { get; set; }

        //----------------------------------------
        public Dictionary<int, int>? EvaluationSelections { get; set; }
        public List<SelectListItem>? Activities { get; set; }

        public string? reqno { get; set; }

        public string? LicNo { get; set; }

        

        
    }

    public class RequestTourLic
    {
        public int? LicenceId { get; set; }

        public int? BuildingId { get; set; }

        public int? CompanyId { get; set; }

        public int? ManagerId { get; set; }
        //public string? LicenseNo { get; set; }

        public int? AppId { get; set; }
        public string? MandoobId { get; set; }
        [DisplayName("رقم الموافقة المبدئية")]

        public string? PreAprove { get; set; }
        public int? PreAppId { get; set; }
        
        public long? RequestId { get; set; }

        public int? LicTypeId { get; set; }

        public string? ClassificationName { get; set; }

        public DateTime? ClassificationDate { get; set; }

        public DateTime? ComIssuingDate { get; set; }

        public DateTime? ComExpiryDate { get; set; }

        public int? ActivityTypeId { get; set; }

        public int? ReqStatusId { get; set; }
        [DisplayName("إسم الترخيص")]
        public string? LicenseName { get; set; }
        [DisplayName("رقم الترخيص")]
        public string? LicenseNo { get; set; }

        public DateTime? LicenseIssueDate { get; set; }

        public DateTime? LicenseExpireDate { get; set; }

        public int? ClassificationId { get; set; }
        [Display(Name = "الرقم المدني للمالك")]

        public string? ApplicantCivilId { get; set; }
        [Display(Name = "رقم الترخيص التجاري")]

        public string? CommercialLicNo { get; set; }
        [Display(Name = "رقم السجل التجاري")]

        public string? RecordNo { get; set; }


        public string? ManagerCivilId { get; set; }
        public string? SalesManagerCivilId { get; set; }
        public string? MarketingManagerCivilId { get; set; }
        public string? OperationsManagerCivilId { get; set; }
        public int? SalesManagerId { get; set; }
        public int? MarketingManagerId { get; set; }
        public int? OperationsManagerId { get; set; }

        public string? UserCivilId { get; set; }

        public int? LicStatusId { get; set; }
        [ForeignKey("RequestId")]
        public virtual RequestVM? Request { get; set; }
        [ForeignKey("ManagerId")]
        public virtual PersonVM? Manager { get; set; }
        [ForeignKey("AppId")]
        public virtual PersonVM? Applicant { get; set; }
        [ForeignKey("MandoobId")]
        public virtual AspnetUserVM? Mandoob { get; set; }

        [ForeignKey("CompanyId")]
        public virtual CompanyVM? Company { get; set; }
        [ForeignKey("BuildingId")]
        public virtual CompanyVM? Building { get; set; }
        [ForeignKey("LicTypeId")]
        public virtual LicencesTypeVM? LicenceTypesLookup { get; set; }
        [ForeignKey("ActivityTypeId")]
        public virtual ActivityTypeVM? ActivityTypesLookup { get; set; }
        [ForeignKey("ReqStatusId")]
        public virtual RequestStatusVM? RequestStatusLookup { get; set; }
        [ForeignKey("LicStatusId")]
        public virtual LicencesStatusVM? LicenseStatusLookup { get; set; }
    }

    public class TourLicRequestResponseVM
    {
        public RequestTourLic? PreApprovalDetails { get; set; }
        public AspnetUserVM? AspnetUserVM { get; set; }
        public List<NamedFile>? NamedFile { get; set; }
        [DisplayName("إسم المنشأة")]
        [Required]
        public string? LicencesName { get; set; }
        public List<FileUploadConfigVM>? FileUploadConfigs { get; set; }
        public LicencesInfoVM? LicencesInfoVM { get; set; }
    }


    public class TourLicActivityVm
    {
        public int CompanyId { get; set; }
        public int ReqtypeId { get; set; }
        public int? CompanyUserId { get; set; }
        public string? ManCivilId { get; set; }
        public string? accountTypeId { get; set; }
        public string? LicenseNo { get; set; }
        [DisplayName("إسم الترخيص")]
        public string? LicName { get; set; }

        public string? ApplicantCivilId { get; set; }
        public string? ApplicantUserName{ get; set; }

        public string? MandoobId { get; set; }
        public int? AppId { get; set; }
        public int? ActivityTypeId { get; set; }
        [Display(Name = "الرقم المدني للمالك")]
        public string? UserCivilId { get; set; }


        [Display(Name = "إسم مالك العقار")]
        public string? UserName { get; set; }


        [Display(Name = "رقم المدني للجهة")]
        public string? CompanyCivilId { get; set; }



        [ForeignKey("SalesManagerId")]
        public virtual PersonVM? SalesManager { get; set; }
        [ForeignKey("MarketingManagerId")]
        public virtual PersonVM? MarketingManager { get; set; }
        [ForeignKey("OperationsManagerId")]
        public virtual PersonVM? OperationsManager { get; set; }

        [Display(Name = "الشركة المالكة")]
        public string? OwnerCompanyAr { get; set; }


        [Display(Name = "رقم السجل التجاري")]
        public string? RecordNo { get; set; }
        [Display(Name = "رقم الترخيص التجاري")]
        public string? CommercialLicNo { get; set; }


       

        [Display(Name = "نشاط الشركة")]
        public string? CompanyActivity { get; set; }

        [Display(Name = "رمز النشاط")]
        public string? ActivityCode { get; set; }

        [Display(Name = "مساحة الارض")]
        public string? AreaSize { get; set; }

        [Display(Name = "رقم المخطط المساحي")]
        public string? AreaChartNo { get; set; }

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

        //-------- Manager Details ----------------

        [Display(Name = "اسم للمدير")]
        public string? ManagerName { get; set; }

        [Display(Name = "رقم جوال للمدير")]
        public string? ManagerMobile { get; set; }

        [Display(Name = "البريد الالكتروني للمدير")]
        public string? ManagerEmail { get; set; }
        [Display(Name = "الرقم المدني للمدير")]

        public string? ManagerCivilId { get; set; }


        //----------------------------------------

        public List<SelectListItem>? Activities { get; set; }

        public string? reqno { get; set; }

        //public string? _Reqno { get; set; }

        public List<AddWorkflowWithAttachmentsVM>? fileUploadConfigs { get; set; }

        [Display(Name = "المرفقات")]
        //public List<IFormFile> ContractDocuments { get; set; }
        public List<NamedFile> NamedFile { get; set; }
        public ActivityTypeVM? ActivityType { get; set; }
        public LicencesInfoVM? LicencesInfo { get; set; }

    }
    public class WhoConcRequestVm
    {
       
        public List<SelectListItem>? Activities { get; set; }

        public string? reqno { get; set; }

      public LicencesVM LicencesVM { get; set; }
        public AspnetUserVM AspnetUserVM { get; set; }
        public List<FileUploadConfigVM>? fileUploadConfigs { get; set; }

        [Display(Name = "المرفقات")]
        //public List<IFormFile> ContractDocuments { get; set; }
        public List<NamedFile> NamedFile { get; set; }
        public ActivityTypeVM? ActivityType { get; set; }
        public LicencesInfoVM? LicencesInfo { get; set; }

    }

    public class RenewRequest
    {
        public LicencesVM? LicencesVM { get; set; }
        public List<FileUploadConfigVM>? FileUploadConfigs { get; set; }
        public List<NamedFile>? NamedFile { get; set; }
    }
    public class EndLicencesRequest
    {
        public LicencesVM? LicencesVM { get; set; }
        [Required(ErrorMessage = "الرجاء اختيار سبب إنهاء الترخيص.")]
        public int? EndingReasonId { get; set; } 

        public List<SelectListItem>? EndingReasons { get; set; }

        public List<FileUploadConfigVM>? FileUploadConfigs { get; set; }
        public List<NamedFile>? NamedFile { get; set; }
    }
    public class RenouncementRequest
    {
        public LicencesVM? LicencesVM { get; set; }
        public string? NewUserName { get; set; }
        public string? NewCivilId { get; set; }
        public string? NewEmail { get; set; }
        public string? NewPhoneNumber {  get; set; }    
        public string? NewMobile {  get; set; } 

        public List<FileUploadConfigVM>? FileUploadConfigs { get; set; }
        public List<NamedFile>? NamedFile { get; set; }
    }

    public class ClassificationFormVM
    {
        public List<ClassificationBranchDetail> ClassificationBranches { get; set; }
        public Dictionary<int, int>? EvaluationSelections { get; set; }
        public List<SelectListItem>? Classificaion { get; set; }
        public int? ClassificationId { get; set; }
        public LicencesVM LicencesVM { get; set; }
        public int RequestTypeId { get; set; }
        public List<FileUploadConfigVM> FileUploadConfigs { get; set; }
        public List<NamedFile>? NamedFile { get; set; }

    }
    public class RequestBaseVM
    {
        public int? CompanyId { get; set; }
        public int? BuildingId { get; set; }
        //public int? AppId { get; set; }

        public int ReqtypeId { get; set; }
        public int? ActivityTypeId { get; set; }
        public string? ActivityCode { get; set; }
        public int? AddressId { get; set; }
        public List<int>? SelectedTransactionTypeIds { get; set; }
        public List<ClassificationBranchDetail>? ClassificationBranches { get; set; }
        public LicencesVM? LicencesVM { get; set; }
        public List<SelectListItem>? EndingReasons { get; set; }
        public List<SelectListItem>? Classificaion { get; set; }
       
        public string? AppCivilId { get; set; }
        public string? MandoobId { get; set; }
        public int? AppId { get; set; }
        public string? UserCivilID { get; set; }
        public string? accountTypeId { get; set; }
        public string? SessionName { get; set; }
        public string? SessionCivilId { get; set; }

        public string? CompanyActivity { get; set; }
        public string? OwnerCompanyAr { get; set; }
        public string? CommercialLicNo { get; set; }
        public string? RecordNo { get; set; }
        public string? CompanyCivilId { get; set; }

        public string? ManagerName { get; set; }
        public string? ManagerMobile { get; set; }
        public string? ManagerEmail { get; set; }
        public string? ManCivilId { get; set; }
        //بيانات المدير الجديدة 
        [DisplayName("إسم المدير الجديد")]

        public string? NewManagerName { get; set; }
        public string? NewManagerMobile { get; set; }
        [DisplayName("الإيميل الجديد للمدير")]

        public string? NewManagerEmail { get; set; }
        [DisplayName("الرقم المدني للمدير الجديد")]

        public string? NewManCivilId { get; set; }
        //بيانات الشركة الجديدة
        [DisplayName("إسم الشركة المالكة الجديدة")]
        public string? NewOwnerCompanyAr { get; set; }
        [DisplayName("إسم الشركة المديرة الجديدة")]

        public string? NewDirCompanyAr { get; set; }
        // بيانات الرخصة الجديدة
        [DisplayName("إسم الترخيص الجديدة")]

        public string NewLicencesName { get; set; }

        //بيانات العنوان الجديدة
        [DisplayName("الرقم الألى الجديد")]

        public string? NewAaliNumber { get; set; }
        [DisplayName(" المنطقة")]

        public string? NewArea { get; set; }
        [DisplayName("المحافظة")]

        public string? NewGovernrate { get; set; }
        [DisplayName("القطعة")]

        public string? NewBlockNo { get; set; }
        [DisplayName("الشارع")]

        public string? NewStreet { get; set; }
        [DisplayName("إسم المبني")]

        public string? NewBuildingName { get; set; }
        [DisplayName("رقم المبني")]

        public string? NewBuildingNo { get; set; }
        [DisplayName("رقم الوحدة")]

        public string? NewUnitNo { get; set; }
        [DisplayName("الدور")]

        public string? NewFloorNo { get; set; }

        public int? ManId { get; set; }

        public string? LicNo { get; set; }
        public int? LicId { get; set; }
        public string? LicencesName { get; set; }

        public string? AaliNumber { get; set; }
        public string? Area { get; set; }
        public string? Governrate { get; set; }
        [DisplayName("مساحة الأرض")]

        public string? NewAreaSize { get; set; }
        [DisplayName("رقم المخطط المساحي")]

        public string? NewAreaChartNo { get; set; }
        public string? BlockNo { get; set; }
        public string? Street { get; set; }
        public string? BuildingName { get; set; }
        public string? BuildingNo { get; set; }
        public string? UnitNo { get; set; }
        public string? FloorNo { get; set; }

        public string? DirCompanyAr { get; set; }

        public string? reqno { get; set; }
        public long SequenceNo { get; set; }

        public List<AddAttachmentsRulesVM>? FileUploadConfigs { get; set; }
        public List<NamedFile>? NamedFile { get; set; }
        public List<FileSaveResponseVM>? saveResponseVMs { get; set; }

        // For Renouncement
        [DisplayName("الإسم الجديد")]

        public string? NewUserName { get; set; }
        [DisplayName("الرقم المدني الجديد")]

        public string? NewCivilId { get; set; }
        [DisplayName("الإيميل")]

        public string? NewEmail { get; set; }
        [DisplayName("رقم التليفون")]

        public string? NewMobile { get; set; }

        // For Classification
        public Dictionary<int, int>? EvaluationSelections { get; set; }
        public int? ClassificationId { get; set; }
        public int? PreApproveId { get; set; }

        // For EndLicences
        public int? EndingReasonId { get; set; }

        public LicencesInfoVM? LicencesInfo { get; set; }
        public AspnetUserVM AspnetUserVM { get; set; }
    }

    public class PreApprovalResult
    {
        public bool IsValid { get; set; }
        public string? Message { get; set; }
    }

    public class ChangeDataRequestVM
    {
        public int LicenseId { get; set; }

        public List<int> SelectedTransactionTypeIds { get; set; }

        // Add fields relevant to each transaction type:
        public string NewCompanyName { get; set; }
        public string NewAddress { get; set; }
        public string NewManagerName { get; set; }
        public string NewLicenseName { get; set; }
        public string ReplacementReason { get; set; }

      
    }

}
