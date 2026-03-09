using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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
                .AsNoTracking()
                .AsQueryable();

			if (locationId.HasValue)
			{
				query = query.Where(e => e.LocationId == locationId.Value);
			}

			if (!string.IsNullOrWhiteSpace(search))
			{
				search = search.Trim();

				query = query.Where(e =>
					e.FirstName.Contains(search) ||
					e.LastName.Contains(search) ||
					e.Email.Contains(search));
			}

			return await query
				.AsNoTracking()
				.ToListAsync();
		}

		public async Task<Employee?> GetByIdAsync(int id)
		{
			return await _context.Employees
				.Include(e => e.Location)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
		}

		public async Task CreateAsync(Employee employee)
		{
			await _context.Employees.AddAsync(employee);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateAsync(Employee updatedEmployee)
		{
			var existingEmployee = await _context.Employees.FindAsync(updatedEmployee.Id);

			if (existingEmployee == null)
			{
				return;
			}

			existingEmployee.FirstName = updatedEmployee.FirstName;
			existingEmployee.LastName = updatedEmployee.LastName;
			existingEmployee.HireDate = updatedEmployee.HireDate;
			existingEmployee.Salary = updatedEmployee.Salary;
			existingEmployee.Email = updatedEmployee.Email;
			existingEmployee.PhoneNumber = updatedEmployee.PhoneNumber;
			existingEmployee.LocationId = updatedEmployee.LocationId;

			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var employee = await _context.Employees.FindAsync(id);

			if (employee == null)
			{
				return;
			}

			employee.IsDeleted = true;
			employee.DeletedAt = DateTime.UtcNow;

			await _context.SaveChangesAsync();
		}
	}
}