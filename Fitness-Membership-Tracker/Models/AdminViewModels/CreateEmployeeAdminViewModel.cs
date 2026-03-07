using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fitness_Membership_Tracker.Models.AdminViewModels
{
    public class CreateEmployeeAdminViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public DateTime HireDate { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public int? LocationId { get; set; }

        public IEnumerable<SelectListItem> Locations { get; set; }
    }
}
