using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SACS.Data.Common.Repositories;
using SACS.Data.Models;

namespace SACS.Services.Data;

public class UserManagementService : IUserManagementService
{
    private readonly IDeletableEntityRepository<ApplicationUser> repository;
    private readonly UserManager<ApplicationUser> userManager;

    public UserManagementService(
        UserManager<ApplicationUser> userManager,
        IDeletableEntityRepository<ApplicationUser> repository)
    {
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public List<ApplicationUser> GetAllUsers()
    {
        return repository.All().ToList();
    }

    public ApplicationUser GetUserById(string id)
    {
        return repository.All().FirstOrDefault(u => u.Id == id);
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
    {
        return await userManager.Users
            .OrderBy(u => u.UserName)
            .ToListAsync();
    }

    public async Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal user)
    {
        return await userManager.GetUserAsync(user);
    }

    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password, string role)
    {
        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded) await userManager.AddToRoleAsync(user, role);

        return result;
    }

    public async Task DeleteUserAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
            throw new KeyNotFoundException("User not found.");

        // Remove user roles first
        var roles = await userManager.GetRolesAsync(user);
        if (roles.Any())
        {
            var roleRemovalResult = await userManager.RemoveFromRolesAsync(user, roles);
            if (!roleRemovalResult.Succeeded)
                throw new InvalidOperationException("Failed to remove user roles before deletion.");
        }

        // Now delete the user
        var result = await userManager.DeleteAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException("Failed to delete user.");
    }

    public async Task UpdateUserAsync(string id, string userName, string email)
    {
        if (string.IsNullOrEmpty(id)) throw new ArgumentException("User ID is required");

        var user = await userManager.FindByIdAsync(id);
        if (user == null) throw new KeyNotFoundException("User not found");

        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Username and email are required");

        // Check for duplicate username
        var existingUser = await userManager.FindByNameAsync(userName);
        if (existingUser != null && existingUser.Id != user.Id)
            throw new InvalidOperationException("Username is already taken");

        // Check for duplicate email
        existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser != null && existingUser.Id != user.Id)
            throw new InvalidOperationException("Email is already in use");

        // Update user properties
        user.UserName = userName;
        user.Email = email;
        user.NormalizedUserName = userName.ToUpper();
        user.NormalizedEmail = email.ToUpper();

        var result = await userManager.UpdateAsync(user);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                $"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
    }

    public async Task<List<ApplicationUser>> GetUsersInRoleAsync(string roleName)
    {
        return (await userManager.GetUsersInRoleAsync(roleName)).ToList();
    }

}