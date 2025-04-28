using SACS.Data.Models;
using SACS.Services.Data.Interfaces;


namespace SACS.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    [IgnoreAntiforgeryToken]
    public class CardReaderAPIController : Controller
    {
        private readonly IRFIDCardService rfidCardService;
        private readonly IEmployeeRFIDCardService employeeRFIDCardService;
        private bool isANewCard = true;

        public CardReaderAPIController(IRFIDCardService rfidCardService, IEmployeeRFIDCardService employeeRFIDCardService)
        {
            this.rfidCardService = rfidCardService;
            this.employeeRFIDCardService = employeeRFIDCardService;
        }

        [HttpPost("apipost")]
        public async Task<ActionResult<RFIDCard>> GetAndAddRFIDCard([FromQuery] string code)
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
                RFIDCard rfidCard = new RFIDCard
                {
                    Id = Guid.NewGuid().ToString(),

                    Code = code,
                };
                await this.rfidCardService.AddAsync(rfidCard);
                return this.Content("Not Connected", "text/plain", Encoding.UTF8);
            }
            else
            {
                if (this.employeeRFIDCardService.All() != null)
                {
                    if (this.employeeRFIDCardService.All().FirstOrDefault(x => x.RFIDCard.Code == code) != null)
                    {
                        return this.Content("Connected", "text/plain", Encoding.UTF8);
                    }
                    else
                    {
                        return this.Content("Not Connected", "text/plain", Encoding.UTF8);
                    }
                }
                else
                {
                    return this.Content("Not Connected", "text/plain", Encoding.UTF8);
                }
            }
        }

        [HttpGet]
        public List<EmployeeRFIDCard> GetAllRFIDCards()
        {
            return this.employeeRFIDCardService.All();
        }
    }
}
