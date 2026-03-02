using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Data.Data.DataModels;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Services.Implementations
{
    public class LocationService : ILocationService
    {
        private readonly ApplicationDbContext _context;

        public LocationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Location>> GetAllAsync()
        {
            return await _context.Locations.AsNoTracking().ToListAsync();
        }

        public Task<Location?> GetByIdAsync(int id)
        {
            return _context.Locations.AsNoTracking().FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}
