using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness_Membership_Tracker.Data.Data.DataModels;

namespace Fitness_Membership_Tracker.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<List<Payment>> GetAllAsync();
        Task<List<Payment>> GetByMemberIdAsync(string memberId);
        Task CreateAsync(Payment payment);
    }
}
