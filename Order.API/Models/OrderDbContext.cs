using Microsoft.EntityFrameworkCore;

namespace Order.API.Models
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(builder =>
            {
                builder.ToTable("Orders");
                builder.Property(x => x.Id).HasMaxLength(36).IsRequired();
                builder.HasKey(x => x.Id);
                builder.Property(x => x.UserId).HasMaxLength(36).IsRequired();
                builder.Property(e => e.Status).IsRequired();
                builder.Property(e => e.FailMessage).IsRequired(false);
                builder.Property(e => e.CreatedDate).IsRequired();
                builder.OwnsOne(p => p.Address, addressBuilder =>
                {
                    addressBuilder.Property(a => a.Province).IsRequired();
                    addressBuilder.Property(a => a.City).IsRequired();
                    addressBuilder.Property(a => a.District).IsRequired();
                    addressBuilder.Property(a => a.Street).IsRequired();
                    addressBuilder.Property(a => a.Text).IsRequired();
                    addressBuilder.Property(a => a.Phone).IsRequired(false);
                    addressBuilder.Property(a => a.ZipCode).IsRequired(false);
                });
                // Define the relationship with OrderItems
                builder.HasMany(e => e.OrderItems)
                .WithOne()
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<OrderItem>(builder =>
            {
                builder.ToTable("OrderItems");
                builder.Property(x => x.Id).HasMaxLength(36).IsRequired();
                builder.HasKey(e => e.Id);
                builder.Property(x => x.OrderId).HasMaxLength(36).IsRequired();
                builder.Property(x => x.ProductId).HasMaxLength(36).IsRequired();
                builder.Property(e => e.ProductName).IsRequired();
                builder.Property(e => e.Price).IsRequired();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}