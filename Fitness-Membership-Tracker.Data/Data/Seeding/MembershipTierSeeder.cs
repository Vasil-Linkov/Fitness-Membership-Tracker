using Fitness_Membership_Tracker.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Data.Seeding
{
    public static class MembershipTierSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.MembershipTiers.AnyAsync())
                return;

            var tiers = new List<MembershipTier>
            {
                new MembershipTier
                {
                    Tier               = "Basic",
                    MonthlyPrice       = 29.99m,
                    Description        = "Perfect for beginners. Access to cardio equipment, free weights area, and locker rooms during staffed hours (06:00–22:00).",
                    Accessibility      = "address",
                    MaxSessionsPerMonth = 8
                },
                new MembershipTier
                {
                    Tier               = "Advanced",
                    MonthlyPrice       = 49.99m,
                    Description        = "Access to all equipment including cable machines, functional training zone, and group fitness classes. Valid at any branch in your city.",
                    Accessibility      = "city",
                    MaxSessionsPerMonth = 12
                },
                new MembershipTier
                {
                    Tier               = "Elite",
                    MonthlyPrice       = 79.99m,
                    Description        = "Full access 24/7 to all equipment, group classes, sauna, and recovery zone. Valid at any branch in Bulgaria. Includes one free PT session per month.",
                    Accessibility      = "country",
                    MaxSessionsPerMonth = 18
                },
                new MembershipTier
                {
                    Tier               = "Ultimate",
                    MonthlyPrice       = 119.99m,
                    Description        = "Unlimited access to every facility worldwide. Priority booking for classes and PT sessions. Includes nutrition consultation, body composition scans, and premium locker.",
                    Accessibility      = "any",
                    MaxSessionsPerMonth = 24
                }
            };

            await context.MembershipTiers.AddRangeAsync(tiers);
            await context.SaveChangesAsync();
        }
    }
}
