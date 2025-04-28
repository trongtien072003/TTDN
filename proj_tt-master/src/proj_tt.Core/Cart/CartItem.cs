using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace proj_tt.Cart
{
    public class CartItem : Entity<long>, IHasCreationTime, IHasModificationTime
    {
        public long CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public virtual Carts Cart { get; set; }

        public CartItem()
        {
            CreationTime = DateTime.Now;
        }
    }
}
