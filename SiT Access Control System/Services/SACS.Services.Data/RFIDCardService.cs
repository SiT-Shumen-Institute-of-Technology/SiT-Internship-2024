namespace SACS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Common.Repositories;
    using SACS.Data.Models;
    using SACS.Services.Data.Interfaces;

    public class RFIDCardService : IRFIDCardService
    {
        private readonly IDeletableEntityRepository<RFIDCard> rfidCardRepository;

        public RFIDCardService(IDeletableEntityRepository<RFIDCard> rfidCardRepository)
        {
            this.rfidCardRepository = rfidCardRepository;
        }

        public async Task AddAsync(RFIDCard rfidCard)
        {
            await this.rfidCardRepository.AddAsync(rfidCard);
            await this.rfidCardRepository.SaveChangesAsync();
        }

        public List<RFIDCard> All()
        {
            return this.rfidCardRepository.All().ToList();
        }
    }
}
