using BazaSmyczy.Core.Models;
using BazaSmyczy.Core.Models.Results;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BazaSmyczy.Core.Services
{
    public interface IManageService
    {
        Task<Result> ChangePasswordAsync(ClaimsPrincipal claimsPrincipal, string oldPassword, string newPassword);
        Task<Result> SetPasswordAsync(ClaimsPrincipal claimsPrincipal, string newPassword);
        Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal user);
        Task<bool> HasPasswordAsync(ApplicationUser user);
    }
}
