using Application.Messagers.EmailService;
using System.Net;
using System.Text;
using Microsoft.Extensions.Logging;
using Application.ConfigService;
using System.Net.Mail;

namespace Infrastructure.Messagers.EmailService
{
    public class SmtpClient_EmailService : IEmailService
    {
        readonly IConfigService _configService;
        readonly ILogger<MailKit_EmailService> _logger;
        public SmtpClient_EmailService(ILogger<MailKit_EmailService> logger, IConfigService configService)
        {
            _logger = logger;
            _configService = configService;
        }
        public async Task<bool> SendEmailAsync(string To, string Subject, string Body)
        {
            //enable less secure apps in account google with link
            //https://myaccount.google.com/lesssecureapps
            //https://mail.google.com/mail/u/0/?tab=km#inbox

            try
            {
                //Take Email configs
                var configEmail = _configService.Config.EmailSetting;

                SmtpClient client = new SmtpClient();
                client.Port = configEmail.Port;
                client.Host = configEmail.Host;
                client.EnableSsl = configEmail.EnableSsl;
                client.Timeout = configEmail.Timeout;
                client.UseDefaultCredentials = configEmail.UseDefaultCredentials;

                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential(configEmail.Email, configEmail.Password);

                MailMessage message = new MailMessage(configEmail.Email, To, Subject, Body);
                message.IsBodyHtml = true;
                message.BodyEncoding = UTF8Encoding.UTF8;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

                client.Send(message);

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
