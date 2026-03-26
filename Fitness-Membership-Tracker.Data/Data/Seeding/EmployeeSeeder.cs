using Fitness_Membership_Tracker.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Data.Seeding
{
    public static class EmployeeSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Employees.IgnoreQueryFilters().AnyAsync())
                return;

            // Locations must already be seeded; fetch their IDs
            var locations = await context.Locations.IgnoreQueryFilters()
                                         .OrderBy(l => l.Id)
                                         .ToListAsync();

            if (!locations.Any()) return;

            // id → locationId map for clarity
            var employees = new List<Employee>
            {
                // ── Sofia Mladost (loc 0) ────────────────────────────────
                new Employee
                {
                    FirstName   = "Teodora",
                    LastName    = "Ivanova",
                    Email       = "teodora.ivanova@fitzone.bg",
                    PhoneNumber = "0887101001",
                    HireDate    = new DateTime(2019, 3, 12),
                    Salary      = 1650m,
                    LocationId  = locations[0].Id
                },
                new Employee
                {
                    FirstName   = "Dimitar",
                    LastName    = "Hristov",
                    Email       = "dimitar.hristov@fitzone.bg",
                    PhoneNumber = "0887101002",
                    HireDate    = new DateTime(2020, 7, 1),
                    Salary      = 1580m,
                    LocationId  = locations[0].Id
                },
                new Employee
                {
                    FirstName   = "Kalina",
                    LastName    = "Nikolova",
                    Email       = "kalina.nikolova@fitzone.bg",
                    PhoneNumber = "0887101003",
                    HireDate    = new DateTime(2021, 1, 15),
                    Salary      = 1500m,
                    LocationId  = locations[0].Id
                },

                // ── Sofia Lyulin (loc 1) ─────────────────────────────────
                new Employee
                {
                    FirstName   = "Georgi",
                    LastName    = "Petrov",
                    Email       = "georgi.petrov@fitzone.bg",
                    PhoneNumber = "0887101004",
                    HireDate    = new DateTime(2018, 9, 5),
                    Salary      = 1720m,
                    LocationId  = locations[1].Id
                },
                new Employee
                {
                    FirstName   = "Silviya",
                    LastName    = "Todorova",
                    Email       = "silviya.todorova@fitzone.bg",
                    PhoneNumber = "0887101005",
                    HireDate    = new DateTime(2022, 4, 20),
                    Salary      = 1480m,
                    LocationId  = locations[1].Id
                },
                new Employee
                {
                    FirstName   = "Nikolay",
                    LastName    = "Vasilev",
                    Email       = "nikolay.vasilev@fitzone.bg",
                    PhoneNumber = "0887101006",
                    HireDate    = new DateTime(2020, 11, 3),
                    Salary      = 1540m,
                    LocationId  = locations[1].Id
                },

                // ── Sofia Tsentar (loc 2) ────────────────────────────────
                new Employee
                {
                    FirstName   = "Mariya",
                    LastName    = "Stoyanova",
                    Email       = "mariya.stoyanova@fitzone.bg",
                    PhoneNumber = "0887101007",
                    HireDate    = new DateTime(2017, 6, 18),
                    Salary      = 1850m,
                    LocationId  = locations[2].Id
                },
                new Employee
                {
                    FirstName   = "Hristo",
                    LastName    = "Angelov",
                    Email       = "hristo.angelov@fitzone.bg",
                    PhoneNumber = "0887101008",
                    HireDate    = new DateTime(2019, 8, 27),
                    Salary      = 1630m,
                    LocationId  = locations[2].Id
                },
                new Employee
                {
                    FirstName   = "Radoslava",
                    LastName    = "Koleva",
                    Email       = "radoslava.koleva@fitzone.bg",
                    PhoneNumber = "0887101009",
                    HireDate    = new DateTime(2023, 2, 6),
                    Salary      = 1460m,
                    LocationId  = locations[2].Id
                },

                // ── Sofia Lozenets (loc 3) ───────────────────────────────
                new Employee
                {
                    FirstName   = "Stefan",
                    LastName    = "Mihaylov",
                    Email       = "stefan.mihaylov@fitzone.bg",
                    PhoneNumber = "0887101010",
                    HireDate    = new DateTime(2021, 5, 10),
                    Salary      = 1600m,
                    LocationId  = locations[3].Id
                },
                new Employee
                {
                    FirstName   = "Eleonora",
                    LastName    = "Dimitrova",
                    Email       = "eleonora.dimitrova@fitzone.bg",
                    PhoneNumber = "0887101011",
                    HireDate    = new DateTime(2022, 9, 14),
                    Salary      = 1510m,
                    LocationId  = locations[3].Id
                },
                new Employee
                {
                    FirstName   = "Boyko",
                    LastName    = "Georgiev",
                    Email       = "boyko.georgiev@fitzone.bg",
                    PhoneNumber = "0887101012",
                    HireDate    = new DateTime(2020, 3, 22),
                    Salary      = 1570m,
                    LocationId  = locations[3].Id
                },

                // ── Sofia Nadezhda (loc 4) ───────────────────────────────
                new Employee
                {
                    FirstName   = "Veronika",
                    LastName    = "Popova",
                    Email       = "veronika.popova@fitzone.bg",
                    PhoneNumber = "0887101013",
                    HireDate    = new DateTime(2023, 6, 1),
                    Salary      = 1440m,
                    LocationId  = locations[4].Id
                },
                new Employee
                {
                    FirstName   = "Ivan",
                    LastName    = "Bozhkov",
                    Email       = "ivan.bozhkov@fitzone.bg",
                    PhoneNumber = "0887101014",
                    HireDate    = new DateTime(2021, 12, 9),
                    Salary      = 1560m,
                    LocationId  = locations[4].Id
                },

                // ── Plovdiv (loc 5) ──────────────────────────────────────
                new Employee
                {
                    FirstName   = "Katerina",
                    LastName    = "Mancheva",
                    Email       = "katerina.mancheva@fitzone.bg",
                    PhoneNumber = "0887101015",
                    HireDate    = new DateTime(2020, 2, 14),
                    Salary      = 1610m,
                    LocationId  = locations[5].Id
                },
                new Employee
                {
                    FirstName   = "Aleksandar",
                    LastName    = "Yordanov",
                    Email       = "aleksandar.yordanov@fitzone.bg",
                    PhoneNumber = "0887101016",
                    HireDate    = new DateTime(2019, 10, 30),
                    Salary      = 1680m,
                    LocationId  = locations[5].Id
                },

                // ── Varna (loc 6) ────────────────────────────────────────
                new Employee
                {
                    FirstName   = "Desislava",
                    LastName    = "Rasheva",
                    Email       = "desislava.rasheva@fitzone.bg",
                    PhoneNumber = "0887101017",
                    HireDate    = new DateTime(2022, 7, 7),
                    Salary      = 1490m,
                    LocationId  = locations[6].Id
                },
                new Employee
                {
                    FirstName   = "Petar",
                    LastName    = "Stanev",
                    Email       = "petar.stanev@fitzone.bg",
                    PhoneNumber = "0887101018",
                    HireDate    = new DateTime(2021, 4, 3),
                    Salary      = 1545m,
                    LocationId  = locations[6].Id
                },

                // ── Burgas (loc 7) ───────────────────────────────────────
                new Employee
                {
                    FirstName   = "Lilyana",
                    LastName    = "Kostadinova",
                    Email       = "lilyana.kostadinova@fitzone.bg",
                    PhoneNumber = "0887101019",
                    HireDate    = new DateTime(2023, 1, 16),
                    Salary      = 1430m,
                    LocationId  = locations[7].Id
                },
                new Employee
                {
                    FirstName   = "Rosen",
                    LastName    = "Ivanov",
                    Email       = "rosen.ivanov@fitzone.bg",
                    PhoneNumber = "0887101020",
                    HireDate    = new DateTime(2020, 5, 28),
                    Salary      = 1590m,
                    LocationId  = locations[7].Id
                }
            };

            await context.Employees.AddRangeAsync(employees);
            await context.SaveChangesAsync();
        }
    }
}
