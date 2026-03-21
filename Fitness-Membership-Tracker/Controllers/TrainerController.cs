using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.Models;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fitness_Membership_Tracker.Controllers
{
    [Authorize]
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;
        private readonly ITrainerScheduleService _scheduleService;
        private readonly ITrainingRequestService _requestService;
        private readonly ITrainerTraineeService _traineeService;
        private readonly IMemberService _memberService;

        public TrainerController(
            ITrainerService trainerService,
            ITrainerScheduleService scheduleService,
            ITrainingRequestService requestService,
            ITrainerTraineeService traineeService,
            IMemberService memberService)
        {
            _trainerService = trainerService;
            _scheduleService = scheduleService;
            _requestService = requestService;
            _traineeService = traineeService;
            _memberService = memberService;
        }

        // ─── Member side ─────────────────────────────────────────────────────

        [HttpGet]
        public async Task<IActionResult> Browse(DayOfWeek? day)
        {
            var member = await _memberService.GetByNameAsync(User.Identity!.Name!);

            var selectedDay = day ?? DateTime.Today.DayOfWeek;
            var availableTrainers = await _scheduleService.GetAvailableTrainersAsync(selectedDay);

            var cards = new List<TrainerCardViewModel>();
            foreach (var trainer in availableTrainers)
            {
                var slots = await _scheduleService.GetByTrainerIdAsync(trainer.Id);
                var todaySlots = slots.Where(s => s.DayOfWeek == (int)selectedDay).ToList();
                int activeCount = await _traineeService.GetActiveTraineeCountAsync(trainer.Id);
                int maxCount = await _traineeService.GetMaxTraineesAsync(trainer.Id);

                cards.Add(new TrainerCardViewModel
                {
                    Trainer = trainer,
                    ActiveTrainees = activeCount,
                    MaxTrainees = maxCount,
                    TodaySlots = todaySlots
                });
            }

            var activeRequest = member != null
                ? (await _requestService.GetByMemberIdAsync(member.Id))
                    .FirstOrDefault(r => r.Status == TrainingRequestStatus.Pending)
                : null;

            var currentRelationship = member != null
                ? await _traineeService.GetActiveRelationshipAsync(member.Id)
                : null;

            var vm = new BrowseTrainersViewModel
            {
                Trainers = cards,
                SelectedDay = selectedDay,
                ActiveRequest = activeRequest,
                CurrentRelationship = currentRelationship
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Request(int trainerId)
        {
            var trainer = await _trainerService.GetByIdAsync(trainerId);
            if (trainer == null) return NotFound();

            if (!await _traineeService.HasCapacityAsync(trainerId))
            {
                TempData["Error"] = "This trainer has reached their maximum trainee capacity.";
                return RedirectToAction(nameof(Browse));
            }

            var member = await _memberService.GetByNameAsync(User.Identity!.Name!);

            if (member != null)
            {
                var pending = (await _requestService.GetByMemberIdAsync(member.Id))
                    .Any(r => r.Status == TrainingRequestStatus.Pending);

                if (pending)
                {
                    TempData["Error"] = "You already have a pending request. Cancel it before sending a new one.";
                    return RedirectToAction(nameof(Browse));
                }

                var activeRel = await _traineeService.GetActiveRelationshipAsync(member.Id);
                if (activeRel != null)
                {
                    TempData["Error"] = "You already have an active trainer. End your current relationship first.";
                    return RedirectToAction(nameof(MyTrainer));
                }
            }

            return View(new RequestTrainerViewModel { Trainer = trainer });
        }

        // POST: /Trainer/Request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Request(int trainerId, string? memberMessage)
        {
            var member = await _memberService.GetByNameAsync(User.Identity!.Name!);
            if (member == null) return RedirectToAction("Index", "Home");

            var trainer = await _trainerService.GetByIdAsync(trainerId);
            if (trainer == null) return NotFound();

            if (!await _traineeService.HasCapacityAsync(trainerId))
            {
                TempData["Error"] = "This trainer is now at full capacity.";
                return RedirectToAction(nameof(Browse));
            }

            var request = new TrainingRequest
            {
                MemberId = member.Id,
                TrainerId = trainerId,
                MemberMessage = memberMessage
            };

            await _requestService.CreateAsync(request);

            TempData["Success"] = "Your request has been sent! The trainer will review it shortly.";
            return RedirectToAction(nameof(MyTrainer));
        }

        // GET: /Trainer/MyTrainer
        [HttpGet]
        public async Task<IActionResult> MyTrainer()
        {
            var member = await _memberService.GetByNameAsync(User.Identity!.Name!);
            if (member == null) return RedirectToAction("Index", "Home");

            var activeRel = await _traineeService.GetActiveRelationshipAsync(member.Id);
            var history = await _requestService.GetByMemberIdAsync(member.Id);

            return View(new MyTrainerViewModel
            {
                ActiveRelationship = activeRel,
                RequestHistory = history
            });
        }

        // POST: /Trainer/CancelRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelRequest(int requestId)
        {
            var member = await _memberService.GetByNameAsync(User.Identity!.Name!);
            var request = await _requestService.GetByIdAsync(requestId);

            if (request == null || request.MemberId != member?.Id)
                return Forbid();

            await _requestService.CancelAsync(requestId);
            TempData["Success"] = "Request cancelled.";
            return RedirectToAction(nameof(MyTrainer));
        }

        // POST: /Trainer/EndRelationship
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndRelationship(int trainerTraineeId)
        {
            var member = await _memberService.GetByNameAsync(User.Identity!.Name!);
            var rel = (await _traineeService.GetActiveRelationshipAsync(member?.Id ?? ""));

            if (rel == null || rel.Id != trainerTraineeId)
                return Forbid();

            await _traineeService.EndRelationshipAsync(trainerTraineeId);
            TempData["Success"] = "Training relationship ended.";
            return RedirectToAction(nameof(Browse));
        }


        private async Task<Trainer?> GetCurrentTrainer()
        {
            var trainers = await _trainerService.GetTrainersAsync(null, User.Identity!.Name!);
            return trainers.FirstOrDefault(t =>
                t.Email.Equals(User.Identity.Name, StringComparison.OrdinalIgnoreCase));
        }

        // GET: /Trainer/Dashboard
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var trainer = await GetCurrentTrainer();
            if (trainer == null)
            {
                TempData["Error"] = "No trainer profile is linked to your account.";
                return RedirectToAction("Index", "Home");
            }

            var pending = await _requestService.GetPendingForTrainerAsync(trainer.Id);
            var trainees = await _traineeService.GetByTrainerIdAsync(trainer.Id);
            var schedule = await _scheduleService.GetByTrainerIdAsync(trainer.Id);
            int maxTrainees = await _traineeService.GetMaxTraineesAsync(trainer.Id);

            return View(new TrainerDashboardViewModel
            {
                Trainer = trainer,
                PendingRequests = pending,
                ActiveTrainees = trainees,
                MaxTrainees = maxTrainees,
                CurrentCount = trainees.Count,
                Schedule = schedule
            });
        }

        // GET: /Trainer/RespondToRequest/{requestId}
        [HttpGet]
        public async Task<IActionResult> RespondToRequest(int requestId)
        {
            var trainer = await GetCurrentTrainer();
            if (trainer == null) return Forbid();

            var request = await _requestService.GetByIdAsync(requestId);
            if (request == null || request.TrainerId != trainer.Id) return NotFound();

            return View(new RespondToRequestViewModel { Request = request });
        }

        // POST: /Trainer/AcceptRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptRequest(int requestId, string? trainerResponse)
        {
            var trainer = await GetCurrentTrainer();
            if (trainer == null) return Forbid();

            var request = await _requestService.GetByIdAsync(requestId);
            if (request == null || request.TrainerId != trainer.Id) return NotFound();

            try
            {
                await _requestService.AcceptAsync(requestId, trainerResponse ?? string.Empty);
                TempData["Success"] = $"You are now training {request.Member?.Email}.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(Dashboard));
        }

        // POST: /Trainer/RejectRequest
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectRequest(int requestId, string? trainerResponse)
        {
            var trainer = await GetCurrentTrainer();
            if (trainer == null) return Forbid();

            var request = await _requestService.GetByIdAsync(requestId);
            if (request == null || request.TrainerId != trainer.Id) return NotFound();

            await _requestService.RejectAsync(requestId, trainerResponse ?? string.Empty);
            TempData["Success"] = "Request rejected.";
            return RedirectToAction(nameof(Dashboard));
        }

        // POST: /Trainer/EndTraineeRelationship
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndTraineeRelationship(int trainerTraineeId)
        {
            var trainer = await GetCurrentTrainer();
            if (trainer == null) return Forbid();

            await _traineeService.EndRelationshipAsync(trainerTraineeId);
            TempData["Success"] = "Training relationship ended.";
            return RedirectToAction(nameof(Dashboard));
        }
    }
}
