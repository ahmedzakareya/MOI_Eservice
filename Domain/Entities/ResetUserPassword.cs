using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ResetUserPassword
    {
        public int Id { get; set; }

        public string? UserCivilID { get; set; }

        public string? UserEmail { get; set; }

        public string? Mobile { get; set; }

        public string? UserNewPass { get; set; }

        public string? FilePath { get; set; }
        public string? Note { get; set; }


        public DateTime? DateAdded { get; set; }

        public bool? Executed { get; set; }
        public bool Status { get; set; }

        public int? ProcessedBy { get; set; }
        public string? ProcessedByName { get; set; }
        public DateTime? ExecutedOn { get; set; }
    }
}
