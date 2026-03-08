using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Services.Implementations
{
	public class MemberService : IMemberService
	{
		private readonly ApplicationDbContext _context;

		public MemberService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<List<Member>> GetAllAsync()
		{
			return await _context.Members
				.Include(m => m.Membership)
				.Include(m => m.Payments)
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<Member?> GetByIdAsync(string id)
		{
			return await _context.Members
				.Include(m => m.Membership)
				.Include(m => m.Payments)
				.FirstOrDefaultAsync(m => m.Id == id);
		}

		public async Task<Member?> GetByNameAsync(string username)
		{
			return await _context.Members
				.Include(m => m.Membership)
				.ThenInclude(m => m.Location)
				.Include(m => m.Membership)
				.ThenInclude(m => m.MembershipTier)
				.Include(m => m.Payments)
				.FirstOrDefaultAsync(m => m.UserName == username);
		}

		public async Task UpdateAsync(Member updatedMember)
		{
			var existingMember = await _context.Members.FindAsync(updatedMember.Id);

			if (existingMember == null)
			{
				return;
			}

			existingMember.UserName = updatedMember.UserName;
			existingMember.Email = updatedMember.Email;
			existingMember.PhoneNumber = updatedMember.PhoneNumber;
			existingMember.MembershipId = updatedMember.MembershipId;

			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(string id)
		{
			var member = await _context.Members.FindAsync(id);

			if (member == null)
			{
				return;
			}

			member.IsDeleted = true;
			member.DeletedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();
		}
	}
}