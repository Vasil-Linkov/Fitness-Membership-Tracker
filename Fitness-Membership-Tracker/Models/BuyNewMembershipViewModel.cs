using Fitness_Membership_Tracker.Data.Data.DataModels;

namespace Fitness_Membership_Tracker.Models
{
	public class BuyNewMembershipViewModel
	{

		public List<MembershipTier> MembershipTiers { get; set; } = new List<MembershipTier>();
	}
}
