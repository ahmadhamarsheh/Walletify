using Walletify.ApplicationDbContext;
using Walletify.Models.Entities;
using Walletify.Repositories.Interfaces;

namespace Walletify.Repositories.Implementation
{
    public class SavingRepositry : RepositoryBase<Saving>, ISavingRepositry
    {
        public SavingRepositry(AppDbContext appDBContext) : base(appDBContext)
        {
        }
    }
}
