using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Data.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Fitness_Membership_Tracker.Services
{
    public static class DBSeeding
	{
		private static readonly IReadOnlyList<string> First_Names = new List<string>
		{
			"Alexander",
			"Ivan",
			"Petar",
			"Georgi",
			"Maria",
			"Elena",
			"Viktoria",
			"Nikola",
			"Dimitar",
			"Kristina",
			"Stefan",
			"Radoslav",
			"Svetlana",
			"Mihail",
			"Iva",
			"Todor",
			"Katerina",
			"Hristo",
			"Boris",
			"Desislava"
		};
		private static readonly IReadOnlyList<string> Last_Names = new List<string>
		{
			"Ivanov",
			"Petrov",
			"Georgiev",
			"Dimitrov",
			"Kovachev",
			"Nikolaev",
			"Radoslavov",
			"Hristov",
			"Vasilev",
			"Stoyanov",
			"Mihaylov",
			"Kolev",
			"Todorov",
			"Nikolov",
			"Zahariev",
			"Simeonov",
			"Vladimirov",
			"Bozhkov",
			"Angelov",
			"Daskalov"
		};
		private static string GenerateEmail(string firstName, string lastName)
		{
			StringBuilder email = new StringBuilder();

			email.Append(firstName);
			email.Append('.');
			email.Append(lastName);
			email.Append("@gmail.com");

			return email.ToString();
		}
		private static string GeneratePhoneNumber()
		{
			string number = "";
			for (int i = 0; i < 10; i++)
			{
				number += Random.Shared.Next(0, 10);
			}
			return number;
		}


		public static List<Location> SeedLocations()
		{
			List<Location> locations = [
					new Location
					{
						Id = 1,
						Address = "бул. Черни връх 47, Младост, 1303",
						City = "Sofia",
						Country = "Bulgaria"
					},
					new Location
					{
						Id = 2,
						Address = "ул. Пирин 12, Люлин, 1324",
						City = "Sofia",
						Country = "Bulgaria"
					},
					new Location
					{
						Id = 3,
						Address = "ул. Христо Ботев 23, Център, 1000",
						City = "Sofia",
						Country = "Bulgaria"
					},
					new Location
					{
						Id = 4,
						Address = "ул. Васил Левски 45",
						City = "Sofia",
						Country = "Bulgaria"
					}];

			return locations;
		}
		public static List<MembershipTier> SeedMembershipTiers()
		{
			List<MembershipTier> membershipTiers = [
					new MembershipTier()
					{
						Id = 1,
						Tier = "Basic",
						MaxSessionsPerMonth = 8,
						Description = "Access to gym facilities during staffed hours.",
						Accessibility = "address",
						MonthlyPrice = 9.99m
					},
					new MembershipTier()
					{
						Id = 2,
						Tier = "Advanced",
						MaxSessionsPerMonth = 12,
						Description = "Access to gym facilities during staffed hours.",
						Accessibility = "city",
						MonthlyPrice = 15.99m
					},
					new MembershipTier()
					{
						Id = 3,
						Tier = "elite",
						MaxSessionsPerMonth = 18,
						Description = "Access to gym facilities during staffed hours.",
						Accessibility = "country",
						MonthlyPrice = 21.99m
					},
					new MembershipTier()
					{
						Id = 4,
						Tier = "Ultimate",
						MaxSessionsPerMonth = 24,
						Description = "Access to gym facilities during staffed hours.",
						Accessibility = "any",
						MonthlyPrice = 29.99m
					}
					];

			return membershipTiers;
		}
		public static List<Employee> SeedEmployees()
		{
			List<Employee> employees = new List<Employee>();

			int tempLocationId = 1;
			for (int i = 1; i <= 12; i++)
			{
				var employee = new Employee
				{
					Id = i,
					FirstName = First_Names[Random.Shared.Next(0, First_Names.Count)],
					LastName = Last_Names[Random.Shared.Next(0, Last_Names.Count)],
					PhoneNumber = GeneratePhoneNumber(),
					HireDate = new DateTime(2020, Random.Shared.Next(1, 13), Random.Shared.Next(1, 29)),
					Salary = Random.Shared.Next(1400, 1700),
					LocationId = tempLocationId
				};
				employee.Email = GenerateEmail(employee.FirstName, employee.LastName);

				employees.Add(employee);


				if (i % 3 == 0)
					tempLocationId++;
			}


			return employees;
		}



        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<Member>>();

            // Create Admin role if not exists
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // Create default admin user
            var adminEmail = "admin@gym.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new Member
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

    }
}
