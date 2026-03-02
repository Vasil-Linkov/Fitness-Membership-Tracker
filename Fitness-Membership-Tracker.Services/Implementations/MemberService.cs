using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Data.Data.DataModels;

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
            => await _context.Users
                .Include(m => m.Membership)
                .AsNoTracking()
                .ToListAsync();

        public async Task<Member?> GetByIdAsync(string id)
            => await _context.Users
                .Include(m => m.Membership)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

        public async Task UpdateAsync(Member member)
        {
            _context.Users.Update(member);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var member = await GetByIdAsync(id);
            
            if (member == null) return;

            member.IsDeleted = true;

            _context.Users.Update(member);
            await _context.SaveChangesAsync();
        }
    }
}
