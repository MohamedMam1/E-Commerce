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
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<OrderItemVariant> OrderItemVariants { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure CartItem with single column primary key to avoid index size issues
            modelBuilder.Entity<CartItem>()
                .HasKey(c => c.Id);

            // Add unique constraint on UserId + ProductVariantId
            modelBuilder.Entity<CartItem>()
                .HasIndex(c => new { c.UserId, c.ProductVariantId })
                .IsUnique();

            // Configure CartItem -> User with cascade delete
            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.User)
                .WithMany(u => u.Carts)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure CartItem -> ProductVariant with cascade delete
            modelBuilder.Entity<CartItem>()
                .HasOne(c => c.ProductVariant)
                .WithMany(pv => pv.CartItems)
                .HasForeignKey(c => c.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);
            
            // Configure Wishlist composite key
            modelBuilder.Entity<Wishlist>()
                .HasKey(w => new { w.UserId, w.ProductId });

            // Configure OrderItem -> Product with NO ACTION to avoid cascade conflicts
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(oi => oi.ProductId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure Order -> User with NO ACTION to avoid cascade conflicts
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure OrderItemVariant -> ProductVariant with NO ACTION
            modelBuilder.Entity<OrderItemVariant>()
                .HasOne(oiv => oiv.ProductVariant)
                .WithMany(pv => pv.OrderItemVariants)
                .HasForeignKey(oiv => oiv.ProductVariantId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
