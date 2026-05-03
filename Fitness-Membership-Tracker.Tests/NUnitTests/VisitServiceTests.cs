using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Implementations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitnessTracker.Tests.Services;

[TestFixture]
[Category("UnitTests")]
public class VisitServiceTests
{
    // ── helpers ───────────────────────────────────────────────────────────────

    private static Visit MakeVisit(string memberId, int locationId, int membershipId,
        DateTime? visitDate = null)
        => new Visit
        {
            MemberId     = memberId,
            LocationId   = locationId,
            MembershipId = membershipId,
            VisitDate    = visitDate ?? DateTime.UtcNow,
            IsDeleted    = false
        };

    // ── GetAllAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task GetAllAsync_ReturnsAllNonDeletedVisits()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.Visits.Add(MakeVisit(member.Id, loc.Id, ms.Id));
        ctx.Visits.Add(MakeVisit(member.Id, loc.Id, ms.Id,
            DateTime.UtcNow.AddDays(-1)));
        ctx.SaveChanges();

        var svc    = new VisitService(ctx);
        var result = await svc.GetAllAsync();

        result.Should().HaveCount(1);
    }

    [Test]
    public async Task GetAllAsync_ReturnsVisitsOrderedByDateDescending()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.Visits.Add(MakeVisit(member.Id, loc.Id, ms.Id, DateTime.UtcNow.AddDays(-5)));
        ctx.Visits.Add(MakeVisit(member.Id, loc.Id, ms.Id, DateTime.UtcNow.AddDays(-1)));
        ctx.Visits.Add(MakeVisit(member.Id, loc.Id, ms.Id, DateTime.UtcNow.AddDays(-3)));
        ctx.SaveChanges();

        var svc    = new VisitService(ctx);
        var result = await svc.GetAllAsync();

        result.Should().BeInDescendingOrder(v => v.VisitDate);
    }

    [Test]
    public async Task GetAllAsync_IncludesNavigationProperties()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx, "Burgas");
        var tier      = DbContextFactory.SeedTier(ctx, "Elite");
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx, "nav@test.com");

        ctx.Visits.Add(MakeVisit(member.Id, loc.Id, ms.Id));
        ctx.SaveChanges();

        var svc    = new VisitService(ctx);
        var result = await svc.GetAllAsync();

        result.First().Member.Should().NotBeNull();
        result.First().Location.Should().NotBeNull();
        result.First().Location!.City.Should().Be("Burgas");
    }

    // ── GetByMemberIdAsync ────────────────────────────────────────────────────

    [Test]
    public async Task GetByMemberIdAsync_ReturnsOnlyThatMembersVisits()
    {
        using var ctx  = DbContextFactory.Create();
        var loc        = DbContextFactory.SeedLocation(ctx);
        var tier       = DbContextFactory.SeedTier(ctx);
        var ms         = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member1    = DbContextFactory.SeedMember(ctx, "m1@test.com");
        var member2    = DbContextFactory.SeedMember(ctx, "m2@test.com");

        ctx.Visits.Add(MakeVisit(member1.Id, loc.Id, ms.Id));
        ctx.Visits.Add(MakeVisit(member1.Id, loc.Id, ms.Id));
        ctx.Visits.Add(MakeVisit(member2.Id, loc.Id, ms.Id));
        ctx.SaveChanges();

        var svc    = new VisitService(ctx);
        var result = await svc.GetByMemberIdAsync(member1.Id);

        result.Should().HaveCount(2);
        result.Should().AllSatisfy(v => v.MemberId.Should().Be(member1.Id));
    }

    [Test]
    public async Task GetByMemberIdAsync_ReturnsEmpty_WhenNoVisits()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        var svc    = new VisitService(ctx);
        var result = await svc.GetByMemberIdAsync(member.Id);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetByMemberIdAsync_ReturnsDescendingOrder()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.Visits.Add(MakeVisit(member.Id, loc.Id, ms.Id, DateTime.UtcNow.AddDays(-10)));
        ctx.Visits.Add(MakeVisit(member.Id, loc.Id, ms.Id, DateTime.UtcNow.AddDays(-2)));
        ctx.SaveChanges();

        var svc    = new VisitService(ctx);
        var result = await svc.GetByMemberIdAsync(member.Id);

        result.Should().BeInDescendingOrder(v => v.VisitDate);
    }

    // ── GetByIdAsync ──────────────────────────────────────────────────────────

    [Test]
    public async Task GetByIdAsync_ReturnsCorrectVisit()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var visit = MakeVisit(member.Id, loc.Id, ms.Id);
        ctx.Visits.Add(visit);
        ctx.SaveChanges();

        var svc    = new VisitService(ctx);
        var result = await svc.GetByIdAsync(visit.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(visit.Id);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new VisitService(ctx);

        var result = await svc.GetByIdAsync(9999);

        result.Should().BeNull();
    }

    // ── GetByDateRangeAsync ───────────────────────────────────────────────────

    [Test]
    public async Task GetByDateRangeAsync_ReturnsVisitsWithinRange()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var inside1 = MakeVisit(member.Id, loc.Id, ms.Id, DateTime.UtcNow.AddDays(-5));
        var inside2 = MakeVisit(member.Id, loc.Id, ms.Id, DateTime.UtcNow.AddDays(-3));
        var outside = MakeVisit(member.Id, loc.Id, ms.Id, DateTime.UtcNow.AddDays(-15));

        ctx.Visits.AddRange(inside1, inside2, outside);
        ctx.SaveChanges();

        var from   = DateTime.UtcNow.AddDays(-7);
        var to     = DateTime.UtcNow;
        var svc    = new VisitService(ctx);
        var result = await svc.GetByDateRangeAsync(from, to);

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task GetByDateRangeAsync_ReturnsEmpty_WhenNoneInRange()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.Visits.Add(MakeVisit(member.Id, loc.Id, ms.Id, DateTime.UtcNow.AddDays(-30)));
        ctx.SaveChanges();

        var svc    = new VisitService(ctx);
        var result = await svc.GetByDateRangeAsync(
            DateTime.UtcNow.AddDays(-5), DateTime.UtcNow);

        result.Should().BeEmpty();
    }

    // ── GetDailyVisitCountsAsync ──────────────────────────────────────────────

    [Test]
    public async Task GetDailyVisitCountsAsync_GroupsVisitsByDate()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var day = DateTime.UtcNow.Date.AddDays(-2);

        // 3 visits on the same day
        ctx.Visits.Add(MakeVisit(member.Id, loc.Id, ms.Id, day.AddHours(8)));
        ctx.Visits.Add(MakeVisit(member.Id, loc.Id, ms.Id, day.AddHours(12)));
        ctx.Visits.Add(MakeVisit(member.Id, loc.Id, ms.Id, day.AddHours(18)));
        // 1 visit on a different day
        ctx.Visits.Add(MakeVisit(member.Id, loc.Id, ms.Id, day.AddDays(-1)));
        ctx.SaveChanges();

        var svc    = new VisitService(ctx);
        var result = await svc.GetDailyVisitCountsAsync(day.AddDays(-1), day);

        result.Should().ContainKey(day).WhoseValue.Should().Be(3);
        result.Should().ContainKey(day.AddDays(-1)).WhoseValue.Should().Be(1);
    }

    [Test]
    public async Task GetDailyVisitCountsAsync_ReturnsEmptyDictionary_WhenNoVisits()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new VisitService(ctx);

        var result = await svc.GetDailyVisitCountsAsync(
            DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);

        result.Should().BeEmpty();
    }

    // ── CreateAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task CreateAsync_PersistsVisit()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var visit = MakeVisit(member.Id, loc.Id, ms.Id);

        var svc = new VisitService(ctx);
        await svc.CreateAsync(visit);

        ctx.Visits.IgnoreQueryFilters().Should().HaveCount(1);
    }

    [Test]
    public async Task CreateAsync_SetsVisitDateToUtcNow()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var before = DateTime.UtcNow;
        var visit  = new Visit { MemberId = member.Id, LocationId = loc.Id, MembershipId = ms.Id };

        var svc = new VisitService(ctx);
        await svc.CreateAsync(visit);

        var after = DateTime.UtcNow;
        var saved = ctx.Visits.IgnoreQueryFilters().First();
        saved.VisitDate.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }

    // ── DeleteAsync (soft delete) ─────────────────────────────────────────────

    [Test]
    public async Task DeleteAsync_SoftDeletesVisit()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var visit = MakeVisit(member.Id, loc.Id, ms.Id);
        ctx.Visits.Add(visit);
        ctx.SaveChanges();

        var svc = new VisitService(ctx);
        await svc.DeleteAsync(visit.Id);

        var deleted = ctx.Visits.IgnoreQueryFilters().First(v => v.Id == visit.Id);
        deleted.IsDeleted.Should().BeTrue();
        deleted.DeletedAt.Should().NotBeNull();
    }

    [Test]
    public async Task DeleteAsync_DoesNotThrow_WhenNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new VisitService(ctx);

        Func<Task> act = () => svc.DeleteAsync(9999);
        await act.Should().NotThrowAsync();
    }
}
