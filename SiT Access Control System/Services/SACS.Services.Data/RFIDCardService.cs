namespace SACS.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using SACS.Data.Common.Repositories;
    using SACS.Data.Models;

    public class RFIDCardService : IRFIDCardService
    {
        private readonly IDeletableEntityRepository<RFIDCard> rfidCardRepository;

        public RFIDCardService(IDeletableEntityRepository<RFIDCard> rfidCardRepository)
        {
            this.rfidCardRepository = rfidCardRepository;
        }

        public void Add(RFIDCard rfidCard)
        {
            this.rfidCardRepository.AddAsync(rfidCard);
            this.rfidCardRepository.SaveChangesAsync();
        }

        public List<RFIDCard> All()
        {
            return this.rfidCardRepository.All().ToList();
        }
    }
}
