using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Web.ViewModels.Administration.Dashboard;

namespace SACS.Services.Data
{
    public interface IDashboardService
    {
        Task<IndexViewModel> GetDashboardDataAsync(System.Security.Claims.ClaimsPrincipal user);
    }
}
