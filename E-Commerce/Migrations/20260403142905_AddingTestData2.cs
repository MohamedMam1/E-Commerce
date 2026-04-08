using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_Commerce.Migrations
{
    /// <inheritdoc />
    public partial class AddingTestData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "IsAvailable", "Name", "Price" },
                values: new object[,]
                {
                    { 5, 3, "This elegant shoulder bag features a sleek and modern design, perfect for adding a touch of sophistication to any outfit. Crafted from high-quality materials, it offers durability and style. The bag includes a spacious main compartment with a secure closure, as well as additional pockets for organizing your essentials. The adjustable shoulder strap allows for comfortable wear, making it an ideal accessory for both casual and formal occasions.", "Product5_Main.avif", true, "Elegant shoulder bag with a sleek and modern design", 300m },
                    { 6, 4, "These men's textile sports sneakers are designed for both comfort and performance. Made from breathable textile material, they provide excellent ventilation to keep your feet cool during physical activities. The sneakers feature a cushioned sole that offers support and shock absorption, making them ideal for running, training, or casual wear. With a stylish design and durable construction, these sneakers are a great addition to any athletic wardrobe.", "Product6_Main.avif", true, "Men Textile Sports Sneakers", 500m },
                    { 7, 5, "This men 's Mason round shape wrist watch features a stainless steel case and a sleek silver finish. The analog display offers a classic look, while the 45 mm case size provides a bold and stylish presence on the wrist. The watch is designed for durability and precision, making it suitable for everyday wear or special occasions.", "Product7_Main.avif", true, "Men's Mason Round Shape Stainless Steel Analog Wrist Watch 45 mm - Silver - 1791788", 4000m }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "ImageUrl", "ProductId" },
                values: new object[,]
                {
                    { 9, "Product5_Add1.avif", 5 },
                    { 10, "Product6_Add1.avif", 6 },
                    { 11, "Product6_Add2.avif", 6 },
                    { 12, "Product7_Add1.avif", 7 },
                    { 13, "Product7_Add2.avif", 7 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);
        }
    }
}
