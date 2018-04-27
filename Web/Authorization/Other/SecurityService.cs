using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Data;

namespace Web.Authorization.Other
{
	public partial class SecurityService : ISecurityService
	{

		private readonly ApplicationDbContext _dbContext;

		public SecurityService(ApplicationDbContext dbContext) => _dbContext = dbContext;

		public async Task<bool> HasAdmin(string userId)
		{
			var user = await GetUserWithRoles(userId);
			if (user == null)
			{
				throw new Exception($"{nameof(HasAdmin)}: User doesn't exist! UserId: {userId}");
			}

			return IsInAdminRole(user);
		}

		private Task<ApplicationUser> GetUserWithRoles(string userId) =>
			_dbContext.Users
				.Include(u => u.UserRoles)
				.ThenInclude(ur => ur.Role)
				.FirstOrDefaultAsync(e => e.Id == userId);

		private bool IsInAdminRole(ApplicationUser user) => IsInRole(user, UserRolesEnum.Admin);

		private bool IsInRole(ApplicationUser user, UserRolesEnum role) => user.UserRoles.Any(ur => ur.Role.Name == role.ToString());
	}
}