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

    public class SummaryServiceTest
    {
        private readonly IDeletableEntityRepository<DailySummary> summaryRepository = A.Fake<IDeletableEntityRepository<DailySummary>>();
        private ISummaryService summaryService;

        public SummaryServiceTest()
        {
            List<DailySummary> dailySummaries = new List<DailySummary>
            {
                new DailySummary
                {
                    Id = Guid.NewGuid().ToString(),
                    CurrentStatus = Status.Active,
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
            };

            A.CallTo(() => this.summaryRepository.All()).Returns(dailySummaries.AsQueryable());

            this.summaryService = new SummaryService(this.summaryRepository);
        }

        [Fact]
        public async Task SummaryService_CreateSummaryAsync_AddsRepository()
        {
            // Arrange
            var summary = this.summaryRepository.All().First();

            // Act
            var result = this.summaryService.CreateSummaryAsync(summary);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
