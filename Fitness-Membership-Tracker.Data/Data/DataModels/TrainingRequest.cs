using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fitness_Membership_Tracker.Data.DataModels
{
    public enum TrainingRequestStatus
    {
        Pending = 0,
        Accepted = 1,
        Rejected = 2,
        Cancelled = 3
    }

    public class TrainingRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public TrainingRequestStatus Status { get; set; } = TrainingRequestStatus.Pending;

        [Required]
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        public DateTime? RespondedAt { get; set; }

        public string? MemberMessage { get; set; }

        public string? TrainerResponse { get; set; }

        [ForeignKey(nameof(Member))]
        public string MemberId { get; set; }
        public Member Member { get; set; }

        [ForeignKey(nameof(Trainer))]
        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }
    }
}
