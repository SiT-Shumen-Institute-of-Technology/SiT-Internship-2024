namespace SACS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.DotNet.Scaffolding.Shared.Messaging;
    using SACS.Data.Models;
    using SACS.Services.Data;

    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class CardReaderAPIController : Controller
    {
        private readonly IRFIDCardService rfidCardService;
        private readonly IEmployeeService employeeService;
        private readonly IEmployeeRFIDCardService employeeRFIDCardService;
        private bool isANewCard = true;

        public CardReaderAPIController(IRFIDCardService rfidCardService, IEmployeeService employeeService, IEmployeeRFIDCardService employeeRFIDCardService)
        {
            this.rfidCardService = rfidCardService;
            this.employeeService = employeeService;
            this.employeeRFIDCardService = employeeRFIDCardService;
        }

        [HttpPost("apipost")]
        public async Task<ActionResult<RFIDCard>> GetAndAddRFIDCard([FromQuery]string code)
        {
            if (this.rfidCardService.All() != null)
            {
                foreach (var card in this.rfidCardService.All())
                {
                    if (card.Code == code)
                    {
                        this.isANewCard = false;
                        break;
                    }
                    else
                    {
                        this.isANewCard = true;
                    }
                }
            }

            if (this.isANewCard == true)
            {
                //TEMPORARY

                Random random = new Random();
                var allEmployees = this.employeeService.GetAllEmployees();

                //TEMPORARY

                var randomEmployeeId = allEmployees[random.Next(0, this.employeeService.GetAllEmployees().Count)].Id;

                RFIDCard rfidCard = new RFIDCard
                {
                    Id = Guid.NewGuid().ToString(),

                    Code = code,
                };

                EmployeeRFIDCard employeeRFIDCard = new EmployeeRFIDCard
                {
                    Id = Guid.NewGuid().ToString(),
                    EmployeeId = randomEmployeeId,
                    Employee = this.employeeService.FindEmployeeById(randomEmployeeId),
                    RFIDCardId = rfidCard.Id,
                    RFIDCard = rfidCard,
                };
                await this.employeeRFIDCardService.AddEmployeeAndRFIDCardServiceAsync(employeeRFIDCard);
                await this.rfidCardService.AddAsync(rfidCard);
                return rfidCard;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public List<EmployeeRFIDCard> GetAllRFIDCards()
        {
            return this.employeeRFIDCardService.All();
        }
    }
}
