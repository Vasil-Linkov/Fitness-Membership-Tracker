using Fitness_Membership_Tracker.Constants;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Models;
using Fitness_Membership_Tracker.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fitness_Membership_Tracker.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly SignInManager<Member> _signInManager;
        private readonly UserManager<Member> _userManager;

        public AccountController(
            SignInManager<Member> signInManager,
            UserManager<Member> userManager)
        {
            _signInManager = signInManager;
            _userManager   = userManager;
        }

        // ─── Login ───────────────────────────────────────────────────────────

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl)
        {
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
                ModelState.AddModelError("", "Invalid login attempt.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Redirect based on role 
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

            ModelState.AddModelError("", "Invalid login attempt.");
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

        // ─── Register (public self-registration — Member role only) ──────────

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError(nameof(model.Email), "Email is already in use.");
                return View(model);
            }

            var user = new Member
            {
                UserName  = model.Email,
                Email     = model.Email,
                IsDeleted = false
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Auto-confirm email (proof-of-concept)
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _userManager.ConfirmEmailAsync(user, token);

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
    }
}
