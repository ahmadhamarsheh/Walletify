using DAL.Categories.CategoryRepos;
using DAL.Repositories.CategoryRepos;
using DAL.Repositories.TransactionRepos;
using DAL.Repositories.UserRepos;
using Walletify.ApplicationDbContext;

namespace DAL.Repositories.RepositoryFactory
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly AppDbContext _appDBContext;
   

        private ITransactionRepository _transaction;
        private IUserRepository _user;
        private ICategoryRepository _category;
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
