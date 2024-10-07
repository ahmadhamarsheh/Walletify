using Walletify.ApplicationDbContext;
using Walletify.Repositories.Interfaces;
using Walletify.Models.Entities;

namespace Walletify.Repositories.Implementation
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
