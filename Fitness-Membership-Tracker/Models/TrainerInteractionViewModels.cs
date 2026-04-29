using System.ComponentModel.DataAnnotations;
using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Models
{
    // ── Member-facing: browse trainers ────────────────────────────────────────

    public class BrowseTrainersViewModel
    {
        public List<TrainerCardViewModel> Trainers { get; set; } = new();
        public TrainingRequest? ActiveRequest { get; set; }
        public TrainerTrainee? CurrentRelationship { get; set; }
    }

    public class TrainerCardViewModel
    {
        public Trainer Trainer { get; set; } = null!;
        public int ActiveTrainees { get; set; }
        public int MaxTrainees { get; set; }

        /// <summary>Full weekly availability slots (all days).</summary>
        public List<TrainerSchedule> WeeklySlots { get; set; } = new();
    }

    public class RequestTrainerViewModel
    {
        public Trainer Trainer { get; set; } = null!;

        [MaxLength(500)]
        [Display(Name = "Message to trainer (optional)")]
        public string? MemberMessage { get; set; }
    }

    public class MyTrainerViewModel
    {
        public TrainerTrainee? ActiveRelationship { get; set; }
        public List<TrainingRequest> RequestHistory { get; set; } = new();
    }

    // ── Member-facing: training sessions ─────────────────────────────────────

    public class MySessionsViewModel
    {
        public List<TrainingSession> Upcoming { get; set; } = new();
        public List<TrainingSession> Past { get; set; } = new();
    }

    // ── Trainer-facing: dashboard ─────────────────────────────────────────────

    public class TrainerDashboardViewModel
    {
        public Trainer Trainer { get; set; } = null!;
        public List<TrainingRequest> PendingRequests { get; set; } = new();
        public List<TrainerTrainee> ActiveTrainees { get; set; } = new();
        public int MaxTrainees { get; set; }
        public int CurrentCount { get; set; }
        public List<TrainerSchedule> Schedule { get; set; } = new();
    }

    public class RespondToRequestViewModel
    {
        public TrainingRequest Request { get; set; } = null!;

        [MaxLength(500)]
        [Display(Name = "Response message (optional)")]
        public string? TrainerResponse { get; set; }
    }

    // ── Trainer-facing: schedule page ────────────────────────────────────────

    public class TrainerSchedulePageViewModel
    {
        public Trainer Trainer { get; set; } = null!;
        public List<TrainerSchedule> AvailabilitySlots { get; set; } = new();
        public List<TrainingSession> Sessions { get; set; } = new();
    }

    public class BookSessionViewModel
    {
        [Required]
        public int TrainerId { get; set; }

        [Required]
        public string MemberId { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Session Date & Time")]
        public DateTime SessionDate { get; set; } = DateTime.Today.AddDays(1).AddHours(9);

        [Required, Range(15, 480)]
        [Display(Name = "Duration (minutes)")]
        public int DurationMinutes { get; set; } = 60;

        [MaxLength(500)]
        [Display(Name = "Session Notes / Focus Area")]
        public string? Notes { get; set; }

        // Populated for the view — not bound from the form
        public Trainer? Trainer { get; set; }
        public List<TrainerTrainee> ActiveTrainees { get; set; } = new();
    }

    // ── Admin-facing: trainer management ─────────────────────────────────────

    public class UpdateTrainerCapacityViewModel
    {
        public int TrainerId { get; set; }
        public string TrainerName { get; set; } = string.Empty;

        [Required, Range(1, 100)]
        [Display(Name = "Max Trainees")]
        public int MaxTrainees { get; set; }

        public int CurrentCount { get; set; }
    }

    public class ManageTrainerScheduleViewModel
    {
        public int TrainerId { get; set; }
        public string TrainerName { get; set; } = string.Empty;
        public List<TrainerSchedule> ExistingSlots { get; set; } = new();

        [Required, Range(0, 6)]
        [Display(Name = "Day of Week")]
        public int DayOfWeek { get; set; }

        [Required]
        [Display(Name = "Start Time")]
        public TimeSpan StartTime { get; set; } = new TimeSpan(9, 0, 0);

        [Required]
        [Display(Name = "End Time")]
        public TimeSpan EndTime { get; set; } = new TimeSpan(17, 0, 0);
    }
}