using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness_Membership_Tracker.Data.Data.DataModels;

namespace Fitness_Membership_Tracker.Services.Interfaces
{
    public interface IMembershipService
    {
        Task<List<Membership>> GetAllAsync();
        Task<Membership?> GetByIdAsync(int id);
        Task CreateAsync(Membership membership);
        Task UpdateAsync(Membership membership);
        Task DeleteAsync(int id);
    }
}
