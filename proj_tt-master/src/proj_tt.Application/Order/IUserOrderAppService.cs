using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Application.Services;
using proj_tt.Order.Dto;

namespace proj_tt.Order
{
   
  public interface IUserOrderAppService : IApplicationService
    {
        Task CancelMyOrderAsync(long orderId);
        Task<PagedResultDto<OrderDto>> GetMyOrdersPagedAsync(PagedOrderRequestDto input);
        Task<OrderDto> GetMyOrderDetailAsync(long orderId);
        Task CreateOrderAsync(CreateOrderInput input);
    }
}
