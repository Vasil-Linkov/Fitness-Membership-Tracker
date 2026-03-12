using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fitness_Membership_Tracker.Models
{
    public class LogVisitAdminViewModel
    {
        [Required]
        public string MemberId { get; set; }

        [Required]
        public int LocationId { get; set; }

        public int? MembershipId { get; set; }

        public IEnumerable<SelectListItem> Members { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Locations { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Memberships { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
