using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_Membership_Tracker.Data.Data.DataModels
{
    public class LocationMembership
    {
        [ForeignKey(nameof(LocationId))]
        public int LocationId { get; set; }
        public Location Location { get; set; }


        [ForeignKey(nameof(MembershipId))]
        public int MembershipId { get; set; }
        public Membership Membership { get; set; }
    }
}
