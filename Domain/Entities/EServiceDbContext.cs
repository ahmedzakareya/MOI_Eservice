using System;
using System.Collections.Generic;
using Azure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Domain.Entities;

public partial class EServiceDbContext : IdentityDbContext<AspNetUser>
{
    public EServiceDbContext()
    {
    }

    public EServiceDbContext(DbContextOptions<EServiceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityChangeTypeTransaction> ActivityChangeTypeTransactions { get; set; }



    public virtual DbSet<ActivityTypesLookup> ActivityTypesLookups { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<AddressChangeTransaction> AddressChangeTransactions { get; set; }

    public virtual DbSet<AreasLookup> AreasLookups { get; set; }
    public virtual DbSet<ValidEserviceCombinations> ValidEserviceCombinations { get; set; }


    public virtual DbSet<AspNetMultipleLicenseUser> AspNetMultipleLicenseUsers { get; set; }

    public virtual DbSet<AspNetMultipleUser> AspNetMultipleUsers { get; set; }

    public virtual DbSet<AspNetRequestMultipleUser> AspNetRequestMultipleUsers { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
    public virtual DbSet<RoleAdmin> RoleAdmins{ get; set; }

    public virtual DbSet<MoiEserviceSysUser> MoiEserviceSysUsers { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
    public virtual DbSet<AspNetUserRoleAdmin> AspNetUserRoleAdmins { get; set; }
    public virtual DbSet<Permission> Permissions { get; set; }
    public virtual DbSet<PermissionAdmin> PermissionAdmins { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }
    public virtual DbSet<RolePermissionAdmin> RolePermissionAdmins { get; set; }
    public virtual DbSet<AspNetUsersAccountType> AspNetUsersAccountTypes { get; set; }

    public virtual DbSet<ChangeEmailTranaction> ChangeEmailTranactions { get; set; }

    public virtual DbSet<ChangeMediaNameTransaction> ChangeMediaNameTransactions { get; set; }

    public virtual DbSet<ChangeSocialMediaTransaction> ChangeSocialMediaTransactions { get; set; }

    public virtual DbSet<CommercialNameChangeTransaction> CommercialNameChangeTransactions { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<CompanyNameChangeTransaction> CompanyNameChangeTransactions { get; set; }

    public virtual DbSet<CountriesLookup> CountriesLookups { get; set; }

    public virtual DbSet<DelegationTransactionLog> DelegationTransactionLogs { get; set; }

    public virtual DbSet<ElawMoiWeLawRule> ElawMoiWeLawRules { get; set; }

    public virtual DbSet<ElawMoiWeNews> ElawMoiWeNews { get; set; }

    public virtual DbSet<ElawMoiWePage> ElawMoiWePages { get; set; }

    public virtual DbSet<Eservice> Eservices { get; set; }

    public virtual DbSet<EserviceTypesLookup> EserviceTypesLookups { get; set; }

    public virtual DbSet<ExceptionsLog> ExceptionsLogs { get; set; }

    public virtual DbSet<Form> Forms { get; set; }

    public virtual DbSet<GovernoratesLookup> GovernoratesLookups { get; set; }

    public virtual DbSet<Licence> Licences { get; set; }

    public virtual DbSet<LicenceTypesLookup> LicenceTypesLookups { get; set; }

    public virtual DbSet<LicenseEndingTransaction> LicenseEndingTransactions { get; set; }

    public virtual DbSet<LicenseRenew> LicenseRenews { get; set; }

    public virtual DbSet<LicenseRenewTransaction> LicenseRenewTransactions { get; set; }

    public virtual DbSet<LicenseStatusLookup> LicenseStatusLookups { get; set; }

    public virtual DbSet<LicenseTypeChangeTransaction> LicenseTypeChangeTransactions { get; set; }

    public virtual DbSet<LinksLookup> LinksLookups { get; set; }

    public virtual DbSet<LogosLookup> LogosLookups { get; set; }

    public virtual DbSet<MenuItem> MenuItems { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<MoiEserviceDepartment> MoiEserviceDepartments { get; set; }

    public virtual DbSet<MoiEserviceLicEndingReason> MoiEserviceLicEndingReasons { get; set; }


    public virtual DbSet<MoiEserviceLicenseInfo> MoiEserviceLicenseInfos { get; set; }

    public virtual DbSet<MoiEserviceLicensesRequest> MoiEserviceLicensesRequests { get; set; }

    public virtual DbSet<MoiEserviceRequestPaymentDetail> MoiEserviceRequestPaymentDetails { get; set; }

    public virtual DbSet<MoiEserviceRequestsAttach> MoiEserviceRequestsAttaches { get; set; }

    public virtual DbSet<MoiEserviceSector> MoiEserviceSectors { get; set; }


    public virtual DbSet<MoiEserviceSysUsersActivityLog> MoiEserviceSysUsersActivityLogs { get; set; }

    public virtual DbSet<MoiEservicesRequestTransaction> MoiEservicesRequestTransactions { get; set; }

    public virtual DbSet<MoiSahelActivityLog> MoiSahelActivityLogs { get; set; }

    public virtual DbSet<MoiSocialMedia> MoiSocialMedia { get; set; }

    public virtual DbSet<MonMoiEservicePartiesSub> MonMoiEservicePartiesSubs { get; set; }

    public virtual DbSet<MonMoiEservicePartiesType> MonMoiEservicePartiesTypes { get; set; }

    public virtual DbSet<MonMoiEservicePartySubscriber> MonMoiEservicePartySubscribers { get; set; }

    public virtual DbSet<MonMoiEservicesPartyPressRequestPhase> MonMoiEservicesPartyPressRequestPhases { get; set; }

    public virtual DbSet<Partner> Partners { get; set; }

    public virtual DbSet<PartnerOldChangeTransaction> PartnerOldChangeTransactions { get; set; }

    public virtual DbSet<PartnerNewChangeTransaction> PartnerNewChangeTransactions { get; set; }

    public virtual DbSet<Person> Persons { get; set; }


    public virtual DbSet<PesronTypeLookUp> PesronTypeLookUps { get; set; }

    public virtual DbSet<QualificationsLookup> QualificationsLookups { get; set; }

    public virtual DbSet<RenouncementTransaction> RenouncementTransactions { get; set; }

    public virtual DbSet<ReplacementOfLostTransaction> ReplacementOfLostTransactions { get; set; }

    public virtual DbSet<RequestStatusLookup> RequestStatusLookups { get; set; }

    public virtual DbSet<RequestsTypesLookup> RequestsTypesLookups { get; set; }

   


    public virtual DbSet<SahelSubscriber> SahelSubscribers { get; set; }

    public virtual DbSet<SettingsLookup> SettingsLookups { get; set; }

    public virtual DbSet<SocialTypeLookup> SocialTypeLookups { get; set; }

    public virtual DbSet<TchangeManager> TchangeManagers { get; set; }


    public virtual DbSet<TestablishContract> TestablishContracts { get; set; }

    public virtual DbSet<TourClassBranchLookUp> TourClassBranchLookUps { get; set; }

    public virtual DbSet<TourClassTypeLookUp> TourClassTypeLookUps { get; set; }

    public virtual DbSet<TourEvaluationListHotel> TourEvaluationListHotels { get; set; }

    public virtual DbSet<TourEvaluationLookUp> TourEvaluationLookUps { get; set; }

    public virtual DbSet<TourHotelClassLookUp> TourHotelClassLookUps { get; set; }

    public virtual DbSet<MoiClassification> MoiClassifications { get; set; }

    public virtual DbSet<TourMoiEserviceTourismHotelOccupancy> TourMoiEserviceTourismHotelOccupancies { get; set; }

    public virtual DbSet<MoiPreApprovement> MoiPreApprovements { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<TransactionLog> TransactionLogs { get; set; }
    public virtual DbSet<EserviceTypeBranch> EserviceTypeBranches { get; set; } 
    public virtual DbSet<TransactionTypesLookup> TransactionTypesLookups { get; set; }

    public virtual DbSet<TransferTransaction> TransferTransactions { get; set; }
    public virtual DbSet<RequestTransaction> RequestTransactions { get; set; }

    public DbSet<SystemOption> SystemOptions { get; set; }
   

    public virtual DbSet<WorkFlow> WorkFlows { get; set; }
    public virtual DbSet<FileUploadConfigurationsFront> FileUploadConfigurationsFronts { get; set; }
    public virtual DbSet<AttachRule> AttachRules { get; set; }
    public virtual DbSet<ContactUs> ContactUs { get; set; }
    public virtual DbSet<ResetUserPassword> ResetUserPassword { get; set; }
    public virtual DbSet<ScheduleReleaseTypes> ScheduleReleaseTypes { get; set; }
    public DbSet<LicencesNameChangeTransaction> LicencesNameChangeTransactions { get; set; }
    public DbSet<WorkFlowActionButton> WorkFlowActionButton { get; set; }
    public DbSet<WorkFlowButtonRoleAdmin> WorkFlowButtonRoleAdmin { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Server=DESKTOP-4E07CTB;Database=MOIInfo_EService4;Trusted_Connection=True;TrustServerCertificate=True;").EnableSensitiveDataLogging();
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //=> optionsBuilder.UseSqlServer("Server=.;Database=MOIInfo_EService4;Trusted_Connection=True;TrustServerCertificate=True;").EnableSensitiveDataLogging();
    => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=MOIInfo_EService43;Trusted_Connection=True;TrustServerCertificate=True;").EnableSensitiveDataLogging().LogTo(Console.WriteLine, LogLevel.Information);
    //=> optionsBuilder.UseSqlServer("Server=MOI-11-0040;Database=MOIInfo_EService4;Trusted_Connection=True;TrustServerCertificate=True;").EnableSensitiveDataLogging();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        try
        {
            modelBuilder.Entity<WorkFlowActionButton>(entity=>
            {
                entity.HasKey(x => x.Id);
            });



            modelBuilder.Entity<WorkFlowButtonRoleAdmin>(entity=>
            {
                entity.HasKey(entity=>entity.Id);
            });
             
            modelBuilder.Entity<ActivityChangeTypeTransaction>(entity =>
            {
                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
                entity.Property(e => e.LastUpdateUser).HasMaxLength(50);
            });
            modelBuilder.Entity<SystemOption>(entity =>
            {
                entity.HasKey(e => e.Id)
                  .HasName("PK_SystemOption");
            });
            modelBuilder.Entity<RequestTransaction>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<ResetUserPassword>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<ContactUs>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<ValidEserviceCombinations>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<LicencesNameChangeTransaction>(entity =>
            {
                entity.Property(e => e.LicencesNameOld);
                entity.Property(e => e.LicencesNameNew);
            });
            
            modelBuilder.Entity<ActivityTypesLookup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Mon_ActivityTypesLookup");

                entity.ToTable("ActivityTypesLookup");

                entity.Property(e => e.ActivityCode).HasMaxLength(50);
                entity.Property(e => e.MainLicenseId).HasColumnName("MainLicenseID");
                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            });

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Address_1");

                entity.ToTable("Address");

                entity.Property(e => e.Address1).HasColumnName("Address");
                entity.Property(e => e.AreaChartNo)
                    .HasMaxLength(50)
                    .HasColumnName("Area_ChartNo");
                entity.Property(e => e.AreaSize)
                    .HasMaxLength(50)
                    .HasColumnName("Area_Size");
                entity.Property(e => e.BlockArabic).HasMaxLength(50);
                entity.Property(e => e.City).HasMaxLength(50);
                entity.Property(e => e.FloorNo).HasMaxLength(50);
                entity.Property(e => e.StreetArabic).HasMaxLength(50);
            });

            modelBuilder.Entity<AddressChangeTransaction>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_AddressChangeTransactions_1");
            });

            modelBuilder.Entity<AreasLookup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Mon_AreasLookup");

                entity.ToTable("AreasLookup");

                entity.Property(e => e.GisAreaId).HasColumnName("GisAreaID");

                entity.HasOne(d => d.Governorate).WithMany(p => p.AreasLookups)
                    .HasForeignKey(d => d.GovernorateId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("GovernoratesLookup_AreasLookup_FK1");
            });

            modelBuilder.Entity<AspNetRequestMultipleUser>(entity =>
            {
                entity.Property(e => e.CreateAt).HasColumnType("datetime");
                entity.Property(e => e.UpdateAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.ToTable("AspNetRoles");
               // entity.HasKey(e => e.Id).HasName("PK_dbo.AspNetRoles");
                //entity.Property(e => e.Id)
                           // .ValueGeneratedOnAdd();
                entity.Property(e => e.Name).HasMaxLength(256);
            });
            modelBuilder.Entity<RoleAdmin>(entity =>
            {
                entity.ToTable("RoleAdmins");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.ToTable("AspNetUsers");
                entity.HasNoDiscriminator();
                entity.Property(e => e.CivilId)
                    .HasMaxLength(50)
                    .HasColumnName("CivilID");
               
                entity.Property(e => e.FullNameAr).HasMaxLength(256);
                entity.Property(e => e.FullNameEn).HasMaxLength(256);
               
               
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                //entity.HasKey(e => e.Id).HasName("PK_dbo.AspNetUserClaims");

                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                //entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId }).HasName("PK_dbo.AspNetUserLogins");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);
                entity.Property(e => e.ProviderKey).HasMaxLength(128);
                entity.Property(e => e.UserId).HasMaxLength(128);

                entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                //entity.HasKey(e => e.Id).HasName("PK_dbo.AspNetUserRoles");

                entity.Property(e => e.RoleId).HasDefaultValueSql("(N'User')");

               
            });
            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.ToTable("RolePermission");

                entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.PermissionId)
                    .HasConstraintName("FK_RolePermission_Permission");

                //entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                //    .HasForeignKey(d => d.RoleId)
                //    .HasConstraintName("FK_RolePermission_AspNetRoles");
            });

            modelBuilder.Entity<AspNetUserRoleAdmin>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_dbo.AspNetUserRoleAdmins");




            });

            modelBuilder.Entity<RolePermissionAdmin>(entity =>
            {
                entity.ToTable("RolePermissionAdmins");

                entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                    .HasForeignKey(d => d.PermissionAdminId);



            });

            modelBuilder.Entity<ChangeEmailTranaction>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Change_Email_Transaction");

                entity.ToTable("Change_Email_Tranaction");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.RequestDate).HasColumnType("datetime");
                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            });

            modelBuilder.Entity<ChangeMediaNameTransaction>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Change_MediaName");

                entity.ToTable("Change_MediaName_Transaction");

                entity.Property(e => e.RequestDate).HasColumnType("datetime");
                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            });



            modelBuilder.Entity<ChangeSocialMediaTransaction>(entity =>
            {
                entity.ToTable("Change_Social_Media_Transaction");

                entity.Property(e => e.NewAccountSocial_Media).HasColumnName("NewAccountSocial_Media");
                entity.Property(e => e.OldAccountSocial_MediaName).HasColumnName("OldAccountSocial_MediaName");
                entity.Property(e => e.RequestDate).HasColumnType("datetime");
                entity.Property(e => e.SocialMediaRequestType)
                    .HasMaxLength(20)
                    .HasColumnName("Social_Media_Request_type");
                entity.Property(e => e.SocialMediaType).HasColumnName("Social_Media_type");
                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            });

            modelBuilder.Entity<CommercialNameChangeTransaction>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_CommercialNameChangeTransactions_1");

                entity.Property(e => e.ComId).HasColumnName("ComID");
                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
                entity.Property(e => e.LastUpdateUser).HasMaxLength(50);
                entity.Property(e => e.NewCommercialName).HasMaxLength(250);
                entity.Property(e => e.OldCommercialName).HasMaxLength(250);
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CompanyNameChangeTransaction>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_CompanyNameChangeTransactions_1");

                entity.Property(e => e.CompId).HasColumnName("CompID");
                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
                entity.Property(e => e.LastUpdateUser).HasMaxLength(50);
                entity.Property(e => e.NewCompanyNameDir).HasMaxLength(250);
                entity.Property(e => e.OldCompnayNameDir).HasMaxLength(250);
                entity.Property(e => e.NewCompanyNameOwner).HasMaxLength(250);
                entity.Property(e => e.OldCompnayNameOwner).HasMaxLength(250);
            });

            modelBuilder.Entity<CountriesLookup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("CountriesLookup_PK");

                entity.ToTable("CountriesLookup");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<DelegationTransactionLog>(entity =>
            {
                entity.ToTable("DelegationTransactionLog");

                entity.Property(e => e.CreateAt).HasColumnType("datetime");
                entity.Property(e => e.LicenseId).HasColumnName("licenseId");
            });

            modelBuilder.Entity<ElawMoiWeLawRule>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Elaw_MOI__3214EC078CA84F54");

                entity.ToTable("Elaw_MOI_WE_Law_Rules");

                entity.Property(e => e.LawDescription).HasColumnName("Law_Description");
                entity.Property(e => e.LawTitle).HasColumnName("Law_Title");
                entity.Property(e => e.LawTypeId).HasColumnName("Law_TypeId");
                entity.Property(e => e.LawTypeName).HasColumnName("Law_TypeName");
            });

            modelBuilder.Entity<ElawMoiWeNews>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_MOI_WE_News");

                entity.ToTable("Elaw_MOI_WE_News");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ElawMoiWePage>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Elaw_MOI__3213E83F89A3B7B6");

                entity.ToTable("Elaw_MOI_WE_Pages");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.DescriptionEn).HasColumnName("Description_EN");
            });

            modelBuilder.Entity<Eservice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.ServiceId).IsUnique();
                //entity.Property(e => e.Id).HasMaxLength(128);
                //entity.Property(e => e.CreatedOn)
                //    .HasDefaultValueSql("(getdate())")
                //    .HasColumnType("datetime");
                //entity.Property(e => e.EserviceName).HasMaxLength(512);
                //entity.Property(e => e.EserviceNameAr).HasMaxLength(512);
                //entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            });

            modelBuilder.Entity<EserviceTypesLookup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_EserviceTypes");

                entity.ToTable("EserviceTypesLookup");

                entity.Property(e => e.CreatedOn)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.EserviceId).HasMaxLength(128);
                entity.Property(e => e.EserviceTypeAr).HasMaxLength(512);
                entity.Property(e => e.EserviceTypeEn).HasMaxLength(512);
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            });

            modelBuilder.Entity<ExceptionsLog>(entity =>
            {
                entity.Property(e => e.Action).HasMaxLength(256);
                entity.Property(e => e.ApplicationName).HasMaxLength(256);
                entity.Property(e => e.Controller).HasMaxLength(256);
                entity.Property(e => e.Created).HasColumnType("datetime");
                entity.Property(e => e.Method).HasMaxLength(256);
                entity.Property(e => e.SolvedOn).HasColumnType("datetime");
                entity.Property(e => e.Status).HasMaxLength(256);
            });

            modelBuilder.Entity<Form>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ServiceId });

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.FormName)
                    .HasMaxLength(1000)
                    .HasColumnName("Form_name");
                entity.Property(e => e.FormPath)
                    .HasMaxLength(1000)
                    .HasColumnName("Form_path");
                entity.Property(e => e.FormStatus)
                    .HasMaxLength(50)
                    .HasColumnName("Form_status");
                entity.Property(e => e.FormType)
                    .HasMaxLength(10)
                    .HasColumnName("Form_type");
            });

            modelBuilder.Entity<GovernoratesLookup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("GovernoratesLookup_PK");

                entity.ToTable("GovernoratesLookup");

                entity.Property(e => e.GisId).HasColumnName("GIS_ID");
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Licence>(entity =>
            {
                entity.HasKey(e => e.LicId).HasName("PK_Licences_1");

                entity.Property(e => e.ActiivityTypeId).HasColumnName("ActiivityTypeID");
                entity.Property(e => e.ClassificationDate).HasColumnType("datetime");
                entity.Property(e => e.ComExpireDate).HasColumnType("datetime");
                entity.Property(e => e.ComIssueDate).HasColumnType("datetime");
                entity.Property(e => e.ExpireDate).HasColumnType("datetime");
                entity.Property(e => e.FirstCreationDate).HasColumnType("datetime");
                entity.Property(e => e.IssueDate).HasColumnType("datetime");
                entity.Property(e => e.LastRenewDate).HasColumnType("datetime");
                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");
                entity.Property(e => e.LicNo).HasMaxLength(50);
                entity.Property(e => e.Motdate)
                    .HasColumnType("datetime")
                    .HasColumnName("MOTDate");
               
                entity.Property(e => e.ParentLicenseId).HasColumnName("ParentLicenseID");
                entity.Property(e => e.PreApprovalNo).HasMaxLength(50);

                entity.HasOne(d => d.LicenseStatusLookup).WithMany(p => p.Licences)
                    .HasForeignKey(d => d.LicStatusId)
                    .HasConstraintName("FK_Licences_LicenseStatusLookup");

                entity.HasOne(d => d.LicenceTypesLookup).WithMany(p => p.Licences)
                    .HasForeignKey(d => d.LicTypeId)
                    .HasConstraintName("FK_Licences_LicenceTypesLookup");
            });

            modelBuilder.Entity<LicenceTypesLookup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("LicenseeTypesLookup_PK");

                entity.ToTable("LicenceTypesLookup");

                entity.Property(e => e.NameAr).HasMaxLength(50);
                entity.Property(e => e.NameEn).HasMaxLength(50);
            });

            modelBuilder.Entity<LicenseEndingTransaction>(entity =>
            {
                entity.ToTable("LicenseEndingTransaction");

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
                entity.Property(e => e.LastUpdateUser).HasMaxLength(50);
                entity.Property(e => e.LicExpiredate)
                    .HasColumnType("datetime")
                    .HasColumnName("Lic_Expiredate");
            });

            modelBuilder.Entity<LicenseRenew>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_LicenseRenew_1");

                entity.ToTable("LicenseRenew");

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
                entity.Property(e => e.LastUpdateUser).HasMaxLength(50);
                entity.Property(e => e.LicenseId).HasColumnName("LicenseID");
                entity.Property(e => e.NewExpiryDateOld)
                    .HasMaxLength(50)
                    .HasColumnName("NewExpiryDate_Old");
                entity.Property(e => e.OldExpiryDateOld)
                    .HasMaxLength(50)
                    .HasColumnName("OldExpiryDate_Old");
            });

            modelBuilder.Entity<LicenseRenewTransaction>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_LicenseRenewTransaction_1");

                entity.ToTable("LicenseRenewTransaction");

                entity.Property(e => e.LicExpiredate)
                    .HasColumnType("datetime")
                    .HasColumnName("Lic_Expiredate");
                entity.Property(e => e.LicRenewDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Lic_RenewDate");
            });

            modelBuilder.Entity<LicenseStatusLookup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("LicenseStatusLookup_PK");

                entity.ToTable("LicenseStatusLookup");

                entity.Property(e => e.NameAr).HasMaxLength(50);
                entity.Property(e => e.NameEn).HasMaxLength(50);
            });

            modelBuilder.Entity<LicenseTypeChangeTransaction>(entity =>
            {
                entity.ToTable("LicenseType_ChangeTransaction");

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
                entity.Property(e => e.LastUpdateUser).HasMaxLength(50);
                entity.Property(e => e.LicTypeNew).HasMaxLength(50);
                entity.Property(e => e.LicTypeOld).HasMaxLength(50);
                entity.Property(e => e.LicenseNo).HasMaxLength(50);
                entity.Property(e => e.NewCivilId)
                    .HasMaxLength(50)
                    .HasColumnName("NewCivilID");
                entity.Property(e => e.NewRequestid).HasColumnName("New_requestid");
                entity.Property(e => e.OldCivilId)
                    .HasMaxLength(50)
                    .HasColumnName("OldCivilID");
                entity.Property(e => e.Requestid).HasColumnName("Requestid");
            });

            modelBuilder.Entity<LinksLookup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_MOI_WE_Links");

                entity.ToTable("LinksLookup");

                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<LogosLookup>(entity =>
            {
                entity.ToTable("LogosLookup");

                entity.Property(e => e.Link).HasMaxLength(500);
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.ToTable("MenuItems");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(50);
                entity.Property(e => e.Url).HasColumnName("URL");
            });

            modelBuilder.Entity<Module>(entity =>
            {
                entity.ToTable("Modules"); // Table name in the database

                entity.HasKey(m => m.Id); // Primary Key

                entity.Property(m => m.Name)
                      .IsRequired()
                      .HasMaxLength(100); // Example: Setting max length

                entity.Property(m => m.Description)
                      .HasMaxLength(255);
                // Example: Setting max length
                // One-to-Many relationship with MenuItems
                entity.HasMany(m => m.MenuItems)
                      .WithOne(mi => mi.Module)
                      .HasForeignKey(mi => mi.ModuleId)
                      .OnDelete(DeleteBehavior.Cascade); // Cascade delete if a module is removed
            });
            modelBuilder.Entity<MoiEserviceDepartment>(entity =>
            {
                entity.ToTable("MOI_EService_Department");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.SectorId).HasColumnName("SectorID");
                entity.Property(e => e.Sort).HasDefaultValue(0);
                entity.Property(e => e.Status).HasDefaultValue(true);
            });

            modelBuilder.Entity<MoiEserviceLicEndingReason>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_MOI_EService_LicEndingReasons");

                entity.ToTable("MOI_EService_LicEndingReasons");

                entity.Property(e => e.ReasonName).HasColumnName("Reason_Name");
            });


            modelBuilder.Entity<MoiEserviceLicenseInfo>(entity =>
            {
                entity.HasKey(e =>  e.Id);

                entity.ToTable("MOI_Eservice_License_Info");

                //entity.Property(e => e.Id).ValueGeneratedOnAdd();
                //entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
                //entity.Property(e => e.Action).HasMaxLength(512);
                //entity.Property(e => e.ActvityTypeId).HasColumnName("ActvityTypeID");
                //entity.Property(e => e.Branch).HasMaxLength(512);
                //entity.Property(e => e.Controller).HasMaxLength(512);
                //entity.Property(e => e.EserviceTypeBranchId).HasColumnName("EserviceTypeBranchID");
                //entity.Property(e => e.ReqTypeId).HasColumnName("EserviceTypeID");
                //entity.Property(e => e.FixedFees).HasColumnType("numeric(18, 3)");
                //entity.Property(e => e.Sort).HasDefaultValue(0);
                //entity.Property(e => e.Status).HasDefaultValue(true);
                //entity.Property(e => e.Url).HasMaxLength(512);
                //entity.Property(e => e.VariableFees).HasColumnType("numeric(18, 3)");
            });

            modelBuilder.Entity<MoiEserviceLicensesRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId).HasName("PK_MOI_EService_LicensesRequests");

                entity.ToTable("MOI_EService_LicensesRequests");

                entity.Property(e => e.RequestId).HasColumnName("Request_id");
                entity.Property(e => e.ActivityCode).HasMaxLength(50);
          
                entity.Property(e => e.ActivityTypeId).HasColumnName("ActivityTypeID");
                entity.Property(e => e.AppCivilId).HasMaxLength(50);
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
             
                entity.Property(e => e.CompletionDocs).HasMaxLength(50);
                entity.Property(e => e.IsRenewTradeApprovalLetter).HasColumnName("isRenewTradeApprovalLetter");
                entity.Property(e => e.IsTradeApprovalLetter).HasColumnName("isTradeApprovalLetter");
                entity.Property(e => e.Licamount).HasColumnType("numeric(8, 3)");
                entity.Property(e => e.LicenseId).HasColumnName("LicenseID");
                entity.Property(e => e.Licexpiredate).HasColumnType("datetime");
                entity.Property(e => e.Licno)
                    .HasMaxLength(50)
                    .HasColumnName("licno");
                entity.Property(e => e.Licpaystatus).HasMaxLength(50);
                entity.Property(e => e.Licreqtime).HasColumnType("datetime");
                entity.Property(e => e.LicrequestIsDeleted).HasColumnName("licrequestIsDeleted");
                entity.Property(e => e.ManCivilId).HasMaxLength(50);
                entity.Property(e => e.RequestAttach)
                    .HasMaxLength(50)
                    .HasColumnName("Request_attach");
                entity.Property(e => e.RequestModDate).HasColumnType("datetime");
                entity.Property(e => e.RequestNote).HasColumnName("Request_note");
                entity.Property(e => e.SectorId).HasColumnName("SectorID");
                entity.Property(e => e.MandoobCivilId).HasMaxLength(50);
            });

            modelBuilder.Entity<MoiEserviceRequestPaymentDetail>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_MOI_EService_RequestPaymentDetails");

                entity.ToTable("MOI_EService_RequestPaymentDetails");

                entity.Property(e => e.LicenceId).HasColumnName("LicenceID");
                entity.Property(e => e.LicenseCategory).HasColumnName("License_Category");
                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(50)
                    .HasColumnName("Payment_Method");
                entity.Property(e => e.RequestId).HasColumnName("RequestId");
                entity.Property(e => e.TotalAmount).HasColumnType("numeric(18, 3)");
                entity.Property(e => e.TrackId).HasColumnName("TrackID");
                entity.Property(e => e.TranId).HasColumnName("TranID");
                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<MoiEserviceRequestsAttach>(entity =>
            {
                entity.HasKey(e => e.AttachId).HasName("PK_MOI_EService_RequestsAttach");

                entity.ToTable("MOI_EService_RequestsAttach");
            });

            modelBuilder.Entity<MoiEserviceSector>(entity =>
            {
                entity.ToTable("MOI_EService_Sector");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Sort).HasDefaultValue(0);
                entity.Property(e => e.Status).HasDefaultValue(true);
            });

            modelBuilder.Entity<MoiEserviceSysUser>(entity =>
            {
                entity.ToTable("MOI_EService_SysUsers");

                //entity.HasNoKey();
                entity.HasNoDiscriminator();

                entity.Property(e => e.SysUserId).HasColumnName("SysUserId");
                entity.Property(e => e.CivilId).HasColumnName("CivilID");
                entity.Property(e => e.CreateDate)
                    .HasDefaultValueSql("(getdate())")
                    .HasColumnType("datetime");
                entity.Property(e => e.Gisaddress).HasColumnName("GISAddress");
                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
                entity.Property(e => e.ModifyDate).HasColumnType("datetime");
                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
                entity.Property(e => e.Status).HasDefaultValue(true);
            });

            modelBuilder.Entity<MoiEserviceSysUsersActivityLog>(entity =>
            {
                entity.ToTable("MOI_EService_SysUsersActivityLog");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.ActivityDate).HasColumnType("datetime");
                entity.Property(e => e.ActivityItemId).HasColumnName("ActivityItemID");
                entity.Property(e => e.SysUserId).HasColumnName("SysUserId");
            });

            modelBuilder.Entity<MoiEservicesRequestTransaction>(entity =>
            {
                entity.HasKey(e => e.TransReqId);

                entity.ToTable("MOI_EServices_Request_Transaction");

                entity.Property(e => e.TransReqId).HasColumnName("TransReq_Id");
                entity.Property(e => e.EmployeeCivilId).HasMaxLength(50);
                entity.Property(e => e.LicenseId).HasColumnName("licenseId");
                entity.Property(e => e.OperationDate).HasColumnType("datetime");
                entity.Property(e => e.ReqStatusName).HasMaxLength(50);

            });

            modelBuilder.Entity<MoiSahelActivityLog>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ServiceId });

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<MoiSocialMedia>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_MOI_WE_Social Media");

                entity.ToTable("MOI_Social_Media");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.AccountSocial).HasMaxLength(250);
                entity.Property(e => e.SocialType).HasColumnName("Social_type");
            });

            modelBuilder.Entity<MonMoiEservicePartiesSub>(entity =>
            {
                entity.ToTable("Mon_MoiEservicePartiesSubs");
            });

            modelBuilder.Entity<MonMoiEservicePartiesType>(entity =>
            {
                entity.ToTable("Mon_MoiEservicePartiesTypes");
            });

            modelBuilder.Entity<MonMoiEservicePartySubscriber>(entity =>
            {
                entity.ToTable("Mon_MoiEservicePartySubscribers");
            });

            modelBuilder.Entity<MonMoiEservicesPartyPressRequestPhase>(entity =>
            {
                entity.HasKey(e => e.PhaseId);

                entity.ToTable("Mon_MoiEservicesPartyPressRequestPhases");
            });

            modelBuilder.Entity<Partner>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Partners_1");

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
                entity.Property(e => e.LastUpdateUser).HasMaxLength(50);
            });

            modelBuilder.Entity<PartnerOldChangeTransaction>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_PartnerChangeTransactions");

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
                entity.Property(e => e.LastUpdateUser).HasMaxLength(50);
                entity.Property(e => e.PartId).HasColumnName("PartID");
            });

            modelBuilder.Entity<PartnerNewChangeTransaction>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_PartnerNewChangeTransactions");

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
                entity.Property(e => e.LastUpdateUser).HasMaxLength(50);
                entity.Property(e => e.PartId).HasColumnName("PartID");
            });
            modelBuilder.Entity<Permission>(entity =>
            {

                entity.ToTable("Permission");
            });
            modelBuilder.Entity<PermissionAdmin>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("PermissionAdmins");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Persons_1");

                entity.Property(e => e.CivilId).HasMaxLength(100);
                entity.Property(e => e.CompanyNo).HasMaxLength(50);
                entity.Property(e => e.Education).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(50);
                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
                entity.Property(e => e.LastUpdateUser).HasMaxLength(100);
                entity.Property(e => e.Name1).HasMaxLength(100);
                entity.Property(e => e.Name2).HasMaxLength(50);
                entity.Property(e => e.Name3).HasMaxLength(50);
                entity.Property(e => e.Name4).HasMaxLength(50);
                entity.Property(e => e.NationaliyName).HasMaxLength(50);
                entity.Property(e => e.NationaltiyNo).HasMaxLength(100);
                entity.Property(e => e.PersonTypeId).HasColumnName("PersonTypeID");
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.QualificationDateOld)
                    .HasMaxLength(100)
                    .HasColumnName("QualificationDate_Old");
            });



            modelBuilder.Entity<PesronTypeLookUp>(entity =>
            {
                entity.ToTable("PesronTypeLookUp");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");
                entity.Property(e => e.Descr).HasMaxLength(50);
            });

            modelBuilder.Entity<QualificationsLookup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("QualificationsLookup_PK");

                entity.ToTable("QualificationsLookup");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<RenouncementTransaction>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_RenouncementTransactions_1");

                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
                entity.Property(e => e.LastUpdateUser).HasMaxLength(50);
                entity.Property(e => e.NewBirthDate).HasMaxLength(50);
                entity.Property(e => e.NewCivilId).HasMaxLength(50);
                entity.Property(e => e.NewExpiryDate).HasColumnType("datetime");
                entity.Property(e => e.NewName).HasMaxLength(250);
                entity.Property(e => e.NewNationallityNo).HasMaxLength(50);
                entity.Property(e => e.NewProfessionalExperience).HasMaxLength(50);
                entity.Property(e => e.NewQualification).HasMaxLength(50);
                entity.Property(e => e.NewQualificationCountry).HasMaxLength(250);
                entity.Property(e => e.NewQualificationDate).HasMaxLength(50);
                entity.Property(e => e.OldBirthDate).HasMaxLength(50);
                entity.Property(e => e.OldCivilId).HasMaxLength(50);
                entity.Property(e => e.OldName).HasMaxLength(250);
                entity.Property(e => e.OldNationalityNo).HasMaxLength(50);
                entity.Property(e => e.OldProfessionalExperience).HasMaxLength(50);
                entity.Property(e => e.OldQualification).HasMaxLength(50);
                entity.Property(e => e.OldQualificationCountry).HasMaxLength(250);
                entity.Property(e => e.OldQualificationDate).HasMaxLength(50);
            });

            modelBuilder.Entity<ReplacementOfLostTransaction>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_ReplacementOfLostTransactions_1");
            });

            modelBuilder.Entity<RequestStatusLookup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_RequestStatusLookup_1");

                entity.ToTable("RequestStatusLookup");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Sort).HasDefaultValue(0);
                entity.Property(e => e.Status).HasDefaultValue(true);
            });

            modelBuilder.Entity<RequestsTypesLookup>(entity =>
            {
                entity.ToTable("RequestsTypesLookup");

                entity.HasKey(e => e.Id);
               
            });



            modelBuilder.Entity<SahelSubscriber>(entity =>
            {
                entity.HasKey(e => e.CivilId).HasName("PK_SahelSubscribers_1");

                entity.Property(e => e.CivilId)
                    .HasMaxLength(12)
                    .HasColumnName("civil_id");
            });

            modelBuilder.Entity<SettingsLookup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_MOI_WE_Settings");

                entity.ToTable("SettingsLookup");

                entity.Property(e => e.Id).HasColumnName("id");
            });

            modelBuilder.Entity<SocialTypeLookup>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_MOI_WE_SocialType");

                entity.ToTable("SocialTypeLookup");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.Sort).HasDefaultValue(0);
                entity.Property(e => e.Status).HasDefaultValue(true);
            });

            modelBuilder.Entity<TchangeManager>(entity =>
            {
                entity.HasKey(e => e.ManagerId).HasName("PK_TChangeManager");

                entity.ToTable("TChangeManager");

                //entity.Property(e => e.ManagerId).HasColumnName("Manager_Id");
                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
                entity.Property(e => e.LastUpdateUser).HasMaxLength(50);
                entity.Property(e => e.ManagerBookdate).HasColumnName("Manager_bookdate");
                entity.Property(e => e.ManagerBookno)
                    .HasMaxLength(50)
                    .HasColumnName("Manager_bookno");
                entity.Property(e => e.ManagerLicno).HasColumnName("Manager_licno");
                entity.Property(e => e.ManagerNewbirthdate).HasColumnName("Manager_newbirthdate");
                entity.Property(e => e.ManagerNewcivilid)
                    .HasMaxLength(50)
                    .HasColumnName("Manager_newcivilid");
                entity.Property(e => e.ManagerNewcountryid).HasColumnName("Manager_newcountryid");
               
                entity.Property(e => e.ManagerNewqualificationid).HasColumnName("Manager_newqualificationid");
                entity.Property(e => e.ManagerOldbirthdate).HasColumnName("Manager_oldbirthdate");
                entity.Property(e => e.ManagerOldcivilid)
                    .HasMaxLength(50)
                    .HasColumnName("Manager_oldcivilid");
                entity.Property(e => e.ManagerOldcountryid).HasColumnName("Manager_oldcountryid");
                entity.Property(e => e.ManagerOldname)
                    .HasMaxLength(100)
                    .HasColumnName("Manager_oldname");
                entity.Property(e => e.ManagerOldqualificationid).HasColumnName("Manager_oldqualificationid");
                entity.Property(e => e.ManagerTradeletterAttach).HasColumnName("Manager_tradeletter_attach");
                entity.Property(e => e.NewAddress).HasMaxLength(50);
                entity.Property(e => e.NewEmail).HasMaxLength(50);
                entity.Property(e => e.NewMobile).HasMaxLength(50);
                entity.Property(e => e.NewNationality).HasMaxLength(50);
                entity.Property(e => e.NewNationaltiyNo).HasMaxLength(100);
                entity.Property(e => e.OldAddress).HasMaxLength(50);
                entity.Property(e => e.OldEmail).HasMaxLength(50);
                entity.Property(e => e.OldMobile).HasMaxLength(50);
                entity.Property(e => e.OldNationality).HasMaxLength(50);
                entity.Property(e => e.OldNationaltiyNo).HasMaxLength(100);

                entity.HasOne(d => d.ManagerOldqualification).WithMany(p => p.TchangeManagers)
                    .HasForeignKey(d => d.ManagerOldqualificationid)
                    .HasConstraintName("FK_TChangeManager_QualificationsLookup");
            });



            modelBuilder.Entity<TestablishContract>(entity =>
            {
                entity.HasKey(e => e.EsId).HasName("PK_TEstablishContract_1");

                entity.ToTable("TEstablishContract");

                entity.Property(e => e.EsId)
                    .ValueGeneratedNever()
                    .HasColumnName("es_id");
                entity.Property(e => e.EsTitle)
                    .HasMaxLength(50)
                    .HasColumnName("es_title");
            });

            modelBuilder.Entity<TourClassBranchLookUp>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Class_Branch_LookUp");

                entity.ToTable("Tour_Class_Branch_LookUp");

               
            });

            modelBuilder.Entity<TourClassTypeLookUp>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Class_Type_LookUp");

                entity.ToTable("Tour_Class_Type_LookUp");

                entity.Property(e => e.Id).HasColumnName("Id");
            });

            modelBuilder.Entity<TourEvaluationListHotel>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_EvaluationList_Hotel");

                entity.ToTable("Tour_EvaluationList_Hotel");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.HotelClassId).HasColumnName("HotelClassId");
                
                entity.Property(e => e.ClassificationId).HasColumnName("ClassificationID");
                entity.Property(e => e.EvalitemId).HasColumnName("EvalitemID");
                entity.Property(e => e.LicId).HasColumnName("LicID");
                entity.Property(e => e.RequestId).HasColumnName("RequestId");
                //  entity.HasOne<TourHotelClassLookUp>()
                //.WithMany() // Set the inverse navigation if needed
                //.HasForeignKey(e => e.ClassItemId);
                //entity.HasOne<TourEvaluationLookUp>()
                //       .WithMany() // TourEvaluationLookUp has many TourEvaluationListHotel
                //       .HasForeignKey(e => e.EvalitemId);
            });

            modelBuilder.Entity<TourEvaluationLookUp>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.ToTable("Tour_Evaluation_LookUp");


                entity.Property(e => e.Id).HasColumnName("Id");
            });

            modelBuilder.Entity<TourHotelClassLookUp>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Hotel_Class");

                entity.ToTable("Tour_Hotel_Class_LookUp");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
                entity.Property(e => e.ClassBranchId).HasColumnName("ClassBranchID");
                entity.Property(e => e.ClassTypeId).HasColumnName("ClassTypeID");
            });

            modelBuilder.Entity<MoiClassification>(entity =>
            {
                entity.HasKey(e => e.ClassifyId).HasName("PK_MoiClassification"); ;

                entity.ToTable("MoiClassification");

                
            });

            modelBuilder.Entity<TourMoiEserviceTourismHotelOccupancy>(entity =>
            {
                entity.HasKey(e => e.OccupancyId).HasName("PK_MOI_EService_TourismHotel_Occupancy");

                entity.ToTable("Tour_MOI_EService_TourismHotel_Occupancy");

                entity.Property(e => e.OccupancyId).HasColumnName("Occupancy_ID");
                entity.Property(e => e.BuildingId).HasColumnName("Building_ID");
            });

            modelBuilder.Entity<MoiPreApprovement>(entity =>
            {
                entity.HasKey(e => e.PreAppId).HasName("PK_MoiPreApprovement");

                entity.ToTable("MoiPreApprovement");

                entity.Property(e => e.PreAppId).HasColumnName("PreAppId");
                entity.Property(e => e.ActivityTypeId).HasColumnName("ActivityTypeID");
                entity.Property(e => e.BuildingId).HasColumnName("Building_ID");
        
                entity.Property(e => e.ClassificationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Classification_Date");
                entity.Property(e => e.ComExpiryDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Com_Expiry_Date");
                entity.Property(e => e.ComIssuingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Com_Issuing_Date");
             
                entity.Property(e => e.CompanyId).HasColumnName("Company_ID");
                entity.Property(e => e.LicTypeId).HasColumnName("LicTypeID");
                entity.Property(e => e.LicenseExpireDate)
                    .HasColumnType("datetime")
                    .HasColumnName("License_ExpireDate");
                entity.Property(e => e.LicenseIssueDate)
                    .HasColumnType("datetime")
                    .HasColumnName("License_IssueDate");
                entity.Property(e => e.LicenseName)
                    .HasMaxLength(200)
                    .HasColumnName("License_Name");
                entity.Property(e => e.LicenseNo)
                    .HasMaxLength(50)
                    .HasColumnName("License_No");
                entity.Property(e => e.ManagerId).HasColumnName("Manager_ID");
                entity.Property(e => e.RequestId).HasColumnName("Request_ID");
                
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK_Transactions_1");

                entity.Property(e => e.RequestDate).HasColumnType("datetime");
                entity.Property(e => e.TransDate).HasColumnType("datetime");
                entity.Property(e => e.UsercivilId).HasMaxLength(50);

                //entity.HasOne(d => d.TransType).WithMany(p => p.Transactions)
                //    .HasForeignKey(d => d.TransTypeId)
                //    .HasConstraintName("FK_Transaction_TransactionTypesLookup_TypeId");
            });

            modelBuilder.Entity<TransactionLog>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ServiceId });

                entity.ToTable("TransactionLog");

                entity.Property(e => e.Lid).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TransactionTypesLookup>(entity =>
            {
                entity.ToTable("TransactionTypesLookup");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<TransferTransaction>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("TransferTransactions_PK");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");
                entity.Property(e => e.CivilId).HasMaxLength(50);
                entity.Property(e => e.Cname)
                    .HasMaxLength(150)
                    .HasColumnName("CName");
                entity.Property(e => e.CompanyCivilId).HasMaxLength(50);
                entity.Property(e => e.CompanyName).HasMaxLength(250);
                entity.Property(e => e.LastUpdateDate).HasColumnType("datetime");
                entity.Property(e => e.LastUpdateUser).HasMaxLength(50);
                entity.Property(e => e.NationaltiyNo).HasMaxLength(50);
                entity.Property(e => e.PartnerNames).HasMaxLength(250);
                entity.Property(e => e.PersonName1).HasMaxLength(50);
                entity.Property(e => e.PersonName2).HasMaxLength(50);
                entity.Property(e => e.PersonName3).HasMaxLength(50);
                entity.Property(e => e.PersonName4).HasMaxLength(50);
                entity.Property(e => e.ProfessionalExperience).HasMaxLength(1000);
                entity.Property(e => e.Qualification).HasMaxLength(50);
                entity.Property(e => e.QualificationCountry).HasMaxLength(50);
                entity.Property(e => e.QualificationDate).HasMaxLength(50);
                entity.Property(e => e.TransferType).HasMaxLength(250);
            });

            modelBuilder.Entity<WorkFlow>(entity =>
            {
                entity.ToTable("WorkFlow");
                entity.HasOne(w => w.Eservice)
            .WithMany() // No navigation property in Eservice for WorkFlow
            .HasForeignKey(w => w.ServiceId) // Foreign key in WorkFlow
            .HasPrincipalKey(e => e.ServiceId);

            });
            modelBuilder.Entity<AttachRule>(entity =>
            {
                entity.ToTable("AttachRule");
                entity.HasOne(w => w.Eservice)
            .WithMany() // No navigation property in Eservice for WorkFlow
            .HasForeignKey(w => w.ServiceId) // Foreign key in WorkFlow
            .HasPrincipalKey(e => e.ServiceId);

            });
            modelBuilder.Entity<FileUploadConfigurationsFront>(entity =>
            {
                entity.ToTable("FileUploadConfigurationsFront");

                entity.HasKey(e => e.Id);
            });
            modelBuilder.Entity<EserviceTypeBranch>(entity =>
            {
                // Define the primary key
                entity.HasKey(e => e.Id).HasName("PK_EserviceTypeBranches");

                // Map the entity to the correct table name in the database
                entity.ToTable("EserviceTypeBranches");

            });
            OnModelCreatingPartial(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Error occurred during model creation: {ex.Message}");
            // Optionally, you can throw the exception again or handle it accordingly
            throw;
        }
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

}
   

