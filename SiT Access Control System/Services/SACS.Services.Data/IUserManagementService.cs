using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SACS.Data.Models;

namespace SACS.Services.Data
{
    public interface IUserManagementService
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetCurrentUserAsync(System.Security.Claims.ClaimsPrincipal user);
        Task DeleteUserAsync(string id);
        Task UpdateUserAsync(string id, string userName, string email);
    }
}
