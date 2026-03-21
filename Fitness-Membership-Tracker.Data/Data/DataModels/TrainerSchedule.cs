using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness_Membership_Tracker.Data.DataModels
{
    public class TrainerSchedule
    {
        [Key]
        public int Id { get; set; }


        [Required, Range(0, 6)]
        public int DayOfWeek { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        public bool IsBlocked { get; set; } = false;

        [ForeignKey(nameof(Trainer))]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }
    }
}
