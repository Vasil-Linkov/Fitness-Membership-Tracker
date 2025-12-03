using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_Membership_Tracker.Data.Data.DataModels
{
    public class Membership
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public MembershipTier Tier { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        public decimal Price { get; set; }

        public string? LocationRegistered { get; set; }


        [ForeignKey(nameof(Member))]
        public string? MemberId { get; set; }
        public Member Member { get; set; }

        public ICollection<LocationMembership> LocationMemberships { get; set; } = new List<LocationMembership>();
    }
}
