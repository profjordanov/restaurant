using Microsoft.Extensions.Options;
using Restaurant.Domain.Connectors;
using Restaurant.Domain.Settings;
using Restaurant.Domain.Utilities;
using System.IO;
using System.Threading.Tasks;

namespace Restaurant.Persistence.Connectors
{
    public class NotificationManager : INotificationManager
    {
        private readonly MailSettings _mailSettings;

        public NotificationManager(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendNotificationAsync(string email, string subject, string message)
        {
            var mailSender = _mailSettings.MailServerSecurityEnabled ?
                new EmailSender(
                    _mailSettings.MailServerAddress,
                    _mailSettings.MailServerPort,
                    _mailSettings.MailServerUserName,
                    _mailSettings.MailServerPassword,
                    true) :
                new EmailSender(
                    _mailSettings.MailServerAddress,
                    _mailSettings.MailServerPort,
                    null,
                    null,
                    false);

            await mailSender.SendAsync(_mailSettings.MailSender, email, subject, message);
        }

        public async Task SendNotificationAsync(string receiver, string subject, string templateFileName, string[] templateParams)
        {
            string messageText;

            using (var reader = new FileStream(Path.Combine(_mailSettings.MailTemplateDirectoryPath, templateFileName), FileMode.Open))
            {
                using (var stream = new StreamReader(reader))
                {
                    messageText = await stream.ReadToEndAsync();
                }
            }

            var formattedMessage = string.Format(messageText, templateParams);

            await SendNotificationAsync(receiver, subject, formattedMessage);
        }
    }
}