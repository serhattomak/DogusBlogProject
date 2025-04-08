using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DogusProject.Infrastructure.Identity;

public static class IdentitySeeder
{
	public static async Task SeedDefaultRolesAsync(IServiceProvider serviceProvider)
	{
		using var scope = serviceProvider.CreateScope();
		var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
		var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

		string[] roles = ["Admin", "Author"];

		foreach (var role in roles)
		{
			if (!await roleManager.RoleExistsAsync(role))
			{
				await roleManager.CreateAsync(new AppRole { Name = role });
			}
		}

		var adminEmail = "admin@blog.com";
		var adminUser = await userManager.FindByEmailAsync(adminEmail);
		if (adminUser == null)
		{
			var user = new AppUser
			{
				UserName = "admin",
				Email = adminEmail,
				EmailConfirmed = true
			};

			var result = await userManager.CreateAsync(user, "Admin123*");

			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(user, "Admin");
			}
		}
	}
}