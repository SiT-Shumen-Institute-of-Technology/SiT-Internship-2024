using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SACS.Data;
using SACS.Data.Models;
using SACS.Services.Data;
using SACS.Web.ViewModels.Administration.Dashboard;

namespace SACS.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService _settingsService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DashboardController(
            ISettingsService settingsService,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _settingsService = settingsService;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users
                .OrderBy(u => u.UserName)
                .ToListAsync();

            var currentUser = await _userManager.GetUserAsync(User);

            return View(new IndexViewModel
            {
                SettingsCount = _settingsService.GetCount(),
                Users = users,
                CurrentUser = currentUser
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to delete user");
            }

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(string id, string userName, string email)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return BadRequest("User ID is required");
                }

                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound("User not found");
                }

                // Validate input
                if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest("Username and email are required");
                }

                // Check for duplicate username
                var existingUser = await _userManager.FindByNameAsync(userName);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    return BadRequest("Username is already taken");
                }

                // Check for duplicate email
                existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser != null && existingUser.Id != user.Id)
                {
                    return BadRequest("Email is already in use");
                }

                // Update user properties
                user.UserName = userName;
                user.Email = email;
                user.NormalizedUserName = userName.ToUpper();
                user.NormalizedEmail = email.ToUpper();

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }
    }
}