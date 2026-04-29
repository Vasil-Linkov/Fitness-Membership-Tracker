using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Services.Implementations
{
    public class TrainingSessionService : ITrainingSessionService
    {
        private readonly ApplicationDbContext _context;

        public TrainingSessionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrainingSession>> GetByTrainerIdAsync(int trainerId)
        {
            return await _context.TrainingSessions
                .Where(s => s.TrainerId == trainerId && !s.IsDeleted)
                .Include(s => s.Member)
                .AsNoTracking()
                .OrderBy(s => s.SessionDate)
                .ToListAsync();
        }

        public async Task<List<TrainingSession>> GetByMemberIdAsync(string memberId)
        {
            return await _context.TrainingSessions
                .Where(s => s.MemberId == memberId && !s.IsDeleted)
                .Include(s => s.Trainer)
                    .ThenInclude(t => t.Location)
                .AsNoTracking()
                .OrderBy(s => s.SessionDate)
                .ToListAsync();
        }

        public async Task<TrainingSession?> GetByIdAsync(int id)
        {
            return await _context.TrainingSessions
                .Include(s => s.Trainer)
                .Include(s => s.Member)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task CreateAsync(TrainingSession session)
        {
            await _context.TrainingSessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var session = await _context.TrainingSessions.FindAsync(id);
            if (session == null) return;

            session.IsDeleted  = true;
            session.DeletedAt  = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
