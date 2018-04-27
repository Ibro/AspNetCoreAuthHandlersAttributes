using Microsoft.AspNetCore.Identity;

namespace Web.Data
{
	public class UserRole : IdentityUserRole<string>
	{
		public string Id { get; set; }

		public virtual ApplicationUser User { get; set; }
		public virtual ApplicationRole Role { get; set; }
	}
}