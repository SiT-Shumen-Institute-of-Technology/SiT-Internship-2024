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
using System;

[Authorize(Roles = "Administrator")]
public class UserController : Controller
{
    private List<SelectListItem> GetAvailableRoles()
    {
        return new List<SelectListItem>
        {
            new SelectListItem { Text = GlobalConstants.AdministratorRoleName, Value = GlobalConstants.AdministratorRoleName },
            new SelectListItem { Text = GlobalConstants.UserRoleName, Value = GlobalConstants.UserRoleName },
            new SelectListItem { Text = GlobalConstants.EmployeeRoleName, Value = GlobalConstants.EmployeeRoleName },
        };
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new CreateUserViewModel
        {
            Roles = GetAvailableRoles()
        };

        return View("~/Views/CreationOfNewUsers/Create.cshtml", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserViewModel model,
    [FromServices] UserManager<ApplicationUser> userManager,
    [FromServices] RoleManager<ApplicationRole> roleManager)
    {
        Console.WriteLine($"Received role: {model.SelectedRole}");

        if (!ModelState.IsValid)
        {
            // Debug: Check validation errors
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
                }
            }

            model.Roles = GetAvailableRoles();
            return View("~/Views/CreationOfNewUsers/Create.cshtml", model);
        }

        try
        {
            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(model.SelectedRole))
                {
                    var roleResult = await roleManager.CreateAsync(new ApplicationRole(model.SelectedRole));
                    if (!roleResult.Succeeded)
                    {
                        // Log or handle role creation errors
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError(string.Empty, $"Role creation failed: {error.Description}");
                        }
                        model.Roles = GetAvailableRoles();
                        return View("~/Views/CreationOfNewUsers/Create.cshtml", model);
                    }
                }

                var addToRoleResult = await userManager.AddToRoleAsync(user, model.SelectedRole);
                if (!addToRoleResult.Succeeded)
                {
                    foreach (var error in addToRoleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, $"Role assignment failed: {error.Description}");
                    }
                    model.Roles = GetAvailableRoles();
                    return View("~/Views/CreationOfNewUsers/Create.cshtml", model);
                }

                return RedirectToAction("Index", "Dashboard", new { area = "Administration" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        catch (Exception ex)
        {
            // Log the exception
            ModelState.AddModelError(string.Empty, "An error occurred while creating the user.");
            // For development, you might want to see the actual error:
            ModelState.AddModelError(string.Empty, ex.Message);
        }

        model.Roles = GetAvailableRoles();
        return View("~/Views/CreationOfNewUsers/Create.cshtml", model);
    }
}