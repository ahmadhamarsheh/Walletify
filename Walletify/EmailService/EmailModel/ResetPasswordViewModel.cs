using System.ComponentModel.DataAnnotations;

namespace Walletify.EmailService.EmailModel
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Code { get; set; }
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&*!?_\\-])[a-zA-Z\d@#$%^&*!?_\\]{8,}$",
            ErrorMessage = "The Password must be at least 8 characters long and contain one special character, one number, one uppercase, and one lowercase letter.")]
        public string NewPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmNewPassword { get; set; }
    }
}
