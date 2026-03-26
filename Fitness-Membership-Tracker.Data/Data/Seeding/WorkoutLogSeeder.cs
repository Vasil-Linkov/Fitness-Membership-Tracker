using Fitness_Membership_Tracker.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Data.Seeding
{
    public static class WorkoutLogSeeder
    {
        // Exercise library grouped by category
        private static readonly (string Name, int? DefaultSets, int? DefaultReps, decimal? DefaultKg, int? DefaultMins)[] Exercises =
        {
            // Compound lifts
            ("Bench Press",        4, 8,  80m,  null),
            ("Squat",              4, 6,  100m, null),
            ("Deadlift",           3, 5,  120m, null),
            ("Overhead Press",     4, 8,  60m,  null),
            ("Barbell Row",        4, 8,  70m,  null),

            // Isolation
            ("Dumbbell Curl",      3, 12, 20m,  null),
            ("Tricep Pushdown",    3, 12, 30m,  null),
            ("Lateral Raise",      3, 15, 10m,  null),
            ("Leg Press",          4, 10, 150m, null),
            ("Leg Curl",           3, 12, 50m,  null),
            ("Calf Raise",         4, 15, 80m,  null),
            ("Cable Fly",          3, 15, 20m,  null),
            ("Face Pull",          3, 15, 25m,  null),

            // Bodyweight / functional
            ("Pull-up",            3, 10, null, null),
            ("Push-up",            3, 20, null, null),
            ("Dip",                3, 12, null, null),
            ("Plank",              3, null, null, 1),
            ("Hanging Leg Raise",  3, 12, null, null),
            ("Box Jump",           3, 10, null, null),
            ("Kettlebell Swing",   4, 15, 24m,  null),

            // Cardio
            ("Treadmill Run",      null, null, null, 30),
            ("Stationary Bike",    null, null, null, 25),
            ("Rowing Machine",     null, null, null, 20),
            ("Elliptical",         null, null, null, 30),
            ("Jump Rope",          null, null, null, 10),

            // CrossFit / HIIT
            ("Burpee",             4, 15, null, null),
            ("Wall Ball",          4, 20, 9m,   null),
            ("Thrusters",          4, 10, 40m,  null),
            ("Box Step-up",        3, 12, null, null),
        };

        private static readonly string[] SessionNotes =
        {
            "Felt strong today — pushed a new PR on squats.",
            "Active recovery day, kept everything light.",
            "Focused on chest and triceps. Good pump.",
            "Cardio-heavy session, maintained zone 2 throughout.",
            "Legs are still sore from Tuesday. Took it easy on squat depth.",
            "Great energy today. Hit all planned sets.",
            "Tested new grip width on bench — noticed better chest activation.",
            "Morning session before work. Short but effective.",
            "Core and conditioning focus. Finished with 10-min HIIT.",
            "Deload week — used 70 % of working weights.",
            "Back felt a bit tight. Skipped deadlifts, added more rows.",
            "Personal best on overhead press: 80 kg × 4.",
            "High-volume day. 20 sets total.",
            "Technique work — paused reps on bench and squat.",
            string.Empty // intentionally blank note
        };

        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.WorkoutLogs.IgnoreQueryFilters().AnyAsync())
                return;

            var members = await context.Members.IgnoreQueryFilters()
                              .Where(m => m.MembershipId != null && m.Email != "admin@fitzone.bg")
                              .ToListAsync();

            if (!members.Any()) return;

            var rnd  = new Random(55);
            var logs = new List<WorkoutLog>();

            foreach (var member in members)
            {
                // Each member gets between 8 and 30 logged workouts over the last 90 days
                int logCount = rnd.Next(8, 31);

                for (int i = 0; i < logCount; i++)
                {
                    int daysAgo = rnd.Next(1, 91);
                    var logDate = DateTime.UtcNow.AddDays(-daysAgo)
                                          .Date.AddHours(rnd.Next(6, 21))
                                          .AddMinutes(rnd.Next(0, 60));

                    // Pick 2–5 exercises for this session
                    int exerciseCount = rnd.Next(2, 6);
                    var picked = Enumerable.Range(0, Exercises.Length)
                                           .OrderBy(_ => rnd.Next())
                                           .Take(exerciseCount)
                                           .Select(idx => Exercises[idx])
                                           .ToList();

                    var workoutExercises = picked.Select(ex =>
                    {
                        // Small random variance on numbers to make data feel organic
                        decimal? kg = ex.DefaultKg.HasValue
                            ? ex.DefaultKg.Value + rnd.Next(-10, 11)
                            : null;
                        if (kg.HasValue && kg < 5) kg = 5m;

                        return new WorkoutExercise
                        {
                            ExerciseName    = ex.Name,
                            Sets            = ex.DefaultSets,
                            Reps            = ex.DefaultReps.HasValue
                                                ? ex.DefaultReps.Value + rnd.Next(-2, 3)
                                                : null,
                            WeightKg        = kg,
                            DurationMinutes = ex.DefaultMins.HasValue
                                                ? ex.DefaultMins.Value + rnd.Next(-5, 6)
                                                : null,
                            Notes           = null
                        };
                    }).ToList();

                    logs.Add(new WorkoutLog
                    {
                        MemberId  = member.Id,
                        LogDate   = logDate,
                        Notes     = SessionNotes[rnd.Next(SessionNotes.Length)],
                        IsDeleted = false,
                        Exercises = workoutExercises
                    });
                }
            }

            await context.WorkoutLogs.AddRangeAsync(logs);
            await context.SaveChangesAsync();
        }
    }
}
