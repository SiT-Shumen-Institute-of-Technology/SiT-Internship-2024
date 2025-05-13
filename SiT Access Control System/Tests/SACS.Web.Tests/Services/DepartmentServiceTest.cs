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

    public class DepartmentServiceTest
    {
        private readonly IDeletableEntityRepository<Department> departmentRepository = A.Fake<IDeletableEntityRepository<Department>>();
        private DepartmentService departmentService;

        public DepartmentServiceTest()
        {
            List<Department> departments = new List<Department>()
            {
                new Department
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = "TestDepartment",
                },
            };

            A.CallTo(() => this.departmentRepository.All()).Returns(departments.AsQueryable());
            this.departmentService = new DepartmentService(this.departmentRepository);
        }

        [Fact]
        public void DepartmentService_Add_AddsDepartment()
        {
            // Arrange
            var department = this.departmentRepository.All().First();

            // Act
            var result = this.departmentService.AddAsync(department);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
