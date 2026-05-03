using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Implementations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitnessTracker.Tests.Services;

[TestFixture]
[Category("UnitTests")]
public class PaymentServiceTests
{
    // ── GetAllAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task GetAllAsync_ReturnsAllNonDeletedPayments()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);
        var emp       = DbContextFactory.SeedEmployee(ctx, loc.Id);

        ctx.Payments.Add(new Payment
        {
            Currency      = "EUR",
            Amount        = 29.99m,
            PaymentDate   = DateTime.UtcNow,
            PaymentMethod = "OnSite",
            MemberId      = member.Id,
            MembershipId  = ms.Id,
            EmployeeId    = emp.Id
        });
        ctx.SaveChanges();

        var svc    = new PaymentService(ctx);
        var result = await svc.GetAllAsync();

        result.Should().HaveCount(1);
    }

    [Test]
    public async Task GetAllAsync_ExcludesSoftDeletedPayments()
    {
        using var ctx  = DbContextFactory.Create();
        var loc        = DbContextFactory.SeedLocation(ctx);
        var tier       = DbContextFactory.SeedTier(ctx);
        var ms         = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member     = DbContextFactory.SeedMember(ctx);

        var payment = new Payment
        {
            Currency      = "EUR",
            Amount        = 49.99m,
            PaymentDate   = DateTime.UtcNow,
            PaymentMethod = "Card",
            MemberId      = member.Id,
            MembershipId  = ms.Id,
            IsDeleted     = true,
            DeletedAt     = DateTime.UtcNow
        };
        ctx.Payments.Add(payment);
        ctx.SaveChanges();

        var svc    = new PaymentService(ctx);
        var result = await svc.GetAllAsync();

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetAllAsync_IncludesMemberAndMembershipNavigationProperties()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx, "nav@test.com");

        ctx.Payments.Add(new Payment
        {
            Currency      = "EUR",
            Amount        = 29.99m,
            PaymentDate   = DateTime.UtcNow,
            PaymentMethod = "OnSite",
            MemberId      = member.Id,
            MembershipId  = ms.Id
        });
        ctx.SaveChanges();

        var svc    = new PaymentService(ctx);
        var result = await svc.GetAllAsync();

        result.First().Member.Should().NotBeNull();
        result.First().Member!.Email.Should().Be("nav@test.com");
        result.First().Membership.Should().NotBeNull();
    }

    // ── GetByMemberIdAsync ────────────────────────────────────────────────────

    [Test]
    public async Task GetByMemberIdAsync_ReturnsOnlyPaymentsForThatMember()
    {
        using var ctx  = DbContextFactory.Create();
        var loc        = DbContextFactory.SeedLocation(ctx);
        var tier       = DbContextFactory.SeedTier(ctx);
        var ms         = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member1    = DbContextFactory.SeedMember(ctx, "m1@test.com");
        var member2    = DbContextFactory.SeedMember(ctx, "m2@test.com");

        ctx.Payments.Add(new Payment
        {
            Currency = "EUR", Amount = 29.99m, PaymentDate = DateTime.UtcNow,
            PaymentMethod = "OnSite", MemberId = member1.Id, MembershipId = ms.Id
        });
        ctx.Payments.Add(new Payment
        {
            Currency = "EUR", Amount = 49.99m, PaymentDate = DateTime.UtcNow,
            PaymentMethod = "Card", MemberId = member2.Id, MembershipId = ms.Id
        });
        ctx.SaveChanges();

        var svc    = new PaymentService(ctx);
        var result = await svc.GetByMemberIdAsync(member1.Id);

        result.Should().HaveCount(1);
        result.First().MemberId.Should().Be(member1.Id);
    }

    [Test]
    public async Task GetByMemberIdAsync_ReturnsEmpty_WhenMemberHasNoPayments()
    {
        using var ctx = DbContextFactory.Create();
        var member    = DbContextFactory.SeedMember(ctx);

        var svc    = new PaymentService(ctx);
        var result = await svc.GetByMemberIdAsync(member.Id);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetByMemberIdAsync_IncludesMembershipNavigation()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        ctx.Payments.Add(new Payment
        {
            Currency = "EUR", Amount = 29.99m, PaymentDate = DateTime.UtcNow,
            PaymentMethod = "OnSite", MemberId = member.Id, MembershipId = ms.Id
        });
        ctx.SaveChanges();

        var svc    = new PaymentService(ctx);
        var result = await svc.GetByMemberIdAsync(member.Id);

        result.First().Membership.Should().NotBeNull();
    }

    // ── CreateAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task CreateAsync_PersistsPayment()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var payment = new Payment
        {
            Currency      = "EUR",
            Amount        = 79.99m,
            PaymentDate   = DateTime.UtcNow,
            PaymentMethod = "Card",
            MemberId      = member.Id,
            MembershipId  = ms.Id
        };

        var svc = new PaymentService(ctx);
        await svc.CreateAsync(payment);

        ctx.Payments.IgnoreQueryFilters().Should().HaveCount(1);
    }

    [Test]
    public async Task CreateAsync_SetsPaymentDateToUtcNow()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var before = DateTime.UtcNow;

        var payment = new Payment
        {
            Currency      = "EUR",
            Amount        = 29.99m,
            PaymentMethod = "OnSite",
            MemberId      = member.Id,
            MembershipId  = ms.Id
        };

        var svc = new PaymentService(ctx);
        await svc.CreateAsync(payment);

        var after = DateTime.UtcNow;

        var saved = ctx.Payments.IgnoreQueryFilters().First();
        saved.PaymentDate.Should().BeOnOrAfter(before).And.BeOnOrBefore(after);
    }

    [Test]
    public async Task CreateAsync_StoresCorrectAmount()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var payment = new Payment
        {
            Currency      = "EUR",
            Amount        = 119.99m,
            PaymentMethod = "Card",
            MemberId      = member.Id,
            MembershipId  = ms.Id
        };

        var svc = new PaymentService(ctx);
        await svc.CreateAsync(payment);

        ctx.Payments.IgnoreQueryFilters().First().Amount.Should().Be(119.99m);
    }

    [Test]
    public async Task CreateAsync_MultiplePayments_AllPersisted()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var tier      = DbContextFactory.SeedTier(ctx);
        var ms        = DbContextFactory.SeedMembership(ctx, loc.Id, tier.Id);
        var member    = DbContextFactory.SeedMember(ctx);

        var svc = new PaymentService(ctx);

        for (int i = 0; i < 3; i++)
        {
            await svc.CreateAsync(new Payment
            {
                Currency      = "EUR",
                Amount        = 29.99m + i,
                PaymentMethod = "OnSite",
                MemberId      = member.Id,
                MembershipId  = ms.Id
            });
        }

        ctx.Payments.IgnoreQueryFilters().Should().HaveCount(3);
    }
}
