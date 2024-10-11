using System.ComponentModel.DataAnnotations;
using Xunit.Sdk;

namespace Walletify.ViewModel.Accounts
{
    public class UpdateAccountViewModel
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "The Amount must be greater than 0.")]
        public decimal SavedAmountPerMonth { get; set; }
    
        [Range(0.01, double.MaxValue, ErrorMessage = "The Amount must be greater than 0.")]
        public decimal SavingTargetAmount { get; set; }
    }
}
