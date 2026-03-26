using Fitness_Membership_Tracker.Data.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fitness_Membership_Tracker.Data.Seeding
{
    /// <summary>
    /// Central orchestrator for all application data seeding.
    /// Called once at application startup; every individual seeder is idempotent
    /// (it checks whether data already exists before inserting).
    /// </summary>
    public static class DataSeeder
    {
        public static async Task SeedAllAsync(IServiceProvider serviceProvider)
        {
            var context     = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Member>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var logger      = serviceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

            try
            {
                logger.LogInformation("Starting database seeding...");

                // ── 1. Roles and admin account ───────────────────────────
                logger.LogInformation("Seeding roles and admin user...");
                await RoleAndAdminSeeder.SeedAsync(userManager, roleManager);

                // ── 2. Reference / lookup tables ────────────────────────
                logger.LogInformation("Seeding locations...");
                await LocationSeeder.SeedAsync(context);

                logger.LogInformation("Seeding membership tiers...");
                await MembershipTierSeeder.SeedAsync(context);

                // ── 3. Staff ─────────────────────────────────────────────
                logger.LogInformation("Seeding employees...");
                await EmployeeSeeder.SeedAsync(context);

                logger.LogInformation("Seeding trainers and schedules...");
                await TrainerSeeder.SeedAsync(context);

                // ── 4. Members (Identity users) ──────────────────────────
                logger.LogInformation("Seeding members...");
                await MemberSeeder.SeedAsync(userManager, roleManager);

                // ── 5. Memberships and payments ──────────────────────────
                logger.LogInformation("Seeding memberships and payment history...");
                await MembershipPaymentSeeder.SeedAsync(context, userManager);

                // ── 6. Activity data ─────────────────────────────────────
                logger.LogInformation("Seeding visit history...");
                await VisitSeeder.SeedAsync(context);

                logger.LogInformation("Seeding workout logs...");
                await WorkoutLogSeeder.SeedAsync(context);

                // ── 7. Trainer-member relationships ──────────────────────
                logger.LogInformation("Seeding trainer relationships and requests...");
                await TrainerRelationshipSeeder.SeedAsync(context);

                logger.LogInformation("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
    }
}
