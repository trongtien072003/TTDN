using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;

namespace proj_tt.Order.Dto
{
    [AutoMapFrom(typeof(Orders))]
    public class OrderDto : FullAuditedEntity<long>
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string Note { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public long? UserId { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        
    } 
}
