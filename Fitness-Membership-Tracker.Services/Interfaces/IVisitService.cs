using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Services.Interfaces
{
    public interface IVisitService
    {
        Task<List<Visit>> GetAllAsync();
        Task<List<Visit>> GetByMemberIdAsync(string memberId);
        Task<Visit?> GetByIdAsync(int id);
        Task<List<Visit>> GetByDateRangeAsync(DateTime from, DateTime to);
        Task<Dictionary<DateTime, int>> GetDailyVisitCountsAsync(DateTime from, DateTime to);
        Task CreateAsync(Visit visit);
        Task DeleteAsync(int id);
    }
}
