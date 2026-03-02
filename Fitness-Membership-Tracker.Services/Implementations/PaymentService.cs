using Fitness_Membership_Tracker.Data;
using Microsoft.EntityFrameworkCore;
using Fitness_Membership_Tracker.Services.Interfaces;
using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Services.Implementations;

public class PaymentService : IPaymentService
{
    private readonly ApplicationDbContext _context;

    public PaymentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Payment>> GetAllAsync()
    {
        return await _context.Payments
            .Include(p => p.Member)
            .Include(p => p.Membership)
            .ToListAsync();
    }

    public async Task<List<Payment>> GetByMemberIdAsync(string memberId)
    {
        return await _context.Payments
            .Where(p => p.MemberId == memberId)
            .Include(p => p.Membership)
            .ToListAsync();
    }

    public async Task CreateAsync(Payment payment)
    {
        payment.PaymentDate = DateTime.UtcNow;

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
    }
}