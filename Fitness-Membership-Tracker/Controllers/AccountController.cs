using Fitness_Membership_Tracker.Constants;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Models;
using Fitness_Membership_Tracker.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fitness_Membership_Tracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Member> _signInManager;
        private readonly UserManager<Member>   _userManager;

        public AccountController(
            SignInManager<Member> signInManager,
            UserManager<Member>   userManager)
        {
            _signInManager = signInManager;
            _userManager   = userManager;
        }

        // ─── Login ───────────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            // If already logged in, go home
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null || user.IsDeleted)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (await _userManager.IsInRoleAsync(user, Roles.Admin))
                    return RedirectToAction("Dashboard", "Admin");

                if (await _userManager.IsInRoleAsync(user, Roles.Trainer))
                    return RedirectToAction("Dashboard", "Trainer");

                if (await _userManager.IsInRoleAsync(user, Roles.Employee))
                    return RedirectToAction("Dashboard", "Employee");

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

            if (result.IsNotAllowed)
            {
                // This happens when RequireConfirmedEmail = true but email not confirmed.
                // Should no longer occur after the Program.cs fix, but kept for safety.
                ModelState.AddModelError("", "Account not confirmed. Please contact an administrator.");
                return View(model);
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Account is locked out. Please try again later.");
                return View(model);
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View(model);
        }

        // ─── Logout ──────────────────────────────────────────────────────────

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // ─── Register ────────────────────────────────────────────────────────

        [HttpGet]
        public IActionResult Register()
        {
            // If already logged in, go home instead of showing register page
            if (_signInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existing = await _userManager.FindByEmailAsync(model.Email);
            if (existing != null)
            {
                ModelState.AddModelError(nameof(model.Email), "This email is already registered.");
                return View(model);
            }

            var user = new Member
            {
                UserName  = model.Email,
                Email     = model.Email,
                IsDeleted = false,
                // Confirm immediately — no email flow needed
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
    }
}
