using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Services.Interfaces
{
    public interface IMemberService
    {
        Task<List<Member>> GetAllAsync();
        Task<Member?> GetByIdAsync(string id);
        Task<Member?> GetByNameAsync(string name);
        Task<List<Member>?> GetMembersWithoutMembership();
        Task UpdateAsync(Member member);
        Task DeleteAsync(string id);
    }
}
