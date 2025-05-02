namespace SACS.Web.Tests.Controller
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;
    using FluentAssertions;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using SACS.Data.Models;
    using SACS.Services.Data;
    using SACS.Web.Controllers;
    using SACS.Web.ViewModels;
    using Xunit;

    public class AddEmployeeControllerTest
    {
        private static IDepartmentService departmentService = A.Fake<IDepartmentService>();
        private static IEmployeeService employeeService = A.Fake<IEmployeeService>();
        private static ISummaryService summaryService = A.Fake<ISummaryService>();
        AddEmployeeController controller;

        public AddEmployeeControllerTest()
        {
            var departments = new List<Department>
            {
                new Department
                {
                    Id = new System.Guid().ToString(),
                    Name = "TestDepartment",
                    Employees = new HashSet<Employee>(),
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
            };

            A.CallTo(() => departmentService.GetAll()).Returns(departments);

            this.controller = new AddEmployeeController(departmentService, employeeService, summaryService);
        }

        [Fact]
        public void AddEmployeeController_Index_ReturnsViewModel()
        {
            // Arrange
            A.CallTo(() => departmentService.GetAll());

            // Act
            var result = this.controller.Index();

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async void AddEmployeeController_CreateAsync_ReturnsRedirectToActionResult()
        {
            // Arrange
            A.CallTo(() => departmentService.GetAll());

            CreateEmployeeAndSummaryViewModel input = new CreateEmployeeAndSummaryViewModel
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Position = "TestPosition",
                PhoneNumber = "TestPhoneNumber",
                CurrentStatus = Data.Models.Status.Active,
                Email = "TestEmail",
                DepartmentId = departmentService.GetAll().First().Id,
                Departments = departmentService.GetAll(),
            };

            // Act
            var result = await this.controller.CreateAsync(input);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
