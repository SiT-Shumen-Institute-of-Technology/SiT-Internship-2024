using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SACS.Data.Models;
using SACS.Web.ViewModels;
using SACS.Web.ViewModels.Administration.Dashboard;

namespace SACS.Web.Controllers;

public class HomeController : BaseController
{
    private readonly UserManager<ApplicationUser> userManager;

    public HomeController(UserManager<ApplicationUser> userManager)
    {
        this.userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var allUsers = await userManager.Users.ToListAsync();
        var userRows = new List<IndexViewModel.UserRow>();

        foreach (var user in allUsers)
        {
            var roles = await userManager.GetRolesAsync(user);

            userRows.Add(new IndexViewModel.UserRow
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = roles
            });
        }

        var model = new IndexViewModel
        {
            Users = userRows
        };

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
