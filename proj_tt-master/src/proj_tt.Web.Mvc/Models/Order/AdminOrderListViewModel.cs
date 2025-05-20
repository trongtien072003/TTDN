using proj_tt.Order;
using proj_tt.Order.Dto;
using System;
using System.Collections.Generic;

namespace proj_tt.Web.Models.Order
{
    public class AdminOrderListViewModel
    {
        public IReadOnlyList<OrderDto> Orders { get; set; }
        public string Keyword { get; set; }
        public OrderStatus? Status { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
    }
}
