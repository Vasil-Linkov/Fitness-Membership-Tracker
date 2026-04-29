using Fitness_Membership_Tracker.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Data.Seeding
{
    /// <summary>
    /// Seeds realistic training sessions (past and upcoming) for every active
    /// trainer-trainee relationship. Multiple sessions per week are created to
    /// demonstrate the scheduling feature.
    /// </summary>
    public static class TrainingSessionSeeder
    {
        private static readonly string[] SessionNotes =
        {
            "Lower body focus — squats, lunges, leg press.",
            "Upper body push — bench press, overhead press, dips.",
            "Upper body pull — rows, pull-ups, face pulls.",
            "Full body conditioning and cardio circuits.",
            "Core stability and mobility work.",
            "Strength assessment and new programme design.",
            "HIIT session — 30/30 intervals.",
            "Technique review — deadlift and squat form.",
            "Active recovery — stretching and light cardio.",
            "Nutrition review and goal-setting session.",
            string.Empty   // intentional blank
        };

        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.TrainingSessions.AnyAsync())
                return;

            var relationships = await context.TrainerTrainees
                .Where(tt => tt.IsActive)
                .ToListAsync();

            if (!relationships.Any()) return;

            var rnd      = new Random(42);
            var sessions = new List<TrainingSession>();
            var now      = DateTime.UtcNow;

            foreach (var rel in relationships)
            {
                // Each active pair gets 2-4 sessions per week over the past 8 weeks
                // plus 2 upcoming sessions in the next 2 weeks.
                int sessionsPerWeek = rnd.Next(2, 5);

                // Past sessions — 8 weeks back
                for (int weekOffset = -8; weekOffset <= -1; weekOffset++)
                {
                    var weekStart = now.AddDays(weekOffset * 7);

                    // Pick random days in this week (avoid duplicates)
                    var chosenDays = Enumerable.Range(0, 7)
                        .OrderBy(_ => rnd.Next())
                        .Take(sessionsPerWeek)
                        .Select(d => weekStart.AddDays(d))
                        .ToList();

                    foreach (var day in chosenDays)
                    {
                        var sessionTime = day.Date
                            .AddHours(rnd.Next(7, 20))
                            .AddMinutes(rnd.Next(0, 4) * 15);   // on the quarter hour

                        sessions.Add(new TrainingSession
                        {
                            TrainerId       = rel.TrainerId,
                            MemberId        = rel.MemberId,
                            SessionDate     = sessionTime,
                            DurationMinutes = new[] { 45, 60, 60, 75, 90 }[rnd.Next(5)],
                            Notes           = SessionNotes[rnd.Next(SessionNotes.Length)],
                            IsDeleted       = false
                        });
                    }
                }

                // 2 upcoming sessions in the next 14 days
                var upcomingDays = Enumerable.Range(1, 14)
                    .OrderBy(_ => rnd.Next())
                    .Take(2)
                    .Select(d => now.AddDays(d))
                    .ToList();

                foreach (var day in upcomingDays)
                {
                    sessions.Add(new TrainingSession
                    {
                        TrainerId       = rel.TrainerId,
                        MemberId        = rel.MemberId,
                        SessionDate     = day.Date.AddHours(rnd.Next(8, 19)),
                        DurationMinutes = 60,
                        Notes           = SessionNotes[rnd.Next(SessionNotes.Length)],
                        IsDeleted       = false
                    });
                }
            }

            if (sessions.Any())
            {
                await context.TrainingSessions.AddRangeAsync(sessions);
                await context.SaveChangesAsync();
            }
        }
    }
}
