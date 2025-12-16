using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class UpdatedRequestVM
    {
        public int RequestId { get; set; }
        public int StatusId { get; set; }
        public int? ServiceId { get; set; }

        public string? FilePath { get; set; }
        public long? SequenceNo { get; set; }
        public string? PreApprovalNo { get; set; }
        public bool? showUploadFinalLicenseButtonForTransaction {  get; set; }    
        public string? FileName { get; set; }
        public List<FileSaveResponseVM>? saveResponseVMs { get; set; }
        public string? LicNo { get; set; }
        public int ReqTypeId { get; set; }
        public int? TransTypeId { get; set; }
        public string? ReqNotes { get; set; }
        public string? Note {  get; set; }
        public int LicStatusId {  get; set; } 
        public string? NameUser {  get; set; }
        public int? UserId {  get; set; }   
        public string? ActionName { get; set; }
        public string? Flag { get; set; }
        public string? FlagCondition { get; set; }
        public int? ClassificationId { get; set; }
        public int? TransId { get; set; }
        public string? requestTypeValue { get; set; }
        public string? requestStatusValue { get; set; }
        public List<int>? HotelClassIds { get; set; }
        public List<int>? EvaluationIds { get; set; }
        public List<string>? Values { get; set; }
        public List<HotelClassEvaluationSelection>? HotelClassEvaluations { get; set; }
        public string? Action { get; set; }
        public List<int>? selectedAttachments { get; set; }
        public List<int>? SelectedHotelClassIds { get; set; }
        public List<int>? SelectedEvaluationIds { get; set; }
        public List<int>? uncheckedAttachmentsFromDOM { get; set; }  // To handle unchecked checkboxes
        public List<string>? ChangeLogs { get; set; }  // List of change log entries
        public List<int>? uncheckedAttachments { get; set; }
        public List<AttachmentState>? AttachmentStates { get; set; }
       


    }



}
