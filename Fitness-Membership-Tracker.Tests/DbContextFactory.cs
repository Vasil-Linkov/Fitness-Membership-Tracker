using Fitness_Membership_Tracker.Data;
using Fitness_Membership_Tracker.Data.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Tests;

/// <summary>
/// Creates an isolated, in-memory <see cref="ApplicationDbContext"/> for each test.
/// Each call produces a unique database name so tests never share state.
/// </summary>
public static class DbContextFactory
{
    public static ApplicationDbContext Create(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    // ── Seed helpers ──────────────────────────────────────────────────────────

    public static Location SeedLocation(ApplicationDbContext ctx, string city = "Sofia")
    {
        var location = new Location
        {
            Address = "Test Street 1",
            City    = city,
            Country = "Bulgaria"
        };
        ctx.Locations.Add(location);
        ctx.SaveChanges();
        return location;
    }

    public static MembershipTier SeedTier(ApplicationDbContext ctx, string name = "Basic",
        decimal price = 29.99m, int sessions = 8)
    {
        var tier = new MembershipTier
        {
            Tier               = name,
            MonthlyPrice       = price,
            Description        = $"{name} tier description",
            Accessibility      = "address",
            MaxSessionsPerMonth = sessions
        };
        ctx.MembershipTiers.Add(tier);
        ctx.SaveChanges();
        return tier;
    }

    public static Member SeedMember(ApplicationDbContext ctx, string email = "test@test.com")
    {
        var member = new Member
        {
            Id             = Guid.NewGuid().ToString(),
            UserName       = email,
            Email          = email,
            EmailConfirmed = true
        };
        ctx.Members.Add(member);
        ctx.SaveChanges();
        return member;
    }

    public static Membership SeedMembership(ApplicationDbContext ctx, int locationId, int tierId,
        int daysActive = 30)
    {
        var membership = new Membership
        {
            StartDate        = DateTime.UtcNow.AddDays(-daysActive),
            EndDate          = DateTime.UtcNow.AddDays(30),
            LocationId       = locationId,
            MembershipTierId = tierId,
            IsDeleted        = false
        };
        ctx.Memberships.Add(membership);
        ctx.SaveChanges();
        return membership;
    }

    public static Employee SeedEmployee(ApplicationDbContext ctx, int locationId)
    {
        var emp = new Employee
        {
            FirstName   = "Jane",
            LastName    = "Doe",
            Email       = "jane.doe@gym.com",
            PhoneNumber = "0888000000",
            HireDate    = new DateTime(2020, 1, 1),
            Salary      = 1500m,
            LocationId  = locationId
        };
        ctx.Employees.Add(emp);
        ctx.SaveChanges();
        return emp;
    }

    public static Trainer SeedTrainer(ApplicationDbContext ctx, int locationId,
        string specialization = "Yoga")
    {
        var trainer = new Trainer
        {
            FirstName      = "John",
            LastName       = "Smith",
            Email          = "john.smith@gym.com",
            PhoneNumber    = "0888111111",
            Specialization = specialization,
            HireDate       = new DateTime(2021, 6, 1),
            Salary         = 2000m,
            LocationId     = locationId
        };
        ctx.Trainers.Add(trainer);
        ctx.SaveChanges();
        return trainer;
    }
}
