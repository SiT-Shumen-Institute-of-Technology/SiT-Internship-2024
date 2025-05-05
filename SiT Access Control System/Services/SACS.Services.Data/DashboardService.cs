using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SACS.Data.Models;
using SACS.Web.ViewModels.Administration.Dashboard;

namespace SACS.Services.Data;

public class DashboardService : IDashboardService
{
    private readonly IUserManagementService _userManagementService;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardService(
        IUserManagementService userManagementService,
        UserManager<ApplicationUser> userManager)
    {
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

        // Ensure the current user is valid
        var currentUser = await _userManagementService.GetCurrentUserAsync(principal);
        List<string> currentUserRoles = new();

        if (currentUser != null) currentUserRoles = (await _userManager.GetRolesAsync(currentUser)).ToList();

        // Optional: log this or handle it differently based on your needs
        // throw new InvalidOperationException("Current user could not be found.");
        return new IndexViewModel
        {
            Users = rows,
            CurrentUser = currentUser
        };
    }
}