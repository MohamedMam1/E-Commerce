namespace E_Commerce.ViewModels.Product
{
    public class ProductFilterVM
    {
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; } 
        public string? Tag { get; set; }
    }
}
