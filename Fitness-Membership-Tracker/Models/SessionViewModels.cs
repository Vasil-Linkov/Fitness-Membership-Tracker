using System.ComponentModel.DataAnnotations;
using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Models
{
    // ── Trainer browse (member-facing) ────────────────────────────────────────

    /// <summary>
    /// Replaces the old BrowseTrainersViewModel — now carries the full weekly
    /// schedule for every trainer so members can see all available slots.
    /// </summary>
    public class BrowseTrainersViewModel
    {
        public List<TrainerCardViewModel> Trainers { get; set; } = new();
        public TrainingRequest?   ActiveRequest      { get; set; }
        public TrainerTrainee?    CurrentRelationship { get; set; }
    }

    public class TrainerCardViewModel
    {
        public Trainer Trainer { get; set; } = null!;
        public int ActiveTrainees { get; set; }
        public int MaxTrainees    { get; set; }

        /// <summary>Full weekly schedule, grouped by day for display.</summary>
        public List<TrainerSchedule> WeeklySlots { get; set; } = new();
    }

    // ── Session booking (trainer-facing) ──────────────────────────────────────

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

        // Populated for the view
        public Trainer? Trainer { get; set; }
        public List<TrainerTrainee> ActiveTrainees { get; set; } = new();
    }

    // ── Trainer schedule page ─────────────────────────────────────────────────

    public class TrainerSchedulePageViewModel
    {
        public Trainer Trainer { get; set; } = null!;

        /// <summary>Availability slots the admin configured.</summary>
        public List<TrainerSchedule> AvailabilitySlots { get; set; } = new();

        /// <summary>Booked sessions (upcoming + recent past).</summary>
        public List<TrainingSession> Sessions { get; set; } = new();
    }

    // ── Member's session history ──────────────────────────────────────────────

    public class MySessionsViewModel
    {
        public List<TrainingSession> Upcoming { get; set; } = new();
        public List<TrainingSession> Past      { get; set; } = new();
    }
}
