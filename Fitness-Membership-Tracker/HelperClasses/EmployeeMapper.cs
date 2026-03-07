using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Models.AdminViewModels;

namespace Fitness_Membership_Tracker.HelperClasses
{
    public static class EmployeeMapper
    {
        public static Employee ToEntity(CreateEmployeeAdminViewModel vm)
        {
            return new Employee
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                HireDate = vm.HireDate,
                Salary = vm.Salary,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber,
                LocationId = vm.LocationId
            };
        }
    }
}
