using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SACS.Data.Models;
using SACS.Services.Data;
using SACS.Services.Data.Interfaces;
using SACS.Web.ViewModels;
using SACS.Web.ViewModels.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

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
        var employees = employeeService.GetAllEmployees(); // however you fetch them

        var viewModel = scheduleService.GetWeeklySchedule();


        viewModel.Employees = employees.Select(e => new SelectListItem
        {
            Value = e.Id,  
            Text = $"{e.FirstName} {e.LastName}"
        }).ToList();

        return View(viewModel);
    }

    [HttpGet]
    [Authorize]
    public IActionResult Info(string sortOrder)
    {
        var employees = employeeService.GetAllEmployees();
        var summaries = summaryService.GetAllSummaries();

        switch (sortOrder)
        {
            case "recent":
                employees = employees.OrderByDescending(e => e.Id).ToList();
                break;
            case "name_desc":
                employees = employees.OrderByDescending(e => e.FirstName).ToList();
                break;
            default:
                employees = employees.OrderBy(e => e.FirstName).ToList();
                break;
        }

        ViewData["CurrentSort"] = sortOrder ?? "name_asc";

        var model = new EmployeeListViewModel
        {
            Employees = employees,
            Summaries = summaries
        };

        // build SelectList for dropdown
        ViewData["SortList"] = new SelectList(new[]
        {
        new { Value = "name_asc", Text = "Sort by Name (A–Z)" },
        new { Value = "name_desc", Text = "Sort by Name (Z–A)" },
        new { Value = "recent", Text = "Most Recent" }
    }, "Value", "Text", sortOrder);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteSelected([FromBody] List<string> ids)
    {
        if (ids == null || ids.Count == 0)
        {
            return BadRequest("No employees selected.");
        }

        try
        {
            foreach (var id in ids)
            {
                // delete summaries first (avoid FK issues)
                await summaryService.DeleteSummaryByEmployeeIdAsync(id);

                // delete employee
                await employeeService.RemoveByIdAsync(id);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            // log exception if you have logging; return server error for now
            return StatusCode(500, $"Error deleting employees: {ex.Message}");
        }
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