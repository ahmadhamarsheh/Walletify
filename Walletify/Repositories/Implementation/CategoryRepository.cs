using Walletify.ApplicationDbContext;
using Walletify.Models.Entities;
using Walletify.Repositories.Interfaces;

namespace Walletify.Repositories.Implementation
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
