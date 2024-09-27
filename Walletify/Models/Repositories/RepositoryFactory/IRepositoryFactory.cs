using DAL.Categories.CategoryRepos;
using DAL.Repositories.TransactionRepos;
using DAL.Repositories.UserRepos;

namespace DAL.Repositories.RepositoryFactory
{
    public interface IRepositoryFactory
    {
        IUserRepository User { get; }
        ICategoryRepository Category { get; }
        ITransactionRepository Transaction { get; }
        void Save();
    }
}
