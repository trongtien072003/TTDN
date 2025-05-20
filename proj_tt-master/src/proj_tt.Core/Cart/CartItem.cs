using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace proj_tt.Cart
{
    public class CartItem : FullAuditedEntity<long>
    {
        public long CartId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ImageUrl { get; set; } 
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
     

        public virtual Carts Cart { get; set; }
        public string ProductImage { get; set; }

        public CartItem()
        {
            CreationTime = DateTime.Now;
        }
    }
}
