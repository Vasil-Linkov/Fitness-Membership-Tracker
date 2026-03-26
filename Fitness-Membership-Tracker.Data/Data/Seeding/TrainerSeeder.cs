using Fitness_Membership_Tracker.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Data.Seeding
{
    public static class TrainerSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Trainers.IgnoreQueryFilters().AnyAsync())
                return;

            var locations = await context.Locations.IgnoreQueryFilters()
                                         .OrderBy(l => l.Id)
                                         .ToListAsync();

            if (!locations.Any()) return;

            var trainers = new List<Trainer>
            {
                // ── Sofia Mladost (loc 0) ────────────────────────────────
                new Trainer
                {
                    FirstName      = "Maria",
                    LastName       = "Ivanova",
                    Email          = "maria.ivanova@fitzone.bg",
                    PhoneNumber    = "0889201001",
                    Specialization = "Yoga",
                    HireDate       = new DateTime(2019, 4, 15),
                    Salary         = 1750m,
                    LocationId     = locations[0].Id
                },
                new Trainer
                {
                    FirstName      = "Georgi",
                    LastName       = "Petrov",
                    Email          = "georgi.petrov.trainer@fitzone.bg",
                    PhoneNumber    = "0889201002",
                    Specialization = "Personal Training",
                    HireDate       = new DateTime(2018, 7, 1),
                    Salary         = 2100m,
                    LocationId     = locations[0].Id
                },
                new Trainer
                {
                    FirstName      = "Desislava",
                    LastName       = "Angelova",
                    Email          = "desislava.angelova@fitzone.bg",
                    PhoneNumber    = "0889201003",
                    Specialization = "Cardio",
                    HireDate       = new DateTime(2021, 9, 3),
                    Salary         = 1680m,
                    LocationId     = locations[0].Id
                },

                // ── Sofia Lyulin (loc 1) ─────────────────────────────────
                new Trainer
                {
                    FirstName      = "Elena",
                    LastName       = "Dimitrova",
                    Email          = "elena.dimitrova@fitzone.bg",
                    PhoneNumber    = "0889201004",
                    Specialization = "Pilates",
                    HireDate       = new DateTime(2020, 1, 10),
                    Salary         = 1800m,
                    LocationId     = locations[1].Id
                },
                new Trainer
                {
                    FirstName      = "Nikola",
                    LastName       = "Hristov",
                    Email          = "nikola.hristov@fitzone.bg",
                    PhoneNumber    = "0889201005",
                    Specialization = "CrossFit",
                    HireDate       = new DateTime(2017, 5, 20),
                    Salary         = 2200m,
                    LocationId     = locations[1].Id
                },
                new Trainer
                {
                    FirstName      = "Alexander",
                    LastName       = "Kolev",
                    Email          = "alexander.kolev@fitzone.bg",
                    PhoneNumber    = "0889201006",
                    Specialization = "Swimming",
                    HireDate       = new DateTime(2022, 8, 18),
                    Salary         = 1900m,
                    LocationId     = locations[1].Id
                },

                // ── Sofia Tsentar (loc 2) ────────────────────────────────
                new Trainer
                {
                    FirstName      = "Viktoria",
                    LastName       = "Stoyanova",
                    Email          = "viktoria.stoyanova@fitzone.bg",
                    PhoneNumber    = "0889201007",
                    Specialization = "Group Fitness",
                    HireDate       = new DateTime(2021, 3, 8),
                    Salary         = 1750m,
                    LocationId     = locations[2].Id
                },
                new Trainer
                {
                    FirstName      = "Stefan",
                    LastName       = "Vasilev",
                    Email          = "stefan.vasilev@fitzone.bg",
                    PhoneNumber    = "0889201008",
                    Specialization = "Strength & Conditioning",
                    HireDate       = new DateTime(2019, 2, 14),
                    Salary         = 2050m,
                    LocationId     = locations[2].Id
                },

                // ── Sofia Lozenets (loc 3) ───────────────────────────────
                new Trainer
                {
                    FirstName      = "Katerina",
                    LastName       = "Nikolova",
                    Email          = "katerina.nikolova.trainer@fitzone.bg",
                    PhoneNumber    = "0889201009",
                    Specialization = "Nutrition",
                    HireDate       = new DateTime(2023, 4, 5),
                    Salary         = 1950m,
                    LocationId     = locations[3].Id
                },
                new Trainer
                {
                    FirstName      = "Hristo",
                    LastName       = "Todorov",
                    Email          = "hristo.todorov@fitzone.bg",
                    PhoneNumber    = "0889201010",
                    Specialization = "Martial Arts",
                    HireDate       = new DateTime(2016, 11, 30),
                    Salary         = 2300m,
                    LocationId     = locations[3].Id
                },

                // ── Sofia Nadezhda (loc 4) ───────────────────────────────
                new Trainer
                {
                    FirstName      = "Boyana",
                    LastName       = "Georgieva",
                    Email          = "boyana.georgieva@fitzone.bg",
                    PhoneNumber    = "0889201011",
                    Specialization = "Yoga",
                    HireDate       = new DateTime(2022, 6, 22),
                    Salary         = 1700m,
                    LocationId     = locations[4].Id
                },
                new Trainer
                {
                    FirstName      = "Todor",
                    LastName       = "Slavchev",
                    Email          = "todor.slavchev@fitzone.bg",
                    PhoneNumber    = "0889201012",
                    Specialization = "Personal Training",
                    HireDate       = new DateTime(2020, 10, 5),
                    Salary         = 2000m,
                    LocationId     = locations[4].Id
                },

                // ── Plovdiv (loc 5) ──────────────────────────────────────
                new Trainer
                {
                    FirstName      = "Milena",
                    LastName       = "Bankova",
                    Email          = "milena.bankova@fitzone.bg",
                    PhoneNumber    = "0889201013",
                    Specialization = "Pilates",
                    HireDate       = new DateTime(2021, 7, 19),
                    Salary         = 1820m,
                    LocationId     = locations[5].Id
                },
                new Trainer
                {
                    FirstName      = "Rumen",
                    LastName       = "Neykov",
                    Email          = "rumen.neykov@fitzone.bg",
                    PhoneNumber    = "0889201014",
                    Specialization = "Strength & Conditioning",
                    HireDate       = new DateTime(2018, 3, 28),
                    Salary         = 2150m,
                    LocationId     = locations[5].Id
                },

                // ── Varna (loc 6) ────────────────────────────────────────
                new Trainer
                {
                    FirstName      = "Yana",
                    LastName       = "Trifonova",
                    Email          = "yana.trifonova@fitzone.bg",
                    PhoneNumber    = "0889201015",
                    Specialization = "Group Fitness",
                    HireDate       = new DateTime(2022, 2, 10),
                    Salary         = 1780m,
                    LocationId     = locations[6].Id
                },
                new Trainer
                {
                    FirstName      = "Plamen",
                    LastName       = "Zhelev",
                    Email          = "plamen.zhelev@fitzone.bg",
                    PhoneNumber    = "0889201016",
                    Specialization = "CrossFit",
                    HireDate       = new DateTime(2020, 9, 15),
                    Salary         = 2080m,
                    LocationId     = locations[6].Id
                },

                // ── Burgas (loc 7) ───────────────────────────────────────
                new Trainer
                {
                    FirstName      = "Antonia",
                    LastName       = "Lebedova",
                    Email          = "antonia.lebedova@fitzone.bg",
                    PhoneNumber    = "0889201017",
                    Specialization = "Cardio",
                    HireDate       = new DateTime(2023, 3, 7),
                    Salary         = 1650m,
                    LocationId     = locations[7].Id
                },
                new Trainer
                {
                    FirstName      = "Momchil",
                    LastName       = "Atanasov",
                    Email          = "momchil.atanasov@fitzone.bg",
                    PhoneNumber    = "0889201018",
                    Specialization = "Martial Arts",
                    HireDate       = new DateTime(2019, 6, 11),
                    Salary         = 2050m,
                    LocationId     = locations[7].Id
                }
            };

            await context.Trainers.AddRangeAsync(trainers);
            await context.SaveChangesAsync();

            // Seed default capacity (5 trainees) for every trainer
            var savedTrainers = await context.Trainers.IgnoreQueryFilters().ToListAsync();
            var capacities = savedTrainers.Select(t => new TrainerCapacity
            {
                TrainerId   = t.Id,
                MaxTrainees = 5
            }).ToList();

            await context.TrainerCapacities.AddRangeAsync(capacities);
            await context.SaveChangesAsync();

            // Seed a representative weekly schedule per trainer
            var scheduleSlots = new List<TrainerSchedule>();
            var random = new Random(42);
            // Days: Mon=1, Tue=2, Wed=3, Thu=4, Fri=5, Sat=6
            int[][] slotPatterns =
            {
                new[] { 1, 3, 5 },        // Mon / Wed / Fri
                new[] { 2, 4, 6 },        // Tue / Thu / Sat
                new[] { 1, 2, 3, 4, 5 },  // Weekdays
                new[] { 1, 3, 6 },        // Mon / Wed / Sat
                new[] { 2, 4, 5 },        // Tue / Thu / Fri
            };
            TimeSpan[][] timePairs =
            {
                new[] { new TimeSpan(8, 0, 0),  new TimeSpan(12, 0, 0) },
                new[] { new TimeSpan(10, 0, 0), new TimeSpan(14, 0, 0) },
                new[] { new TimeSpan(14, 0, 0), new TimeSpan(18, 0, 0) },
                new[] { new TimeSpan(16, 0, 0), new TimeSpan(20, 0, 0) },
                new[] { new TimeSpan(9, 0, 0),  new TimeSpan(13, 0, 0) },
            };

            foreach (var trainer in savedTrainers)
            {
                var pattern = slotPatterns[random.Next(slotPatterns.Length)];
                var times   = timePairs[random.Next(timePairs.Length)];

                foreach (var day in pattern)
                {
                    scheduleSlots.Add(new TrainerSchedule
                    {
                        TrainerId = trainer.Id,
                        DayOfWeek = day,
                        StartTime = times[0],
                        EndTime   = times[1],
                        IsBlocked = false
                    });
                }
            }

            await context.TrainerSchedules.AddRangeAsync(scheduleSlots);
            await context.SaveChangesAsync();
        }
    }
}
