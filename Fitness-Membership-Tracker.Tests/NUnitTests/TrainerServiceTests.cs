using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Implementations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitnessTracker.Tests.Services;

[TestFixture]
[Category("UnitTests")]
public class TrainerServiceTests
{
    // ── GetTrainersAsync ──────────────────────────────────────────────────────

    [Test]
    public async Task GetTrainersAsync_ReturnsAllNonDeletedTrainers()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        DbContextFactory.SeedTrainer(ctx, loc.Id, "Yoga");
        DbContextFactory.SeedTrainer(ctx, loc.Id, "CrossFit");

        var svc    = new TrainerService(ctx);
        var result = await svc.GetTrainersAsync(null, string.Empty);

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task GetTrainersAsync_FiltersDeletedTrainers()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        trainer.IsDeleted = true;
        trainer.DeletedAt = DateTime.UtcNow;
        ctx.SaveChanges();

        var svc    = new TrainerService(ctx);
        var result = await svc.GetTrainersAsync(null, string.Empty);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetTrainersAsync_FiltersByLocationId()
    {
        using var ctx = DbContextFactory.Create();
        var loc1      = DbContextFactory.SeedLocation(ctx, "Sofia");
        var loc2      = DbContextFactory.SeedLocation(ctx, "Varna");
        DbContextFactory.SeedTrainer(ctx, loc1.Id, "Yoga");
        DbContextFactory.SeedTrainer(ctx, loc2.Id, "Pilates");

        var svc    = new TrainerService(ctx);
        var result = await svc.GetTrainersAsync(loc1.Id, string.Empty);

        result.Should().HaveCount(1);
        result.First().Location!.City.Should().Be("Sofia");
    }

    [Test]
    public async Task GetTrainersAsync_SearchByFirstName()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        trainer.FirstName = "Unique";
        ctx.SaveChanges();

        var svc    = new TrainerService(ctx);
        var result = await svc.GetTrainersAsync(null, "Unique");

        result.Should().HaveCount(1);
        result.First().FirstName.Should().Be("Unique");
    }

    [Test]
    public async Task GetTrainersAsync_SearchByLastName()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        trainer.LastName = "Landmark";
        ctx.SaveChanges();

        var svc    = new TrainerService(ctx);
        var result = await svc.GetTrainersAsync(null, "Landmark");

        result.Should().HaveCount(1);
    }

    [Test]
    public async Task GetTrainersAsync_SearchByEmail()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        DbContextFactory.SeedTrainer(ctx, loc.Id); // email: john.smith@gym.com

        var svc    = new TrainerService(ctx);
        var result = await svc.GetTrainersAsync(null, "john.smith");

        result.Should().HaveCount(1);
    }

    [Test]
    public async Task GetTrainersAsync_ReturnsEmpty_WhenSearchMatchesNothing()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        DbContextFactory.SeedTrainer(ctx, loc.Id);

        var svc    = new TrainerService(ctx);
        var result = await svc.GetTrainersAsync(null, "XXXXXXXXX");

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetTrainersAsync_CombinesLocationAndSearch()
    {
        using var ctx = DbContextFactory.Create();
        var loc1      = DbContextFactory.SeedLocation(ctx, "Sofia");
        var loc2      = DbContextFactory.SeedLocation(ctx, "Plovdiv");

        var t1 = DbContextFactory.SeedTrainer(ctx, loc1.Id);
        t1.FirstName = "Target";
        ctx.SaveChanges();

        var t2 = DbContextFactory.SeedTrainer(ctx, loc2.Id);
        t2.FirstName = "Target";
        ctx.SaveChanges();

        var svc    = new TrainerService(ctx);
        // loc1 AND name "Target" → only t1 should match
        var result = await svc.GetTrainersAsync(loc1.Id, "Target");

        result.Should().HaveCount(1);
        result.First().LocationId.Should().Be(loc1.Id);
    }

    // ── GetByIdAsync ──────────────────────────────────────────────────────────

    [Test]
    public async Task GetByIdAsync_ReturnsTrainer()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id, "Swimming");

        var svc    = new TrainerService(ctx);
        var result = await svc.GetByIdAsync(trainer.Id);

        result.Should().NotBeNull();
        result!.Specialization.Should().Be("Swimming");
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new TrainerService(ctx);

        var result = await svc.GetByIdAsync(9999);

        result.Should().BeNull();
    }

    [Test]
    public async Task GetByIdAsync_IncludesLocationNavigation()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx, "Burgas");
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        var svc    = new TrainerService(ctx);
        var result = await svc.GetByIdAsync(trainer.Id);

        result!.Location.Should().NotBeNull();
        result.Location!.City.Should().Be("Burgas");
    }

    // ── CreateAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task CreateAsync_PersistsTrainer()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);

        var trainer = new Trainer
        {
            FirstName      = "New",
            LastName       = "Trainer",
            Email          = "new.trainer@gym.com",
            PhoneNumber    = "0888777666",
            Specialization = "Nutrition",
            HireDate       = DateTime.Today,
            Salary         = 2500m,
            LocationId     = loc.Id
        };

        var svc = new TrainerService(ctx);
        await svc.CreateAsync(trainer);

        ctx.Trainers.IgnoreQueryFilters().Should().HaveCount(1);
    }

    // ── UpdateAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task UpdateAsync_UpdatesSpecialization()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id, "Yoga");

        trainer.Specialization = "CrossFit";

        var svc = new TrainerService(ctx);
        await svc.UpdateAsync(trainer);

        var updated = ctx.Trainers.IgnoreQueryFilters().First(t => t.Id == trainer.Id);
        updated.Specialization.Should().Be("CrossFit");
    }

    [Test]
    public async Task UpdateAsync_UpdatesSalary()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        trainer.Salary = 9999m;

        var svc = new TrainerService(ctx);
        await svc.UpdateAsync(trainer);

        ctx.Trainers.IgnoreQueryFilters().First(t => t.Id == trainer.Id)
            .Salary.Should().Be(9999m);
    }

    [Test]
    public async Task UpdateAsync_DoesNotThrow_WhenTrainerNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new TrainerService(ctx);

        var ghost = new Trainer
        {
            Id             = 9999,
            FirstName      = "Ghost",
            LastName       = "Trainer",
            Email          = "ghost@gym.com",
            PhoneNumber    = "0000000000",
            Specialization = "Yoga",
            HireDate       = DateTime.Today,
            Salary         = 0m
        };

        Func<Task> act = () => svc.UpdateAsync(ghost);
        await act.Should().NotThrowAsync();
    }

    // ── DeleteAsync (soft delete) ─────────────────────────────────────────────

    [Test]
    public async Task DeleteAsync_SoftDeletesTrainer()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        var svc = new TrainerService(ctx);
        await svc.DeleteAsync(trainer.Id);

        var deleted = ctx.Trainers.IgnoreQueryFilters().First(t => t.Id == trainer.Id);
        deleted.IsDeleted.Should().BeTrue();
        deleted.DeletedAt.Should().NotBeNull();
    }

    [Test]
    public async Task DeleteAsync_DoesNotThrow_WhenNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new TrainerService(ctx);

        Func<Task> act = () => svc.DeleteAsync(9999);
        await act.Should().NotThrowAsync();
    }

    [Test]
    public async Task DeleteAsync_HidesTrainerFromNormalQueries()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var trainer   = DbContextFactory.SeedTrainer(ctx, loc.Id);

        var svc = new TrainerService(ctx);
        await svc.DeleteAsync(trainer.Id);

        // Normal query (with global filter) should not return it
        var result = await svc.GetTrainersAsync(null, string.Empty);
        result.Should().BeEmpty();
    }
}
