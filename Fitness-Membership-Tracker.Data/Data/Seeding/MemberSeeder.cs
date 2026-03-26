using Fitness_Membership_Tracker.Data.DataModels;
using Microsoft.AspNetCore.Identity;

namespace Fitness_Membership_Tracker.Data.Seeding
{
    public static class MemberSeeder
    {
        // These are kept internal so other seeders can look up the emails
        public static readonly IReadOnlyList<string> MemberEmails = new[]
        {
            "aleksandar.kolev@gmail.com",
            "boryana.todorova@gmail.com",
            "viktor.mihaylov@abv.bg",
            "galina.stefanova@gmail.com",
            "daniel.petrov@abv.bg",
            "emiliya.hristova@gmail.com",
            "filip.georgiev@gmail.com",
            "hristina.nikolova@abv.bg",
            "ivan.stoyanov@gmail.com",
            "joanna.angelova@gmail.com",
            "kaloyan.dimitrov@abv.bg",
            "lora.borisova@gmail.com",
            "martin.ivanov@gmail.com",
            "nelly.popova@abv.bg",
            "ognyan.slavchev@gmail.com",
            "petya.vasileva@gmail.com",
            "radoslav.neykov@abv.bg",
            "silviya.mancheva@gmail.com",
            "teodor.rashev@gmail.com",
            "ursula.georgieva@gmail.com",
            "vasil.trifonov@abv.bg",
            "yana.bankova@gmail.com",
            "zdravko.atanasov@gmail.com",
            "zlatina.lebedova@abv.bg",
            "andrey.zhelev@gmail.com",
            "blagovesta.kostadinova@gmail.com",
            "cvetan.kolev@abv.bg",
            "diana.bozhkova@gmail.com",
            "evelin.yordanov@gmail.com",
            "fани.mineva@gmail.com"
        };

        public static async Task SeedAsync(
            UserManager<Member> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            const string defaultPassword = "Member123!";

            // Ensure Member role exists
            if (!await roleManager.RoleExistsAsync("Member"))
                await roleManager.CreateAsync(new IdentityRole("Member"));

            foreach (var email in MemberEmails)
            {
                if (await userManager.FindByEmailAsync(email) != null)
                    continue;

                var member = new Member
                {
                    UserName       = email,
                    Email          = email,
                    EmailConfirmed = true,
                    IsDeleted      = false
                };

                var result = await userManager.CreateAsync(member, defaultPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(member, "Member");
            }
        }
    }
}
