using Fitness_Membership_Tracker.Data.DataModels;
using Microsoft.AspNetCore.Identity;

namespace Fitness_Membership_Tracker.Data.Seeding
{
    public static class RoleAndAdminSeeder
    {
        public static async Task SeedAsync(
            UserManager<Member> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Ensure all application roles exist
            string[] roles = { "Admin", "Member", "Employee", "Trainer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // Seed the admin user
            const string adminEmail    = "admin@fitzone.bg";
            const string adminPassword = "Admin123!";

            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new Member
                {
                    UserName       = adminEmail,
                    Email          = adminEmail,
                    EmailConfirmed = true,
                    IsDeleted      = false
                };

                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}
