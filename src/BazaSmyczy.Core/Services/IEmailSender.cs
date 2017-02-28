﻿using System.Threading.Tasks;

namespace BazaSmyczy.Core.Services
{
    public interface IEmailSender
    {
        Task SendEmail(string recipient, string subject, string htmlBody);
        Task SendAccountConfirmationEmail(string recipient, string callbackUrl);
    }
}
