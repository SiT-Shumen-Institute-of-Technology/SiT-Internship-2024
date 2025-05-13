namespace SACS.Web.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FakeItEasy;
    using FluentAssertions;
    using Microsoft.Identity.Client;
    using SACS.Data.Common.Repositories;
    using SACS.Data.Models;
    using SACS.Services.Data;
    using Xunit;

    public class DayServiceTest
    {
        private readonly IDeletableEntityRepository<Day> dayRepository = A.Fake<IDeletableEntityRepository<Day>>();
        private DayService dayService;

        public DayServiceTest()
        {
            List<Day> days = new List<Day>()
            {
                new Day()
                {
                    Id = Guid.NewGuid().ToString(),
                    Date = DateTime.Now,
                    WorkedHours = 1,
                    Employee = A.Fake<Employee>(),
                    State = 's',
                },
            };
            A.CallTo(() => this.dayRepository.All()).Returns(days.AsQueryable());
        }

        [Fact]
        public async Task DayService_AddAsync_AddsDay()
        {
            // Arrange
            var day = this.dayRepository.All().First();

            // Act
            var result = this.dayService.AddAsync(day);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task DayService_RemoveByIdAsync_RemovesDay()
        {
            // Arrange
            var day = this.dayRepository.All().First();

            // Act
            var result = this.dayService.RemoveByIdAsync(day.Id);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
