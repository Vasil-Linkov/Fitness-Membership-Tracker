using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Data.Data.DataModels;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Models;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Controllers
{
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
            var user = await _memberService.GetByNameAsync(User.Identity.Name);

            var membershipViewModel = await _membershipService.GetMembershipByMember(user);
            

            if (membershipViewModel == null)
            {
                return View(null);
            }

            return View(membershipViewModel);
        }

        public async Task<IActionResult> BuyNewMembership()
        {
            var membershipTiers = new BuyNewMembershipViewModel()
            {
                MembershipTiers = await _membershipTierService.GetTiersAsync()
			};

			return View(membershipTiers);
		}

        [HttpPost]
        public async Task<IActionResult> PurchaseMembership(int membershipTierId)
        {
            var membershipTier = await _membershipTierService.GetByIdAsync(membershipTierId);

            if (membershipTier != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var member = await _memberService.GetByIdAsync(User.Identity.Name);

				var locations = await _locationService.GetAllAsync();

                var location = locations[Random.Shared.Next(0, locations.Count)];

                var employeesAtLocation = await _employeeService.GetEmployeesAsync(location.Id, string.Empty);

                Employee employee = null;

                if (employeesAtLocation.Any())
                {
                    employee = employeesAtLocation[
                        Random.Shared.Next(0, employeesAtLocation.Count)
                    ];
                }

				var newMembership = new Membership()
				{
					MembershipTierId = membershipTier.Id,
					StartDate = DateTime.Now,
					EndDate = DateTime.Now.AddMonths(1),
					LocationId = location.Id
				};

				await _membershipService.CreateAsync(newMembership);

                var payment = new Payment()
                {
                    Currency = "EUR",
                    Amount = membershipTier.MonthlyPrice,
                    PaymentDate = DateTime.Now,
                    PaymentMethod = "OnSite",
                    MemberId = member.Id,
                    MembershipId = newMembership.Id,
                    EmployeeId = employee?.Id
                };

                await _paymentService.CreateAsync(payment);

                member.MembershipId = newMembership.Id;
                await _memberService.UpdateAsync(member);
                
            }
            else
            {
                throw new Exception("Membership Tier not found or user not authenticated.");
            }

            return Redirect("YourMembership");
        }

    }
}
