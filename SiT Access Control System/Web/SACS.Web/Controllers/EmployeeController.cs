using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SACS.Data.Models;
using SACS.Services.Data;
using SACS.Services.Data.Interfaces;
using SACS.Web.ViewModels;
using SACS.Web.ViewModels.Employee;

namespace SACS.Web.Controllers;

public class EmployeeController : Controller
{
    private readonly IDepartmentService departmentService;
    private readonly IEmployeeService employeeService;
    private readonly IMapper mapper;
    private readonly IScheduleService scheduleService;
    private readonly ISummaryService summaryService;
    private readonly IUserManagementService userManagementService;

    public EmployeeController(IUserManagementService userManagementService, IEmployeeService employeeService,
        IDepartmentService departmentService, ISummaryService summaryService, IScheduleService scheduleService,
        IMapper mapper)
    {
        this.departmentService = departmentService;
        this.employeeService = employeeService;
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
    public IActionResult Create(CreateEmployeeAndSummaryViewModel input)
    {
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = userManagementService.GetUserById(currentUserId);

        if (user == null)
        {
            TempData["ErrorMessage"] = "Invalid user.";
            return RedirectToAction("Create");
        }

        try
        {
            var newEmployee = new Employee
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = input.FirstName,
                LastName = input.LastName,
                Position = input.Position,
                PhoneNumber = input.PhoneNumber,
                Email = input.Email,
                DepartmentId = input.DepartmentId,
                Department = departmentService.GetDepartmentById(input.DepartmentId),
                UserId = user.Id,
                User = user
            };

            var newSummary = new Summary
            {
                Id = Guid.NewGuid().ToString(),
                CurrentState = input.CurrentState,
                TimesLate = input.TimesLate,
                TotalHoursWorked = input.TotalHoursWorked,
                Timesabscent = input.Timesabscent,
                VacationDays = input.VacationDays,
                EmployeeId = newEmployee.Id,
                Employee = newEmployee
            };

            employeeService.AddAsync(newEmployee);
            summaryService.CreateSummaryAsync(newSummary);

            // Change from TempData to match Dashboard's approach
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
        var currentEmployee = employeeService.FindEmployeeById(id);

        if (currentEmployee == null) return NotFound();

        var viewModel = new EmployeeInformationViewModel
        {
            Id = currentEmployee.Id,
            FirstName = currentEmployee.FirstName,
            LastName = currentEmployee.LastName,
            PhoneNumber = currentEmployee.PhoneNumber,
            Position = currentEmployee.Position,
            Department = departmentService.GetDepartmentById(currentEmployee.DepartmentId),
            Email = currentEmployee.Email
        };

        return View(viewModel);
    }

    // GET: /Employee/EmployeeInformation/{id}
    public IActionResult EmployeeInformation(string id)
    {
        var currentEmployee = employeeService.FindEmployeeById(id);

        if (currentEmployee == null) return NotFound();

        var model = mapper.Map<EmployeeInformationViewModel>(currentEmployee);

        return View(model);
    }

    // GET: /Employee/Schedule
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Schedule()
    {
        var employeeUsers = await userManagementService.GetUsersInRoleAsync("Employee");

        var viewModel = scheduleService.GetWeeklySchedule();

        viewModel.Employees = employeeUsers.Select(u => new SelectListItem
        {
            Value = u.Id,
            Text = $"{u.UserName}",
        }).ToList();

        return View(viewModel);
    }


    // POST: /Employee/Schedule
    [HttpPost]
    public async Task<IActionResult> Schedule(ScheduleViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var employeeUsers = await userManagementService.GetUsersInRoleAsync("Employee");

            model.Employees = employeeUsers.Select(u => new SelectListItem
            {
                Value = u.Id,
                Text = $"{u.UserName}"
            }).ToList();

            model.WeeklySchedule = scheduleService.GetWeeklySchedule().WeeklySchedule;

            return View("Schedule", model);
        }

        var schedule = mapper.Map<EmployeeSchedule>(model);

        await scheduleService.AddScheduleAsync(schedule);
        return RedirectToAction(nameof(this.Schedule));
    }


}