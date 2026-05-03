using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services.Implementations;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FitnessTracker.Tests.Services;

[TestFixture]
[Category("UnitTests")]
public class EmployeeServiceTests
{
    // ── GetEmployeesAsync ─────────────────────────────────────────────────────

    [Test]
    public async Task GetEmployeesAsync_ReturnsAllEmployees_WhenNoFilters()
    {
        using var ctx = DbContextFactory.Create();
        var loc = DbContextFactory.SeedLocation(ctx);
        DbContextFactory.SeedEmployee(ctx, loc.Id);
        DbContextFactory.SeedEmployee(ctx, loc.Id);

        var svc    = new EmployeeService(ctx);
        var result = await svc.GetEmployeesAsync(null, string.Empty);

        result.Should().HaveCount(2);
    }

    [Test]
    public async Task GetEmployeesAsync_FiltersDeletedEmployees()
    {
        using var ctx = DbContextFactory.Create();
        var loc = DbContextFactory.SeedLocation(ctx);

        var emp = DbContextFactory.SeedEmployee(ctx, loc.Id);
        emp.IsDeleted = true;
        emp.DeletedAt = DateTime.UtcNow;
        ctx.SaveChanges();

        var svc    = new EmployeeService(ctx);
        var result = await svc.GetEmployeesAsync(null, string.Empty);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetEmployeesAsync_FiltersByLocationId()
    {
        using var ctx  = DbContextFactory.Create();
        var loc1 = DbContextFactory.SeedLocation(ctx, "Sofia");
        var loc2 = DbContextFactory.SeedLocation(ctx, "Plovdiv");

        DbContextFactory.SeedEmployee(ctx, loc1.Id);
        DbContextFactory.SeedEmployee(ctx, loc2.Id);

        var svc    = new EmployeeService(ctx);
        var result = await svc.GetEmployeesAsync(loc1.Id, string.Empty);

        result.Should().HaveCount(1);
        result.First().Location!.City.Should().Be("Sofia");
    }

    [Test]
    public async Task GetEmployeesAsync_SearchByFirstName()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);

        var emp = DbContextFactory.SeedEmployee(ctx, loc.Id);
        emp.FirstName = "Unique";
        ctx.SaveChanges();

        var svc    = new EmployeeService(ctx);
        var result = await svc.GetEmployeesAsync(null, "Unique");

        result.Should().HaveCount(1);
        result.First().FirstName.Should().Be("Unique");
    }

    [Test]
    public async Task GetEmployeesAsync_SearchByEmail()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        DbContextFactory.SeedEmployee(ctx, loc.Id); // email: jane.doe@gym.com

        var svc    = new EmployeeService(ctx);
        var result = await svc.GetEmployeesAsync(null, "jane.doe");

        result.Should().HaveCount(1);
    }

    [Test]
    public async Task GetEmployeesAsync_ReturnsEmpty_WhenSearchMatchesNothing()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        DbContextFactory.SeedEmployee(ctx, loc.Id);

        var svc    = new EmployeeService(ctx);
        var result = await svc.GetEmployeesAsync(null, "XXXXXXXXXX");

        result.Should().BeEmpty();
    }

    // ── GetByIdAsync ──────────────────────────────────────────────────────────

    [Test]
    public async Task GetByIdAsync_ReturnsEmployee()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var emp       = DbContextFactory.SeedEmployee(ctx, loc.Id);

        var svc    = new EmployeeService(ctx);
        var result = await svc.GetByIdAsync(emp.Id);

        result.Should().NotBeNull();
        result!.Id.Should().Be(emp.Id);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new EmployeeService(ctx);

        var result = await svc.GetByIdAsync(9999);

        result.Should().BeNull();
    }

    // ── CreateAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task CreateAsync_PersistsEmployee()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);

        var emp = new Employee
        {
            FirstName   = "New",
            LastName    = "Employee",
            Email       = "new.employee@gym.com",
            PhoneNumber = "0888999999",
            HireDate    = DateTime.Today,
            Salary      = 1800m,
            LocationId  = loc.Id
        };

        var svc = new EmployeeService(ctx);
        await svc.CreateAsync(emp);

        ctx.Employees.IgnoreQueryFilters().Should().HaveCount(1);
    }

    // ── UpdateAsync ───────────────────────────────────────────────────────────

    [Test]
    public async Task UpdateAsync_UpdatesSalary()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var emp       = DbContextFactory.SeedEmployee(ctx, loc.Id);

        emp.Salary = 9999m;

        var svc = new EmployeeService(ctx);
        await svc.UpdateAsync(emp);

        var updated = ctx.Employees.IgnoreQueryFilters().First(e => e.Id == emp.Id);
        updated.Salary.Should().Be(9999m);
    }

    [Test]
    public async Task UpdateAsync_DoesNotThrow_WhenEmployeeNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new EmployeeService(ctx);

        var ghost = new Employee
        {
            Id          = 9999,
            FirstName   = "Ghost",
            LastName    = "User",
            Email       = "ghost@gym.com",
            PhoneNumber = "0000000000",
            HireDate    = DateTime.Today,
            Salary      = 0m
        };

        Func<Task> act = () => svc.UpdateAsync(ghost);
        await act.Should().NotThrowAsync();
    }

    // ── DeleteAsync (soft delete) ─────────────────────────────────────────────

    [Test]
    public async Task DeleteAsync_SoftDeletesEmployee()
    {
        using var ctx = DbContextFactory.Create();
        var loc       = DbContextFactory.SeedLocation(ctx);
        var emp       = DbContextFactory.SeedEmployee(ctx, loc.Id);

        var svc = new EmployeeService(ctx);
        await svc.DeleteAsync(emp.Id);

        var deleted = ctx.Employees.IgnoreQueryFilters().First(e => e.Id == emp.Id);
        deleted.IsDeleted.Should().BeTrue();
        deleted.DeletedAt.Should().NotBeNull();
    }

    [Test]
    public async Task DeleteAsync_DoesNotThrow_WhenNotFound()
    {
        using var ctx = DbContextFactory.Create();
        var svc       = new EmployeeService(ctx);

        Func<Task> act = () => svc.DeleteAsync(9999);
        await act.Should().NotThrowAsync();
    }
}
