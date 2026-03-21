using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Services.Interfaces
{
    public interface ITrainerTraineeService
    {
        Task<List<TrainerTrainee>> GetByTrainerIdAsync(int trainerId);
        Task<TrainerTrainee?> GetActiveRelationshipAsync(string memberId);
        Task<int> GetActiveTraineeCountAsync(int trainerId);
        Task<int> GetMaxTraineesAsync(int trainerId);
        Task<bool> HasCapacityAsync(int trainerId);
        Task UpdateMaxTraineesAsync(int trainerId, int newMax);
        Task EndRelationshipAsync(int trainerTraineeId);
    }
}
