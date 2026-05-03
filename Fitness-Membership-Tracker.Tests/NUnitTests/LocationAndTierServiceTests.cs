using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Implementations;
using FluentAssertions;
using NUnit.Framework;

namespace FitnessTracker.Tests.Services;

// ═══════════════════════════════════════════════════════════════════════════════
//  LocationServiceTests
// ═══════════════════════════════════════════════════════════════════════════════

[TestFixture]
[Category("UnitTests")]
public class LocationServiceTests
{
    [Test]
    public async Task GetAllAsync_ReturnsAllLocations()
    {
        using var ctx = DbContextFactory.Create();
        DbContextFactory.SeedLocation(ctx, "Sofia");
        DbContextFactory.SeedLocation(ctx, "Varna");

        var svc    = new LocationService(ctx);
        var result = await svc.GetAllAsync();

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task GetAllAsync_DoesNotReturnSoftDeletedLocations()
    {
        using var ctx = DbContextFactory.Create();
        var loc = DbContextFactory.SeedLocation(ctx);

        loc.IsDeleted = true;
        loc.DeletedAt = DateTime.UtcNow;
        ctx.SaveChanges();

        var svc    = new LocationService(ctx);
        var result = await svc.GetAllAsync();

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoLocations()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new LocationService(ctx);

        var result = await svc.GetAllAsync();

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetByIdAsync_ReturnsCorrectLocation()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx, "Plovdiv");

        var svc    = new LocationService(ctx);
        var result = await svc.GetByIdAsync(loc.Id);

        result.Should().NotBeNull();
        result!.City.Should().Be("Plovdiv");
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNull_ForUnknownId()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new LocationService(ctx);

        var result = await svc.GetByIdAsync(9999);

        result.Should().BeNull();
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNull_ForSoftDeletedLocation()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);

        loc.IsDeleted = true;
        loc.DeletedAt = DateTime.UtcNow;
        ctx.SaveChanges();

        var svc    = new LocationService(ctx);
        var result = await svc.GetByIdAsync(loc.Id);

        result.Should().BeNull();
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
//  MembershipTierServiceTests
// ═══════════════════════════════════════════════════════════════════════════════

[TestFixture]
[Category("UnitTests")]
public class MembershipTierServiceTests
{
    [Test]
    public async Task GetTiersAsync_ReturnsAllTiers()
    {
        using var ctx = DbContextFactory.Create();
        DbContextFactory.SeedTier(ctx, "Basic",    29.99m);
        DbContextFactory.SeedTier(ctx, "Advanced", 49.99m);
        DbContextFactory.SeedTier(ctx, "Elite",    79.99m);

        var svc    = new MembershipTierService(ctx);
        var result = await svc.GetTiersAsync();

        result.Should().HaveCount(3);
    }

    [Test]
    public async Task GetTiersAsync_ReturnsEmptyList_WhenNoTiers()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new MembershipTierService(ctx);

        var result = await svc.GetTiersAsync();

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetByIdAsync_ReturnsCorrectTier()
    {
        using var ctx = DbContextFactory.Create();
        var tier      = DbContextFactory.SeedTier(ctx, "Ultimate", 119.99m, 24);

        var svc    = new MembershipTierService(ctx);
        var result = await svc.GetByIdAsync(tier.Id);

        result.Should().NotBeNull();
        result!.Tier.Should().Be("Ultimate");
        result.MonthlyPrice.Should().Be(119.99m);
        result.MaxSessionsPerMonth.Should().Be(24);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new MembershipTierService(ctx);

        var result = await svc.GetByIdAsync(9999);

        result.Should().BeNull();
    }

    [Test]
    public async Task GetTiersAsync_ReturnsCorrectPrices()
    {
        using var ctx = DbContextFactory.Create();
        DbContextFactory.SeedTier(ctx, "Basic", 29.99m);

        var svc    = new MembershipTierService(ctx);
        var result = await svc.GetTiersAsync();

        result.First().MonthlyPrice.Should().Be(29.99m);
    }
}
