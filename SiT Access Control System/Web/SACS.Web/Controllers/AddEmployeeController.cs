namespace SACS.Web.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using SACS.Data.Models;
    using SACS.Services.Data;
    using SACS.Web.ViewModels;

    public class AddEmployeeController : Controller
    {
        private readonly IDepartmentService departmentService;
        private readonly IEmployeeService employeeService;
        private readonly ISummaryService summaryService;
        private readonly IUserManagementService userManagementService;


        public AddEmployeeController(
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

        public IActionResult Index()
        {
            return this.View(new CreateEmployeeAndSummaryViewModel
            {
                Departments = this.departmentService.GetAll(),
                Users = this.userManagementService.GetAllUsers(), // implement this if needed
            });
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeAndSummaryViewModel input)
        {
            var user = this.userManagementService.GetUserById(input.UserId); // or however you fetch a user

            Employee newEmployee = new Employee
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = input.FirstName,
                LastName = input.LastName,
                Position = input.Position,
                PhoneNumber = input.PhoneNumber,
                Email = input.Email,
                Department = this.departmentService.GetDepartmentById(input.DepartmentId),
                DepartmentId = input.DepartmentId,
                User = user,
                UserId = user.Id,
            };

            Summary newSummary = new Summary
            {
                Id = Guid.NewGuid().ToString(),
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
