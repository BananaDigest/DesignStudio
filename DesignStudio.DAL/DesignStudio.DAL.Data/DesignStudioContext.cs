using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using DesignStudio.DAL.Models;

namespace DesignStudio.DAL.Data
{
    public class DesignStudioContext : DbContext, IDbContext
    {
        public DbSet<DesignService> DesignServices { get; set; } = null!;
        public DbSet<Order> Orders { get; set; }
        public DbSet<PortfolioItem> PortfolioItems { get; set; } = null!;


        public DesignStudioContext(DbContextOptions<DesignStudioContext> options)
            : base(options)
        {
        }

        // Виставляєме many-to-many між Order та DesignService
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>()
                .HasMany(o => o.DesignServices)
                .WithMany(ds => ds.Orders)
                .UsingEntity<Dictionary<string, object>>(                 
                    "OrderDesignService",
                    j => j
                        .HasOne<DesignService>()
                        .WithMany()
                        .HasForeignKey("DesignServiceId")
                        .HasConstraintName("FK_OrderDesignService_DesignServices_DesignServiceId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Order>()
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .HasConstraintName("FK_OrderDesignService_Orders_OrderId")
                        .OnDelete(DeleteBehavior.Cascade));
        }

        // Реалізація IDbContext
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class => base.Set<TEntity>();
        public new DatabaseFacade Database => base.Database;
    }
}
