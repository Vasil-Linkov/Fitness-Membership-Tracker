using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Services.Implementations
{
    public class TrainingRequestService : ITrainingRequestService
    {
        private readonly ApplicationDbContext _context;
        private readonly ITrainerTraineeService _trainerTraineeService;

        public TrainingRequestService(
            ApplicationDbContext context,
            ITrainerTraineeService trainerTraineeService)
        {
            _context = context;
            _trainerTraineeService = trainerTraineeService;
        }

        public async Task<List<TrainingRequest>> GetPendingForTrainerAsync(int trainerId)
        {
            return await _context.TrainingRequests
                .Where(r => r.TrainerId == trainerId && r.Status == TrainingRequestStatus.Pending)
                .Include(r => r.Member)
                .Include(r => r.Trainer)
                .AsNoTracking()
                .OrderByDescending(r => r.RequestedAt)
                .ToListAsync();
        }

        public async Task<List<TrainingRequest>> GetByMemberIdAsync(string memberId)
        {
            return await _context.TrainingRequests
                .Where(r => r.MemberId == memberId)
                .Include(r => r.Trainer)
                    .ThenInclude(t => t.Location)
                .AsNoTracking()
                .OrderByDescending(r => r.RequestedAt)
                .ToListAsync();
        }

        public async Task<TrainingRequest?> GetByIdAsync(int id)
        {
            return await _context.TrainingRequests
                .Include(r => r.Member)
                .Include(r => r.Trainer)
                    .ThenInclude(t => t.Location)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task CreateAsync(TrainingRequest request)
        {
            request.RequestedAt = DateTime.UtcNow;
            request.Status = TrainingRequestStatus.Pending;
            await _context.TrainingRequests.AddAsync(request);
            await _context.SaveChangesAsync();
        }

        public async Task AcceptAsync(int requestId, string trainerResponse)
        {
            var request = await _context.TrainingRequests.FindAsync(requestId);
            if (request == null || request.Status != TrainingRequestStatus.Pending)
                return;

            bool hasCapacity = await _trainerTraineeService.HasCapacityAsync(request.TrainerId);
            if (!hasCapacity)
                throw new InvalidOperationException("Trainer has reached maximum trainee capacity.");

            request.Status = TrainingRequestStatus.Accepted;
            request.RespondedAt = DateTime.UtcNow;
            request.TrainerResponse = trainerResponse;

            var relationship = new TrainerTrainee
            {
                TrainerId = request.TrainerId,
                MemberId = request.MemberId,
                StartDate = DateTime.UtcNow,
                IsActive = true
            };

            await _context.TrainerTrainees.AddAsync(relationship);
            await _context.SaveChangesAsync();
        }

        public async Task RejectAsync(int requestId, string trainerResponse)
        {
            var request = await _context.TrainingRequests.FindAsync(requestId);
            if (request == null || request.Status != TrainingRequestStatus.Pending)
                return;

            request.Status = TrainingRequestStatus.Rejected;
            request.RespondedAt = DateTime.UtcNow;
            request.TrainerResponse = trainerResponse;

            await _context.SaveChangesAsync();
        }

        public async Task CancelAsync(int requestId)
        {
            var request = await _context.TrainingRequests.FindAsync(requestId);
            if (request == null || request.Status != TrainingRequestStatus.Pending)
                return;

            request.Status = TrainingRequestStatus.Cancelled;
            request.RespondedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
