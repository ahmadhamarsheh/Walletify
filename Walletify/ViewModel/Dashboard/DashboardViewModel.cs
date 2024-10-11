using Walletify.ViewModel.Transactions;

namespace Walletify.ViewModel.Dashboard
{
    public class DashboardViewModel
    {
        public decimal TotalSavedAmount { get; set; }
        public decimal Balance { get; set; }
        public decimal SavedAmountPerMonth { get; set; }
        public int Ratio { get; set; }
        public IEnumerable<DashboardTransactionViewModel> TopTransactions { get; set; }

    }
}
