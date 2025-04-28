using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace proj_tt.Cart
{
    public class Carts : Entity<long>, IHasCreationTime, IHasModificationTime
    {
        public long UserId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public virtual ICollection<CartItem> Items { get; set; }

        public Carts()
        {
            CreationTime = DateTime.Now;
            Items = new List<CartItem>();
        }
    }
}
