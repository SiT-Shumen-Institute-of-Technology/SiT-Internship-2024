using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SACS.Data;
using SACS.Data.Models;
using SACS.Services.Data;
using SACS.Web.ViewModels.Administration.Dashboard;

namespace SACS.Web.Areas.Administration.Controllers
{
    public class DashboardController : AdministrationController
    {
        private readonly ISettingsService settingsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext _context;

        public DashboardController(
            ISettingsService settingsService,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.settingsService = settingsService;
            this.userManager = userManager;
            this._context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();
            var currentUser = await userManager.GetUserAsync(User);

            var model = new IndexViewModel
            {
                SettingsCount = settingsService.GetCount(),
                Users = users,
                CurrentUser = currentUser
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUsers(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index"); // If no ID, just reload page
            }

            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index"); // Refresh page after deletion
        }
    }
}

