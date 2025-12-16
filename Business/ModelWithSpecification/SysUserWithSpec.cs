using Business.Repository;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ModelWithSpecification
{
	public class SysUserWithSpec:Specification<MoiEserviceSysUser>
	{
		public SysUserWithSpec(string UserName,string UserPasswordEncrypted,bool? Status):base(x=>x.Username==UserName&&x.UserPasswordEncrypted==UserPasswordEncrypted&&x.Status==Status) 
		{
            

        }
        public SysUserWithSpec():base()
        {
		
            Includes.Add(s => s.AspNetUserRoles);
            //Includes.Add(s=>s.Roles);   

        }
        public SysUserWithSpec(int id):base(s=>s.SysUserId == id)
        {
	
			Includes.Add(s => s.AspNetUserRoles);
            //Includes.Add(s => s.Roles);

        }
        public SysUserWithSpec(string civilid):base(s=>s.CivilId==civilid)
        {
                
        }
    }
}
