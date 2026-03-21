using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Services.Implementations
{
    public class TrainerScheduleService : ITrainerScheduleService
    {
        private readonly ApplicationDbContext _context;

        public TrainerScheduleService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TrainerSchedule>> GetByTrainerIdAsync(int trainerId)
        {
            return await _context.TrainerSchedules
                .Where(s => s.TrainerId == trainerId && !s.IsBlocked)
                .AsNoTracking()
                .OrderBy(s => s.DayOfWeek)
                .ThenBy(s => s.StartTime)
                .ToListAsync();
        }

        public async Task<List<Trainer>> GetAvailableTrainersAsync(DayOfWeek day)
        {
            int dayInt = (int)day;

            var trainerIdsWithSlot = await _context.TrainerSchedules
                .Where(s => s.DayOfWeek == dayInt && !s.IsBlocked)
                .Select(s => s.TrainerId)
                .Distinct()
                .ToListAsync();

            if (!trainerIdsWithSlot.Any())
                return new List<Trainer>();

            const int defaultMax = 5;

            var availableTrainers = new List<Trainer>();

            foreach (var trainerId in trainerIdsWithSlot)
            {
                int activeCount = await _context.TrainerTrainees
                    .CountAsync(tt => tt.TrainerId == trainerId && tt.IsActive);

                var capacityRecord = await _context.TrainerCapacities
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.TrainerId == trainerId);

                int maxAllowed = capacityRecord?.MaxTrainees ?? defaultMax;

                if (activeCount < maxAllowed)
                {
                    var trainer = await _context.Trainers
                        .Include(t => t.Location)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(t => t.Id == trainerId);

                    if (trainer != null)
                        availableTrainers.Add(trainer);
                }
            }

            return availableTrainers;
        }

        public async Task AddSlotAsync(TrainerSchedule slot)
        {
            await _context.TrainerSchedules.AddAsync(slot);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveSlotAsync(int slotId)
        {
            var slot = await _context.TrainerSchedules.FindAsync(slotId);
            if (slot == null) return;

            _context.TrainerSchedules.Remove(slot);
            await _context.SaveChangesAsync();
        }
    }
}
