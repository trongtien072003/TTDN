using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace proj_tt.Order.Dto
{
    [AutoMapFrom(typeof(Orders))]
    public class OrderDto : FullAuditedEntityDto<long>
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string StatusText => Status.ToString();
        public string Note { get; set; }
        public long? UserId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public List<OrderItemDto> OrderItems { get; set; }
    }
}
