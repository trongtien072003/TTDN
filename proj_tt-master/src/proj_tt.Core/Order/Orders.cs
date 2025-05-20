using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace proj_tt.Order
{
    public class Orders : FullAuditedEntity<long>
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string Note { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public long? UserId { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public Orders()
        {
            OrderItems = new List<OrderItem>();
        }
    }

    public enum OrderStatus
    {
        Pending = 0,
        Processing = 1,
        Shipped = 2,
        Delivered = 3,
        Cancelled = 4
    }
}
