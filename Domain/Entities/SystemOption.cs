using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class SystemOption
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string? NameArabic { get; set; }
        public string? NameEnglish { get; set; }
        public string? CategoryNameAr { get; set; }
        public string? CategoryNameEn { get; set; }
        public string? ControlType { get; set; }
        public string? DropdownOptions { get; set; }
        public string? DefaultValue { get; set; }
        public string? Value { get; set; }
        public bool? IsReadOnly { get; set; }
        public bool? IsHidden { get; set; }
        public int? DisplayOrder { get; set; }
    }
}
