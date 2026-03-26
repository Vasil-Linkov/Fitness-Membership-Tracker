using Fitness_Membership_Tracker.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Data.Seeding
{
    public static class VisitSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Visits.IgnoreQueryFilters().AnyAsync())
                return;

            var members   = await context.Members.IgnoreQueryFilters()
                                .Where(m => m.MembershipId != null && m.Email != "admin@fitzone.bg")
                                .Include(m => m.Membership)
                                .ToListAsync();

            var locations = await context.Locations.IgnoreQueryFilters().ToListAsync();

            if (!members.Any() || !locations.Any())
                return;

            var rnd    = new Random(77);
            var visits = new List<Visit>();
            var today  = DateTime.UtcNow.Date;

            // Typical visit frequencies by member tier index (rough sessions / week)
            // Basic=1–2, Advanced=2–3, Elite=3–4, Ultimate=4–5
            var tierFrequency = new Dictionary<int, int[]>
            {
                { 0, new[] { 1, 2 } }, // Basic
                { 1, new[] { 2, 3 } }, // Advanced
                { 2, new[] { 3, 4 } }, // Elite
                { 3, new[] { 4, 5 } }, // Ultimate
            };

            var tierIds  = await context.MembershipTiers.Select(t => t.Id).ToListAsync();

            foreach (var member in members)
            {
                var membership = member.Membership;
                if (membership == null) continue;

                // Find tier index (0-3) based on price ordering
                var tierIdx = tierIds.IndexOf(membership.MembershipTierId);
                if (tierIdx < 0) tierIdx = 0;

                var freqRange = tierFrequency.ContainsKey(tierIdx)
                    ? tierFrequency[tierIdx]
                    : new[] { 1, 2 };

                // Seed visits for last 90 days, capped to membership start
                var seedStart = today.AddDays(-90);
                if (membership.StartDate > seedStart)
                    seedStart = membership.StartDate.Date;

                for (var day = seedStart; day <= today; day = day.AddDays(1))
                {
                    // Skip Sunday (~rest day probability)
                    if (day.DayOfWeek == DayOfWeek.Sunday && rnd.Next(100) < 80)
                        continue;

                    int sessionsThisDay = rnd.Next(0, 8) < freqRange[1] ? 1 : 0;
                    if (sessionsThisDay == 0) continue;

                    // Members with higher tiers are more likely to visit
                    if (rnd.Next(7) >= freqRange[1])
                        continue;

                    // Choose a nearby location — members mostly use the membership location
                    var loc = rnd.Next(100) < 75
                        ? locations.FirstOrDefault(l => l.Id == membership.LocationId)
                          ?? locations[rnd.Next(locations.Count)]
                        : locations[rnd.Next(locations.Count)];

                    // Randomise visit time during gym hours (06:00–22:00)
                    var hour    = rnd.Next(6, 22);
                    var minute  = rnd.Next(0, 60);
                    var visitAt = day.AddHours(hour).AddMinutes(minute).ToUniversalTime();

                    visits.Add(new Visit
                    {
                        MemberId     = member.Id,
                        LocationId   = loc.Id,
                        MembershipId = membership.Id,
                        VisitDate    = visitAt,
                        IsDeleted    = false
                    });
                }
            }

            // Batch insert
            if (visits.Any())
            {
                await context.Visits.AddRangeAsync(visits);
                await context.SaveChangesAsync();
            }
        }
    }
}
