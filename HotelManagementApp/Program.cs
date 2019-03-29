using HotelManagementApp.DbInit;
using HotelManagementApp.Models;
using HotelManagementApp.Models.Context;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HotelManagementApp
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var host = CreateWebHostBuilder(args).Build();

			using (var scope = host.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					var userManager = services.GetRequiredService<UserManager<UserModel>>();
					var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
					await UserRoleInitializer.InitializeAsync(userManager, rolesManager);
					var dbContext = services.GetRequiredService<ApplicationDbContext>();
					await DbInitializer.InitializeAsync(dbContext);
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while seeding the database.");
				}
			}

			host.Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseWebRoot(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
				.UseStartup<Startup>();
	}
}
