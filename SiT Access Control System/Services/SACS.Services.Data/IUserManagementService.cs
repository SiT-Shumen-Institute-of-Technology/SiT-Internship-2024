using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SACS.Data.Models;

namespace SACS.Services.Data
{
    public interface IUserManagementService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        List<ApplicationUser> GetAllUsers();

        ApplicationUser GetUserById(string id);

        Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal user);

        Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password, string role);

        Task UpdateUserAsync(string id, string userName, string email);

        Task DeleteUserAsync(string id);
    }
}
