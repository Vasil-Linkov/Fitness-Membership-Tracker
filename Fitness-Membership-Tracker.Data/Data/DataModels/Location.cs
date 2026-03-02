using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_Membership_Tracker.Data.DataModels
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();

        public ICollection<LocationMembership> LocationMemberships { get; set; } = new List<LocationMembership>();
    }
}
