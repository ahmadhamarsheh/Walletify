using MailKit.Net.Smtp;
using MimeKit;
using Walletify.EmailService.EmailModel;
using Microsoft.Extensions.Configuration;

namespace Walletify.EmailService
{
    public class EmailSender:IEmailSender
    {
        private readonly IConfiguration _configuration;

        // Inject IConfiguration through constructor
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Remove static modifier to use instance variable _configuration
        public void SendEmailWithCode(string email, EmailMessage emailMessage)
        {
            // Fetch SMTP settings from appsettings.json using _configuration
            var host = _configuration["Smtp:Host"];
            var userName = _configuration["Smtp:UserName"];
            var fromEmail = _configuration["Smtp:FromEmail"];
            var password = _configuration["Smtp:Password"];
            var port = Convert.ToInt16(_configuration["Smtp:Port"]);

            // Prepare the email message
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(userName, fromEmail));
            message.To.Add(new MailboxAddress("user", email));
            message.Subject = emailMessage.Subject;
            message.Body = new TextPart("plain")
            {
                Text = emailMessage.Body
            };

            // Send the email using MailKit
            using (var client = new SmtpClient())
            {
                client.Connect(host, port, false); // Use false for non-SSL connections
                client.Authenticate(fromEmail, password);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
