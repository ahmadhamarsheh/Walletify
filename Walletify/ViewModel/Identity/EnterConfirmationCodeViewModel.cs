using System.ComponentModel.DataAnnotations;

namespace Walletify.ViewModel.Identity
{
    public class EnterConfirmationCodeViewModel
    {
        [Required(ErrorMessage = "Confirmation code is required.")]
        public string ConfirmationCode { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
