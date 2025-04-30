namespace SACS.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Mvc;
    using SACS.Data.Models;
    using SACS.Services.Data;
    using SACS.Web.ViewModels.Employee;

    public class EmployeeController : Controller
    {
        private readonly IDepartmentService departmentService;
        private readonly IEmployeeService employeeService;
        private readonly ISummaryService summaryService;
        private readonly IUserManagementService userManagementService;

        public EmployeeController(
            IDepartmentService departmentService,
            IEmployeeService employeeService,
            ISummaryService summaryService,
            IUserManagementService userManagementService)
        {
            this.departmentService = departmentService;
            this.employeeService = employeeService;
            this.summaryService = summaryService;
            this.userManagementService = userManagementService;
        }

        // GET: /Employee/Create
        public IActionResult Create()
        {
            var model = new CreateEmployeeAndSummaryViewModel
            {
                Departments = this.departmentService.GetAll(),
            };

            return this.View(model);
        }

        // POST: /Employee/Create
        [HttpPost]
        public IActionResult Create(CreateEmployeeAndSummaryViewModel input)
        {
            var currentUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = this.userManagementService.GetUserById(currentUserId);

            if (user == null)
            {
                TempData["ErrorMessage"] = "Invalid user.";
                return this.RedirectToAction("Create");
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
                    Department = this.departmentService.GetDepartmentById(input.DepartmentId),
                    UserId = user.Id,
                    User = user,
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
                    Employee = newEmployee,
                };

                this.employeeService.Add(newEmployee);
                this.summaryService.CreateSummary(newSummary);

                // Change from TempData to match Dashboard's approach
                TempData["ToastMessage"] = "Employee created successfully!";
                TempData["ToastType"] = "success";

                return this.RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                TempData["ToastMessage"] = $"Error creating employee: {ex.Message}";
                TempData["ToastType"] = "error";
                return this.RedirectToAction("Create");
            }
        }

        // GET: /Employee/Details/{id}
        public IActionResult Details(string id)
        {
            var currentEmployee = this.employeeService.FindEmployeeById(id);

            var viewModel = new EmployeeInformationViewModel
            {
                Id = currentEmployee.Id,
                FirstName = currentEmployee.FirstName,
                LastName = currentEmployee.LastName,
                PhoneNumber = currentEmployee.PhoneNumber,
                Position = currentEmployee.Position,
                Department = this.departmentService.GetDepartmentById(currentEmployee.DepartmentId),
                Email = currentEmployee.Email,
            };

            return this.View(viewModel);
        }
    }
}
