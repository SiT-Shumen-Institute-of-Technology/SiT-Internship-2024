using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SACS.Common;
using SACS.Data.Models;
using SACS.Web.ViewModels.Administration.Users;

[Authorize(Roles = "Administrator")]
public class UserController : Controller
{
    private List<SelectListItem> GetAvailableRoles()
    {
        return new List<SelectListItem>
        {
            new() { Text = GlobalConstants.AdministratorRoleName, Value = GlobalConstants.AdministratorRoleName },
            new() { Text = GlobalConstants.UserRoleName, Value = GlobalConstants.UserRoleName },
            new() { Text = GlobalConstants.EmployeeRoleName, Value = GlobalConstants.EmployeeRoleName }
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
                foreach (var error in state.Value.Errors)
                    Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");

            model.Roles = GetAvailableRoles();
            return View("~/Views/CreationOfNewUsers/Create.cshtml", model);
        }

        try
        {
            // Ensure first and last names are present if required
            if (string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName))
            {
                ModelState.AddModelError(string.Empty, "First name and last name are required.");
                model.Roles = GetAvailableRoles();
                return View("~/Views/CreationOfNewUsers/Create.cshtml", model);
            }

            // Generate UserName from FirstName and LastName
            var userName = $"{model.FirstName}{model.LastName}";

            var user = new ApplicationUser
            {
                UserName = userName,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
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
                            ModelState.AddModelError(string.Empty, $"Role creation failed: {error.Description}");
                        model.Roles = GetAvailableRoles();
                        return View("~/Views/CreationOfNewUsers/Create.cshtml", model);
                    }
                }

                var addToRoleResult = await userManager.AddToRoleAsync(user, model.SelectedRole);
                if (!addToRoleResult.Succeeded)
                {
                    foreach (var error in addToRoleResult.Errors)
                        ModelState.AddModelError(string.Empty, $"Role assignment failed: {error.Description}");
                    model.Roles = GetAvailableRoles();
                    return View("~/Views/CreationOfNewUsers/Create.cshtml", model);
                }

                return RedirectToAction("Index", "Dashboard", new { area = "Administration" });
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
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
