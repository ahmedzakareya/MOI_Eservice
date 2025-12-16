using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class WorkFlowButtonRoleAdmin
    {

        public int Id { get; set; }

        public int? WorkFlowActionButtonId { get; set; }

   
        public int? RoleAdminId { get; set; }

        [ForeignKey("WorkFlowActionButtonId")]

        public WorkFlowActionButton? WorkFlowActionButton { get; set; }
        [ForeignKey("RoleAdminId")]
        public RoleAdmin? RoleAdmin { get; set; }  // if using ASP.NET Core Identity
    }
}
