using Business.ViewModel.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Account
{
    public class ContactUsVM
    {
      
            public int Id { get; set; }
            public string? FullNameAr { get; set; }
            public string? FullNameEn { get; set; }
            public string? Email { get; set; }
            public string? Mobile { get; set; }
            public string? Message { get; set; }
        public int? ProcessedBy { get; set; }
        public string? ProcessedByName { get; set; }
        public string? Note { get; set; }
            public Nullable<DateTime> CreatedOn { get; set; }
            public Nullable<bool> IsDeleted { get; set; }
           public bool IsReplayed { get; set; }
        public bool Status { get; set; }


    }
    public class ContactReplyVM
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public int? ProcessedBy { get; set; }
        public string? ProcessedByName { get; set; }
        public string FullNameAr { get; set; }
        public string Note { get; set; }
    }

    public class ContactUsPageViewModel
    {
        public ContactUsVM? contactUsVM { get; set; }
        public List<SystemOptionVM>? SystemOptionVM { get; set; }
    }

}
