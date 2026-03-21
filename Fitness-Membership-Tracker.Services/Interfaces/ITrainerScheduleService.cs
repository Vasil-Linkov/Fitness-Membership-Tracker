using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Services.Interfaces
{
    public interface ITrainerScheduleService
    {
        Task<List<TrainerSchedule>> GetByTrainerIdAsync(int trainerId);
        Task<List<Trainer>> GetAvailableTrainersAsync(DayOfWeek day);
        Task AddSlotAsync(TrainerSchedule slot);
        Task RemoveSlotAsync(int slotId);
    }
}
