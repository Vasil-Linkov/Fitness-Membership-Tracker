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
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }

        public string? LocationRegistered { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }


        [ForeignKey(nameof(MembershipTier))]
        public int MembershipTierId { get; set; }
        public MembershipTier MembershipTier { get; set; }

        [ForeignKey(nameof(Member))]
        public string? MemberId { get; set; }
        public Member Member { get; set; }

        public ICollection<LocationMembership> LocationMemberships { get; set; } = new List<LocationMembership>();
    }
}
