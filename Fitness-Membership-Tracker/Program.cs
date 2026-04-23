using Fitness_Membership_Tracker.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Fitness_Membership_Tracker.Services.Implementations;
using Fitness_Membership_Tracker.Services.Interfaces;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Data.Seeding;

namespace Fitness_Membership_Tracker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Database
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Identity
            builder.Services.AddIdentity<Member, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = true;

                options.Lockout.AllowedForNewUsers = false;
                options.Lockout.MaxFailedAccessAttempts = int.MaxValue;

                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // MVC
            builder.Services.AddControllersWithViews();

            // Application services
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IMembershipService, MembershipService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<IMembershipTierService, MembershipTierService>();
            builder.Services.AddScoped<IVisitService, VisitService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<ITrainerScheduleService, TrainerScheduleService>();
            builder.Services.AddScoped<ITrainerTraineeService, TrainerTraineeService>();
            builder.Services.AddScoped<ITrainingRequestService, TrainingRequestService>();
            builder.Services.AddScoped<IWorkoutService, WorkoutService>();

            // Build app
            var app = builder.Build();

            // HTTP pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Seed database on startup
            using (var scope = app.Services.CreateScope())
            {
                await DataSeeder.SeedAllAsync(scope.ServiceProvider);
            }

            app.Run();
        }
    }
}
