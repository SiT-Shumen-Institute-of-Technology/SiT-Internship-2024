using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SACS.Common;
using SACS.Data.Models;
using SACS.Services.Data;
using SACS.Web.ViewModels.Administration.Users;

namespace SACS.Web.Areas.Administration.Controllers;

[Authorize(Roles = GlobalConstants.AdministratorRoleName)]
public class DashboardController : AdministrationController
{
    private readonly IDashboardService _dashboardService;
    private readonly IUserManagementService _userManagementService;
    private readonly UserManager<ApplicationUser> _userManager;

    public DashboardController(
        IDashboardService dashboardService,
        IUserManagementService userManagementService,
        UserManager<ApplicationUser> userManager)
    {
        _dashboardService = dashboardService;
        _userManagementService = userManagementService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = await _dashboardService.GetDashboardDataAsync(User);
        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(string id)
    {
        try
        {
            await _userManagementService.DeleteUserAsync(id);
            return Ok();
        }
        catch (ArgumentException)
        {
            return RedirectToAction(nameof(Index));
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateUser(string id, string userName, string email, string role)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // Update editable fields
            user.UserName = userName;
            user.NormalizedUserName = userName.ToUpperInvariant();
            user.Email = email;
            user.NormalizedEmail = email.ToUpperInvariant();

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded) return BadRequest("Failed to update user.");

            // Update role if changed
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(role))
            {
                foreach (var currentRole in currentRoles)
                    await _userManager.RemoveFromRoleAsync(user, currentRole);

                await _userManager.AddToRoleAsync(user, role);
            }

            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
        }
    }

    [HttpGet]
    public IActionResult AddUser()
    {
        var model = new CreateUserViewModel
        {
            Roles = GetAvailableRoles()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddUser(CreateUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.Roles = GetAvailableRoles();
            return View(model);
        }

        // Auto-generate username from FirstName + LastName if left blank
        var username = string.IsNullOrWhiteSpace(model.UserName)
            ? $"{model.FirstName}{model.LastName}".ToLowerInvariant()
            : model.UserName;

        var newUser = new ApplicationUser
        {
            UserName = username,
            NormalizedUserName = username.ToUpperInvariant(),
            Email = model.Email,
            NormalizedEmail = model.Email.ToUpperInvariant(),
            EmailConfirmed = false,
        };

        var result = await _userManager.CreateAsync(newUser, model.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            model.Roles = GetAvailableRoles();
            return View(model);
        }

        await _userManager.AddToRoleAsync(newUser, model.SelectedRole);

        return RedirectToAction(nameof(Index));
    }

    private List<SelectListItem> GetAvailableRoles()
    {
        return new[]
            {
                GlobalConstants.AdministratorRoleName,
                GlobalConstants.UserRoleName,
                GlobalConstants.EmployeeRoleName
            }
            .Select(role => new SelectListItem { Value = role, Text = role })
            .ToList();
    }
}
