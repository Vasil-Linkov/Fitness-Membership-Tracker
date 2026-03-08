using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Models.AdminViewModels;

namespace Fitness_Membership_Tracker.HelperClasses
{
    public static class MembershipMapper
    {

		public static Membership ToEntity(CreateMembershipAdminViewModel vm)
		{
			return new Membership
			{
				StartDate = vm.StartDate,
				EndDate = vm.EndDate,
				LocationId = vm.LocationId,
				MembershipTierId = vm.MembershipTierId
			};
		}
	}
}
