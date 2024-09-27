﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Walletify.Models.Entities
{
    public class Saving
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public decimal SavingTargetAmount { get; set; }
        [Required]
        public decimal TotalSavedAmount { get; set; }


        //Navigation Properties
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
