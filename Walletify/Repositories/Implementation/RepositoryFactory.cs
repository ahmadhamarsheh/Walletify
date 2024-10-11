using Walletify.ApplicationDbContext;
using Walletify.Models.Entities;
using Walletify.Repositories.Interfaces;

namespace Walletify.Repositories.Implementation
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly AppDbContext _appDBContext;


        private ITransactionRepository _transaction;
        private IUserRepository _user;
        private ICategoryRepository _category;
        private IAccountRepositry _account;
        private ISavingRepositry _saving;
        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_appDBContext);
                }
                return _user;
            }
        }
        public ITransactionRepository Transaction
        {
            get
            {
                if (_transaction == null)
                {
                    _transaction = new TransactionRepository(_appDBContext);
                }
                return _transaction;
            }
        }
        public ICategoryRepository Category
        {
            get
            {
                if (_category == null)
                {
                    _category = new CategoryRepository(_appDBContext);
                }
                return _category;
            }
        }

        public IAccountRepositry Account
        {
            get
            {
                if (_account == null)
                {
                    _account = new AccountRepositry(_appDBContext);
                }
                return _account;
            }
        }
        public ISavingRepositry Saving
        {
            get
            {
                if (_saving == null)
                {
                    _saving = new SavingRepositry(_appDBContext);
                }
                return _saving;
            }
        }

        public RepositoryFactory(AppDbContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        public void Save()
        {
            _appDBContext.SaveChanges();
        }
    }
}
