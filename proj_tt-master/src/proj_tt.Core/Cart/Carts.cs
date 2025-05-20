using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace proj_tt.Cart
{
    public class Carts : FullAuditedEntity<long>
    {
        public long UserId { get; set; }
   
        public virtual ICollection<CartItem> Items { get; set; }

        public Carts()
        {
            CreationTime = DateTime.Now;
            Items = new List<CartItem>();
        }
    }
}
