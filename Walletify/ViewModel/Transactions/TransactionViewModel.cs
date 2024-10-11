using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walletify.ViewModel.Transactions
{
    public class TransactionViewModel
    {
        [Required(ErrorMessage = "The amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "The balance must be greater than 0.")]     
        public decimal Amount { get; set; }

        public string? Note { get; set; }   

        public int CategoryId { get; set; }

    }
}
