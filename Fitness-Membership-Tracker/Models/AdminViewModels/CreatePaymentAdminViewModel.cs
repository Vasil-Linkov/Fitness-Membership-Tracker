using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fitness_Membership_Tracker.Models.AdminViewModels
{
    public class CreatePaymentAdminViewModel
    {
        [Required]
        public string Currency { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        public int? EmployeeId { get; set; }

        public string? MemberId { get; set; }

        public int? MembershipId { get; set; }

        public IEnumerable<SelectListItem> Employees { get; set; }

        public IEnumerable<SelectListItem> Members { get; set; }

        public IEnumerable<SelectListItem> Memberships { get; set; }
    }
}
