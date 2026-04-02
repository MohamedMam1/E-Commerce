namespace E_Commerce.ViewModels.Product
{
    public class ProductListVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
    }
}
