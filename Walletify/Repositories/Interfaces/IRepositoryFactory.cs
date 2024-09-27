namespace Walletify.Repositories.Interfaces
{
    public interface IRepositoryFactory
    {
        IUserRepository User { get; }
        ICategoryRepository Category { get; }
        ITransactionRepository Transaction { get; }
        void Save();
    }
}
