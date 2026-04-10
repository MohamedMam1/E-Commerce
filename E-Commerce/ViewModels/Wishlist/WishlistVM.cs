using System.Collections.Generic;

namespace E_Commerce.ViewModels.Wishlist
{
    public class WishlistVM
    {
        public IEnumerable<WishlistItemVM> Items { get; set; } = new List<WishlistItemVM>();
    }
}
