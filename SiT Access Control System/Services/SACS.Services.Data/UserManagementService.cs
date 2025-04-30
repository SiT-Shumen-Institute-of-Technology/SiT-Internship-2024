using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SACS.Data.Models;
using SACS.Data;
using Microsoft.EntityFrameworkCore;

namespace SACS.Services.Data
{
    public class UserManagementService : IUserManagementService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public UserManagementService(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext dbContext)
        {
            this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return dbContext.Users.ToList();
        }

        public ApplicationUser GetUserById(string id)
        {
            return dbContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await userManager.Users
                .OrderBy(u => u.UserName)
                .ToListAsync();
        }

        public async Task<ApplicationUser> GetCurrentUserAsync(System.Security.Claims.ClaimsPrincipal user)
        {
            return await userManager.GetUserAsync(user);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password, string role)
        {
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, role);
            }

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
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("User ID is required");
            }

            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Username and email are required");
            }

            // Check for duplicate username
            var existingUser = await userManager.FindByNameAsync(userName);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                throw new InvalidOperationException("Username is already taken");
            }

            // Check for duplicate email
            existingUser = await userManager.FindByEmailAsync(email);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                throw new InvalidOperationException("Email is already in use");
            }

            // Update user properties
            user.UserName = userName;
            user.Email = email;
            user.NormalizedUserName = userName.ToUpper();
            user.NormalizedEmail = email.ToUpper();

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}
