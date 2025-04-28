using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using proj_tt.Products;

namespace proj_tt.Cart.Dto
{
    [AutoMapFrom(typeof(Carts))]
    public class CartItemDto : EntityDto<long>
    {
        public long CartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Price * Quantity;
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Product products { get; set; }
        public string ImageUrl { get; set; }
    }
}
