namespace SACS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using SACS.Data;
    using SACS.Data.Models;
    using SACS.Services.Data;
    using SACS.Web.Profiles;
    using SACS.Web.ViewModels;
    using SendGrid.Helpers.Mail;

    public class EmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly IDepartmentService departmentService;
        private readonly ApplicationDbContext db;
        private readonly ISummaryService summaryService;
        private readonly IScheduleService scheduleService;
        private readonly IMapper mapper;

        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService, ApplicationDbContext db, ISummaryService summaryService, IScheduleService scheduleService, IMapper mapper)
        {
            this.employeeService = employeeService;
            this.departmentService = departmentService;
            this.db = db;
            this.summaryService = summaryService;
            this.scheduleService = scheduleService;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeAndSummaryViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var newEmployee = new Employee
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Position = input.Position,
                PhoneNumber = input.PhoneNumber,
                Email = input.Email,
            };

            var newSummary = new Summary
            {
                CurrentState = input.CurrentState,
                TimesLate = input.TimesLate,
                TotalHoursWorked = input.TotalHoursWorked,
                Timesabscent = input.Timesabscent,
                VacationDays = input.VacationDays,
                Employee = newEmployee,
            };

            this.employeeService.Add(newEmployee);
            this.summaryService.CreateSummary(newSummary);

            return this.RedirectToAction("EmployeeSchedule");
        }

        public IActionResult EmployeeInformation(string id)
        {
            var currentEmployee = this.employeeService.FindEmployeeById(id);

            if (currentEmployee == null)
            {
                return this.NotFound();
            }

            var model = new EmployeeInformationViewModel
            {
                FirstName = currentEmployee.FirstName,
                LastName = currentEmployee.LastName,
                PhoneNumber = currentEmployee.PhoneNumber,
                Position = currentEmployee.Position,
                Department = this.departmentService.GetDepartmentById(currentEmployee.DepartmentId),
                Email = currentEmployee.Email,
            };

            return this.View(model);
        }




        [HttpGet]
        public IActionResult Schedule()
        {
            var model = this.scheduleService.GetWeeklySchedule();
            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Schedule(ScheduleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Employees = this.scheduleService.GetWeeklySchedule().Employees;
                return this.View("Schedule", model);
            }

            var schedule = this.mapper.Map<EmployeeSchedule>(model);

            await this.scheduleService.AddScheduleAsync(schedule);
            return this.RedirectToAction(nameof(this.Schedule));
        }
    }
}
