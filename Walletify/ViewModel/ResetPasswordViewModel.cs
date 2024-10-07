using System.ComponentModel.DataAnnotations;

namespace Walletify.ViewModel.Identity
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&*!?_\\-])[a-zA-Z\d@#$%^&*!?_\\]{8,}$",
            ErrorMessage = "The Password must be at least 8 characters long and contain one special character, one number, one uppercase, and one lowercase letter.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Token { get; set; }

    }
}
