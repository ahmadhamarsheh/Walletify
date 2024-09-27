using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Walletify.ApplicationDbContext;
using Walletify.Repositories.Interfaces;

namespace Walletify.Repositories.Implementation
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected AppDbContext _appDBContext { get; set; }
        public RepositoryBase(AppDbContext appDBContext)
        {
            _appDBContext = appDBContext;
        }
        //AsNoTracking()
        //ensures the results are not tracked
        //, improving performance if you only need to read the data.

        public IQueryable<T> FindAll() => _appDBContext.Set<T>().AsNoTracking();
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) =>
            _appDBContext.Set<T>().Where(expression).AsNoTracking();
        public void Create(T entity) => _appDBContext.Set<T>().Add(entity);
        public void Update(T entity) => _appDBContext.Set<T>().Update(entity);
        public void Delete(T entity) => _appDBContext.Set<T>().Remove(entity);
    }
}
