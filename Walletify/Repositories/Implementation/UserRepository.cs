using Walletify.ApplicationDbContext;
using Walletify.Models.Entities;
using Walletify.Repositories.Interfaces;

namespace Walletify.Repositories.Implementation
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext appDBContext)
            : base(appDBContext)
        {
        }
    }
}
