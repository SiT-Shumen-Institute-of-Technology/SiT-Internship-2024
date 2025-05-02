namespace SACS.Web.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;
    using FluentAssertions;
    using SACS.Data.Models;
    using SACS.Services.Data;
    using SACS.Web.Controllers;
    using Xunit;

    public class EmployeeInformationControllerTest
    {
        private readonly IEmployeeService employeeService = A.Fake<IEmployeeService>();
        private readonly ISummaryService summaryService = A.Fake<ISummaryService>();
        private readonly IDepartmentService departmentService = A.Fake<IDepartmentService>();
        private readonly EmployeeInformationController controller;

        public EmployeeInformationControllerTest()
        {
            List<Employee> employees = new List<Employee>
                {
                    new Employee
                    {
                        Id = new Guid().ToString(),
                        FirstName = "TestEmployeeFirstName",
                        LastName = "TestEmployeeLastName",
                        Position = "TestEmployeePosition",
                        PhoneNumber = "TestEmployeePhoneNumber",
                        Email = "TestEmployeeEmail",
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                    },
                };
            A.CallTo(() => this.employeeService.GetAllEmployees()).Returns(employees);

            List<DailySummary> dailySummaries = new List<DailySummary>
            {
                new DailySummary
                {
                    Id = new Guid().ToString(),
                    CurrentStatus = Status.Active,
                    Employee = employees.First(),
                    EmployeeId = employees.First().Id,
                    IsDeleted = false,
                    CreatedOn = DateTime.Now,
                },
            };

            A.CallTo(() => this.summaryService.GetAllSummaries()).Returns(dailySummaries);

            List<Department> departments = new List<Department>
            {
                new Department
                {
                    Id = new Guid().ToString(),
                    Name = "TestDepartmentName",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
            };

            A.CallTo(() => this.departmentService.GetAll()).Returns(departments);

            this.controller = new EmployeeInformationController(this.employeeService, this.summaryService, this.departmentService);
        }

        [Fact]
        public void EmployeeInformationController_Index_ReturnsEmployeeInformationViewModel()
        {
            // Arrange
            A.CallTo(() => this.employeeService.GetAllEmployees());
            A.CallTo(() => this.summaryService.GetAllSummaries());
            A.CallTo(() => this.departmentService.GetAll());
            string id = this.employeeService.GetAllEmployees().First().Id;

            // Act
            var result = this.controller.Index(id);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
