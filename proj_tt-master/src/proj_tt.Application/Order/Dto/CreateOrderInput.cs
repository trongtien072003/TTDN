using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace proj_tt.Order.Dto
{
    public class CreateOrderInput : FullAuditedEntity<long>
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string PhoneNumber { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public List<CreateOrderItemInput> Items { get; set; } = new();
    }
}
