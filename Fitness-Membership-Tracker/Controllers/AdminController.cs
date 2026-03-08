using Fitness_Membership_Tracker.Constants;
using Fitness_Membership_Tracker.HelperClasses;
using Fitness_Membership_Tracker.Models.AdminViewModels;
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
		private readonly IMembershipTierService _membershipTierService;

		public AdminController(
			IEmployeeService employeeService,
			IMemberService memberService,
			IMembershipService membershipService,
			IPaymentService paymentService,
			ILocationService locationService,
			IMembershipTierService membershipTierService)
		{
			_employeeService = employeeService;
			_memberService = memberService;
			_membershipService = membershipService;
			_paymentService = paymentService;
			_locationService = locationService;
			_membershipTierService = membershipTierService;
		}

		[HttpGet]
		public async Task<IActionResult> Dashboard()
		{
			ViewBag.EmployeeCount = (await _employeeService.GetEmployeesAsync(null, string.Empty)).Count();
			ViewBag.MemberCount = (await _memberService.GetAllAsync()).Count();
			ViewBag.MembershipCount = (await _membershipService.GetAllAsync()).Count();
			ViewBag.PaymentCount = (await _paymentService.GetAllAsync()).Count();

			return View();
		}

		#region Employees

		[HttpGet]
		public async Task<IActionResult> Employees(int? locationId, string? search)
		{
			if(search == null)
				search = string.Empty;

			ViewBag.SelectedLocationId = locationId;
			ViewBag.Search = search;
			ViewBag.Locations = await GetLocations();

			var employees = await _employeeService.GetEmployeesAsync(locationId, search);

			return View(employees);
		}

		[HttpGet]
		public async Task<IActionResult> CreateEmployee()
		{
			var model = new CreateEmployeeAdminViewModel
			{
				Locations = await GetLocations()
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateEmployee(CreateEmployeeAdminViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Locations = await GetLocations();
				return View(model);
			}
			model.Email = model.FirstName + "." + model.LastName + "@gym.com";
			await _employeeService.CreateAsync(EmployeeMapper.ToEntity(model));

			return RedirectToAction(nameof(Employees));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteEmployee(int id)
		{
			await _employeeService.DeleteAsync(id);

			return RedirectToAction(nameof(Employees));
		}

		#endregion


		#region Members

		[HttpGet]
		public async Task<IActionResult> Members()
		{
			var members = await _memberService.GetAllAsync();
			return View(members);
		}

		#endregion


		#region Memberships

		[HttpGet]
		public async Task<IActionResult> Memberships()
		{
			var memberships = await _membershipService.GetAllAsync();
			return View(memberships);
		}

		[HttpGet]
		public async Task<IActionResult> CreateMembership()
		{
			var model = new CreateMembershipAdminViewModel
			{
				StartDate = DateTime.Now,
				EndDate = DateTime.Now.AddMonths(1),
				Locations = await GetLocations(),
				Tiers = await GetTiers(),
				Members = await GetMembers()
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateMembership(CreateMembershipAdminViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Locations = await GetLocations();
				model.Tiers = await GetTiers();
				model.Members = await GetMembers();
				return View(model);
			}

			await _membershipService.CreateAsync(MembershipMapper.ToEntity(model));

			return RedirectToAction(nameof(Memberships));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteMembership(int id)
		{
			await _membershipService.DeleteAsync(id);

			return RedirectToAction(nameof(Memberships));
		}

		#endregion


		#region Payments

		[HttpGet]
		public async Task<IActionResult> Payments()
		{
			var payments = await _paymentService.GetAllAsync();
			return View(payments);
		}

		[HttpGet]
		public async Task<IActionResult> CreatePayment()
		{
			var model = new CreatePaymentAdminViewModel
			{
				PaymentDate = DateTime.Now,
				Employees = await GetEmployees(),
				Members = await GetMembers(),
				Memberships = await GetMemberships()
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreatePayment(CreatePaymentAdminViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Employees = await GetEmployees();
				model.Members = await GetMembers();
				model.Memberships = await GetMemberships();
				return View(model);
			}

			await _paymentService.CreateAsync(PaymentMapper.ToEntity(model));

			return RedirectToAction(nameof(Payments));
		}

		#endregion


		/* Since dropdown menues are used in many selections,
		these helper methods are used to create SelectList
		for every dropdown in the admin views
		to make the code more readable.
		(since i constantly got confused even while writing it)*/
		#region Dropdown Helpers

		private async Task<IEnumerable<SelectListItem>> GetLocations()
		{
			return (await _locationService.GetAllAsync())
				.Select(location => new SelectListItem
				{
					Value = location.Id.ToString(),
					Text = $"{location.City} - {location.Address}"
				});
		}

		private async Task<IEnumerable<SelectListItem>> GetTiers()
		{
			return (await _membershipTierService.GetTiersAsync())
				.Select(tier => new SelectListItem
				{
					Value = tier.Id.ToString(),
					Text = tier.Tier
				});
		}

		private async Task<IEnumerable<SelectListItem>> GetMembers()
		{
			return (await _memberService.GetAllAsync())
				.Select(member => new SelectListItem
				{
					Value = member.Id,
					Text = member.Email
				});
		}

		private async Task<IEnumerable<SelectListItem>> GetEmployees()
		{
			return (await _employeeService.GetEmployeesAsync(null, string.Empty))
				.Select(employee => new SelectListItem
				{
					Value = employee.Id.ToString(),
					Text = employee.Email
				});
		}

		private async Task<IEnumerable<SelectListItem>> GetMemberships()
		{
			return (await _membershipService.GetAllAsync())
				.Select(membership => new SelectListItem
				{
					Value = membership.Id.ToString(),
					Text = $"Membership #{membership.Id}"
				});
		}

		#endregion
	}
}