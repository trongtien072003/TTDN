using System.Collections.Generic;
using System;
using Abp.Application.Services.Dto;
using proj_tt.Order.Dto;
using proj_tt.Order;

namespace proj_tt.Web.Models.Orders
{
    public class MyOrderListViewModel
    {
        public IReadOnlyList<OrderDto> Orders { get; set; }
        public string Keyword { get; set; }
        public OrderStatus? Status { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
    }
}
