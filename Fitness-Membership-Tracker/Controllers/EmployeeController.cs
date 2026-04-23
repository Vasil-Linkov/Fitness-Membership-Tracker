using Fitness_Membership_Tracker.Constants;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Fitness_Membership_Tracker.Data.DataModels;

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
            _userManager     = userManager;
        }

        
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            // Attempt to find the linked Employee record by matching email
            var employees = await _employeeService.GetEmployeesAsync(null, User.Identity!.Name ?? string.Empty);
            var linkedEmployee = employees.FirstOrDefault(e =>
                e.Email.Equals(User.Identity!.Name, StringComparison.OrdinalIgnoreCase));

            ViewBag.LinkedEmployee = linkedEmployee;
            return View();
        }
    }
}
