using Fitness_Membership_Tracker.Constants;
using Fitness_Membership_Tracker.Data.DataModels;
using Fitness_Membership_Tracker.HelperClasses;
using Fitness_Membership_Tracker.Models;
using Fitness_Membership_Tracker.Models.AdminViewModels;
using Fitness_Membership_Tracker.Services.Implementations;
using Fitness_Membership_Tracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Fitness_Membership_Tracker.Controllers
{
	[Authorize(Roles = Roles.Admin)]
	public class AdminController : Controller
	{
		private readonly IEmployeeService _employeeService;
		private readonly IMemberService _memberService;
		private readonly IMembershipService _membershipService;
		private readonly IPaymentService _paymentService;
		private readonly ILocationService _locationService;
		private readonly IMembershipTierService _membershipTierService;
		private readonly IVisitService _visitService;
		private readonly ITrainerService _trainerService;
        private readonly ITrainerScheduleService _trainerScheduleService;
        private readonly ITrainerTraineeService _trainerTraineeService;
        private readonly ITrainingRequestService _trainingRequestService;


		public AdminController(
            IEmployeeService employeeService,
            IMemberService memberService,
            IMembershipService membershipService,
            IPaymentService paymentService,
            ILocationService locationService,
            IMembershipTierService membershipTierService,
            IVisitService visitService,
            ITrainerService trainerService,
            ITrainerScheduleService trainerScheduleService,
            ITrainerTraineeService trainerTraineeService,
            ITrainingRequestService trainingRequestService)
        {
            _employeeService = employeeService;
            _memberService = memberService;
            _membershipService = membershipService;
            _paymentService = paymentService;
            _locationService = locationService;
            _membershipTierService = membershipTierService;
            _visitService = visitService;
            _trainerService = trainerService;
            _trainerScheduleService = trainerScheduleService;
            _trainerTraineeService = trainerTraineeService;
            _trainingRequestService = trainingRequestService;
        }

        [HttpGet]
		public async Task<IActionResult> Dashboard()
		{
			ViewBag.EmployeeCount = (await _employeeService.GetEmployeesAsync(null, string.Empty)).Count();
			ViewBag.MemberCount = (await _memberService.GetAllAsync()).Count();
			ViewBag.MembershipCount = (await _membershipService.GetAllAsync()).Count();
			ViewBag.PaymentCount = (await _paymentService.GetAllAsync()).Count();
            ViewBag.TrainerCount = (await _trainerService.GetTrainersAsync(null, string.Empty)).Count();

            var to = DateTime.Today;
            var from = to.AddDays(-29);
            ViewBag.VisitStats = await BuildVisitStatsViewModel(from, to, includeList: false);

            return View();
		}

		#region Employees

		[HttpGet]
		public async Task<IActionResult> Employees(int? locationId, string? search)
		{
			if(search == null)
				search = string.Empty;

			ViewBag.SelectedLocationId = locationId;
			ViewBag.Search = search;
			ViewBag.Locations = await GetLocations();

			var employees = await _employeeService.GetEmployeesAsync(locationId, search);

			return View(employees);
		}

		[HttpGet]
		public async Task<IActionResult> CreateEmployee()
		{
			var model = new CreateEmployeeAdminViewModel
			{
				Locations = await GetLocations()
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateEmployee(CreateEmployeeAdminViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Locations = await GetLocations();
				return View(model);
			}
			model.Email = model.FirstName + "." + model.LastName + "@gym.com";
			await _employeeService.CreateAsync(EmployeeMapper.ToEntity(model));

			return RedirectToAction(nameof(Employees));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteEmployee(int id)
		{
			await _employeeService.DeleteAsync(id);

			return RedirectToAction(nameof(Employees));
		}

        #endregion


        #region Trainers

        [HttpGet]
        public async Task<IActionResult> Trainers(int? locationId, string? search)
        {
            search ??= string.Empty;
            ViewBag.SelectedLocationId = locationId;
            ViewBag.Search = search;
            ViewBag.Locations = await GetLocations();

            var trainers = await _trainerService.GetTrainersAsync(locationId, search);
            return View(trainers);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTrainer()
        {
            var model = new CreateTrainerAdminViewModel
            {
                HireDate = DateTime.Today,
                Locations = await GetLocations()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTrainer(CreateTrainerAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Locations = await GetLocations();
                return View(model);
            }

            var trainer = TrainerMapper.ToEntity(model);
            trainer.Email = $"{model.FirstName}.{model.LastName}@gym.com";
            await _trainerService.CreateAsync(trainer);
            return RedirectToAction(nameof(Trainers));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTrainer(int id)
        {
            await _trainerService.DeleteAsync(id);
            return RedirectToAction(nameof(Trainers));
        }

        #endregion


        #region Members

        [HttpGet]
		public async Task<IActionResult> Members()
		{
			var members = await _memberService.GetAllAsync();
			return View(members);
		}

		#endregion


		#region Memberships

		[HttpGet]
		public async Task<IActionResult> Memberships()
		{
			var memberships = await _membershipService.GetAllAsync();
			return View(memberships);
		}

		[HttpGet]
		public async Task<IActionResult> CreateMembership()
		{
			var model = new CreateMembershipAdminViewModel
			{
				StartDate = DateTime.Now,
				EndDate = DateTime.Now.AddMonths(1),
				Locations = await GetLocations(),
				Tiers = await GetTiers(),
				Members = await GetMembersWithoutMembership()
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateMembership(CreateMembershipAdminViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Locations = await GetLocations();
				model.Tiers = await GetTiers();
				return View(model);
			}

			await _membershipService.CreateAsync(MembershipMapper.ToEntity(model));

			return RedirectToAction(nameof(Memberships));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteMembership(int id)
		{
			await _membershipService.DeleteAsync(id);

			return RedirectToAction(nameof(Memberships));
		}

		#endregion


		#region Payments

		[HttpGet]
		public async Task<IActionResult> Payments()
		{
			var payments = await _paymentService.GetAllAsync();
			return View(payments);
		}

		[HttpGet]
		public async Task<IActionResult> CreatePayment()
		{
			var model = new CreatePaymentAdminViewModel
			{
				PaymentDate = DateTime.Now,
				Employees = await GetEmployees(),
				Members = await GetMembers(),
				Memberships = await GetMemberships()
			};

			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreatePayment(CreatePaymentAdminViewModel model)
		{
			if (!ModelState.IsValid)
			{
				model.Employees = await GetEmployees();
				model.Members = await GetMembers();
				model.Memberships = await GetMemberships();
				return View(model);
			}

			await _paymentService.CreateAsync(PaymentMapper.ToEntity(model));

			return RedirectToAction(nameof(Payments));
		}

        #endregion


        #region Visits

        [HttpGet]
        public async Task<IActionResult> Visits(DateTime? from, DateTime? to)
        {
            var dateTo = to ?? DateTime.Today;
            var dateFrom = from ?? dateTo.AddDays(-29);

            var model = await BuildVisitStatsViewModel(dateFrom, dateTo, includeList: true);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LogVisit()
        {
            var model = new LogVisitAdminViewModel
            {
                Members = await GetMembers(),
                Locations = await GetLocations(),
                Memberships = await GetMemberships()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogVisit(LogVisitAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Members = await GetMembers();
                model.Locations = await GetLocations();
                model.Memberships = await GetMemberships();
                return View(model);
            }

            var visit = new Visit
            {
                MemberId = model.MemberId,
                LocationId = model.LocationId,
                MembershipId = model.MembershipId
            };

            await _visitService.CreateAsync(visit);
            return RedirectToAction(nameof(Visits));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVisit(int id)
        {
            await _visitService.DeleteAsync(id);
            return RedirectToAction(nameof(Visits));
        }

        #endregion


        #region Trainer Schedule & Capacity 

        [HttpGet]
        public async Task<IActionResult> TrainerSchedule(int trainerId)
        {
            var trainer = await _trainerService.GetByIdAsync(trainerId);
            if (trainer == null) return NotFound();

            var slots = await _trainerScheduleService.GetByTrainerIdAsync(trainerId);

            var vm = new ManageTrainerScheduleViewModel
            {
                TrainerId = trainerId,
                TrainerName = $"{trainer.FirstName} {trainer.LastName}",
                ExistingSlots = slots
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTrainerSlot(ManageTrainerScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var trainer = await _trainerService.GetByIdAsync(model.TrainerId);
                model.TrainerName = trainer != null ? $"{trainer.FirstName} {trainer.LastName}" : string.Empty;
                model.ExistingSlots = await _trainerScheduleService.GetByTrainerIdAsync(model.TrainerId);
                return View("TrainerSchedule", model);
            }

            var slot = new TrainerSchedule
            {
                TrainerId = model.TrainerId,
                DayOfWeek = model.DayOfWeek,
                StartTime = model.StartTime,
                EndTime = model.EndTime
            };

            await _trainerScheduleService.AddSlotAsync(slot);
            TempData["Success"] = "Schedule slot added.";
            return RedirectToAction(nameof(TrainerSchedule), new { trainerId = model.TrainerId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveTrainerSlot(int slotId, int trainerId)
        {
            await _trainerScheduleService.RemoveSlotAsync(slotId);
            TempData["Success"] = "Schedule slot removed.";
            return RedirectToAction(nameof(TrainerSchedule), new { trainerId });
        }

        [HttpGet]
        public async Task<IActionResult> TrainerCapacity(int trainerId)
        {
            var trainer = await _trainerService.GetByIdAsync(trainerId);
            if (trainer == null) return NotFound();

            int current = await _trainerTraineeService.GetActiveTraineeCountAsync(trainerId);
            int max = await _trainerTraineeService.GetMaxTraineesAsync(trainerId);

            var vm = new UpdateTrainerCapacityViewModel
            {
                TrainerId = trainerId,
                TrainerName = $"{trainer.FirstName} {trainer.LastName}",
                MaxTrainees = max,
                CurrentCount = current
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TrainerCapacity(UpdateTrainerCapacityViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _trainerTraineeService.UpdateMaxTraineesAsync(model.TrainerId, model.MaxTrainees);
            TempData["Success"] = $"Capacity updated to {model.MaxTrainees} trainees.";
            return RedirectToAction(nameof(Trainers));
        }

        #endregion


        /* Since dropdown menues are used in many selections,
		these helper methods are used to create SelectList
		for every dropdown in the admin views
		to make the code more readable.
		(since i constantly got confused even while writing it)*/
        #region Dropdown Helpers

        private async Task<IEnumerable<SelectListItem>> GetLocations()
		{
			return (await _locationService.GetAllAsync())
				.Select(location => new SelectListItem
				{
					Value = location.Id.ToString(),
					Text = $"{location.City} - {location.Address}"
				});
		}

		private async Task<IEnumerable<SelectListItem>> GetTiers()
		{
			return (await _membershipTierService.GetTiersAsync())
				.Select(tier => new SelectListItem
				{
					Value = tier.Id.ToString(),
					Text = tier.Tier
				});
		}

		private async Task<IEnumerable<SelectListItem>> GetMembers()
		{
			return (await _memberService.GetAllAsync())
				.Select(member => new SelectListItem
				{
					Value = member.Id,
					Text = member.Email
				});
		}

		private async Task<IEnumerable<SelectListItem>> GetEmployees()
		{
			return (await _employeeService.GetEmployeesAsync(null, string.Empty))
				.Select(employee => new SelectListItem
				{
					Value = employee.Id.ToString(),
					Text = employee.Email
				});
		}

		private async Task<IEnumerable<SelectListItem>> GetMemberships()
		{
			return (await _membershipService.GetAllAsync())
				.Select(membership => new SelectListItem
				{
					Value = membership.Id.ToString(),
					Text = $"Membership #{membership.Id}"
				});
		}

        private async Task<IEnumerable<SelectListItem>> GetMembersWithoutMembership()
        {
            return (await _memberService.GetMembersWithoutMembership())
                .Select(membership => new SelectListItem
                {
                    Value = membership.Id.ToString(),
                    Text = $"Membership #{membership.Id}"
                });
        }

        #endregion

        #region Visit view model creator

        private async Task<VisitStatsViewModel> BuildVisitStatsViewModel(DateTime from, DateTime to, bool includeList)
        {
            var dailyCounts = await _visitService.GetDailyVisitCountsAsync(from, to.AddDays(1).AddTicks(-1));

            int total = dailyCounts.Values.Sum();
            int days = Math.Max(1, (int)(to - from).TotalDays + 1);
            double avg = (double)total / days;
            var peak = dailyCounts.OrderByDescending(kv => kv.Value).FirstOrDefault();

            var model = new VisitStatsViewModel
            {
                From = from,
                To = to,
                DailyVisitCounts = dailyCounts,
                TotalVisits = total,
                AveragePerDay = avg,
                PeakCount = peak.Value,
                PeakDay = dailyCounts.Any() ? peak.Key : null
            };

            if (includeList)
                model.Visits = await _visitService.GetByDateRangeAsync(from, to.AddDays(1).AddTicks(-1));

            return model;
        }

        #endregion
    }
}