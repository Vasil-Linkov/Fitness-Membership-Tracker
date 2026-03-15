using System.Reflection.Emit;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Fitness_Membership_Tracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<Member>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<MembershipTier> MembershipTiers { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<LocationMembership> LocationMemberships { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Trainer> Trainers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<LocationMembership>()
                .HasKey(lm => new {lm.LocationId, lm.MembershipId});


            builder.Entity<Location>().HasData(DBSeeding.SeedLocations());
            builder.Entity<MembershipTier>().HasData(DBSeeding.SeedMembershipTiers());
            builder.Entity<Employee>().HasData(DBSeeding.SeedEmployees());
            builder.Entity<Trainer>().HasData(DBSeeding.SeedTrainers());


            builder.Entity<Employee>().HasQueryFilter(e => !e.IsDeleted);
            builder.Entity<Member>().HasQueryFilter(m => !m.IsDeleted);
            builder.Entity<Membership>().HasQueryFilter(m => !m.IsDeleted);
            builder.Entity<Payment>().HasQueryFilter(p => !p.IsDeleted);
			builder.Entity<Location>().HasQueryFilter(p => !p.IsDeleted);
            builder.Entity<Visit>().HasQueryFilter(v => !v.IsDeleted);
            builder.Entity<Trainer>().HasQueryFilter(t => !t.IsDeleted);

        }
    }
}
