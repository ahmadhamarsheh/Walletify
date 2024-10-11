using Walletify.EmailService.EmailModel;

namespace Walletify.EmailService
{
    public interface IEmailSender
    {
        public void SendEmailWithCode(string email, EmailMessage emailMessage);


    }
}
