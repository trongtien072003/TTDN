using proj_tt.Order.Dto;
using System.Collections.Generic;

namespace proj_tt.Web.Models.Order
{
    public class IndexViewOrderModel
    {
        public IReadOnlyList<OrderDto> Orders { get; set; }
        public OrderDto Order { get; set; }

        public IndexViewOrderModel(IReadOnlyList<OrderDto> orders)
        {
            Orders = orders;
        }

        public IndexViewOrderModel()
        {
        }
    }
}
