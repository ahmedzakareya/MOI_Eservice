using Business.ViewModel;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUpdateDataService
    {
        Task HandleAttachmentsAsync(UpdatedRequestVM updatedRequestVM, MoiEserviceLicensesRequest request, MoiEserviceSysUser employee,int serviceId);
        Task<MoiEservicesRequestTransaction> SaveRequestTransaction(MoiEserviceLicensesRequest request, string action, string statusName, UpdatedRequestVM updatedRequestVM, MoiEserviceSysUser employee,int serviceId);
        Task<MoiEserviceSysUsersActivityLog> SaveUserLog(MoiEserviceSysUser employee, string action, UpdatedRequestVM updatedRequestVM);
        Task<(bool, string, string)> IsFinalCycleStatusAsync(/*int ActivityTypeId,*/ int RequestTypeId, int serviceId, int Requeststatusid);
        Task HandleRenewal(UpdatedRequestVM updatedRequestVM, MoiEserviceLicensesRequest request, DateTime? ExpireDate, DateTime? IssueDate, MoiEserviceSysUser employee);
        Task HandleRenouncement(UpdatedRequestVM updatedRequestVM, MoiEserviceLicensesRequest request);
        Task HandleEndingLicenses(UpdatedRequestVM updatedRequestVM, MoiEserviceLicensesRequest request);
        Task HandleCompanyNameChangeAsync(long requestId, MoiEserviceSysUser employee, UpdatedRequestVM updatedRequestVM);
        Task HandleAddressChangeAsync(long requestId, UpdatedRequestVM updatedRequestVM);
        Task HandleLicenseNameChangeAsync(MoiEserviceLicensesRequest request, UpdatedRequestVM updatedRequestVM);
        Task HandleManagerChangeAsync(MoiEserviceLicensesRequest request, MoiEserviceSysUser employee, UpdatedRequestVM updatedRequestVM);
        Task HandleSocialMediaNameChangeAsync(MoiEserviceLicensesRequest request, MoiEserviceSysUser employee, UpdatedRequestVM updatedRequestVM);
        Task HandlePartnerChangeAsync(MoiEserviceLicensesRequest request, MoiEserviceSysUser employee, UpdatedRequestVM updatedRequestVM);
        Task HandleEmailChangeAsync(MoiEserviceLicensesRequest request, MoiEserviceSysUser employee, UpdatedRequestVM updatedRequestVM);
        Task<ErrorMessage> InsertUpdateAttachementToTable(UpdatedAttachVM model, int serviceId);

    }
}
