using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Data.DataModels
{
	public class Membership
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public DateTime StartDate { get; set; }

		[Required]
		public DateTime EndDate { get; set; }


		[Required]
		[ForeignKey(nameof(Location))]
		public int LocationId { get; set; }

		public Location Location { get; set; }


		[Required]
		[ForeignKey(nameof(MembershipTier))]
		public int MembershipTierId { get; set; }

		public MembershipTier MembershipTier { get; set; }

		// Soft delete
		public bool IsDeleted { get; set; } = false;
		public DateTime? DeletedAt { get; set; }
	}
}