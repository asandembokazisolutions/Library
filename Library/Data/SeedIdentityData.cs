using Library.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Library.Data
{
    public static class SeedIdentityData
    {
        private const string adminUserName = "admin";
        private const string adminPassword = "Admin@123";
        private const string adminEmail = "admin@library.com";
        private const string adminRole = "Admin";
        private const string memberRole = "Member";

        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            AppIdentityDbContext context = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<AppIdentityDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            UserManager<AppUser> userManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<UserManager<AppUser>>();

            RoleManager<IdentityRole> roleManager = app.ApplicationServices
                .CreateScope().ServiceProvider
                .GetRequiredService<RoleManager<IdentityRole>>();

            // Seed roles
            if (await roleManager.FindByNameAsync(adminRole) == null)
                await roleManager.CreateAsync(new IdentityRole(adminRole));

            if (await roleManager.FindByNameAsync(memberRole) == null)
                await roleManager.CreateAsync(new IdentityRole(memberRole));

            // Seed admin user
            if (await userManager.FindByNameAsync(adminUserName) == null)
            {
                AppUser admin = new AppUser
                {
                    UserName = adminUserName,
                    Email = adminEmail
                };

                IdentityResult result = await userManager.CreateAsync(admin, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, adminRole);
                }
            }
        }
    }
}
