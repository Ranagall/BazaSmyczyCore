using System.Threading.Tasks;

namespace BazaSmyczy.Core.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string recipient, string subject, string htmlBody);
        Task SendAccountConfirmationEmailAsync(string recipient, string callbackUrl);
    }
}
