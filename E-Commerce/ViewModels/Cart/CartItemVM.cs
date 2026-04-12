public class CartItemVM
{
    public int CartItemId { get; set; }
    public int ProductVariantId { get; set; } 
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public string ImageUrl { get; set; }
    public int Quantity { get; set; }
    public string Size { get; set; }
    public string Color { get; set; }
    public int MaxQuantity { get; set; }
    public decimal Subtotal => Price * Quantity;
}