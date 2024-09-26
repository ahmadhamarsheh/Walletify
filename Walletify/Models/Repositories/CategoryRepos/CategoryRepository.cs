using DAL.Categories.CategoryRepos;
using Walletify.ApplicationDbContext;
using Walletify.Models.Entities;

namespace DAL.Repositories.CategoryRepos
{
    public class CategoryRepository : RepositoryBase<Category> , ICategoryRepository
    {
        public CategoryRepository(AppDbContext dbContext) : base(dbContext) { }
    }
}
