using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Services.Interfaces
{
    public interface ITrainingRequestService
    {
        Task<List<TrainingRequest>> GetPendingForTrainerAsync(int trainerId);
        Task<List<TrainingRequest>> GetByMemberIdAsync(string memberId);
        Task<TrainingRequest?> GetByIdAsync(int id);
        Task CreateAsync(TrainingRequest request);
        Task AcceptAsync(int requestId, string trainerResponse);
        Task RejectAsync(int requestId, string trainerResponse);
        Task CancelAsync(int requestId);
    }
}
