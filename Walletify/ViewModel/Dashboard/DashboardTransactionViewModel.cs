namespace Walletify.ViewModel.Dashboard
{
    public class DashboardTransactionViewModel
    {
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string? Note { get; set; }
        public string TransationType { get; set; }
        public string CategoryName { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}