using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Services.Implementations
{
    public class VisitService : IVisitService
    {
        private readonly ApplicationDbContext _context;

        public VisitService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Visit>> GetAllAsync()
        {
            return await _context.Visits
                .Include(v => v.Member)
                .Include(v => v.Location)
                .Include(v => v.Membership)
                .ThenInclude(m => m.MembershipTier)
                .AsNoTracking()
                .OrderByDescending(v => v.VisitDate)
                .ToListAsync();
        }

        public async Task<List<Visit>> GetByMemberIdAsync(string memberId)
        {
            return await _context.Visits
                .Where(v => v.MemberId == memberId)
                .Include(v => v.Location)
                .Include(v => v.Membership)
                .ThenInclude(m => m.MembershipTier)
                .AsNoTracking()
                .OrderByDescending(v => v.VisitDate)
                .ToListAsync();
        }

        public async Task<Visit?> GetByIdAsync(int id)
        {
            return await _context.Visits
                .Include(v => v.Member)
                .Include(v => v.Location)
                .Include(v => v.Membership)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<List<Visit>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _context.Visits
                .Where(v => v.VisitDate >= from && v.VisitDate <= to)
                .Include(v => v.Member)
                .Include(v => v.Location)
                .Include(v => v.Membership)
                .ThenInclude(m => m.MembershipTier)
                .AsNoTracking()
                .OrderByDescending(v => v.VisitDate)
                .ToListAsync();
        }

        public async Task<Dictionary<DateTime, int>> GetDailyVisitCountsAsync(DateTime from, DateTime to)
        {
            var visits = await _context.Visits
                .Where(v => v.VisitDate >= from && v.VisitDate <= to)
                .AsNoTracking()
                .ToListAsync();

            return visits
                .GroupBy(v => v.VisitDate.Date)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public async Task CreateAsync(Visit visit)
        {
            visit.VisitDate = DateTime.UtcNow;
            await _context.Visits.AddAsync(visit);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var visit = await _context.Visits.FindAsync(id);

            if (visit == null)
                return;

            visit.IsDeleted = true;
            visit.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
