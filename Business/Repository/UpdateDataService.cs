using Business.Enums;
using Business.Interfaces;
using Business.ModelWithSpecification;
using Business.ViewModel;
using Business.ViewModel.Dynamic;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class UpdateDataService:IUpdateDataService
    {
        private readonly IUnitOfwork _unitOfwork;
        private readonly IDataFetchService _dataFetchService;

        public UpdateDataService(IUnitOfwork unitOfwork,IDataFetchService dataFetchService)
        {
            _unitOfwork = unitOfwork;
            _dataFetchService = dataFetchService;
        }

        public async Task HandleAttachmentsAsync(UpdatedRequestVM updatedRequestVM, MoiEserviceLicensesRequest request, MoiEserviceSysUser employee,int serviceId)
        {
            // Handle new attachment if a file is provided
            if (updatedRequestVM.saveResponseVMs != null)
            {
                foreach (var item in updatedRequestVM.saveResponseVMs)
                {
                    var newAttachment = new MoiEserviceRequestsAttach
                    {
                        AttachName = item.FileName,
                        AttachPath = item.FilePath,
                        AttachFlag=item.FieldName,
                        IsDeleted=false,
                        AttachRequestid = request.RequestId,
                        ServiceId = serviceId,
                        UploadedBy = employee.CivilId,
                        UploadedDate = DateTime.Now,
                        IsMandatory = item.IsRequired,
                        IsApproved = true,
                        AttachType = ".pdf",
                        IsLatest=true
                    };

                    await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>().Create(newAttachment);
                }
            }

            // Update existing attachments based on the provided states
            var existingAttachments = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>()
                .GetByCondition(a => a.AttachRequestid == request.RequestId).ToListAsync();

            if (updatedRequestVM.AttachmentStates != null)
            {
                var updatedAttachmentStates = updatedRequestVM.AttachmentStates;

                foreach (var attachment in existingAttachments)
                {
                    var updatedState = updatedAttachmentStates.FirstOrDefault(x => x.AttachmentId == attachment.AttachId);

                    if (updatedState != null)
                    {
                        if (updatedState.State == "checked" && (attachment.IsApproved == null || attachment.IsApproved == false))
                        {
                            attachment.IsApproved = true;
                            attachment.IsLatest = true;
                        }
                        else if (updatedState.State == "unchecked" && attachment.IsApproved == true)
                        {
                            attachment.IsApproved = false;
                            attachment.IsLatest = false;
                        }
                    }
                }

                await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>().UpdateRange(existingAttachments);
            }
        }

        public async Task<MoiEservicesRequestTransaction> SaveRequestTransaction(MoiEserviceLicensesRequest request, string action, string statusName, UpdatedRequestVM updatedRequestVM, MoiEserviceSysUser employee,int serviceId)
        {
            var requestTransaction = new MoiEservicesRequestTransaction
            {
                RequestId = request.RequestId,
                OperationDate = DateTime.UtcNow,
                ServiceId = serviceId,
                LicenseId = request.LicenseId,
                Notes = updatedRequestVM.Note,
                ReqStatusName = statusName,
                ReqStatusId = updatedRequestVM.StatusId,
                EmployeeCivilId = updatedRequestVM.NameUser,
                Activity = action,
                EmployeeId = employee.SysUserId
            };

            await _unitOfwork.genericRepository<MoiEservicesRequestTransaction>().Create(requestTransaction);
            return requestTransaction; // Return the created transaction
        }

        public async Task<MoiEserviceSysUsersActivityLog> SaveUserLog(MoiEserviceSysUser employee, string action, UpdatedRequestVM updatedRequestVM)
        {
            var userLog = new MoiEserviceSysUsersActivityLog
            {
                UserFullName = employee.Name,
                SysUserId = employee.SysUserId,
                ActivityDate = DateTime.UtcNow,
                Note = updatedRequestVM.Note,
                Section = updatedRequestVM.ActionName,
                RequestId=updatedRequestVM.RequestId,
                
                Activity = action,
                ChangeLogs = string.Join(", ", updatedRequestVM.ChangeLogs)
            };

            await _unitOfwork.genericRepository<MoiEserviceSysUsersActivityLog>().Create(userLog);
            return userLog; // Return the created user log
        }

        public string RemoveHtmlRow(string htmlContent, string rowId)
        {
            string pattern = $@"<tr id=""{rowId}"">.*?</tr>";
            return System.Text.RegularExpressions.Regex.Replace(htmlContent, pattern, string.Empty, System.Text.RegularExpressions.RegexOptions.Singleline);
        }

        public async Task<(bool, string, string)> IsFinalCycleStatusAsync(/*int ActivityTypeId,*/ int RequestTypeId, int serviceId, int Requeststatusid)
        {
            var workflow = await _unitOfwork.genericRepository<WorkFlow>()
                .GetByCondition(w => w.RequestTypeId == RequestTypeId
                                 // && w.ActivityTypeId == ActivityTypeId
                                  && w.ServiceId == serviceId
                                  && w.NextStatusId == Requeststatusid)
                .FirstOrDefaultAsync();

            string conditionMet = null; // لمعرفة الشرط الذي تحقق
            string FlagrequestStatus = "";

            if (workflow != null && workflow.Conditions != "" && workflow.Conditions != null && workflow.FlagRequestStatus != null)
            {
                var conditions = JsonConvert.DeserializeObject<List<Condition>>(workflow.Conditions);

                if (conditions.Any(c =>
                    c.Field.Equals("RequestStatus", StringComparison.OrdinalIgnoreCase) &&
                    c.Value.ToString().Equals("final", StringComparison.OrdinalIgnoreCase)))
                {
                    conditionMet = "final";
                }

                if (conditions.Any(c =>
                    c.Field.Equals("RequestStatus", StringComparison.OrdinalIgnoreCase) &&
                    c.Value.ToString().Equals("Payment", StringComparison.OrdinalIgnoreCase)))
                {
                    conditionMet = "Payment";
                }

                if (!string.IsNullOrEmpty(conditionMet) || !string.IsNullOrEmpty(FlagrequestStatus))
                {
                    return (true, conditionMet, "No FlagrequestStatus"); // الشرط تحقق مع تحديد نوعه
                }
            }
            else if (workflow != null && workflow.FlagRequestStatus != "" && workflow.FlagRequestStatus != null && workflow.Conditions == "")
            {
                FlagrequestStatus = workflow.FlagRequestStatus;
                return (true, "No Condition met", FlagrequestStatus);
            }
            else if (workflow != null && workflow.FlagRequestStatus == "" && workflow.FlagRequestStatus == null && workflow.Conditions != null && workflow.Conditions != "")
            {
                var conditions = JsonConvert.DeserializeObject<List<Condition>>(workflow.Conditions);

                if (conditions.Any(c =>
                    c.Field.Equals("RequestStatus", StringComparison.OrdinalIgnoreCase) &&
                    c.Value.ToString().Equals("final", StringComparison.OrdinalIgnoreCase)))
                {
                    conditionMet = "final";
                }

                if (conditions.Any(c =>
                    c.Field.Equals("RequestStatus", StringComparison.OrdinalIgnoreCase) &&
                    c.Value.ToString().Equals("Payment", StringComparison.OrdinalIgnoreCase)))
                {
                    conditionMet = "Payment";
                }

                if (!string.IsNullOrEmpty(conditionMet))
                {
                    return (true, conditionMet, "No Flag Request status"); // الشرط تحقق مع تحديد نوعه
                }
            }

            return (false, "No condition met", "No RequestStatus Flag"); // لا يوجد شرط متحقق
        }
        public async Task HandleRenewal(UpdatedRequestVM updatedRequestVM, MoiEserviceLicensesRequest request, DateTime? ExpireDate, DateTime? IssueDate, MoiEserviceSysUser employee)
        {
            if (updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.Renew)
            {
                var renewSpec = new RenewWithSpec(request.LicenseId ?? 0, request.ServiceId ?? 0);
                var Renew = await _unitOfwork.genericRepository<LicenseRenew>().GetByIdWithSpec(renewSpec);
                var licencesSpec = new LicencesWithSpecificService(request.LicenseId ?? 0, request.ServiceId ?? 0);
                var Licences = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);

                Renew.RequestStatusId = updatedRequestVM.StatusId;

                DateTime? ExpireDateold = ExpireDate ?? Licences.ExpireDate;
                DateTime? renewDateTrans = updatedRequestVM.requestStatusValue != "final" ? Licences.ExpireDate : ExpireDate;

                if (updatedRequestVM.requestStatusValue == "final" || updatedRequestVM.Flag == "final")
                {
                    Renew.LastUpdateDate = DateTime.Now;
                    Renew.OldExpiryDate = Licences.ExpireDate ?? default(DateTime);
                    Renew.NewExpiryDate = ExpireDate ?? default(DateTime);
                    Renew.LastUpdateUser = employee.Name;

                    Licences.ExpireDate = ExpireDate;
                    Licences.IssueDate = IssueDate;
                    Licences.LastRenewDate = DateTime.Now;
                    Licences.LicStatusId = (int)licencesStatusEnum.Updated;
                }

                var renewtransaction = new LicenseRenewTransaction
                {
                    LicExpiredate = ExpireDateold,
                    RequestId = (int)request.RequestId,
                    LicRenewDate = renewDateTrans,
                    RequestStatusId = updatedRequestVM.StatusId,
                    ServiceId = request.ServiceId
                };

                await _unitOfwork.genericRepository<LicenseRenewTransaction>().Create(renewtransaction);
                await _unitOfwork.genericRepository<Licence>().Update(Licences);
                await _unitOfwork.genericRepository<LicenseRenew>().Update(Renew);
            }
        }
        public async Task HandleRenouncement(UpdatedRequestVM updatedRequestVM, MoiEserviceLicensesRequest request)
        {
            if (updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.Renouncement && (updatedRequestVM.requestStatusValue == "final" || updatedRequestVM.Flag == "final"))
            {
                var renouncementSpec = new OwnerChangeTransWithSpec(request.ServiceId ?? 0, request.RequestId);
                var renouncement = await _unitOfwork.genericRepository<RenouncementTransaction>().GetByIdWithSpec(renouncementSpec);
                var personNew = await _unitOfwork.genericRepository<Person>().GetByIdObject(p => p.CivilId == renouncement.NewCivilId);

                var licencesSpec = new LicencesWithSpecificService(request.LicenseId ?? 0, request.ServiceId ?? 0);
                var licences = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);

                licences.ApplicantCivilId = renouncement.NewCivilId;
                licences.ApplicantId = personNew.Id;

                await _unitOfwork.genericRepository<Licence>().Update(licences);
            }
        }
        public async Task HandleEndingLicenses(UpdatedRequestVM updatedRequestVM, MoiEserviceLicensesRequest request)
        {
            if (updatedRequestVM.ReqTypeId == (int)RequestTypeEnum.EndLicences && (updatedRequestVM.requestStatusValue == "final" || updatedRequestVM.Flag == "final"))
            {
                var licenEndingWithSpec = new EndingReasonChangeTransWithSpec(request.RequestId);
                var licenEnding = await _unitOfwork.genericRepository<LicenseEndingTransaction>().GetByIdWithSpec(licenEndingWithSpec);
                var licencesSpec = new LicencesWithSpecificService(request.LicenseId ?? 0, request.ServiceId ?? 0);
                var licences = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);
                licences.LicStatusId = (int)licencesStatusEnum.Ending;

                await _unitOfwork.genericRepository<Licence>().Update(licences);
            }
        }
        public async Task HandleManagerChangeAsync(MoiEserviceLicensesRequest request, MoiEserviceSysUser employee, UpdatedRequestVM updatedRequestVM)
        {
            var managertrans = await _unitOfwork.genericRepository<TchangeManager>().GetByIdObject(c => c.RequestId == request.RequestId);
            var personmanager = await _unitOfwork.genericRepository<Person>().GetByIdObject(p => p.CivilId == managertrans.ManagerNewcivilid);

            var licencesSpec = new LicencesWithSpecificService(request.LicenseId ?? 0, request.ServiceId ?? 0);
            var licences = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);

            if (updatedRequestVM.requestStatusValue == "final" || updatedRequestVM.Flag == "final")
            {
               
                licences.ManagerId = request.ManagerId;
                licences.ManagerCivilId = request.ManCivilId;
                await _unitOfwork.genericRepository<Licence>().Update(licences);
            }
        }
        public async Task HandleAddressChangeAsync(long requestId, UpdatedRequestVM updatedRequestVM)
        {
            var addresstrans = await _unitOfwork.genericRepository<AddressChangeTransaction>().GetByIdObject(c => c.RequestId == requestId);
            var address = await _unitOfwork.genericRepository<Address>().GetByIdObject(a => a.Id == addresstrans.AddId);

            if (updatedRequestVM.requestStatusValue == "final" || updatedRequestVM.Flag == "final")
            {
                address.Area = addresstrans.NewArea;
                address.AreaSize = addresstrans.AreaSizeNew;
                address.AalliNo = addresstrans.AalliNoNew;
                address.AreaChartNo = addresstrans.AreaChartNoNew;
                address.BlockArabic = addresstrans.NewBlock;
                address.BuildingName = addresstrans.NewBuildingName;
                address.StreetArabic = addresstrans.NewStreet;
                address.GovernorateArabic = addresstrans.NewGovernorate;
                address.FloorNo = addresstrans.NewFloor;
                await _unitOfwork.genericRepository<Address>().Update(address);
            }
        }
        public async Task HandleLicenseNameChangeAsync(MoiEserviceLicensesRequest request, UpdatedRequestVM updatedRequestVM)
        {
            var LicencesNametrans = await _unitOfwork.genericRepository<LicencesNameChangeTransaction>().GetByIdObject(c => c.RequestId == request.RequestId);
            var licencesSpec = new LicencesWithSpecificService(request.LicenseId ?? 0, request.ServiceId ?? 0);
            var licences = await _unitOfwork.genericRepository<Licence>().GetByIdWithSpec(licencesSpec);

            if (updatedRequestVM.requestStatusValue == "final" || updatedRequestVM.Flag == "final")
            {
                licences.LicName = LicencesNametrans.LicencesNameNew;
                await _unitOfwork.genericRepository<Licence>().Update(licences);
            }
        }
        public async Task HandleCompanyNameChangeAsync(long requestId, MoiEserviceSysUser employee, UpdatedRequestVM updatedRequestVM)
        {
            var companytrans = await _unitOfwork.genericRepository<CompanyNameChangeTransaction>().GetByIdObject(c => c.RequestId == requestId);
            companytrans.LastUpdateDate = DateTime.UtcNow;
            companytrans.LastUpdateUser = employee.Name;

            if (updatedRequestVM.requestStatusValue == "final" || updatedRequestVM.Flag == "final")
            {
                var company = await _unitOfwork.genericRepository<Company>().GetByIdObject(c => c.Id == companytrans.CompId);
                company.DirCompanyAr = companytrans.NewCompanyNameDir;
                await _unitOfwork.genericRepository<Company>().Update(company);
            }
        }
        public async Task HandleEmailChangeAsync(MoiEserviceLicensesRequest request, MoiEserviceSysUser employee, UpdatedRequestVM updatedRequestVM)
        {
            var Emailtrans = await _unitOfwork.genericRepository<ChangeEmailTranaction>().GetByIdObject(c => c.RequestId == request.RequestId);
            
            Emailtrans.LastUpdateUser= employee.Name;
            if (updatedRequestVM.requestStatusValue == "final" ||updatedRequestVM.Flag == "final")
            {
                
                var personApplicant = await _unitOfwork.genericRepository<Person>().GetByIdObject(p => p.CivilId == request.AppCivilId);
                var personManager = await _unitOfwork.genericRepository<Person>().GetByIdObject(p => p.CivilId == request.ManCivilId);


                personApplicant.Email = Emailtrans.NewOwnerEmail;

                personManager.Email = Emailtrans.NewmanagerEmail;
                await _unitOfwork.genericRepository<Person>().Update(personManager);
                await _unitOfwork.genericRepository<Person>().Update(personApplicant);

            }
        }
        public async Task HandlePartnerChangeAsync(MoiEserviceLicensesRequest request, MoiEserviceSysUser employee, UpdatedRequestVM updatedRequestVM)
        {
            var partnerspec = new PartnerWithSpec(request.LicenseId??0, request.ServiceId ?? 0);
            var Partnertrans = await _unitOfwork.genericRepository<Partner>().GetTableWithSpec(partnerspec);
            var changepartnerSpec = new PartnerChangeTransWithSpec(request.ServiceId ?? 0, request.RequestId);
            var ChangePartner=await _unitOfwork.genericRepository<PartnerOldChangeTransaction>().GetTableWithSpec(changepartnerSpec);
            var changepartnerNewSpec = new PartnerChangeTransNewWithSpec(request.ServiceId ?? 0, request.RequestId);
            var ChangePartnerNew = await _unitOfwork.genericRepository<PartnerNewChangeTransaction>().GetTableWithSpec(changepartnerNewSpec);
            if (updatedRequestVM.requestStatusValue == "final" || updatedRequestVM.Flag == "final")
            {
                foreach (var item in Partnertrans)
                {
                   await _unitOfwork.genericRepository<Partner>().Delete(item);
                }

                // Add new ChangePartner record(s)
                foreach (var change in ChangePartnerNew)
                {
                    var partner = new Partner()
                    {
                        LastUpdateDate = DateTime.Now,
                        ServiceId=request.ServiceId,
                        LastUpdateUser=employee.Username,
                        LicenseId=request.LicenseId,
                        Name= change.NewPartner,
                        
                    };
                  await  _unitOfwork.genericRepository<Partner>().Create(partner);
                }

            }
        }
       
        public async Task HandleSocialMediaNameChangeAsync(MoiEserviceLicensesRequest request, MoiEserviceSysUser employee, UpdatedRequestVM updatedRequestVM)
        {
            var socialtransspec = new SocialMediaChangeTransWithSpec(request.RequestId);
            var socialtrans=await _unitOfwork.genericRepository<ChangeSocialMediaTransaction>().GetByIdWithSpec(socialtransspec);
            var socialmediaspec =new SocialWithSpec(request.LicenseId??0,socialtrans.SocialMediaType??0); 
            var socialmedia=await _unitOfwork.genericRepository<MoiSocialMedia>().GetByIdWithSpec(socialmediaspec);

            socialmedia.AccountSocial = socialtrans.NewAccountSocial_Media;
                await _unitOfwork.genericRepository<MoiSocialMedia>().Update(socialmedia);
                
            
        }
        public async Task<ErrorMessage> InsertUpdateAttachementToTable(UpdatedAttachVM model, int serviceId)
        {
            var old = await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>()
                .GetByCondition(x => x.AttachId == model.AttachId /*&& x.IsLatest == true*/)
                .FirstOrDefaultAsync();

            if (old != null)
            {
                old.IsLatest = false;
                old.ReplacedByAttachId = null;
                old.IsApproved = true;
                await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>().Update(old);
            }
           
                var newAttach = new MoiEserviceRequestsAttach
                {
                    AttachName = model.FileSaveResponseVM.LabelName,
                    AttachRequestid = model.RequestId,
                    AttachStatus = "Pending",
                    AttachType = "Main",
                    AttachPath = model.FileSaveResponseVM.FilePath,
                    IsApproved = true,
                    IsMandatory = model.FileSaveResponseVM.IsRequired,
                    ServiceId = serviceId,
                    AttachFlag = model.FileSaveResponseVM.FileName,
                    IsLatest = true,
                    UploadedDate = DateTime.Now,
                    ReplacedByAttachId = old?.AttachId
                };

                await _unitOfwork.genericRepository<MoiEserviceRequestsAttach>().Create(newAttach);
                await _unitOfwork.Complete();
            
            return new ErrorMessage
            {
                Error = false,
                Status = "Success",
                Message = "Inserted successfully"
            };
        }
    }
}
