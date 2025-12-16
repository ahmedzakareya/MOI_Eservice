using AutoMapper;
using Business.Interfaces;
using Business.Repository;
using Business.ViewModel;
using Business.ViewModel.Account;
using Business.ViewModel.ClassificationVM;
using Business.ViewModel.Dynamic;
using Business.ViewModel.HomePage;
using Business.ViewModel.Tourism;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Mapping
{
    public class MappingProfile : Profile
    {


        public MappingProfile()
        {


            CreateMap<MoiEserviceLicensesRequest, RequestVM>()
    .ForMember(dest => dest.ActivityName, opt => opt.MapFrom(src => src.ActivityTypeNavigation.NameAr))
    .ForMember(dest => dest.LicTypeName, opt => opt.MapFrom(src => src.LicenceTypeNavigation.NameAr))
    .ForMember(dest => dest.ReqStatusName, opt => opt.MapFrom(src => src.RequestStatusNavigation.NameAr))
    .ForMember(desc => desc.ReqTypeName, opt => opt.MapFrom(src => src.RequestsTypesNavigation.NameAr))
     // REMOVE manual object creation and just map the nested objects:
     .ForMember(dest => dest.attachVMs, opt => opt.MapFrom(src => src.attachVMs))
    //.ForMember(dest => dest.LicenceNavigation, opt => opt.MapFrom(src => src.LicenceNavigation))
    //.ForMember(dest => dest.attachVMs, opt => opt.MapFrom(src => src.attachVMs))
    //.ForMember(dest => dest.ClassificationData, opt => opt.MapFrom(src => src.ClassificationData))
    .ReverseMap();
            CreateMap<MoiEserviceSysUser, MoiEserviceSysUserVM>().ReverseMap();
          CreateMap<ContactUs, ContactUsVM>().ReverseMap(); 
            CreateMap<ResetUserPassword, ResetUserPasswordVM>().ReverseMap();   

                
            CreateMap<SystemOption, SystemOptionVM>().ReverseMap();
            CreateMap<WorkFlowActionButton, WorkFlowActionButtonVM>().ReverseMap(); 
            CreateMap<LicenseEndingTransaction, EndLicencesTransVM>().ReverseMap();
            CreateMap<AttachRule, AddAttachmentsRulesVM>().ReverseMap();
            CreateMap<Licence, LicencesVM>()
                .ForMember(des => des.LicTypeName, opt => opt.MapFrom(src => src.LicenceTypesLookup.NameAr))
                .ForMember(des => des.LicStatusName, opt => opt.MapFrom(src => src.LicenseStatusLookup.NameAr))
                .ForMember(des => des.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(des => des.ActivityTypeName, opt => opt.MapFrom(src => src.ActivityTypesLookup.NameAr))
                .ReverseMap();
            //CreateMap<AspNetRole, RoleVM>()
            //    .ForMember(des => des.ModuleName, opt => opt.MapFrom(src => src.RolePermissions.Select(p=>p.Module.Name)))
            //     .ForMember(des => des.MenuItemName, opt => opt.MapFrom(src => src.RolePermissions.Select(p => p.MenuItem.Name)))

            //.ForMember(des => des.PermissionAction, opt => opt.MapFrom(src => src.RolePermissions.Select(r => r.Permission.NameAr)))
            //.ReverseMap();
            

            //CreateMap<MoiEserviceLicenseInfo, LicencesInfoVM>()
            //    .ForMember(des=>des.ActivityType,opt=>opt.MapFrom(src=>src.ActivityTypesLookup.NameAr))
            //    .ForMember(des=>des.EserviceType,opt=>opt.MapFrom(src=>src.EserviceTypesLookup.EserviceTypeAr))
            //    .ForMember(des=>des.EServiceName,opt=>opt.MapFrom(src=>src.ActivityTypesLookup.Eservice.EserviceNameAr))
            //    .ReverseMap();

            // Mapping for Role to RoleVM
            CreateMap<RoleAdmin, RoleVM>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Modules, opt => opt.MapFrom(src => src.RolePermissions
					.GroupBy(rp => new { rp.ModuleId, rp.Module.Name })
					.Select(g => new ModuleVM
					{
						Id = g.Key.ModuleId,
						Name = g.Key.Name,
						MenuItems = g.GroupBy(mi => new { mi.MenuItemId, mi.MenuItem.Name, mi.MenuItem.Url, mi.MenuItem.IsVisible })
							.Select(miGroup => new AddMenuItemVM
							{
								Id = miGroup.Key.MenuItemId,
								Name = miGroup.Key.Name,
								Url = miGroup.Key.Url,
								IsVisible = miGroup.Key.IsVisible,
								Permissions = miGroup.Select(mip => new PermissionVM
								{
									Id = mip.PermissionAdminId,
									NameAr = mip.Permission.NameAr
								}).ToList()
							}).ToList()
					}).ToList()));

			// Mapping for Module to ModuleVM
			CreateMap<Module, ModuleVM>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            CreateMap<Form,FormsViewModel>().ReverseMap();
            CreateMap<MoiSocialMedia, SocialMediaVM>().ReverseMap();
            CreateMap<PartnerNewChangeTransaction, ChangeNewPartnerTransVM>().ReverseMap();
            CreateMap<PartnerOldChangeTransaction, ChangeOldPartnerTransVM>().ReverseMap();
            CreateMap<TransactionTypesLookup, TransactionTypesLookupVM>().ReverseMap();
            CreateMap<MoiEserviceLicEndingReason, MoiEserviceLicEndingReasonVM>().ReverseMap();


            // Mapping for MenuItem to AddMenuItemVM
            CreateMap<MenuItem, AddMenuItemVM>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
				.ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
				.ForMember(dest => dest.IsVisible, opt => opt.MapFrom(src => src.IsVisible));
            CreateMap<FileUploadConfigurationsFront, FileUploadConfigVM>().ReverseMap();
            CreateMap<MoiPreApprovement, PreApprovalRequestModel>().ReverseMap();
            CreateMap<MoiPreApprovement, RequestTourLic>().ReverseMap();
            CreateMap<LicenceTypesLookup, LicencesTypeVM>().ReverseMap();
            CreateMap<LicenseStatusLookup, LicencesStatusVM>().ReverseMap();
            CreateMap<ActivityTypesLookup, ActivityTypeVM>().ReverseMap();
            CreateMap<AspNetUser, AspnetUserVM>().ReverseMap();
            CreateMap<MoiEserviceLicenseInfo, LicencesInfoVM>().ReverseMap();
            CreateMap<TestablishContract, TestablishContractVM>().ReverseMap();
            CreateMap<PesronTypeLookUp, PesronTypeLookUpVM>().ReverseMap();
            CreateMap<QualificationsLookup, QualificationsLookupVM>().ReverseMap();
            CreateMap<AttachRule,Business.ViewModel. AttachRuleVM>().ReverseMap();
            CreateMap<MoiEserviceLicensesRequest, MoiEserviceLicensesRequestVM>().ReverseMap();


            // Mapping for Permission to PermissionVM
            CreateMap<PermissionAdmin, PermissionVM>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr));
            
            CreateMap<Company, CompanyVM>()
                .ForMember(des => des.AddressId, opt => opt.MapFrom(src => src.AddressNavigation.Id))
                .ReverseMap();
            CreateMap<Company, BuildingVM>()
               .ForMember(des => des.AddressId, opt => opt.MapFrom(src => src.AddressNavigation.Id))
               .ReverseMap();
            CreateMap<Address, AddressVM>().ReverseMap();
            CreateMap<Address, AddressVM>().ReverseMap();
            CreateMap<WorkFlow, WorkFlowVM>()
                .ForMember(des => des.ServiceName, opt => opt.MapFrom(src => src.Eservice.EserviceName))
                .ForMember(des => des.NextStatusName, opt => opt.MapFrom(src => src.RequestStatusNext.NameAr))
                .ForMember(des => des.CurrentStatusName, opt => opt.MapFrom(src => src.RequestStatusCurrent.NameAr))
                .ForMember(des => des.RequestTypeName, opt => opt.MapFrom(src => src.RequestsTypesLookup.NameAr))
                .ReverseMap();
            
           
            CreateMap<MenuItem, AddMenuItemVM>()
            .ForMember(dest => dest.ModuleName, opt => opt.MapFrom(src => src.Module.Name));
            /*.ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Module.Permissions))*/
            CreateMap<PermissionAdmin, PermissionVM>().ReverseMap();
            //.ForMember(dest => dest.ModuleName, opt => opt.MapFrom(src => src.Module.Name));
            CreateMap<ElawMoiWeNews, NewsVM>().ReverseMap();
            CreateMap<MoiPreApprovement, PreApprovementVM>().ReverseMap();
            CreateMap<Person, PersonVM>().ReverseMap();
            CreateMap<ElawMoiWeNews, NewsItem>()
        .ForMember(dest => dest.SmallDescription, opt => opt.MapFrom(src => src.SmallDescription))
        .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
        .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate ?? DateTime.Now));
            CreateMap<NewsItem, ElawMoiWeNews>().ReverseMap();
        CreateMap<MoiEserviceRequestsAttach, AttachVM>().ReverseMap();
            CreateMap<LinksLookup, LinksVM>().ReverseMap();
            CreateMap<LinksLookup, AddLinksVM>().ReverseMap();

            CreateMap<ActivityChangeTypeTransaction,ActivityChangeTransVM>().ReverseMap();  
            CreateMap<ChangeSocialMediaTransaction, ChangeSocialMediaTransVM>().ReverseMap();
            CreateMap<CommercialNameChangeTransaction,CommercialTransVM>().ReverseMap();    
            CreateMap<CompanyNameChangeTransaction,CompanyTransVM>().ReverseMap();
            CreateMap<ChangeMediaNameTransaction,MediaChangeTransVM>().ReverseMap();    
            CreateMap<RenouncementTransaction,ChangeOwnerTransVM>().ReverseMap();   
            CreateMap<TchangeManager,ChangeManagerTransVM>().ReverseMap();  
            CreateMap<ChangeEmailTranaction,EmailChangeTransVM>().ReverseMap();
            CreateMap<MoiEserviceRequestPaymentDetail, PaymentDetailsVM>().ReverseMap();
            CreateMap<AddressChangeTransaction,AddressChangeTransVM>().ReverseMap();
            CreateMap<LicencesNameChangeTransaction, LicencesNameChangeTransactionVM>().ReverseMap();
            CreateMap<LicenseTypeChangeTransaction, ChangeLicencesTypeTransVM>().ReverseMap();
            CreateMap<MoiEservicesRequestTransaction, RequestTransactionVM>()
            .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.MoiEserviceSysUser.Username));
            CreateMap<Partner,PartnerVM>().ReverseMap();    
            CreateMap<PartnerOldChangeTransaction,ChangeNewPartnerTransVM>().ReverseMap();
            CreateMap<LicenseRenewTransaction,LicenceRenewTransVM>().ReverseMap();
            CreateMap<RequestsTypesLookup,RequestTypeVM>().ReverseMap();
            CreateMap<RequestStatusLookup, RequestStatusVM>().ReverseMap();
            CreateMap<LicenseRenew, RenewVM>().ForMember(dest => dest.NewExpiryDate, opt => opt.MapFrom(src => src.NewExpiryDate))
        .ReverseMap();
            CreateMap<Person,PersonVM>().ReverseMap();  
            CreateMap<ReplacementOfLostTransaction,ReplacementOfLostTransVM>().ReverseMap();
            CreateMap<Transaction,TransactionVM>().ReverseMap();    
            CreateMap<MoiEserviceSysUser,UserVM>().ReverseMap();
            CreateMap<AspNetUserRoleAdmin, AspnetUserRoleVM>().ReverseMap();
          
            CreateMap<PermissionAdmin,PermissionVMV>().ReverseMap();  
            CreateMap<MenuItem,MenuItemVMV>().ReverseMap(); 
            CreateMap<Module,ModuleVMV>().ReverseMap();
            CreateMap<RoleWithModulesDTO, RoleVMV>()
           .ForMember(dest => dest.Modules, opt => opt.MapFrom(src => src.Modules));

            CreateMap<ModuleWithMenuItemsDTO, ModuleVM>()
                .ForMember(dest => dest.MenuItems, opt => opt.MapFrom(src => src.MenuItems));

            CreateMap<MenuItemWithPermissionsDTO, AddMenuItemVM>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.Permissions));

            CreateMap<PermissionDTO, PermissionVM>();
            CreateMap<CountriesLookup, CountriesLookupVM>().ReverseMap();

            CreateMap<LicenceTypesLookup, LicenceTypesLookupVM>();
            CreateMap<ScheduleReleaseTypes, ScheduleReleaseTypesVM>
                ();

        }
    }
}
