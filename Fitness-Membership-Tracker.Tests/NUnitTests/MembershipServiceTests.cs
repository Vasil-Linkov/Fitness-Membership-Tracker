using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Implementations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitnessTracker.Tests.Services;

[TestFixture]
[Category("UnitTests")]
public class MembershipServiceTests
{
    // ── GetAllAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task GetAllAsync_ReturnsOnlyNonDeletedMemberships()
    {
        using var ctx = DbContextFactory.Create();
        var loc  = DbContextFactory.SeedLocation(ctx);
        var tier = DbContextFactory.SeedTier(ctx);

        DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);

        var deleted = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        deleted.IsDeleted = true;
        deleted.DeletedAt = DateTime.UtcNow;
        ctx.SaveChanges();

        var svc    = new MembershipService(ctx);
        var result = await svc.GetAllAsync();

        result.Should().HaveCount(1);
    }

    [Test]
    public async Task GetAllAsync_IncludesLocationAndTier()
    {
        using var ctx = DbContextFactory.Create();
        var loc  = DbContextFactory.SeedLocation(ctx, "Varna");
        var tier = DbContextFactory.SeedTier(ctx, "Elite");
        DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);

        var svc    = new MembershipService(ctx);
        var result = await svc.GetAllAsync();

        result.First().Location.Should().NotBeNull();
        result.First().MembershipTier.Should().NotBeNull();
        result.First().Location!.City.Should().Be("Varna");
        result.First().MembershipTier.Tier.Should().Be("Elite");
    }

    // ── GetByIdAsync ──────────────────────────────────────────────────────────

    [Test]
    public async Task GetByIdAsync_ReturnsCorrectMembership()
    {
        using var ctx = DbContextFactory.Create();
        var loc  = DbContextFactory.SeedLocation(ctx);
        var tier = DbContextFactory.SeedTier(ctx);
        var ms   = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);

        var svc    = new MembershipService(ctx);
        var result = await svc.GetByIdAsync(ms.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(ms.Id);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new MembershipService(ctx);

        var result = await svc.GetByIdAsync(9999);

        result.Should().BeNull();
    }

    // ── GetMembershipByMember ─────────────────────────────────────────────────

    [Test]
    public async Task GetMembershipByMember_ReturnsMembership_WhenLinked()
    {
        using var ctx = DbContextFactory.Create();
        var loc  = DbContextFactory.SeedLocation(ctx);
        var tier = DbContextFactory.SeedTier(ctx);
        var ms   = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var m    = DbContextFactory.SeedMember(ctx);

        m.MembershipId = ms.Id;
        ctx.SaveChanges();

        var svc    = new MembershipService(ctx);
        var result = await svc.GetMembershipByMember(m);

        result.Should().NotBeNull();
        result!.Id.Should().Be(ms.Id);
    }

    [Test]
    public async Task GetMembershipByMember_ReturnsNull_WhenMemberHasNoMembership()
    {
        using var ctx = DbContextFactory.Create();
        var m         = DbContextFactory.SeedMember(ctx); // MembershipId = null

        var svc    = new MembershipService(ctx);
        var result = await svc.GetMembershipByMember(m);

        result.Should().BeNull();
    }

    [Test]
    public async Task GetMembershipByMember_ReturnsNull_WhenMemberIsNull()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new MembershipService(ctx);

        var result = await svc.GetMembershipByMember(null!);

        result.Should().BeNull();
    }

    // ── GetByIdIncludingDeletedAsync ──────────────────────────────────────────

    [Test]
    public async Task GetByIdIncludingDeletedAsync_ReturnsDeletedMembership()
    {
        using var ctx = DbContextFactory.Create();
        var loc  = DbContextFactory.SeedLocation(ctx);
        var tier = DbContextFactory.SeedTier(ctx);
        var ms   = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);

        ms.IsDeleted = true;
        ms.DeletedAt = DateTime.UtcNow;
        ctx.SaveChanges();

        var svc    = new MembershipService(ctx);
        var result = await svc.GetByIdIncludingDeletedAsync(ms.Id);

        result.Should().NotBeNull();
        result!.IsDeleted.Should().BeTrue();
    }

    // ── CreateAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task CreateAsync_PersistsMembership()
    {
        using var ctx = DbContextFactory.Create();
        var loc  = DbContextFactory.SeedLocation(ctx);
        var tier = DbContextFactory.SeedTier(ctx);

        var ms = new Membership
        {
            StartDate        = DateTime.Now,
            EndDate          = DateTime.Now.AddMonths(1),
            LocationId       = loc.Id,
            MembershipTierId = tier.Id
        };

        var svc = new MembershipService(ctx);
        await svc.CreateAsync(ms);

        ctx.Memberships.Should().HaveCount(1);
        ctx.Memberships.First().Id.Should().Be(ms.Id);
    }

    // ── UpdateAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task UpdateAsync_UpdatesDateFields()
    {
        using var ctx = DbContextFactory.Create();
        var loc  = DbContextFactory.SeedLocation(ctx);
        var tier = DbContextFactory.SeedTier(ctx);
        var ms   = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);

        var newEnd = ms.EndDate.AddMonths(3);
        ms.EndDate = newEnd;

        var svc = new MembershipService(ctx);
        await svc.UpdateAsync(ms);

        var updated = ctx.Memberships.IgnoreQueryFilters().First(m => m.Id == ms.Id);
        updated.EndDate.Should().BeCloseTo(newEnd, TimeSpan.FromSeconds(1));
    }

    [Test]
    public async Task UpdateAsync_DoesNothing_WhenMembershipNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new MembershipService(ctx);

        var ghost = new Membership
        {
            Id               = 9999,
            StartDate        = DateTime.Now,
            EndDate          = DateTime.Now.AddMonths(1),
            LocationId       = 1,
            MembershipTierId = 1
        };

        Func<Task> act = () => svc.UpdateAsync(ghost);
        await act.Should().NotThrowAsync();
    }

    // ── DeleteAsync (soft delete) ─────────────────────────────────────────────

    [Test]
    public async Task DeleteAsync_SoftDeletesMembership()
    {
        using var ctx = DbContextFactory.Create();
        var loc  = DbContextFactory.SeedLocation(ctx);
        var tier = DbContextFactory.SeedTier(ctx);
        var ms   = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);

        var svc = new MembershipService(ctx);
        await svc.DeleteAsync(ms.Id);

        var deleted = ctx.Memberships.IgnoreQueryFilters().First(m => m.Id == ms.Id);
        deleted.IsDeleted.Should().BeTrue();
        deleted.DeletedAt.Should().NotBeNull();
    }

    [Test]
    public async Task DeleteAsync_DoesNotThrow_WhenNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new MembershipService(ctx);

        Func<Task> act = () => svc.DeleteAsync(9999);
        await act.Should().NotThrowAsync();
    }
}
