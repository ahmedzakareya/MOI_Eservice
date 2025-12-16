using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class EmployeeLogVM
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string? UserFullName { get; set; }

        public string? Section { get; set; }

        public string? Activity { get; set; }

        public DateTime? ActivityDate { get; set; }

        public int? ActivityItemId { get; set; }

        public string? ActivityItemName { get; set; }

        public string? Note { get; set; }
    }
}
