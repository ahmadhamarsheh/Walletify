using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Walletify.ApplicationDbContext;

namespace DAL.Repositories.TransactionRepos
{
    public class TransactionRepository : RepositoryBase<Transaction> , ITransactionRepository
    {
        public TransactionRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
