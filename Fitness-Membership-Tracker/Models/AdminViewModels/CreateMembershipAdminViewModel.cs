using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Fitness_Membership_Tracker.Data.DataModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fitness_Membership_Tracker.Models.AdminViewModels
{
    public class CreateMembershipAdminViewModel
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int LocationId { get; set; }

        [Required]
        public int MembershipTierId { get; set; }
        public int MemberId { get; set; }

        public IEnumerable<SelectListItem> Members { get; set; }

        public IEnumerable<SelectListItem> Locations { get; set; }

        public IEnumerable<SelectListItem> Tiers { get; set; }
    }
}
