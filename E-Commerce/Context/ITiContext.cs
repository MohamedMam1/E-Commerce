using E_Commerce.Models;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Permissions;

namespace FinalProject.Context
{
    public class ITiContext : IdentityDbContext<ApplicationUser>
    {
        public ITiContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite primary keys
            modelBuilder.Entity<Cart>().HasKey(c => new { c.UserId, c.ProductId });
            modelBuilder.Entity<Wishlist>().HasKey(w => new { w.UserId, w.ProductId });

            // Additional configurations can be added here if needed
        }
    }
}
