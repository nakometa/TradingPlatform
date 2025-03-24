using Microsoft.EntityFrameworkCore;

namespace Order.Api.Data
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Models.Entities.Order> Orders { get; set; }
    }
}
