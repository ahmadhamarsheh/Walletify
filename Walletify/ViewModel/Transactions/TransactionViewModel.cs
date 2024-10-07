using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walletify.ViewModel.Transactions
{
    public class TransactionViewModel
    {
        public decimal Amount { get; set; }

        public string? Note { get; set; }   

        public int CategoryId { get; set; }

    }
}
