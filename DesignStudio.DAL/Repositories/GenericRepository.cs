using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using DesignStudio.DAL.Data;

namespace DesignStudio.DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : class
    {
        private readonly IDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(IDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(int id) =>
            await _dbSet.FindAsync(id);

        public async Task<IEnumerable<TEntity>> GetAllAsync() =>
            await _dbSet.ToListAsync();

        public async Task<IEnumerable<TEntity>> GetWhereAsync(
            Expression<Func<TEntity, bool>> predicate) =>
            await _dbSet.Where(predicate).ToListAsync();

        public async Task<IEnumerable<TEntity>> GetWithIncludeAsync(
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;
            foreach (var include in includes)
                query = query.Include(include);
            return await query.ToListAsync();
        }

        public async Task AddAsync(TEntity entity) =>
            await _dbSet.AddAsync(entity);

        public void Update(TEntity entity) =>
            _dbSet.Update(entity);

        public void Remove(TEntity entity) =>
            _dbSet.Remove(entity);
    }
}
