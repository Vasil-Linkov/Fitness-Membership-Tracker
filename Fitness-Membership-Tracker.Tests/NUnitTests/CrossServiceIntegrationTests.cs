using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Implementations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitnessTracker.Tests.Services;

/// <summary>
/// Integration-style tests that wire together two or more real service
/// implementations against the same in-memory database.  These verify that
/// the services co-operate correctly (e.g. accepting a training request creates
/// a relationship that the trainee service then reports on).
/// </summary>
[TestFixture]
[Category("UnitTests")]
public class CrossServiceIntegrationTests
{
    // ── helpers ───────────────────────────────────────────────────────────────

    private static (TrainerTraineeService traineeService,
                    TrainingRequestService requestService)
        BuildServices(Fitness_Membership_Tracker.Data.ApplicationDbContext ctx)
    {
        var traineeService  = new TrainerTraineeService(ctx);
        var requestService  = new TrainingRequestService(ctx, traineeService);
        return (traineeService, requestService);
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Membership + Payment pipeline
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public async Task PurchaseMembership_ThenRecordPayment_BothPersisted()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx, "Basic", 29.99m);
        var member    = DbContextFactory.SeedMember(ctx);
        var emp       = DbContextFactory.SeedEmployee(ctx, loc.Id);

        var membershipService = new MembershipService(ctx);
        var paymentService    = new PaymentService(ctx);

        // 1. Create membership
        var membership = new Membership
        {
            StartDate        = DateTime.Now,
            EndDate          = DateTime.Now.AddMonths(1),
            LocationId       = loc.Id,
            MembershipTierId = tier.Id
        };
        await membershipService.CreateAsync(membership);

        // 2. Link to member
        var memberService = new MemberService(ctx);
        member.MembershipId = membership.Id;
        await memberService.UpdateAsync(member);

        // 3. Record payment
        var payment = new Payment
        {
            Currency      = "EUR",
            Amount        = tier.MonthlyPrice,
            PaymentMethod = "OnSite",
            MemberId      = member.Id,
            MembershipId  = membership.Id,
            EmployeeId    = emp.Id
        };
        await paymentService.CreateAsync(payment);

        // Assert
        var payments = await paymentService.GetByMemberIdAsync(member.Id);
        payments.Should().HaveCount(1);
        payments.First().Amount.Should().Be(29.99m);

        var fetchedMs = await membershipService.GetMembershipByMember(
            await memberService.GetByIdAsync(member.Id));
        fetchedMs.Should().NotBeNull();
        fetchedMs!.Id.Should().Be(membership.Id);
    }

    [Test]
    public async Task DeleteMembership_DoesNotRemovePaymentHistory()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var paymentService    = new PaymentService(ctx);
        var membershipService = new MembershipService(ctx);

        await paymentService.CreateAsync(new Payment
        {
            Currency = "EUR", Amount = 29.99m, PaymentMethod = "Card",
            MemberId = member.Id, MembershipId = ms.Id
        });

        // Soft-delete the membership
        await membershipService.DeleteAsync(ms.Id);

        // Payment history should still be queryable
        var payments = await paymentService.GetByMemberIdAsync(member.Id);
        payments.Should().HaveCount(1);
    }

    [Test]
    public async Task RenewExpiredMembership_ExtendsEndDate()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);

        var membershipService = new MembershipService(ctx);

        // Create a membership that ended yesterday
        var ms = new Membership
        {
            StartDate        = DateTime.Now.AddMonths(-2),
            EndDate          = DateTime.Now.AddDays(-1),
            LocationId       = loc.Id,
            MembershipTierId = tier.Id
        };
        await membershipService.CreateAsync(ms);

        // Renew: extend end date by one month
        ms.EndDate    = DateTime.Now.AddMonths(1);
        ms.IsDeleted  = false;
        ms.DeletedAt  = null;
        await membershipService.UpdateAsync(ms);

        var updated = await membershipService.GetByIdAsync(ms.Id);
        updated.Should().NotBeNull();
        updated!.EndDate.Should().BeAfter(DateTime.Now);
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Trainer request → accept → relationship → capacity enforced
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public async Task AcceptRequest_ThenCheckCapacity_ReflectsNewTrainee()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 3 });
        ctx.SaveChanges();

        var (traineeService, requestService) = BuildServices(ctx);

        // Initially has capacity
        (await traineeService.HasCapacityAsync(trainer.Id)).Should().BeTrue();
        (await traineeService.GetActiveTraineeCountAsync(trainer.Id)).Should().Be(0);

        // Send & accept request
        var request = new TrainingRequest { TrainerId = trainer.Id, MemberId = member.Id };
        await requestService.CreateAsync(request);
        await requestService.AcceptAsync(request.Id, "Welcome!");

        // Count should now be 1
        (await traineeService.GetActiveTraineeCountAsync(trainer.Id)).Should().Be(1);
        (await traineeService.HasCapacityAsync(trainer.Id)).Should().BeTrue(); // 1 of 3
    }

    [Test]
    public async Task FillTrainerCapacity_ThenAcceptAnotherRequest_Throws()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        // Cap = 1
        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 1 });

        var m1 = DbContextFactory.SeedMember(ctx, "first@t.com");
        var m2 = DbContextFactory.SeedMember(ctx, "second@t.com");
        ctx.SaveChanges();

        var (traineeService, requestService) = BuildServices(ctx);

        // Accept first request — fills capacity
        var req1 = new TrainingRequest { TrainerId = trainer.Id, MemberId = m1.Id };
        await requestService.CreateAsync(req1);
        await requestService.AcceptAsync(req1.Id, string.Empty);

        // Second request should throw
        var req2 = new TrainingRequest { TrainerId = trainer.Id, MemberId = m2.Id };
        await requestService.CreateAsync(req2);

        Func<Task> act = () => requestService.AcceptAsync(req2.Id, string.Empty);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*capacity*");
    }

    [Test]
    public async Task EndRelationship_ThenHasCapacity_ReturnsTrue()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 1 });
        ctx.SaveChanges();

        var (traineeService, requestService) = BuildServices(ctx);

        var request = new TrainingRequest { TrainerId = trainer.Id, MemberId = member.Id };
        await requestService.CreateAsync(request);
        await requestService.AcceptAsync(request.Id, string.Empty);

        // Trainer now full
        (await traineeService.HasCapacityAsync(trainer.Id)).Should().BeFalse();

        // End the relationship
        var rel = await traineeService.GetActiveRelationshipAsync(member.Id);
        await traineeService.EndRelationshipAsync(rel!.Id);

        // Trainer has capacity again
        (await traineeService.HasCapacityAsync(trainer.Id)).Should().BeTrue();
    }

    [Test]
    public async Task CancelRequest_DoesNotCreateRelationship()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 5 });
        ctx.SaveChanges();

        var (traineeService, requestService) = BuildServices(ctx);

        var request = new TrainingRequest { TrainerId = trainer.Id, MemberId = member.Id };
        await requestService.CreateAsync(request);
        await requestService.CancelAsync(request.Id);

        ctx.TrainerTrainees.Should().BeEmpty();
        (await traineeService.GetActiveTraineeCountAsync(trainer.Id)).Should().Be(0);
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Visit + daily counts pipeline
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public async Task LogMultipleVisits_ThenGetDailyCounts_ReturnsAccurateSummary()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var visitService = new VisitService(ctx);

        var today     = DateTime.UtcNow.Date;
        var yesterday = today.AddDays(-1);

        // 3 visits today, 2 yesterday
        for (int i = 0; i < 3; i++)
            await visitService.CreateAsync(new Visit
            {
                MemberId     = member.Id,
                LocationId   = loc.Id,
                MembershipId = ms.Id,
                VisitDate    = today.AddHours(8 + i)
            });

        for (int i = 0; i < 2; i++)
            await visitService.CreateAsync(new Visit
            {
                MemberId     = member.Id,
                LocationId   = loc.Id,
                MembershipId = ms.Id,
                VisitDate    = yesterday.AddHours(9 + i)
            });

        var counts = await visitService.GetDailyVisitCountsAsync(yesterday, today);

        counts.Should().ContainKey(today).WhoseValue.Should().Be(3);
        counts.Should().ContainKey(yesterday).WhoseValue.Should().Be(2);

        var total  = counts.Values.Sum();
        var avg    = (double)total / 2;
        var peak   = counts.Values.Max();

        total.Should().Be(5);
        avg.Should().Be(2.5);
        peak.Should().Be(3);
    }

    [Test]
    public async Task DeleteVisit_ReducesDailyCount()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var visitService = new VisitService(ctx);
        var today        = DateTime.UtcNow.Date;

        var v1 = new Visit { MemberId = member.Id, LocationId = loc.Id, MembershipId = ms.Id, VisitDate = today.AddHours(8) };
        var v2 = new Visit { MemberId = member.Id, LocationId = loc.Id, MembershipId = ms.Id, VisitDate = today.AddHours(12) };

        await visitService.CreateAsync(v1);
        await visitService.CreateAsync(v2);

        var countsBeforeDelete = await visitService.GetDailyVisitCountsAsync(today, today);
        countsBeforeDelete[today].Should().Be(2);

        await visitService.DeleteAsync(v1.Id);

        var countsAfterDelete = await visitService.GetDailyVisitCountsAsync(today, today);
        countsAfterDelete[today].Should().Be(1);
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Workout pipeline
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public async Task LogWorkout_ThenDeleteMember_WorkoutStillExistsInDb()
    {
        using var ctx  = DbContextFactory.Create();
        var member     = DbContextFactory.SeedMember(ctx);
        var workoutSvc = new WorkoutService(ctx);
        var memberSvc  = new MemberService(ctx);

        var log = new Fitness_Membership_Tracker.Data.DataModels.WorkoutLog
        {
            MemberId  = member.Id,
            Notes     = "Last session before leaving",
            Exercises = new List<Fitness_Membership_Tracker.Data.DataModels.WorkoutExercise>
            {
                new() { ExerciseName = "Squat", Sets = 3, Reps = 10 }
            }
        };
        await workoutSvc.CreateAsync(log);

        // Soft-delete the member
        await memberSvc.DeleteAsync(member.Id);

        // Workout log still physically present (soft delete doesn't cascade)
        var rawLog = ctx.WorkoutLogs.IgnoreQueryFilters().FirstOrDefault(l => l.Id == log.Id);
        rawLog.Should().NotBeNull();
    }

    [Test]
    public async Task MultipleMembers_WorkoutLogs_AreIsolated()
    {
        using var ctx  = DbContextFactory.Create();
        var m1         = DbContextFactory.SeedMember(ctx, "m1@t.com");
        var m2         = DbContextFactory.SeedMember(ctx, "m2@t.com");
        var workoutSvc = new WorkoutService(ctx);

        // m1 logs 3 workouts, m2 logs 1
        for (int i = 0; i < 3; i++)
            await workoutSvc.CreateAsync(new Fitness_Membership_Tracker.Data.DataModels.WorkoutLog
            {
                MemberId  = m1.Id,
                Notes     = $"Session {i}",
                Exercises = new List<Fitness_Membership_Tracker.Data.DataModels.WorkoutExercise>
                {
                    new() { ExerciseName = "Push-up", Sets = 3, Reps = 20 }
                }
            });

        await workoutSvc.CreateAsync(new Fitness_Membership_Tracker.Data.DataModels.WorkoutLog
        {
            MemberId  = m2.Id,
            Notes     = "Solo session",
            Exercises = new List<Fitness_Membership_Tracker.Data.DataModels.WorkoutExercise>
            {
                new() { ExerciseName = "Run", DurationMinutes = 30 }
            }
        });

        var m1Logs = await workoutSvc.GetByMemberIdAsync(m1.Id);
        var m2Logs = await workoutSvc.GetByMemberIdAsync(m2.Id);

        m1Logs.Should().HaveCount(3);
        m2Logs.Should().HaveCount(1);
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Soft-delete global filter consistency
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public async Task SoftDeletedEmployee_NotReturnedByService_ButVisibleWithIgnoreFilter()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var emp       = DbContextFactory.SeedEmployee(ctx, loc.Id);
        var empSvc    = new EmployeeService(ctx);

        await empSvc.DeleteAsync(emp.Id);

        // Service query respects the global filter
        var visible = await empSvc.GetEmployeesAsync(null, string.Empty);
        visible.Should().BeEmpty();

        // Raw IgnoreQueryFilters shows the record
        var raw = ctx.Employees.IgnoreQueryFilters().First(e => e.Id == emp.Id);
        raw.IsDeleted.Should().BeTrue();
    }

    [Test]
    public async Task SoftDeletedLocation_NotReturnedByLocationService()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx, "GhostCity");
        var locSvc    = new LocationService(ctx);

        loc.IsDeleted = true;
        loc.DeletedAt = DateTime.UtcNow;
        ctx.SaveChanges();

        var result = await locSvc.GetAllAsync();
        result.Should().NotContain(l => l.City == "GhostCity");
    }

    [Test]
    public async Task SoftDeletedMembership_NotReturnedByGetAll_ButFoundByIncludeDeleted()
    {
        using var ctx  = DbContextFactory.Create();
        var loc        = DbContextFactory.SeedLocation(ctx);
        var tier       = DbContextFactory.SeedTier(ctx);
        var ms         = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var msSvc      = new MembershipService(ctx);

        await msSvc.DeleteAsync(ms.Id);

        var all = await msSvc.GetAllAsync();
        all.Should().BeEmpty();

        var withDeleted = await msSvc.GetByIdIncludingDeletedAsync(ms.Id);
        withDeleted.Should().NotBeNull();
        withDeleted!.IsDeleted.Should().BeTrue();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  TrainerSchedule → availability pipeline
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public async Task AddScheduleSlot_ThenTrainerAppearsInAvailability()
    {
        using var ctx   = DbContextFactory.Create();
        var loc         = DbContextFactory.SeedLocation(ctx);
        var trainer     = DbContextFactory.SeedTrainer(ctx, loc.Id);

        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 5 });
        ctx.SaveChanges();

        var scheduleSvc = new TrainerScheduleService(ctx);

        // Before slot: trainer not available on Wednesday
        var before = await scheduleSvc.GetAvailableTrainersAsync(DayOfWeek.Wednesday);
        before.Should().BeEmpty();

        // Add Wednesday slot
        await scheduleSvc.AddSlotAsync(new TrainerSchedule
        {
            TrainerId = trainer.Id,
            DayOfWeek = (int)DayOfWeek.Wednesday,
            StartTime = new TimeSpan(9, 0, 0),
            EndTime   = new TimeSpan(13, 0, 0)
        });

        // After slot: trainer appears on Wednesday
        var after = await scheduleSvc.GetAvailableTrainersAsync(DayOfWeek.Wednesday);
        after.Should().HaveCount(1);
        after.First().Id.Should().Be(trainer.Id);
    }

    [Test]
    public async Task RemoveScheduleSlot_ThenTrainerDisappearsFromAvailability()
    {
        using var ctx   = DbContextFactory.Create();
        var loc         = DbContextFactory.SeedLocation(ctx);
        var trainer     = DbContextFactory.SeedTrainer(ctx, loc.Id);

        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 5 });
        var slot = new TrainerSchedule
        {
            TrainerId = trainer.Id,
            DayOfWeek = (int)DayOfWeek.Thursday,
            StartTime = new TimeSpan(14, 0, 0),
            EndTime   = new TimeSpan(18, 0, 0)
        };
        ctx.TrainerSchedules.Add(slot);
        ctx.SaveChanges();

        var scheduleSvc = new TrainerScheduleService(ctx);

        var before = await scheduleSvc.GetAvailableTrainersAsync(DayOfWeek.Thursday);
        before.Should().HaveCount(1);

        await scheduleSvc.RemoveSlotAsync(slot.Id);

        var after = await scheduleSvc.GetAvailableTrainersAsync(DayOfWeek.Thursday);
        after.Should().BeEmpty();
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  Capacity update reflected in HasCapacity
    // ═══════════════════════════════════════════════════════════════════════════

    [Test]
    public async Task IncreaseCapacity_AfterBeingFull_ReturnsHasCapacityTrue()
    {
        using var ctx  = DbContextFactory.Create();
        var loc        = DbContextFactory.SeedLocation(ctx);
        var trainer    = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member     = DbContextFactory.SeedMember(ctx);

        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 1 });
        ctx.TrainerTrainees.Add(new TrainerTrainee
        {
            TrainerId = trainer.Id,
            MemberId  = member.Id,
            StartDate = DateTime.UtcNow,
            IsActive  = true
        });
        ctx.SaveChanges();

        var traineeService = new TrainerTraineeService(ctx);

        (await traineeService.HasCapacityAsync(trainer.Id)).Should().BeFalse();

        await traineeService.UpdateMaxTraineesAsync(trainer.Id, 5);

        (await traineeService.HasCapacityAsync(trainer.Id)).Should().BeTrue();
    }
}
