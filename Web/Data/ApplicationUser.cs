using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Web.Data
{
	// Add profile data for application users by adding properties to the ApplicationUser class
	public class ApplicationUser : IdentityUser
	{
		public List<UserRole> UserRoles { get; set; }

		public string Name { get; set; }
	}
}