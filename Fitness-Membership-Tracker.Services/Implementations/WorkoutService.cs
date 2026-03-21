using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Services.Implementations
{
    public class WorkoutService : IWorkoutService
    {
        private readonly ApplicationDbContext _context;

        public WorkoutService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<WorkoutLog>> GetByMemberIdAsync(string memberId)
        {
            return await _context.WorkoutLogs
                .Where(wl => wl.MemberId == memberId)
                .Include(wl => wl.Exercises)
                .AsNoTracking()
                .OrderByDescending(wl => wl.LogDate)
                .ToListAsync();
        }

        public async Task<WorkoutLog?> GetByIdAsync(int id)
        {
            return await _context.WorkoutLogs
                .Include(wl => wl.Exercises)
                .Include(wl => wl.Member)
                .AsNoTracking()
                .FirstOrDefaultAsync(wl => wl.Id == id);
        }

        public async Task CreateAsync(WorkoutLog log)
        {
            log.LogDate = DateTime.UtcNow;
            await _context.WorkoutLogs.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var log = await _context.WorkoutLogs.FindAsync(id);
            if (log == null) return;

            log.IsDeleted = true;
            log.DeletedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
