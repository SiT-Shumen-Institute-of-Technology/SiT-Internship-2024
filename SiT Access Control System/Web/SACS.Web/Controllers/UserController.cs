using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SACS.Web.ViewModels.Administration.Dashboard;
using SACS.Common;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using SACS.Data.Models;
using SACS.Web.ViewModels.Administration.Users;
using System.Threading.Tasks;

[Authorize(Roles = "Administrator")]
public class UserController : Controller
{
    // Method to get available roles
    private List<SelectListItem> GetAvailableRoles()
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Text = GlobalConstants.AdministratorRoleName, Value = GlobalConstants.AdministratorRoleName },
            new SelectListItem { Text = GlobalConstants.UserRoleName, Value = GlobalConstants.UserRoleName },
            new SelectListItem { Text = GlobalConstants.EmployeeRoleName, Value = GlobalConstants.EmployeeRoleName },
        };
    }

    // GET method for creating a new user
    [HttpGet]
    public IActionResult Create()
    {
        var model = new CreateUserViewModel
        {
            Roles = GetAvailableRoles()  // Use the GetAvailableRoles method
        };

        return View("~/Views/CreationOfNewUsers/Create.cshtml", model);
    }

    // POST method for creating a new user
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model, [FromServices] UserManager<ApplicationUser> userManager, [FromServices] RoleManager<ApplicationRole> roleManager)
{
    if (!ModelState.IsValid)
    {
        model.Roles = GetAvailableRoles();  // Ensure roles are repopulated on validation failure
        return View("~/Views/CreationOfNewUsers/Create.cshtml", model);
    }

    var user = new ApplicationUser
    {
        UserName = model.UserName,
        Email = model.Email,
    };

    var result = await userManager.CreateAsync(user, model.Password);

    if (result.Succeeded)
    {
        // Check if the selected role exists, if not create it
        if (!await roleManager.RoleExistsAsync(model.SelectedRole))
        {
            await roleManager.CreateAsync(new ApplicationRole(model.SelectedRole));
        }

        // Add the user to the selected role
        await userManager.AddToRoleAsync(user, model.SelectedRole);

        // Redirect to the Admin Dashboard after successful user creation
        return RedirectToAction("Index", "Dashboard", new { area = "Administration" });
    }

    // Add errors to the model if the user creation failed
    foreach (var error in result.Errors)
    {
        ModelState.AddModelError(string.Empty, error.Description);
    }

    model.Roles = GetAvailableRoles();  // Ensure roles are repopulated on error
    return View("~/Views/CreationOfNewUsers/Create.cshtml", model);
}
}
