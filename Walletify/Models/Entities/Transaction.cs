using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Walletify.Models.Entities
{
    public enum TransationType
    {
        Spending, Income
    }
    public class Transaction
    {
        public int Id { get; set; }

        public string TransationId = Guid.NewGuid().ToString();
        [Required]
        public string UserId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public decimal Amount { get; set; }
        [Required]
        public TransationType TransationType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string? Note { get; set; }
        //Navigation Properties
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }


    }
}
