using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace proj_tt.Order.Dto
{
    public class UpdateOrderStatusInput : EntityDto<long>
    {
        public OrderStatus Status { get; set; }
        public string Note { get; set; }
    }
}
