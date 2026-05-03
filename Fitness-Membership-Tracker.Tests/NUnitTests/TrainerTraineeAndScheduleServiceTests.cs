using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Implementations;
using FluentAssertions;
using NUnit.Framework;

namespace FitnessTracker.Tests.Services;

// ═══════════════════════════════════════════════════════════════════════════════
//  TrainerTraineeServiceTests
// ═══════════════════════════════════════════════════════════════════════════════

[TestFixture]
[Category("UnitTests")]
public class TrainerTraineeServiceTests
{
    private static TrainerTrainee ActiveRelationship(int trainerId, string memberId) =>
        new TrainerTrainee
        {
            TrainerId = trainerId,
            MemberId  = memberId,
            StartDate = DateTime.UtcNow.AddDays(-10),
            IsActive  = true
        };

    // ── GetByTrainerIdAsync ───────────────────────────────────────────────────

    [Test]
    public async Task GetByTrainerIdAsync_ReturnsOnlyActiveRelationships()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var m1        = DbContextFactory.SeedMember(ctx, "m1@t.com");
        var m2        = DbContextFactory.SeedMember(ctx, "m2@t.com");

        ctx.TrainerTrainees.Add(ActiveRelationship(trainer.Id, m1.Id));
        ctx.TrainerTrainees.Add(new TrainerTrainee
        {
            TrainerId = trainer.Id, MemberId = m2.Id,
            StartDate = DateTime.UtcNow.AddDays(-30),
            EndDate   = DateTime.UtcNow.AddDays(-5),
            IsActive  = false
        });
        ctx.SaveChanges();

        var svc    = new TrainerTraineeService(ctx);
        var result = await svc.GetByTrainerIdAsync(trainer.Id);

        result.Should().HaveCount(1);
        result.First().MemberId.Should().Be(m1.Id);
    }

    [Test]
    public async Task GetByTrainerIdAsync_ReturnsEmpty_WhenNoActiveTrainees()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        var svc    = new TrainerTraineeService(ctx);
        var result = await svc.GetByTrainerIdAsync(trainer.Id);

        result.Should().BeEmpty();
    }

    // ── GetActiveRelationshipAsync ────────────────────────────────────────────

    [Test]
    public async Task GetActiveRelationshipAsync_ReturnsActiveRelationship()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.TrainerTrainees.Add(ActiveRelationship(trainer.Id, member.Id));
        ctx.SaveChanges();

        var svc    = new TrainerTraineeService(ctx);
        var result = await svc.GetActiveRelationshipAsync(member.Id);

        result.Should().NotBeNull();
        result!.TrainerId.Should().Be(trainer.Id);
    }

    [Test]
    public async Task GetActiveRelationshipAsync_ReturnsNull_WhenNoActiveRelationship()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        var svc    = new TrainerTraineeService(ctx);
        var result = await svc.GetActiveRelationshipAsync(member.Id);

        result.Should().BeNull();
    }

    [Test]
    public async Task GetActiveRelationshipAsync_ReturnsNull_WhenRelationshipInactive()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.TrainerTrainees.Add(new TrainerTrainee
        {
            TrainerId = trainer.Id, MemberId = member.Id,
            StartDate = DateTime.UtcNow.AddDays(-20),
            EndDate   = DateTime.UtcNow.AddDays(-1),
            IsActive  = false
        });
        ctx.SaveChanges();

        var svc    = new TrainerTraineeService(ctx);
        var result = await svc.GetActiveRelationshipAsync(member.Id);

        result.Should().BeNull();
    }

    // ── GetActiveTraineeCountAsync ────────────────────────────────────────────

    [Test]
    public async Task GetActiveTraineeCountAsync_ReturnsCorrectCount()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var m1        = DbContextFactory.SeedMember(ctx, "a@t.com");
        var m2        = DbContextFactory.SeedMember(ctx, "b@t.com");
        var m3        = DbContextFactory.SeedMember(ctx, "c@t.com");

        ctx.TrainerTrainees.Add(ActiveRelationship(trainer.Id, m1.Id));
        ctx.TrainerTrainees.Add(ActiveRelationship(trainer.Id, m2.Id));
        ctx.TrainerTrainees.Add(new TrainerTrainee   // inactive
        {
            TrainerId = trainer.Id, MemberId = m3.Id,
            StartDate = DateTime.UtcNow.AddDays(-60), IsActive = false
        });
        ctx.SaveChanges();

        var svc    = new TrainerTraineeService(ctx);
        var result = await svc.GetActiveTraineeCountAsync(trainer.Id);

        result.Should().Be(2);
    }

    [Test]
    public async Task GetActiveTraineeCountAsync_ReturnsZero_WhenNoTrainees()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        var svc    = new TrainerTraineeService(ctx);
        var result = await svc.GetActiveTraineeCountAsync(trainer.Id);

        result.Should().Be(0);
    }

    // ── GetMaxTraineesAsync ───────────────────────────────────────────────────

    [Test]
    public async Task GetMaxTraineesAsync_ReturnsDefault5_WhenNoCapacityRecord()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        var svc    = new TrainerTraineeService(ctx);
        var result = await svc.GetMaxTraineesAsync(trainer.Id);

        result.Should().Be(5);
    }

    [Test]
    public async Task GetMaxTraineesAsync_ReturnsCustomValue_WhenCapacityExists()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 12 });
        ctx.SaveChanges();

        var svc    = new TrainerTraineeService(ctx);
        var result = await svc.GetMaxTraineesAsync(trainer.Id);

        result.Should().Be(12);
    }

    // ── HasCapacityAsync ──────────────────────────────────────────────────────

    [Test]
    public async Task HasCapacityAsync_ReturnsTrue_WhenBelowMax()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 3 });
        ctx.TrainerTrainees.Add(ActiveRelationship(trainer.Id, member.Id));
        ctx.SaveChanges();

        var svc    = new TrainerTraineeService(ctx);
        var result = await svc.HasCapacityAsync(trainer.Id);

        result.Should().BeTrue();
    }

    [Test]
    public async Task HasCapacityAsync_ReturnsFalse_WhenAtMax()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 2 });

        var m1 = DbContextFactory.SeedMember(ctx, "x1@t.com");
        var m2 = DbContextFactory.SeedMember(ctx, "x2@t.com");
        ctx.TrainerTrainees.Add(ActiveRelationship(trainer.Id, m1.Id));
        ctx.TrainerTrainees.Add(ActiveRelationship(trainer.Id, m2.Id));
        ctx.SaveChanges();

        var svc    = new TrainerTraineeService(ctx);
        var result = await svc.HasCapacityAsync(trainer.Id);

        result.Should().BeFalse();
    }

    // ── UpdateMaxTraineesAsync ────────────────────────────────────────────────

    [Test]
    public async Task UpdateMaxTraineesAsync_CreatesNewCapacityRecord_WhenNoneExists()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        var svc = new TrainerTraineeService(ctx);
        await svc.UpdateMaxTraineesAsync(trainer.Id, 10);

        var cap = ctx.TrainerCapacities.First(c => c.TrainerId == trainer.Id);
        cap.MaxTrainees.Should().Be(10);
    }

    [Test]
    public async Task UpdateMaxTraineesAsync_UpdatesExistingCapacityRecord()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 5 });
        ctx.SaveChanges();

        var svc = new TrainerTraineeService(ctx);
        await svc.UpdateMaxTraineesAsync(trainer.Id, 15);

        ctx.TrainerCapacities.First(c => c.TrainerId == trainer.Id)
            .MaxTrainees.Should().Be(15);
    }

    // ── EndRelationshipAsync ──────────────────────────────────────────────────

    [Test]
    public async Task EndRelationshipAsync_SetsIsActiveFalseAndEndDate()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var rel = ActiveRelationship(trainer.Id, member.Id);
        ctx.TrainerTrainees.Add(rel);
        ctx.SaveChanges();

        var svc = new TrainerTraineeService(ctx);
        await svc.EndRelationshipAsync(rel.Id);

        var ended = ctx.TrainerTrainees.First(tt => tt.Id == rel.Id);
        ended.IsActive.Should().BeFalse();
        ended.EndDate.Should().NotBeNull();
    }

    [Test]
    public async Task EndRelationshipAsync_DoesNotThrow_WhenNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new TrainerTraineeService(ctx);

        Func<Task> act = () => svc.EndRelationshipAsync(9999);
        await act.Should().NotThrowAsync();
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
//  TrainerScheduleServiceTests
// ═══════════════════════════════════════════════════════════════════════════════

[TestFixture]
[Category("UnitTests")]
public class TrainerScheduleServiceTests
{
    private static TrainerSchedule MakeSlot(int trainerId, int dayOfWeek,
        int startHour = 9, int endHour = 13, bool isBlocked = false)
        => new TrainerSchedule
        {
            TrainerId = trainerId,
            DayOfWeek = dayOfWeek,
            StartTime = new TimeSpan(startHour, 0, 0),
            EndTime   = new TimeSpan(endHour, 0, 0),
            IsBlocked = isBlocked
        };

    // ── GetByTrainerIdAsync ───────────────────────────────────────────────────

    [Test]
    public async Task GetByTrainerIdAsync_ReturnsNonBlockedSlots()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        ctx.TrainerSchedules.Add(MakeSlot(trainer.Id, 1));            // Monday, not blocked
        ctx.TrainerSchedules.Add(MakeSlot(trainer.Id, 2, isBlocked: true)); // Tuesday, blocked
        ctx.SaveChanges();

        var svc    = new TrainerScheduleService(ctx);
        var result = await svc.GetByTrainerIdAsync(trainer.Id);

        result.Should().HaveCount(1);
        result.First().DayOfWeek.Should().Be(1);
    }

    [Test]
    public async Task GetByTrainerIdAsync_ReturnsEmpty_WhenNoSlots()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        var svc    = new TrainerScheduleService(ctx);
        var result = await svc.GetByTrainerIdAsync(trainer.Id);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetByTrainerIdAsync_ReturnsSlotsSortedByDayThenTime()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        ctx.TrainerSchedules.Add(MakeSlot(trainer.Id, 3, 14, 18));
        ctx.TrainerSchedules.Add(MakeSlot(trainer.Id, 1, 9,  13));
        ctx.TrainerSchedules.Add(MakeSlot(trainer.Id, 1, 14, 18));
        ctx.SaveChanges();

        var svc    = new TrainerScheduleService(ctx);
        var result = await svc.GetByTrainerIdAsync(trainer.Id);

        result.First().DayOfWeek.Should().Be(1);
        result.First().StartTime.Should().Be(new TimeSpan(9, 0, 0));
        result.Last().DayOfWeek.Should().Be(3);
    }

    // ── GetAvailableTrainersAsync ─────────────────────────────────────────────

    [Test]
    public async Task GetAvailableTrainersAsync_ReturnsTrainersWithSlotsOnDay()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 5 });
        ctx.TrainerSchedules.Add(MakeSlot(trainer.Id, (int)DayOfWeek.Monday));
        ctx.SaveChanges();

        var svc    = new TrainerScheduleService(ctx);
        var result = await svc.GetAvailableTrainersAsync(DayOfWeek.Monday);

        result.Should().HaveCount(1);
        result.First().Id.Should().Be(trainer.Id);
    }

    [Test]
    public async Task GetAvailableTrainersAsync_ExcludesFullTrainers()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);
        var m1        = DbContextFactory.SeedMember(ctx, "a@t.com");
        var m2        = DbContextFactory.SeedMember(ctx, "b@t.com");

        // Capacity = 2, fill it completely
        ctx.TrainerCapacities.Add(new TrainerCapacity { TrainerId = trainer.Id, MaxTrainees = 2 });
        ctx.TrainerTrainees.Add(new TrainerTrainee { TrainerId = trainer.Id, MemberId = m1.Id, StartDate = DateTime.UtcNow, IsActive = true });
        ctx.TrainerTrainees.Add(new TrainerTrainee { TrainerId = trainer.Id, MemberId = m2.Id, StartDate = DateTime.UtcNow, IsActive = true });
        ctx.TrainerSchedules.Add(MakeSlot(trainer.Id, (int)DayOfWeek.Tuesday));
        ctx.SaveChanges();

        var svc    = new TrainerScheduleService(ctx);
        var result = await svc.GetAvailableTrainersAsync(DayOfWeek.Tuesday);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetAvailableTrainersAsync_ReturnsEmpty_WhenNoTrainersScheduledOnDay()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        DbContextFactory.SeedTrainer(ctx, loc.Id);
        // No schedule slots added

        var svc    = new TrainerScheduleService(ctx);
        var result = await svc.GetAvailableTrainersAsync(DayOfWeek.Friday);

        result.Should().BeEmpty();
    }

    // ── AddSlotAsync ──────────────────────────────────────────────────────────

    [Test]
    public async Task AddSlotAsync_PersistsNewSlot()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        var slot = MakeSlot(trainer.Id, (int)DayOfWeek.Wednesday, 10, 14);

        var svc = new TrainerScheduleService(ctx);
        await svc.AddSlotAsync(slot);

        ctx.TrainerSchedules.Should().HaveCount(1);
        ctx.TrainerSchedules.First().DayOfWeek.Should().Be((int)DayOfWeek.Wednesday);
    }

    [Test]
    public async Task AddSlotAsync_CanAddMultipleSlotsForSameTrainer()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        var svc = new TrainerScheduleService(ctx);
        await svc.AddSlotAsync(MakeSlot(trainer.Id, 1));
        await svc.AddSlotAsync(MakeSlot(trainer.Id, 3));
        await svc.AddSlotAsync(MakeSlot(trainer.Id, 5));

        ctx.TrainerSchedules.Should().HaveCount(3);
    }

    // ── RemoveSlotAsync ───────────────────────────────────────────────────────

    [Test]
    public async Task RemoveSlotAsync_DeletesSlotFromDatabase()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        var slot = MakeSlot(trainer.Id, 1);
        ctx.TrainerSchedules.Add(slot);
        ctx.SaveChanges();

        var svc = new TrainerScheduleService(ctx);
        await svc.RemoveSlotAsync(slot.Id);

        ctx.TrainerSchedules.Should().BeEmpty();
    }

    [Test]
    public async Task RemoveSlotAsync_DoesNotThrow_WhenSlotNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new TrainerScheduleService(ctx);

        Func<Task> act = () => svc.RemoveSlotAsync(9999);
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task RemoveSlotAsync_OnlyRemovesTargetSlot()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        var slot1 = MakeSlot(trainer.Id, 1);
        var slot2 = MakeSlot(trainer.Id, 3);
        ctx.TrainerSchedules.AddRange(slot1, slot2);
        ctx.SaveChanges();

        var svc = new TrainerScheduleService(ctx);
        await svc.RemoveSlotAsync(slot1.Id);

        ctx.TrainerSchedules.Should().HaveCount(1);
        ctx.TrainerSchedules.First().Id.Should().Be(slot2.Id);
    }
}
