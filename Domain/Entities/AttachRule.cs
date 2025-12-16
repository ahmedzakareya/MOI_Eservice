using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AttachRule
    {
        public int Id { get; set; } 
        public int? ServiceId { get; set; }  
        public int? ActivityTypeId { get; set; }
        public int? RequestTypeId {  get; set; } 
        public int? RequestStatusId {  get; set; }   
        public int? TransactionTypeId { get; set; }
        public string? AttachName {  get; set; } 
        public bool IsMandatory {  get; set; }
        public int? MaxFileSize { get; set; }   
        public string? Description { get; set; }
        public string? FieldName { get; set; }          // For input name in frontend
        //public string Label { get; set; }              // Label shown to user
     

        public string? AllowedFileTypes { get; set; }   // Comma-separated
     
        public string? ViewType { get; set; }           // For differentiating pages
        public string? FlagView { get; set; }

        [ForeignKey("TransactionTypeId")]
        public virtual TransactionTypesLookup? TransactionTypesLookup { get; set; }
       [ForeignKey("ActivityTypeId")]
        public virtual ActivityTypesLookup? ActivityTypesLookup { get; set; }
        [ForeignKey("RequestStatusId")]
        public virtual RequestStatusLookup RequestStatusLookup { get; set; }
       
        [ForeignKey("ServiceId")]
        public virtual Eservice Eservice { get; set; }
        [ForeignKey("RequestTypeId")]
        public virtual RequestsTypesLookup RequestsTypesLookup { get; set; }
    }
}
