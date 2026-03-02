using Fitness_Membership_Tracker.Constants;
using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public AdminController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Employees(int? locationId, string search)
        {
            var employees = await _employeeService.GetEmployeesAsync(locationId, search);
            return View(employees);
        }

    }
}
