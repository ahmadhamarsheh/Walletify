using System.ComponentModel.DataAnnotations;
namespace Walletify.ViewModel.Identity;

public class LoginViewModel
{
    [Required(ErrorMessage ="Please enter your email")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Please enter your password")]
    public string Password { get; set; }
    [Display(Name ="Remember Me")]
    public bool RememberMe { get; set; } 
}
