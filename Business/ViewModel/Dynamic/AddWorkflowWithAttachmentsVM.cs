using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Dynamic
{
    public class AddWorkflowWithAttachmentsVM
    {
        public int Id { get; set; }
        public string? ServiceName { get; set; }
        public int? ServiceId { get; set; }
        public string? ActivityTypeName { get; set; }
        public int? ActivityTypeId { get; set; }
        public string? RequestTypeName { get; set; }
        public int? RequestTypeId { get; set; }
       public string? RequestStatusName { get; set; }
        public int? RequestStatusId { get; set; }
        public int? TransactionTypeId { get; set; }
        public string? TransactionTypeName { get; set; } 
        public bool IsMandatory { get; set; }
       public string? AttachName { get; set; }   // Label shown to user
        public string? FieldName { get; set; }          // For input name in frontend
        public int? MaxFileSize { get; set; }
        public string? Description { get; set; }

        public string? AllowedFileTypes { get; set; }   // Comma-separated

        public string? ViewTypeForAttach { get; set; }           // For differentiating pages
        public string? FlagView { get; set; }

        public List<IFormFile>? Attachments { get; set; }

        public IEnumerable<SelectListItem>? Services { get; set; }
        public IEnumerable<SelectListItem>? ActivityTypes { get; set; }
        public IEnumerable<SelectListItem>? RequestTypes { get; set; }
        public IEnumerable<SelectListItem>? RequestStatus { get; set; }
        public IEnumerable<SelectListItem>? TransactionTypes { get; set; }
    }
}
