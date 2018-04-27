using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Web.Data
{
	public class ApplicationRole : IdentityRole
	{
		public ApplicationRole()
		{
		}

		public ApplicationRole(string name) : base(name)
		{
		}

		public List<UserRole> RoleUsers { get; set; }
	}
}