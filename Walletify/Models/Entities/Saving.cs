using System.ComponentModel.DataAnnotations.Schema;

namespace Walletify.Models.Entities
{
    public class Saving
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public decimal SavingTargetAmount { get; set; }
        public decimal TotalSavedAmount { get; set; }


        //Navigation Properties
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
