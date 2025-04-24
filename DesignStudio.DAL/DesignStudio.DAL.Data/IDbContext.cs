using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace DesignStudio.DAL.Data
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DatabaseFacade Database { get; }
    }
}