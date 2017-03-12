using BazaSmyczy.Core.Consts;
using BazaSmyczy.Core.Models;
using BazaSmyczy.Core.Models.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BazaSmyczy.Core.Services
{
    public class ManageService : IManageService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<ManageService> _logger;

        public ManageService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<ManageService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<Result> ChangePasswordAsync(ClaimsPrincipal claimsPrincipal, string oldPassword, string newPassword)
        {
            var result = new Result();
            var user = await GetCurrentUserAsync(claimsPrincipal);
            if (user != null)
            {
                var changePassResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                if (changePassResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(EventsIds.Account.ChangedPassword, "User changed their password successfully.");
                    return result;
                }

                foreach (var error in changePassResult.Errors)
                {
                    result.Errors.Add(error.Description);
                }
            }
            else
            {
                result.Errors.Add("Error");
            }

            return result;
        }

        public async Task<Result> SetPasswordAsync(ClaimsPrincipal claimsPrincipal, string newPassword)
        {
            var result = new Result();
            var user = await GetCurrentUserAsync(claimsPrincipal);
            if (user != null)
            {
                var addPassResult = await _userManager.AddPasswordAsync(user, newPassword);
                if (addPassResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(EventsIds.Account.SetPassword, "User set their password successfully.");
                    return result;
                }

                foreach (var error in addPassResult.Errors)
                {
                    result.Errors.Add(error.Description);
                };
            }
            else
            {
                result.Errors.Add("Error");
            }

            return result;
        }

        public Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal user)
        {
            return _userManager.GetUserAsync(user);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return _userManager.HasPasswordAsync(user);
        }
    }
}
