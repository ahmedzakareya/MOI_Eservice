using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class FileUploadConfigurationsFront
    {
        public int Id { get; set; }           
        public string FieldName { get; set; }  
        public string Label { get; set; }     
        public int? MaxFileSize { get; set; }  
        public string AllowedFileTypes { get; set; } 
        public bool IsRequired { get; set; }    
        public string ViewType { get; set; }
        public int? ReqStatusId { get; set; }
        [ForeignKey("ReqStatusId")]
        public virtual RequestStatusLookup? RequestStatusLookup { get; set; }
    }
}
