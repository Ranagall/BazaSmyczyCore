using BazaSmyczy.Core.Consts;
using BazaSmyczy.Core.Models;
using BazaSmyczy.Core.Models.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BazaSmyczy.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<AccountService> _logger;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailSender emailSender, ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task<LoginResult> LoginAsync(string username, string password, bool rememberMe)
        {
            var result = new LoginResult();

            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    result.Errors.Add("You must have a confirmed email to log in.");
                    return result;
                }
            }

            var sinInResult = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: true);
            if (sinInResult.Succeeded)
            {
                //if (await _userManager.IsInRoleAsync(user, Roles.Administrator))
                //{
                //    var ipAddress = HttpContext.Features.Get<Microsoft.AspNetCore.Http.Features.IHttpConnectionFeature>()?.RemoteIpAddress;
                //    _logger.LogInformation(EventsIds.Account.AdminLogged, $"User from {ipAddress} logged in to admin account( {user.UserName} )");
                //}
                await _userManager.ResetAccessFailedCountAsync(user);
                return result;
            }

            if (sinInResult.IsLockedOut)
            {
                _logger.LogWarning(EventsIds.Account.LockedOut, $"User {user.UserName} locked out.");
                result.IsLockedOut = true;
            }
            else
            {
                var attemptsLeft = IdentityConsts.MaxFailedAccessAttempts - await _userManager.GetAccessFailedCountAsync(user);
                result.Errors.Add($"Invalid login attempt. Attempts to lock: {attemptsLeft}");
            }

            return result;
        }

        public async Task<RegistrationResult> RegisterAsync(string username, string password, string email)
        {
            var result = new RegistrationResult();
            var user = new ApplicationUser { UserName = username, Email = email };
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                result.Errors.Add("Email is already used.");
                return result;
            }
            var createResult = await _userManager.CreateAsync(user, password);
            if (createResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Member);
                result.User = user;
                _logger.LogInformation(EventsIds.Account.Created, "User created a new account with password.");
            }

            return result;
        }

        public async Task SendConfirmationEmailAsync(string recipient, string callbackUrl)
        {
            await _emailSender.SendAccountConfirmationEmailAsync(recipient, callbackUrl);
        }

        public async Task<Result> ConfirmEmailAsync(string userId, string code)
        {
            var result = new Result();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                result.Errors.Add("Error");
                return result;
            }

            var confirmationResult = await _userManager.ConfirmEmailAsync(user, code);
            if (!confirmationResult.Succeeded)
            {
                result.Errors.Add("Error");
            }

            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            return (await _userManager.GenerateEmailConfirmationTokenAsync(user));
        }
    }
}
