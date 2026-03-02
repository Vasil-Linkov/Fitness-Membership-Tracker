using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetEmployeesAsync(int? locationId, string search);
        Task<Employee?> GetByIdAsync(int id);
        Task CreateAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);
    }
}
