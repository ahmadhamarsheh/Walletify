using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walletify.Models.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        public decimal Balance { get; set; }
        [Required]
        public decimal SavedAmountPerMonth { get; set; }
        //Navigation Properties
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
