using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Application.Services;
using proj_tt.Products.Dto;

namespace proj_tt.Products
{
    public interface IUserProductAppService : IApplicationService
    {
        Task<PagedResultDto<ProductDto>> GetAllAsync(PagedProductDto input);
        Task<ProductDto> GetAsync(EntityDto<int> input);
    }
}
