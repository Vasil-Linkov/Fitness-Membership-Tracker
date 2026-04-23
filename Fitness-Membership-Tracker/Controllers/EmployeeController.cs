using Fitness_Membership_Tracker.Constants;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fitness_Membership_Tracker.Controllers
{
    [Authorize(Roles = Roles.Employee)]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly UserManager<Member> _userManager;

        public EmployeeController(
            IEmployeeService employeeService,
            UserManager<Member> userManager)
        {
            _employeeService = employeeService;
            _userManager = userManager;
        }

        // GET /Employee/Dashboard
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            // Find the Employee record whose email matches the logged-in account
            var user = await _userManager.GetUserAsync(User);
            Employee? linked = null;

            if (user != null)
            {
                var all = await _employeeService.GetEmployeesAsync(null, string.Empty);
                linked = all.FirstOrDefault(e =>
                    string.Equals(e.Email, user.Email, StringComparison.OrdinalIgnoreCase));
            }

            ViewBag.LinkedEmployee = linked;
            return View();
        }
    }
}