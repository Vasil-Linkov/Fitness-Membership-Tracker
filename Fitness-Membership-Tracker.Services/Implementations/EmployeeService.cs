using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetEmployeesAsync(int? locationId, string search)
        {
            var query = _context.Employees
                .Include(e => e.Location)
                .AsQueryable();

            if (locationId.HasValue && locationId != 0)
                query = query.Where(e => e.LocationId == locationId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(e => e.FirstName.Contains(search) || e.LastName.Contains(search));

            return await query.ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id)
            => await _context.Employees.FindAsync(id);

        public async Task CreateAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await GetByIdAsync(id);

            if (employee == null) return;

            employee.IsDeleted = true;

            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
        }
    }
}
