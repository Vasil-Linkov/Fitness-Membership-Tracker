using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Services.Implementations
{
    public class MembershipTierService : IMembershipTierService
    {
        private readonly ApplicationDbContext _context;

        public MembershipTierService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<MembershipTier>> GetTiersAsync()
            => await _context.MembershipTiers.AsNoTracking().ToListAsync();
    }
}
