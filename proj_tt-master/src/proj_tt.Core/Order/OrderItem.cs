using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.AutoMapper;

namespace proj_tt.Order
{
    [AutoMapFrom(typeof(Orders))]
    public class OrderItem : FullAuditedEntity<long>
    {
        public long OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Price * Quantity;
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public virtual Orders Orders { get; set; }

        public OrderItem()
        {
            CreationTime = DateTime.Now;
        }
    }
}
