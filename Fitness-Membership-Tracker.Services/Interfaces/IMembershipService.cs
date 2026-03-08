using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Services.Interfaces
{
    public interface IMembershipService
    {
        Task<List<Membership>> GetAllAsync();
        Task<Membership?> GetByIdAsync(int id);
        Task<Membership?> GetMembershipByMember(Member member);
		Task<Membership?> GetByIdIncludingDeletedAsync(int id);
		Task CreateAsync(Membership membership);
        Task UpdateAsync(Membership membership);
        Task DeleteAsync(int id);
    }
}
