using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Services.Implementations
{
    public class TrainerTraineeService : ITrainerTraineeService
    {
        private readonly ApplicationDbContext _context;
        private const int DefaultMaxTrainees = 5;

        public TrainerTraineeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrainerTrainee>> GetByTrainerIdAsync(int trainerId)
        {
            return await _context.TrainerTrainees
                .Where(tt => tt.TrainerId == trainerId && tt.IsActive)
                .Include(tt => tt.Member)
                .AsNoTracking()
                .OrderBy(tt => tt.StartDate)
                .ToListAsync();
        }

        public async Task<TrainerTrainee?> GetActiveRelationshipAsync(string memberId)
        {
            return await _context.TrainerTrainees
                .Where(tt => tt.MemberId == memberId && tt.IsActive)
                .Include(tt => tt.Trainer)
                    .ThenInclude(t => t.Location)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<int> GetActiveTraineeCountAsync(int trainerId)
        {
            return await _context.TrainerTrainees
                .CountAsync(tt => tt.TrainerId == trainerId && tt.IsActive);
        }

        public async Task<int> GetMaxTraineesAsync(int trainerId)
        {
            var cap = await _context.TrainerCapacities
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.TrainerId == trainerId);

            return cap?.MaxTrainees ?? DefaultMaxTrainees;
        }

        public async Task<bool> HasCapacityAsync(int trainerId)
        {
            int active = await GetActiveTraineeCountAsync(trainerId);
            int max = await GetMaxTraineesAsync(trainerId);
            return active < max;
        }

        public async Task UpdateMaxTraineesAsync(int trainerId, int newMax)
        {
            var cap = await _context.TrainerCapacities
                .FirstOrDefaultAsync(c => c.TrainerId == trainerId);

            if (cap == null)
            {
                cap = new TrainerCapacity { TrainerId = trainerId, MaxTrainees = newMax };
                await _context.TrainerCapacities.AddAsync(cap);
            }
            else
            {
                cap.MaxTrainees = newMax;
            }

            await _context.SaveChangesAsync();
        }

        public async Task EndRelationshipAsync(int trainerTraineeId)
        {
            var rel = await _context.TrainerTrainees.FindAsync(trainerTraineeId);
            if (rel == null) return;

            rel.IsActive = false;
            rel.EndDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}
