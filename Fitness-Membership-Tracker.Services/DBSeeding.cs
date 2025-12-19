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
        ApplicationDbContext _context;

        public DBSeeding(ApplicationDbContext context)
        {
            _context = context;
        }


        public void Seed()
        {
            _context.Database.EnsureCreated();

            if (!_context.Members.Any())
            {


                _context.SaveChanges();
            }

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


                _context.SaveChanges();
            }

            if (!_context.Memberships.Any())
            {


                _context.SaveChanges();
            }

            if (!_context.Payments.Any())
            {


                _context.SaveChanges();
            }
        }
    }
}
