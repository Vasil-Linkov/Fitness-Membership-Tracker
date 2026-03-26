using Fitness_Membership_Tracker.Data.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Data.Seeding
{
    public static class MembershipPaymentSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            UserManager<Member> userManager)
        {
            // Only run if no memberships exist yet
            if (await context.Memberships.IgnoreQueryFilters().AnyAsync())
                return;

            var tiers     = await context.MembershipTiers.ToListAsync();
            var locations = await context.Locations.IgnoreQueryFilters().ToListAsync();
            var employees = await context.Employees.IgnoreQueryFilters().ToListAsync();

            if (!tiers.Any() || !locations.Any() || !employees.Any())
                return;

            var members = await context.Members.IgnoreQueryFilters()
                              .Where(m => m.Email != "admin@fitzone.bg")
                              .ToListAsync();

            if (!members.Any()) return;

            var rnd = new Random(99);

            // Distribution of how long a member has had their membership
            // so we get a realistic mix of active / recently joined / long-term
            var membershipScenarios = new[]
            {
                // (monthsAgo, isActive, tierIndex)
                (6,  true,  2),  // Elite,    6 months, active
                (2,  true,  0),  // Basic,    2 months, active
                (12, true,  3),  // Ultimate, 1 year,   active
                (1,  true,  1),  // Advanced, 1 month,  active
                (3,  true,  1),  // Advanced, 3 months, active
                (9,  true,  2),  // Elite,    9 months, active
                (4,  true,  0),  // Basic,    4 months, active
                (18, true,  3),  // Ultimate, 1.5 year, active
                (2,  true,  1),  // Advanced, 2 months, active
                (5,  true,  2),  // Elite,    5 months, active
                (7,  true,  0),  // Basic,    7 months, active
                (3,  true,  3),  // Ultimate, 3 months, active
                (1,  true,  1),  // Advanced, 1 month,  active
                (10, true,  2),  // Elite,    10 months, active
                (8,  true,  0),  // Basic,    8 months, active
                (4,  true,  1),  // Advanced, 4 months, active
                (2,  true,  3),  // Ultimate, 2 months, active
                (6,  true,  2),  // Elite,    6 months, active
                (3,  true,  0),  // Basic,    3 months, active
                (14, true,  1),  // Advanced, 14 months, active
                // Remaining members have no membership — realistic "never joined" pool
            };

            for (int i = 0; i < members.Count; i++)
            {
                var member = members[i];

                // Members beyond the scenario list have no membership
                if (i >= membershipScenarios.Length)
                    break;

                var (monthsAgo, isActive, tierIdx) = membershipScenarios[i];
                var tier     = tiers[tierIdx % tiers.Count];
                var location = locations[rnd.Next(locations.Count)];
                var employee = employees.Where(e => e.LocationId == location.Id).ToList();
                var emp      = employee.Any() ? employee[rnd.Next(employee.Count)] : employees[rnd.Next(employees.Count)];

                // Build payment history: one payment per month the member has been active
                var payments = new List<Payment>();
                for (int month = monthsAgo; month >= 1; month--)
                {
                    var payDate = DateTime.UtcNow.AddMonths(-month).Date;
                    payments.Add(new Payment
                    {
                        Currency      = "EUR",
                        Amount        = tier.MonthlyPrice,
                        PaymentDate   = payDate,
                        PaymentMethod = month % 3 == 0 ? "Card" : "OnSite",
                        MemberId      = member.Id,
                        EmployeeId    = emp.Id
                        // MembershipId filled in after membership is saved
                    });
                }

                var startDate = DateTime.UtcNow.AddMonths(-monthsAgo).Date;
                var endDate   = startDate.AddMonths(monthsAgo + 1);

                var membership = new Membership
                {
                    MembershipTierId = tier.Id,
                    LocationId       = location.Id,
                    StartDate        = startDate,
                    EndDate          = endDate,
                    IsDeleted        = false
                };

                await context.Memberships.AddAsync(membership);
                await context.SaveChangesAsync();

                // Link membership to payments and to member
                foreach (var p in payments)
                    p.MembershipId = membership.Id;

                await context.Payments.AddRangeAsync(payments);

                // Update member FK — need tracked entity
                var trackedMember = await context.Members.FindAsync(member.Id);
                if (trackedMember != null)
                {
                    trackedMember.MembershipId = membership.Id;
                }

                await context.SaveChangesAsync();
            }
        }
    }
}
