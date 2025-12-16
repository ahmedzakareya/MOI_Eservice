using Business.Helpers;
using Business.ViewModel;
using Business.ViewModel.Dynamic;
using Microsoft.AspNetCore.Mvc;

namespace MOI_Eservice.Areas.Admin.Components
{
    public class AttachmentViewComponent:ViewComponent
    {
        private readonly string _baseUrl;
        private readonly HelperUrlApi _helperUrlApi;

        public AttachmentViewComponent(IConfiguration configuration,HelperUrlApi helperUrlApi)
        {
            _baseUrl = configuration["ApiSettings:BaseUrl"];
            _helperUrlApi = helperUrlApi;
        }
        public async Task<IViewComponentResult> InvokeAsync(string ReqNo, long RequestId, int serviceId, int activityTypeId, int RequestStatusId, int requestTypeId, int? transactionTypeId = null, string uploadController = "Tourism")
        {
            //activityTypeId ={ activityTypeId}
            //&
            var apiSettings = $"{_baseUrl}Dynamic/GetAttachmentRules?serviceId={serviceId}&requestStatusId={RequestStatusId}&requestTypeId={requestTypeId}&TransactionTypeId={transactionTypeId}";

            var attah =await  _helperUrlApi.GetDataFromApiNewHttpClient<List<AddAttachmentsRulesVM>>(apiSettings);
            var dynamicUrl = _baseUrl + $"Dynamic";
            ViewBag.DynamicUrlApi = dynamicUrl;
            // new to make the controller is dynamic
            ViewBag.UploadUrl = Url.Action("SaveFile", uploadController, new { area = "Admin" });
            ViewBag.ServiceId = serviceId;
            ViewBag.ReqNo = ReqNo;

            ViewBag.RequestId = RequestId;
            ViewBag.ActivityTypeId = activityTypeId;
            ViewBag.CurrentStatusId = RequestStatusId;
            ViewBag.RequestTypeId = requestTypeId;
            ViewBag.TransactionTypeId = transactionTypeId;
            ViewBag.AttachmentRules = attah;
            return View("Default");
        }
    }
}
