namespace SACS.Web.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FakeItEasy;
    using FluentAssertions;
    using SACS.Data.Common.Repositories;
    using SACS.Data.Models;
    using SACS.Services.Data;
    using Xunit;

    public class EmployeeRFIDCardServiceTest
    {
        private readonly IDeletableEntityRepository<EmployeeRFIDCard> employeeRFIDCardRepository = A.Fake<IDeletableEntityRepository<EmployeeRFIDCard>>();
        private EmployeeRFIDCardService employeeRFIDCardService;

        public EmployeeRFIDCardServiceTest()
        {
            List<RFIDCard> rFIDCards = new List<RFIDCard>()
            {
                new RFIDCard
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = "TestCode",
                },
            };

            List<ApplicationUser> employees = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Id = new Guid().ToString(),
                    UserName = "test",
                    PhoneNumber = "TestEmployeePhoneNumber",
                    Email = "TestEmployeeEmail",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
            };

            List<EmployeeRFIDCard> employeeRFIDCards = new List<EmployeeRFIDCard>()
            {
                new EmployeeRFIDCard
                {
                    Id = Guid.NewGuid().ToString(),
                    User = employees[0],
                    UserId = employees[0].Id,
                    RFIDCard = rFIDCards[0],
                    RFIDCardId = rFIDCards[0].Id,
                },
            };

            A.CallTo(() => this.employeeRFIDCardRepository.All()).Returns(employeeRFIDCards.AsQueryable());
            this.employeeRFIDCardService = new EmployeeRFIDCardService(this.employeeRFIDCardRepository);
        }

        [Fact]
        public async Task EmployeeRFIDCardService_AddEmployeeAndRFIDCardServiceAsync_AwaitsEmployeeRFIDCardRepository()
        {
            // Arrange
            var employee = this.employeeRFIDCardRepository.All().First();

            // Act
            var result = this.employeeRFIDCardService.AddEmployeeAndRFIDCardServiceAsync(employee);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
