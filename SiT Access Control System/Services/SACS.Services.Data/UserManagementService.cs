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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserManagementService(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users
                .OrderBy(u => u.UserName)
                .ToListAsync();
        }

        public async Task<ApplicationUser> GetCurrentUserAsync(System.Security.Claims.ClaimsPrincipal user)
        {
            return await _userManager.GetUserAsync(user);
        }

        public async Task DeleteUserAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("User ID cannot be empty");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to delete user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        public async Task UpdateUserAsync(string id, string userName, string email)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("User ID is required");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Username and email are required");
            }

            // Check for duplicate username
            var existingUser = await _userManager.FindByNameAsync(userName);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                throw new InvalidOperationException("Username is already taken");
            }

            // Check for duplicate email
            existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                throw new InvalidOperationException("Email is already in use");
            }

            // Update user properties
            user.UserName = userName;
            user.Email = email;
            user.NormalizedUserName = userName.ToUpper();
            user.NormalizedEmail = email.ToUpper();

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to update user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}
