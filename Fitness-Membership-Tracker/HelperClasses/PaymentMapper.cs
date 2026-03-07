using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Models.AdminViewModels;

namespace Fitness_Membership_Tracker.HelperClasses
{
    public static class PaymentMapper
    {
        public static Payment ToEntity(CreatePaymentAdminViewModel vm)
        {
            return new Payment
            {
                Currency = vm.Currency,
                Amount = vm.Amount,
                PaymentDate = vm.PaymentDate,
                PaymentMethod = vm.PaymentMethod,
                EmployeeId = vm.EmployeeId,
                MemberId = vm.MemberId,
                MembershipId = vm.MembershipId
            };
        }
    }
}
