using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_Membership_Tracker.Data.Data.DataModels
{
    public class Payment
    {
        public int Id { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public string PaymentMethod { get; set; }





        [ForeignKey(nameof(Employee))]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }


        [ForeignKey(nameof(Member))]
        public string MemberId { get; set; }
        public Member Member { get; set; }
    }
}
