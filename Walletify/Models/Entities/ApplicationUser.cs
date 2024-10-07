namespace Walletify.Models.Entities;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    // Navigation properties
    public Account Account { get; set; }  // One-to-one relationship
    public Saving Saving { get; set; }    // One-to-one relationship
    public ICollection<Transaction> Transactions { get; set; } // Many-to-one relationship

}
