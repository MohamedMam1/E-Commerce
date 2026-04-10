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

            modelBuilder.Entity<Category>().HasData(
                   new Category { Id = 1, Name = "Men" , Description = "Elevate your wardrobe with our premium men's collection, featuring everything from rugged denim to sleek athletic wear." },
                   new Category { Id = 2, Name = "Women" , Description = "Discover the latest trends in women's fashion, from elegant evening wear to comfortable daily essentials." },
                   new Category { Id = 3, Name = "Bag", Description = "Functional meets fashionable. Explore our range of durable backpacks, stylish totes, and professional briefcases." },
                   new Category { Id = 4, Name = "Shoes", Description = "Step out in style with our curated selection of footwear, ranging from high-performance sneakers to classic leather boots." },
                   new Category { Id = 5, Name = "Watches", Description = "Timeless pieces for the modern individual. Precision-engineered watches that make a statement on any wrist." }
            );
            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Name = "Washed Black Long Sleeves Regular Denim Jacket",
                    ImageUrl = "Product1_Main.avif",
                    Description = "This denim jacket features a washed black finish, long sleeves, and a regular fit. It is made from high-quality denim fabric, providing durability and comfort. The jacket includes classic details such as button closures, chest pockets, and side pockets. It is a versatile piece that can be styled in various ways for a trendy and casual look.",
                    Price = 800,
                    IsAvailable = true,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 2,
                    Name = "sportswear Men's Black 1/4 zip Long sleeve",
                    ImageUrl = "Product2_Main.avif",
                    Description = "This long sleeve features a 1/4 zip design, allowing for easy ventilation and a customizable fit. It is made from high-quality sportswear fabric that offers breathability and moisture-wicking properties, keeping you comfortable during physical activities. The sleek black color adds a stylish touch to your athletic wardrobe, making it suitable for both workouts and casual wear.",
                    Price = 1100,
                    IsAvailable = true,
                    CategoryId = 1
                },
                new Product
                {
                    Id = 3,
                    Name = "Sportswear - Sport Top Long Sleeves",
                    Price = 400,
                    Description = "This sportswear top features long sleeves and is designed for active individuals. It is made from high-quality, moisture-wicking fabric that helps keep you dry and comfortable during workouts. The top has a sleek design with a comfortable fit, making it suitable for various sports and fitness activities. Whether you're hitting the gym or going for a run, this long sleeve sport top is a great choice for performance and style.",
                    ImageUrl = "Product3_Main.avif",
                    IsAvailable = true,
                    CategoryId = 2
                },
                new Product
                {
                    Id = 4,
                    Name = "VERO MODA Womens Tessa Wide Denim Jeans",
                    Description = "These wide denim jeans from VERO MODA are designed for women who want to make a fashion statement. The jeans feature a wide-leg silhouette that offers a comfortable and relaxed fit. Made from high-quality denim fabric, they provide durability and style. The jeans have a classic five-pocket design and a button and zip closure. They can be dressed up or down, making them a versatile addition to any wardrobe.",
                    Price = 900,
                    ImageUrl = "Product4_Main.avif",
                    IsAvailable = true,
                    CategoryId = 2

                },
                new Product
                {
                    Id = 5,
                    Name = "Elegant shoulder bag with a sleek and modern design",
                    Description = "This elegant shoulder bag features a sleek and modern design, perfect for adding a touch of sophistication to any outfit. Crafted from high-quality materials, it offers durability and style. The bag includes a spacious main compartment with a secure closure, as well as additional pockets for organizing your essentials. The adjustable shoulder strap allows for comfortable wear, making it an ideal accessory for both casual and formal occasions.",
                    Price = 300,
                    ImageUrl = "Product5_Main.avif",
                    IsAvailable = true,
                    CategoryId = 3
                },
                new Product
                {
                    Id = 6,
                    Name = "Men Textile Sports Sneakers",
                    Description = "These men's textile sports sneakers are designed for both comfort and performance. Made from breathable textile material, they provide excellent ventilation to keep your feet cool during physical activities. The sneakers feature a cushioned sole that offers support and shock absorption, making them ideal for running, training, or casual wear. With a stylish design and durable construction, these sneakers are a great addition to any athletic wardrobe.",
                    Price = 500,
                    ImageUrl = "Product6_Main.avif",
                    IsAvailable = true,
                    CategoryId = 4
                },
                new Product
                {
                    Id = 7,
                    Name = "Men's Mason Round Shape Stainless Steel Analog Wrist Watch 45 mm - Silver - 1791788",
                    Description= "This men 's Mason round shape wrist watch features a stainless steel case and a sleek silver finish. The analog display offers a classic look, while the 45 mm case size provides a bold and stylish presence on the wrist. The watch is designed for durability and precision, making it suitable for everyday wear or special occasions.",
                    Price = 4000,
                    ImageUrl = "Product7_Main.avif",
                    IsAvailable = true,
                    CategoryId = 5
                }



            );


            modelBuilder.Entity<ProductImage>().HasData(
                new ProductImage
                {
                    Id = 1,
                    ImageUrl = "Product1_Add1.avif",
                    ProductId = 1,
                },
                new ProductImage
                {
                    Id = 2,
                    ImageUrl = "Product1_Add2.avif",
                    ProductId = 1,
                },
                new ProductImage
                {
                    Id = 3,
                    ImageUrl = "Product2_Add1.avif",
                    ProductId = 2,
                },
                new ProductImage
                {
                    Id = 4,
                    ImageUrl = "Product2_Add2.avif",
                    ProductId = 2,
                },
                new ProductImage
                {
                    Id = 5,
                    ImageUrl = "Product3_Add1.avif",
                    ProductId = 3,
                },
                new ProductImage
                {
                    Id = 6,
                    ImageUrl = "Product3_Add2.avif",
                    ProductId = 3,
                },
                new ProductImage
                {
                    Id = 7,
                    ImageUrl = "Product4_Add1.avif",
                    ProductId = 4,
                },
                new ProductImage
                {
                    Id = 8,
                    ImageUrl = "Product4_Add2.avif",
                    ProductId = 4,
                },
                new ProductImage
                {
                    Id = 9,
                    ImageUrl = "Product5_Add1.avif",
                    ProductId = 5,
                },
                new ProductImage
                {
                    Id = 10,
                    ImageUrl = "Product6_Add1.avif",
                    ProductId = 6,
                },
                new ProductImage
                {
                    Id = 11,
                    ImageUrl = "Product6_Add2.avif",
                    ProductId = 6,
                },
                new ProductImage
                {
                    Id = 12,
                    ImageUrl = "Product7_Add1.avif",
                    ProductId = 7,
                },
                new ProductImage
                {
                    Id = 13,
                    ImageUrl = "Product7_Add2.avif",
                    ProductId = 7,
                }
            );

        }
    }
}
