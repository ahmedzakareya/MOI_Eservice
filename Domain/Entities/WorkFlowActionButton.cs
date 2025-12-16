using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WorkFlowActionButton
    {
        public int Id { get; set; }

        public int? WorkFlowId { get; set; }

        public string? ButtonText { get; set; }

       
        public string? PermissionKey { get; set; }

        
        public string? ActionKey { get; set; }

        public bool IsDefault { get; set; } = false;

        [ForeignKey("WorkFlowId")]
        public WorkFlow? WorkFlow { get; set; }

        public ICollection<WorkFlowButtonRoleAdmin>? WorkFlowButtonRoleAdmins { get; set; }
    }
}
