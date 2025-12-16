using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class FileSaveResponseVM
    {
        public bool? Success { get; set; }
        public string? FileName { get; set; }
   
        public string? Flag { get; set; }
        public string? LabelName { get; set; }
        public bool? IsRequired { get; set; }
        public string? FilePath { get; set; }
        public string? FieldName { get; set; }
        public string? Message { get; set; } // For success or error messages
        public string? Error { get; set; }
    }
}
