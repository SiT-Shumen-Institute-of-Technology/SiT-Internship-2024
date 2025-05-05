using System.Security.Claims;
using System.Threading.Tasks;
using SACS.Web.ViewModels.Administration.Dashboard;

namespace SACS.Services.Data;

public interface IDashboardService
{
    Task<IndexViewModel> GetDashboardDataAsync(ClaimsPrincipal user);
}