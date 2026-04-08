using System.Collections.Generic;
using System.Linq;

namespace E_Commerce.ViewModels.Cart
{
    public class CartVM
    {
        public IEnumerable<CartItemVM> Items { get; set; } = new List<CartItemVM>();

        public decimal TotalPrice => Items.Sum(i => i.Subtotal);
    }
}
