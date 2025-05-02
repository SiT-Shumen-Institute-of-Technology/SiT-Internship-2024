namespace SACS.Web.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using FakeItEasy;
    using FluentAssertions;
    using SACS.Data.Models;
    using SACS.Services.Data;
    using SACS.Web.Controllers;
    using SendGrid.Helpers.Mail;
    using Xunit;

    public class CardReaderAPIControllerTest
    {
        private readonly IRFIDCardService rfidCardService = A.Fake<IRFIDCardService>();
        private readonly IEmployeeRFIDCardService employeeRFIDCardService = A.Fake<IEmployeeRFIDCardService>();
        private bool isANewCard = true;
        CardReaderAPIController controller;

        public CardReaderAPIControllerTest()
        {
            List<RFIDCard> rFIDCards = new List<RFIDCard>
            {
                new RFIDCard
                {
                    Id = new Guid().ToString(),
                    Code = "TestCode",
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
            };

            A.CallTo(() => this.rfidCardService.All()).Returns(rFIDCards);

            List<EmployeeRFIDCard> employeeRFIDCards = new List<EmployeeRFIDCard>
            {
                new EmployeeRFIDCard
                {
                    Id = new Guid().ToString(),
                    RFIDCard = rFIDCards.First(),
                    RFIDCardId = rFIDCards.First().Id,
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                },
            };

            A.CallTo(() => this.employeeRFIDCardService.All()).Returns(employeeRFIDCards);

            this.controller = new CardReaderAPIController(rfidCardService, employeeRFIDCardService);
        }

        [Fact]

        public async void CardReaderAPIController_GetAndAddRFIDCard_ReturnsContentResultNotConnected()
        {
            // Arrange
            string code = "TestCode";
            A.CallTo(() => this.employeeRFIDCardService.All());
            A.CallTo(() => this.rfidCardService.All());

            // Act
            var result = await this.controller.GetAndAddRFIDCard(code);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
