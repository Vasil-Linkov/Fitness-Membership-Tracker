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

        [ForeignKey(nameof(Payment))]   
        public int? PaymentId { get; set; }
        public Payment Payment { get; set; }

        [ForeignKey(nameof(Membership))]
        public int? MembershipId { get; set; }   
        public Membership Membership { get; set; } = null;
    }
}
