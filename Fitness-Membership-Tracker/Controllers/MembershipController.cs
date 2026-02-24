using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Data.Data.DataModels;
using Fitness_Membership_Tracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Controllers
{
    public class MembershipController : Controller
    {
        ApplicationDbContext _context;

        public MembershipController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> YourMembership()
        {

            var membershipViewModel = await _context.Members
                .Include(m => m.Membership)
                .ThenInclude(m => m.Member)
                .Where(m => m.UserName == User.Identity.Name)
                .Select(m => new YourMembershipViewModel
                {
                    Membership = m.Membership
                })
                .FirstOrDefaultAsync(); 

            if (membershipViewModel == null)
            {
                return View(null);
            }

            return View(membershipViewModel);
        }

        public IActionResult BuyNewMembership()
        {
            var membershipTiers = new BuyNewMembershipViewModel()
            {
                MembershipTiers = _context.MembershipTiers.ToList()
			};

			return View(membershipTiers);
		}

        [HttpPost]
        public IActionResult PurchaseMembership(int membershipTierId)
        {
            var membershipTier = _context.MembershipTiers.Find(membershipTierId);

            if (membershipTier != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
                var member = _context.Members
                    .FirstOrDefault(m => m.UserName == User.Identity.Name);

                var locations = _context.Locations.ToList();

                var location = locations[Random.Shared.Next(0, locations.Count)];

                var employeesAtLocation = _context.Employees
                    .Where(e => e.LocationId == location.Id)
                    .ToList();

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
                    LocationId = location.Id,
                    MemberId = member.Id
                };

                _context.Memberships.Add(newMembership);
                _context.SaveChanges();

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

                _context.Payments.Add(payment);
                _context.SaveChanges();

                member.MembershipId = newMembership.Id;

                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Membership Tier not found or user not authenticated.");
            }

            return Redirect("YourMembership");
        }

    }
}
