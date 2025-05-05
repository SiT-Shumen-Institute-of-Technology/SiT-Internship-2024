using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SACS.Data.Common.Repositories;
using SACS.Data.Models;
using SACS.Services.Data.Interfaces;

namespace SACS.Services.Data;

public class RFIDCardService : IRFIDCardService
{
    private readonly IDeletableEntityRepository<RFIDCard> rfidCardRepository;

    public RFIDCardService(IDeletableEntityRepository<RFIDCard> rfidCardRepository)
    {
        this.rfidCardRepository = rfidCardRepository;
    }

    public async Task AddAsync(RFIDCard rfidCard)
    {
        await rfidCardRepository.AddAsync(rfidCard);
        await rfidCardRepository.SaveChangesAsync();
    }

    public List<RFIDCard> All()
    {
        return rfidCardRepository.All().ToList();
    }
}