using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace proj_tt.Cart.Dto
{
    public class UpdateCartItemInput : FullAuditedEntity<long>
    {
        public long CartItemId { get; set; }
        public int Quantity { get; set; }
    }
}
