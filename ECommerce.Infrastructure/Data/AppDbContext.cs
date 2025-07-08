using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ECommerce.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// Optional: Configure relationships if needed explicitly
            //modelBuilder.Entity<OrderItem>()
            //    .HasOne(oi => oi.Order)
            //    .WithMany(o => o.OrderItems)
            //    .HasForeignKey(oi => oi.OrderId);

            //modelBuilder.Entity<OrderItem>()
            //    .HasOne(oi => oi.Product)
            //    .WithMany(p => p.OrderItems)
            //    .HasForeignKey(oi => oi.ProductId);
        }
    }
}
