using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness_Membership_Tracker.Data;
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


                _context.SaveChanges();
            }
            
            if (!_context.MembershipTiers.Any())
            {


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
