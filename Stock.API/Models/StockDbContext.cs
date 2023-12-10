using Microsoft.EntityFrameworkCore;

namespace Stock.API.Models
{
    public class StockDbContext : DbContext
    {
        public StockDbContext(DbContextOptions<StockDbContext> options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>(builder =>
            {
                builder.Property(x => x.Id).HasMaxLength(36).IsRequired();
                builder.HasKey(x => x.Id);
                builder.Property(x => x.ProductId).HasMaxLength(36).IsRequired();
                builder.Property(x => x.Count);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}