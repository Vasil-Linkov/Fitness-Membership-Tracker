using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness_Membership_Tracker.Data.DataModels
{
    /// <summary>
    /// A concrete booked session between a trainer and one of their trainees.
    /// Multiple sessions per week are supported — just create one row per session.
    /// </summary>
    public class TrainingSession
    {
        [Key]
        public int Id { get; set; }

        /// <summary>Date and start time of the session (UTC).</summary>
        [Required]
        public DateTime SessionDate { get; set; }

        /// <summary>Duration in minutes.</summary>
        [Required, Range(15, 480)]
        public int DurationMinutes { get; set; } = 60;

        /// <summary>Optional notes / focus area for the session.</summary>
        [MaxLength(500)]
        public string? Notes { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        [ForeignKey(nameof(Trainer))]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = null!;

        [ForeignKey(nameof(Member))]
        public string MemberId { get; set; } = null!;
        public Member Member { get; set; } = null!;
    }
}
