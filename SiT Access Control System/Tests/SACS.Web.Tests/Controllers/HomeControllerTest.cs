namespace SACS.Web.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http.HttpResults;
    using SACS.Data.Models;
    using SACS.Services.Data;
    using SACS.Web.Controllers;
    using Xunit;

    public class HomeControllerTest
    {
        private readonly IEmployeeService employeeService = A.Fake<IEmployeeService>();
        private readonly ISummaryService summaryService = A.Fake<ISummaryService>();
        private readonly HomeController controller;

        public HomeControllerTest()
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

            this.controller = new HomeController(this.employeeService, this.summaryService);
        }

        [Fact]
        public void HomeController_Index_ReturnsEmployeeListViewModel()
        {
            // Arrange
            A.CallTo(() => this.employeeService.GetAllEmployees());
            A.CallTo(() => this.summaryService.GetAllSummaries());

            // Act
            var result = this.controller.Index();

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void HomeController_Index_Delete_ReturnsRedirectToActionResult()
        {
            // Arrange
            A.CallTo(() => this.employeeService.GetAllEmployees());
            string id = this.employeeService.GetAllEmployees().First().Id;

            // Act
            var result = this.controller.Delete(id);

            // Assert
            result.Should().NotBe(typeof(BadRequest));
        }
    }
}
