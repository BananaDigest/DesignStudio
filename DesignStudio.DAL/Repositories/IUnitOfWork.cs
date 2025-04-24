using DesignStudio.DAL.Models;

namespace DesignStudio.DAL.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<DesignService> Services { get; }
        IGenericRepository<Order> Orders { get; }
        IGenericRepository<PortfolioItem> Portfolio { get; }

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> CommitAsync();
    }
}