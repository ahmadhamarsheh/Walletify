using System.ComponentModel.DataAnnotations.Schema;

namespace Walletify.Models.Entities
{
    public enum TransationType {
        DEPOSIT, WITHDRAWAL
    }
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public TransationType TransationType { get; set; }
        public DateTime TransactionDate { get; set; }   
        public string Note { get; set; }
        //Navigation Properties
        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

    }
}
