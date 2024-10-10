using Walletify.EmailService.EmailModel;

namespace Walletify.EmailService
{
    public class EmailMessageGenerator
    {
        public static EmailMessage GenerateEmailMessageForCodeVerification(string code)
        {
            var subject = "Verification Account";
            var body = $"{subject} code : {code} , copy it and send it";
            return new EmailMessage()
            {
                Subject = subject,
                Body = body
            };
        }

        public static EmailMessage GenerateEmailMessageForResetPasswordCode(string code)
        {
            var subject = "Reset Password";
            var body = $"{subject} code : {code} , copy it and send it";
            return new EmailMessage()
            {
                Subject = subject,
                Body = body
            };
        }

    }
}
