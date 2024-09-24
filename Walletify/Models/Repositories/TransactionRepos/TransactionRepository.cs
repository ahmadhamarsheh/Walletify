using System.Transactions;
using Walletify.ApplicationDbContext;

namespace DAL.Repositories.TransactionRepos
{
    public class TransactionRepository : RepositoryBase<Transaction> , ITransactionRepository
    {
        public TransactionRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
