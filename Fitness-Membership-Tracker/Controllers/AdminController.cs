using Fitness_Membership_Tracker.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task<IActionResult> Employees(int? locationId, string search)
        {
            var query = _context.Employees
                .Include(e => e.Location)
                .AsQueryable();

            if (locationId.HasValue && locationId != 0)
                query = query.Where(e => e.LocationId == locationId);

            if (!string.IsNullOrEmpty(search))
                query = query.Where(e =>
                    e.FirstName.Contains(search) ||
                    e.LastName.Contains(search));

            var employees = await query.ToListAsync();

            ViewBag.Locations = new SelectList(_context.Locations, "Id", "Name");
            return View(employees);
        }

    }
}
