using System.Collections.Generic;
using System.Linq;
using proj_tt.Cart.Dto;

namespace proj_tt.Web.Models.Cart
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();

        public decimal TotalAmount => Items.Sum(x => x.TotalPrice);
        public int TotalQuantity => Items.Sum(x => x.Quantity);
    }
}
