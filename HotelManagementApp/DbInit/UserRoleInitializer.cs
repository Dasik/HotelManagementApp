using HotelManagementApp.Models;
using HotelManagementApp.Utils;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace HotelManagementApp.DbInit
{
	public class UserRoleInitializer
	{
		public static async Task InitializeAsync(UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
		{
			if (await roleManager.FindByNameAsync(Constants.Roles.Admin) == null)
			{
				await roleManager.CreateAsync(new IdentityRole(Constants.Roles.Admin));
			}
			if (await roleManager.FindByNameAsync(Constants.Roles.User) == null)
			{
				await roleManager.CreateAsync(new IdentityRole(Constants.Roles.User));
			}
			if (await roleManager.FindByNameAsync(Constants.Roles.Manager) == null)
			{
				await roleManager.CreateAsync(new IdentityRole(Constants.Roles.Manager));
			}

			string adminEmail = "admin@mail.com";
			string password = "qQ!2345678";
			if (await userManager.FindByNameAsync(adminEmail) == null)
			{
				UserModel admin = new UserModel { Email = adminEmail, UserName = adminEmail };
				IdentityResult result = await userManager.CreateAsync(admin, password);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(admin, Constants.Roles.Admin);
				}
			}

			string managerEmail = "manager@mail.com";
			if (await userManager.FindByNameAsync(managerEmail) == null)
			{
				UserModel manager = new UserModel { Email = managerEmail, UserName = managerEmail };
				IdentityResult result = await userManager.CreateAsync(manager, password);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(manager, Constants.Roles.Manager);
				}
			}

			string userEmail = "user@mail.com";
			if (await userManager.FindByNameAsync(userEmail) == null)
			{
				UserModel user = new UserModel { Email = userEmail, UserName = userEmail };
				IdentityResult result = await userManager.CreateAsync(user, password);
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, Constants.Roles.User);
				}
			}
		}
	}
}