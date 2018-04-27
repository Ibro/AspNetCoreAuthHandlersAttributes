using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Web.Data
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		public DbSet<Category> Categories { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<ApplicationUser>(user =>
			{
				user.ToTable(nameof(Users));
				user.HasMany(u => u.UserRoles)
					.WithOne(ur => ur.User)
					.HasForeignKey(ur => ur.UserId)
					.IsRequired();
			});

			builder.Entity<ApplicationRole>(role =>
			{
				role.ToTable(nameof(Roles));
				role.HasKey(r => r.Id);
				role.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();
				role.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

				role.Property(u => u.Name).HasMaxLength(256);
				role.Property(u => u.NormalizedName).HasMaxLength(256);

				role.HasMany<UserRole>()
					.WithOne(ur => ur.Role)
					.HasForeignKey(ur => ur.RoleId)
					.IsRequired();

				role.HasMany<IdentityRoleClaim<string>>()
					.WithOne()
					.HasForeignKey(rc => rc.RoleId)
					.IsRequired();
			});

			builder.Entity<IdentityRoleClaim<string>>(roleClaim =>
			{
				roleClaim.HasKey(rc => rc.Id);
				roleClaim.ToTable(nameof(RoleClaims));
			});

			builder.Entity<IdentityUserRole<string>>(userRole =>
			{
				userRole.ToTable(nameof(UserRoles));
				userRole.HasKey(r => new { r.UserId, r.RoleId });
			});

			builder.Entity<IdentityUserLogin<string>>().ToTable(nameof(UserLogins));
			builder.Entity<IdentityUserClaim<string>>().ToTable(nameof(UserClaims));
			builder.Entity<IdentityUserToken<string>>().ToTable(nameof(UserTokens));
		}
	}
}