using Walletify.ApplicationDbContext;
using Walletify.Models.Entities;
using Walletify.Repositories.Interfaces;

namespace Walletify.Repositories.Implementation
{
    public class AccountRepositry : RepositoryBase<Account>, IAccountRepositry
    {
        public AccountRepositry(AppDbContext dbContext) : base(dbContext) { }

    }
}
