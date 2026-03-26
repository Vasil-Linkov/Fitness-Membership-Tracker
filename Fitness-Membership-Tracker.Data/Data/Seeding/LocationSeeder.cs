using Fitness_Membership_Tracker.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Data.Seeding
{
    public static class LocationSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.Locations.IgnoreQueryFilters().AnyAsync())
                return;

            var locations = new List<Location>
            {
                new Location
                {
                    Address = "бул. Черни връх 47, Младост",
                    City    = "Sofia",
                    Country = "Bulgaria"
                },
                new Location
                {
                    Address = "ул. Пирин 12, Люлин",
                    City    = "Sofia",
                    Country = "Bulgaria"
                },
                new Location
                {
                    Address = "ул. Христо Ботев 23, Център",
                    City    = "Sofia",
                    Country = "Bulgaria"
                },
                new Location
                {
                    Address = "ул. Васил Левски 45, Лозенец",
                    City    = "Sofia",
                    Country = "Bulgaria"
                },
                new Location
                {
                    Address = "бул. Александър Стамболийски 10, Надежда",
                    City    = "Sofia",
                    Country = "Bulgaria"
                },
                new Location
                {
                    Address = "ул. Патриарх Евтимий 22, Яворов",
                    City    = "Plovdiv",
                    Country = "Bulgaria"
                },
                new Location
                {
                    Address = "бул. Мария Луиза 38, Център",
                    City    = "Varna",
                    Country = "Bulgaria"
                },
                new Location
                {
                    Address = "ул. Шейново 5, Борово",
                    City    = "Burgas",
                    Country = "Bulgaria"
                }
            };

            await context.Locations.AddRangeAsync(locations);
            await context.SaveChangesAsync();
        }
    }
}
