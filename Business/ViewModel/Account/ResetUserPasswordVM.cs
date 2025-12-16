using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Account
{
    public class ResetUserPasswordVM
    {
        public int Id { get; set; }

        public string? UserCivilID { get; set; }

        public string? UserEmail { get; set; }

        public string? Mobile { get; set; }
        public string? Note { get; set; }


        public string? UserNewPass { get; set; }

        public string? FilePath { get; set; } // This stores the uploaded file path instead of base64

        public DateTime? DateAdded { get; set; }

        public bool? Executed { get; set; }
        public bool Status { get; set; }
        public int? ProcessedBy { get; set; }
        public string? ProcessedByName { get; set; }
        public DateTime? ExecutedOn { get; set; }
    }
}
