using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SACS.Common;
using SACS.Data.Models;
using SACS.Web.ViewModels.Administration.Dashboard;

namespace SACS.Services.Data
{
    public class DashboardService : IDashboardService
    {
        private readonly ISettingsService _settingsService;
        private readonly IUserManagementService _userManagementService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DashboardService(
            ISettingsService settingsService,
            IUserManagementService userManagementService,
            UserManager<ApplicationUser> userManager)
        {
            _settingsService = settingsService;
            _userManagementService = userManagementService;
            _userManager = userManager;
        }

        public async Task<IndexViewModel> GetDashboardDataAsync(ClaimsPrincipal principal)
        {
            var users = await _userManagementService.GetAllUsersAsync();
            var rows = new List<IndexViewModel.UserRow>();

            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);

                rows.Add(new IndexViewModel.UserRow
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    Roles = roles.ToList()
                });
            }

            // Ensure the current user has the correct roles
            var currentUser = await _userManagementService.GetCurrentUserAsync(principal);
            var currentUserRoles = await _userManager.GetRolesAsync(currentUser);

            // Add the current user's roles for correct display
            return new IndexViewModel
            {
                SettingsCount = _settingsService.GetCount(),
                Users = rows,
                CurrentUser = currentUser
            };
        }
    }
}
