using System.Transactions;
using Walletify.ApplicationDbContext;
using Walletify.Repositories.Interfaces;

namespace Walletify.Repositories.Implementation
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
