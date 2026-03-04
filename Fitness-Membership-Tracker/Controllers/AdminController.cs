using Fitness_Membership_Tracker.Constants;
using Fitness_Membership_Tracker.Data.Data.DataModels;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fitness_Membership_Tracker.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    public class AdminController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMemberService _memberService;
        private readonly IMembershipService _membershipService;
        private readonly IPaymentService _paymentService;
        private readonly ILocationService _locationService;

        public AdminController(
            IEmployeeService employeeService,
            IMemberService memberService,
            IMembershipService membershipService,
            IPaymentService paymentService,
            ILocationService locationService)
        {
            _employeeService = employeeService;
            _memberService = memberService;
            _membershipService = membershipService;
            _paymentService = paymentService;
            _locationService = locationService;
        }

        public async Task<IActionResult> Dashboard()
        {
            var employees = await _employeeService.GetEmployeesAsync(null, null);
            var members = await _memberService.GetAllAsync();
            var memberships = await _membershipService.GetAllAsync();
            var payments = await _paymentService.GetAllAsync();

            ViewBag.EmployeeCount = employees.Count;
            ViewBag.MemberCount = members.Count;
            ViewBag.MembershipCount = memberships.Count;
            ViewBag.PaymentCount = payments.Count;

            return View();
        }

        #region Employees

        public async Task<IActionResult> Employees(int? locationId, string search)
        {
            var employees = await _employeeService.GetEmployeesAsync(locationId, search);
            ViewBag.Locations = new SelectList(await _locationService.GetAllAsync(), "Id", "Name");
            return View(employees);
        }

        public async Task<IActionResult> CreateEmployee()
        {
            ViewBag.Locations = new SelectList(await _locationService.GetAllAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(Employee employee)
        {
            if (!ModelState.IsValid)
                return View(employee);

            await _employeeService.CreateAsync(employee);
            return RedirectToAction(nameof(Employees));
        }

        public async Task<IActionResult> DeleteEmployee(int id)
        {
            await _employeeService.DeleteAsync(id);
            return RedirectToAction(nameof(Employees));
        }

        #endregion

        #region Members

        public async Task<IActionResult> Members()
        {
            var members = await _memberService.GetAllAsync();
            return View(members);
        }

        public async Task<IActionResult> DeleteMember(string id)
        {
            await _memberService.DeleteAsync(id);
            return RedirectToAction(nameof(Members));
        }

        #endregion

    }
}