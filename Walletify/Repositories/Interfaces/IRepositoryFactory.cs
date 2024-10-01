using Walletify.Repositories.Implementation;

namespace Walletify.Repositories.Interfaces
{
    public interface IRepositoryFactory
    {
        IUserRepository User { get; }
        ICategoryRepository Category { get; }
        ITransactionRepository Transaction { get; }
        IAccountRepositry Account { get; }
        void Save();
    }
}
