using System.ComponentModel.DataAnnotations;

namespace Walletify.ViewModel.Identity
{
    public class RegisterViewModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9._]{4,20}$", ErrorMessage = "Username must be between 5 and 20 characters.")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid Email")]
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

        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
