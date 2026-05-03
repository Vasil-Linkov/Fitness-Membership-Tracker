using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Implementations;
using FluentAssertions;
using NUnit.Framework;

namespace FitnessTracker.Tests.Services;

[TestFixture]
[Category("UnitTests")]
public class TrainingRequestServiceTests
{
    // ── factory helpers ───────────────────────────────────────────────────────

    private static (TrainingRequest request, TrainerTraineeService traineeService)
        BuildRequest(
            Fitness_Membership_Tracker.Data.ApplicationDbContext ctx,
            int trainerId,
            string memberId,
            TrainingRequestStatus status = TrainingRequestStatus.Pending)
    {
        var request = new TrainingRequest
        {
            TrainerId   = trainerId,
            MemberId    = memberId,
            Status      = status,
            RequestedAt = DateTime.UtcNow.AddHours(-1)
        };
        ctx.TrainingRequests.Add(request);
        ctx.SaveChanges();

        var traineeService = new TrainerTraineeService(ctx);
        return (request, traineeService);
    }

    // ── GetPendingForTrainerAsync ─────────────────────────────────────────────

    [Test]
    public async Task GetPendingForTrainerAsync_ReturnsOnlyPendingRequests()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var m1        = DbContextFactory.SeedMember(ctx, "a@t.com");
        var m2        = DbContextFactory.SeedMember(ctx, "b@t.com");
        var m3        = DbContextFactory.SeedMember(ctx, "c@t.com");

        ctx.TrainingRequests.Add(new TrainingRequest
            { TrainerId = trainer.Id, MemberId = m1.Id, Status = TrainingRequestStatus.Pending,
              RequestedAt = DateTime.UtcNow });
        ctx.TrainingRequests.Add(new TrainingRequest
            { TrainerId = trainer.Id, MemberId = m2.Id, Status = TrainingRequestStatus.Accepted,
              RequestedAt = DateTime.UtcNow });
        ctx.TrainingRequests.Add(new TrainingRequest
            { TrainerId = trainer.Id, MemberId = m3.Id, Status = TrainingRequestStatus.Rejected,
              RequestedAt = DateTime.UtcNow });
        ctx.SaveChanges();

        var traineeService = new TrainerTraineeService(ctx);
        var svc    = new TrainingRequestService(ctx, traineeService);
        var result = await svc.GetPendingForTrainerAsync(trainer.Id);

        result.Should().HaveCount(1);
        result.First().MemberId.Should().Be(m1.Id);
    }

    [Test]
    public async Task GetPendingForTrainerAsync_ReturnsEmpty_WhenNoPendingRequests()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        var traineeService = new TrainerTraineeService(ctx);
        var svc    = new TrainingRequestService(ctx, traineeService);
        var result = await svc.GetPendingForTrainerAsync(trainer.Id);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetPendingForTrainerAsync_IsOrderedDescendingByRequestedAt()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var m1        = DbContextFactory.SeedMember(ctx, "older@t.com");
        var m2        = DbContextFactory.SeedMember(ctx, "newer@t.com");

        ctx.TrainingRequests.Add(new TrainingRequest
            { TrainerId = trainer.Id, MemberId = m1.Id, Status = TrainingRequestStatus.Pending,
              RequestedAt = DateTime.UtcNow.AddHours(-5) });
        ctx.TrainingRequests.Add(new TrainingRequest
            { TrainerId = trainer.Id, MemberId = m2.Id, Status = TrainingRequestStatus.Pending,
              RequestedAt = DateTime.UtcNow.AddHours(-1) });
        ctx.SaveChanges();

        var traineeService = new TrainerTraineeService(ctx);
        var svc    = new TrainingRequestService(ctx, traineeService);
        var result = await svc.GetPendingForTrainerAsync(trainer.Id);

        result.First().MemberId.Should().Be(m2.Id);
    }

    // ── GetByMemberIdAsync ────────────────────────────────────────────────────

    [Test]
    public async Task GetByMemberIdAsync_ReturnsAllStatusesForMember()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.TrainingRequests.Add(new TrainingRequest
            { TrainerId = trainer.Id, MemberId = member.Id, Status = TrainingRequestStatus.Pending,
              RequestedAt = DateTime.UtcNow });
        ctx.TrainingRequests.Add(new TrainingRequest
            { TrainerId = trainer.Id, MemberId = member.Id, Status = TrainingRequestStatus.Rejected,
              RequestedAt = DateTime.UtcNow.AddDays(-3) });
        ctx.SaveChanges();

        var traineeService = new TrainerTraineeService(ctx);
        var svc    = new TrainingRequestService(ctx, traineeService);
        var result = await svc.GetByMemberIdAsync(member.Id);

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task GetByMemberIdAsync_DoesNotReturnOtherMembersRequests()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var m1        = DbContextFactory.SeedMember(ctx, "m1@t.com");
        var m2        = DbContextFactory.SeedMember(ctx, "m2@t.com");

        ctx.TrainingRequests.Add(new TrainingRequest
            { TrainerId = trainer.Id, MemberId = m1.Id, Status = TrainingRequestStatus.Pending,
              RequestedAt = DateTime.UtcNow });
        ctx.SaveChanges();

        var traineeService = new TrainerTraineeService(ctx);
        var svc    = new TrainingRequestService(ctx, traineeService);
        var result = await svc.GetByMemberIdAsync(m2.Id);

        result.Should().BeEmpty();
    }

    // ── GetByIdAsync ──────────────────────────────────────────────────────────

    [Test]
    public async Task GetByIdAsync_ReturnsRequest()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var (request, traineeService) = BuildRequest(ctx, trainer.Id, member.Id);

        var svc    = new TrainingRequestService(ctx, traineeService);
        var result = await svc.GetByIdAsync(request.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(request.Id);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        using var ctx      = DbContextFactory.Create();
        var traineeService = new TrainerTraineeService(ctx);
        var svc            = new TrainingRequestService(ctx, traineeService);

        var result = await svc.GetByIdAsync(9999);

        result.Should().BeNull();
    }

    // ── CreateAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task CreateAsync_PersistsRequestWithPendingStatus()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var request = new TrainingRequest
        {
            TrainerId     = trainer.Id,
            MemberId      = member.Id,
            MemberMessage = "Hello trainer!"
        };

        var traineeService = new TrainerTraineeService(ctx);
        var svc            = new TrainingRequestService(ctx, traineeService);
        await svc.CreateAsync(request);

        ctx.TrainingRequests.Should().HaveCount(1);
        ctx.TrainingRequests.First().Status.Should().Be(TrainingRequestStatus.Pending);
    }

    [Test]
    public async Task CreateAsync_SetsRequestedAtToUtcNow()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var before  = DateTime.UtcNow;
        var request = new TrainingRequest { TrainerId = trainer.Id, MemberId = member.Id };

        var traineeService = new TrainerTraineeService(ctx);
        var svc            = new TrainingRequestService(ctx, traineeService);
        await svc.CreateAsync(request);

        var after = DateTime.UtcNow;
        ctx.TrainingRequests.First().RequestedAt
            .Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }

    // ── AcceptAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task AcceptAsync_ChangesStatusToAccepted()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 5 });
        var (request, traineeService) = BuildRequest(ctx, trainer.Id, member.Id);

        var svc = new TrainingRequestService(ctx, traineeService);
        await svc.AcceptAsync(request.Id, "Welcome aboard!");

        var updated = ctx.TrainingRequests.First(r => r.Id == request.Id);
        updated.Status.Should().Be(TrainingRequestStatus.Accepted);
        updated.TrainerResponse.Should().Be("Welcome aboard!");
        updated.RespondedAt.Should().NotBeNull();
    }

    [Test]
    public async Task AcceptAsync_CreatesTrainerTraineeRelationship()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 5 });
        var (request, traineeService) = BuildRequest(ctx, trainer.Id, member.Id);

        var svc = new TrainingRequestService(ctx, traineeService);
        await svc.AcceptAsync(request.Id, string.Empty);

        ctx.TrainerTrainees.Should().HaveCount(1);
        ctx.TrainerTrainees.First().IsActive.Should().BeTrue();
    }

    [Test]
    public async Task AcceptAsync_ThrowsInvalidOperationException_WhenTrainerAtCapacity()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var m1        = DbContextFactory.SeedMember(ctx, "full1@t.com");
        var m2        = DbContextFactory.SeedMember(ctx, "requester@t.com");

        // Capacity 1, already full
        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 1 });
        ctx.TrainerTrainees.Add(new TrainerTrainee
        {
            TrainerId = trainer.Id, MemberId = m1.Id,
            StartDate = DateTime.UtcNow, IsActive = true
        });
        var (request, traineeService) = BuildRequest(ctx, trainer.Id, m2.Id);

        var svc = new TrainingRequestService(ctx, traineeService);

        Func<Task> act = () => svc.AcceptAsync(request.Id, string.Empty);
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test]
    public async Task AcceptAsync_DoesNothing_WhenRequestNotPending()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 5 });
        var (request, traineeService) =
            BuildRequest(ctx, trainer.Id, member.Id, TrainingRequestStatus.Rejected);

        var svc = new TrainingRequestService(ctx, traineeService);
        await svc.AcceptAsync(request.Id, "too late");

        // Still rejected, no relationship created
        ctx.TrainingRequests.First().Status.Should().Be(TrainingRequestStatus.Rejected);
        ctx.TrainerTrainees.Should().BeEmpty();
    }

    // ── RejectAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task RejectAsync_ChangesStatusToRejected()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var (request, traineeService) = BuildRequest(ctx, trainer.Id, member.Id);
        var svc = new TrainingRequestService(ctx, traineeService);
        await svc.RejectAsync(request.Id, "Sorry, full schedule.");

        var updated = ctx.TrainingRequests.First(r => r.Id == request.Id);
        updated.Status.Should().Be(TrainingRequestStatus.Rejected);
        updated.TrainerResponse.Should().Be("Sorry, full schedule.");
    }

    [Test]
    public async Task RejectAsync_DoesNothing_WhenRequestNotPending()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var (request, traineeService) =
            BuildRequest(ctx, trainer.Id, member.Id, TrainingRequestStatus.Accepted);

        var svc = new TrainingRequestService(ctx, traineeService);
        await svc.RejectAsync(request.Id, "changed my mind");

        ctx.TrainingRequests.First().Status.Should().Be(TrainingRequestStatus.Accepted);
    }

    // ── CancelAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task CancelAsync_ChangesStatusToCancelled()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var (request, traineeService) = BuildRequest(ctx, trainer.Id, member.Id);
        var svc = new TrainingRequestService(ctx, traineeService);
        await svc.CancelAsync(request.Id);

        ctx.TrainingRequests.First().Status.Should().Be(TrainingRequestStatus.Cancelled);
    }

    [Test]
    public async Task CancelAsync_DoesNothing_WhenRequestNotPending()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var (request, traineeService) =
            BuildRequest(ctx, trainer.Id, member.Id, TrainingRequestStatus.Accepted);

        var svc = new TrainingRequestService(ctx, traineeService);
        await svc.CancelAsync(request.Id);

        ctx.TrainingRequests.First().Status.Should().Be(TrainingRequestStatus.Accepted);
    }
}
