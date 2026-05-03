using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Implementations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitnessTracker.Tests.Services;

[TestFixture]
[Category("UnitTests")]
public class WorkoutServiceTests
{
    // ── helpers ───────────────────────────────────────────────────────────────

    private static WorkoutLog MakeLog(string memberId, DateTime? logDate = null,
        string notes = "Test session") =>
        new WorkoutLog
        {
            MemberId  = memberId,
            LogDate   = logDate ?? DateTime.UtcNow,
            Notes     = notes,
            IsDeleted = false,
            Exercises = new List<WorkoutExercise>
            {
                new WorkoutExercise
                {
                    ExerciseName = "Bench Press",
                    Sets         = 4,
                    Reps         = 8,
                    WeightKg     = 80m
                }
            }
        };

    // ── GetByMemberIdAsync ────────────────────────────────────────────────────

    [Test]
    public async Task GetByMemberIdAsync_ReturnsOnlyThatMembersLogs()
    {
        using var ctx = DbContextFactory.Create();
        var m1        = DbContextFactory.SeedMember(ctx, "m1@t.com");
        var m2        = DbContextFactory.SeedMember(ctx, "m2@t.com");

        ctx.WorkoutLogs.Add(MakeLog(m1.Id));
        ctx.WorkoutLogs.Add(MakeLog(m1.Id));
        ctx.WorkoutLogs.Add(MakeLog(m2.Id));
        ctx.SaveChanges();

        var svc    = new WorkoutService(ctx);
        var result = await svc.GetByMemberIdAsync(m1.Id);

        result.Should().HaveCount(2);
        result.Should().AllSatisfy(l => l.MemberId.Should().Be(m1.Id));
    }

    [Test]
    public async Task GetByMemberIdAsync_ReturnsEmpty_WhenNoLogs()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        var svc    = new WorkoutService(ctx);
        var result = await svc.GetByMemberIdAsync(member.Id);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetByMemberIdAsync_ExcludesSoftDeletedLogs()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        var log = MakeLog(member.Id);
        ctx.WorkoutLogs.Add(log);
        ctx.SaveChanges();

        log.IsDeleted = true;
        log.DeletedAt = DateTime.UtcNow;
        ctx.SaveChanges();

        var svc    = new WorkoutService(ctx);
        var result = await svc.GetByMemberIdAsync(member.Id);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetByMemberIdAsync_ReturnsLogsOrderedDescendingByDate()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.WorkoutLogs.Add(MakeLog(member.Id, DateTime.UtcNow.AddDays(-10)));
        ctx.WorkoutLogs.Add(MakeLog(member.Id, DateTime.UtcNow.AddDays(-1)));
        ctx.WorkoutLogs.Add(MakeLog(member.Id, DateTime.UtcNow.AddDays(-5)));
        ctx.SaveChanges();

        var svc    = new WorkoutService(ctx);
        var result = await svc.GetByMemberIdAsync(member.Id);

        result.Should().BeInDescendingOrder(l => l.LogDate);
    }

    [Test]
    public async Task GetByMemberIdAsync_IncludesExercises()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.WorkoutLogs.Add(MakeLog(member.Id));
        ctx.SaveChanges();

        var svc    = new WorkoutService(ctx);
        var result = await svc.GetByMemberIdAsync(member.Id);

        result.First().Exercises.Should().NotBeEmpty();
        result.First().Exercises.First().ExerciseName.Should().Be("Bench Press");
    }

    // ── GetByIdAsync ──────────────────────────────────────────────────────────

    [Test]
    public async Task GetByIdAsync_ReturnsCorrectLog()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        var log = MakeLog(member.Id, notes: "Leg day");
        ctx.WorkoutLogs.Add(log);
        ctx.SaveChanges();

        var svc    = new WorkoutService(ctx);
        var result = await svc.GetByIdAsync(log.Id);

        result.Should().NotBeNull();
        result!.Notes.Should().Be("Leg day");
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new WorkoutService(ctx);

        var result = await svc.GetByIdAsync(9999);

        result.Should().BeNull();
    }

    [Test]
    public async Task GetByIdAsync_IncludesExercisesAndMember()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx, "ex@test.com");

        var log = MakeLog(member.Id);
        ctx.WorkoutLogs.Add(log);
        ctx.SaveChanges();

        var svc    = new WorkoutService(ctx);
        var result = await svc.GetByIdAsync(log.Id);

        result!.Exercises.Should().NotBeEmpty();
        result.Member.Should().NotBeNull();
        result.Member!.Email.Should().Be("ex@test.com");
    }

    // ── CreateAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task CreateAsync_PersistsLogWithExercises()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        var log = MakeLog(member.Id);

        var svc = new WorkoutService(ctx);
        await svc.CreateAsync(log);

        ctx.WorkoutLogs.IgnoreQueryFilters().Should().HaveCount(1);
        ctx.WorkoutExercises.Should().HaveCount(1);
    }

    [Test]
    public async Task CreateAsync_SetsLogDateToUtcNow()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        var before = DateTime.UtcNow;
        var log    = new WorkoutLog { MemberId = member.Id, Notes = "Test", Exercises = new List<WorkoutExercise>() };

        var svc = new WorkoutService(ctx);
        await svc.CreateAsync(log);

        var after = DateTime.UtcNow;
        ctx.WorkoutLogs.IgnoreQueryFilters().First().LogDate
            .Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }

    [Test]
    public async Task CreateAsync_PersistsMultipleExercisesInLog()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        var log = new WorkoutLog
        {
            MemberId  = member.Id,
            Notes     = "Upper body",
            Exercises = new List<WorkoutExercise>
            {
                new WorkoutExercise { ExerciseName = "Bench Press", Sets = 4, Reps = 8, WeightKg = 80m },
                new WorkoutExercise { ExerciseName = "Overhead Press", Sets = 4, Reps = 8, WeightKg = 50m },
                new WorkoutExercise { ExerciseName = "Lateral Raise", Sets = 3, Reps = 15, WeightKg = 10m }
            }
        };

        var svc = new WorkoutService(ctx);
        await svc.CreateAsync(log);

        ctx.WorkoutExercises.Should().HaveCount(3);
    }

    [Test]
    public async Task CreateAsync_PreservesExerciseDetails()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        var log = new WorkoutLog
        {
            MemberId = member.Id,
            Notes    = "Strength",
            Exercises = new List<WorkoutExercise>
            {
                new WorkoutExercise
                {
                    ExerciseName    = "Deadlift",
                    Sets            = 3,
                    Reps            = 5,
                    WeightKg        = 120m,
                    DurationMinutes = null,
                    Notes           = "New PR"
                }
            }
        };

        var svc = new WorkoutService(ctx);
        await svc.CreateAsync(log);

        var saved = ctx.WorkoutExercises.First();
        saved.ExerciseName.Should().Be("Deadlift");
        saved.Sets.Should().Be(3);
        saved.Reps.Should().Be(5);
        saved.WeightKg.Should().Be(120m);
        saved.Notes.Should().Be("New PR");
    }

    // ── DeleteAsync (soft delete) ─────────────────────────────────────────────

    [Test]
    public async Task DeleteAsync_SoftDeletesLog()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        var log = MakeLog(member.Id);
        ctx.WorkoutLogs.Add(log);
        ctx.SaveChanges();

        var svc = new WorkoutService(ctx);
        await svc.DeleteAsync(log.Id);

        var deleted = ctx.WorkoutLogs.IgnoreQueryFilters().First(l => l.Id == log.Id);
        deleted.IsDeleted.Should().BeTrue();
        deleted.DeletedAt.Should().NotBeNull();
    }

    [Test]
    public async Task DeleteAsync_HidesLogFromNormalQueries()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        var log = MakeLog(member.Id);
        ctx.WorkoutLogs.Add(log);
        ctx.SaveChanges();

        var svc = new WorkoutService(ctx);
        await svc.DeleteAsync(log.Id);

        var result = await svc.GetByMemberIdAsync(member.Id);
        result.Should().BeEmpty();
    }

    [Test]
    public async Task DeleteAsync_DoesNotThrow_WhenLogNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new WorkoutService(ctx);

        Func<Task> act = () => svc.DeleteAsync(9999);
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task DeleteAsync_SetsDeletedAtToUtcNow()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        var log = MakeLog(member.Id);
        ctx.WorkoutLogs.Add(log);
        ctx.SaveChanges();

        var before = DateTime.UtcNow;
        var svc    = new WorkoutService(ctx);
        await svc.DeleteAsync(log.Id);
        var after  = DateTime.UtcNow;

        var deleted = ctx.WorkoutLogs.IgnoreQueryFilters().First(l => l.Id == log.Id);
        deleted.DeletedAt.Should().NotBeNull();
        deleted.DeletedAt!.Value.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }
}
