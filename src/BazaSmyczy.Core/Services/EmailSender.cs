using BazaSmyczy.Core.Configs;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;

namespace BazaSmyczy.Core.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailClientConfig _clientOptions;
        private readonly INotificationComposer _notificationComposer;

        public EmailSender(IOptionsSnapshot<EmailClientConfig> clientOptions, INotificationComposer notificationComposer)
        {
            _clientOptions = clientOptions.Value;
            _notificationComposer = notificationComposer;
        }

        public async Task SendAccountConfirmationEmailAsync(string recipient, string callbackUrl)
        {
            var subject = _notificationComposer.ComposeNotificationSubject(NotificationType.Confirmation);
            var body = _notificationComposer.ComposeNotificationBody(NotificationType.Confirmation, callbackUrl);
            await SendEmailAsync(recipient, subject, body);
        }

        public async Task SendEmailAsync(string recipient, string subject, string htmlBody)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_clientOptions.Name, _clientOptions.Address));
            message.To.Add(new MailboxAddress(recipient));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = htmlBody
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_clientOptions.Host, _clientOptions.Port, _clientOptions.UseSsl);

                client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.AuthenticateAsync(_clientOptions.Username, _clientOptions.Password);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
