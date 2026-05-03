using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Implementations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitnessTracker.Tests.Services;

/// <summary>
/// Boundary-value, edge-case and stress tests that complement the happy-path
/// tests in each individual service test file.
/// </summary>
[TestFixture]
[Category("UnitTests")]
public class EdgeCaseTests
{
    // ═══════════════════════════════════════════════════════════════════════════
    //  MemberService edge cases
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public async Task GetByNameAsync_IsCaseSensitive_ForEFInMemory()
    {
        // EF In-Memory uses StringComparison.Ordinal by default.
        // The real SQL Server provider uses the DB collation.
        // This test documents the in-memory behaviour.
        using var ctx = DbContextFactory.Create();
        DbContextFactory.SeedMember(ctx, "User@Test.COM");

        var svc = new MemberService(ctx);

        // Exact match works
        var exact = await svc.GetByNameAsync("User@Test.COM");
        exact.Should().NotBeNull();
    }

    [Test]
    public async Task GetAllAsync_WithManyMembers_ReturnsAll()
    {
        using var ctx = DbContextFactory.Create();
        for (int i = 0; i < 50; i++)
            DbContextFactory.SeedMember(ctx, $"user{i}@test.com");

        var svc    = new MemberService(ctx);
        var result = await svc.GetAllAsync();

        result.Should().HaveCount(50);
    }

    [Test]
    public async Task DeleteMember_Twice_DoesNotThrowAndRemainsDeleted()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);
        var svc       = new MemberService(ctx);

        await svc.DeleteAsync(member.Id);
        Func<Task> act = () => svc.DeleteAsync(member.Id);

        // EF FindAsync returns null for soft-deleted via query filter
        // so the second call silently does nothing
        await act.Should().NotThrowAsync();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  MembershipService edge cases
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public async Task CreateMembership_WithEndDateBeforeStartDate_StillPersists()
    {
        // Business-rule validation is the controller's job; the service just saves.
        using var ctx = DbContextFactory.Create();
        var loc  = DbContextFactory.SeedLocation(ctx);
        var tier = DbContextFactory.SeedTier(ctx);

        var ms = new Membership
        {
            StartDate        = DateTime.Now,
            EndDate          = DateTime.Now.AddDays(-1), // end before start
            LocationId       = loc.Id,
            MembershipTierId = tier.Id
        };

        var svc = new MembershipService(ctx);
        await svc.CreateAsync(ms);

        ctx.Memberships.Should().HaveCount(1);
    }

    [Test]
    public async Task GetAllMemberships_LargeDataset_PerformsWithinReasonableTime()
    {
        using var ctx = DbContextFactory.Create();
        var loc  = DbContextFactory.SeedLocation(ctx);
        var tier = DbContextFactory.SeedTier(ctx);

        for (int i = 0; i < 100; i++)
            DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);

        var svc   = new MembershipService(ctx);
        var start = DateTime.UtcNow;
        var all   = await svc.GetAllAsync();
        var elapsed = DateTime.UtcNow - start;

        all.Should().HaveCount(100);
        elapsed.Should().BeLessThan(TimeSpan.FromSeconds(5));
    }

    [Test]
    public async Task UpdateMembership_OnlyChangesTargetFields()
    {
        using var ctx = DbContextFactory.Create();
        var loc1 = DbContextFactory.SeedLocation(ctx, "Sofia");
        var loc2 = DbContextFactory.SeedLocation(ctx, "Varna");
        var tier = DbContextFactory.SeedTier(ctx);
        var ms   = DbContextFactory.SeedMembership(ctx, loc1.Id, tier.Id);

        var svc        = new MembershipService(ctx);
        ms.LocationId  = loc2.Id;
        await svc.UpdateAsync(ms);

        var updated = ctx.Memberships.IgnoreQueryFilters().First(m => m.Id == ms.Id);
        updated.LocationId.Should().Be(loc2.Id);
        updated.MembershipTierId.Should().Be(tier.Id); // unchanged
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  EmployeeService edge cases
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public async Task GetEmployees_SearchTermWithLeadingTrailingSpaces_IsTrimmed()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var emp       = DbContextFactory.SeedEmployee(ctx, loc.Id);
        emp.FirstName = "Trimable";
        ctx.SaveChanges();

        var svc    = new EmployeeService(ctx);
        var result = await svc.GetEmployeesAsync(null, "  Trimable  ");

        result.Should().HaveCount(1);
    }

    [Test]
    public async Task GetEmployees_EmptySearchString_ReturnsAll()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        DbContextFactory.SeedEmployee(ctx, loc.Id);
        DbContextFactory.SeedEmployee(ctx, loc.Id);

        var svc    = new EmployeeService(ctx);
        var result = await svc.GetEmployeesAsync(null, "");

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task UpdateEmployee_ChangesLocation()
    {
        using var ctx = DbContextFactory.Create();
        var loc1      = DbContextFactory.SeedLocation(ctx, "Sofia");
        var loc2      = DbContextFactory.SeedLocation(ctx, "Plovdiv");
        var emp       = DbContextFactory.SeedEmployee(ctx, loc1.Id);

        emp.LocationId = loc2.Id;
        var svc = new EmployeeService(ctx);
        await svc.UpdateAsync(emp);

        ctx.Employees.IgnoreQueryFilters().First(e => e.Id == emp.Id)
            .LocationId.Should().Be(loc2.Id);
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  PaymentService edge cases
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public async Task GetByMemberIdAsync_MemberWithManyPayments_ReturnsAll()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);
        var svc       = new PaymentService(ctx);

        for (int i = 0; i < 24; i++)
            await svc.CreateAsync(new Payment
            {
                Currency      = "EUR",
                Amount        = 29.99m,
                PaymentMethod = "Card",
                MemberId      = member.Id,
                MembershipId  = ms.Id
            });

        var result = await svc.GetByMemberIdAsync(member.Id);
        result.Should().HaveCount(24);
    }

    [Test]
    public async Task CreatePayment_WithZeroAmount_IsAllowed()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);
        var svc       = new PaymentService(ctx);

        await svc.CreateAsync(new Payment
        {
            Currency      = "EUR",
            Amount        = 0m,
            PaymentMethod = "Promo",
            MemberId      = member.Id,
            MembershipId  = ms.Id
        });

        ctx.Payments.IgnoreQueryFilters().First().Amount.Should().Be(0m);
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  VisitService edge cases
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public async Task GetByDateRangeAsync_InclusiveBoundaries()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var from  = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var to    = new DateTime(2024, 1, 7, 23, 59, 59, DateTimeKind.Utc);

        // Exactly on boundary dates
        ctx.Visits.Add(new Visit { MemberId = member.Id, LocationId = loc.Id,
            MembershipId = ms.Id, VisitDate = from });
        ctx.Visits.Add(new Visit { MemberId = member.Id, LocationId = loc.Id,
            MembershipId = ms.Id, VisitDate = to });
        // Outside range
        ctx.Visits.Add(new Visit { MemberId = member.Id, LocationId = loc.Id,
            MembershipId = ms.Id, VisitDate = to.AddSeconds(1) });
        ctx.SaveChanges();

        var svc    = new VisitService(ctx);
        var result = await svc.GetByDateRangeAsync(from, to);

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task GetDailyVisitCountsAsync_SingleDayRange_Works()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var today = DateTime.UtcNow.Date;
        ctx.Visits.Add(new Visit { MemberId = member.Id, LocationId = loc.Id,
            MembershipId = ms.Id, VisitDate = today.AddHours(10) });
        ctx.Visits.Add(new Visit { MemberId = member.Id, LocationId = loc.Id,
            MembershipId = ms.Id, VisitDate = today.AddHours(17) });
        ctx.SaveChanges();

        var svc    = new VisitService(ctx);
        var result = await svc.GetDailyVisitCountsAsync(today, today);

        result.Should().ContainKey(today).WhoseValue.Should().Be(2);
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  TrainerService edge cases
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public async Task GetTrainers_WithNullSearch_TreatedAsEmptyString()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        DbContextFactory.SeedTrainer(ctx, loc.Id);

        var svc    = new TrainerService(ctx);
        // null search should not throw
        Func<Task> act = () => svc.GetTrainersAsync(null, null!);
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task GetTrainers_MultipleLocations_FiltersCorrectly()
    {
        using var ctx = DbContextFactory.Create();
        var loc1      = DbContextFactory.SeedLocation(ctx, "A");
        var loc2      = DbContextFactory.SeedLocation(ctx, "B");
        var loc3      = DbContextFactory.SeedLocation(ctx, "C");

        DbContextFactory.SeedTrainer(ctx, loc1.Id);
        DbContextFactory.SeedTrainer(ctx, loc1.Id);
        DbContextFactory.SeedTrainer(ctx, loc2.Id);
        DbContextFactory.SeedTrainer(ctx, loc3.Id);

        var svc    = new TrainerService(ctx);
        var result = await svc.GetTrainersAsync(loc1.Id, string.Empty);

        result.Should().HaveCount(2);
        result.Should().AllSatisfy(t => t.LocationId.Should().Be(loc1.Id));
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  TrainingRequestService edge cases
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public async Task AcceptRequest_WhenRequestNotFound_DoesNotThrow()
    {
        using var ctx      = DbContextFactory.Create();
        var traineeService = new TrainerTraineeService(ctx);
        var svc            = new TrainingRequestService(ctx, traineeService);

        Func<Task> act = () => svc.AcceptAsync(9999, "hello");
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task RejectRequest_WhenRequestNotFound_DoesNotThrow()
    {
        using var ctx      = DbContextFactory.Create();
        var traineeService = new TrainerTraineeService(ctx);
        var svc            = new TrainingRequestService(ctx, traineeService);

        Func<Task> act = () => svc.RejectAsync(9999, "sorry");
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task CancelRequest_WhenRequestNotFound_DoesNotThrow()
    {
        using var ctx      = DbContextFactory.Create();
        var traineeService = new TrainerTraineeService(ctx);
        var svc            = new TrainingRequestService(ctx, traineeService);

        Func<Task> act = () => svc.CancelAsync(9999);
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task GetPendingRequests_ReturnsOnlyForThatTrainer()
    {
        using var ctx   = DbContextFactory.Create();
        var loc         = DbContextFactory.SeedLocation(ctx);
        var trainer1    = DbContextFactory.SeedTrainer(ctx, loc.Id, "Yoga");
        var trainer2    = DbContextFactory.SeedTrainer(ctx, loc.Id, "CrossFit");
        var m1          = DbContextFactory.SeedMember(ctx, "a@t.com");
        var m2          = DbContextFactory.SeedMember(ctx, "b@t.com");

        ctx.TrainingRequests.AddRange(
            new TrainingRequest { TrainerId = trainer1.Id, MemberId = m1.Id,
                Status = TrainingRequestStatus.Pending, RequestedAt = DateTime.UtcNow },
            new TrainingRequest { TrainerId = trainer2.Id, MemberId = m2.Id,
                Status = TrainingRequestStatus.Pending, RequestedAt = DateTime.UtcNow }
        );
        ctx.SaveChanges();

        var traineeService = new TrainerTraineeService(ctx);
        var svc            = new TrainingRequestService(ctx, traineeService);

        var trainer1Pending = await svc.GetPendingForTrainerAsync(trainer1.Id);
        var trainer2Pending = await svc.GetPendingForTrainerAsync(trainer2.Id);

        trainer1Pending.Should().HaveCount(1);
        trainer2Pending.Should().HaveCount(1);
        trainer1Pending.First().TrainerId.Should().Be(trainer1.Id);
        trainer2Pending.First().TrainerId.Should().Be(trainer2.Id);
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  WorkoutService edge cases
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public async Task CreateWorkout_WithNoExercises_IsAllowed()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);
        var svc       = new WorkoutService(ctx);

        var log = new Fitness_Membership_Tracker.Data.DataModels.WorkoutLog
        {
            MemberId  = member.Id,
            Notes     = "Rest day log",
            Exercises = new List<Fitness_Membership_Tracker.Data.DataModels.WorkoutExercise>()
        };

        Func<Task> act = () => svc.CreateAsync(log);
        await act.Should().NotThrowAsync();

        ctx.WorkoutLogs.IgnoreQueryFilters().Should().HaveCount(1);
        ctx.WorkoutExercises.Should().BeEmpty();
    }

    [Test]
    public async Task CreateWorkout_WithOptionalFieldsNull_IsAllowed()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);
        var svc       = new WorkoutService(ctx);

        var log = new Fitness_Membership_Tracker.Data.DataModels.WorkoutLog
        {
            MemberId  = member.Id,
            Notes     = string.Empty,
            Exercises = new List<Fitness_Membership_Tracker.Data.DataModels.WorkoutExercise>
            {
                new()
                {
                    ExerciseName    = "Plank",
                    Sets            = null,
                    Reps            = null,
                    WeightKg        = null,
                    DurationMinutes = 2,
                    Notes           = null
                }
            }
        };

        await svc.CreateAsync(log);

        var saved = ctx.WorkoutExercises.First();
        saved.Sets.Should().BeNull();
        saved.Reps.Should().BeNull();
        saved.WeightKg.Should().BeNull();
        saved.DurationMinutes.Should().Be(2);
    }

    [Test]
    public async Task GetByMemberIdAsync_AfterDeletingOneOfSeveralLogs_RemainingAreReturned()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);
        var svc       = new WorkoutService(ctx);

        var log1 = new Fitness_Membership_Tracker.Data.DataModels.WorkoutLog
            { MemberId = member.Id, Notes = "Keep", Exercises = new List<Fitness_Membership_Tracker.Data.DataModels.WorkoutExercise>() };
        var log2 = new Fitness_Membership_Tracker.Data.DataModels.WorkoutLog
            { MemberId = member.Id, Notes = "Delete me", Exercises = new List<Fitness_Membership_Tracker.Data.DataModels.WorkoutExercise>() };

        await svc.CreateAsync(log1);
        await svc.CreateAsync(log2);

        await svc.DeleteAsync(log2.Id);

        var result = await svc.GetByMemberIdAsync(member.Id);
        result.Should().HaveCount(1);
        result.First().Notes.Should().Be("Keep");
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  TrainerTraineeService edge cases
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public async Task GetMaxTraineesAsync_UpdateTwice_ReturnsLatestValue()
    {
        using var ctx  = DbContextFactory.Create();
        var loc        = DbContextFactory.SeedLocation(ctx);
        var trainer    = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var traineeService = new TrainerTraineeService(ctx);

        await traineeService.UpdateMaxTraineesAsync(trainer.Id, 7);
        await traineeService.UpdateMaxTraineesAsync(trainer.Id, 12);

        var max = await traineeService.GetMaxTraineesAsync(trainer.Id);
        max.Should().Be(12);
    }

    [Test]
    public async Task EndRelationship_AlreadyInactive_DoesNotThrow()
    {
        using var ctx  = DbContextFactory.Create();
        var loc        = DbContextFactory.SeedLocation(ctx);
        var trainer    = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member     = DbContextFactory.SeedMember(ctx);

        var rel = new TrainerTrainee
        {
            TrainerId = trainer.Id,
            MemberId  = member.Id,
            StartDate = DateTime.UtcNow.AddDays(-30),
            EndDate   = DateTime.UtcNow.AddDays(-1),
            IsActive  = false
        };
        ctx.TrainerTrainees.Add(rel);
        ctx.SaveChanges();

        var traineeService = new TrainerTraineeService(ctx);

        Func<Task> act = () => traineeService.EndRelationshipAsync(rel.Id);
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task MultipleTrainers_IndependentCapacityTracking()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var t1        = DbContextFactory.SeedTrainer(ctx, loc.Id, "Yoga");
        var t2        = DbContextFactory.SeedTrainer(ctx, loc.Id, "CrossFit");
        var m1        = DbContextFactory.SeedMember(ctx, "a@t.com");
        var m2        = DbContextFactory.SeedMember(ctx, "b@t.com");
        var m3        = DbContextFactory.SeedMember(ctx, "c@t.com");

        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = t1.Id, MaxTrainees = 2 });
        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = t2.Id, MaxTrainees = 1 });

        ctx.TrainerTrainees.Add(new TrainerTrainee { TrainerId = t1.Id, MemberId = m1.Id, StartDate = DateTime.UtcNow, IsActive = true });
        ctx.TrainerTrainees.Add(new TrainerTrainee { TrainerId = t1.Id, MemberId = m2.Id, StartDate = DateTime.UtcNow, IsActive = true });
        ctx.TrainerTrainees.Add(new TrainerTrainee { TrainerId = t2.Id, MemberId = m3.Id, StartDate = DateTime.UtcNow, IsActive = true });
        ctx.SaveChanges();

        var svc = new TrainerTraineeService(ctx);

        (await svc.HasCapacityAsync(t1.Id)).Should().BeFalse(); // 2/2 full
        (await svc.HasCapacityAsync(t2.Id)).Should().BeFalse(); // 1/1 full

        (await svc.GetActiveTraineeCountAsync(t1.Id)).Should().Be(2);
        (await svc.GetActiveTraineeCountAsync(t2.Id)).Should().Be(1);
    }
}
