using BazaSmyczy.Core.Models;
using BazaSmyczy.Core.Models.Results;
using System.Threading.Tasks;

namespace BazaSmyczy.Core.Services
{
    public interface IAccountService
    {
        Task<LoginResult> LoginAsync(string username, string password, bool rememberMe);
        Task<RegistrationResult> RegisterAsync(string username, string password, string email);
        Task SendConfirmationEmailAsync(string recipient, string callbackUrl);
        Task<Result> ConfirmEmailAsync(string userId, string code);
        Task LogoutAsync();
        Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user);
    }
}
