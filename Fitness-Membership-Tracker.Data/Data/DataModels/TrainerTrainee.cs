using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness_Membership_Tracker.Data.DataModels
{
    public class TrainerTrainee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;

        public string? Notes { get; set; }

        [ForeignKey(nameof(Trainer))]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        [ForeignKey(nameof(Member))]
        public string MemberId { get; set; }
        public Member Member { get; set; }
    }


    public class TrainerCapacity
    {
        [Key]
        public int TrainerId { get; set; }

        [Required, Range(1, 100)]
        public int MaxTrainees { get; set; } = 5;

        public Trainer Trainer { get; set; }
    }
}
