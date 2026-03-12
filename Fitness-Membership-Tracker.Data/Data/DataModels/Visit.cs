using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness_Membership_Tracker.Data.DataModels
{
    public class Visit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime VisitDate { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        [ForeignKey(nameof(Member))]
        public string MemberId { get; set; }
        public Member Member { get; set; }

        [ForeignKey(nameof(Location))]
        public int LocationId { get; set; }
        public Location Location { get; set; }

        [ForeignKey(nameof(Membership))]
        public int? MembershipId { get; set; }
        public Membership Membership { get; set; }
    }
}




