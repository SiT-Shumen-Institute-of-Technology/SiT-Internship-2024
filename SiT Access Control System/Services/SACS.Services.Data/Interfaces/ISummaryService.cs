using System.Collections.Generic;
using System.Threading.Tasks;
using SACS.Data.Models;

namespace SACS.Services.Data.Interfaces;

public interface ISummaryService
{
    List<Summary> GetAllSummaries();

    Task CreateSummaryAsync(Summary summary);
}