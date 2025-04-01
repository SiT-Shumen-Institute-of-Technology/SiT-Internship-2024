using System;
using System.Collections.Generic;
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
        private readonly IDashboardService _dashboardService;
        private readonly IUserManagementService _userManagementService;

        public DashboardController(
            IDashboardService dashboardService,
            IUserManagementService userManagementService)
        {
            _dashboardService = dashboardService;
            _userManagementService = userManagementService;
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
            catch (ArgumentException ex)
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
        public async Task<IActionResult> UpdateUser(string id, string userName, string email)
        {
            try
            {
                await _userManagementService.UpdateUserAsync(id, userName, email);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }
    }
}
