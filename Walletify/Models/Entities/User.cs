using System.ComponentModel.DataAnnotations;

namespace Walletify.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9._]{4,20}$")]
        public string UserName { get; set; }
        [RegularExpression(@"^[a-zA-Z]$")]
        public string FirstName { get; set; }
        [RegularExpression(@"^[a-zA-Z]$")]
        public string LastName { get; set; }
        [Required]
        [RegularExpression(@"^[^@\s]+@+[^@\s]+\.+[^@\s]+$", ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%^&*!?_\\-])[a-zA-Z\d@#$%^&*!?_\\]{8,}$", 
            ErrorMessage = "The Password must be at least 8 characters long and, " +
            "contains one special character, one number, one uppercase, and one lower case")]
        public string Password { get; set; }
        public decimal Balance { get; set; }
        public decimal SavedAmountPerMonth { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

    }
}
