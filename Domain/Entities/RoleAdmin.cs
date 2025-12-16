using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RoleAdmin 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsAdmin { get; set; }
        [JsonIgnore]
        public virtual ICollection<RolePermissionAdmin> RolePermissions { get; set; } = new List<RolePermissionAdmin>();

        [JsonIgnore]
        public virtual ICollection<AspNetUserRoleAdmin> UserRoles { get; set; } = new List<AspNetUserRoleAdmin>();
    }
}
