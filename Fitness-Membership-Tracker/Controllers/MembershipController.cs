using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Models;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness_Membership_Tracker.Controllers
{
	[Authorize]
	public class MembershipController : Controller
	{
		private readonly IMemberService _memberService;
		private readonly IMembershipService _membershipService;
		private readonly IMembershipTierService _membershipTierService;
		private readonly ILocationService _locationService;
		private readonly IEmployeeService _employeeService;
		private readonly IPaymentService _paymentService;

		public MembershipController(
			IMembershipService membershipService,
			IMemberService memberService,
			IMembershipTierService membershipTierService,
			ILocationService locationService,
			IEmployeeService employeeService,
			IPaymentService paymentService)
		{
			_membershipService = membershipService;
			_memberService = memberService;
			_membershipTierService = membershipTierService;
			_locationService = locationService;
			_employeeService = employeeService;
			_paymentService = paymentService;
		}

		[HttpGet]
		public async Task<IActionResult> YourMembership()
		{
			var member = await _memberService.GetByNameAsync(User.Identity.Name);

			if (member == null)
			{
				return View(new YourMembershipViewModel());
			}

			var membership = await _membershipService.GetMembershipByMember(member);

			var model = new YourMembershipViewModel
			{
				Membership = membership
			};

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> BuyNewMembership()
		{
			var model = new BuyNewMembershipViewModel
			{
				MembershipTiers = await _membershipTierService.GetTiersAsync()
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> PurchaseMembership(int membershipTierId)
		{
			var membershipTier = await _membershipTierService.GetByIdAsync(membershipTierId);
			var member = await _memberService.GetByNameAsync(User.Identity.Name);
			var locations = await _locationService.GetAllAsync();
			var location = locations[Random.Shared.Next(locations.Count)];

			var employeesAtLocation = await _employeeService.GetEmployeesAsync(location.Id, string.Empty);

			Employee? employee = null;
			if (employeesAtLocation.Any())
			{
				employee = employeesAtLocation[Random.Shared.Next(employeesAtLocation.Count)];
			}

			Membership membership;

			int? membershipId = member.MembershipId;

			Membership? existingMembership;

			if (membershipId == null)
				existingMembership = null;
			else
				existingMembership = await _membershipService.GetByIdIncludingDeletedAsync(membershipId.Value);


			if (existingMembership == null)
			{
				membership = new Membership
				{
					MembershipTierId = membershipTier.Id,
					StartDate = DateTime.Now,
					EndDate = DateTime.Now.AddMonths(1),
					LocationId = location.Id,
					IsDeleted = false,
					DeletedAt = null
				};

				await _membershipService.CreateAsync(membership);

				member.MembershipId = membership.Id;
				await _memberService.UpdateAsync(member);
			}
			else
			{
				existingMembership.MembershipTierId = membershipTier.Id;
				existingMembership.LocationId = location.Id;

				if (existingMembership.IsDeleted || existingMembership.EndDate < DateTime.Now)
				{
					existingMembership.IsDeleted = false;
					existingMembership.DeletedAt = null;
					existingMembership.StartDate = DateTime.Now;
					existingMembership.EndDate = DateTime.Now.AddMonths(1);
				}
				else
				{
					existingMembership.EndDate = existingMembership.EndDate.AddMonths(1);
				}

				await _membershipService.UpdateAsync(existingMembership);
				membership = existingMembership;
			}

			var payment = new Payment
			{
				Currency = "EUR",
				Amount = membershipTier.MonthlyPrice,
				PaymentDate = DateTime.Now,
				PaymentMethod = "OnSite",
				MemberId = member.Id,
				MembershipId = membership.Id,
				EmployeeId = employee?.Id
			};

			await _paymentService.CreateAsync(payment);

			return RedirectToAction(nameof(YourMembership));
		}
	}
}