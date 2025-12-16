using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Dynamic
{
    public class WorkFlowActionButtonVM
    {
        public int Id { get; set; }
        public int? WorkFlowId { get; set; }
        public string? ButtonText { get; set; }
        public string? ActionKey { get; set; }
        public string? PermissionKey { get; set; }
        public virtual WorkFlowVM? WorkFlow{get; set;}
        // Dropdowns
        public List<SelectListItem>? WorkFlows { get; set; } = new();
        public string? WorkFlowName { get; set; } // Optional display field
    }
}
