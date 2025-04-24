using System.Threading.Tasks;
using DesignStudio.DAL.Models;
using DesignStudio.DAL.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace DesignStudio.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _context;
        private IDbContextTransaction? _transaction;

        public IGenericRepository<DesignService> Services { get; }
        public IGenericRepository<Order> Orders { get; }
        public IGenericRepository<PortfolioItem> Portfolio { get; }

        public UnitOfWork(IDbContext context)
        {
            _context = context;
            Services  = new GenericRepository<DesignService>(context);
            Orders    = new GenericRepository<Order>(context);
            Portfolio = new GenericRepository<PortfolioItem>(context);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
                await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
                await _transaction.RollbackAsync();
        }

        public async Task<int> CommitAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _transaction?.Dispose();
    }
}