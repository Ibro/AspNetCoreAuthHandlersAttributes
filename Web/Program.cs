using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Web.Data;

namespace Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var webHost = CreateWebHostBuilder(args).Build();

			using (var scope = webHost.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var context = services.GetRequiredService<ApplicationDbContext>();
					var dbInitializer = services.GetRequiredService<IDbInitializer>();
					dbInitializer.Seed(context).GetAwaiter().GetResult();
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while seeding the database.");
				}
			}

			webHost.Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}
