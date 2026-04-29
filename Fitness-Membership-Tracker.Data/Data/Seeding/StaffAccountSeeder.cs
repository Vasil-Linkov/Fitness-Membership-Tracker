using Fitness_Membership_Tracker.Data.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Data.Seeding
{
    /// <summary>
    /// Creates an Identity (Member) account for every seeded Trainer and Employee
    /// that does not already have one, then assigns the correct role.
    ///
    /// Password for all staff accounts: Staff123!
    /// Trainers  log in with their trainer email  (e.g. maria.ivanova@fitzone.bg)
    /// Employees log in with their employee email (e.g. teodora.ivanova@fitzone.bg)
    /// </summary>
    public static class StaffAccountSeeder
    {
        private const string StaffPassword = "Staff123!";

        public static async Task SeedAsync(
            ApplicationDbContext    context,
            UserManager<Member>     userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Ensure roles exist (RoleAndAdminSeeder may have run first, but be safe)
            foreach (var role in new[] { "Trainer", "Employee" })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // ── Trainers ─────────────────────────────────────────────────────
            var trainers = await context.Trainers.IgnoreQueryFilters().ToListAsync();

            foreach (var trainer in trainers)
            {
                // Skip if an Identity account with this email already exists
                if (await userManager.FindByEmailAsync(trainer.Email) != null)
                    continue;

                var user = new Member
                {
                    UserName       = trainer.Email,
                    Email          = trainer.Email,
                    EmailConfirmed = true,
                    IsDeleted      = false
                };

                var result = await userManager.CreateAsync(user, StaffPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, "Trainer");
            }

            // ── Employees ────────────────────────────────────────────────────
            var employees = await context.Employees.IgnoreQueryFilters().ToListAsync();

            foreach (var employee in employees)
            {
                if (await userManager.FindByEmailAsync(employee.Email) != null)
                    continue;

                var user = new Member
                {
                    UserName       = employee.Email,
                    Email          = employee.Email,
                    EmailConfirmed = true,
                    IsDeleted      = false
                };

                var result = await userManager.CreateAsync(user, StaffPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, "Employee");
            }
        }
    }
}
