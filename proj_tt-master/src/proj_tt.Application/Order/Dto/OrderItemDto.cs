﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace proj_tt.Order.Dto
{
    public class OrderItemDto : EntityDto<long>
    {
        public long OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Price * Quantity;
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
