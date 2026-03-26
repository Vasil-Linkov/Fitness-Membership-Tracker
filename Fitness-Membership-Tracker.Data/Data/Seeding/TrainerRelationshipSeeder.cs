using Fitness_Membership_Tracker.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Data.Seeding
{
    public static class TrainerRelationshipSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            if (await context.TrainerTrainees.AnyAsync()
             || await context.TrainingRequests.AnyAsync())
                return;

            var trainers = await context.Trainers.IgnoreQueryFilters().ToListAsync();
            var members  = await context.Members.IgnoreQueryFilters()
                               .Where(m => m.MembershipId != null && m.Email != "admin@fitzone.bg")
                               .ToListAsync();

            if (!trainers.Any() || !members.Any())
                return;

            var rnd            = new Random(33);
            var relationships  = new List<TrainerTrainee>();
            var requests       = new List<TrainingRequest>();
            var usedMemberIds  = new HashSet<string>();

            // Capacities per trainer — never exceed 5 (default max)
            var capacityUsed = trainers.ToDictionary(t => t.Id, _ => 0);

            // Assign ~10 active trainer-trainee pairs
            int targetPairs = Math.Min(10, members.Count);
            var shuffled    = members.OrderBy(_ => rnd.Next()).ToList();

            foreach (var member in shuffled)
            {
                if (relationships.Count >= targetPairs) break;

                // Pick a trainer that still has capacity
                var available = trainers
                    .Where(t => capacityUsed[t.Id] < 5)
                    .OrderBy(_ => rnd.Next())
                    .FirstOrDefault();

                if (available == null) break;

                var startedDaysAgo = rnd.Next(7, 180);

                relationships.Add(new TrainerTrainee
                {
                    TrainerId = available.Id,
                    MemberId  = member.Id,
                    StartDate = DateTime.UtcNow.AddDays(-startedDaysAgo),
                    IsActive  = true
                });

                capacityUsed[available.Id]++;
                usedMemberIds.Add(member.Id);
            }

            // Seed a handful of historical (closed) relationships for variety
            var historicalCandidates = members
                .Where(m => !usedMemberIds.Contains(m.Id))
                .OrderBy(_ => rnd.Next())
                .Take(6)
                .ToList();

            foreach (var member in historicalCandidates)
            {
                var trainer     = trainers[rnd.Next(trainers.Count)];
                var endedDaysAgo = rnd.Next(5, 60);
                var startedAgo   = rnd.Next(endedDaysAgo + 30, 270);

                relationships.Add(new TrainerTrainee
                {
                    TrainerId = trainer.Id,
                    MemberId  = member.Id,
                    StartDate = DateTime.UtcNow.AddDays(-startedAgo),
                    EndDate   = DateTime.UtcNow.AddDays(-endedDaysAgo),
                    IsActive  = false
                });

                usedMemberIds.Add(member.Id);
            }

            // Seed a few pending requests from members not currently linked
            var requestCandidates = members
                .Where(m => !usedMemberIds.Contains(m.Id))
                .OrderBy(_ => rnd.Next())
                .Take(4)
                .ToList();

            string[] memberMessages =
            {
                "Hi, I'm looking to build strength and lose some body fat. I'm a complete beginner.",
                "I've been training for about a year but feel stuck. Looking for guidance on programming.",
                "Interested in improving my running endurance alongside gym sessions.",
                "Would love help with technique — especially squats and deadlifts."
            };

            for (int i = 0; i < requestCandidates.Count; i++)
            {
                var member  = requestCandidates[i];
                var trainer = trainers.Where(t => capacityUsed[t.Id] < 5)
                                      .OrderBy(_ => rnd.Next())
                                      .FirstOrDefault()
                             ?? trainers[rnd.Next(trainers.Count)];

                requests.Add(new TrainingRequest
                {
                    MemberId      = member.Id,
                    TrainerId     = trainer.Id,
                    Status        = TrainingRequestStatus.Pending,
                    RequestedAt   = DateTime.UtcNow.AddDays(-rnd.Next(1, 5)),
                    MemberMessage = memberMessages[i % memberMessages.Length]
                });
            }

            // Also seed a couple of rejected / accepted-historical requests for request history
            var historyRequestCandidates = members
                .Where(m => usedMemberIds.Contains(m.Id))
                .OrderBy(_ => rnd.Next())
                .Take(5)
                .ToList();

            string[] trainerResponses =
            {
                "Happy to help! Let's start with an assessment session.",
                "Looking forward to working with you. See you on Monday.",
                "Unfortunately my schedule is full at the moment — please try again next month.",
                "I'd love to take you on. Let's discuss your goals in our first session.",
            };

            var statuses = new[]
            {
                TrainingRequestStatus.Accepted,
                TrainingRequestStatus.Accepted,
                TrainingRequestStatus.Rejected,
                TrainingRequestStatus.Accepted,
                TrainingRequestStatus.Rejected
            };

            for (int i = 0; i < historyRequestCandidates.Count; i++)
            {
                var member       = historyRequestCandidates[i];
                var trainer      = trainers[rnd.Next(trainers.Count)];
                var requestedAgo = rnd.Next(30, 120);

                requests.Add(new TrainingRequest
                {
                    MemberId       = member.Id,
                    TrainerId      = trainer.Id,
                    Status         = statuses[i],
                    RequestedAt    = DateTime.UtcNow.AddDays(-requestedAgo),
                    RespondedAt    = DateTime.UtcNow.AddDays(-requestedAgo + rnd.Next(1, 4)),
                    MemberMessage  = "I'm looking to improve my overall fitness.",
                    TrainerResponse = trainerResponses[i % trainerResponses.Length]
                });
            }

            if (relationships.Any())
            {
                await context.TrainerTrainees.AddRangeAsync(relationships);
                await context.SaveChangesAsync();
            }

            if (requests.Any())
            {
                await context.TrainingRequests.AddRangeAsync(requests);
                await context.SaveChangesAsync();
            }
        }
    }
}
