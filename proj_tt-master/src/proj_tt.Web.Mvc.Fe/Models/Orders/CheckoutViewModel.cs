using proj_tt.Cart.Dto;
using proj_tt.Order.Dto;
using proj_tt.Web.Models.Cart;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace proj_tt.Web.Models.Orders
{
    public class CheckoutViewModel
    {
        [Required] public string Name { get; set; }
        [Required] public string PhoneNumber { get; set; }
        [Required][EmailAddress] public string Email { get; set; }
        [Required] public string Address { get; set; }
        public string Note { get; set; }

        public List<CartItemDto> CartItems { get; set; } = new();
        public decimal TotalAmount => CartItems?.Sum(x => x.Quantity * x.UnitPrice) ?? 0;
    }

    public class CheckoutItemViewModel
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
