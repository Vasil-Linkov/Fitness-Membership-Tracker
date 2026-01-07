using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fitness_Membership_Tracker.Controllers
{
    public class MembershipController : Controller
    {
        ApplicationDbContext _context;

        public MembershipController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult YourMembership()
        {

            
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var memberships = _context.Members
                .Where(m => m.UserName == User.Identity.Name)
                .Select(m => new YourMembershipViewModel()
                {
                    Membership = m.Membership
                })
                .ToList();

                if (memberships.Count < 1)
                {
					return View();
				}
                else
                {
                    return View(memberships[0]);
                }

            }
             
            return View();
        }

		public IActionResult BuyNewMembership()
        {
            var membershipTiers = new BuyNewMembershipViewModel()
            {
                MembershipTiers = _context.MembershipTiers.ToList()
			};

			return View(membershipTiers);
		}

        public IActionResult PurchaseMembership(int id)
        {
            var membershipTier = _context.MembershipTiers.Find(id);

            if (membershipTier != null && User.Identity != null && User.Identity.IsAuthenticated)
            {
				var member = _context.Members
					.FirstOrDefault(m => m.UserName == User.Identity.Name);




				_context.Memberships.Add(new Data.Data.DataModels.Membership()
                {
					MembershipTier = membershipTier,
					StartDate = DateTime.Now,
					EndDate = DateTime.Now.AddMonths(1),
					
				});
			}

            return View("YourMembership");
        }
	}
}
