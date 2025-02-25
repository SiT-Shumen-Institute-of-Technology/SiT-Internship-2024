namespace SACS.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using SACS.Data.Models;
    using SACS.Services.Data;
    using SACS.Web.ViewModels;

    public class AddEmployeeController : Controller
    {
        private readonly IEmployeeService employeeService;
        private readonly ISummaryService summaryService;

        public AddEmployeeController(IEmployeeService employeeService, ISummaryService summaryService)
        {
            this.employeeService = employeeService;
            this.summaryService = summaryService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeAndSummaryViewModel input)
        {
            Employee newEmployee = new Employee
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Position = input.Position,
                PhoneNumber = input.PhoneNumber,
                Email = input.Email,
            };
            Summary newSummary = new Summary
            {
                CurrentState = input.CurrentState,
                TimesLate = input.TimesLate,
                TotalHoursWorked = input.TotalHoursWorked,
                Timesabscent = input.Timesabscent,
                VacationDays = input.VacationDays,
                Employee = newEmployee,
                EmployeeId = newEmployee.Id,
            };
            this.employeeService.Add(newEmployee);
            this.summaryService.CreateSummary(newSummary);
            return this.Redirect("/");
        }
    }
}
