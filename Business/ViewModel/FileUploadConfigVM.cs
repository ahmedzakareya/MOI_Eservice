using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class FileUploadConfigVM
    {
        public int Id { get; set; }
        public string FieldName { get; set; }
        public string Label { get; set; }
        public int? MaxFileSize { get; set; }
        public string AllowedFileTypes { get; set; }
        public bool IsRequired { get; set; }
        public string ViewType { get; set; }
    }
}
