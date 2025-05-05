using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SACS.Data.Models;
using SACS.Services.Data.Interfaces;

namespace SACS.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[IgnoreAntiforgeryToken]
public class CardReaderAPIController : Controller
{
    private readonly IEmployeeRFIDCardService employeeRFIDCardService;
    private readonly IRFIDCardService rfidCardService;
    private bool isANewCard = true;

    public CardReaderAPIController(IRFIDCardService rfidCardService, IEmployeeRFIDCardService employeeRFIDCardService)
    {
        this.rfidCardService = rfidCardService;
        this.employeeRFIDCardService = employeeRFIDCardService;
    }

    [HttpPost("apipost")]
    public async Task<ActionResult<RFIDCard>> GetAndAddRFIDCard([FromQuery] string code)
    {
        if (rfidCardService.All() != null)
            foreach (var card in rfidCardService.All())
            {
                if (card.Code == code)
                {
                    isANewCard = false;
                    break;
                }

                isANewCard = true;
            }

        if (isANewCard)
        {
            var rfidCard = new RFIDCard
            {
                Id = Guid.NewGuid().ToString(),

                Code = code
            };
            await rfidCardService.AddAsync(rfidCard);
            return Content("Not Connected", "text/plain", Encoding.UTF8);
        }

        if (employeeRFIDCardService.All() != null)
        {
            if (employeeRFIDCardService.All().FirstOrDefault(x => x.RFIDCard.Code == code) != null)
                return Content("Connected", "text/plain", Encoding.UTF8);

            return Content("Not Connected", "text/plain", Encoding.UTF8);
        }

        return Content("Not Connected", "text/plain", Encoding.UTF8);
    }

    [HttpGet]
    public List<EmployeeRFIDCard> GetAllRFIDCards()
    {
        return employeeRFIDCardService.All();
    }
}