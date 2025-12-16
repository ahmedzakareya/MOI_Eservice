using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ContactUs
    {

            public int Id { get; set; }
            public string? FullNameAr { get; set; }
            public string? FullNameEn { get; set; }
            public string? Email { get; set; }
            public string? Mobile { get; set; }
            public string? Message { get; set; }
            public string? Note { get; set; }
        public int? ProcessedBy { get; set; }
        public string? ProcessedByName { get; set; }
        public Nullable<DateTime> CreatedOn { get; set; }
            public Nullable<bool> IsDeleted { get; set; }
           public bool IsReplayed { get; set; }


    }
}
