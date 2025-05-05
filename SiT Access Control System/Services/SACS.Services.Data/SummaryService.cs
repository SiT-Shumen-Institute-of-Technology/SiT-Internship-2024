using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SACS.Data.Common.Repositories;
using SACS.Data.Models;
using SACS.Services.Data.Interfaces;

namespace SACS.Services.Data;

public class SummaryService : ISummaryService
{
    private readonly IRepository<Summary> summaryRepository;

    public SummaryService(IRepository<Summary> summaryRepository)
    {
        this.summaryRepository = summaryRepository;
    }


    public async Task CreateSummaryAsync(Summary summary)
    {
        await summaryRepository.AddAsync(summary);
        await summaryRepository.SaveChangesAsync();
    }

    public List<Summary> GetAllSummaries()
    {
        return summaryRepository.All().ToList();
    }
}