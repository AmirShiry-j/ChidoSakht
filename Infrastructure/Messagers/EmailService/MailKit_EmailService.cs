using Application.Messagers.EmailService;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Smtp;
using MimeKit;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Application.ConfigService;

namespace Infrastructure.Messagers.EmailService
{
    public class MailKit_EmailService : IEmailService
    {
        readonly IConfigService _configService;
        readonly ILogger<MailKit_EmailService> _logger;
        public MailKit_EmailService(ILogger<MailKit_EmailService> logger, IConfigService configService)
        {
            _logger = logger;
            _configService = configService;
        }
        public async Task<bool> SendEmailAsync(string To, string Subject, string Body)
        {
            try
            {
                //Take Email configs
                var configEmail = _configService.Config.EmailSetting;

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Your Name", configEmail.Email));
                message.To.Add(new MailboxAddress("", To));
                message.Subject = Subject;
                message.Body = new TextPart("html") { Text = Body };
                if (!Enum.TryParse(configEmail.SecureSocketOptions, out MailKit.Security.SecureSocketOptions SecureSocketEnum))
                    SecureSocketEnum = MailKit.Security.SecureSocketOptions.StartTls;

                using (var client = new SmtpClient())
                {
                    await client.ConnectAsync(configEmail.Host, configEmail.Port, SecureSocketEnum);
                    await client.AuthenticateAsync(configEmail.Email, configEmail.Password);
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }

                return true;
            }
            catch (Exception error)
            {
                //Log error
                _logger.LogError(error.ToString());

                return false;
            }
        }
    }
}
