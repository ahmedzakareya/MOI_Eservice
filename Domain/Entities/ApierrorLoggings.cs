using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ApierrorLoggings
    {
        public long Id { get; set; }
        public string Type { get; set; } = null!;
        public string? Message { get; set; }
        public string? Details { get; set; }
        public string? StackTrace { get; set; }
        public string? Source { get; set; }
        public string? UserName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
