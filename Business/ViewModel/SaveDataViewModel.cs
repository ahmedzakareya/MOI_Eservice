using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class SaveDataViewModel
    {
        public int RequestId { get; set; }
        public int ReqStatusId { get; set; }
        public int ReqTypeId { get; set; }
        public int ActivityTypeId { get; set; }
        public string? ReqNo { get; set; }
        //public IFormFile? Files { get; set; }
        //public string? filename { get; set; }
        public string? Note { get; set; }
        public List<files>? files { get; set; }


        public string? Action { get; set; }
        public List<int>? selectedAttachments { get; set; }
       
        
        public List<int>? uncheckedAttachmentsFromDOM { get; set; } 
        public List<string>? ChangeLogs { get; set; }  // List of change log entries
        //public List<files>? files { get; set; }
        //public IFormFile? Files { get; set; } // ملفات مرفقة

       
        public List<int>? uncheckedAttachments { get; set; }
        public List<AttachmentState>? allAttachmentsState { get; set; }

       
    }
    public class SaveDataClassificationViewModel
    {
        public int RequestId { get; set; }
        public int ReqStatusId { get; set; }
        public int ReqTypeId { get; set; }
        public int ActivityTypeId { get; set; }
        public string? ReqNo { get; set; }
        //public IFormFile? PdfFile { get; set; }
        public List<files>? files { get; set; }

        public string? FileName { get; set; }
        public string? Note { get; set; }


        public string? Action { get; set; }
        public List<int>? selectedAttachments { get; set; }
        public List<int>? SelectedHotelClassIds { get; set; }
        public List<int>? SelectedEvaluationIds { get; set; }
        public List<HotelClassEvaluationSelection>? HotelClassEvaluations { get; set; }  // List of selections
        public List<int>? uncheckedAttachmentsFromDOM { get; set; }  // To handle unchecked checkboxes
        public List<string>? ChangeLogs { get; set; }  // List of change log entries
        //public List<files>? files { get; set; }
        //public IFormFile? Files { get; set; } // ملفات مرفقة

        public List<int>? HotelClassIds { get; set; }
        public List<int>? EvaluationIds { get; set; }
        public List<string>? Values { get; set; }
        public List<int>? uncheckedAttachments { get; set; }
        public List<AttachmentState>? AttachmentStates { get; set; }
        public int? ClassificationId { get; set; }

    }
    public class SaveDataViewModelPreApprove
    {
        public int RequestId { get; set; }
        public int ReqStatusId { get; set; }
        public int ReqTypeId { get; set; }
        public string? ReqNo { get; set; }

        public int ActivityTypeId { get; set; }
        public string? Note { get; set; }
        public string? Action { get; set; }
       // public IFormFile Files { get; set; } // ملفات مرفقة
       // public string filname { get; set; }
        public List<files>? files { get; set; }
        public List<int>? SelectedAttachments { get; set; }
        public List<int>? UncheckedAttachments { get; set; }
        public List<string>? ChangeLogs { get; set; }
        public List<AttachmentState>? allAttachmentsState { get; set; }

    }

    public class SaveDataViewModelTransactonType
    {
        public int RequestId { get; set; }
        public int ReqStatusId { get; set; }
        public int ReqTypeId { get; set; }
        public int ActivityTypeId { get; set; }
        public string? ReqNo { get; set; }
        public List<files>? files { get; set; }
        //public string? filename { get; set; }
        public string? Note { get; set; }
        public int TransactionId { get; set; }
        public int transTypeId { get; set; }
        public string? Action { get; set; }
        public List<int>? selectedAttachments { get; set; }


        public List<int>? uncheckedAttachmentsFromDOM { get; set; }  // To handle unchecked checkboxes
        public List<string>? ChangeLogs { get; set; }  // List of change log entries
                                                       //public List<files>? files { get; set; }
                                                       //public IFormFile? Files { get; set; } // ملفات مرفقة
        public List<int>? uncheckedAttachments { get; set; }
        public List<AttachmentState>? AttachmentStates { get; set; }

                  

                 
    }
    public class AttachmentState
    {
        public int AttachmentId { get; set; }  // معرف المرفق
        public string State { get; set; }      // حالة المرفق (checked أو unchecked)
    }

    public class files
    {
        public IFormFile Files { get; set; }
        public string filename { get; set; }
        public bool ismandatory { get; set; }
        public string fieldname { get; set; }
    }

    public class fileAttach
    {
        public List<files>? files { get; set; }
        public int? RequestId { get; set; }
        public string? ReqNo { get; set; }


    }
    public class HotelClassEvaluationSelection
    {
        public int? ClassificationId { get; set; }

        public int HotelClassId { get; set; }
        public int EvaluationId { get; set; }
        public bool IsSelected { get; set; }  // Whether the evaluation was selected (true/false)
    }
}
