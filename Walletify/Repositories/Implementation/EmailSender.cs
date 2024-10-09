using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;
using Walletify.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using Walletify.Models.Entities;

namespace Walletify.Repositories.Implementation
{
    public class EmailSender : IEmailSender
    {
        //private readonly SmtpSettings _smtpSettings;

        //public EmailSender(IOptions<SmtpSettings> smtpSettings)
        //{
        //    _smtpSettings = smtpSettings.Value;
        //}

        //public async Task SendEmailAsync(string email, string subject, string message)
        //{
        //    using var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
        //    {
        //        Credentials = new NetworkCredential(_smtpSettings.UserName, _smtpSettings.Password),
        //        EnableSsl = _smtpSettings.EnableSsl,
        //    };

        //    var mailMessage = new MailMessage
        //    {
        //        From = new MailAddress("hello@demomailtrap.com"), 
        //        Subject = subject,
        //        Body = message,
        //        IsBodyHtml = true,
        //    };
        //    mailMessage.To.Add(email);

        //    await smtpClient.SendMailAsync(mailMessage);
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var fromMail = "walletify2024@outlook.com";
            var fromPassword = "W@7am25$Y2024";

            var message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(email);
            message.Body = $"<html><body>{htmlMessage}</body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp-mail.outlook.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true
            };

            smtpClient.Send(message);
        }
    }
}
