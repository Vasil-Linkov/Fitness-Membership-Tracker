using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Services.Interfaces
{
    public interface ITrainingSessionService
    {
        Task<List<TrainingSession>> GetByTrainerIdAsync(int trainerId);
        Task<List<TrainingSession>> GetByMemberIdAsync(string memberId);
        Task<TrainingSession?> GetByIdAsync(int id);
        Task CreateAsync(TrainingSession session);
        Task DeleteAsync(int id);
    }
}
