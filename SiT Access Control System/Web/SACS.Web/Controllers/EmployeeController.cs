using SACS.Data;
using SACS.Data.Models;
using SACS.Web.ViewModels;
using SACS.Services.Data.Interfaces;


namespace SACS.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Create(CreateEmployeeAndSummaryViewModel input)
        {
            var newEmployee = this.mapper.Map<Employee>(input);

            var newSummary = this.mapper.Map<Summary>(input);

            await this.employeeService.AddAsync(newEmployee);
            await this.summaryService.CreateSummaryAsync(newSummary);
            return this.RedirectToAction(nameof(this.Create));
        }

        public IActionResult EmployeeInformation(string id)
        {
            var currentEmployee = this.employeeService.FindEmployeeById(id);

            if (currentEmployee == null)
            {
                return this.NotFound();
            }

            var model = this.mapper.Map<Employee>(id);


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
