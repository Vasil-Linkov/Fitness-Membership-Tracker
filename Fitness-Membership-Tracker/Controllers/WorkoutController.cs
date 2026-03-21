using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Models;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness_Membership_Tracker.Controllers
{
    [Authorize]
    public class WorkoutController : Controller
    {
        private readonly IWorkoutService _workoutService;
        private readonly IMemberService _memberService;

        public WorkoutController(IWorkoutService workoutService, IMemberService memberService)
        {
            _workoutService = workoutService;
            _memberService = memberService;
        }

        // GET: /Workout/Log
        [HttpGet]
        public IActionResult Log()
        {
            return View(new LogWorkoutViewModel());
        }

        // POST: /Workout/Log
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Log(LogWorkoutViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var member = await _memberService.GetByNameAsync(User.Identity!.Name!);
            if (member == null)
                return RedirectToAction("Index", "Home");

            var log = new WorkoutLog
            {
                MemberId = member.Id,
                Notes = model.Notes,
                Exercises = model.Exercises.Select(e => new WorkoutExercise
                {
                    ExerciseName = e.ExerciseName,
                    Sets = e.Sets,
                    Reps = e.Reps,
                    WeightKg = e.WeightKg,
                    DurationMinutes = e.DurationMinutes,
                    Notes = e.Notes
                }).ToList()
            };

            await _workoutService.CreateAsync(log);

            TempData["Success"] = "Workout logged successfully!";
            return RedirectToAction(nameof(History));
        }

        // GET: /Workout/History
        [HttpGet]
        public async Task<IActionResult> History()
        {
            var member = await _memberService.GetByNameAsync(User.Identity!.Name!);
            if (member == null)
                return RedirectToAction("Index", "Home");

            var logs = await _workoutService.GetByMemberIdAsync(member.Id);

            return View(new WorkoutHistoryViewModel { Logs = logs });
        }

        // POST: /Workout/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var log = await _workoutService.GetByIdAsync(id);
            var member = await _memberService.GetByNameAsync(User.Identity!.Name!);

            if (log == null || log.MemberId != member?.Id)
                return Forbid();

            await _workoutService.DeleteAsync(id);
            TempData["Success"] = "Workout log deleted.";
            return RedirectToAction(nameof(History));
        }
    }
}
