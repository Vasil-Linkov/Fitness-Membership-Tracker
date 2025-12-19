using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Data.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Services
{
    public class DBSeeding
    {
        static ApplicationDbContext _context;
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

        public DBSeeding(ApplicationDbContext context)
        {
            _context = context;
        }


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

		public static void Seed()
        {
            _context.Database.EnsureCreated();

            if (!_context.Locations.Any())
            {
                _context.AddRange(
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
					}
                    );

                _context.SaveChanges();
            }
            
            if (!_context.MembershipTiers.Any())
            {
				_context.AddRange(
					new MembershipTier()
                    {
                        Id = 1,
                        Tier = "Basic",
                        MaxSessionsPerMonth = 8,
                        MonthlyPrice = 9.99m
                    },
					new MembershipTier()
					{
						Id = 2,
						Tier = "Advanced",
						MaxSessionsPerMonth = 12,
						MonthlyPrice = 15.99m
					},
					new MembershipTier()
					{
						Id = 3,
						Tier = "elite",
						MaxSessionsPerMonth = 18,
						MonthlyPrice = 21.99m
					},
					new MembershipTier()
					{
						Id = 4,
						Tier = "Ultimate",
						MaxSessionsPerMonth = 24,
						MonthlyPrice = 29.99m
					}
					);

				_context.SaveChanges();
            }

            if (!_context.Employees.Any())
            {
				int tempLocationId = 1;
                for(int  i = 1; i <= 12; i++ )
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

					_context.Employees.Add(employee);


					if (i % 3 == 0)
						tempLocationId++;
				}
				

                _context.SaveChanges();
            }
        }
    }
}
