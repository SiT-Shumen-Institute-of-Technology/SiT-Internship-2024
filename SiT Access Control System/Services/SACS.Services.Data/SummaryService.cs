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

    public void DeleteSummaryByEmployeeId(string employeeId)
    {
        var summaries = summaryRepository.All().Where(s => s.EmployeeId == employeeId).ToList();

        foreach (var summary in summaries)
        {
            summaryRepository.Delete(summary);
        }

        summaryRepository.SaveChangesAsync().GetAwaiter().GetResult(); // sync call
    }

    public async Task DeleteSummaryByEmployeeIdAsync(string employeeId)
    {
        var summaries = summaryRepository.All().Where(s => s.EmployeeId == employeeId).ToList();
        if (summaries.Any())
        {
            foreach (var summary in summaries)
            {
                summaryRepository.Delete(summary);
            }
            await summaryRepository.SaveChangesAsync();
        }
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