using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public partial class ScheduleReleaseTypes
    {
        public int Id { get; set; }
        public string? ReleaseTypeName { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }
}
