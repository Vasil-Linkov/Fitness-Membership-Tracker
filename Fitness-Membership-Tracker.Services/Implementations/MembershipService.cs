using Fitness_Membership_Tracker.Data;
using Microsoft.EntityFrameworkCore;
using Fitness_Membership_Tracker.Services.Interfaces;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Data.Data.DataModels;

namespace Fitness_Membership_Tracker.Services.Implementations;

public class MembershipService : IMembershipService
{
    private readonly ApplicationDbContext _context;

    public MembershipService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Membership>> GetAllAsync()
    {
        return await _context.Memberships.AsNoTracking().ToListAsync();
    }

    public async Task<Membership?> GetByIdAsync(int id)
    {
        return await _context.Memberships.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Membership?> GetMembershipByMember(Member member)
    {
		return await _context.Memberships.AsNoTracking().FirstOrDefaultAsync(m => m.MemberId == member.Id);
	}

    public async Task CreateAsync(Membership membership)
    {
        _context.Memberships.Add(membership);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Membership updatedMembership)
    {
        var oldMembership = await GetByIdAsync(updatedMembership.Id);
        if (oldMembership == null) return;

        oldMembership = updatedMembership;

        _context.Memberships.Update(oldMembership);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var membership = await GetByIdAsync(id);
        if (membership == null) return;

        membership.IsDeleted = true;

        _context.Memberships.Update(membership);
        await _context.SaveChangesAsync();
    }
}