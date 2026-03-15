using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Models.AdminViewModels;

namespace Fitness_Membership_Tracker.HelperClasses
{
    public static class TrainerMapper
    {
        public static Trainer ToEntity(CreateTrainerAdminViewModel vm)
        {
            return new Trainer
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                HireDate = vm.HireDate,
                Salary = vm.Salary,
                Specialization = vm.Specialization,
                PhoneNumber = vm.PhoneNumber,
                LocationId = vm.LocationId
            };
        }
    }
}
