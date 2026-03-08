using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Services.Interfaces
{
    public interface IMembershipTierService
    {

        public Task<List<MembershipTier>> GetTiersAsync();
        public Task<MembershipTier?> GetByIdAsync(int id);
    }
}
