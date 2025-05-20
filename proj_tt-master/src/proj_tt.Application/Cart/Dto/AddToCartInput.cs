using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;

namespace proj_tt.Cart.Dto
{
    [AutoMapFrom(typeof(Carts))]
    public class AddToCartInput : FullAuditedEntity<long>
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; } 
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
    }
}
