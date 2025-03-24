using Microsoft.EntityFrameworkCore;
using Portfolio.Api.Models.Entities;

namespace Portfolio.Api.Data
{
    public class PortfolioDbContext : DbContext
    {
        public PortfolioDbContext(DbContextOptions<PortfolioDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserStock> UserStocks { get; set; }
        public DbSet<StockCatalog> StockCatalogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<User>()
                .HasMany(u => u.UserStocks)
                .WithOne()
                .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<UserStock>()
                .HasKey(us => us.Id);

            modelBuilder.Entity<UserStock>()
                .HasOne(us => us.StockCatalog)
                .WithMany()
                .HasForeignKey(us => us.CatalogId);

            modelBuilder.Entity<StockCatalog>()
                .HasKey(sc => sc.Id);

            modelBuilder.Entity<StockCatalog>()
                .HasIndex(sc => sc.Ticker)
                .IsUnique();

            PortfolioDbContextSeed.Seed(modelBuilder);
        }

    }

    public static class PortfolioDbContextSeed
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StockCatalog>().HasData(
                new StockCatalog { Id = 1, Ticker = "AAPL" },
                new StockCatalog { Id = 2, Ticker = "TSLA" },
                new StockCatalog { Id = 3, Ticker = "NVDA" },
                new StockCatalog { Id = 4, Ticker = "AMZN" },
                new StockCatalog { Id = 5, Ticker = "GOOGL" }
            );
        }
    }
}
