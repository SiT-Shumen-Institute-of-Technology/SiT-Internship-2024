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

    public class RFIDCardServiceTest
    {
        private readonly IDeletableEntityRepository<RFIDCard> rfidCardRepository = A.Fake<IDeletableEntityRepository<RFIDCard>>();
        private readonly IRFIDCardService rFIDCardService;

        public RFIDCardServiceTest()
        {
            List<RFIDCard> rFIDCards = new List<RFIDCard>()
            {
                new RFIDCard
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = "TestCode",
                },
            };

            A.CallTo(() => this.rfidCardRepository.All()).Returns(rFIDCards.AsQueryable());

            this.rFIDCardService = new RFIDCardService(this.rfidCardRepository);
        }

        public async Task RFIDCardService_AddAsync_AddsRFIDCards()
        {
            // Arrange
            var rfidCard = this.rfidCardRepository.All().First();

            // Act
            var result = this.rfidCardRepository.AddAsync(rfidCard);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
