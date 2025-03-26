namespace SACS.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using SACS.Data.Models;
    using SACS.Services.Data;

    [Route("/api/[controller]")]
    [ApiController]
    public class CardReaderAPIController : Controller
    {
        private readonly IRFIDCardService rfidCardService;

        public CardReaderAPIController(IRFIDCardService rfidCardService)
        {
            this.rfidCardService = rfidCardService;
        }

        [HttpPost]
        public async Task GetAndAddRFIDCard([FromForm] string code)
        {
            RFIDCard rfidCard = new RFIDCard
            {
                Code = code,
            };
            await this.rfidCardService.AddAsync(rfidCard);
        }

        [HttpGet]
        public List<RFIDCard> GetAllRFIDCards()
        {
            return this.rfidCardService.All();
        }
    }
}
