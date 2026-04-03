using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace E_Commerce.Migrations
{
    /// <inheritdoc />
    public partial class AddingTestData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Elevate your wardrobe with our premium men's collection, featuring everything from rugged denim to sleek athletic wear.", "Men" },
                    { 2, "Discover the latest trends in women's fashion, from elegant evening wear to comfortable daily essentials.", "Women" },
                    { 3, "Functional meets fashionable. Explore our range of durable backpacks, stylish totes, and professional briefcases.", "Bag" },
                    { 4, "Step out in style with our curated selection of footwear, ranging from high-performance sneakers to classic leather boots.", "Shoes" },
                    { 5, "Timeless pieces for the modern individual. Precision-engineered watches that make a statement on any wrist.", "Watches" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "IsAvailable", "Name", "Price" },
                values: new object[,]
                {
                    { 1, 1, "This denim jacket features a washed black finish, long sleeves, and a regular fit. It is made from high-quality denim fabric, providing durability and comfort. The jacket includes classic details such as button closures, chest pockets, and side pockets. It is a versatile piece that can be styled in various ways for a trendy and casual look.", "Product1_Main.avif", true, "Washed Black Long Sleeves Regular Denim Jacket", 800m },
                    { 2, 1, "This long sleeve features a 1/4 zip design, allowing for easy ventilation and a customizable fit. It is made from high-quality sportswear fabric that offers breathability and moisture-wicking properties, keeping you comfortable during physical activities. The sleek black color adds a stylish touch to your athletic wardrobe, making it suitable for both workouts and casual wear.", "Product2_Main.avif", true, "sportswear Men's Black 1/4 zip Long sleeve", 1100m },
                    { 3, 2, "This sportswear top features long sleeves and is designed for active individuals. It is made from high-quality, moisture-wicking fabric that helps keep you dry and comfortable during workouts. The top has a sleek design with a comfortable fit, making it suitable for various sports and fitness activities. Whether you're hitting the gym or going for a run, this long sleeve sport top is a great choice for performance and style.", "Product3_Main.avif", true, "Sportswear - Sport Top Long Sleeves", 400m },
                    { 4, 2, "These wide denim jeans from VERO MODA are designed for women who want to make a fashion statement. The jeans feature a wide-leg silhouette that offers a comfortable and relaxed fit. Made from high-quality denim fabric, they provide durability and style. The jeans have a classic five-pocket design and a button and zip closure. They can be dressed up or down, making them a versatile addition to any wardrobe.", "Product4_Main.avif", true, "VERO MODA Womens Tessa Wide Denim Jeans", 900m }
                });

            migrationBuilder.InsertData(
                table: "ProductImages",
                columns: new[] { "Id", "ImageUrl", "ProductId" },
                values: new object[,]
                {
                    { 1, "Product1_Add1.avif", 1 },
                    { 2, "Product1_Add2.avif", 1 },
                    { 3, "Product2_Add1.avif", 2 },
                    { 4, "Product2_Add2.avif", 2 },
                    { 5, "Product3_Add1.avif", 3 },
                    { 6, "Product3_Add2.avif", 3 },
                    { 7, "Product4_Add1.avif", 4 },
                    { 8, "Product4_Add2.avif", 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductImages",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
