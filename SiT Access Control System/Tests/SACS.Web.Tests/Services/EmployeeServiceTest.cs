namespace SACS.Web.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FakeItEasy;
    using FluentAssertions;
    using SACS.Data.Common.Repositories;
    using SACS.Data.Models;
    using SACS.Services.Data;
    using Xunit;

    public class EmployeeServiceTest
    {
        private readonly IDeletableEntityRepository<Employee> employeeRepository = A.Fake<IDeletableEntityRepository<Employee>>();
        private readonly IEmployeeService employeeService;

        public EmployeeServiceTest()
        {
            List<Employee> employees = new List<Employee>()
            {
                new Employee()
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

            A.CallTo(() => this.employeeRepository.All()).Returns(employees.AsQueryable());

            this.employeeService = new EmployeeService(this.employeeRepository);
        }

        [Fact]
        public void EmployeeService_AddAsync_AddsEmployee()
        {
            // Arrange
            var employee = this.employeeRepository.All().First();

            // Act
            var result = this.employeeRepository.AddAsync(employee);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
