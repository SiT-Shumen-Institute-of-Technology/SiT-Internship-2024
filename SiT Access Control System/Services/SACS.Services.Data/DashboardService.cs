using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Web.ViewModels.Administration.Dashboard;

namespace SACS.Services.Data
{
    public class DashboardService : IDashboardService
    {
        private readonly ISettingsService _settingsService;
        private readonly IUserManagementService _userManagementService;

        public DashboardService(
            ISettingsService settingsService,
            IUserManagementService userManagementService)
        {
            _settingsService = settingsService;
            _userManagementService = userManagementService;
        }

        public async Task<IndexViewModel> GetDashboardDataAsync(System.Security.Claims.ClaimsPrincipal user)
        {
            var users = await _userManagementService.GetAllUsersAsync();
            var currentUser = await _userManagementService.GetCurrentUserAsync(user);

            return new IndexViewModel
            {
                SettingsCount = _settingsService.GetCount(),
                Users = users.ToList(),
                CurrentUser = currentUser
            };
        }
    }
}
