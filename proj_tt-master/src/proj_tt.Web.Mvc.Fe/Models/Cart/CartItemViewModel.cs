using System;
using System.Collections.Generic;
using proj_tt.Cart.Dto;

namespace proj_tt.Web.Models.Cart
{
    public class CartItemViewModel
    {
        public long Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string ProductImage { get; set; }
    }
}
