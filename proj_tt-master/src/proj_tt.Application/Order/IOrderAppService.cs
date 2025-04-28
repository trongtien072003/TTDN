using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Application.Services;
using proj_tt.Order.Dto;
using proj_tt.Products.Dto;

namespace proj_tt.Order
{
    public interface IOrderAppService : IApplicationService
    {
        Task<OrderDto> GetOrder(long id);
        //System.Threading.Tasks.Task Create(O input);
        Task<PagedResultDto<OrderDto>> GetAllOrders(PagedOrderResultRequestDto input);
        Task UpdateOrderStatus(UpdateOrderStatusInput input);
        Task DeleteOrder(long id);
    }
}
