using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness_Membership_Tracker.Data.DataModels
{
    public class WorkoutExercise
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ExerciseName { get; set; } = string.Empty;

        public int? Sets { get; set; }

        public int? Reps { get; set; }

        public decimal? WeightKg { get; set; }

        public int? DurationMinutes { get; set; }

        public string? Notes { get; set; }

        [ForeignKey(nameof(WorkoutLog))]
        public int WorkoutLogId { get; set; }
        public WorkoutLog WorkoutLog { get; set; }
    }
}
