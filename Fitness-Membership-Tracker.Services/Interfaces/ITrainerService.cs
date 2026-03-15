using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<List<Trainer>> GetTrainersAsync(int? locationId, string search);
        Task<Trainer?> GetByIdAsync(int id);
        Task CreateAsync(Trainer trainer);
        Task UpdateAsync(Trainer trainer);
        Task DeleteAsync(int id);
    }
}
