using System.ComponentModel.DataAnnotations;
using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Models
{
    public class LogWorkoutViewModel
    {
        [Required]
        [Display(Name = "General Notes (optional)")]
        public string Notes { get; set; } = string.Empty;

        [Required, MinLength(1, ErrorMessage = "Please add at least one exercise.")]
        public List<WorkoutExerciseInputModel> Exercises { get; set; } = new()
        {
            new WorkoutExerciseInputModel()
        };
    }

    public class WorkoutExerciseInputModel
    {
        [Required(ErrorMessage = "Exercise name is required.")]
        [Display(Name = "Exercise")]
        public string ExerciseName { get; set; } = string.Empty;

        [Range(1, 100)]
        public int? Sets { get; set; }

        [Range(1, 10000)]
        public int? Reps { get; set; }

        [Range(0, 9999)]
        [Display(Name = "Weight (kg)")]
        public decimal? WeightKg { get; set; }

        [Range(1, 600)]
        [Display(Name = "Duration (min)")]
        public int? DurationMinutes { get; set; }

        public string? Notes { get; set; }
    }

    public class WorkoutHistoryViewModel
    {
        public List<WorkoutLog> Logs { get; set; } = new();
    }
}
