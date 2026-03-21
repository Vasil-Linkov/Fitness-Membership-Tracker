using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness_Membership_Tracker.Data.DataModels
{
    public class WorkoutLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime LogDate { get; set; }

        [Required]
        public string Notes { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        [ForeignKey(nameof(Member))]
        public string MemberId { get; set; }
        public Member Member { get; set; }

        public ICollection<WorkoutExercise> Exercises { get; set; } = new List<WorkoutExercise>();
    }
}
