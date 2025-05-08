using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SACS.Data.Models;
using SACS.Services.Data;
using SACS.Services.Data.Interfaces;
using SACS.Web.ViewModels;
using SACS.Web.ViewModels.Employee;

namespace SACS.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IDepartmentService departmentService;
        private readonly ISummaryService summaryService;
        private readonly IScheduleService scheduleService;
        private readonly IUserManagementService userManagementService;
        private readonly IMapper mapper;

        public EmployeeController(
            IUserManagementService userManagementService,
            IDepartmentService departmentService,
            ISummaryService summaryService,
            IScheduleService scheduleService,
            IMapper mapper)
        {
            this.departmentService = departmentService;
            this.summaryService = summaryService;
            this.userManagementService = userManagementService;
            this.scheduleService = scheduleService;
            this.mapper = mapper;
        }

        // GET: /Employee/Create
        public IActionResult Create()
        {
            var model = new CreateEmployeeAndSummaryViewModel
            {
                Departments = departmentService.GetAll()
            };
            return View(model);
        }

        // POST: /Employee/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeAndSummaryViewModel input)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var adminUser = userManagementService.GetUserById(currentUserId);

            if (adminUser == null)
            {
                TempData["ErrorMessage"] = "Invalid user.";
                return RedirectToAction("Create");
            }

            try
            {
                var newUser = new ApplicationUser
                {
                    UserName = input.Email,
                    Email = input.Email,
                    PhoneNumber = input.PhoneNumber,
                    DepartmentId = input.DepartmentId,
                    Position = input.Position,
                    CreatedOn = DateTime.UtcNow
                };

                var result = await userManagementService.CreateUserAsync(newUser, "Default123!", "Employee");

                if (!result.Succeeded)
                {
                    TempData["ToastMessage"] = "Failed to create employee user.";
                    TempData["ToastType"] = "error";
                    return RedirectToAction("Create");
                }

                var newSummary = new Summary
                {
                    Id = Guid.NewGuid().ToString(),
                    CurrentState = input.CurrentState,
                    TimesLate = input.TimesLate,
                    TotalHoursWorked = input.TotalHoursWorked,
                    Timesabscent = input.Timesabscent,
                    VacationDays = input.VacationDays,
                    UserId = newUser.Id,
                };

                await summaryService.CreateSummaryAsync(newSummary);

                TempData["ToastMessage"] = "Employee created successfully!";
                TempData["ToastType"] = "success";

                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = $"Error creating employee: {ex.Message}";
                TempData["ToastType"] = "error";
                return RedirectToAction("Create");
            }
        }

        // GET: /Employee/Details/{id}
        public IActionResult Details(string id)
        {
            var user = userManagementService.GetUserById(id);

            if (user == null)
                return NotFound();

            var viewModel = new EmployeeInformationViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Position = user.Position,
                Department = departmentService.GetDepartmentById(user.DepartmentId),
                Email = user.Email
            };

            return View(viewModel);
        }

        // GET: /Employee/EmployeeInformation/{id}
        public IActionResult EmployeeInformation(string id)
        {
            var user = userManagementService.GetUserById(id);

            if (user == null)
                return NotFound();

            var model = mapper.Map<EmployeeInformationViewModel>(user);

            return View(model);
        }

        // GET: /Employee/Schedule
        [HttpGet]
        public IActionResult Schedule()
        {
            var model = scheduleService.GetWeeklySchedule();
            return View(model);
        }

        // POST: /Employee/Schedule
        [HttpPost]
        public async Task<IActionResult> Schedule(ScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Employees = scheduleService.GetWeeklySchedule().Employees;
                return View("Schedule", model);
            }

            var schedule = mapper.Map<EmployeeSchedule>(model);

            await scheduleService.AddScheduleAsync(schedule);
            return RedirectToAction(nameof(Schedule));
        }
    }
}
