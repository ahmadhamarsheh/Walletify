namespace Walletify.EmailService.EmailModel
{
    public class ResetPasswordViewModel
    {
        public string Code { get; set; }
        public string Email { get; set; }

        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
    }
}
