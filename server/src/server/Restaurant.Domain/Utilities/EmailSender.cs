using Baseline;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Restaurant.Domain.Utilities
{
    public class EmailSender
    {
        private readonly string _server;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly bool _isAuthenticationNeeded;

        public EmailSender(string server, int port, string username, string password, bool isAuthenticationNeeded)
        {
            _server = server;
            _port = port;
            _username = username;
            _password = password;
            _isAuthenticationNeeded = isAuthenticationNeeded;
        }

        public async Task SendAsync(string fromAddress, string toAddress, string subject, string message)
        {
            var mailMessage = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = message
            };

            using (var client = new SmtpClient(_server, _port))
            {
                if (_isAuthenticationNeeded)
                {
                    client.Credentials = new NetworkCredential(_username, _password);
                    client.EnableSsl = true;
                }

                await client.SendMailAsync(mailMessage);
            }

            mailMessage.SafeDispose();
        }
    }
}