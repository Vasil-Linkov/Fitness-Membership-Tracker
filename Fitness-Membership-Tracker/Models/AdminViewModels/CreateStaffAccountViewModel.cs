using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fitness_Membership_Tracker.Models.AdminViewModels
{
    public class CreateStaffAccountViewModel
    {
        [Required]
        [Display(Name = "Staff Role")]
        public string Role { get; set; } = string.Empty;

        // Used when Role = "Trainer"
        [Display(Name = "Link to Trainer Profile")]
        public int? TrainerId { get; set; }

        // Used when Role = "Employee"
        [Display(Name = "Link to Employee Profile")]
        public int? EmployeeId { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Login Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        // Dropdown data
        public IEnumerable<SelectListItem> Trainers { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> Employees { get; set; } = Enumerable.Empty<SelectListItem>();
    }
}
