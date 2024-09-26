using Walletify.ApplicationDbContext;
using Walletify.Models.Entities;

namespace DAL.Repositories.UserRepos
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(AppDbContext appDBContext)
            : base(appDBContext)
        {
        }
    }
}
