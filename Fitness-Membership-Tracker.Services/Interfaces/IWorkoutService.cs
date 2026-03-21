using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Services.Interfaces
{
    public interface IWorkoutService
    {
        Task<List<WorkoutLog>> GetByMemberIdAsync(string memberId);
        Task<WorkoutLog?> GetByIdAsync(int id);
        Task CreateAsync(WorkoutLog log);
        Task DeleteAsync(int id);
    }
}
