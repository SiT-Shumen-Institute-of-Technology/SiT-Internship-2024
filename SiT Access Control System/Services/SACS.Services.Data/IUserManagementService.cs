using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SACS.Data.Models;

namespace SACS.Services.Data;

public interface IUserManagementService
{
    Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();

    Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal user);

    Task DeleteUserAsync(string id);

    Task UpdateUserAsync(string id, string userName, string email);

    List<ApplicationUser> GetAllUsers();

    Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password, string role);

    ApplicationUser GetUserById(string id);
}