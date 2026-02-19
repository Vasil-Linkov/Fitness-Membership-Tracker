using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Fitness_Membership_Tracker.Data.Data.DataModels
{
    public class Member : IdentityUser
    {
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }


        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

        [ForeignKey(nameof(Membership))]
        public int? MembershipId { get; set; }   
        public Membership Membership { get; set; } = null;
    }
}
