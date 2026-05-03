using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Implementations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitnessTracker.Tests.Services;

[TestFixture]
[Category("UnitTests")]
public class MemberServiceTests
{
    // ── GetAllAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task GetAllAsync_ReturnsAllNonDeletedMembers()
    {
        using var ctx = DbContextFactory.Create();
        var m1 = DbContextFactory.SeedMember(ctx, "a@test.com");
        var m2 = DbContextFactory.SeedMember(ctx, "b@test.com");

        // soft-delete m2
        m2.IsDeleted  = true;
        m2.DeletedAt  = DateTime.UtcNow;
        ctx.SaveChanges();

        var svc    = new MemberService(ctx);
        var result = await svc.GetAllAsync();

        result.Should().HaveCount(1);
        result.First().Email.Should().Be("a@test.com");
    }

    [Test]
    public async Task GetAllAsync_ReturnsEmptyList_WhenNoMembers()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new MemberService(ctx);

        var result = await svc.GetAllAsync();

        result.Should().BeEmpty();
    }

    // ── GetByIdAsync ──────────────────────────────────────────────────────────

    [Test]
    public async Task GetByIdAsync_ReturnsCorrectMember()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        var svc    = new MemberService(ctx);
        var result = await svc.GetByIdAsync(member.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(member.Id);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNull_WhenMemberNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new MemberService(ctx);

        var result = await svc.GetByIdAsync(Guid.NewGuid().ToString());

        result.Should().BeNull();
    }

    // ── GetByNameAsync ────────────────────────────────────────────────────────

    [Test]
    public async Task GetByNameAsync_FindsMemberByUsername()
    {
        using var ctx = DbContextFactory.Create();
        DbContextFactory.SeedMember(ctx, "user@test.com");

        var svc    = new MemberService(ctx);
        var result = await svc.GetByNameAsync("user@test.com");

        result.Should().NotBeNull();
        result!.Email.Should().Be("user@test.com");
    }

    [Test]
    public async Task GetByNameAsync_ReturnsNull_ForUnknownUsername()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new MemberService(ctx);

        var result = await svc.GetByNameAsync("nobody@nowhere.com");

        result.Should().BeNull();
    }

    // ── GetMembersWithoutMembership ───────────────────────────────────────────

    [Test]
    public async Task GetMembersWithoutMembership_ReturnsOnlyUnlinkedMembers()
    {
        using var ctx = DbContextFactory.Create();

        var loc        = DbContextFactory.SeedLocation(ctx);
        var tier       = DbContextFactory.SeedTier(ctx);
        var membership = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);

        var withMembership    = DbContextFactory.SeedMember(ctx, "has@test.com");
        withMembership.MembershipId = membership.Id;
        ctx.SaveChanges();

        DbContextFactory.SeedMember(ctx, "noMembership@test.com");

        var svc    = new MemberService(ctx);
        var result = await svc.GetMembersWithoutMembership();

        result.Should().NotBeNull();
        result!.Should().HaveCount(1);
        result!.First().Email.Should().Be("noMembership@test.com");
    }

    // ── UpdateAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task UpdateAsync_UpdatesMemberFields()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx, "old@test.com");

        member.Email       = "new@test.com";
        member.UserName    = "new@test.com";
        member.PhoneNumber = "0888123456";

        var svc = new MemberService(ctx);
        await svc.UpdateAsync(member);

        using var ctx2   = DbContextFactory.Create(ctx.Database.GetDbConnection().Database);
        var updated = ctx.Members.Find(member.Id);
        updated!.Email.Should().Be("new@test.com");
    }

    [Test]
    public async Task UpdateAsync_DoesNotThrow_WhenMemberNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new MemberService(ctx);

        var ghost = new Member { Id = Guid.NewGuid().ToString(), UserName = "x", Email = "x@x.com" };

        Func<Task> act = () => svc.UpdateAsync(ghost);
        await act.Should().NotThrowAsync();
    }

    // ── DeleteAsync (soft delete) ─────────────────────────────────────────────

    [Test]
    public async Task DeleteAsync_SoftDeletesMember()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        var svc = new MemberService(ctx);
        await svc.DeleteAsync(member.Id);

        var deleted = ctx.Members.IgnoreQueryFilters().First(m => m.Id == member.Id);
        deleted.IsDeleted.Should().BeTrue();
        deleted.DeletedAt.Should().NotBeNull();
    }

    [Test]
    public async Task DeleteAsync_DoesNotThrow_WhenMemberNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new MemberService(ctx);

        Func<Task> act = () => svc.DeleteAsync(Guid.NewGuid().ToString());
        await act.Should().NotThrowAsync();
    }
}
