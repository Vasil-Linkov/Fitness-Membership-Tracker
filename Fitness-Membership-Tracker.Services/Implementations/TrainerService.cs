using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Services.Implementations
{
    public class TrainerService : ITrainerService
    {
        private readonly ApplicationDbContext _context;

        public TrainerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Trainer>> GetTrainersAsync(int? locationId, string search)
        {
            var query = _context.Trainers
                .Include(t => t.Location)
                .AsNoTracking()
                .AsQueryable();

            if (locationId.HasValue)
                query = query.Where(t => t.LocationId == locationId.Value);

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(t =>
                    t.FirstName.Contains(search) ||
                    t.LastName.Contains(search) ||
                    t.Email.Contains(search));
            }

            return await query.ToListAsync();
        }

        public async Task<Trainer?> GetByIdAsync(int id)
            => await _context.Trainers
                .Include(t => t.Location)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

        public async Task CreateAsync(Trainer trainer)
        {
            await _context.Trainers.AddAsync(trainer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Trainer updatedTrainer)
        {
            var existing = await _context.Trainers.FindAsync(updatedTrainer.Id);
            if (existing == null) return;

            existing.FirstName = updatedTrainer.FirstName;
            existing.LastName = updatedTrainer.LastName;
            existing.Email = updatedTrainer.Email;
            existing.PhoneNumber = updatedTrainer.PhoneNumber;
            existing.Specialization = updatedTrainer.Specialization;
            existing.HireDate = updatedTrainer.HireDate;
            existing.Salary = updatedTrainer.Salary;
            existing.LocationId = updatedTrainer.LocationId;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null) return;

            trainer.IsDeleted = true;
            trainer.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
