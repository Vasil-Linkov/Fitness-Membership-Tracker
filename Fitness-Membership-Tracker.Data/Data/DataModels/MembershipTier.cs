using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_Membership_Tracker.Data.Data.DataModels
{
    public class MembershipTier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Tier { get; set; }

        [Required]
        public decimal MonthlyPrice { get; set; }

        [Required]
        [Range(1, 12)]
        public int Duration { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public bool IsLocationRestricted { get; set; }

        [Required, Range(1, 24)]
        public int MaxSessionsPerMonth { get; set; }   
    }
}
