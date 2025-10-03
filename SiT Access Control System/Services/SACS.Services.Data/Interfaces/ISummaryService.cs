using System.Collections.Generic;
using System.Threading.Tasks;
using SACS.Data.Models;

namespace SACS.Services.Data.Interfaces;

public interface ISummaryService
{
    List<Summary> GetAllSummaries();

    void DeleteSummaryByEmployeeId(string employeeId);

    Task DeleteSummaryByEmployeeIdAsync(string employeeId);

    Task CreateSummaryAsync(Summary summary);
}