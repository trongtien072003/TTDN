using proj_tt.Order.Dto;
using proj_tt.Web.Models.Cart;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace proj_tt.Web.Models.Orders
{
    public class CheckoutViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }

        public List<CartItemViewModel> Items { get; set; } = new();
    }

    public class CheckoutItemViewModel
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
