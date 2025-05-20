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
        Task<OrderDto> GetOrderAsync(long id);
        Task<PagedResultDto<OrderDto>> GetAllAsync(PagedOrderRequestDto input);
        Task UpdateStatusAsync(UpdateOrderStatusInput input);
        Task DeleteAsync(long id);
        Task RestoreAsync(long id);
        Task<FileDto> ExportAllToFileAsync();
    }
}
