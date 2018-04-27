using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Web.Data
{
	public interface IDbInitializer
	{
		Task Seed(ApplicationDbContext context);
	}

	public class DbInitializer : IDbInitializer
	{
		private readonly RoleManager<ApplicationRole> _roleManager;
		private readonly ApplicationDbContext _dbContext;
		private readonly UserManager<ApplicationUser> _userManager;

		public DbInitializer(
			ApplicationDbContext dbContext,
			UserManager<ApplicationUser> userManager,
			RoleManager<ApplicationRole> roleManager
		)
		{
			_dbContext = dbContext;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task Seed(ApplicationDbContext context)
		{
			context.Database.EnsureCreated();

			// Look for any students.
			if (context.Users.Any())
			{
				return;   // DB has been seeded
			}

			string adminRoleName = "Administrator";
			string userRoleName = "User";

			await _roleManager.CreateAsync(new ApplicationRole(adminRoleName));
			await _roleManager.CreateAsync(new ApplicationRole(userRoleName));

			var admin = await CreateUser("admin@codingblast.com", "CodingBlast_2018", adminRoleName, "1980-08-08");
			var user = await CreateUser("user@codingblast.com", "CodingBlast_2018", userRoleName, "1980-08-08");

			var category1 = new Category
			{
				Name = "First category, Admins",
				User = admin
			};

			var category2 = new Category
			{
				Name = "Second category, User",
				User = user
			};

			await _dbContext.Categories.AddAsync(category1);
			await _dbContext.Categories.AddAsync(category2);

			await _dbContext.SaveChangesAsync();
		}

		private async Task<ApplicationUser> CreateUser(string email, string password, string role, string dateOfBirth)
		{
			var user = new ApplicationUser
			{
				Email = email,
				UserName = email,
				Name = email,
			};

			var result = await _userManager.CreateAsync(user, password);

			// Assigns role to the user
			await _userManager.AddToRoleAsync(user, role);

			// Assigns claims to the user
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.DateOfBirth, dateOfBirth)
			};

			await _userManager.AddClaimsAsync(user, claims);

			return user;
		}
	}
}