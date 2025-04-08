using Microsoft.EntityFrameworkCore;
using DesignStudio.DAL.Models;

namespace DesignStudio.DAL.Data
{
    // Контекст бази даних для застосунку
    public class DesignStudioContext : DbContext
    {
        public DbSet<DesignService> DesignServices { get; set; }
        public DbSet<PortfolioItem> PortfolioItems { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DesignStudioContext(DbContextOptions<DesignStudioContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DesignService>()
                .HasMany(ds => ds.Orders)
                .WithMany(o => o.DesignServices)
                .UsingEntity(j => j.ToTable("OrderDesignServices"));
        }
    }
}
