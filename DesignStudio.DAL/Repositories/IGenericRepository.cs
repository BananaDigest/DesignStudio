using System.Linq.Expressions;

namespace DesignStudio.DAL.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetWhereAsync(Expression<Func<TEntity, bool>> predicate);
        
        // Новий метод для Include
        Task<IEnumerable<TEntity>> GetWithIncludeAsync(
            params Expression<Func<TEntity, object>>[] includes);

        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
