using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Services.Implementations
{
	public class MembershipService : IMembershipService
	{
		private readonly ApplicationDbContext _context;

		public MembershipService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<Membership>> GetAllAsync()
		{
			return await _context.Memberships
				.Include(m => m.Location)
				.Include(m => m.MembershipTier)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<Membership?> GetByIdAsync(int id)
		{
			return await _context.Memberships
				.Include(m => m.Location)
				.Include(m => m.MembershipTier)
				.FirstOrDefaultAsync(m => m.Id == id);
		}

		public async Task<Membership?> GetMembershipByMember(Member member)
		{
			if (member == null || member.MembershipId == null)
			{
				return null;
			}

			return await _context.Memberships
				.Include(m => m.Location)
				.Include(m => m.MembershipTier)
				.FirstOrDefaultAsync(m => m.Id == member.MembershipId);
		}

		public async Task CreateAsync(Membership membership)
		{
			await _context.Memberships.AddAsync(membership);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Membership updatedMembership)
		{
			var existingMembership = await _context.Memberships.FindAsync(updatedMembership.Id);

			if (existingMembership == null)
			{
				return;
			}

			existingMembership.StartDate = updatedMembership.StartDate;
			existingMembership.EndDate = updatedMembership.EndDate;
			existingMembership.LocationId = updatedMembership.LocationId;
			existingMembership.MembershipTierId = updatedMembership.MembershipTierId;

			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var membership = await _context.Memberships.FindAsync(id);

			if (membership == null)
			{
				return;
			}

			membership.IsDeleted = true;
			membership.DeletedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();
		}
	}
}