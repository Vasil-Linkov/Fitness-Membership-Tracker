using Fitness_Membership_Tracker.Data.DataModels;

namespace Fitness_Membership_Tracker.Models
{
    public class VisitStatsViewModel
    {
        public Dictionary<DateTime, int> DailyVisitCounts { get; set; } = new();

        public DateTime From { get; set; }
        public DateTime To { get; set; }

        public int TotalVisits { get; set; }
        public double AveragePerDay { get; set; }
        public int PeakCount { get; set; }
        public DateTime? PeakDay { get; set; }

        public List<Visit>? Visits { get; set; }
    }
}
