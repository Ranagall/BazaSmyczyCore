using BazaSmyczy.Core.Config;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading.Tasks;
using System;

namespace BazaSmyczy.Core.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailClientConfig _clientOptions;
        private readonly INotificationComposer _notificationComposer;

        public EmailSender(IOptions<EmailClientConfig> clientOptions, INotificationComposer notificationComposer)
        {
            _clientOptions = clientOptions.Value;
            _notificationComposer = notificationComposer;
        }

        public async Task SendAccountConfirmationEmail(string recipient, string callbackUrl)
        {
            var subject = _notificationComposer.ComposeNotificationSubject(NotificationType.Confirmation);
            var body = _notificationComposer.ComposeNotificationBody(NotificationType.Confirmation, callbackUrl);
            await SendEmail(recipient, subject, body);
        }

        public async Task SendEmail(string recipient, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("BazaSmyczy Notifications", "noreply@bazasmyczy.ovh"));
            message.To.Add(new MailboxAddress(recipient));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_clientOptions.Host, _clientOptions.Port, true);

                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Authenticate(_clientOptions.Username, _clientOptions.Password);

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
