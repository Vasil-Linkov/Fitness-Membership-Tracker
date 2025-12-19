using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_Membership_Tracker.Data.Data.DataModels
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Address { get; set; }

        public ICollection<LocationMembership> LocationMemberships { get; set; } = new List<LocationMembership>();
    }
}
